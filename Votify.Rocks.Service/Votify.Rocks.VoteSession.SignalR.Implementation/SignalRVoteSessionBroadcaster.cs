using System;
using Microsoft.AspNet.SignalR.Infrastructure;
using Votify.Rocks.Service.Models;
using Votify.Rocks.VoteSession.SignalR.Hubs;

namespace Votify.Rocks.VoteSession.SignalR
{
    public class SignalRVoteSessionBroadcaster : ISignalRVoteSessionBroadcaster
    {
        private readonly Func<IConnectionManager> _connectionManager;

        public SignalRVoteSessionBroadcaster(Func<IConnectionManager> connectionManager)
        {
            if (connectionManager == null) throw new ArgumentNullException(nameof(connectionManager));
            _connectionManager = connectionManager;
        }

        public void BroadcastPing(string message, string groupName = "", string signalRClientId = "")
        {
            var hub = _connectionManager().GetHubContext<SimpleVoteSessionHub>();
            if (!string.IsNullOrWhiteSpace(groupName))
            {
                hub.Clients.Group(groupName).Ping(message);
                return;
            }

            if (string.IsNullOrWhiteSpace(signalRClientId))
            {
                hub.Clients.All.Ping(message);
                return;
            }

            hub.Clients.Client(signalRClientId).Ping(message);
        }

        public void BroadcastParticipantJoined(string voteSessionKey, Participant participant)
        {
            var hub = _connectionManager().GetHubContext<SimpleVoteSessionHub>();
            hub.Clients.Group(voteSessionKey).ParticipantJoin(participant);
        }

        public void BroadcastVoteSessionOpen(string voteSessionKey)
        {
            var hub = _connectionManager().GetHubContext<SimpleVoteSessionHub>();
            hub.Clients.Group(voteSessionKey).VoteSessionOpen();
        }

        public void JoinVoteSessionGroup(string signalRClientId, string voteSessionKey)
        {
            var hub = _connectionManager().GetHubContext<SimpleVoteSessionHub>();
            hub.Groups.Add(signalRClientId, voteSessionKey);
        }

        public void LeaveVoteSessionGroup(string signalRClientId, string voteSessionKey)
        {
            var hub = _connectionManager().GetHubContext<SimpleVoteSessionHub>();
            hub.Groups.Remove(signalRClientId, voteSessionKey);
        }

        public void BroadcastParticipantLeft(string voteSessionKey, Guid participantUid, string displayName)
        {
            var hub = _connectionManager().GetHubContext<SimpleVoteSessionHub>();
            hub.Clients.Group(voteSessionKey).ParticipantLeave(participantUid.ToString(), displayName);
        }

        public void BroadcastParticipantVoted(string voteSessionKey, Guid participantUid, int voteValue, double voteSessionAverage)
        {
            var hub = _connectionManager().GetHubContext<SimpleVoteSessionHub>();
            hub.Clients.Group(voteSessionKey).ParticipantVote(participantUid.ToString(), voteValue, voteSessionAverage);
        }
    }
}
