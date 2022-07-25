namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.models
{
    public class AppUpdateRequestBody
    {
        public string version { get; set; }
        public Push[] pushes { get; set; }

        public class Push
        {
            public string id { get; set; }
            public string type { get; set; }
            public string token { get; set; }
        }

    }
}
