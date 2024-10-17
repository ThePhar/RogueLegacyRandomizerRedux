using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RogueCastle.LightingEffect;

public class QuadRenderComponent(Game game) : DrawableGameComponent(game)
{
    private short[] _ib;
    private VertexPositionTexture[] _verts;

    protected override void LoadContent()
    {
        _verts =
        [
            new VertexPositionTexture(
                new Vector3(0, 0, 0),
                new Vector2(1, 1)),
            new VertexPositionTexture(
                new Vector3(0, 0, 0),
                new Vector2(0, 1)),
            new VertexPositionTexture(
                new Vector3(0, 0, 0),
                new Vector2(0, 0)),
            new VertexPositionTexture(
                new Vector3(0, 0, 0),
                new Vector2(1, 0)),
        ];

        _ib = [0, 1, 2, 2, 3, 0];

        base.LoadContent();
    }

    protected override void UnloadContent()
    {
        base.UnloadContent();
    }

    public void Render(Vector2 v1, Vector2 v2)
    {
        _verts[0].Position.X = v2.X;
        _verts[0].Position.Y = v1.Y;

        _verts[1].Position.X = v1.X;
        _verts[1].Position.Y = v1.Y;

        _verts[2].Position.X = v1.X;
        _verts[2].Position.Y = v2.Y;

        _verts[3].Position.X = v2.X;
        _verts[3].Position.Y = v2.Y;

        GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, _verts, 0, 4, _ib, 0, 2);
    }
}
