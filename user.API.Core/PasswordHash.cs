using System.Security.Cryptography;
using System.Text;

namespace User.Api.Core
{
    public class PasswordHash
    {
        public static string Generate(string password, string username)
        {
            using var sha256 = SHA256.Create();
            var raw = Encoding.UTF8.GetBytes(password + username);
            var hash = sha256.ComputeHash(raw);
            return Convert.ToBase64String(hash);
        }

        public static ServiceState Validate(string password, string username, string hash)
        {
            var generatedHash = Generate(password, username);
            if (generatedHash == hash) return ServiceState.Accepted;
            return ServiceState.Rejected;
        }
    }
}