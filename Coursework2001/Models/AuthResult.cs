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
        public static AuthResult SuccessAdmin()
        {
            return new AuthResult(true, true, false);//archiving an admin does nothing
        }

        public static AuthResult Success(bool isArchived)
        {
            return new AuthResult(true, false, isArchived);//valid user
        }



        public static AuthResult Failure()
        {
            return new AuthResult(false, false, false);//invalid user
        }
    }


}
