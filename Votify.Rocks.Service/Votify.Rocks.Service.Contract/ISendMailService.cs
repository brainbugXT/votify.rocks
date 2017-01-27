using System.Threading.Tasks;
using Votify.Rocks.Service.Models;

namespace Votify.Rocks.Service
{
    public interface ISendMailService
    {
        Task ShareSessionViaEmail(string email, string voteSessionKey, string base64GaugeImage = null);
    }
}
