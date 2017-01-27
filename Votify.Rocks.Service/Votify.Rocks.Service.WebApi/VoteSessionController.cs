using System;
using System.Web.Http;
using Votify.Rocks.Service.Models;
using Votify.Rocks.VoteSession.SignalR;

namespace Votify.Rocks.Service.WebApi
{
    [RoutePrefix("")]
    public class VoteSessionController : ApiController
    {
        private readonly IVoteSessionService _voteSessionService;
        private readonly ISignalRVoteSessionBroadcaster _signalRVoteSessionBroadcaster;


        public VoteSessionController(IVoteSessionService voteSessionService, ISignalRVoteSessionBroadcaster signalRVoteSessionBroadcaster)
        {
            if (voteSessionService == null) throw new ArgumentNullException(nameof(voteSessionService));
            if (signalRVoteSessionBroadcaster == null) throw new ArgumentNullException(nameof(signalRVoteSessionBroadcaster));
            _voteSessionService = voteSessionService;
            _signalRVoteSessionBroadcaster = signalRVoteSessionBroadcaster;
        }

        /// <summary>
        /// Create a new vote session
        /// </summary>
        /// <param name="sessionKey">The key of the session</param>
        [Route("{sessionKey}")]
        [HttpGet]
        public VoteSessionDto Get(string sessionKey)
        {
            var voteSession = _voteSessionService.Get(sessionKey);
            return VoteSessionDto.Map(voteSession);
        }

        /// <summary>
        /// Create a new vote session
        /// </summary>
        /// <param name="participant">Organizer Participant</param>
        /// <param name="signalRClientId">The unique SignaR client GUID</param>
        /// <param name="description">Describe the purpose of this vote session</param>
        [Route("Create")]
        [HttpPost]
        public VoteSessionDto Create(Participant participant, string signalRClientId="", string description="")
        {
            var voteSession = _voteSessionService.Create(participant, description);
            if (!string.IsNullOrWhiteSpace(signalRClientId))
            {
                _signalRVoteSessionBroadcaster.JoinVoteSessionGroup(signalRClientId, voteSession.SessionKey);
            }
            return VoteSessionDto.Map(voteSession);
        }

        /// <summary>
        /// Join a vote session
        /// </summary>
        /// <param name="sessionKey">The key of the session you want to join</param>
        /// <param name="participant">Joining Participant</param>
        /// <param name="signalRClientId">The unique SignaR client ID</param>
        [Route("{sessionKey}/Join")]
        [HttpPost]
        public VoteSessionJoinResult Join(string sessionKey, Participant participant, string signalRClientId = "")
        {       
            var joinResult = _voteSessionService.Join(sessionKey, participant);
            _signalRVoteSessionBroadcaster.BroadcastParticipantJoined(joinResult.Votesession.SessionKey, participant);
            if (!string.IsNullOrWhiteSpace(signalRClientId))
                _signalRVoteSessionBroadcaster.JoinVoteSessionGroup(signalRClientId, joinResult.Votesession.SessionKey);
            return joinResult;
        }

        /// <summary>
        /// Join a vote session
        /// </summary>
        /// <param name="sessionKey">The key of the session you want to join</param>
        /// <param name="participantUid">Participant unique id</param>
        [Route("{sessionKey}/Open")]
        [HttpPost]
        public void Open(string sessionKey, Guid participantUid)
        {
            _voteSessionService.Open(sessionKey, participantUid);
            var voteSession = _voteSessionService.Get(sessionKey);
            _signalRVoteSessionBroadcaster.BroadcastVoteSessionOpen(voteSession.SessionKey);
        }

        /// <summary>
        /// Leave the vote session
        /// </summary>
        /// <param name="sessionKey">The key of the session you want leave</param>
        /// <param name="participantUid">Unique Id of the Participant</param>
        /// <param name="signalRClientId">The unique SignaR client Id</param>
        [Route("{sessionKey}/{participantUid}/Leave")]
        [HttpPost]
        public void Leave(string sessionKey, Guid participantUid, string signalRClientId = "")
        {
            var participant = _voteSessionService.Leave(sessionKey, participantUid);
            var voteSession = _voteSessionService.Get(sessionKey);
            if (!string.IsNullOrWhiteSpace(signalRClientId))
                _signalRVoteSessionBroadcaster.LeaveVoteSessionGroup(signalRClientId, voteSession.SessionKey);

            _signalRVoteSessionBroadcaster.BroadcastParticipantLeft(voteSession.SessionKey, participantUid, participant.DisplayName);
        }

        /// <summary>
        /// Cast your vote
        /// </summary>
        /// <param name="sessionKey">The key of the session you want cast a vote for</param>
        /// <param name="participantUid">Unique Id of the Participant</param>
        /// <param name="value">Vote value between 1 and 5</param>
        [Route("{sessionKey}/{participantUid}/Vote")]
        [HttpPost]
        public VoteSessionDto Vote(string sessionKey, Guid participantUid, int value)
        {
            var voteSession = _voteSessionService.CastVote(sessionKey, participantUid, value);
            _signalRVoteSessionBroadcaster.BroadcastParticipantVoted(voteSession.SessionKey, participantUid, value, voteSession.VoteAverage);
            return VoteSessionDto.Map(voteSession);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message">Masseage to broadcast to client(s)</param>
        /// <param name="groupName">SignalR client group, empty will broadcast to all connected clients</param>
        /// <param name="signalRClientId">Unique SignalR client ID, empty will broadcast to all connected clients</param>
        [Route("SignalRPing")]
        [HttpPost]
        public void SignalRPing(string message, string groupName = "", string signalRClientId = "")
        {
            _signalRVoteSessionBroadcaster.BroadcastPing(message, groupName, signalRClientId);
        }
    }
}
