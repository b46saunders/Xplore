using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Xplore
{
    public class ShipExhaust : Sprite , IParticle
    {
        private Vector2 velocityGoal;
        private float Speed = 3f;
        private float fadeSpeed = 90f;
        private float fadeCurrent = 0f;
        public float fadePercent = 1f;
        private Vector2 _directionVector;

        public ShipExhaust(Texture2D texture, Vector2 position, Vector2 directionVector) : base(texture, position)
        {
            Origin = position;
            _directionVector = directionVector;
        }

        public override void Update(GameTime gameTime)
        {

            var movementRegister = false;

            //lets spin
            //rotation = (float)_directionVector.GetRotationFromVector();
            velocityGoal = _directionVector * Speed;
            velocity = Vector2.Lerp(velocityGoal, velocity, 0.99f);
            //progressivly fade


            fadeCurrent += gameTime.ElapsedGameTime.Milliseconds;
            //fadeCurrent += random.Next(3);
            if (fadeCurrent > fadeSpeed)
            {
                fadeCurrent = 0;
                fadePercent -= 0.1f;
            }
            if (fadePercent < 0f ) IsActive = false;
            //rotation = (float)getRotationFromDirection(directionVector);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, null, Color.White * fadePercent, rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0f);
        }

        public Vector2 Origin { get; set; }
        public bool IsActive { get; set; } = true;
    }
}