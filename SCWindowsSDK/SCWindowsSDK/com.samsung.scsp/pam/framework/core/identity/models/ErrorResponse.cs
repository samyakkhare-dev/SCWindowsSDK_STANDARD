using System;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.models
{
    public class ErrorResponse
    {
        public int rcode { get; set; }
        public string rmsg { get; set; }
        public Details details { get; set; }
    }

    public class Details
    {
        public string serverName { get; set; }
        public string path { get; set; }
        public DateTime timestamp { get; set; }
        public string traceId { get; set; }
        public Error[] errors { get; set; }
    }

    public class Error
    {
        public string reason { get; set; }
    }
}
