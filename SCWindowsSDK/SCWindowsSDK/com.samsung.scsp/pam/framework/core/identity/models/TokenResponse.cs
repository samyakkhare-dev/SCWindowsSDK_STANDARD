namespace SCWindowsSDK.com.samsung.scsp.pam.framework.core.identity.models
{
    public class TokenResponse
    {
        public string accessToken { get; set; }
        public string tokenType { get; set; }
        public long expiresAt { get; set; }
    }
}
