namespace SCWindowsSDK.com.samsung.scsp.pam.error
{
    public class Result
    {
        private readonly static int SUCCESS = 1;
        private readonly static int FAIL = 2;

        public readonly bool success;
        public readonly int result;
        public readonly int rcode;
        public readonly string rmsg;

        public Result(int rcode, string rmsg)
        {
            this.success = false;
            this.result = FAIL;
            this.rcode = rcode;
            this.rmsg = rmsg;
        }

        public Result()
        {
            this.success = true;
            this.result = SUCCESS;
            this.rcode = 20000000; // OK
            this.rmsg = "";
        }

    }
}
