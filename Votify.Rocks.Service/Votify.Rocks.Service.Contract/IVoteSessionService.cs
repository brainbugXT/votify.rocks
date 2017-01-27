using System;
using Votify.Rocks.Service.Models;

namespace Votify.Rocks.Service
{
    public interface IVoteSessionService
    {
        VoteSession Create(Participant organizer, string description);
        VoteSession Get(string sessionKey);
        VoteSessionJoinResult Join(string sessionKey, Participant participant);
        VoteSessionJoinResult ReJoin(string sessionKey, Guid participantUid);
        Participant Leave(string sessionKey, Guid participantUid);
        VoteSession CastVote(string sessionKey, Guid participantUid, int value);
        Participant CreateParticipant(string displayName, bool canVote);
        VoteSession ChangeParticipantName(string sessionKey, Guid participantUid, string newDiplayName);
        VoteSession PromoteParticipantToOrganizer(string sessionKey, Guid orgenizerUid, Guid participantUid);
        Participant KickParticipant(string sessionKey, Guid orgenizerUid, Guid participantUid);
        void Open(string sessionKey, Guid orgenizerUid);
    }
}
