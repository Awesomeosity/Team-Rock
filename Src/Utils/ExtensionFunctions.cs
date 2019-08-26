using System;

namespace TeamRock.Utils
{
    public static class ExtensionFunctions
    {
        private static readonly Random _random = new Random();

        public static float RandomInRange(float minValue, float maxValue)
        {
            double nextRandom = _random.NextDouble();
            return (float) (minValue + nextRandom * (maxValue - minValue));
        }

        public static float Random()
        {
            return (float) _random.NextDouble();
        }

        public static float Map(float from, float fromMin, float fromMax, float toMin, float toMax)
        {
            var fromAbs = from - fromMin;
            var fromMaxAbs = fromMax - fromMin;

            var normal = fromAbs / fromMaxAbs;

            var toMaxAbs = toMax - toMin;
            var toAbs = toMaxAbs * normal;

            var to = toAbs + toMin;

            return to;
        }
    }
}