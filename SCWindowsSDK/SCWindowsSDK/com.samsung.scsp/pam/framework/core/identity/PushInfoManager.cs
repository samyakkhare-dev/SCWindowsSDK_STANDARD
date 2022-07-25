using SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.api;
using SCWindowsSDK.com.samsung.scsp.pam.util;
using SCWindowsSDK.com.samsung.scsp.pam.common;
using Newtonsoft.Json;
using SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity
{
    public class PushInfoManager : InfoManager<PushInfoList>
    {
        private readonly AppApiImpl appApi;
        private readonly string TAG = "[PushInfoManager]";

        public PushInfoManager(AppApiImpl appApi)
        {
            this.appApi = appApi;
        }

        public override PushInfoList make(PushInfoList newPushInfoList)
        {
            string cachedPushInfos = ScspCorePreferences.get().pushInfos.get();
            SDKLogger.debug("cachedPushInfos isEmpty = " + StringUtil.isEmpty(cachedPushInfos) + " cachedPushInfos = " + cachedPushInfos);
            if ((cachedPushInfos).Equals("{}") || StringUtil.isEmpty(cachedPushInfos))
            {
                return newPushInfoList;
            }

            PushInfoList oldPushInfoList = new PushInfoList(cachedPushInfos);

            List<PushInfo> newPushInfo = newPushInfoList.getPushInfoList();
            List<PushInfo> oldPushInfo = oldPushInfoList.getPushInfoList();
            List<PushInfo> tempPushInfo = new List<PushInfo>(oldPushInfo);

            foreach (PushInfo newOne in newPushInfo)
            {
                bool isPresent = oldPushInfo.Exists(v => v.getType().Equals(newOne.getType()));
                if (!isPresent)
                {
                    tempPushInfo.Add(newOne);
                }
                else
                {
                    PushInfo oldOne = (PushInfo)newOne;
                    if (!oldOne.equalsValue(newOne))
                    {
                        foreach (PushInfo temp in tempPushInfo)
                        {
                            if (tempPushInfo.Exists(v => v.getType().Equals(oldOne.getType())))
                                tempPushInfo.Remove(temp);
                            tempPushInfo.Add(newOne);
                        }

                    }
                }
            }

            if (tempPushInfo.Equals(oldPushInfo))
                return null;

            return new PushInfoList(tempPushInfo);
        }


        public override void updateCache(PushInfoList pushInfoList)
        {
            var json = pushInfoList.toJsonArray().ToString();
            SDKLogger.debug("updateCache " + json);
            ScspCorePreferences.get().pushInfos.accept(json);
        }

        public override async Task accept(PushInfoList pushInfoList)
        {
            PushInfoList newPushInfoList = make(pushInfoList);
            if (newPushInfoList != null)
            {
                try
                {
                    bool result = await appApi.update(newPushInfoList);
                    if (result)
                    {
                        updateCache(newPushInfoList);
                    }
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
