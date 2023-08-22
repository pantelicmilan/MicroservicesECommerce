namespace AuthService.Exceptions
{
    public class InputValidationException : Exception
    {
        public InputValidationException(string msg) : base(msg){}
    }
}
