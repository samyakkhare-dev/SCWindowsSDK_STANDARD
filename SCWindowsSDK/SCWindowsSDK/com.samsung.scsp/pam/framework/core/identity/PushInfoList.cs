using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SCWindowsSDK.com.samsung.scsp.pam.common;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity
{
    public class PushInfoList
    {
        private List<PushInfo> pushInfoList = new List<PushInfo>();
        public PushInfoList(string str)
        {
            JObject jobject = JObject.Parse(str);
            SDKLogger.debug("Parsed pushinfo = " + str);

            JsonSerializer serializer = new JsonSerializer();
            List<PushInfo> list = new List<PushInfo>();
            foreach (JObject obj in jobject["pushes"])
            {
                PushInfo p = serializer.Deserialize(new JTokenReader(obj), typeof(PushInfo)) as PushInfo;
                pushInfoList.Add(p);
            }
        }
        public PushInfoList(PushInfo[] pushInfo)
        {
            foreach (var pushInfoItem in pushInfo)
            {
                pushInfoList.Add(new PushInfo(pushInfoItem.getId(), pushInfoItem.getType(), pushInfoItem.getToken()));
            }
        }

        public PushInfoList(List<PushInfo> pushInfoList)
        {
            this.pushInfoList = pushInfoList;
        }

        public void add(PushInfo pushInfo)
        {
            pushInfoList.Add(pushInfo);
        }

        public List<PushInfo> getPushInfoList()
        {
            return pushInfoList;
        }
        public JObject toJsonArray()
        {
            JObject obj = new JObject();
            JArray array = new JArray();
            pushInfoList.ForEach(v => {
                JObject newJObject = new JObject();
                newJObject.Add("id", v.getId());
                newJObject.Add("type", v.getType());
                newJObject.Add("token", v.getToken());
                array.Add(newJObject);
            });
            obj["pushes"] = array;
            return obj;
        }
    }
}
