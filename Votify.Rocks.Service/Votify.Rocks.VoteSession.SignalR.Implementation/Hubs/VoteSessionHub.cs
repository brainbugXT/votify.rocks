using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Votify.Rocks.Service;

namespace Votify.Rocks.VoteSession.SignalR.Hubs
{
    public class VoteSessionHub : Hub
    {
        private readonly IVoteSessionService _voteSessionService;
        private readonly ClientVoteSessionManager _voteSessionBroadcaster;

        public VoteSessionHub(IVoteSessionService voteSessionService) : this(ClientVoteSessionManager.Instance)
        {
            if (voteSessionService == null) throw new ArgumentNullException(nameof(voteSessionService));
            _voteSessionService = voteSessionService;
        }

        public VoteSessionHub(ClientVoteSessionManager voteSessionBroadcaster)
        {
            _voteSessionBroadcaster = voteSessionBroadcaster;
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var autoEvent = new AutoResetEvent(false);
            var deferredDisconnect = new DeferredDisconnect(Context, Clients, _voteSessionBroadcaster, _voteSessionService);

            TimerCallback tcb = deferredDisconnect.DisconnectAndBroadcast;

            const int fifteenSeconds = 15000;
            var disconnectTimer = new Timer(tcb, autoEvent, fifteenSeconds, 0);

            if (stopCalled)
            {
                var clientVoteSession = _voteSessionBroadcaster.ClientVoteSessions[Context.ConnectionId];
                if (clientVoteSession != null)
                {
                    clientVoteSession.Disconnected = true;
                    clientVoteSession.DisconnectTimeout = disconnectTimer;
                    _voteSessionBroadcaster.ClientVoteSessions.AddOrUpdate(Context.ConnectionId, clientVoteSession, (key, oldClientVoteSession) => clientVoteSession);
                }
            }
            else
            {
                ClientVoteSessionManager.ClientVoteSession removedClientVoteSession;
                _voteSessionBroadcaster.ClientVoteSessions.TryRemove(Context.ConnectionId, out removedClientVoteSession);

                if (removedClientVoteSession != null)
                {
                    Clients.Group(removedClientVoteSession.SessionKey)
                        .participantLeftViaHost(removedClientVoteSession.ParticipantUid);
                    removedClientVoteSession.DisconnectTimeout.Dispose();
                }

            }

            return base.OnDisconnected(stopCalled);
        }

        public async Task JoinVoteSessionGroup(string voteSessionKey, string participantUid)
        {
            await Groups.Add(Context.ConnectionId, voteSessionKey);
            var uid = Guid.Parse(participantUid);
            var oldConnectionId = _voteSessionBroadcaster.ClientVoteSessions.Where(
                    y => y.Value.SessionKey == voteSessionKey && y.Value.ParticipantUid == uid && y.Value.Disconnected).Select(x => x.Key).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(oldConnectionId) && oldConnectionId != Context.ConnectionId)
            {
                ClientVoteSessionManager.ClientVoteSession removedClientVoteSession;
                _voteSessionBroadcaster.ClientVoteSessions.TryRemove(oldConnectionId, out removedClientVoteSession);
                removedClientVoteSession?.DisconnectTimeout?.Dispose();
            }

            var newClientVoteSession = new ClientVoteSessionManager.ClientVoteSession
            {
                SessionKey = voteSessionKey,
                ParticipantUid = uid,
                Disconnected = false
            };

            _voteSessionBroadcaster.ClientVoteSessions.TryAdd(Context.ConnectionId, newClientVoteSession);
        }

        private class DeferredDisconnect
        {
            private readonly HubCallerContext _hubContext;
            private readonly IHubCallerConnectionContext<dynamic> _hubClients;
            private readonly ClientVoteSessionManager _voteSessionBroadcaster;
            private readonly IVoteSessionService _voteSessionService;

            public DeferredDisconnect(HubCallerContext hubContext, IHubCallerConnectionContext<dynamic> hubClients, ClientVoteSessionManager voteSessionBroadcaster, IVoteSessionService voteSessionService)
            {
                _hubContext = hubContext;
                _hubClients = hubClients;
                _voteSessionBroadcaster = voteSessionBroadcaster;
                _voteSessionService = voteSessionService;
            }

            public async void DisconnectAndBroadcast(object state)
            {
                ClientVoteSessionManager.ClientVoteSession clientVoteSession;
                _voteSessionBroadcaster.ClientVoteSessions.TryRemove(_hubContext.ConnectionId, out clientVoteSession);

                if (clientVoteSession == null) return;

                var voteSession = await _voteSessionService.GetAsync(clientVoteSession.SessionKey);

                var participant = voteSession?.Participants.FirstOrDefault(x => x.Uid == clientVoteSession.ParticipantUid);
                if (participant != null)
                {
                    //if (!participant.IsOrganizer && participant.VoteValue <= 0)
                    //{
                    //    voteSession = _voteSessionService.Leave(clientVoteSession.SessionKey,
                    //        clientVoteSession.ParticipantUid);
                    //}
                    //_hubClients.Group(clientVoteSession.SessionKey)
                    //    .participantLeft(participant, voteSession);
                }
            }
        }
    }
}
