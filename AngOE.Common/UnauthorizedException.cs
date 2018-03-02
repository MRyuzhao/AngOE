namespace AngOE.Common
{
    public class UnauthorizedException:AngOeException
    {
        public UnauthorizedException(string message) : base(message)
        {
        }
    }
}