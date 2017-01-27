using Serko.Services;

namespace Votify.Rocks.Service.Exceptions
{
    public class ParticipantNotFoundException : BadRequestException
    {
        public ParticipantNotFoundException(string message) : base(message)
        {
        }
    }
}
