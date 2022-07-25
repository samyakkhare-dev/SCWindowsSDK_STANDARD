namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity
{
    public interface AccountInfoSupplier
    {
        /**
        * Returns appId
        * This field cannot be null
        * @return Application ID registered on the Samsung Account
        */
        string getAppId();

        /**
         * Returns accessToken
         * @return AccessToken received by the Samsung Account
         */
        string getAccessToken();

        /**
         * Returns userId
         * @return account's guid received from Samsung Account
         */
        string getUserId();

        /**
         * This is called when the current accessToken expires.
         * Please re-issue accessToken from Samsung Account in this method,
         * and then please call {@link AccountInfoSupplier#update()}
         */
        void onUnauthorized();

        /**
         * Please call after completing accessToken re-issue in {@link AccountInfoSupplier#onUnauthorized()}
         */
        void update();

        /**
         * Please call when receiving SignOut broadcast from Samsung Account
         */
        void signOut();

        bool has();
    }
}
