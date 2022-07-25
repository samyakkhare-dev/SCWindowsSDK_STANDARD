using SCWindowsSDK.com.samsung.scsp.pam.common;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.models;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.network.impl
{
    public class NetworkImpl : Network
    {
        private readonly string TAG = "[NetworkImpl]";

        private static readonly Dictionary<HttpStatusCode, bool> RESPONSIBLE_STATUS_ARRAY = new Dictionary<HttpStatusCode, bool>();
        private static readonly Dictionary<HttpStatusCode, bool> REDIRECTED_STATUS_ARRAY = new Dictionary<HttpStatusCode, bool>();

        HttpClient httpClient;
        static NetworkImpl()
        {
            RESPONSIBLE_STATUS_ARRAY.Add(HttpStatusCode.OK, true);
            RESPONSIBLE_STATUS_ARRAY.Add(HttpStatusCode.Created, true);
            RESPONSIBLE_STATUS_ARRAY.Add(HttpStatusCode.Accepted, true);
            RESPONSIBLE_STATUS_ARRAY.Add(HttpStatusCode.NoContent, true);
            RESPONSIBLE_STATUS_ARRAY.Add(HttpStatusCode.PartialContent, true);
            RESPONSIBLE_STATUS_ARRAY.Add(HttpStatusCode.Redirect, true);
            RESPONSIBLE_STATUS_ARRAY.Add(HttpStatusCode.NotModified, true);

            REDIRECTED_STATUS_ARRAY.Add(HttpStatusCode.Moved, true);
            REDIRECTED_STATUS_ARRAY.Add(HttpStatusCode.RedirectMethod, true);
            REDIRECTED_STATUS_ARRAY.Add(HttpStatusCode.TemporaryRedirect, true);
        }

        public NetworkImpl(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<T> get<T>(HttpRequestMessage httpRequest, ErrorResponse errorResponse, DefaultErrorListener errorListener)
        {
            SDKLogger.debug(TAG + " get():Network Op: Get");
            httpRequest.Method = HttpMethod.Get;
            try
            {
                HttpResponseMessage response = await execute(httpRequest, errorResponse, errorListener);
                if (response != null)
                    return await response.Content.ReadFromJsonAsync<T>();
                else
                    return default;
            }
            catch
            {
                throw;
            }
        }

        public async Task<HttpResponseMessage> get(HttpRequestMessage httpRequest, ErrorResponse errorResponse, DefaultErrorListener errorListener)
        {
            SDKLogger.debug(TAG + " get():Network Op: Get");
            httpRequest.Method = HttpMethod.Get;
            try
            {
                return await execute(httpRequest, errorResponse, errorListener);
            }
            catch
            {
                throw;
            }
        }

        public async Task<T> post<T>(HttpRequestMessage httpRequest, ErrorResponse errorResponse, DefaultErrorListener errorListener)
        {
            SDKLogger.debug(TAG + " post():Network Op: Post");
            httpRequest.Method = HttpMethod.Post;
            try
            {
                HttpResponseMessage response = await execute(httpRequest, errorResponse, errorListener);
                if (response != null)
                    return await response.Content.ReadFromJsonAsync<T>();
                else
                    return default;
            }
            catch
            {
                throw;
            }
        }

        public async Task<HttpResponseMessage> post(HttpRequestMessage httpRequest, ErrorResponse errorResponse, DefaultErrorListener errorListener)
        {
            SDKLogger.debug(TAG + " post(): Network Op: Post");
            httpRequest.Method = HttpMethod.Post;
            try
            {
                return await execute(httpRequest, errorResponse, errorListener);
            }
            catch
            {
                throw;
            }
        }

        public async Task<T> patch<T>(HttpRequestMessage httpRequest, ErrorResponse errorResponse, DefaultErrorListener errorListener)
        {
            SDKLogger.debug(TAG + " patch(): Network Op: Patch");
            httpRequest.Method = new HttpMethod("PATCH");
            try
            {
                HttpResponseMessage response = await execute(httpRequest, errorResponse, errorListener);
                if (response != null)
                    return await response.Content.ReadFromJsonAsync<T>();
                else
                    return default;
            }
            catch
            {
                throw;
            }
        }

        public async Task<HttpResponseMessage> patch(HttpRequestMessage httpRequest, ErrorResponse errorResponse, DefaultErrorListener errorListener)
        {
            SDKLogger.debug(TAG + " patch(): Network Op: Patch");
            httpRequest.Method = new HttpMethod("PATCH");
            {

            };
            try
            {
                return await execute(httpRequest, errorResponse, errorListener);
            }
            catch
            {
                throw;
            }
        }

        private async Task<HttpResponseMessage> execute(HttpRequestMessage httpRequest, ErrorResponse errorResponse, DefaultErrorListener errorListener)
        {
            try
            {
                HttpResponseMessage httpResponseMessage;
                httpResponseMessage = await httpClient.SendAsync(httpRequest);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return httpResponseMessage;
                }
                //TODO: AutoRedirect is true by default, should it be explicitly implemented
                /* https to http redirect is not allowed
                else if (REDIRECTED_STATUS_ARRAY.ContainsKey((int)httpResponseMessage.StatusCode)){}
                */
                else
                {
                    errorResponse = await httpResponseMessage.Content.ReadFromJsonAsync<ErrorResponse>();
                    SDKLogger.info(TAG + " execute(): Network call failed. Message: " + errorResponse.rmsg + ". Error path: " + errorResponse.details.path.ToString());
                    errorListener.onError(errorResponse, httpRequest);
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }

        public void close(HttpRequestMessage httpRequestMessage)
        {
            SDKLogger.debug(TAG + " close(): Network closed.");
            try
            {
                httpRequestMessage.Dispose();
            }
            catch
            {
                throw;
            }
        }
    }
}
