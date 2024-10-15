using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.Screens;
using SpriteSystem;

namespace RogueCastle;

internal class VirtualScreen
{
    public readonly float VirtualAspectRatio;
    public readonly int   VirtualHeight;
    public readonly int   VirtualWidth;

    private Rectangle area;

    private bool areaIsDirty = true;

    private GraphicsDevice graphicsDevice;

    public VirtualScreen(int virtualWidth, int virtualHeight, GraphicsDevice graphicsDevice)
    {
        VirtualWidth = virtualWidth;
        VirtualHeight = virtualHeight;
        VirtualAspectRatio = virtualWidth / (float) virtualHeight;

        this.graphicsDevice = graphicsDevice;
        //screen = new RenderTarget2D(graphicsDevice, virtualWidth, virtualHeight, false, graphicsDevice.PresentationParameters.BackBufferFormat, graphicsDevice.PresentationParameters.DepthStencilFormat, graphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.DiscardContents);
        RenderTarget = new RenderTarget2D(graphicsDevice, virtualWidth, virtualHeight, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
    }

    public RenderTarget2D RenderTarget { get; private set; }

    public void ReinitializeRTs(GraphicsDevice graphicsDevice)
    {
        this.graphicsDevice = graphicsDevice;
        if (RenderTarget.IsDisposed == false)
        {
            RenderTarget.Dispose();
            RenderTarget = null;
        }

        //screen = new RenderTarget2D(graphicsDevice, VirtualWidth, VirtualHeight, false, graphicsDevice.PresentationParameters.BackBufferFormat, graphicsDevice.PresentationParameters.DepthStencilFormat, graphicsDevice.PresentationParameters.MultiSampleCount, RenderTargetUsage.DiscardContents);
        RenderTarget = new RenderTarget2D(graphicsDevice, VirtualWidth, VirtualHeight, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
    }

    public void PhysicalResolutionChanged()
    {
        areaIsDirty = true;
    }

    public void Update()
    {
        if (!areaIsDirty)
        {
            return;
        }

        areaIsDirty = false;
        var physicalWidth = graphicsDevice.Viewport.Width;
        var physicalHeight = graphicsDevice.Viewport.Height;
        var physicalAspectRatio = graphicsDevice.Viewport.AspectRatio;

        // This 'if' was commented out during Switch development, flibit added it back
        if ((int) (physicalAspectRatio * 10) == (int) (VirtualAspectRatio * 10))
        {
            area = new Rectangle(0, 0, physicalWidth, physicalHeight);
            return;
        }

        if (VirtualAspectRatio > physicalAspectRatio)
        {
            var scaling = physicalWidth / (float) VirtualWidth;
            var width = VirtualWidth * scaling;
            var height = VirtualHeight * scaling;
            var borderSize = (int) ((physicalHeight - height) / 2);
            area = new Rectangle(0, borderSize, (int) width, (int) height);
        }
        else
        {
            var scaling = physicalHeight / (float) VirtualHeight;
            var width = VirtualWidth * scaling;
            var height = VirtualHeight * scaling;
            var borderSize = (int) ((physicalWidth - width) / 2);
            area = new Rectangle(borderSize, 0, (int) width, (int) height);
        }
    }

    public void RecreateGraphics()
    {
        Console.WriteLine("GraphicsDevice Virtualization failed");

        var newDevice = (Game.ScreenManager.Game as Game).graphics.GraphicsDevice;
        Game.ScreenManager.ReinitializeCamera(newDevice);
        SpriteLibrary.ClearLibrary();
        (Game.ScreenManager.Game as Game).LoadAllSpriteFonts();
        (Game.ScreenManager.Game as Game).LoadAllEffects();
        (Game.ScreenManager.Game as Game).LoadAllSpritesheets();

        if (Game.GenericTexture.IsDisposed == false)
        {
            Game.GenericTexture.Dispose();
        }

        Game.GenericTexture = new Texture2D(newDevice, 1, 1);
        Game.GenericTexture.SetData(new[] { Color.White });

        Game.ScreenManager.ReinitializeContent(null, null);
    }

    public void BeginCapture()
    {
        // XNA failed to properly reinitialize GraphicsDevice in virtualization. Time to recreate graphics device.
        if (graphicsDevice.IsDisposed)
        {
            RecreateGraphics();
        }

        graphicsDevice.SetRenderTarget(RenderTarget);
    }

    public void EndCapture()
    {
        graphicsDevice.SetRenderTarget(null);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (Game.ScreenManager.CurrentScreen is SkillScreen == false && Game.ScreenManager.CurrentScreen is LineageScreen == false &&
            Game.ScreenManager.CurrentScreen is SkillUnlockScreen == false && Game.ScreenManager.GetLevelScreen() != null &&
            (Game.PlayerStats.Traits.X == TraitType.Vertigo || Game.PlayerStats.Traits.Y == TraitType.Vertigo) && Game.PlayerStats.SpecialItem != SpecialItemType.Glasses)
        {
            spriteBatch.Draw(RenderTarget, area, null, Color.White, 0, Vector2.Zero, SpriteEffects.FlipVertically, 0);
        }
        else
        {
            spriteBatch.Draw(RenderTarget, area, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 0);
        }
    }
}
