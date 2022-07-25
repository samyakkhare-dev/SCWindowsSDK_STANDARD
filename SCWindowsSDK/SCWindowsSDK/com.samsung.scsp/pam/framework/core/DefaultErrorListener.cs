using SCWindowsSDK.com.samsung.scsp.pam.framework.core.network;
using SCWindowsSDK.com.samsung.scsp.pam.util;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.models;
using SCWindowsSDK.com.samsung.scsp.pam.common;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Linq;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core
{
    public class DefaultErrorListener
    {
        private static readonly string TAG = "[DefaultErrorListener]";
        private string tag;
        public DefaultErrorListener(string tag)
        {
            this.tag = tag;
        }

        public void onError(ErrorResponse errorResponse, HttpRequestMessage request)
        {
            SDKLogger.error(TAG + "onError()");

            ScspException scspException = new ScspException((int)errorResponse.rcode, errorResponse.rmsg);

            handleUnauthenticatedForSC(scspException, request);

            throw scspException;
        }

        protected void handleUnauthenticatedForSC(ScspException scspException, HttpRequestMessage request)
        {
            if (scspException.rcode == ScspException.Code.UNAUTHENTICATED_FOR_SC)
            {
                string token;
                HttpRequestHeaders headers = request.Headers;
                if (headers.Contains(HeaderSetup.Key.AUTHORIZATION))
                {
                    token = headers.TryGetValues(HeaderSetup.Key.AUTHORIZATION, out var values) ? values.FirstOrDefault() : null;
                    ScspIdentity.onUnauthenticatedForSC(token).Wait();
                }
            }
        }
    }
}
