using System.Collections.Generic;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.network
{
    public class HeaderSetup
    {
        public static class Key
        {
            public static readonly string X_SC_APP_ID = "x-sc-app-id";
            public static readonly string X_SC_DEVICE_MODEL = "x-sc-device-model";
            public static readonly string X_SC_OS_VERSION = "x-sc-os-version";
            public static readonly string X_SC_OS_TYPE = "x-sc-os-type";

            public static readonly string X_SC_NETWORK_MNC = "x-sc-network-mnc";
            public static readonly string X_SC_NETWORK_MCC = "x-sc-network-mcc";
            public static readonly string X_SC_DEVICE_CSC = "x-sc-device-csc";
            public static readonly string X_SC_DEVICE_CC = "x-sc-device-cc";

            public static readonly string USER_AGENT = "User-Agent";
            public static readonly string AUTHORIZATION = "Authorization";
        }

        public static Dictionary<string, string> commonHeader(SContextHolder scontextHolder)
        {
            Dictionary<string, string> header = new Dictionary<string, string>();

            header.Add(Key.USER_AGENT, scontextHolder.userAgent);
            header.Add(Key.AUTHORIZATION, scontextHolder.scontext.getCloudToken(scontextHolder.isAccountRequiredFeature));

            return header;
        }
    }
}
