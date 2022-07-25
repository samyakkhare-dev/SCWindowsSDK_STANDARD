using SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.api;
using SCWindowsSDK.com.samsung.scsp.pam.common;
using SCWindowsSDK.com.samsung.scsp.pam.util;
using System.Threading.Tasks;
using System;
using SCWindowsSDK.com.samsung.scsp.pam.error;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity
{
    public class Registration
    {
        private readonly RegistrationApiImpl registrationApi;
        private readonly AccountInfoSupplier accountInfoSupplier;
        private readonly string TAG = "[Registration]";

        public Registration(RegistrationApiImpl registrationApi, AccountInfoSupplier accountInfoSupplier)
        {
            this.registrationApi = registrationApi;
            this.accountInfoSupplier = accountInfoSupplier;
        }

        /**
         * To use Samsung Cloud Service, app must register for the first time.
         * @param isAccountRequiredFeature true if the feature of SDK is a feature that requires an account, otherwise false
         * @throws ScspException
         */
        public Task register(bool isAccountRequiredFeature)
        {
            SDKLogger.info(TAG + " register():");
            verify(isAccountRequiredFeature);
            bool hasAccInfo = !String.IsNullOrEmpty(accountInfoSupplier.getAccessToken()) && !String.IsNullOrEmpty(accountInfoSupplier.getUserId());

            if (isAccountRequiredFeature && !hasAccInfo)
            {
                throw new ScspException(ScspException.Code.BAD_IMPLEMENTATION, "Not support your feature when accountInfo is not valid");
            }

            if ((ScspCorePreferences.get().isAccountRegistered.get() && hasAccInfo)
                    || (ScspCorePreferences.get().isDeviceRegistered.get() && !hasAccInfo))
            {
                SDKLogger.info(TAG + " register(): Already registered.");
                return Task.CompletedTask;
            }
            try
            {
                return Task.Run(() => handle(() => registrationApi.register().Wait()));
            }
            catch
            {
                throw;
            }
        }

        void verify(bool isAccountRequiredFeature)
        {
            try
            {
                bool hasAccInfo = !StringUtil.isEmpty(accountInfoSupplier.getAccessToken()) && !StringUtil.isEmpty(accountInfoSupplier.getUserId());
                if (isAccountRequiredFeature && !hasAccInfo)
                {
                    throw new ScspException(ScspException.Code.BAD_IMPLEMENTATION, "Not support your feature when accountInfo is not valid");
                }
            }
            catch
            {
                throw;
            }
        }

        /**
         * If app no longer wants to use Samsung cloud services (for example, when SignOut), app can deregister.
         */
        public void deregister(string token)
        {
            SDKLogger.info(TAG + " deregister()");

            try
            {
                registrationApi.deregister(token).Wait();
            }
            catch (ScspException e)
            {
                SDKLogger.error(TAG + " deregister(): Fail deregister. " + e.ToString());
                throw;
            }
        }

        private void handle(Action action)
        {
            Result result = FaultBarrier.run(action);

            if (result.rcode == ScspException.Code.UNAUTHENTICATED_FOR_SA)
            {
                ScspIdentity.onUnauthenticatedForSA();
            }

            if (!result.success)
            {
                throw new ScspException(result.rcode, result.rmsg);
            }
        }
    }
}
