using System;

namespace Votify.Rocks.Service.Models
{
    public class Participant
    {
        public Guid? _uid { get; set; }

        public Guid Uid
        {
            get
            {
                if (!_uid.HasValue)
                {
                    _uid = Guid.NewGuid();
                }
                return _uid.Value;
            }
            protected set { _uid = value; }
        }

        public string DisplayName { get; set; }
        public bool IsOrganizer { get; set; }
        public int VoteValue { get; set; }
        public bool CanVote { get; set; }
        public string Email { get; set; }
    }
}
