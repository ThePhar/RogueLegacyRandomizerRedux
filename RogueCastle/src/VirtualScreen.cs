using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.GameStructs;
using SpriteSystem;

namespace RogueCastle;

internal class VirtualScreen(int virtualWidth, int virtualHeight, GraphicsDevice graphicsDevice) {
    public readonly float VirtualAspectRatio = virtualWidth / (float)virtualHeight;
    public readonly int VirtualHeight = virtualHeight;
    public readonly int VirtualWidth = virtualWidth;

    private Rectangle _area;
    private bool _areaIsDirty = true;
    private GraphicsDevice _graphicsDevice = graphicsDevice;

    public RenderTarget2D RenderTarget { get; private set; } = new(graphicsDevice, virtualWidth, virtualHeight, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

    public void ReinitializeRTs(GraphicsDevice graphicsDevice) {
        _graphicsDevice = graphicsDevice;
        if (RenderTarget.IsDisposed == false) {
            RenderTarget.Dispose();
            RenderTarget = null;
        }

        RenderTarget = new RenderTarget2D(graphicsDevice, VirtualWidth, VirtualHeight, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
    }

    public void PhysicalResolutionChanged() {
        _areaIsDirty = true;
    }

    public void Update() {
        if (!_areaIsDirty) {
            return;
        }

        _areaIsDirty = false;
        var physicalWidth = _graphicsDevice.Viewport.Width;
        var physicalHeight = _graphicsDevice.Viewport.Height;
        var physicalAspectRatio = _graphicsDevice.Viewport.AspectRatio;

        // This 'if' was commented out during Switch development, flibit added it back
        if ((int)(physicalAspectRatio * 10) == (int)(VirtualAspectRatio * 10)) {
            _area = new Rectangle(0, 0, physicalWidth, physicalHeight);
            return;
        }

        if (VirtualAspectRatio > physicalAspectRatio) {
            var scaling = physicalWidth / (float)VirtualWidth;
            var width = VirtualWidth * scaling;
            var height = VirtualHeight * scaling;
            var borderSize = (int)((physicalHeight - height) / 2);
            _area = new Rectangle(0, borderSize, (int)width, (int)height);
        } else {
            var scaling = physicalHeight / (float)VirtualHeight;
            var width = VirtualWidth * scaling;
            var height = VirtualHeight * scaling;
            var borderSize = (int)((physicalWidth - width) / 2);
            _area = new Rectangle(borderSize, 0, (int)width, (int)height);
        }
    }

    public static void RecreateGraphics() {
        Console.WriteLine(@"GraphicsDevice Virtualization failed.");

        var newDevice = Program.Game.Graphics.GraphicsDevice;
        Game.ScreenManager.ReinitializeCamera(newDevice);
        SpriteLibrary.ClearLibrary();
        Program.Game.LoadAllSpriteFonts();
        Program.Game.LoadAllEffects();
        Program.Game.LoadAllSpritesheets();

        if (Game.GenericTexture.IsDisposed == false) {
            Game.GenericTexture.Dispose();
        }

        Game.GenericTexture = new Texture2D(newDevice, 1, 1);
        Game.GenericTexture.SetData([Color.White]);

        Game.ScreenManager.ReinitializeContent(null, null);
    }

    public void BeginCapture() {
        // XNA failed to properly reinitialize GraphicsDevice in virtualization. Time to recreate graphics device.
        if (_graphicsDevice.IsDisposed) {
            RecreateGraphics();
        }

        _graphicsDevice.SetRenderTarget(RenderTarget);
    }

    public void EndCapture() {
        _graphicsDevice.SetRenderTarget(null);
    }

    public void Draw(SpriteBatch spriteBatch) {
        var predicate = Game.ScreenManager.CurrentScreen is SkillScreen == false && Game.ScreenManager.CurrentScreen is LineageScreen == false &&
                        Game.ScreenManager.CurrentScreen is SkillUnlockScreen == false && Game.ScreenManager.GetLevelScreen() != null &&
                        Game.PlayerStats.HasTrait(TraitType.VERTIGO) && Game.PlayerStats.SpecialItem != SpecialItemType.GLASSES;

        spriteBatch.Draw(RenderTarget, _area, null, Color.White, 0, Vector2.Zero, predicate
            ? SpriteEffects.FlipVertically
            : SpriteEffects.None, 0);
    }
}
