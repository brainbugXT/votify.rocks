using Serko.Services;

namespace Votify.Rocks.Service.Exceptions
{
    public class MaxParticipantsException : BadRequestException
    {
        public MaxParticipantsException(string message) : base(message)
        {
        }
    }
}
