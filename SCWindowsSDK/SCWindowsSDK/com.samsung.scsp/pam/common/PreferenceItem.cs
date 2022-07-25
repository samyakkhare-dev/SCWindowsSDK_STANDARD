using System.Collections.Generic;
using System;
using SCWindowsSDK.com.samsung.scsp.pam.error;

namespace SCWindowsSDK.com.samsung.scsp.pam.common
{
    public class PreferenceItem<T>
    {
        private readonly string name;
        private readonly T defaultValue;
        private readonly Preferences preferences;

        private static readonly Dictionary<Type, Action<PreferenceItem<T>, object>> SETTERS = new Dictionary<Type, Action<PreferenceItem<T>, object>>();
        static PreferenceItem()
        {
            SETTERS.Add(typeof(T), (item, obj) => item.preferences.Write(item.name, (T)obj));

        }

        public PreferenceItem(Preferences preferences, string name, T defaultValue)
        {
            this.preferences = preferences;
            this.name = name;
            this.defaultValue = defaultValue;
        }


        public T get()
        {

            return FaultBarrier.get(() =>
            {

                object obj = preferences.Read(name, defaultValue);
                if (obj != null)
                {
                    return (T)obj;
                }
                return defaultValue;
            }, defaultValue).obj;
        }


        public void accept(T t)
        {
            foreach (var kvp in SETTERS)
            {
                kvp.Value.Invoke(this, t);
            }
        }

        public void remove()
        {
            preferences.remove(name);
        }

    }
}
