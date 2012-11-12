using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace StormGame
{
    /// <summary>
    /// Controls playback of an Animation.
    /// </summary>
    public struct AnimationPlayer
    {
        /// <summary>
        /// Gets the animation which is currently playing.
        /// </summary>
        public Animation Animation
        {
            get { return animation; }
        }
        Animation animation;

        /// <summary>
        /// Gets the index of the current frame in the animation.
        /// </summary>
        public int FrameIndex
        {
            get { return frameIndex; }
        }
        int frameIndex;

        string currentFrame;

        public bool OneTimeLoopComplete
        {
            get { return oneTimeLoopComplete; }
        }
        bool oneTimeLoopComplete;

        /// <summary>
        /// The amount of time in seconds that the current frame has been shown for.
        /// </summary>
        private float time;

        /// <summary>
        /// Gets a texture origin at the bottom center of each frame.
        /// </summary>
        public Vector2 Origin
        {
            get { return new Vector2(Animation.FrameWidth / 2.0f, Animation.FrameHeight); }
        }

        /// <summary>
        /// Begins or continues playback of an animation.
        /// </summary>
        public void PlayAnimation(Animation animation, string animationName, float frameTime, bool isLooping)
        {            
            // If this animation is already running, do not restart it.
            if (previousAnimationName == animationName)
            {
                if (Animation.Name == animation.Name)
                    return;
            }

            animation.SetAnimation(animationName, frameTime, isLooping);
            oneTimeLoopComplete = false;
            // Start the new animation.
            this.animation = animation;
            this.frameIndex = 0;
            this.time = 0.0f;
            this.currentFrame = animation.Name + "-0";
            previousAnimationName = animation.Name;
        }
        private string previousAnimationName;

        /// <summary>
        /// Advances the time position and draws the current frame of the animation.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        {
            if (Animation == null)
                throw new NotSupportedException("No animation is currently playing.");

            // Process passing time.
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (time > Animation.FrameTime)
            {
                time -= Animation.FrameTime;
                string searchName = animation.Name + "-";
                var frames = animation.Map.Keys.Where(x => x.Contains(searchName)).ToList();

                // Advance the frame index; looping or clamping as appropriate.
                if (Animation.IsLooping)
                {
                    frameIndex += 1;

                    if (frameIndex > frames.Count() - 1)
                        frameIndex = 0;

                    currentFrame = frames[frameIndex];
                }
                else
                {
                    if (frameIndex >= Math.Min(frameIndex + 1, Animation.FrameCount - 1))
                    {
                        oneTimeLoopComplete = true;
                    }

                    frameIndex = Math.Min(frameIndex + 1, Animation.FrameCount - 1);
                }
            }

            // Calculate the source rectangle of the current frame.
            Rectangle source = animation.GetFrameRectangle(currentFrame);

            // Draw the current frame.

            spriteBatch.Draw(animation.AnimationSheet, position, source, Color.White);
                
                //Animation., position, source, Color.White, 0.0f, Origin, 1.0f, spriteEffects, 0.0f);
        }
    }
}
