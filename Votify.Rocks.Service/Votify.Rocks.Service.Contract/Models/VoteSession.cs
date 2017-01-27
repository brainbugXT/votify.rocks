using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Votify.Rocks.Service.Models
{
    public class VoteSession
    {
        public string SessionKey { get; set; }
        public Participant _organizer { get; set; }
        public Participant Organizer
        {
            get
            {
                if (_organizer == null && Participants.Any())
                {
                    _organizer = Participants.FirstOrDefault(x => x.IsOrganizer);
                }
                return _organizer;
            }
            protected set { _organizer = value; }
        }
        public List<Participant> Participants { get; set; }
        public double VoteAverage { get; set; }
        public bool OpenForVoting { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string Description { get; set; }
    }
}
