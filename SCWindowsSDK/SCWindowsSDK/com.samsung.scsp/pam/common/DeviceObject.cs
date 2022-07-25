using Microsoft.Win32;

namespace SCWindowsSDK.com.samsung.scsp.pam.common
{
    public class DeviceObject
    {
        public class Info
        {
            public string deviceModel = "";
            public string deviceManufacturer = "";

            public override string ToString()
            {
                return "[Device Object Info]\n" +
                    "DeviceModel : " + deviceModel + "\n" +
                    "DeviceManufacturer : " + deviceManufacturer + "\n"
                    ;
            }

        }

        public static Info GetInfo()
        {
            string BIOS = @"HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\System\BIOS";
            string modelCode = Registry.GetValue(BIOS, "BaseBoardProduct", "").ToString();
            string modelManufacturer = Registry.GetValue(BIOS, "BaseBoardManufacturer", "").ToString();

            return new Info()
            {
                deviceModel = modelCode,
                deviceManufacturer = modelManufacturer
            };
        }

    }
}
