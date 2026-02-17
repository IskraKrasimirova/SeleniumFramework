using System.Security.Cryptography;
using System.Text;

namespace RestSharp.ApiTests.Utilities.Types
{
    public static class StringUtils
    {
        public static string Sha256(string input)
        {
            byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(hash).ToLowerInvariant();
        }
    }
}
