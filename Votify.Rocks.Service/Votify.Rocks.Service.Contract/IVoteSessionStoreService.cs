using System.Threading.Tasks;
using Votify.Rocks.Service.Models;

namespace Votify.Rocks.Service
{
    public interface IVoteSessionStoreService
    {
        Task SaveVoteSessionAsync(VoteSession voteSession);
        Task<VoteSession> LoadVoteSessionAsync(string voteSessionKey);
        Task RemoveVoteSessionAsync(VoteSession votesession);
    }
}
