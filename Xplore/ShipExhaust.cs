using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore
{
    public class ShipExhaust : Sprite , IParticle
    {
        private float Speed = 5f;
        private float fadeSpeed = 20f;
        private float _fadeCurrent = 0f;
        public float FadePercent = 1f;
        private readonly Vector2 _directionVector;
        protected Vector2 Velocity = Vector2.Zero;


        public ShipExhaust(Texture2D texture, Vector2 position, Vector2 directionVector) : base(texture, position)
        {
            Origin = position;
            _directionVector = directionVector;
        }

        public override void Update(GameTime gameTime)
        {
            //lets spin
            //rotation = (float)_directionVector.GetRotationFromVector();
            Velocity = _directionVector * Speed;
            //Velocity = Vector2.Lerp(velocityGoal, Velocity, 0.99f);
            //progressivly fade


            _fadeCurrent += gameTime.ElapsedGameTime.Milliseconds;
            //fadeCurrent += Random.Next(3);
            if (_fadeCurrent > fadeSpeed)
            {
                _fadeCurrent = 0;
                FadePercent -= 0.1f;
            }
            if (FadePercent < 0f ) IsActive = false;
            //rotation = (float)getRotationFromDirection(directionVector);
            position = position + Velocity;
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White * FadePercent, Rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0f);
        }

        public Vector2 Origin { get; set; }
        public bool IsActive { get; set; } = true;
    }
}