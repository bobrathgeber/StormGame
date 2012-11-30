namespace StormGame
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Layer
    {
        public Layer(Camera camera)
        {
            _camera = camera;
            Parallax = Vector2.One;
            Sprites = new List<DrawableObject>();
        }

        public Vector2 Parallax { get; set; }

        public List<DrawableObject> Sprites { get; private set; }

        public void ResetTiles()
        {
            Sprites = new List<DrawableObject>();
        }

        public void Draw(SpriteBatch spriteBatch, List<DrawableObject> drawableObjects, Storm storm)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _camera.GetViewMatrix(Parallax));

            foreach (DrawableObject sprite in Sprites)
                sprite.Draw();            

            foreach (DrawableObject dObj in drawableObjects)
            {
                dObj.Draw();
            }

            storm.Draw();

            spriteBatch.End();
        }

        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, _camera.GetViewMatrix(Parallax));
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(_camera.GetViewMatrix(Parallax)));
        }

        private readonly Camera _camera;
    }
}
