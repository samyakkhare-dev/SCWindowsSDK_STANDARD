using SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.api;
using SCWindowsSDK.com.samsung.scsp.pam.util;
using SCWindowsSDK.com.samsung.scsp.pam.common;
using System.Threading.Tasks;
using System;
using SCWindowsSDK.com.samsung.scsp.pam.error;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity
{
    public class Token
    {

        private TokenApiImpl tokenApi;
        private readonly AccountInfoSupplier accountInfoSupplier;
        private readonly string TAG = "[Token]";

        public Token(TokenApiImpl tokenApi, AccountInfoSupplier accountInfoSupplier)
        {
            this.tokenApi = tokenApi;
            this.accountInfoSupplier = accountInfoSupplier;
        }

        /**
         * To use Samsung Cloud Service, app must issue a token after registration.
         * @param isAccountRequiredFeature true if the feature of SDK is a feature that requires an account, otherwise false
         * @return CloudToken
         * @throws ScspException
         */
        public Task<string> get(bool isAccountRequiredFeature)
        {
            SDKLogger.info("Get token");
            verify(isAccountRequiredFeature);
            /**
             * If CloudToken is valid,
             * return cloudToken
             */
            string cloudToken = ScspCorePreferences.get().cloudToken.get();
            SDKLogger.info(TAG + "cached cloudToken: " + cloudToken);
            if (!StringUtil.isEmpty(cloudToken))
            {
                long tokenExpireOn = (long)ScspCorePreferences.get().cloudTokenExpiredOn.get();
                if (tokenExpireOn > DateTimeOffset.Now.ToUnixTimeMilliseconds())
                {
                    SDKLogger.info(TAG + "get(): CloudToken is valid");
                    return Task.FromResult(cloudToken);
                }
                else
                {
                    ScspCorePreferences.get().cloudToken.accept("");
                    ScspCorePreferences.get().cloudTokenExpiredOn.accept(-1L);

                    ScspCorePreferences.get().cloudToken.remove();
                    ScspCorePreferences.get().cloudTokenExpiredOn.remove();
                    SDKLogger.info(TAG + " get(): CloudToken is expired");
                }
            }

            /**
             * If DeviceCloudToken is valid and request is not from accountRequiredFeature,
             * return deviceCloudToken
             */
            string deviceToken = ScspCorePreferences.get().deviceToken.get();
            if (!StringUtil.isEmpty(deviceToken) && !isAccountRequiredFeature)
            {
                long tokenExpireTime = (long)ScspCorePreferences.get().deviceTokenExpiredOn.get();
                if (tokenExpireTime > DateTimeOffset.Now.ToUnixTimeMilliseconds())
                {
                    SDKLogger.info(TAG + " get():DeviceToken is valid");
                    return Task.FromResult(deviceToken);
                }
                else
                {
                    ScspCorePreferences.get().deviceToken.accept("");
                    ScspCorePreferences.get().deviceTokenExpiredOn.accept(-1L);

                    ScspCorePreferences.get().deviceToken.remove();
                    ScspCorePreferences.get().deviceTokenExpiredOn.remove();
                    SDKLogger.info(TAG + " get(): DeviceToken is expired");
                }
            }
            try
            {
                return handle(tokenApi.issue());
            }
            catch
            {
                throw;
            }
        }

        /**
         * App need to reissue the token to proceed if the token has expired.
         * (40100002 will be transmitted as an error code when you use cloud apis)
         * @throws ScspException
         */
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
        public Task<string> refresh(string expiredToken)
        {

            SDKLogger.info(TAG + " refresh():");
            clearToken(expiredToken);

            try
            {
                return handle(tokenApi.issue());
            }
            catch
            {
                throw;
            }
        }
        public Task<string> updateAccount()
        {

            SDKLogger.info(TAG + " updateAccount():");
            //verify
            try
            {
                return handle(tokenApi.issue());
            }
            catch
            {
                throw;
            }
        }

        private void clearToken(string expiredToken)
        {
            if (StringUtil.equals(expiredToken, ScspCorePreferences.get().cloudToken.get()))
            {
                ScspCorePreferences.get().cloudToken.accept("");
                ScspCorePreferences.get().cloudTokenExpiredOn.accept(-1L);

                ScspCorePreferences.get().cloudToken.remove();
                ScspCorePreferences.get().cloudTokenExpiredOn.remove();
                SDKLogger.info(TAG + " refresh(): Remove cloudToken");
            }
            else if (StringUtil.equals(expiredToken, ScspCorePreferences.get().deviceToken.get()))
            {
                ScspCorePreferences.get().deviceToken.accept("");
                ScspCorePreferences.get().deviceTokenExpiredOn.accept(-1L);

                ScspCorePreferences.get().deviceToken.remove();
                ScspCorePreferences.get().deviceTokenExpiredOn.remove();
                SDKLogger.info(TAG + " refresh(): Remove deviceToken");
            }
            else
            {
                ScspCorePreferences.get().clear();
            }
        }

        private Task<string> handle(Task action)
        {
            try
            {
                Func<Task<string>> myFun = new Func<Task<string>>(() => (Task<string>)action);
                return handle(myFun, false);
            }
            catch
            {
                throw;
            }
        }

        private Task<string> handle(Func<Task<string>> action, bool ignoreSaError)
        {

            //Func<Task<string>> myFun = new Func<Task<string>>(() => tokenApi.issue());
            //Response<string> response = FaultBarrier.get<string>(() => myFun, null).obj;
            Response<string> response = FaultBarrier.get<string>(action, null);

            if (!ignoreSaError && response.rcode == ScspException.Code.UNAUTHENTICATED_FOR_SA)
            {
                ScspIdentity.onUnauthenticatedForSA();
            }

            if (response.rcode == ScspException.Code.REGISTRATION_REQUIRED)
            {
                ScspIdentity.onRegistrationRequired().Wait();
                response = FaultBarrier.get<string>(action, null);
            }
            if (response.success)
            {
                return Task.FromResult(response.obj);
            }
            throw new ScspException(response.rcode, response.rmsg);
        }
    }
}
