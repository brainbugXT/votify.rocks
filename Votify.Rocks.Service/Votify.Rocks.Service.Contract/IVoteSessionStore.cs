
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Votify.Rocks.Service.Models;

namespace Votify.Rocks.Service
{
    public interface IVoteSessionStore
    {
        Task DeleteVoteSessionAsync(VoteSessionEntity voteSessionEntity);
        Task<IEnumerable<VoteSessionEntity>> ReadAsync(Guid voteSessionUid, string sessionId);
        Task WriteVoteSessionAsync(VoteSessionEntity voteSessionEntity);
    }
}
