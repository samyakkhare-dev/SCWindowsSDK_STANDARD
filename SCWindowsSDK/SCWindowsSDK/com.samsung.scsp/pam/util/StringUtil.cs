namespace SCWindowsSDK.com.samsung.scsp.pam.util
{
    public class StringUtil
    {
        /**
        * Returns true if the string is null or 0-length.
        * @param str the string to be examined
        */
        public static bool isEmpty(string str)
        {
            return str == null || str.Length == 0;
        }

        /**
         * Returns true if a and b are not null and equal
         * @param str,str: the strings to be compared
         */
        public static bool equals(string a, string b)
        {
            if (a == null || b == null)
                return false;
            return string.Equals(a, b);
        }

    }
}
