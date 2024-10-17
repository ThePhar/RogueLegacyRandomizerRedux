using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueCastle.RandomChaos2DGodRays
{
    public class BasePostProcess
    {
        public Vector2 HalfPixel;

        public Texture2D BackBuffer;
        public Texture2D orgBuffer;

        public bool Enabled = true;
        protected Effect effect;

        protected Microsoft.Xna.Framework.Game Game;
        public RenderTarget2D newScene;

        ScreenQuad sq;

        public bool UsesVertexShader = false;

        protected SpriteBatch spriteBatch
        {
            get { return (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch)); }
        }

        public BasePostProcess(Microsoft.Xna.Framework.Game game)
        {
            Game = game;

        }

        public virtual void Draw(GameTime gameTime)
        {
            if (Enabled)
            {
                if (sq == null)
                {
                    sq = new ScreenQuad(Game);
                    sq.Initialize();
                }

                effect.CurrentTechnique.Passes[0].Apply();
                sq.Draw();
            }
        }
    }
}
