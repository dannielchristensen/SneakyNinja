using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace SneakyNinja
{
    public static class RandomHelper
    {
        static Random random = new Random();
        public static int Next(int minValue, int maxValue) => random.Next(minValue, maxValue);

        public static int Next() => random.Next();

        public static float NextFloat()
        {
            return (float)random.NextDouble();
        }
        public static float NextFloat(float minValue, float maxValue)
        {
            return minValue + (float)random.NextDouble() * (maxValue - minValue);
        }

        public static Vector2 RandomPosition(Rectangle bounds)
        {
            return new Vector2(
                NextFloat(bounds.X, bounds.X + bounds.Width),
                NextFloat(bounds.Y, bounds.Y + bounds.Height)
                );
        }
    }
}
