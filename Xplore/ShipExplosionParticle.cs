using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore
{
    public class ShipExplosionParticle : Sprite, IParticle
    {
        protected Vector2 Velocity = Vector2.Zero;
        private float Speed = 3f;
        private float fadeSpeed = 60f;
        private float _fadeCurrent = 0f;
        public float fadePercent = 1f;
        private Vector2 _directionVector;
        
        
        public ShipExplosionParticle(Texture2D texture, Vector2 position,Vector2 directionVector,float speed) : base(texture, position)
        {
            IsActive = true;
            Origin = position;
            Speed = Speed * speed;
            _directionVector = directionVector;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White * fadePercent, rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0f);
        }

        public override void Update(GameTime gameTime)
        {
            //lets spin
            //rotation = (float)_directionVector.GetRotationFromVector();
            Velocity = _directionVector * Speed ;
            //Velocity = Vector2.Lerp(velocityGoal, Velocity, 0.99f);
            //progressivly fade

            _fadeCurrent += gameTime.ElapsedGameTime.Milliseconds;
            //fadeCurrent += Random.Next(3);
            if (_fadeCurrent > fadeSpeed)
            {
                _fadeCurrent = 0;
                fadePercent -= 0.1f;
            }
            if (fadePercent < 0f) IsActive = false;
            //rotation = (float)getRotationFromDirection(directionVector);
            position = position + Velocity;
            base.Update(gameTime);
        }

        public Vector2 Origin { get; set; }
        public bool IsActive { get; set; }
    }
}