namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.models
{
    public class DeviceUpdateRequestBody
    {
        public string osVersion { get; set; }
        public string alias { get; set; }
        public string simMcc { get; set; }
        public string simMnc { get; set; }

    }
}
