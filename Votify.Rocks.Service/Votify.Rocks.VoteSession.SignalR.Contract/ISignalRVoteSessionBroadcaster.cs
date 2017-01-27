using System;
using Votify.Rocks.Service.Models;

namespace Votify.Rocks.VoteSession.SignalR
{
    public interface ISignalRVoteSessionBroadcaster
    {
        void BroadcastPing(string message, string groupName = "", string signalRClientId = "");
        void BroadcastParticipantJoined(string voteSessionKey, Participant participant);
        void BroadcastParticipantLeft(string voteSessionKey, Guid participantUid, string displayName);
        void BroadcastParticipantVoted(string voteSessionKey, Guid participantUid, int voteValue, double voteSessionAverage);

        void BroadcastVoteSessionOpen(string voteSessionKey);
        void JoinVoteSessionGroup(string signalRClientId, string voteSessionKey);
        void LeaveVoteSessionGroup(string signalRClientId, string voteSessionKey);
    }
}
