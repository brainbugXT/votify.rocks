using System;
using System.Collections.Generic;
using System.Linq;
using Votify.Rocks.Service.Exceptions;
using Votify.Rocks.Service.Models;

namespace Votify.Rocks.Service
{
    public class VoteSessionService : IVoteSessionService
    {
        private readonly ICacheObject _memoryCacheObject;
        private readonly int _maxParticipants;

        public VoteSessionService(ICacheObject memoryCacheObject, int maxParticipants)
        {
            if (memoryCacheObject == null) throw new ArgumentNullException(nameof(memoryCacheObject));
            if (maxParticipants <= 0) throw new ArgumentOutOfRangeException(nameof(maxParticipants));
            _memoryCacheObject = memoryCacheObject;
            _maxParticipants = maxParticipants;
        }

        public VoteSession Create(Participant organizer, string description)
        {
            var voteSessionKey = GenerateVoteSessionKey();
            organizer.IsOrganizer = true;
            var voteSessionObject = new VoteSession
            {
                SessionKey = voteSessionKey,
                Participants = new List<Participant> { organizer },
                Description = description,
                CreateDateTime = DateTime.Now,
                OpenForVoting = false
            };

            _memoryCacheObject.SetCachedObject(voteSessionKey, voteSessionObject, TimeSpan.FromHours(1));

            return voteSessionObject;
        }

        public VoteSession Get(string sessionKey)
        {
            var voteSession = GetVoteSessionFromMemory(sessionKey);
            return voteSession;
        }

        public VoteSessionJoinResult Join(string sessionKey, Participant participant)
        {
            var voteSession = GetVoteSessionFromMemory(sessionKey);
            if (voteSession.Participants.Count(x => x.CanVote) >= _maxParticipants)
                throw new MaxParticipantsException($"The vote session has reached the maximum participants of {_maxParticipants}");

            var participantDisplayNames = voteSession.Participants.Select(x => x.DisplayName).ToList();
            participant.DisplayName = GenerateUniqueDisplayName(participant.DisplayName, participantDisplayNames);
            voteSession.Participants.Add(participant);
            
            return new VoteSessionJoinResult
            {
                ParticipantUid = participant.Uid,
                Votesession = VoteSessionDto.Map(voteSession)
            };
        }

        public void Open(string sessionKey, Guid participantUid)
        {
            var voteSession = GetVoteSessionFromMemory(sessionKey);
            if (participantUid != voteSession.Organizer.Uid)
                throw new UnauthorizedActionException("Only the vote session organizer can open the vote session");

            voteSession.OpenForVoting = true;
        }

        public VoteSessionJoinResult ReJoin(string sessionKey, Guid participantUid)
        {
            throw new NotImplementedException();

            return new VoteSessionJoinResult
            {
                ParticipantUid = participantUid,
                Votesession = null
            };
        }

        public Participant Leave(string sessionKey, Guid participantUid)
        {
            var voteSession = GetVoteSessionFromMemory(sessionKey);
            var participant = voteSession.Participants.FirstOrDefault(x => x.Uid == participantUid);
            if (participant == null) throw new ParticipantNotFoundException("Participant is not in the current vote session");

            voteSession.Participants = voteSession.Participants.Where(x => x.Uid != participantUid).ToList();

            return participant;
        }

        public VoteSession CastVote(string sessionKey, Guid participantUid, int value)
        {
            var voteSession = GetVoteSessionFromMemory(sessionKey);
            var participant = voteSession.Participants.FirstOrDefault(x => x.Uid == participantUid);
            if (participant == null) throw new ParticipantNotFoundException("Participant is not in the current vote session");
            if (!voteSession.OpenForVoting) throw new VoteSessionLockedException("This vote session is not open for voting");
            if (!ValidateVote(value))
                throw new VoteValueOutOfRangeException($"Vote value {value} is invalid, vote value must be a number between 1 and 5");
            participant.VoteValue = value;

            var participantsThatCanAndHasVoted = voteSession.Participants.Where(x => x.CanVote && x.VoteValue > 0).ToArray();
            float voteTotal = participantsThatCanAndHasVoted.Sum(x => x.VoteValue);
            if (voteTotal > 0)
                voteSession.VoteAverage = voteTotal / participantsThatCanAndHasVoted.Length;

            return voteSession;
        }

        public VoteSession ChangeParticipantName(string sessionKey, Guid participantUid, string newDiplayName)
        {
            var voteSession = GetVoteSessionFromMemory(sessionKey);
            var participant = voteSession.Participants.FirstOrDefault(x => x.Uid == participantUid);
            if (participant == null) throw new ParticipantNotFoundException("Participant is not in the current vote session");

            participant.DisplayName = newDiplayName;

            return voteSession;
        }

        public VoteSession PromoteParticipantToOrganizer(string sessionKey, Guid orgenizerUid, Guid participantUid)
        {
            var voteSession = GetVoteSessionFromMemory(sessionKey);
            if (voteSession.Organizer.Uid != orgenizerUid)
            {
                throw new UnauthorizedActionException("You are not the organizer of this session");
            }

            var organizer = voteSession.Participants.FirstOrDefault(x => x.Uid == orgenizerUid && x.IsOrganizer);
            if (organizer == null)
                throw new ParticipantNotFoundException("Organizer is not in the current vote session");

            var participant = voteSession.Participants.FirstOrDefault(x => x.Uid == participantUid);
            if (participant == null)
                throw new ParticipantNotFoundException("Participant is not in the current vote session");

            organizer.IsOrganizer = false;
            participant.IsOrganizer = true;

            return voteSession;
        }

        public Participant KickParticipant(string sessionKey, Guid orgenizerUid, Guid participantUid)
        {
            var voteSession = GetVoteSessionFromMemory(sessionKey);
            if (voteSession.Organizer.Uid != orgenizerUid)
            {
                throw new UnauthorizedAccessException("You are not the organizer of this session");
            }

            return Leave(sessionKey, participantUid);
        }

        public Participant CreateParticipant(string displayName, bool canVote)
        {
            return new Participant
            {
                DisplayName = displayName,
                CanVote = canVote,
                VoteValue = 0,
                IsOrganizer = false
            };
        }

        private VoteSession GetVoteSessionFromMemory(string sessionKey)
        {
            var voteSession = _memoryCacheObject.GetCachedObject(sessionKey, new VoteSession());
            if(string.IsNullOrWhiteSpace(voteSession.SessionKey)) throw new VoteSessionNotFoundException($"Vote session {sessionKey} not found");
            return voteSession;
        }
        internal string GenerateVoteSessionKey()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 4)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        internal string GenerateUniqueDisplayName(string displayName, List<string> names)
        {
            //Find the first possible available username.
            if (names.Count == 0 || !names.Contains(displayName))
            {
                return displayName;
            }

            for (int i = 2; i < 20; i++)
            {
                var possibleDisplayName = displayName + i;
                if (names.FindIndex(x => x.Equals(possibleDisplayName, StringComparison.OrdinalIgnoreCase)) == -1)
                    return possibleDisplayName;
            }

            return displayName;
        }

        internal bool ValidateVote(int voteValue)
        {
            var valids = new[] {1, 2, 3, 4, 5};

            return valids.Contains(voteValue);
        }
    }
}
