namespace Coursework2001.Models
{
    public class AuthResult
    {
        public bool IsValid { get; }
        public bool IsAdmin { get; }

        private AuthResult(bool isValid, bool isAdmin)
        {
            IsValid = isValid;
            IsAdmin = isAdmin;
        }

        public static AuthResult Success(bool isAdmin)
        {
            return new AuthResult(true, isAdmin);
        }

        public static AuthResult Failure()
        {
            return new AuthResult(false, false);
        }
    }


}
