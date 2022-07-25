namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity
{
    public interface PushInfoSupplier
    {
        /**
        * Returns PushInfo
        * @return Array of PushInfo
        */
        PushInfo[] getPushInfo();
        void update();
        bool has();
    }
}
