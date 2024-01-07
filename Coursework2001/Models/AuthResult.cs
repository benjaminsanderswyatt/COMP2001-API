namespace Coursework2001.Models
{
    public class AuthResult
    {
        public bool IsValid { get; }
        public bool IsAdmin { get; }
        public bool IsArchived { get; }

        private AuthResult(bool isValid, bool isAdmin, bool isArchived)
        {
            IsValid = isValid;
            IsAdmin = isAdmin;
            IsArchived = isArchived;
        }

        public static AuthResult Success(bool isAdmin, bool isArchived)
        {
            return new AuthResult(true, isAdmin, isArchived);
        }

        public static AuthResult Failure()
        {
            return new AuthResult(false, false, false);
        }
    }


}
