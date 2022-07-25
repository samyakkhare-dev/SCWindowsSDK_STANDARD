using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity;
using SCWindowsSDK.com.samsung.scsp.pam.util;
using System;
using System.Net.Http;
using System.Reflection;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core
{
    public class SContext
    {
        HttpClient httpClient;
        private static readonly Lazy<SContext> scontext = new Lazy<SContext>(() => new SContext());
        AccountInfoSupplier accountInfoSupplier;
        PushInfoSupplier pushInfoSupplier;
        ScspConfig scspConfig;

        private SContext()
        {
        }

        public static SContext getInstance()
        {
            return scontext.Value;
        }

        public static void initialize(HttpClient httpClient, AccountInfoSupplier accountInfoSupplier, PushInfoSupplier pushInfoSupplier, ScspConfig scspConfig)
        {
            SContext scontext = getInstance();
            scontext.httpClient = httpClient ?? new HttpClient();
            scontext.accountInfoSupplier = accountInfoSupplier;
            scontext.pushInfoSupplier = pushInfoSupplier;
            scontext.scspConfig = scspConfig;
        }
        public AccountInfoSupplier getAccountInfoSupplier()
        {
            return accountInfoSupplier;
        }

        public PushInfoSupplier getPushInfoSupplier()
        {
            return pushInfoSupplier;
        }

        public HttpClient getHttpClient()
        {
            return httpClient;
        }

        public string getCloudToken(bool isAccountRequiredFeature)
        {
            string cloudToken = ScspCorePreferences.get().cloudToken.get();
            if (!StringUtil.isEmpty(cloudToken))
            {
                return cloudToken;
            }
            else if (!isAccountRequiredFeature)
            {
                return ScspCorePreferences.get().deviceToken.get();
            }
            return "";
        }

        public string getAppVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public void clear()
        {
            accountInfoSupplier = null;
            pushInfoSupplier = null;
        }

        public void verify()
        {
            if (accountInfoSupplier == null)
                throw new ScspException(ScspException.Code.NOT_INITIALIZED, "SDK is not initialized. please check if Scsp.initialize has been called.");
        }

    }
}
