using SCWindowsSDK.com.samsung.scsp.pam.common;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity;

namespace SCWindowsSDK.com.samsung.scsp.pam.util
{
    public class ScspCorePreferences : Preferences
    {

        private static ScspCorePreferences preference;
        private static readonly object Lock = new object();

        public readonly PreferenceItem<string> cloudToken;
        public readonly PreferenceItem<long> cloudTokenExpiredOn;
        public readonly PreferenceItem<string> deviceToken;
        public readonly PreferenceItem<long> deviceTokenExpiredOn;
        public readonly PreferenceItem<bool> isDeviceRegistered;
        public readonly PreferenceItem<bool> isAccountRegistered;
        public readonly PreferenceItem<string> hashedUid;
        public readonly PreferenceItem<string> pdid;
        public readonly PreferenceItem<string> cdid;
        public readonly PreferenceItem<string> pushInfos;
        public readonly PreferenceItem<string> appVersion;
        public readonly PreferenceItem<string> simMcc;
        public readonly PreferenceItem<string> simMnc;
        public readonly PreferenceItem<string> osVersion;
        public readonly PreferenceItem<string> deviceAlias;
        public ScspCorePreferences()
        {
            this.cloudToken = new PreferenceItem<string>(this, "cloud_token", "");
            this.cloudTokenExpiredOn = new PreferenceItem<long>(this, "cloud_token_expire_time", -1L);
            this.deviceToken = new PreferenceItem<string>(this, "device_cloud_token", "");
            this.deviceTokenExpiredOn = new PreferenceItem<long>(this, "device_cloud_token_expire_time", -1L);
            this.isDeviceRegistered = new PreferenceItem<bool>(this, "is_device_registered", false);
            this.isAccountRegistered = new PreferenceItem<bool>(this, "is_account_registered", false);
            this.hashedUid = new PreferenceItem<string>(this, "hashed_uid", "");
            this.pdid = new PreferenceItem<string>(this, "physical_device_id", "");
            this.cdid = new PreferenceItem<string>(this, "client_device_id", "");
            this.pushInfos = new PreferenceItem<string>(this, "push_infos", "");
            this.appVersion = new PreferenceItem<string>(this, "app_version", "");
            this.simMcc = new PreferenceItem<string>(this, "sim_mcc", "");
            this.simMnc = new PreferenceItem<string>(this, "sim_mnc", "");
            this.osVersion = new PreferenceItem<string>(this, "os_version", "");
            this.deviceAlias = new PreferenceItem<string>(this, "device_name", "");

        }
        public static ScspCorePreferences get()
        {
            if (preference == null)
            {
                lock (Lock)
                {
                    if (preference == null)
                    {
                        preference = new ScspCorePreferences();
                    }
                }
            }
            return preference;
        }

        public void clearAll()
        {
            this.cloudToken.accept("");
            this.cloudTokenExpiredOn.accept(-1L);
            this.deviceToken.accept("");
            this.deviceTokenExpiredOn.accept(-1L);
            this.isDeviceRegistered.accept(false);
            this.isAccountRegistered.accept(false);
            this.hashedUid.accept("");
            this.pdid.accept("");
            this.cdid.accept("");
            this.pushInfos.accept("");
            this.appVersion.accept("");
            this.simMcc.accept("");
            this.simMnc.accept("");
            this.osVersion.accept("");
            this.deviceAlias.accept("");

            this.cloudToken.remove();
            this.cloudTokenExpiredOn.remove();
            this.deviceToken.remove();
            this.deviceTokenExpiredOn.remove();
            this.isDeviceRegistered.remove();
            this.isAccountRegistered.remove();
            this.hashedUid.remove();
            this.pdid.remove();
            this.cdid.remove();
            this.pushInfos.remove();
            this.appVersion.remove();
            this.simMcc.remove();
            this.simMnc.remove();
            this.osVersion.remove();
            this.deviceAlias.remove();
        }
    }
}

