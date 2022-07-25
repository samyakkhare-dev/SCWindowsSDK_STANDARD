using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core;

namespace SCWindowsSDK.com.samsung.scsp.pam.common
{
    public class SDKLogger
    {
        public static ILogger Logger { get; set; } = NullLogger.Instance;
        private static string TAG = "[SCWSDK]";
        public SDKLogger(ILogger logger)
        {
            Logger = logger;
        }
        public static void debug(string message)
        {
            Logger.LogDebug(TAG + message);
            Console.WriteLine(message);
        }


        public static void info(string message)
        {
            Logger.LogInformation(TAG + message);
            Console.WriteLine(message);
        }

        public static void warn(string message)
        {
            Logger.LogWarning(TAG + message);
            Console.WriteLine(message);
        }

        public static void fatal(string message)
        {
            Logger.LogCritical(TAG + message);
            Console.WriteLine(message);
        }

        public static void error(string message)
        {
            Logger.LogError(TAG + message);
            Console.WriteLine(message);
        }
    }
}
