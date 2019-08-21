using System;

namespace TeamRock.Utils
{
    public static class ExtensionFunctions
    {
        private static Random _random = new Random();

        public static float RandomInRange(float minValue, float maxValue)
        {
            double nextRandom = _random.NextDouble();
            return (float)(minValue + nextRandom * (maxValue - minValue));
        }

        public static float Random()
        {
            return (float)_random.NextDouble();
        }
    }
}