using SCWindowsSDK.com.samsung.scsp.pam.framework.core.network;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.network.impl;
using SCWindowsSDK.com.samsung.scsp.pam.common;
using SCWindowsSDK.com.samsung.scsp.pam.error;
using SCWindowsSDK.com.samsung.scsp.pam.util;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.models;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.ers
{
    public class ErsImpl
    {
        private static readonly string TAG = "[ErsImpl]";

        private readonly NetworkImpl network;

        private static string url = null;

        /**
         * PRD  : All kinds of services should use PRD server.
         * STG  : While developing new features or services we should user STG server.
         */
        private static readonly string CONFIG_PKG_HASH = "03:38:BA:A6:9A:4E:57:0C:58:A6:08:2A:5E:21:B2:A7:B7:FD:16:F4";
        private static readonly string CONFIG_PKG_NAME = "com.samsung.android.scloud.config";
        private static readonly string CONFIG_SERVER_META_NAME = "com.samsung.android.scloud.config.server";

        private static readonly string PRD = "prd";
        private static readonly string STG = "stg";

        private static readonly Dictionary<string, string> ERS_MAIN_SERVER_MAP = new Dictionary<string, string>();
        private static readonly Dictionary<string, string> ERS_BACKUP_SERVER_MAP = new Dictionary<string, string>();

        public ErsImpl(HttpClient httpClient)
        {
            //TODO: Remove the if conditions
            if (!ERS_MAIN_SERVER_MAP.ContainsKey(PRD))
                ERS_MAIN_SERVER_MAP.Add(PRD, "https://ers.samsungcloud.com/ers/v1/endpoints");
            if (!ERS_MAIN_SERVER_MAP.ContainsKey(STG))
                ERS_MAIN_SERVER_MAP.Add(STG, "https://stg-ers.samsungcloud.com/ers/v1/endpoints");
            if (!ERS_BACKUP_SERVER_MAP.ContainsKey(PRD))
                ERS_BACKUP_SERVER_MAP.Add(PRD, "https://ers.samsungcloudplatform.com/ers/v1/endpoints");
            if (!ERS_BACKUP_SERVER_MAP.ContainsKey(STG))
                ERS_BACKUP_SERVER_MAP.Add(STG, "https://stg-ers.samsungcloud.com/ers/v1/endpoints");
            network = new NetworkImpl(httpClient);
        }

        public async Task getDomain(string appId)
        {
            ErsPreferences preferences = ErsPreferences.get();
            if (preferences.expiredTime.get() < DateTimeOffset.Now.ToUnixTimeMilliseconds())
            {
                try
                {
                    bool result = await execute(appId, getPrimaryUrl());
                    if (!result)
                    {
                        SDKLogger.error(TAG + " getDomain(): Cannot get ers url from server, So use backup server url");

                        result = await execute(appId, getSecondaryUrl());

                    }
                    if (!result)
                    {
                        SDKLogger.error(TAG + " getDomain(): Cannot get ers url from backup server, So use default preferences");
                    }
                }
                catch
                {
                    throw;
                }
            }
            DomainVo domainVo = new DomainVo();
            domainVo.defaultUrl = preferences.defaultUrl.get();
            domainVo.playUrl = preferences.playUrl.get();
            domainVo.expiredTime = preferences.expiredTime.get();
            //return domainVo;
        }

        private string getPrimaryUrl()
        {
            if (url == null)
            {
                string serverConfig = getServerConfig();
                if (!StringUtil.isEmpty(serverConfig))
                {
                    ERS_MAIN_SERVER_MAP.TryGetValue(serverConfig, out url);
                }
                else
                {
                    ERS_MAIN_SERVER_MAP.TryGetValue(STG, out url);
                }
            }
            return url;
        }

        private string getSecondaryUrl()
        {
            if (url == null)
            {
                string serverConfig = getServerConfig();
                if (!StringUtil.isEmpty(serverConfig))
                {
                    ERS_BACKUP_SERVER_MAP.TryGetValue(serverConfig, out url);
                }
                else
                {
                    ERS_BACKUP_SERVER_MAP.TryGetValue(STG, out url);
                }
            }
            return url;
        }

        private async Task<bool> execute(string appId, string url)
        {
            ErrorResponse error = new ErrorResponse();

            SDKLogger.debug(TAG + " execute(): " + url);
            HttpRequestMessage request = new HttpRequestBuilder(getHeaders(appId), url).build();
            try
            {
                HttpResponseMessage response = await network.get(request, error, new DefaultErrorListener(TAG));
                if (response != null)
                {

                    ErsResponse ersResponse = await response.Content.ReadFromJsonAsync<ErsResponse>();

                    if (!StringUtil.isEmpty(ersResponse.@default) && !StringUtil.isEmpty(ersResponse.play))
                    {
                        ErsPreferences preferences = ErsPreferences.get();
                        long expiredTime = getMaxAge(response.Headers);
                        preferences.defaultUrl.accept(ersResponse.@default);
                        preferences.playUrl.accept(ersResponse.play);
                        expiredTime += DateTimeOffset.Now.ToUnixTimeMilliseconds();
                        preferences.expiredTime.accept(expiredTime);
                        return true;
                    }
                }
            }
            catch
            {
                SDKLogger.error(TAG + " execute(): Ers Network call failed.");
                throw;
            }

            return false;
        }

        private Dictionary<string, string> getHeaders(string appId)
        {
            Dictionary<string, string> header = new Dictionary<string, string>();

            header.Add(HeaderSetup.Key.X_SC_APP_ID, appId);
            header.Add(HeaderSetup.Key.X_SC_DEVICE_MODEL, DeviceUtil.getModelName());
            header.Add(HeaderSetup.Key.X_SC_OS_TYPE, DeviceUtil.getOsType());
            header.Add(HeaderSetup.Key.X_SC_OS_VERSION, DeviceUtil.getOsVersion());

            //header.Add(HeaderSetup.Key.X_SC_NETWORK_MNC, DeviceUtil.getNetworkMnc(context));
            //header.Add(HeaderSetup.Key.X_SC_NETWORK_MCC, DeviceUtil.getNetworkMcc(context));
            //header.Add(HeaderSetup.Key.X_SC_DEVICE_CSC, DeviceUtil.getCsc(context));
            header.Add(HeaderSetup.Key.X_SC_DEVICE_CC, DeviceUtil.getIso3CountryCode());

            return header;
        }

        private long getMaxAge(HttpHeaders headers)
        {
            string maxAge = headers.TryGetValues("Cache-Control", out var values) ? values.FirstOrDefault() : null;
            long result = FaultBarrier.get(() => long.Parse(maxAge.Split('=')[1]), 86400L, true).obj;
            return result * 1000L;
        }

        private string getServerConfig()
        {
            //try
            //{
            //    PackageManager packageManager = context.getPackageManager();
            //    PackageInfo packageInfo = packageManager.getPackageInfo(CONFIG_PKG_NAME, PackageManager.GET_SIGNATURES);
            //    Signature[] signatures = packageInfo.signatures;
            //    byte[] cert = signatures[0].toByteArray();
            //    InputStream input = new ByteArrayInputStream(cert);
            //    CertificateFactory cf = CertificateFactory.getInstance("X509");
            //    X509Certificate c = (X509Certificate)cf.generateCertificate(input);
            //    MessageDigest md = MessageDigest.getInstance("SHA1");
            //    byte[] publicKey = md.digest(c.getEncoded());
            //    string hexString = byte2HexFormatted(publicKey);
            //    if (CONFIG_PKG_HASH.equals(hexString))
            //    {
            //        ApplicationInfo applicationInfo = packageManager.getApplicationInfo(CONFIG_PKG_NAME, PackageManager.GET_META_DATA);
            //        Bundle bundle = applicationInfo.metaData;
            //        return bundle.getString(CONFIG_SERVER_META_NAME);
            //    }
            //}
            //catch (Exception ignored) { }
            return null;
        }

        //private string byte2HexFormatted(byte[] arr)
        //{
        //    StringBuilder str = new StringBuilder(arr.length * 2);
        //    for (int i = 0; i < arr.length; i++)
        //    {
        //        string h = Integer.toHexString(arr[i]);
        //        int l = h.length();
        //        if (l == 1)
        //            h = "0" + h;
        //        if (l > 2)
        //            h = h.substring(l - 2, l);
        //        str.append(h.toUpperCase(Locale.ENGLISH));
        //        if (i < (arr.length - 1))
        //            str.append(':');
        //    }
        //    return str.toString();
        //}
    }
}
