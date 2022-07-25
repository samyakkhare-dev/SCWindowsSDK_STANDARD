namespace SCWindowsSDK.com.samsung.scsp.pam.common
{
    public class Preferences
    {
        public void Write<T>(string key, T value)
        {
            Settings.Default.Add(key, value);
        }

        public T Read<T>(string key, T defaultValue)
        {
            return Settings.Default.Get(key, defaultValue);
        }


        public void clear()
        {
            Settings.Default.Reset();
            Settings.Default.Save();
        }

        public void remove(string name)
        {
            Settings.Default.Properties.Remove(name);
        }

    }
}
