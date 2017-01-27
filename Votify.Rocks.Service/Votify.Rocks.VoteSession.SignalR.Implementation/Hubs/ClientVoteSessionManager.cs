using System;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.AspNet.SignalR;

namespace Votify.Rocks.VoteSession.SignalR.Hubs
{
    public class ClientVoteSessionManager
    {
        // Singleton instance
        private static readonly Lazy<ClientVoteSessionManager> _instance =
            new Lazy<ClientVoteSessionManager>(
                () => new ClientVoteSessionManager(GlobalHost.ConnectionManager.GetHubContext<VoteSessionHub>()));

        private readonly ConcurrentDictionary<string, ClientVoteSession> _clientVoteSessions =
            new ConcurrentDictionary<string, ClientVoteSession>();

        private ClientVoteSessionManager(IHubContext hubContext)
        {
            HubContext = hubContext;
            _clientVoteSessions.Clear();
        }

        public static ClientVoteSessionManager Instance => _instance.Value;
        public ConcurrentDictionary<string, ClientVoteSession> ClientVoteSessions => _clientVoteSessions;

        private IHubContext HubContext { get; set; }


        //public void BroadcastParticipantJoined(Participant participant, VoteSession voteSession)
        //{
        //    HubContext.Clients.Group(voteSession.SessionKey).participantJoined(participant);
        //}

        //public void BroadcastParticipantLeft(Guid participantUid, VoteSession voteSession)
        //{
        //    HubContext.Clients.Group(voteSession.SessionKey).participantLeft(participantUid);
        //}

        //public void BroadcastParticipantVoted(Guid participantUid, int voteValue, VoteSession voteSession)
        //{
        //    HubContext.Clients.Group(voteSession.SessionKey).participantVoted(participantUid, voteValue, voteSession.VoteAverage);
        //}

        //public void BroadcastSessionOpen(VoteSession voteSession)
        //{
        //    HubContext.Clients.Group(voteSession.SessionKey).startSessionOpenCountdown();
        //}

        public class ClientVoteSession
        {
            public string SessionKey { get; set; }
            public Guid ParticipantUid { get; set; }
            public bool Disconnected { get; set; }
            public Timer DisconnectTimeout { get; set; }
        }
    }
}