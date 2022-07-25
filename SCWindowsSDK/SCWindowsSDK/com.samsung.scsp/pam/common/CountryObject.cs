using System;
using System.Globalization;

namespace SCWindowsSDK.com.samsung.scsp.pam.common
{
    public class CountryObject
    {

        public class Info
        {
            public string osLanguage = "";
            public string deviceRegion = "";


            public override string ToString()
            {
                return "[Country Object Info]\n" +
                    "osLanguage : " + osLanguage + "\n"
                    + "deviceRegion : " + deviceRegion
                    ;
            }

        }

        public static Info GetInfo()
        {

            TimeZoneInfo localZone = TimeZoneInfo.Local;
            var result = localZone.StandardName;
            var s = result.Split(' ');


            return new Info()
            {
                deviceRegion = s[0],
                osLanguage = RegionInfo.CurrentRegion.TwoLetterISORegionName

            };

        }

    }
}
