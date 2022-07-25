using System.Reflection;

namespace SCWindowsSDK.com.samsung.scsp.pam.common
{
    public class AppObject
    {

        public class Info
        {
            public string Version = "";
            public string appName = "";


            public override string ToString()
            {
                return "[App Object Info]\n" +
                    "Name: " + appName +
                    "Version : " + Version + "\n";
            }
        }

        public static Info GetInfo()
        {

            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            string name = Assembly.GetExecutingAssembly().GetName().Name.ToString();

            return new Info()
            {
                Version = version,
                appName = name
            };
        }


    }

}
