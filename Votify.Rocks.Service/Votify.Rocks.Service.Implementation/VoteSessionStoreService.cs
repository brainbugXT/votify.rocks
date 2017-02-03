using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Votify.Rocks.Service.Models;

namespace Votify.Rocks.Service
{
    public class VoteSessionStoreService : IVoteSessionStoreService
    {
        private readonly IVoteSessionStore _voteSessionStore;

        public VoteSessionStoreService(IVoteSessionStore voteSessionStore)
        {
            if (voteSessionStore == null) throw new ArgumentNullException(nameof(voteSessionStore));
            _voteSessionStore = voteSessionStore;
        }

        public async Task SaveVoteSessionAsync(VoteSession voteSession)
        {
            var voteSessionEntity = new VoteSessionEntity
            {
                RowKey = voteSession.SessionKey.ToUpper(),
                PartitionKey = voteSession.Organizer.Email,
                VoteSessionJSON = JsonConvert.SerializeObject(voteSession)
            };
            await _voteSessionStore.WriteVoteSessionAsync(voteSessionEntity);
        }

        public async Task<VoteSession> LoadVoteSessionAsync(string voteSessionKey)
        {
            var voteSessionEntity = await _voteSessionStore.ReadAsync(voteSessionKey.ToUpper());
            if (voteSessionEntity == null)
            {
                return null;
            }
            var voteSession = JsonConvert.DeserializeObject<VoteSession>(voteSessionEntity.VoteSessionJSON);
            return voteSession;
        }

        public async Task RemoveVoteSessionAsync(VoteSession voteSession)
        {
            var voteSessionEntity = new VoteSessionEntity
            {
                RowKey = voteSession.SessionKey,
                PartitionKey = voteSession.Organizer.Email,
                VoteSessionJSON = JsonConvert.SerializeObject(voteSession)
            };
            await _voteSessionStore.DeleteVoteSessionAsync(voteSessionEntity);
        }
    }
}
