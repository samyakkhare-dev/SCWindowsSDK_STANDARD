using SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.models;
using SCWindowsSDK.com.samsung.scsp.pam.util;

namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity
{
    public class PushInfo
    {
        private string id;
        private string type;
        private string token;

        public PushInfo(string id, string type, string token)
        {
            this.id = id;
            this.type = type;
            this.token = token;
        }
        public string toString()
        {
            return string.Format("{0}_{1}_{2}", id, type, token);
        }

        public bool equalsValue(PushInfo target)
        {

            return StringUtil.equals(this.id, target.id) && StringUtil.equals(this.type, target.type) && StringUtil.equals(this.token, target.token);
        }

        public string getId()
        {
            return id;
        }

        public string getType()
        {
            return type;
        }

        public string getToken()
        {
            return token;
        }

        public RegistrationRequestBody.Push registrationGetPush()
        {
            RegistrationRequestBody.Push push = new RegistrationRequestBody.Push();
            push.id = getId();
            push.token = getToken();
            push.type = getType();
            return push;
        }

        public AppUpdateRequestBody.Push appUpdateGetPush()
        {
            AppUpdateRequestBody.Push push = new AppUpdateRequestBody.Push();
            push.id = getId();
            push.token = getToken();
            push.type = getType();
            return push;
        }
    }
}
