using System;
using System.Collections.Generic;
using System.Text;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.models
{
    public class PushInfoModel
    {
        public List<PushInfo> pushes { get; set; }
    }

    public class Push
    {
        public string id { get; set; }
        public string type { get; set; }
        public string token { get; set; }
    }
}
