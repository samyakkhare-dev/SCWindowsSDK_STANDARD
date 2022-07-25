using SCWindowsSDK.com.samsung.scsp.pam.framework.core;
using Microsoft.Win32;
using System.Globalization;
using System;
using SCWindowsSDK.com.samsung.scsp.pam.common;

namespace SCWindowsSDK.com.samsung.scsp.pam.util
{
    public class DeviceUtil
    {
        private static readonly string TAG = "[DeviceUtil]";
        private static readonly string DEFAULT_MCC = "";
        private static readonly string DEFAULT_MNC = "";
        private static readonly string DEFAULT_CSC = "";
        private static string deviceUniqueId = "";
        private static readonly string BIOS_REGISTRY_PATH = @"HKEY_LOCAL_MACHINE\HARDWARE\DESCRIPTION\System\BIOS";
        private static readonly string DEVICE_INFO_REGISTRY_PATH = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion";
        private static readonly string PRODUCT_ID = "ProductId";
        private static readonly string BASE_BOARD_PRODUCT = "BaseBoardProduct";
        private static readonly string BASE_BOARD_MANUFACTURER = "BaseBoardManufacturer";


        public static string getDeviceUniqueId()
        {
            RegistryKey regKey = null;
            if (StringUtil.isEmpty(deviceUniqueId))
            {
                try
                {
                    RegistryKey localKey;
                    if (Environment.Is64BitOperatingSystem)
                        localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                    else
                        localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);

                    deviceUniqueId = localKey.OpenSubKey(DEVICE_INFO_REGISTRY_PATH).GetValue(PRODUCT_ID).ToString();

                    SDKLogger.info(TAG + " deviceUniqueId: " + deviceUniqueId);
                    //var regDefault = Registry.LocalMachine.OpenSubKey(DEVICE_INFO_REGISTRY_PATH, false);
                    //deviceUniqueId =  regDefault.GetValue(PRODUCT_ID).ToString();

                    if (StringUtil.isEmpty(deviceUniqueId) || deviceUniqueId.Equals("0"))
                    {
                        throw new ScspException(ScspException.Code.NO_DEVICE_ID, "Runtime policy error. There is an exception while creating device id. {" + deviceUniqueId + "}");
                    }
                }
                catch (Exception e)
                {
                    throw new ScspException(ScspException.Code.NO_PERMISSION, "Runtime policy error.");
                }
                finally
                {
                    if (regKey != null)
                        regKey.Close();
                }
            }
            return deviceUniqueId;
        }
        public static string getDeviceName()
        {
            //TODO: implement alias
            return "MyGalaxy";
        }
        public static string getModelName()
        {
            if (getOsType() == "win")
            {
                string buildName = Registry.GetValue(BIOS_REGISTRY_PATH, BASE_BOARD_PRODUCT, "").ToString();
                SDKLogger.info(TAG + " " + buildName);
                return buildName;
            }
            return null;
        }

        public static string getIso3CountryCode()
        {
            RegionInfo region = new RegionInfo(RegionInfo.CurrentRegion.ToString());
            return region.ThreeLetterISORegionName;
        }

        public static string getOsVersion()
        {
            OperatingSystem os = Environment.OSVersion;
            string osMajorVersion = os.Version.Major.ToString();
            SDKLogger.info(TAG + " osMajorVersion: " + osMajorVersion);
            if (!StringUtil.isEmpty(osMajorVersion))
            {
                return osMajorVersion;
            }
            return "";
        }

        public static string getPlatformVersion()
        {
            OperatingSystem os = Environment.OSVersion;
            return os.Version.ToString();
        }

        public static string getOsType()
        {
            return "win";
        }
        public static string getDeviceType()
        {
            return "pc";
        }

        public static string getSimMcc()
        {
            return DEFAULT_MCC;
            //return "450";
        }

        public static string getSimMnc()
        {
            return DEFAULT_MNC;
            //return "05";
        }
    }
}
