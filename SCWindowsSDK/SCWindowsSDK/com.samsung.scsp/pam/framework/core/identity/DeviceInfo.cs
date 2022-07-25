namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity
{
    public class DeviceInfo
    {
        private readonly string simMcc;
        private readonly string simMnc;
        private readonly string osVersion;
        private readonly string alias;

        public DeviceInfo(string simMcc, string simMnc, string osVersion, string alias)
        {
            this.simMcc = simMcc;
            this.simMnc = simMnc;
            this.osVersion = osVersion;
            this.alias = alias;
        }
        public string getSimMcc()
        {
            return simMcc;
        }

        public string getSimMnc()
        {
            return simMnc;
        }

        public string getOsVersion()
        {
            return osVersion;
        }

        public string getAlias()
        {
            return alias;
        }
        public override string ToString()
        {
            return "DeviceInfo: [mcc:" + simMcc + " mnc:" + simMnc + " osVersion: " + osVersion + " alias: " + alias + "]";
        }
    }
}
