using SCWindowsSDK.com.samsung.scsp.pam.common;
using System;
using System.Threading.Tasks;

namespace SCWindowsSDK.com.samsung.scsp.pam.error
{
    public class FaultBarrier
    {
        private static readonly string TAG = "[FaultBarrier]";
        public interface ThrowableRunnable
        {
            void run();
        }

        public interface ThrowableSupplier<T>
        {
            T get();
        }

        public static Result run(Action action)
        {
            return run(action, false);
        }

        public static Result run(Action action, bool silent)
        {
            try
            {
                action.Invoke();
                return new Result();
            }
            catch (Exception e)
            {
                if (!silent)
                {
                    SDKLogger.error(TAG + $"run(): Generic Exception Handler: {e}");
                }
                return ErrorSupplier.get(e);
            }
        }

        public static Response<T> get<T>(Func<T> supplier, T defaultValue)
        {
            return get(supplier, defaultValue, false);
        }
        public static Response<T> get<T>(Func<T> supplier, T defaultValue, bool silent)
        {
            try
            {
                return new Response<T>(supplier.Invoke());
            }
            catch (Exception e)
            {
                if (!silent)
                {
                    SDKLogger.error(TAG + $"get(): Generic Exception Handler: {e}");
                }
                return new Response<T>(defaultValue, ErrorSupplier.get(e));
            }
        }

        public static Response<T> get<T>(Func<Task<T>> supplier, T defaultValue)
        {
            return get(supplier, defaultValue, false);
        }
        public static Response<T> get<T>(Func<Task<T>> supplier, T defaultValue, bool silent)
        {
            try
            {
                return new Response<T>(supplier.Invoke().Result);
            }
            catch (Exception e)
            {
                if (!silent)
                {
                    SDKLogger.error(TAG + $"get(): Generic Exception Handler: {e}");
                }
                return new Response<T>(defaultValue, ErrorSupplier.get(e));
            }
        }
    }
}
