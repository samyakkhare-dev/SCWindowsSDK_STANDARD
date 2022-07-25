using SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.api;
using SCWindowsSDK.com.samsung.scsp.pam.common;
using SCWindowsSDK.com.samsung.scsp.pam.util;
using System.Threading.Tasks;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity
{
    public class AppInfoManager : InfoManager<AppInfo>
    {
        private static readonly string TAG = "[AppInfoManager]";
        private readonly AppApiImpl appApi;

        public AppInfoManager(AppApiImpl appApi)
        {
            this.appApi = appApi;
        }

        public override AppInfo make(AppInfo appInfo)
        {
            string appVersion = ScspCorePreferences.get().appVersion.get();
            if (StringUtil.equals(appInfo.version, appVersion))
                return null;
            else
                return appInfo;
        }

        public override void updateCache(AppInfo appInfoOfOriginalVersion)
        {
            SDKLogger.info(TAG + " updateCache(): update App info preference");
            ScspCorePreferences.get().appVersion.accept(appInfoOfOriginalVersion.version);
        }

        public async override Task accept(AppInfo appInfo)
        {
            AppInfo newAppInfo = make(appInfo);
            if (newAppInfo != null)
            {
                try
                {
                    bool result = await appApi.update(newAppInfo);
                    if (result)
                    {
                        updateCache(newAppInfo);
                    }
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
