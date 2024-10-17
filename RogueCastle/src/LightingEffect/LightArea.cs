using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueCastle.LightingEffect;

internal class LightArea
{
    private readonly GraphicsDevice _graphicsDevice;

    public LightArea(GraphicsDevice graphicsDevice, ShadowmapSize size)
    {
        var baseSize = 2 << (int)size;
        LightAreaSize = new Vector2(baseSize);
        RenderTarget = new RenderTarget2D(graphicsDevice, baseSize, baseSize, false, SurfaceFormat.Color,
            DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
        _graphicsDevice = graphicsDevice;
    }

    public RenderTarget2D RenderTarget { get; }
    public Vector2 LightPosition { get; set; }
    public Vector2 LightAreaSize { get; set; }

    public Vector2 ToRelativePosition(Vector2 worldPosition)
    {
        return worldPosition - (LightPosition - (LightAreaSize * 0.5f));
    }

    public void BeginDrawingShadowCasters()
    {
        _graphicsDevice.SetRenderTarget(RenderTarget);
        _graphicsDevice.Clear(Color.Transparent);
    }

    public void EndDrawingShadowCasters()
    {
        _graphicsDevice.SetRenderTarget(null);
    }
}
