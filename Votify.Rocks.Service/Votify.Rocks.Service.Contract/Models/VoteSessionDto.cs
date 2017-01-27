using System;
using System.Collections.Generic;
using System.Linq;

namespace Votify.Rocks.Service.Models
{
    public class VoteSessionDto
    {
        public static VoteSessionDto Map(VoteSession voteSession)
        {
            return new VoteSessionDto
            {
                SessionKey = voteSession.SessionKey,
                Participants = voteSession.Participants,
                Description = voteSession.Description,
                OrganizerUid = voteSession.Participants.FirstOrDefault(x => x.IsOrganizer)?.Uid,
                VoteAverage = voteSession.VoteAverage,
                OpenForVoting = voteSession.OpenForVoting
            };
        }

        public string SessionKey { get; set; }
        public Guid? OrganizerUid { get; set; }
        public Guid? ParticipantUid { get; set; }
        public List<Participant> Participants { get; set; }
        public double VoteAverage { get; set; }
        public bool OpenForVoting { get; set; }
        public string Description { get; set; }
    }
}
