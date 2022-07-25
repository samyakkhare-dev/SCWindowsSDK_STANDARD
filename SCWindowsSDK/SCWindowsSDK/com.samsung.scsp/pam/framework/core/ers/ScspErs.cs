using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using SCWindowsSDK.com.samsung.scsp.pam.common;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.ers
{
    public class ScspErs
    {
        private static readonly string TAG = "[ScspErs]";
        private static ErsImpl ers = null;

        public async static Task<string> getBaseUrlOfPath(HttpClient httpClient, string appId, string uri)
        {
            ErsPreferences preferences = ErsPreferences.get();
            if (ers == null)
            {
                ers = new ErsImpl(httpClient);
            }
            if (preferences.expiredTime.get() < DateTimeOffset.Now.ToUnixTimeMilliseconds())
            {
                SDKLogger.info(TAG + " getBaseUrlOfPath():" + "Ers expired, getting ers server");
                try
                {
                    ers.getDomain(appId).Wait();
                }
                catch
                {
                    throw;
                }
            }
            HashSet<string> featureSet = ErsPreferences.get().playFeatures.get();
            foreach (string feature in featureSet)
            {
                if (uri.Contains(feature))
                {
                    SDKLogger.debug(TAG + " getBaseUrlOfPath():" + preferences.playUrl.get());
                    return preferences.playUrl.get();
                }
            }
            SDKLogger.debug(TAG + " getBaseUrlOfPath():" + preferences.defaultUrl.get());


            return preferences.defaultUrl.get();
        }

        public static void clear()
        {
            ErsPreferences.get().clear();
        }
    }
}
