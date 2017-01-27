using System;
using System.Collections.Generic;
using System.Linq;

namespace Votify.Rocks.Service.Models
{
    public class VoteSession
    {
        public string SessionKey { get; set; }
        public Participant Organizer
        {
            get { return Participants.FirstOrDefault(x => x.IsOrganizer); }
        }
        public List<Participant> Participants { get; set; }
        public double VoteAverage { get; set; }
        public bool OpenForVoting { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string Description { get; set; }
    }
}
