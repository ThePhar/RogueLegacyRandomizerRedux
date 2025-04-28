using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.EnvironmentVariables;

namespace RogueCastle;

public class FrameRateCounter : DrawableGameComponent {
    private readonly ContentManager _content;
    private TimeSpan _elapsedTime = TimeSpan.Zero;
    private int _frameCounter;
    private int _frameRate;
    private SpriteBatch _spriteBatch;
    private SpriteFont _spriteFont;

    public FrameRateCounter(Game game) : base(game) {
        _content = Game.Content;
    }

    protected override void LoadContent() {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _spriteFont = _content.Load<SpriteFont>(@"Fonts\FpsFont");
    }

    protected override void UnloadContent() {
        _content.Unload();
    }

    public override void Update(GameTime gameTime) {
        _elapsedTime += gameTime.ElapsedGameTime;
        if (_elapsedTime <= TimeSpan.FromSeconds(1)) {
            return;
        }

        _elapsedTime -= TimeSpan.FromSeconds(1);
        _frameRate = _frameCounter;
        _frameCounter = 0;
    }

    public override void Draw(GameTime gameTime) {
        _frameCounter++;

        var fps = $"fps: {_frameRate}";

        _spriteBatch.Begin();

        _spriteBatch.DrawString(_spriteFont, fps, new Vector2(GlobalEV.SCREEN_WIDTH - 150, 33), Color.Black);
        _spriteBatch.DrawString(_spriteFont, fps, new Vector2(GlobalEV.SCREEN_WIDTH - 149, 32), Color.White);

        _spriteBatch.End();
    }
}
