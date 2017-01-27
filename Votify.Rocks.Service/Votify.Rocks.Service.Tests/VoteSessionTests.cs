using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Votify.Rocks.Service.Exceptions;

namespace Votify.Rocks.Service.Tests
{
    [TestFixture]
    public class VoteSessionTests
    {
        private VoteSessionService _underTest;
        private ICacheObject _cacheObjectMock;
        private Mock<IVoteSessionStoreService> _voteSessionStoreServiceMock;
        private int _maxParticipants = 20;

        [SetUp]
        public void Setup()
        {
            _cacheObjectMock = new MemoryCacheObject(TimeSpan.FromMinutes(1));
            _voteSessionStoreServiceMock = new Mock<IVoteSessionStoreService>();
            _underTest = new VoteSessionService(_voteSessionStoreServiceMock.Object, _cacheObjectMock, _maxParticipants);
        }

        [TestCase("John", "John4")]
        [TestCase("John2", "John22")]
        [TestCase("John3", "John32")]
        public void GenerateUniqueDisplayNameTests(string testData, string expected)
        {
            var names = new List<string> {"Mike", "John", "John2", "John3", "Dave"};
            var result = _underTest.GenerateUniqueDisplayName(testData, names);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GenerateVoteSessionKeyTests()
        {
            var uniqueSessionKeys = new List<string>();
            for (int i = 0; i < 100; i++)
            {
                System.Threading.Thread.Sleep(16);
                uniqueSessionKeys.Add(_underTest.GenerateVoteSessionKey());
            }
            var duplicates = uniqueSessionKeys.GroupBy(x => x).Where(g => g.Count() > 1).ToList().Count;
            Assert.AreEqual(0, duplicates);
        }

        [TestCase(true, 20)]
        [TestCase(false, 21)]
        [ExpectedException(typeof(MaxParticipantsException))]
        public void MaxParticipantsTests(bool canVote, int participantCount)
        {
            var organizer = _underTest.CreateParticipant("Organizer", canVote);
            var voteSession = _underTest.Create(organizer, "");

            for (int i = 0; i < participantCount; i++)
            {
                var newParticipant = _underTest.CreateParticipant("Participant", true);
                _underTest.Join(voteSession.SessionKey, newParticipant);
            }
        }

        [Test]
        public void ParticipantsTests()
        {
            var organizer = _underTest.CreateParticipant("Organizer", false);
            var voteSession = _underTest.Create(organizer, "");

            for (int i = 0; i < 20; i++)
            {
                var newParticipant = _underTest.CreateParticipant("Participant", true);
                _underTest.Join(voteSession.SessionKey, newParticipant);
            }

            Assert.AreEqual(1, voteSession.Participants.Count(x => x.IsOrganizer));
            Assert.AreEqual(20, voteSession.Participants.Count(x => x.CanVote));
        }

        [Test]
        public async Task VoteSessionsMustBeCaseInsesitive()
        {
            var organizer = _underTest.CreateParticipant("Organizer", false);
            var voteSession = _underTest.Create(organizer, "");

            var voteSessionFromMemory = await _underTest.GetAsync(voteSession.SessionKey.ToLower());

            Assert.AreSame(voteSession, voteSessionFromMemory);
        }
    }
}
