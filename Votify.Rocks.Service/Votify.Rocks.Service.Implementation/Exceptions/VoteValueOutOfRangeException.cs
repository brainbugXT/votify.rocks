using Serko.Services;

namespace Votify.Rocks.Service.Exceptions
{
    public class VoteValueOutOfRangeException : BadRequestException
    {
        public VoteValueOutOfRangeException(string message) : base(message)
        {
        }
    }
}
