//-----------------------------------------------------------------------------
// Copyright (c) 2008-2011 dhpoware. All Rights Reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
// OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

using System;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.EnvironmentVariables;

namespace RogueCastle;

/// <summary>
///     The RADIUS constant in the effect file must match the radius value in the GaussianBlur class. The effect
///     file's weights global variable corresponds to the GaussianBlur class' kernel field. The effect file's offsets
///     global variable corresponds to the GaussianBlur class' offsetsHoriz and offsetsVert fields.
/// </summary>
public class GaussianBlur {
    private readonly Effect _effect;
    private readonly EffectParameter _offsetParameters;
    private readonly RenderTarget2D _renderHolder;
    private readonly RenderTarget2D _renderHolder2;
    private float _amount;
    private bool _invertMask;
    private int _radius;

    /// <summary>
    ///     Default constructor for the GaussianBlur class. This constructor should be called if you don't want the
    ///     GaussianBlur class to use its GaussianBlur.fx effect file to perform the two pass Gaussian blur operation.
    /// </summary>
    public GaussianBlur() { }

    /// <summary>
    ///     This overloaded constructor instructs the GaussianBlur class to load and use its GaussianBlur.fx effect file
    ///     that implements the two pass Gaussian blur operation on the GPU. The effect file must be already bound to the asset
    ///     name: 'Effects\GaussianBlur' or 'GaussianBlur'.
    /// </summary>
    public GaussianBlur(Game game, int screenWidth, int screenHeight) {
        if (_renderHolder is { IsDisposed: false }) {
            _renderHolder.Dispose();
        }

        if (_renderHolder2 is { IsDisposed: false }) {
            _renderHolder2.Dispose();
        }

        if (LevelEV.SaveFrames) {
            _renderHolder = new RenderTarget2D(game.GraphicsDevice, screenWidth / 2, screenHeight / 2, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
            _renderHolder2 = new RenderTarget2D(game.GraphicsDevice, screenWidth / 2, screenHeight / 2, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
        } else {
            _renderHolder = new RenderTarget2D(game.GraphicsDevice, screenWidth, screenHeight, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
            _renderHolder2 = new RenderTarget2D(game.GraphicsDevice, screenWidth, screenHeight, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
        }

        _effect = game.Content.Load<Effect>(@"Shaders\GaussianBlurMask");
        _offsetParameters = _effect.Parameters["offsets"];
    }

    /// <summary>Returns the radius of the Gaussian blur filter kernel in pixels.</summary>
    public int Radius {
        get => _radius;
        set {
            _radius = value;
            ComputeKernel();
        }
    }

    /// <summary>
    ///     Returns the blur amount. This value is used to calculate the Gaussian blur filter kernel's sigma value. Good
    ///     values for this property are 2 and 3. 2 will give a more blurred result whilst 3 will give a less blurred result
    ///     with sharper details.
    /// </summary>
    public float Amount {
        get => _amount;
        set {
            _amount = value;
            ComputeKernel();
        }
    }

    /// <summary>Returns the Gaussian blur filter's standard deviation.</summary>
    public float Sigma { get; private set; }

    /// <summary>
    ///     Returns the Gaussian blur filter kernel matrix. Note that the kernel returned is for a 1D Gaussian blur filter
    ///     kernel matrix intended to be used in a two pass Gaussian blur operation.
    /// </summary>
    public float[] Kernel { get; private set; }

    /// <summary>Returns the texture offsets used for the horizontal Gaussian blur pass.</summary>
    public Vector2[] TextureOffsetsX { get; private set; }

    /// <summary>Returns the texture offsets used for the vertical Gaussian blur pass.</summary>
    public Vector2[] TextureOffsetsY { get; private set; }

    public bool InvertMask {
        get => _invertMask;
        set {
            _invertMask = value;
            _effect.Parameters["invert"].SetValue(_invertMask);
        }
    }

    /// <summary>
    ///     Calculates the Gaussian blur filter kernel. This implementation is ported from the original Java code
    ///     appearing in chapter 16 of "Filthy Rich Clients: Developing Animated and Graphical Effects for Desktop Java".
    /// </summary>
    public void ComputeKernel() {
        Kernel = null;
        Kernel = new float[(_radius * 2) + 1];
        Sigma = _radius / _amount;

        // flibit added this to avoid sending NaN to the shader
        if (_radius == 0) {
            return;
        }

        var twoSigmaSquare = 2.0f * Sigma * Sigma;
        var sigmaRoot = (float)Math.Sqrt(twoSigmaSquare * Math.PI);
        var total = 0.0f;

        for (var i = -_radius; i <= _radius; ++i) {
            float distance = i * i;
            var index = i + _radius;
            Kernel[index] = (float)Math.Exp(-distance / twoSigmaSquare) / sigmaRoot;
            total += Kernel[index];
        }

        for (var i = 0; i < Kernel.Length; ++i) {
            Kernel[i] /= total;
        }

        _effect.Parameters["weights"].SetValue(Kernel);
    }

    /// <summary>
    ///     Calculates the texture coordinate offsets corresponding to the calculated Gaussian blur filter kernel. Each of
    ///     these offset values are added to the current pixel's texture coordinates in order to obtain the neighboring texture
    ///     coordinates that are affected by the Gaussian blur filter kernel. This implementation has been adapted from chapter
    ///     17 of "Filthy Rich Clients: Developing Animated and Graphical Effects for Desktop Java".
    /// </summary>
    public void ComputeOffsets() {
        TextureOffsetsX = null;
        TextureOffsetsX = new Vector2[(_radius * 2) + 1];

        TextureOffsetsY = null;
        TextureOffsetsY = new Vector2[(_radius * 2) + 1];

        var xOffset = 1.0f / _renderHolder.Width;
        var yOffset = 1.0f / _renderHolder.Height;

        for (var i = -_radius; i <= _radius; ++i) {
            var index = i + _radius;
            TextureOffsetsX[index] = new Vector2(i * xOffset, 0.0f);
            TextureOffsetsY[index] = new Vector2(0.0f, i * yOffset);
        }
    }

    /// <summary>
    ///     Performs the Gaussian blur operation on the source texture image. The Gaussian blur is performed in two
    ///     passes: a horizontal blur pass followed by a vertical blur pass. The output from the first pass is rendered to
    ///     renderTarget1. The output from the second pass is rendered to renderTarget2. The dimensions of the blurred texture
    ///     is therefore equal to the dimensions of renderTarget2.
    /// </summary>
    /// <returns>The resulting Gaussian blurred image.</returns>
    public void Draw(RenderTarget2D srcTexture, Camera2D camera, RenderTarget2D mask = null) {
        if (_effect == null) {
            throw new InvalidOperationException("GaussianBlur.fx effect not loaded.");
        }

        // Perform horizontal Gaussian blur.
        camera.GraphicsDevice.SetRenderTarget(_renderHolder);

        _offsetParameters.SetValue(TextureOffsetsX);

        if (mask != null) {
            camera.GraphicsDevice.Textures[1] = mask;
            camera.GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
        }

        camera.Begin(0, BlendState.Opaque, SamplerState.LinearClamp, null, null, _effect);
        if (LevelEV.SaveFrames) {
            camera.Draw(srcTexture, Vector2.Zero, null, Color.White, 0, Vector2.Zero, new Vector2(0.5f, 0.5f), SpriteEffects.None, 1);
        } else {
            camera.Draw(srcTexture, Vector2.Zero, Color.White);
        }

        camera.End();

        if (LevelEV.SaveFrames) {
            // Perform vertical Gaussian blur.
            camera.GraphicsDevice.SetRenderTarget(_renderHolder2);

            _offsetParameters.SetValue(TextureOffsetsY);

            if (mask != null) {
                camera.GraphicsDevice.Textures[1] = mask;
            }

            camera.Begin(0, BlendState.Opaque, null, null, null, _effect);
            camera.Draw(_renderHolder, Vector2.Zero, Color.White);
            camera.End();

            camera.GraphicsDevice.SetRenderTarget(srcTexture);
            camera.Begin(0, BlendState.Opaque, SamplerState.PointClamp, null, null);
            camera.Draw(_renderHolder2, Vector2.Zero, null, Color.White, 0, Vector2.Zero, new Vector2(2, 2), SpriteEffects.None, 1);
        } else {
            // Perform vertical Gaussian blur.
            camera.GraphicsDevice.SetRenderTarget(srcTexture);

            _offsetParameters.SetValue(TextureOffsetsY);

            if (mask != null) {
                camera.GraphicsDevice.Textures[1] = mask;
            }

            camera.Begin(0, BlendState.Opaque, null, null, null, _effect);
            camera.Draw(_renderHolder, Vector2.Zero, Color.White);
        }

        camera.End();
    }
}
