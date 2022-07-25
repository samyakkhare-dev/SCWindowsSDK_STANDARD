using SCWindowsSDK.com.samsung.scsp.pam.common;
using SCWindowsSDK.com.samsung.scsp.pam.error;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.api;
using SCWindowsSDK.com.samsung.scsp.pam.util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity
{
    public class IdentityImpl
    {
        private static readonly string TAG = "[IdentityImpl]";
        private readonly AccountInfoSupplier accountInfoSupplier;
        private readonly Registration registration;
        private readonly Token token;
        private readonly AppInfoManager app;
        private readonly DeviceInfoManager device;
        private readonly PushInfoManager push;

        public IdentityImpl()
        {
            this.accountInfoSupplier = SContext.getInstance().getAccountInfoSupplier();
            SContextHolder scontextHolder = new SContextHolder(AppObject.GetInfo().appName, AppObject.GetInfo().Version, false);
            registration = new Registration(new RegistrationApiImpl(scontextHolder), accountInfoSupplier);
            token = new Token(new TokenApiImpl(scontextHolder), accountInfoSupplier);
            AppApiImpl appApi = new AppApiImpl(scontextHolder);
            app = new AppInfoManager(appApi);
            push = new PushInfoManager(appApi);
            device = new DeviceInfoManager(new DeviceApiImpl(scontextHolder));
        }

        public void initialize(bool isAccountRequiredFeature)
        {
            try
            {
                checkRegister(isAccountRequiredFeature).Wait();
                checkToken(isAccountRequiredFeature).Wait();

                checkPush().Wait();
                checkApp().Wait();
                checkDevice().Wait();
            }
            catch (ScspException scspException)
            {
                throw scspException;
            }
        }

        public Task checkRegister(bool isAccountRequiredFeature)
        {
            SDKLogger.info(TAG + " CheckRegister():");
            try
            {
                return Task.Run(() => registration.register(isAccountRequiredFeature).Wait());
            }
            catch
            {
                throw;
            }
        }

        public Task checkToken(bool isAccountRequiredFeature)
        {
            SDKLogger.info(TAG + " Check Token(): ");
            try
            {
                return Task.Run(() => token.get(isAccountRequiredFeature).Wait());
            }
            catch
            {
                throw;
            }
        }

        public Task checkPush()
        {
            SDKLogger.info(TAG + " CheckPush():");
            try
            {
                return Task.Run(() =>
                {
                    FaultBarrier.run(() =>
                    {
                        PushInfoSupplier pushInfoSupplier = SContext.getInstance().getPushInfoSupplier();

                        SDKLogger.debug("pushInfo is null as requested by backend Team");

                        if (pushInfoSupplier != null)
                        {
                            PushInfo[] pushInfo = pushInfoSupplier.getPushInfo();

                            if (pushInfo != null && pushInfo.Length > 0)
                            {
                                SDKLogger.debug("id = " + pushInfo[0].getId() + " type = " + pushInfo[0].getType() + " token = " + pushInfo[0].getToken());
                                push.accept(new PushInfoList(pushInfo)).Wait();
                            }
                        }
                    });
                });
            }
            catch
            {
                throw;
            }

        }

        public Task checkApp()
        {
            SDKLogger.info(TAG + " checkApp(): ");
            try
            {
                return Task.Run(() =>
                {
                    FaultBarrier.run(() =>
                    {
                        AppInfo appInfo = new AppInfo(SContext.getInstance().getAppVersion());
                        SDKLogger.debug(TAG + " checkApp(): version: " + appInfo.version);
                        app.accept(appInfo).Wait();
                    });
                }
           );
            }
            catch
            {
                throw;
            }

        }

        public Task checkDevice()
        {
            SDKLogger.info(TAG + " CheckDevice():");
            try
            {
                return Task.Run(() =>
                {
                    FaultBarrier.run(() =>
                    {
                        DeviceInfo deviceInfo = new DeviceInfo(
                                DeviceUtil.getSimMcc(),
                                DeviceUtil.getSimMnc(),
                                WindowsObject.GetInfo().Version,
                                DeviceUtil.getDeviceName()
                        );
                        device.accept(deviceInfo).Wait();
                    });
                });
            }
            catch
            {
                throw;
            }
        }

        public Task onUnauthenticatedForSC(string expiredToken)
        {
            try
            {
                return Task.Run(() => token.refresh(expiredToken).Wait());
            }
            catch
            {
                throw;
            }
        }

        public void onUnauthenticatedForSA()
        {
            ScspCorePreferences.get().cloudToken.accept("");
            ScspCorePreferences.get().cloudTokenExpiredOn.accept(-1L);
            ScspCorePreferences.get().cloudToken.remove();
            ScspCorePreferences.get().cloudTokenExpiredOn.remove();
            SContext.getInstance().getAccountInfoSupplier().onUnauthorized();
        }

        public Task onRegistrationRequired()
        {
            ScspCorePreferences.get().clearAll();
            ScspCorePreferences.get().clear();
            try
            {
                return Task.Run(() => registration.register(false).Wait());
            }
            catch
            {
                throw;
            }
        }

        public Task refreshToken(string expiredToken)
        {
            SDKLogger.info(TAG + " refreshToken():");
            try
            {
                return Task.Run(() => token.refresh(expiredToken).Wait());
            }
            catch
            {
                throw;
            }

        }

        public Task updateAccount()
        {
            ScspCorePreferences.get().cloudToken.accept("");
            ScspCorePreferences.get().cloudToken.remove();
            ScspCorePreferences.get().cloudTokenExpiredOn.accept(-1L);
            ScspCorePreferences.get().cloudTokenExpiredOn.remove();

            return Task.Run(() => token.updateAccount().Wait());
        }
        public void signOut()
        {
            SDKLogger.info(TAG + " signOut():");
            try
            {
                if (!StringUtil.isEmpty(ScspCorePreferences.get().cloudToken.get()))
                {
                    registration.deregister(ScspCorePreferences.get().cloudToken.get());
                }
                else if (!StringUtil.isEmpty(ScspCorePreferences.get().deviceToken.get()))
                {
                    registration.deregister(ScspCorePreferences.get().deviceToken.get());
                }
            }
            catch
            {
                throw;
            }
            ScspCorePreferences.get().cloudToken.accept("");
            ScspCorePreferences.get().cloudToken.remove();
            ScspCorePreferences.get().deviceToken.accept("");
            ScspCorePreferences.get().deviceToken.remove();
            ScspCorePreferences.get().cloudTokenExpiredOn.accept(-1L);
            ScspCorePreferences.get().cloudTokenExpiredOn.remove();
            ScspCorePreferences.get().deviceTokenExpiredOn.accept(-1L);
            ScspCorePreferences.get().deviceTokenExpiredOn.remove();
            ScspCorePreferences.get().isAccountRegistered.accept(false);
            ScspCorePreferences.get().isAccountRegistered.remove();
            ScspCorePreferences.get().isDeviceRegistered.accept(false);
            ScspCorePreferences.get().isDeviceRegistered.remove();
            ScspCorePreferences.get().isAccountRegistered.accept(false);
            ScspCorePreferences.get().hashedUid.accept("");
            ScspCorePreferences.get().hashedUid.remove();
            ScspCorePreferences.get().pushInfos.accept("");
            ScspCorePreferences.get().pushInfos.remove();
        }
    }
}
