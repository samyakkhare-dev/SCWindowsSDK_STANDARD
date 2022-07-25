using SCWindowsSDK.com.samsung.scsp.pam.framework.core.ers;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.api.constant;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.models;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.network;
using SCWindowsSDK.com.samsung.scsp.pam.common;
using SCWindowsSDK.com.samsung.scsp.pam.util;
using System.Threading.Tasks;
using System.Net.Http;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.api
{
    public class DeviceApiImpl
    {

        private static readonly string IDENTITY_V_1_DEVICE = "/identity/v1/device";
        private readonly string TAG = "[DeviceApiImpl]";
        private readonly SContextHolder scontextHolder;
        private readonly SContext scontext;

        public DeviceApiImpl(SContextHolder scontextHolder)
        {
            this.scontextHolder = scontextHolder;
            this.scontext = scontextHolder.scontext;
        }

        public async Task<bool> update(DeviceInfo deviceInfo)
        {
            try
            {
                string url = await ScspErs.getBaseUrlOfPath(scontext.getHttpClient(), scontext.getAccountInfoSupplier().getAppId(), IDENTITY_V_1_DEVICE) + IDENTITY_V_1_DEVICE;
                SDKLogger.debug(TAG + " update():" + url);
                DeviceUpdateRequestBody payloadObject = makePayload(deviceInfo);
                ErrorResponse errorResponse = new ErrorResponse();
                HttpRequestMessage request = new HttpRequestBuilder(scontextHolder, url)
                        .payload(IdentityApiContract.CONTENT_TYPE_JSON, payloadObject)
                        .build();

                HttpResponseMessage response = await scontextHolder.network.patch(request, errorResponse, new DefaultErrorListener(TAG));

                if (response == null)
                {
                    SDKLogger.info(TAG + " update():" + "Device info Update failed.");
                    if (errorResponse != null)
                        throw new ScspException((int)errorResponse.rcode, errorResponse.rmsg);
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    SDKLogger.info(TAG + " update():" + "Device info updated.");
                    return true;
                }
                return false;
            }
            catch
            {
                throw;
            }
        }

        private DeviceUpdateRequestBody makePayload(DeviceInfo deviceInfo)
        {
            DeviceUpdateRequestBody deviceUpdateRequest = new DeviceUpdateRequestBody();

            string simMcc, simMnc, alias, osVersion;
            
            alias = deviceInfo.getAlias();
            osVersion = deviceInfo.getOsVersion();
            simMcc = deviceInfo.getSimMcc();
            simMnc = deviceInfo.getSimMnc();
            
            if (!StringUtil.isEmpty(alias))
            {
                deviceUpdateRequest.alias = alias;
            }
            if (!StringUtil.isEmpty(osVersion))
            {
                deviceUpdateRequest.osVersion = osVersion;
            }
            if (!StringUtil.isEmpty(simMcc))
            {
                deviceUpdateRequest.simMcc = simMcc;
            }
            if (!StringUtil.isEmpty(simMnc))
            {
                deviceUpdateRequest.simMnc = simMnc;
            }
            return deviceUpdateRequest;
        }

    }
}
