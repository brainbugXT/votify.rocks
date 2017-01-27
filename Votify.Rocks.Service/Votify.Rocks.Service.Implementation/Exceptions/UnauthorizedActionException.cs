using Serko.Services;

namespace Votify.Rocks.Service.Exceptions
{
    public class UnauthorizedActionException : BadRequestException
    {
        public UnauthorizedActionException(string message) : base(message)
        {
        }
    }
}
