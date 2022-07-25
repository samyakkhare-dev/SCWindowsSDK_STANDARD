
using System;
using SCWindowsSDK.com.samsung.scsp.pam.util;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.api
{
    public class DeviceId
    {
        public static string getPhysicalDeviceId()
        {
            string pdid = ScspCorePreferences.get().pdid.get();

            if (StringUtil.isEmpty(pdid))
            {
                try
                {
                    string key = DeviceUtil.getDeviceUniqueId();

                    if (key != null)
                    {

                        var hash = HashUtil.CreateHash(key);

                        pdid = HashUtil.ByteArrayToString(hash);

                    }
                }
                catch (Exception)
                {
                    //TODO: KeystoreExceptions.writeThrowable("SCUtils", "GetPdid", ex);
                }

                ScspCorePreferences.get().pdid.accept(pdid);
            }
            return pdid;
        }

        public static string getClientDeviceId(string appId)
        {
            string cdid = ScspCorePreferences.get().cdid.get();

            if (StringUtil.isEmpty(cdid))
            {
                try
                {
                    string key = DeviceUtil.getDeviceUniqueId();

                    if (key != null)
                    {
                        key += appId;

                        var hash = HashUtil.CreateHash(key);

                        cdid = HashUtil.ByteArrayToString(hash);
                    }
                }
                catch (Exception)
                {
                    //TODO: KeystoreExceptions.writeThrowable("SCUtils", "GetPdid", ex);
                }

                ScspCorePreferences.get().cdid.accept(cdid);
            }
            return cdid;
        }
    }
}
