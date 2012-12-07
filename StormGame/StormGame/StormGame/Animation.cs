using System;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StormGame
{
    /// <summary>
    /// Represents an animated texture.
    /// </summary>
    /// <remarks>
    /// Currently, this class assumes that each frame of animation is
    /// as wide as each animation is tall. The number of frames in the
    /// animation are inferred from this.
    /// </remarks>
    public class Animation
    {
        /// <summary>
        /// Duration of time to show each frame.
        /// </summary>
        public float FrameTime
        {
            get { return frameTime; }
        }
        float frameTime;

        /// <summary>
        /// When the end of the animation is reached, should it
        /// continue playing from the beginning?
        /// </summary>
        public bool IsLooping
        {
            get { return isLooping; }
        }
        bool isLooping;

        /// <summary>
        /// Gets the number of frames in the animation.
        /// </summary>
        public int FrameCount
        {
            get { return frameCount; }
        }
        int frameCount;

        /// <summary>
        /// Gets the width of a frame in the animation.
        /// </summary>
        public int FrameWidth
        {
            get { return GetFrameRectangle(Name + "-0").Width; }
        }

        /// <summary>
        /// Gets the height of a frame in the animation.
        /// </summary>
        public int FrameHeight
        {
            get { return GetFrameRectangle(Name + "-0").Height; }
        }

        /// <summary>
        /// Constructors a new animation.
        /// </summary>        
        public Animation(Texture2D spriteSheet, Dictionary<string, Rectangle> map)
        {
            this.AnimationSheet = spriteSheet;
            this.Map = map;
        }
        public Texture2D AnimationSheet { get; set; }
        public Dictionary<string, Rectangle> Map { get; set; }

        public Rectangle GetFrameRectangle(string frameName)
        {
            return Map[frameName];
        }

        public string Name;
        public void SetAnimation(string animationName, float frameTime, bool isLooping)
        {
            this.Name = animationName;
            this.frameTime = frameTime;
            this.isLooping = isLooping;
        }

        public bool ContainsAnimation(string animation)
        {
            if (Map[animation+"-0"] != null)
                return true;
            else
                return false;
        }
    }
}
