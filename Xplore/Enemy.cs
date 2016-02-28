using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Xplore
{
    public class Enemy : Ship, IShip
    {
        private Vector2 _destination;
        private bool _seeking = false;
        private bool _fleeing = false;

        public Enemy(Texture2D texture, Vector2 position, Rectangle screenBounds) : base(texture, position, screenBounds)
        {
            RotationSpeed = 0.99f;
        }

        private void Flee(Vector2 location)
        {
            _fleeing = true;
            _destination = location;
        }

        private void Seek(Vector2 location)
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
            if (keyboardState.IsKeyDown(Keys.Space) && !previousKeyboardState.IsKeyDown(Keys.Space))
            {
                var mouseWorldPosition = Camera.GetWorldPosition(new Vector2(mouseState.X, mouseState.Y));
                Seek(mouseWorldPosition);
            }

            if (_seeking || _fleeing)
            {
                var directionVector = _destination - position;
                float vectorLength = directionVector.Length();
                if (_fleeing)
                {
                    directionVector = -directionVector;
                }
                directionVector.Normalize();
                VelocityGoal = directionVector * Speed;

                if (_seeking && vectorLength < 50)
                {
                    _seeking = false;
                }
                if (_fleeing && vectorLength > 1000)
                {
                    _fleeing = false;
                }

                DirectionGoalVector = directionVector;

                //check if we need to stop fleeing/seeking
            }
            else
            {
                VelocityGoal = Vector2.Zero;
            }
            velocity = Vector2.Lerp(VelocityGoal, velocity, 0.99f);
            DirectionVector = Vector2.Lerp(DirectionGoalVector, DirectionVector, RotationSpeed);

            rotation = (float)DirectionVector.GetRotationFromVector();
            base.Update(gameTime);
        }
    }
}