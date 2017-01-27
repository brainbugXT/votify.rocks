using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Votify.Rocks.Service.Models
{
    public class VoteSessionJoinResult
    {
        public Guid ParticipantUid { get; set; }
        public VoteSessionDto Votesession { get; set; }
    }
}
