using Serko.Services;

namespace Votify.Rocks.Service.Exceptions
{
    public class VoteSessionNotFoundException : BadRequestException
    {
        public VoteSessionNotFoundException(string message) : base(message)
        {
        }
    }
}
