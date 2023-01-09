using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadinProjectNotes
{
    public class Security
    {
        public static readonly string ResetPassword = "000111222333444555666777888999AAABBBCCC0";   //password used when an administator resets a user's password

        public static byte[] desKey = { 10, 25, 11, 35, 207, 108, 56, 99 };
        public static byte[] desIV = { 38, 13, 7, 1, 116, 222, 111, 6 };

        public static string HashSHA1(string value)
        {
            var sha1 = System.Security.Cryptography.SHA1.Create();
            var inputBytes = Encoding.ASCII.GetBytes(value);
            var hash = sha1.ComputeHash(inputBytes);

            var sb = new StringBuilder();
            for (var i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }
    }
}
