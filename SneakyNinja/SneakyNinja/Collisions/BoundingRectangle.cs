using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;


namespace SneakyNinja.Collisions
{
    public struct BoundingRectangle
    {
        /// <summary>
        /// X position of the upper left hand corner of the BoundingRectangle
        /// </summary>
        public float X;
        /// <summary>
        /// Y position of the upper left hand corner of the BoundingRectangle
        /// </summary>
        public float Y;
        /// <summary>
        /// Width of the BoundingRectangle
        /// </summary>
        public float Width;
        /// <summary>
        /// Height of the BoundingRectangle
        /// </summary>
        public float Height;
        /// <summary>
        /// Left most point of the BoundingRectangle
        /// </summary>
        public float Left => X;
        /// <summary>
        /// right most point of the BoundingRectangle
        /// </summary>
        public float Right => X + Width;
        /// <summary>
        /// top of the BoundingRectangle
        /// </summary>
        public float Top => Y;
        /// <summary>
        /// bottom of the BoundingRectangle
        /// </summary>
        public float Bottom => Y + Height;
        /// <summary>
        /// Creates a BouningRectangle using x and y coordinates
        /// </summary>
        /// <param name="x">x position of upper left hand corner</param>
        /// <param name="y">y position of the upper left hand corner</param>
        /// <param name="width">width of the BoundingRectangle</param>
        /// <param name="height">height of the BoundingRectangle</param>
        public BoundingRectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        /// <summary>
        /// Creates a BouningRectangle using a vector2
        /// </summary>
        /// <param name="pos">vector position of the bounding rectangle</param>
        /// <param name="width">width of the BoundingRectangle</param>
        /// <param name="height">height of the BoundingRectangle</param>
        public BoundingRectangle(Vector2 pos, float width, float height)
        {
            X = pos.X;
            Y = pos.Y;
            Width = width;
            Height = height;
        }
        /// <summary>
        /// checks if the rectangle has collided with another BoundingRectangle
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }
    }
}
