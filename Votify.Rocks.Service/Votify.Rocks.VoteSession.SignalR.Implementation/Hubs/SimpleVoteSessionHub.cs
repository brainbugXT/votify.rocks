using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Votify.Rocks.VoteSession.SignalR.Hubs
{
    public class SimpleVoteSessionHub : Hub
    {
        public async Task JoinVoteSessionGroup(string voteSessionKey)
        {
            await Groups.Add(Context.ConnectionId, voteSessionKey);
        }
    }
}
