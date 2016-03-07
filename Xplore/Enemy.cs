using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Xplore
{
    public class Enemy : Ship, IShip
    {
        private HealthBar healthBar;
        private Vector2 _destination;
        private readonly float _seekDistance = 100f;
        private readonly float _fleeDistance = 200f;
        private bool _seeking = false;
        private bool _fleeing = false;

        public Enemy(Texture2D texture, Vector2 position, Rectangle screenBounds) : base(texture, position, screenBounds)
        {
            Speed = 2f;
            DirectionVector = new Vector2(0,-1);
            RotationSpeed = 0.95f;
            healthBar = new HealthBar(this);
        }

        public void Flee(Vector2 location)
        {
            _fleeing = true;
            _destination = location;
        }

        public void Seek(Vector2 location)
        {
            _seeking = true;
            _destination = location;
        }

        private void Wander()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                var mouseWorldPosition = Camera.GetWorldPosition(new Vector2(mouseState.X, mouseState.Y));
                Seek(mouseWorldPosition);
            }
            if (keyboardState.IsKeyDown(Keys.G))
            {
                var mouseWorldPosition = Camera.GetWorldPosition(new Vector2(mouseState.X, mouseState.Y));
                Flee(mouseWorldPosition);
            }

            if (_seeking || _fleeing)
            {
                Maneuver();
            }
            else
            {
                VelocityGoal = Vector2.Zero;
            }
            Velocity = Vector2.Lerp(VelocityGoal, Velocity, 0.99f);
            DirectionVector = Vector2.Lerp(DirectionGoalVector, DirectionVector, RotationSpeed);

            CheckBounds();
            rotation = (float)DirectionVector.GetRotationFromVector();
            healthBar.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //if (_seeking)
            //{
            //    spriteBatch.Draw(ContentProvider.ExhaustParticles[0], _destination, Color.Black);
            //}
            healthBar.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        

        private void Maneuver()
        {
            var directionVector = _destination - Center;
            float vectorLength = directionVector.Length();
            if (_seeking && vectorLength < _seekDistance)
            {
                _seeking = false;
                VelocityGoal = Vector2.Zero;
                return;
            }
            if (_fleeing && vectorLength > _fleeDistance)
            {
                _fleeing = false;
                VelocityGoal = Vector2.Zero;
                return;
            }
            
            if (_fleeing)
            {
                directionVector = -directionVector;
            }
            directionVector.Normalize();

            DirectionGoalVector = directionVector;

            var c = Math.Abs(Vector2.Dot(DirectionVector, directionVector));
            var a = MathHelper.Clamp(c,0,1);
            VelocityGoal = DirectionVector * Speed * a;
            //CreateExhaustParticles();
        }

        
    }

}