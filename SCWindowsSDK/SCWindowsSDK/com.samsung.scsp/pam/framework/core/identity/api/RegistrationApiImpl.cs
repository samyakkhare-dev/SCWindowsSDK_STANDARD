using SCWindowsSDK.com.samsung.scsp.pam.common;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.ers;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.api.constant;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.network.impl;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.network;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.models;
using SCWindowsSDK.com.samsung.scsp.pam.util;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.api
{
    public class RegistrationApiImpl
    {
        private static readonly string TAG = "[RegistrationApiImpl]";
        private static readonly string BASE_URI = "/identity/v1";
        private static readonly string REGISTER_URI = BASE_URI + "/register";
        private static readonly string DEREGISTER_URI = BASE_URI + "/deregister";

        private readonly SContextHolder scontextHolder;
        private readonly SContext scontext;

        private readonly IdentityHeader identityHeader = new IdentityHeader();

        public RegistrationApiImpl(SContextHolder scontextHolder)
        {
            this.scontextHolder = scontextHolder;
            this.scontext = scontextHolder.scontext;
        }

        public async Task register()
        {
            try
            {
                string url = await ScspErs.getBaseUrlOfPath(scontext.getHttpClient(), scontext.getAccountInfoSupplier().getAppId(), REGISTER_URI) + REGISTER_URI;
                SDKLogger.debug(TAG + " register(): " + url);
                RegistrationRequestBody payloadObject = makePayload();

                Dictionary<string, string> requestHeader = identityHeader.get(scontextHolder);
                HttpRequestMessage request = new HttpRequestBuilder(scontextHolder, url)
                        .clearHeader()
                        .payload(IdentityApiContract.CONTENT_TYPE_JSON, payloadObject)
                        .addHeaderMap(requestHeader)
                        .build();

                NetworkImpl network = (NetworkImpl)scontextHolder.network;
                HttpResponseMessage response = await network.post(request, new ErrorResponse(), new DefaultErrorListener(TAG));

                if (response != null && response.IsSuccessStatusCode)
                {
                    if (requestHeader.ContainsKey(IdentityHeader.X_SC_ACCESS_TOKEN) && requestHeader.ContainsKey(IdentityHeader.X_SC_UID))
                    {
                        ScspCorePreferences.get().isAccountRegistered.accept(true);
                        ScspCorePreferences.get().cloudToken.accept("");
                        ScspCorePreferences.get().cloudTokenExpiredOn.accept(-1L);
                        ScspCorePreferences.get().cloudToken.remove();
                        ScspCorePreferences.get().cloudTokenExpiredOn.remove();
                        SDKLogger.info(TAG + " register(): Successfully registered with account, remove cloudToken");
                    }
                    else
                    {
                        ScspCorePreferences.get().isDeviceRegistered.accept(true);
                        SDKLogger.info(TAG + " register(): Successfully registered without account.");
                    }

                    RegistrationRequestBody.App app = payloadObject.app;
                    ScspCorePreferences.get().appVersion.accept(app.version);
                    if (app.pushes != null)
                    {
                        List<PushInfo> pushInfos = new List<PushInfo>();
                        foreach (var push in app.pushes)
                        {
                            pushInfos.Add(new PushInfo(push.id, push.type, push.token));
                        }
                        PushInfoList pushInfoList = new PushInfoList(pushInfos);
                        var json = pushInfoList.toJsonArray().ToString();
                        SDKLogger.debug(TAG + "Register api json: " + json);
                        ScspCorePreferences.get().pushInfos.accept(json);
                    }

                    RegistrationRequestBody.Device device = payloadObject.device;
                    ScspCorePreferences.get().osVersion.accept(device.osVersion);
                    ScspCorePreferences.get().deviceAlias.accept(device.alias);
                    if (device.simMcc != null)
                    {
                        ScspCorePreferences.get().simMcc.accept(device.simMcc);
                    }
                    if (device.simMnc != null)
                    {
                        ScspCorePreferences.get().simMnc.accept(device.simMnc);
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public async Task deregister(string token)
        {
            try
            {
                string url = await ScspErs.getBaseUrlOfPath(scontext.getHttpClient(), scontext.getAccountInfoSupplier().getAppId(), DEREGISTER_URI) + DEREGISTER_URI;

                HttpRequestMessage request = new HttpRequestBuilder(scontextHolder, url)
                        .removeHeader(HeaderSetup.Key.AUTHORIZATION)
                        .addHeader(HeaderSetup.Key.AUTHORIZATION, token)
                        .build();

                NetworkImpl network = (NetworkImpl)scontextHolder.network;
                HttpResponseMessage response = await network.post(request, new ErrorResponse(), new NotHandleAuthenticateErrorListener(TAG));
                if (response != null && response.IsSuccessStatusCode)
                {
                    SDKLogger.info(TAG + " deregister():Successfully deregistered.");
                }
            }
            catch
            {
                throw;
            }

        }

        private RegistrationRequestBody makePayload()
        {
            RegistrationRequestBody registrationRequestBody = new RegistrationRequestBody();

            /**
             * App Object
             * */
            registrationRequestBody.app.version = scontext.getAppVersion();
            if (scontext.getPushInfoSupplier() != null)
            {
                PushInfo[] pushInfo = scontext.getPushInfoSupplier().getPushInfo();
                if (pushInfo != null && pushInfo.Length > 0)
                {
                    PushInfo[] pushInfos = pushInfo;
                    List<RegistrationRequestBody.Push> push = new List<RegistrationRequestBody.Push>();
                    foreach (PushInfo p in pushInfos)
                    {
                        push.Add(p.registrationGetPush());
                    }

                    registrationRequestBody.app.pushes = push.ToArray();
                }
            }

            /**
             * Device Object
             * */
            registrationRequestBody.device.osType = DeviceUtil.getOsType();
            registrationRequestBody.device.osVersion = DeviceUtil.getOsVersion();
            registrationRequestBody.device.type = DeviceUtil.getDeviceType();
            registrationRequestBody.device.countryCode = DeviceUtil.getIso3CountryCode();
            registrationRequestBody.device.modelName = DeviceUtil.getModelName();
            registrationRequestBody.device.alias = DeviceUtil.getDeviceName();
            registrationRequestBody.device.platformVersion = DeviceUtil.getPlatformVersion();

            /* if (!StringUtil.isEmpty(DeviceUtil.getSimMcc(scontext.getContext())))
             {
                 registrationRequestBody.device.simMcc = DeviceUtil.getSimMcc(scontext.getContext());
             }
             if (!StringUtil.isEmpty(DeviceUtil.getSimMnc(scontext.getContext())))
             {
                 registrationRequestBody.device.simMnc = DeviceUtil.getSimMnc(scontext.getContext());
             }
             if (!StringUtil.isEmpty(DeviceUtil.getCsc(scontext.getContext())))
             {
                 registrationRequestBody.device.csc = DeviceUtil.getCsc(scontext.getContext());
             }
             */
            SDKLogger.debug(TAG + "adding registration body payload");
            return registrationRequestBody;
        }

        private class NotHandleAuthenticateErrorListener : DefaultErrorListener
        {
            public NotHandleAuthenticateErrorListener(string tag) : base(tag)
            {
            }

            protected new void handleUnauthenticatedForSA(ScspException scspException, HttpRequestMessage request)
            {
            }


            protected new void handleUnauthenticatedForSC(ScspException scspException, HttpRequestMessage request)
            {
            }
        }
    }
}
