using System;
using System.Threading.Tasks;
using SCWindowsSDK.com.samsung.scsp.pam.common;
using SCWindowsSDK.com.samsung.scsp.pam.error;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity
{
    public class ScspIdentity
    {
        private static readonly IdentityImpl identityImpl = new IdentityImpl();
        private static readonly string TAG = "[ScspIdentity]";

        public async static Task initialize(bool isAccountRequiredFeature)
        {
            SDKLogger.info(TAG + " initialize():");
            SContext.getInstance().verify();
            AccountInfoSupplier accountInfoSupplier = SContext.getInstance().getAccountInfoSupplier();

            if (isAccountRequiredFeature && (String.IsNullOrEmpty(accountInfoSupplier.getAccessToken()) || String.IsNullOrEmpty(accountInfoSupplier.getUserId())))
            {
                throw new ScspException(ScspException.Code.BAD_IMPLEMENTATION, "No Samsung Account information(access token, uid).");
            }

            try
            {
                await Task.Run(() => identityImpl.initialize(isAccountRequiredFeature));
            }
            catch (ScspException scspException)
            {
                throw scspException;
            }
        }
        public static string getCloudToken(bool isAccountRequiredFeature)
        {
            string token = SContext.getInstance().getCloudToken(isAccountRequiredFeature);
            SDKLogger.info(TAG + " getCloudToken(): " + token);
            return token;
        }

        public static Task updateAccount()
        {
            return Task.Run(() => FaultBarrier.run(() => identityImpl.updateAccount()));
        }

        public static Task updatePush()
        {
            return Task.Run(() => FaultBarrier.run(() => identityImpl.checkPush()));
        }

        public static Task signOut()
        {
            return Task.Run(() => identityImpl.signOut());
        }

        public static Task refreshToken(string expiredToken)
        {
            return Task.Run(() => identityImpl.refreshToken(expiredToken));
        }
        public static Task onUnauthenticatedForSC(string expiredToken)
        {
            try
            {
                return Task.Run(() => identityImpl.onUnauthenticatedForSC(expiredToken));

            }
            catch
            {
                throw;
            }
        }
        public static Task onRegistrationRequired()
        {
            try
            {
                return Task.Run(() => identityImpl.onRegistrationRequired());
            }
            catch
            {
                throw;
            }
        }
        public static void onUnauthenticatedForSA()
        {
            identityImpl.onUnauthenticatedForSA();
        }

    }
}
