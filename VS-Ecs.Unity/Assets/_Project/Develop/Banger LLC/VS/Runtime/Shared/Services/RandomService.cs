using System;
using VS.Core.Configs.Features;

namespace VS.Runtime.Services
{
    public class RandomService : IRandomService
    {
        public static IRandomService Current { get; private set; }

        private readonly Random _random;

        public RandomService(SessionSettingsConfig config)
        {
            _random = new Random(config.LevelSeed);
            Current = this;
        }

        public int Next(int min, int max) => _random.Next(min, max);
    }
}
