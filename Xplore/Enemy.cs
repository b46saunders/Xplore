using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Xplore
{
    public class Enemy : Ship, IShip
    {
        private readonly HealthBar _healthBar;
        private Vector2 _destination;
        private const float SeekDistance = 100f;
        private const float FleeDistance = 200f;
        public event EventHandler Destroyed;
        private double _lastWanderDecision;
        private const float CircleDistance = 0.4f;
        private const float CircleRadius = 100f;
        private float _wanderAngle = 30f;
        private float _angleChange = 60f;
        private readonly Random _random;
        private ShipBehaviour _shipBehaviour;

        public Enemy(Texture2D texture, Vector2 position, Rectangle screenBounds) : base(texture, position, screenBounds)
        {
            _lastWanderDecision = 0;
            _random = new Random();
            Speed = 3.5f;
            DirectionVector = new Vector2(0, -1);
            RotationSpeed = 0.95f;
            _healthBar = new HealthBar(this);
        }
        
        public void Flee(Vector2 location)
        {
            _shipBehaviour = ShipBehaviour.Flee;
            _destination = location;
        }

        public void Seek(Vector2 location)
        {
            _shipBehaviour = ShipBehaviour.Seek;
            _destination = location;
        }

        public void Wander()
        {
            _shipBehaviour = ShipBehaviour.Wander;
        }

        private void WanderBehaviour(GameTime gameTime)
        {
            throw new NotImplementedException();
            //TODO needs implementing
            //var newVector = Wander(gameTime);

            //var normailzed = new Vector2(newVector.X, newVector.Y);
            //normailzed.Normalize();
            //DirectionGoalVector = normailzed;
        }

        private  void SeekBehaviour()
        {
            var directionVector = _destination - Center;
            float vectorLength = directionVector.Length();
            if (vectorLength < SeekDistance)
            {
                
                _shipBehaviour = ShipBehaviour.None;
                return;
            }
            directionVector.Normalize();

            DirectionGoalVector = directionVector;

            var c = Math.Abs(Vector2.Dot(DirectionVector, directionVector));
            var a = MathHelper.Clamp(c, 0, 1);
            VelocityGoal = DirectionVector * Speed * a;
        }

        private void FleeBehaviour()
        {
            var directionVector = -(_destination - Center);
            float vectorLength = directionVector.Length();
            if (vectorLength > FleeDistance)
            {
                
                _shipBehaviour = ShipBehaviour.None;
                return;
            }
            directionVector.Normalize();
            DirectionGoalVector = directionVector;

            var c = Math.Abs(Vector2.Dot(DirectionVector, directionVector));
            var a = MathHelper.Clamp(c, 0, 1);
            VelocityGoal = DirectionVector * Speed * a;
            
        }

        public override void Update(GameTime gameTime)
        {
            //DEBUGGIN HELP
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
            //END DEBUGGING HELP

            switch (_shipBehaviour)
            {
                case ShipBehaviour.Seek:
                    SeekBehaviour();
                    break;
                case ShipBehaviour.Flee:
                    FleeBehaviour();
                    break;
                case ShipBehaviour.Wander:
                    WanderBehaviour(gameTime);
                    break;
                default:
                    VelocityGoal = Vector2.Zero;
                    break;
            }

            if (_shipBehaviour != ShipBehaviour.None)
            {
                CreateExhaustParticles();
            }

            Velocity = Vector2.Lerp(VelocityGoal, Velocity, 0.99f);
            DirectionVector = Vector2.Lerp(DirectionGoalVector, DirectionVector, RotationSpeed);

            CheckBounds();
            rotation = (float) DirectionVector.GetRotationFromVector();
            _healthBar.Update(gameTime);

            CheckIfDestroyed();

            base.Update(gameTime);
        }

        public void CheckIfDestroyed()
        {
            if (HealthPoints == 0)
            {
                Destroyed?.Invoke(this, null);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _healthBar.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        private Vector2 Wander(GameTime gameTime)
        {
            var circleCenter = new Vector2(Velocity.X, Velocity.Y);
            circleCenter.Normalize();
            circleCenter *= CircleDistance;
            //calc displacement force
            var displacement = new Vector2(0, -1);
            displacement *= CircleRadius;

            //randomly change the vector direction
            // by making it change its current angle
            SetAngle(displacement, _wanderAngle);

            //change wander angle just a bit
            //so it wont have the same value
            //in the next game frame

            _wanderAngle += _random.Next(0, 100)*_angleChange - _angleChange*.5f;

            var wanderForce = circleCenter + displacement;
            return wanderForce;
        }

        private void SetAngle(Vector2 displacement, float angle)
        {
            var length = displacement.Length();
            displacement.X = (float) Math.Cos(angle)*length;
            displacement.X = (float) Math.Sin(angle)*length;
        }
    }
}