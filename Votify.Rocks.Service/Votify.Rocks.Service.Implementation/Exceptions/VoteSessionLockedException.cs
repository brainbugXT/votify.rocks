using Serko.Services;

namespace Votify.Rocks.Service.Exceptions
{
    public class VoteSessionLockedException : BadRequestException
    {
        public VoteSessionLockedException(string message) : base(message)
        {
        }
    }
}
