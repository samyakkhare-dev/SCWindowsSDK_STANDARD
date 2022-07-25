namespace SCWindowsSDK.com.samsung.scsp.pam.error
{
    public class Response<T> : Result
    {
        public readonly T obj;

        public Response(T obj) : base()
        {
            this.obj = obj;
        }

        public Response(T defaultValue, Result result) : base(result.rcode, result.rmsg)
        {
            this.obj = defaultValue;
        }
    }
}
