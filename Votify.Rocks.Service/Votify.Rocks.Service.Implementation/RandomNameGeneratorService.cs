using System;

namespace Votify.Rocks.Service
{
    public class RandomNameGeneratorService : IRandomNameGeneratorService
    {
        Random _rnd = new Random();

        private readonly string[] _partsA = { "Red", "Green", "Blue", "Violet", "Pink","Silver", "Gold", "Black", "White" };
        private readonly string[] _partsB = { "Spoon", "Chopstick", "Fork", "Knife", "Shoe", "Sock", "Rug", "Hat" };
        private readonly string[] _partsC = { "Crusher", "Destroyer", "Demolisher", "Painter", "Restorer", "Seller" };
        public string Generate()
        {
            
            var partA = _partsA[_rnd.Next(0, 8)];
            var partB = _partsB[_rnd.Next(0, 7)];
            var partC = _partsC[_rnd.Next(0, 5)];

            return string.Format($"{partA}{partB}{partC}");
        }

        
    }
}
