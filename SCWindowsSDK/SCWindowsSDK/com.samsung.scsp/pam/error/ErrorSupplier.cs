using SCWindowsSDK.com.samsung.scsp.pam.framework.core;
using System;
using System.Collections.Generic;

namespace SCWindowsSDK.com.samsung.scsp.pam.error
{
    public class ErrorSupplier
    {
        private static readonly Dictionary<Type, Func<Exception, Result>> ERROR_SUPPLIER_MAP = new Dictionary<Type, Func<Exception, Result>>();

        static ErrorSupplier()
        {
            add(typeof(ScspException), (e) => new Result(((ScspException)e).rcode, ((ScspException)e).rmsg));
        }

        public static void add(Type clz, Func<Exception, Result> function)
        {
            ERROR_SUPPLIER_MAP.Add(clz, function);
        }

        public static Result get(Exception throwable)
        {
            ResultHolder resultHolder = new ResultHolder(throwable);
            foreach (var kvp in ERROR_SUPPLIER_MAP)
            {

                if (throwable.GetType() == kvp.Key)
                {
                    Result result = resultHolder.getResult();
                    result = kvp.Value.Invoke(throwable);
                    resultHolder.setResult(result);
                }
            }
            return resultHolder.getResult();
        }

        public class ResultHolder
        {
            private Result result;

            public void setResult(Result result)
            {
                this.result = result;
            }

            public Result getResult()
            {
                return this.result;
            }

            public ResultHolder(Exception e)
            {
                this.result = new Result(0, "Not defined error. There is exception {" + e + "}");
            }
        }
    }
}

