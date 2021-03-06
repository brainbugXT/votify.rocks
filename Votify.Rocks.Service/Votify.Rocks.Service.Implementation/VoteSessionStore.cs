﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serko.Client.Azure.Storage;
using Votify.Rocks.Service.Models;

namespace Votify.Rocks.Service
{
    public class VoteSessionStore : IVoteSessionStore
    {
        private readonly string _connectionString;
        private readonly string _voteSessionTableName;

        public VoteSessionStore(string connectionString, string voteSessionTableName)
        {
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));
            _connectionString = connectionString;
            _voteSessionTableName = voteSessionTableName;
        }

        private StorageTable GetStorageTable(string tableName)
        {
            var storageTableClient = new StorageTableClient(_connectionString);
            return new StorageTable(storageTableClient, tableName);
        }

        public async Task<VoteSessionEntity> ReadAsync(string sessionKey)
        {
            var storageTable = GetStorageTable(_voteSessionTableName);

            var query = storageTable.BuildQuery<VoteSessionEntity>(rowKey:sessionKey);

            var queryResult = await storageTable.QueryAsync(query);

            return queryResult.FirstOrDefault();
        }

        public async Task WriteVoteSessionAsync(VoteSessionEntity voteSessionEntity)
        {
            var storageTable = GetStorageTable(_voteSessionTableName);
            await storageTable.InsertOrReplaceAsync(voteSessionEntity);
        }

        public async Task DeleteVoteSessionAsync(VoteSessionEntity voteSessionEntity)
        {
            var storageTable = GetStorageTable(_voteSessionTableName);
            await storageTable.DeleteAsync(voteSessionEntity);
        }
    }
}
