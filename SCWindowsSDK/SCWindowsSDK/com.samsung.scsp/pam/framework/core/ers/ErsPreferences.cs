using System.Collections.Generic;
using SCWindowsSDK.com.samsung.scsp.pam.common;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.ers
{
    public class ErsPreferences : Preferences
    {
        private static ErsPreferences preference;
        private static readonly object Lock = new object();

        private static readonly HashSet<string> PLAY_FEATURE_SET = new HashSet<string>();

        public readonly PreferenceItem<HashSet<string>> playFeatures;
        public readonly PreferenceItem<string> defaultUrl;
        public readonly PreferenceItem<string> playUrl;
        public readonly PreferenceItem<long> expiredTime;
        private ErsPreferences()
        {
            PLAY_FEATURE_SET.Add("configuration/v1");
            PLAY_FEATURE_SET.Add("help/v1");
            PLAY_FEATURE_SET.Add("pki/v1");
            PLAY_FEATURE_SET.Add("identity/v1");
            PLAY_FEATURE_SET.Add("certificate/v2");
            PLAY_FEATURE_SET.Add("tncpp/v1");
            PLAY_FEATURE_SET.Add("ctb/v1");
            PLAY_FEATURE_SET.Add("platform-config/v1");
            this.playFeatures = new PreferenceItem<HashSet<string>>(this, "play_feature", new HashSet<string>());
            this.defaultUrl = new PreferenceItem<string>(this, "defaultUrl", "https://api.samsungcloud.com");
            this.playUrl = new PreferenceItem<string>(this, "playUrl", "https://play.samsungcloud.com");
            this.expiredTime = new PreferenceItem<long>(this, "expiredTime", 7200000L);
            // TODO: need to handle with server payload.
            playFeatures.accept(PLAY_FEATURE_SET);
        }

        public static ErsPreferences get()
        {
            if (preference == null)
            {
                lock (Lock)
                {
                    if (preference == null)
                    {
                        preference = new ErsPreferences();
                    }
                }
            }
            return preference;
        }
    }
}

