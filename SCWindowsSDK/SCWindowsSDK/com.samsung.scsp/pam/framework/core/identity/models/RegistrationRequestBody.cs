namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.models
{
    public class RegistrationRequestBody
    {

        public RegistrationRequestBody()
        {
            app = new App();
            device = new Device();
        }
        public App app { get; set; }
        public Device device { get; set; }


        public class App
        {
            public string version { get; set; }
            public Push[] pushes { get; set; }
        }

        public class Push
        {
            public string id { get; set; }
            public string type { get; set; }
            public string token { get; set; }
        }

        public class Device
        {
            public string osType { get; set; }
            public string osVersion { get; set; }
            public string platformVersion { get; set; }
            public string type { get; set; }
            public string countryCode { get; set; }
            public string modelName { get; set; }
            public string alias { get; set; }
            public string simMcc { get; set; }
            public string simMnc { get; set; }
            public string csc { get; set; }
        }
    }
}
