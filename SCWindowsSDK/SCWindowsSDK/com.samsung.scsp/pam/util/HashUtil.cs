using SCWindowsSDK.com.samsung.scsp.pam.framework.core;
using System;
using System.Security.Cryptography;
using System.Text;

namespace SCWindowsSDK.com.samsung.scsp.pam.util
{
    public class HashUtil
    {
        //public static readonly string SHA256 = "SHA-256";

        private static readonly char[] DIGITS_LOWER =
                {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'};

        /**
         * Used to build output as Hex
         */
        private static readonly char[] DIGITS_UPPER =
                {'0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};


        private static byte[] decodeHex(char[] data)
        {
            int len = data.Length;
            if ((len & 0x01) != 0)
            {
                throw new ScspException(ScspException.Code.BAD_IMPLEMENTATION, "Odd number of characters.");
            }

            byte[] out1 = new byte[len >> 1];

            // two characters form the hex value.
            for (int i = 0, j = 0; j < len; i++)
            {
                int f = toDigit(data[j], j) << 4;
                j++;
                f = f | toDigit(data[j], j);
                j++;
                out1[i] = (byte)(f & 0xFF);
            }

            return out1;
        }

        private static string encodeHexString(byte[] data)
        {
            return new string(encodeHex(data));
        }

        private static char[] encodeHex(byte[] data)
        {
            return encodeHex(data, true);
        }


        private static char[] encodeHex(byte[] data, bool toLowerCase)
        {
            return encodeHex(data, toLowerCase ? DIGITS_LOWER : DIGITS_UPPER);
        }


        private static char[] encodeHex(byte[] data, char[] toDigits)
        {
            int l = data.Length;
            char[] out1 = new char[l << 1];
            // two characters form the hex value.
            for (int i = 0, j = 0; i < l; i++)
            {
                out1[j++] = toDigits[(0xF0 & data[i]) >> 4];
                out1[j++] = toDigits[0x0F & data[i]];
            }
            return out1;
        }


        private static int toDigit(char ch, int index)
        {
            int digit = (int)char.GetNumericValue(ch);
            if (digit == -1)
            {
                throw new ScspException(ScspException.Code.BAD_IMPLEMENTATION, "Illegal hexadecimal character " + ch + " at index " + index);
            }
            return digit;
        }

        public static string getStringSHA256(string data)
        {
            SHA256 mySHA256 = SHA256.Create();
            byte[] dataArray = Encoding.UTF8.GetBytes(data);
            byte[] encoded = mySHA256.ComputeHash(dataArray);
            return Encoding.UTF8.GetString(encoded);
        }

        public static byte[] CreateHash(string input)

        {

            int HASH_SIZE = 16; // size in bytes

            int ITERATIONS = 30; // number of pbkdf2 iterations



            // Generate a salt

            string saltString = "windowsq";  //string saltString = input.Substring(0, 8);

            byte[] salt = Encoding.ASCII.GetBytes(saltString);



            // Generate the hash

            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(input, salt, ITERATIONS);



            return pbkdf2.GetBytes(HASH_SIZE);

        }

        public static string ByteArrayToString(byte[] ba)

        {

            return BitConverter.ToString(ba).Replace("-", "").ToLower();

        }
    }
}
