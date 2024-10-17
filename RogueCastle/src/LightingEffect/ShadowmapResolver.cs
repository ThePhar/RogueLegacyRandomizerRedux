using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RogueCastle.LightingEffect;

public enum ShadowmapSize
{
    Size128 = 6,
    Size256 = 7,
    Size512 = 8,
    Size1024 = 9,
}

public class ShadowmapResolver
{
    public static Effect Blender;
    private readonly int _baseSize;
    private int _depthBufferSize;
    private RenderTarget2D _distancesRt;
    private RenderTarget2D _distortRt;
    private readonly GraphicsDevice _graphicsDevice;
    private RenderTarget2D _processedShadowsRt;
    private readonly QuadRenderComponent _quadRender;
    private readonly int _reductionChainCount;
    private Effect _reductionEffect;
    private RenderTarget2D[] _reductionRt;
    private Effect _resolveShadowsEffect;
    private RenderTarget2D _shadowMap;
    private RenderTarget2D _shadowsRt;


    /// <summary>
    ///     Creates a new shadowmap resolver
    /// </summary>
    /// <param name="graphicsDevice">The Graphics Device used by the XNA game</param>
    /// <param name="quadRender"></param>
    /// <param name="maxShadowmapSize"></param>
    /// <param name="maxDepthBufferSize"></param>
    public ShadowmapResolver(
        GraphicsDevice graphicsDevice,
        QuadRenderComponent quadRender,
        ShadowmapSize maxShadowmapSize,
        ShadowmapSize maxDepthBufferSize
    )
    {
        _graphicsDevice = graphicsDevice;
        _quadRender = quadRender;

        _reductionChainCount = (int)maxShadowmapSize;
        _baseSize = 2 << _reductionChainCount;
        _depthBufferSize = 2 << (int)maxDepthBufferSize;
    }

    public void LoadContent(ContentManager content)
    {
        _reductionEffect = content.Load<Effect>(@"Shaders\reductionEffect");
        _resolveShadowsEffect = content.Load<Effect>(@"Shaders\resolveShadowsEffect");
        Blender = content.Load<Effect>(@"Shaders\2xMultiBlend");

        var surfaceFormat = SurfaceFormat.Color;

        _distortRt = new RenderTarget2D(_graphicsDevice, _baseSize, _baseSize, false, surfaceFormat, DepthFormat.None, 0,
            RenderTargetUsage.PreserveContents);
        _distancesRt = new RenderTarget2D(_graphicsDevice, _baseSize, _baseSize, false, surfaceFormat, DepthFormat.None, 0,
            RenderTargetUsage.PreserveContents);
        _shadowMap = new RenderTarget2D(_graphicsDevice, 2, _baseSize, false, surfaceFormat, DepthFormat.None, 0,
            RenderTargetUsage.PreserveContents);
        _reductionRt = new RenderTarget2D[_reductionChainCount];
        for (var i = 0; i < _reductionChainCount; i++)
        {
            _reductionRt[i] = new RenderTarget2D(_graphicsDevice, 2 << i, _baseSize, false, surfaceFormat,
                DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
        }


        _shadowsRt = new RenderTarget2D(_graphicsDevice, _baseSize, _baseSize, false, SurfaceFormat.Color, DepthFormat.None,
            0, RenderTargetUsage.PreserveContents);
        _processedShadowsRt = new RenderTarget2D(_graphicsDevice, _baseSize, _baseSize, false, SurfaceFormat.Color,
            DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
    }

    public void ResolveShadows(Texture2D shadowCastersTexture, RenderTarget2D result, Vector2 lightPosition)
    {
        _graphicsDevice.BlendState = BlendState.Opaque;

        ExecuteTechnique(shadowCastersTexture, _distancesRt, "ComputeDistances");
        ExecuteTechnique(_distancesRt, _distortRt, "Distort");
        ApplyHorizontalReduction(_distortRt, _shadowMap);
        ExecuteTechnique(null, _shadowsRt, "DrawShadows", _shadowMap);
        ExecuteTechnique(_shadowsRt, _processedShadowsRt, "BlurHorizontally");
        ExecuteTechnique(_processedShadowsRt, result, "BlurVerticallyAndAttenuate");
    }

    private void ExecuteTechnique(Texture2D source, RenderTarget2D destination, string techniqueName)
    {
        ExecuteTechnique(source, destination, techniqueName, null);
    }

    private void ExecuteTechnique(
        Texture2D source,
        RenderTarget2D destination,
        string techniqueName,
        Texture2D shadowMap
    )
    {
        var renderTargetSize = new Vector2(_baseSize, _baseSize);
        _graphicsDevice.SetRenderTarget(destination);
        _graphicsDevice.Clear(Color.White);
        _resolveShadowsEffect.Parameters["renderTargetSize"].SetValue(renderTargetSize);

        if (source != null)
        {
            _resolveShadowsEffect.Parameters["InputTexture"].SetValue(source);
        }

        if (shadowMap != null)
        {
            _resolveShadowsEffect.Parameters["ShadowMapTexture"].SetValue(shadowMap);
        }

        _resolveShadowsEffect.CurrentTechnique = _resolveShadowsEffect.Techniques[techniqueName];

        foreach (var pass in _resolveShadowsEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            _quadRender.Render(Vector2.One * -1, Vector2.One);
        }

        _graphicsDevice.SetRenderTarget(null);
    }


    private void ApplyHorizontalReduction(RenderTarget2D source, RenderTarget2D destination)
    {
        var step = _reductionChainCount - 1;
        var s = source;
        var d = _reductionRt[step];
        _reductionEffect.CurrentTechnique = _reductionEffect.Techniques["HorizontalReduction"];

        while (step >= 0)
        {
            d = _reductionRt[step];

            _graphicsDevice.SetRenderTarget(d);
            _graphicsDevice.Clear(Color.White);

            _reductionEffect.Parameters["SourceTexture"].SetValue(s);
            var textureDim = new Vector2(1.0f / s.Width, 1.0f / s.Height);
            _reductionEffect.Parameters["TextureDimensions"].SetValue(textureDim);

            foreach (var pass in _reductionEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _quadRender.Render(Vector2.One * -1, new Vector2(1, 1));
            }

            _graphicsDevice.SetRenderTarget(null);

            s = d;
            step--;
        }

        //copy to destination
        _graphicsDevice.SetRenderTarget(destination);
        _reductionEffect.CurrentTechnique = _reductionEffect.Techniques["Copy"];
        _reductionEffect.Parameters["SourceTexture"].SetValue(d);

        foreach (var pass in _reductionEffect.CurrentTechnique.Passes)
        {
            pass.Apply();
            _quadRender.Render(Vector2.One * -1, new Vector2(1, 1));
        }

        _reductionEffect.Parameters["SourceTexture"].SetValue(_reductionRt[_reductionChainCount - 1]);
        _graphicsDevice.SetRenderTarget(null);
    }
}
