using SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.api;
using SCWindowsSDK.com.samsung.scsp.pam.util;
using SCWindowsSDK.com.samsung.scsp.pam.common;
using System.Threading.Tasks;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity
{
    public class DeviceInfoManager : InfoManager<DeviceInfo>
    {
        private static readonly string TAG = "[DeviceInfoManager]";

        private readonly DeviceApiImpl deviceApi;

        public DeviceInfoManager(DeviceApiImpl deviceApi) : base()
        {
            this.deviceApi = deviceApi;
        }

        public override DeviceInfo make(DeviceInfo deviceInfo)
        {
            string cachedMnc = ScspCorePreferences.get().simMcc.get();
            string cachedMcc = ScspCorePreferences.get().simMnc.get();
            string cachedOsVersion = ScspCorePreferences.get().osVersion.get();
            string cachedAlias = ScspCorePreferences.get().deviceAlias.get();

            if (StringUtil.equals(cachedMnc, deviceInfo.getSimMnc()) && StringUtil.equals(cachedMcc, deviceInfo.getSimMcc()) &&
                    StringUtil.equals(cachedOsVersion, deviceInfo.getOsVersion()) && StringUtil.equals(cachedAlias, deviceInfo.getAlias()))
                return null;
            else
                return deviceInfo;
        }


        public override void updateCache(DeviceInfo deviceInfo)
        {
            SDKLogger.info(TAG + " updateCache(): update Device info preference");

            ScspCorePreferences.get().simMcc.accept(deviceInfo.getSimMcc());
            ScspCorePreferences.get().simMnc.accept(deviceInfo.getSimMnc());
            ScspCorePreferences.get().osVersion.accept(deviceInfo.getOsVersion());
            ScspCorePreferences.get().deviceAlias.accept(deviceInfo.getAlias());
        }

        public override async Task accept(DeviceInfo deviceInfo)
        {
            DeviceInfo newDeviceInfo = make(deviceInfo);
            if (newDeviceInfo != null)
            {
                try
                {
                    bool result = await deviceApi.update(newDeviceInfo);
                    if (result)
                    {
                        updateCache(newDeviceInfo);
                        SDKLogger.debug(TAG + " updating new cache: " + newDeviceInfo.ToString());
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
