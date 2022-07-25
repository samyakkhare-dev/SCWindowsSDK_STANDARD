using Newtonsoft.Json;
using SCWindowsSDK.com.samsung.scsp.pam.common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.network
{
    public class HttpRequestBuilder
    {
        private static readonly string RANGE = "Range";
        private static readonly string BYTES = "bytes";
        private static readonly string CONTENT_TYPE = "Content-Type";
        private static readonly string CONTENT_RANGE = "Content-Range";
        private static readonly string CONTENT_LENGTH = "Content-Length";

        private static readonly string CONTENT_ID = "SINGLE_REQUEST";
        private static readonly string BOUNDARY = "NO_BOUNDARY";
        private static readonly string CONTENT_DISPOSITION = "NO_CONTENT_DISPOSITION";
        private static readonly string CHARSET = "UTF-8";

        private static readonly string NEW_LINE = "\r\n";
        private string url;
        private long range = 0L;
        private long length = 0L;
        private string contentType;
        private HttpContent content;
        //TODO: implement generic payloadwriter
        private bool supportChunkedStream = false;

        private Dictionary<string, string> headerMap = new Dictionary<string, string>();

        public HttpRequestBuilder(SContextHolder scontextHolder, string url)
        {
            this.url = url;
            foreach (var header in HeaderSetup.commonHeader(scontextHolder))
            {
                this.headerMap.Add(header.Key, header.Value);
               // this.headerMap.TryAdd(header.Key, header.Value);
            }
        }
        public HttpRequestBuilder(Dictionary<string, string> headers, string url)
        {
            this.url = url;
            this.headerMap = headers;
        }

        public HttpRequestBuilder payload(string contentType, object content)
        {
            this.contentType = contentType;
            var payloadJson = JsonConvert.SerializeObject(content);
            var payloadContent = new StringContent(payloadJson, Encoding.UTF8, "application/json");
            this.content = payloadContent;

            //TODO: implement payload writer for object type
            //payloadWriter = new StringPayloadWriter();
            //uploadDataProvider = new CronetStringPayloadWriter(content);
            return this;
        }

        public HttpRequestBuilder payload(string contentType, string filePath)
        {
            this.contentType = contentType;
            var multipartFormContent = new MultipartFormDataContent();
            var fileStreamContent = new StreamContent(File.OpenRead(filePath));
            multipartFormContent.Add(fileStreamContent);
            this.content = multipartFormContent;

            //TODO: implement payload writer for file type
            //payloadWriter = new FilePayloadWriter();
            return this;
        }
        public HttpRequestBuilder addHeader(string key, string value)
        {
            this.headerMap.Add(key, value);
            //this.headerMap.TryAdd(key, value);
            return this;
        }

        public HttpRequestBuilder addHeaderMap(Dictionary<string, string> headerMap)
        {
            this.headerMap = headerMap;
            return this;
        }

        public HttpRequestBuilder removeHeader(string key)
        {
            this.headerMap.Remove(key);
            return this;
        }

        public HttpRequestBuilder clearHeader()
        {
            this.headerMap.Clear();
            return this;
        }

        public HttpRequestBuilder addRange(long range)
        {
            addHeader(RANGE, BYTES + "=0-");

            if (range > 0)
            {
                range += 1;
                addHeader(RANGE, BYTES + "=" + range + "-");
            }

            this.range = range;
            return this;
        }

        public HttpRequestBuilder addRange(long start, long length, long total)
        {
            addHeader(CONTENT_RANGE, BYTES + " " + start + "-" + length + "/" + total);

            this.range = start;
            return this;
        }

        public HttpRequestBuilder addLength(long length)
        {
            addHeader(CONTENT_LENGTH, length.ToString());

            this.length = length;
            return this;
        }

        public HttpRequestBuilder supportChunkedStreaming(bool supportChunkedStreaming)
        {
            this.supportChunkedStream = supportChunkedStreaming;
            return this;
        }
        public HttpRequestMessage build()
        {
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.RequestUri = new Uri(this.url);

            if (this.content != null)
            {
                httpRequestMessage.Content = this.content;
            }

            foreach (var header in headerMap)
            {
                SDKLogger.debug("HttpRequestBuilder key: " + header.Key + " value: " + header.Value);
                if (header.Key.Equals(HeaderSetup.Key.AUTHORIZATION))
                {
                    httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", header.Value);
                }
                else if (header.Key.Equals(HeaderSetup.Key.USER_AGENT))
                {
                    httpRequestMessage.Headers.UserAgent.ParseAdd(header.Value);
                }
                else
                {
                    httpRequestMessage.Headers.Add(header.Key, header.Value);
                }
            }
            httpRequestMessage.Headers.TransferEncodingChunked = this.supportChunkedStream;

            return httpRequestMessage;
        }
    }
}
