using System;
using System.Threading.Tasks;
using Votify.Rocks.Service.Models;

namespace Votify.Rocks.Service
{
    public interface IVoteSessionService
    {
        VoteSession Create(Participant organizer, string description);
        Task<VoteSession> GetAsync(string sessionKey);
        Task<VoteSessionJoinResult> Join(string sessionKey, Participant participant);
        VoteSessionJoinResult ReJoin(string sessionKey, Guid participantUid);
        Task<Participant> Leave(string sessionKey, Guid participantUid);
        Task<VoteSession> CastVote(string sessionKey, Guid participantUid, int value);
        Participant CreateParticipant(string displayName, bool canVote);
        Task<VoteSession> ChangeParticipantName(string sessionKey, Guid participantUid, string newDiplayName);
        Task<VoteSession> PromoteParticipantToOrganizer(string sessionKey, Guid orgenizerUid, Guid participantUid);
        Task<Participant> KickParticipant(string sessionKey, Guid orgenizerUid, Guid participantUid);
        Task Open(string sessionKey, Guid orgenizerUid);
    }
}
