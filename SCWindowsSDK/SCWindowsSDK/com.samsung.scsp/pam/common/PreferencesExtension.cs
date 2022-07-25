using System.Configuration;

namespace SCWindowsSDK.com.samsung.scsp.pam.common
{
    public static class PreferencesExtension
    {
        public static void Add<T>(this ApplicationSettingsBase settings, string propertyName, T val)
        {
            bool itemExists = false;
            foreach (SettingsProperty property in settings.Properties)
            {
                if (property.Name == propertyName)
                {
                    itemExists = true;
                    break;
                }
            }


            if (!itemExists)
            {
                var p = new SettingsProperty(propertyName)
                {
                    PropertyType = typeof(T),
                    Provider = settings.Providers["LocalFileSettingsProvider"],
                    SerializeAs = SettingsSerializeAs.Xml
                };

                p.Attributes.Add(typeof(UserScopedSettingAttribute), new UserScopedSettingAttribute());

                settings.Properties.Add(p);
                settings.Reload();
            }

            settings[propertyName] = val;
            //TODO: check the cast exception while saving
            settings.Save();
        }

        public static T Get<T>(this ApplicationSettingsBase settings, string propertyName, T defaultValue)
        {
            bool itemExists = false;
            foreach (SettingsProperty property in settings.Properties)
            {
                if (property.Name == propertyName)
                {
                    itemExists = true;
                    break;
                }
            }
            if (!itemExists)
            {
                var p = new SettingsProperty(propertyName)
                {
                    PropertyType = typeof(T),
                    Provider = settings.Providers["LocalFileSettingsProvider"],
                    SerializeAs = SettingsSerializeAs.Xml
                };

                p.Attributes.Add(typeof(UserScopedSettingAttribute), new UserScopedSettingAttribute());

                settings.Properties.Add(p);
                settings.Reload();
            }
            //finally set value with new value if none was loaded from userConfig.xml
            var item = settings[propertyName];
            if (item == null)
            {
                settings[propertyName] = defaultValue;
                //TODO: check the cast exception while saving
                settings.Save();
            }

            return (T)settings[propertyName];
        }

    }
}
