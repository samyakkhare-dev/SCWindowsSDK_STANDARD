using System;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core
{
    public class ScspException : Exception
    {
        public int rcode;
        public string rmsg;

        public static class Code
        {
            public static int RUNTIME_ENVIRONMENT = 60000000;
            public static int NOT_INITIALIZED = 60000002;

            public static int NO_DEVICE_ID = 70000000;
            public static int NOT_SUPPORT_OS_VERSION = 70000001;
            public static int NO_PERMISSION = 70000002;

            public static int BAD_IMPLEMENTATION = 80000000;
            public static int FORBIDDEN = 80300000;

            public static int REGISTRATION_REQUIRED = 40001001;
            public static int UNAUTHENTICATED_FOR_SA = 40100001;
            public static int UNAUTHENTICATED_FOR_SC = 40100002;
        }

        public ScspException(int rcode, string rmsg, Exception exception) : base(rmsg, exception)
        {
            this.rcode = rcode;
            this.rmsg = rmsg;
        }

        public ScspException(int rcode, string rmsg) : base(rmsg)
        {
            this.rcode = rcode;
            this.rmsg = rmsg;
        }

        public override string ToString()
        {
            return string.Format("[rcode]: {0}, [rmsg]: {1}", rcode, rmsg);
        }

    }
}
