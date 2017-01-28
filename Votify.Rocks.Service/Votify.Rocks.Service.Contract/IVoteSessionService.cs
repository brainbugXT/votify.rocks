using System;
using System.Threading.Tasks;
using Votify.Rocks.Service.Models;

namespace Votify.Rocks.Service
{
    public interface IVoteSessionService
    {
        Task<VoteSession> CreateAsync(Participant organizer, string description);
        Task<VoteSession> GetAsync(string sessionKey);
        Task<VoteSessionJoinResult> JoinAsync(string sessionKey, Participant participant);
        VoteSessionJoinResult ReJoin(string sessionKey, Guid participantUid);
        Task<Participant> LeaveAsync(string sessionKey, Guid participantUid);
        Task<VoteSession> CastVoteAsync(string sessionKey, Guid participantUid, int value);
        Participant CreateParticipant(string displayName, bool canVote);
        Task<VoteSession> ChangeParticipantNameAsync(string sessionKey, Guid participantUid, string newDiplayName);
        Task<VoteSession> PromoteParticipantToOrganizerAsync(string sessionKey, Guid orgenizerUid, Guid participantUid);
        Task<Participant> KickParticipantAsync(string sessionKey, Guid orgenizerUid, Guid participantUid);
        Task OpenAsync(string sessionKey, Guid orgenizerUid);
    }
}
