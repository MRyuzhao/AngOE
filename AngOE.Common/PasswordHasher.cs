using DevOne.Security.Cryptography.BCrypt;

namespace AngOE.Common
{
    public abstract class PasswordHasher
    {
        public static string CreateHash(string password)
        {
            return BCryptHelper.HashPassword(password, BCryptHelper.GenerateSalt(5));
        }

        public static bool ValidateHash(string password, string hash)
        {
            return BCryptHelper.CheckPassword(password, hash);
        }
    }
}