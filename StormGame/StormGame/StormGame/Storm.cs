using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace StormGame
{
    public class Storm : DrawableObject
    {        
        public float speed = 15f;

        private const float rotationSpeed = 0.13f;

        private StormLayer firstLayer;
        private StormLayer secondLayer;
        private StormLayer thirdLayer;
        private StormLayer fourthLayer;
        private StormLayer fifthLayer;
        public InventoryManager inventoryManager;
        public Healthbar stormHealth;
        public float orbitPullRange;
        public bool isExpanding = false;

        public Storm(StartingLocation startingPosition)
        {
            //Texture = Globals.Content.Load<Texture2D>("StormLines1");
            firstLayer = new StormLayer("StormLines1", startingPosition.Position);            
            secondLayer = new StormLayer("StormLines2", startingPosition.Position);
            thirdLayer = new StormLayer("StormLines3", startingPosition.Position);
            fourthLayer = new StormLayer("StormLines4", startingPosition.Position);
            fifthLayer = new StormLayer("StormLines5", startingPosition.Position);
            firstLayer.SetDepth(100);
            secondLayer.SetDepth(101);
            thirdLayer.SetDepth(102);
            fourthLayer.SetDepth(103);
            fifthLayer.SetDepth(104);
            inventoryManager = new InventoryManager(this);
            stormHealth = new Healthbar(10000, new Vector2(20));   
            Position = startingPosition.Position;
            Velocity = new Vector2();
            _color = new Color(255, 255, 255, 70);
            orbitPullRange = 100;

        }

        public void Update(GameTime gameTime)
        {
            UpdateStormLayers();
            inventoryManager.Update();
            foreach (var item in inventoryManager.GetDebris())
                item.Move(Position, Globals.GameTime);
        }

        private void UpdateStormLayers()
        {
            Rotation = (Rotation + rotationSpeed) % (MathHelper.Pi * 2);
            firstLayer.Rotation = (Rotation + (rotationSpeed * 1.0f)) % (MathHelper.Pi * 2);
            secondLayer.Rotation = (Rotation + (rotationSpeed * 1.5f)) % (MathHelper.Pi * 2);
            thirdLayer.Rotation = (Rotation + (rotationSpeed * 2f)) % (MathHelper.Pi * 2);
            fourthLayer.Rotation = (Rotation + (rotationSpeed * 2.5f)) % (MathHelper.Pi * 2);
            fifthLayer.Rotation = (Rotation + (rotationSpeed * 3f)) % (MathHelper.Pi * 2);


            firstLayer.Position = Position;
            Vector2 direction = Position - secondLayer.Position;
            secondLayer.Position += direction * 0.2f;

            direction = Position - thirdLayer.Position;
            thirdLayer.Position += direction * 0.11f;

            direction = Position - fourthLayer.Position;
            fourthLayer.Position += direction * 0.08f;

            direction = Position - fifthLayer.Position;
            fifthLayer.Position += direction * 0.06f;

        }

        public override void Draw()
        {
            fifthLayer.Draw();
            fourthLayer.Draw();
            thirdLayer.Draw();
            secondLayer.Draw();
            firstLayer.Draw();
            inventoryManager.Draw();
            stormHealth.Draw();
        }

        public void ApplyVelocity()
        {
            Position += Velocity;
        }

        public void BounceOffObject(Vector2 direction)
        {
            Velocity *= direction;
            Velocity *= 0.8f;
        }

        public void Expand(float deltaTime)
        {
            isExpanding = true;
            foreach (Item deb in inventoryManager.satalliteList)
            {
                var distance = (deb.Position - Position);
                if (distance.Length() < 200)
                    deb.Velocity += (distance * deltaTime * 20);
            }
            isExpanding = false;
        }

        public bool withinPickupRangeOf(DrawableObject obj)
        {
            Vector2 Distance = Vector2.Subtract(obj.Position, Position);
            if (Distance.Length() <= orbitPullRange)
                return true;
            else
                return false;
        }
    }
}
