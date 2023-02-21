using System;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace CottageApi.Core.Helpers
{
    public static class EncryptionHelper
    {
        public static string HashWithHMACMD5(string value, string secret)
        {
            var data = Encoding.UTF8.GetBytes(value);
            var key = Encoding.UTF8.GetBytes(secret);

            using (HMACMD5 hmacMd5Hash = new HMACMD5(key))
            {
                var hashBytes = hmacMd5Hash.ComputeHash(data);

                return BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
            }
        }

        public static string HashWithMD5(string value)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] hash = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(value));

                return HashToBinaryString(hash);
            }
        }

        public static string HashWithBASE64URL(string value)
        {
           return Base64UrlEncoder.Encode(value);
        }

        public static string DecodeBASE64URL(string value)
        {
            return Base64UrlEncoder.Decode(value);
        }

        private static string HashToBinaryString(byte[] hash)
        {
            StringBuilder output = new StringBuilder();

            foreach (var t in hash)
            {
                output.Append(t.ToString("x2"));
            }

            return output.ToString();
        }
    }
}
