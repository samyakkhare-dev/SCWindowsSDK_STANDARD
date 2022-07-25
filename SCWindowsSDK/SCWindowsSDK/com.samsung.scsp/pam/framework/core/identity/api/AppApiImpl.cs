using SCWindowsSDK.com.samsung.scsp.pam.common;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.ers;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.network;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.models;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.api.constant;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.api
{
    public class AppApiImpl
    {
        private static readonly string TAG = "[AppApiImpl]";
        private static readonly string IDENTITY_V_1_APP = "/identity/v1/app";
        private readonly SContextHolder scontextHolder;
        private readonly SContext scontext;

        public AppApiImpl(SContextHolder scontextHolder)
        {
            this.scontextHolder = scontextHolder;
            this.scontext = scontextHolder.scontext;
        }

        public async Task<bool> update(AppInfo appInfo)
        {
            try
            {
                string url = await ScspErs.getBaseUrlOfPath(scontext.getHttpClient(), scontext.getAccountInfoSupplier().getAppId(), IDENTITY_V_1_APP) + IDENTITY_V_1_APP;
                SDKLogger.debug(TAG + " update(): App info: " + url);
                AppUpdateRequestBody payloadObject = new AppUpdateRequestBody();
                payloadObject.version = appInfo.version;
                ErrorResponse errorResponse = new ErrorResponse();
                HttpRequestMessage request = new HttpRequestBuilder(scontextHolder, url)
                        .payload(IdentityApiContract.CONTENT_TYPE_JSON, payloadObject)
                        .build();

                HttpResponseMessage response = await scontextHolder.network.patch(request, errorResponse, new DefaultErrorListener(TAG));
                if (response == null)
                {
                    SDKLogger.info(TAG + " update():App info Update failed.");
                    if (errorResponse != null)
                        throw new ScspException((int)errorResponse.rcode, errorResponse.rmsg);
                    return false;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    SDKLogger.info(TAG + " update(): App info updated.");
                    return true;
                }
                return false;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> update(PushInfoList pushInfoList)
        {
            try
            {
                string url = await ScspErs.getBaseUrlOfPath(scontext.getHttpClient(), scontext.getAccountInfoSupplier().getAppId(), IDENTITY_V_1_APP) + IDENTITY_V_1_APP;
                SDKLogger.debug(TAG + " update(): Push info " + url);
                AppUpdateRequestBody payloadObject = new AppUpdateRequestBody();
                List<AppUpdateRequestBody.Push> push = new List<AppUpdateRequestBody.Push>();
                foreach (PushInfo pushInfo in pushInfoList.getPushInfoList())
                {
                    push.Add(pushInfo.appUpdateGetPush());
                }
                payloadObject.pushes = push.ToArray();
                ErrorResponse errorResponse = new ErrorResponse();
                HttpRequestMessage request = new HttpRequestBuilder(scontextHolder, url)
                        .payload(IdentityApiContract.CONTENT_TYPE_JSON, payloadObject)
                        .build();

                HttpResponseMessage response = await scontextHolder.network.patch(request, errorResponse, new DefaultErrorListener(TAG));

                if (response == null)
                {
                    SDKLogger.info(TAG + " update(): PushInfo Update failed.");
                    if (errorResponse != null)
                        throw new ScspException((int)errorResponse.rcode, errorResponse.rmsg);
                    return false;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    SDKLogger.info(TAG + " update(): Push info updated.");
                    return true;
                }

                return false;
            }
            catch
            {
                throw;
            }
        }
    }
}
