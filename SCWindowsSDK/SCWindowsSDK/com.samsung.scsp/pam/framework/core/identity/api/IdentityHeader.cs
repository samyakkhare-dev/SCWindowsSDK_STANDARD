using System;
using System.Collections.Generic;
using SCWindowsSDK.com.samsung.scsp.pam.error;
using SCWindowsSDK.com.samsung.scsp.pam.util;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.api
{
    public class IdentityHeader
    {
        public static readonly string X_SC_UID = "x-sc-uid";
        public static readonly string X_SC_ACCESS_TOKEN = "x-sc-access-token";
        public static readonly string X_SC_APP_VERSION = "x-sc-app-version";
        private static readonly string X_SC_APP_ID = "x-sc-app-id";
        private static readonly string X_SC_CDID = "x-sc-cdid";
        private static readonly string X_SC_PDID = "x-sc-pdid";
        private static readonly string USER_AGENT = "User-Agent";

        public Dictionary<string, string> get(SContextHolder scontextHolder)
        {
            Dictionary<string, string> header = new Dictionary<string, string>();
            AccountInfoSupplier accountInfoSupplier = scontextHolder.scontext.getAccountInfoSupplier();

            string cdid = FaultBarrier.get(() => DeviceId.getClientDeviceId(accountInfoSupplier.getAppId()), null, true).obj;
            string pdid = FaultBarrier.get(() => DeviceId.getPhysicalDeviceId(), null, true).obj;

            header.Add(USER_AGENT, scontextHolder.userAgent);
            header.Add(X_SC_APP_ID, accountInfoSupplier.getAppId());

            if (!StringUtil.isEmpty(pdid))
            {
                header.Add(X_SC_CDID, cdid);
            }

            if (!StringUtil.isEmpty(pdid))
            {
                header.Add(X_SC_PDID, pdid);
            }
            bool hasAccInfo = !String.IsNullOrEmpty(accountInfoSupplier.getAccessToken()) && !String.IsNullOrEmpty(accountInfoSupplier.getUserId());
            if (hasAccInfo)
            {
                header.Add(X_SC_ACCESS_TOKEN, accountInfoSupplier.getAccessToken());
                header.Add(X_SC_UID, accountInfoSupplier.getUserId());
            }
            return header;
        }
    }
}
