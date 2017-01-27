
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Votify.Rocks.Service.Models;

namespace Votify.Rocks.Service
{
    public interface IVoteSessionStore
    {
        Task DeleteVoteSessionAsync(VoteSessionEntity voteSessionEntity);
        Task<VoteSessionEntity> ReadAsync(string sessionKey);
        Task WriteVoteSessionAsync(VoteSessionEntity voteSessionEntity);
    }
}
