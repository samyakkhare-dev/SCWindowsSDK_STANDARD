using SCWindowsSDK.com.samsung.scsp.pam.common;
using SCWindowsSDK.com.samsung.scsp.pam.util;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.ers;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.models;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.network.impl;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.network;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.api
{
    public class TokenApiImpl
    {
        private static readonly string TAG = "[TokenApiImpl]";
        private static readonly string TOKEN_URI = "/identity/v1/tokens";


        private readonly SContextHolder scontextHolder;
        private readonly SContext scontext;

        private readonly IdentityHeader identityHeader = new IdentityHeader();

        public TokenApiImpl(SContextHolder scontextHolder)
        {
            this.scontextHolder = scontextHolder;
            this.scontext = scontextHolder.scontext;
        }

        public async Task<string> issue()
        {
            try
            {
                string url = await ScspErs.getBaseUrlOfPath(scontext.getHttpClient(), scontext.getAccountInfoSupplier().getAppId(), TOKEN_URI) + TOKEN_URI;
                SDKLogger.debug(TAG + " issue(): " + url);
                Dictionary<string, string> requestHeader = identityHeader.get(scontextHolder);
                HttpRequestMessage request = new HttpRequestBuilder(scontextHolder, url)
                        .addHeaderMap(requestHeader)
                        .addHeader(IdentityHeader.X_SC_APP_VERSION, scontext.getAppVersion())
                        .build();

                string[] cloudToken = new string[1];
                NetworkImpl network = (NetworkImpl)scontextHolder.network;

                TokenResponse response = await network.post<TokenResponse>(request, new ErrorResponse(), new DefaultErrorListener(TAG));

                if (response != null)
                {
                    string tokenType = response.tokenType;
                    string accessToken = response.accessToken;
                    long tokenExpireTime = response.expiresAt;

                    cloudToken[0] = accessToken;

                    if (requestHeader.ContainsKey(IdentityHeader.X_SC_ACCESS_TOKEN) && requestHeader.ContainsKey(IdentityHeader.X_SC_UID))
                    {
                        ScspCorePreferences.get().cloudToken.accept(cloudToken[0]);
                        ScspCorePreferences.get().cloudTokenExpiredOn.accept(tokenExpireTime * 1000);
                        SDKLogger.info(TAG + " issue():Success to issue token with account");
                    }
                    else
                    {
                        ScspCorePreferences.get().deviceToken.accept(cloudToken[0]);
                        ScspCorePreferences.get().deviceTokenExpiredOn.accept(tokenExpireTime * 1000);
                        SDKLogger.info(TAG + " issue():Success to issue token without account");
                    }
                }
                return cloudToken[0];
            }
            catch
            {
                throw;
            }
        }
    }
}
