namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core
{
    public class ScspConfig
    {
        private bool enableHttp2 = true;
        private bool enableQuic = false;
        private bool forceFallback = false;

        private ScspConfig()
        {
        }

        public bool isEnableHttp2()
        {
            return enableHttp2;
        }

        public bool isEnableQuic()
        {
            return enableQuic;
        }

        public bool isForceFallback()
        {
            return forceFallback;
        }

        public static class Builder
        {
            private static readonly ScspConfig scspConfig = new ScspConfig();

            public static void enableHttp2(bool enableHttp2)
            {
                scspConfig.enableHttp2 = enableHttp2;
            }

            public static void enableQuic(bool enableQuic)
            {
                scspConfig.enableQuic = enableQuic;
            }

            public static void forceFallback(bool forceFallback)
            {
                scspConfig.forceFallback = forceFallback;
            }

            public static ScspConfig build()
            {
                return scspConfig;
            }
        }
    }
}
