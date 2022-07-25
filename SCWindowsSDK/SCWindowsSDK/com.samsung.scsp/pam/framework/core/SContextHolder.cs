using System.Globalization;
using SCWindowsSDK.com.samsung.scsp.pam.common;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.network;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.network.impl;
using SCWindowsSDK.com.samsung.scsp.pam.util;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core
{
    public class SContextHolder
    {
        public SContext scontext;
        public readonly string userAgent;
        public Network network;
        public readonly bool isAccountRequiredFeature;

        public SContextHolder(string applicationId, string version, bool isAccountRequiredFeature)
        {
            this.scontext = SContext.getInstance();
            this.userAgent = UserAgentFactory.get(scontext, applicationId, version);
            this.network = NetworkFactory.get(scontext);
            this.isAccountRequiredFeature = isAccountRequiredFeature;
        }

        private static class NetworkFactory
        {
            public static Network get(SContext scontext)
            {
                return new NetworkImpl(scontext.getHttpClient());
            }
        }

        private static class UserAgentFactory
        {
            //TODO: set user agent for windows apk 
            public static string get(SContext scontext, string applicationId, string version)
            {
                WindowsObject.Info info = WindowsObject.GetInfo();
                AppObject.Info appInfo = AppObject.GetInfo();

                object[] parameters = new object[8]
                {
                    DeviceUtil.getModelName(),
                        info.buildNumber,
                        appInfo.appName,
                        scontext.getAppVersion(),
                        info.Version,
                        info.codeName,
                        applicationId.Substring(applicationId.LastIndexOf('.') + 1),
                        version
                };
                return string.Format(CultureInfo.InvariantCulture, "({0}; {1}; {2}={3}; windows sdk={4}, sw={5}; {6}={7};)", parameters);
            }

        }
    }
}
