using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Xplore
{
    public class Enemy : Ship, IShip
    {
        private bool _destroyAnimationStarted;
        private readonly HealthBar _healthBar;
        private Vector2 _destination;
        private const float WanderRotationSpeed = 0.99f;
        private float SeekDistance = 100f;
        private float _fleeDistance = 200f;
        public event EventHandler Destroyed;
        private double _lastWanderDecision;
        private const float CircleDistance = 2f;
        private const float CircleRadius = 400f;
        private float _wanderAngle = 5f;
        private float _angleChange = 2f;
        private ShipBehaviour _shipBehaviour;

        public Enemy(Texture2D texture, Vector2 position,ShipType shipType) : base(texture, position, shipType)
        {
            _lastWanderDecision = 0;
            Speed = 3.5f;
            DirectionVector = new Vector2(0, 1);
            RotationSpeed = 0.95f;
            _healthBar = new HealthBar(new ShipHealthBar(this));
        }
        
        public void Flee(Vector2 location,float distance)
        {
            _fleeDistance = distance;
            Speed = 3.5f;
            _shipBehaviour = ShipBehaviour.Flee;
            _destination = location;
        }

        public void Seek(Vector2 location)
        {
            Speed = 3.5f;
            _shipBehaviour = ShipBehaviour.Seek;
            _destination = location;
        }

        public void Wander()
        {
            Speed = 2f;
            _shipBehaviour = ShipBehaviour.Wander;
        }

        private void WanderBehaviour(GameTime gameTime)
        {

            if (gameTime.TotalGameTime.TotalMilliseconds > _lastWanderDecision + 200)
            {
                _lastWanderDecision = gameTime.TotalGameTime.TotalMilliseconds;
                var newVector = Wander(gameTime);

                var normailzed = new Vector2(newVector.X, newVector.Y);
                normailzed.Normalize();
                DirectionGoalVector = normailzed;
            }
            var c = Math.Abs(Vector2.Dot(DirectionVector, DirectionGoalVector));
            var a = MathHelper.Clamp(c, 0, 1);
            VelocityGoal = DirectionVector * Speed * a;

        }

        private  void SeekBehaviour()
        {
            var directionVector = _destination - Center;
            float vectorLength = directionVector.Length();
            if (vectorLength < SeekDistance)
            {
                
                _shipBehaviour = ShipBehaviour.Wander;
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
            if (vectorLength > _fleeDistance)
            {

                _shipBehaviour = ShipBehaviour.Wander;
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
                Flee(mouseWorldPosition,2000f);
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

            Velocity = Vector2.Lerp(VelocityGoal, Velocity, 0.99f);
            if (_shipBehaviour == ShipBehaviour.Wander)
            {
                DirectionVector = Vector2.Lerp(DirectionGoalVector, DirectionVector, WanderRotationSpeed);
            }
            else
            {
                DirectionVector = Vector2.Lerp(DirectionGoalVector, DirectionVector, RotationSpeed);
            }
            rotation = (float) DirectionVector.GetRotationFromVector();
            _healthBar.Update(gameTime);

            CheckIfDestroyed();

            base.Update(gameTime);
        }

        public void CheckIfDestroyed()
        {
            if (HealthPoints == 0 && !_destroyAnimationStarted)
            {
                _destroyAnimationStarted = true;
                var explosion = new ShipExplosion(Center);
                CurrentParticles.Add(explosion);
                explosion.AnimationFinished += (sender, args) => Destroyed?.Invoke(this, null);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _healthBar.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }

        private Vector2 Wander(GameTime gameTime)
        {
            var circleCenter = new Vector2(DirectionVector.X, DirectionVector.Y);
            circleCenter.Normalize();
            circleCenter *= CircleDistance;
            
            //calc displacement force - this is 90 degrees left
            var displacement = new Vector2(0,-1);
            displacement.Normalize();
            displacement *= CircleRadius;

            //randomly change the vector direction
            // by making it change its current angle
            displacement = SetAngle(displacement, _wanderAngle);

            //change wander angle just a bit
            //so it wont have the same value
            //in the next game frame

            _wanderAngle += (float)Random.NextDouble() *_angleChange - _angleChange*.5f;

            var wanderForce = circleCenter + displacement;
            return wanderForce;
        }

        private Vector2 SetAngle(Vector2 displacement, float angle)
        {
            var length = displacement.Length();
            displacement.X = (float) Math.Cos(angle)*length;
            displacement.Y = (float) Math.Sin(angle)*length;
            return displacement;
        }
    }
}