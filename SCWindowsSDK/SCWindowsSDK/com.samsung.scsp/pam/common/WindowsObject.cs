using System;
using Microsoft.Win32;

namespace SCWindowsSDK.com.samsung.scsp.pam.common
{
    public class WindowsObject
    {
        public class Info
        {
            public string Name = "";
            public string fullName = "";
            public string UserAgent = "";
            public string Type = "";
            public string codeName = "";
            public string Version = "";
            public string buildNumber = "";


            public override string ToString()
            {
                return "[Windows Object Info]\n" +
                    "Name : " + Name + "\n" +
                    "fullName : " + fullName + "\n" +
                    "UserAgent : " + UserAgent + "\n" +
                    "Type :" + Type + "\n" +
                    "codeName : " + codeName + "\n" +
                    "Version : " + Version + "\n" +
                    "buildNumber :" + buildNumber + "\n"
                    ;
            }

        }

        public static Info GetInfo()
        {
            string HKLMWinNTCurrent = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion";
            string osBuild = Registry.GetValue(HKLMWinNTCurrent, "CurrentBuildNumber", "").ToString();
            string fullName = Registry.GetValue(HKLMWinNTCurrent, "productName", "").ToString();
            string Type = Environment.Is64BitOperatingSystem ? "x64" : "x84";


            string win11 = "Windows 11 version 21H2";
            string win10 = "Windows 10 version ";
            string win81 = "Windows 8.1";
            string win8 = "Windows 8";
            string win7 = "Windows 7";
            string winVista = "Windows Vista";
            string winXP = "Windows XP";
            string winMe = "Windows Me";
            string win98 = "Windows 98";
            string win98_2 = "Windows 98 Second Edition";
            string win2000 = "Windows 2000";

            string v11 = "Windows NT 11";
            string v10 = "Windows NT 10";
            string v81 = "Windows NT 6.3";
            string v8 = "Windows NT 6.2";
            string v7 = "Windows NT 6.1";
            string vVista = "Windows NT 6.0";
            string vXP1 = "Windows NT 5.2";
            string vXP = "Windows NT 5.1";
            string vMe = "Windows NT 4.9";
            string v98 = "Windows NT 4.1";
            string v2000 = "Windows NT 5.0";


            switch (osBuild)
            {
                case "22000":
                    return new Info()
                    {
                        Name = win11,
                        Version = "21H2",
                        UserAgent = v11,
                        buildNumber = "22000",
                        codeName = "Sun Valley",
                        fullName = fullName,
                        Type = Type
                    };
                case "19044":
                    return new Info()
                    {
                        Name = win10 + "21H2",
                        Version = "21H2",
                        UserAgent = v10,
                        buildNumber = "19044",
                        codeName = "Vibranium",
                        fullName = fullName,
                        Type = Type
                    };
                case "19043":
                    return new Info()
                    {
                        Name = win10 + "21H1",
                        Version = "21H1",
                        UserAgent = v10,
                        buildNumber = "19043",
                        codeName = "Vibranium",
                        fullName = fullName,
                        Type = Type
                    };
                case "19042":
                    return new Info()
                    {
                        Name = win10 + "20H2",
                        Version = "20H2",
                        UserAgent = v10,
                        buildNumber = "19042",
                        codeName = "Vibranium",
                        fullName = fullName,
                        Type = Type
                    };
                case "19041":
                    return new Info()
                    {
                        Name = win10 + "2004",
                        Version = "2004",
                        UserAgent = v10,
                        buildNumber = "19041",
                        codeName = "Vibranium",
                        fullName = fullName,
                        Type = Type
                    };
                case "18363":
                    return new Info()
                    {
                        Name = win10 + "1909",
                        Version = "1909",
                        UserAgent = v10,
                        buildNumber = "18363",
                        codeName = "Vibranium",
                        fullName = fullName,
                        Type = Type
                    };
                case "18362":
                    return new Info()
                    {
                        Name = win10 + "1903",
                        Version = "1903",
                        UserAgent = v10,
                        buildNumber = "18362",
                        codeName = "19H1",
                        fullName = fullName,
                        Type = Type
                    };
                case "17763":
                    return new Info()
                    {
                        Name = win10 + "1809",
                        Version = "1809",
                        UserAgent = v10,
                        buildNumber = "17763",
                        codeName = "Redstone 5",
                        fullName = fullName,
                        Type = Type
                    };
                case "17134":
                    return new Info()
                    {
                        Name = win10 + "1803",
                        Version = "1803",
                        UserAgent = v10,
                        buildNumber = "17134",
                        codeName = "Redstone 4",
                        fullName = fullName,
                        Type = Type
                    };
                case "16299":
                    return new Info()
                    {
                        Name = win10 + "1709",
                        Version = "1709",
                        UserAgent = v10,
                        buildNumber = "16299",
                        codeName = "Redstone 3",
                        fullName = fullName,
                        Type = Type
                    };
                case "15063":
                    return new Info()
                    {
                        Name = win10 + "1703",
                        Version = "1703",
                        UserAgent = v10,
                        buildNumber = "15063",
                        codeName = "Redstone 2",
                        fullName = fullName,
                        Type = Type
                    };
                case "14393":
                    return new Info()
                    {
                        Name = win10 + "1607",
                        Version = "1607",
                        UserAgent = v10,
                        buildNumber = "14393",
                        codeName = "Redstone 1",
                        fullName = fullName,
                        Type = Type
                    };
                case "10586":
                    return new Info()
                    {
                        Name = win10 + "1511",
                        Version = "1511",
                        UserAgent = v10,
                        buildNumber = "10586",
                        codeName = "Threshold 2",
                        fullName = fullName,
                        Type = Type
                    };
                case "10240":
                    return new Info()
                    {
                        Name = win10 + "1507",
                        Version = "NT 10.0",
                        UserAgent = v10,
                        buildNumber = "10240",
                        codeName = "Threshold 1",
                        fullName = fullName,
                        Type = Type
                    };
                case "9600":
                    return new Info()
                    {
                        Name = win81,
                        Version = "NT 6.3",
                        UserAgent = v81,
                        buildNumber = "9600",
                        codeName = "Blue",
                        fullName = fullName,
                        Type = Type
                    };
                case "9200":
                    return new Info()
                    {
                        Name = win8,
                        Version = "NT 6.2",
                        UserAgent = v8,
                        buildNumber = "9200",
                        codeName = win8,
                        fullName = fullName,
                        Type = Type
                    };
                case "7601":
                    return new Info()
                    {
                        Name = win7,
                        Version = "NT 6.1",
                        UserAgent = v7,
                        buildNumber = "7601",
                        codeName = win7,
                        fullName = fullName,
                        Type = Type
                    };
                case "6002":
                    return new Info()
                    {
                        Name = winVista,
                        Version = "NT 6.0",
                        UserAgent = vVista,
                        buildNumber = "6002",
                        codeName = "Longhorn",
                        fullName = fullName,
                        Type = Type
                    };
                case "2715":
                    return new Info()
                    {
                        Name = winXP,
                        Version = "NT 5.2",
                        UserAgent = vXP1,
                        buildNumber = "2715",
                        codeName = "Emerald",
                        fullName = fullName,
                        Type = Type
                    };
                case "3790":
                    return new Info()
                    {
                        Name = winXP,
                        Version = "NT 5.2",
                        UserAgent = vXP1,
                        buildNumber = "3790",
                        codeName = "Anvil",
                        fullName = fullName,
                        Type = Type
                    };
                case "2600":
                    return new Info()
                    {
                        Name = winXP,
                        Version = "NT 5.1",
                        UserAgent = vXP,
                        buildNumber = "2600",
                        codeName = "Symphony, Harmony, Freestyle, Whistler",
                        fullName = fullName,
                        Type = Type
                    };
                case "3000":
                    return new Info()
                    {
                        Name = winMe,
                        Version = "4.9",
                        UserAgent = vMe,
                        buildNumber = "3000",
                        codeName = "Millennium",
                        fullName = fullName,
                        Type = Type
                    };
                case "1998":
                    return new Info()
                    {
                        Name = win98,
                        Version = "4.10",
                        UserAgent = v98,
                        buildNumber = "1998",
                        codeName = "Memphis",
                        fullName = fullName,
                        Type = Type
                    };
                case "2222":
                    return new Info()
                    {
                        Name = win98_2,
                        Version = "4.10",
                        UserAgent = v98,
                        buildNumber = "2222",
                        codeName = "Unknow",
                        fullName = fullName,
                        Type = Type
                    };
                case "2195":
                    return new Info()
                    {
                        Name = win2000,
                        Version = "NT 5.0",
                        UserAgent = v2000,
                        buildNumber = "2195",
                        codeName = "Windows NT 5.0",
                        fullName = fullName,
                        Type = Type
                    };

                default:
                    return new Info()
                    {
                        Name = "Unknow",
                        Version = "Unknow",
                        UserAgent = "Unknow",
                        buildNumber = "Unknow",
                        codeName = "Unknow"
                    };
            }
        }
    }
}
