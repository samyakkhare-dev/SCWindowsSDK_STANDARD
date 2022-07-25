using Microsoft.Extensions.Logging;
using SCWindowsSDK.com.samsung.scsp.pam.common;
using SCWindowsSDK.com.samsung.scsp.pam.error;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity;
using SCWindowsSDK.com.samsung.scsp.pam.util;
using System.Net.Http;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core
{
    public class Scsp
    {
        private static readonly object INITIALIZE_LOCK = new object();
        private static readonly string TAG = "[Scsp]";

        /**
         * Creating SContext what including information for using cloud service.
         *
         * @param context
         * @param accountInfoSupplier
         * @param pushInfoSupplier
         * @throws ScspException
         */
        public static void initialize(HttpClient httpClient, AccountInfoSupplier accountInfoSupplier, PushInfoSupplier pushInfoSupplier)
        {
            initialize(httpClient, accountInfoSupplier, pushInfoSupplier, ScspConfig.Builder.build());
        }

        /**
         * Creating SContext what including information for using cloud service.
         *
         * @param context
         * @param accountInfoSupplier
         * @param pushInfoSupplier
         * @param scspConfig
         * @throws ScspException
         */
        public static void initialize(HttpClient httpClient, AccountInfoSupplier accountInfoSupplier, PushInfoSupplier pushInfoSupplier, ScspConfig scspConfig)
        {

            SDKLogger.info(TAG + " initialize(): " + AppObject.GetInfo().appName + " initialized: " + AppObject.GetInfo().Version);

            lock (INITIALIZE_LOCK)
            {
                checkAppId(accountInfoSupplier);
                checkUserId(accountInfoSupplier.getUserId());

                SContext.initialize(httpClient, accountInfoSupplier, pushInfoSupplier, scspConfig);
            }
        }

        public static void clear()
        {
            SDKLogger.info(TAG + " clear()");
            //TODO: Implement clear resources
        }

        private static void checkAppId(AccountInfoSupplier accountInfoSupplier)
        {
            if (accountInfoSupplier == null) throw new ScspException(ScspException.Code.BAD_IMPLEMENTATION, "AccountInfoSupplier is null");
            if (StringUtil.isEmpty(accountInfoSupplier.getAppId())) throw new ScspException(ScspException.Code.BAD_IMPLEMENTATION, "The appId is null");
        }

        private static void checkUserId(string userId)
        {
            if (!StringUtil.isEmpty(userId))
            {
                string storedUid = ScspCorePreferences.get().hashedUid.get();
                string currentUid = FaultBarrier.get<string>(() => HashUtil.getStringSHA256(userId), null).obj;

                if (!StringUtil.isEmpty(storedUid) && !StringUtil.equals(storedUid, currentUid))
                {
                    SDKLogger.error(TAG + " checkUserId(): Current uid is not same with stored uid");
                    ScspCorePreferences.get().clear();
                }
            }
        }
    }
}
