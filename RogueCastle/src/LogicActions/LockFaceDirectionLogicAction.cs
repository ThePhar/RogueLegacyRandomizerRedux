using DS2DEngine;
using Microsoft.Xna.Framework.Graphics;

namespace RogueCastle.LogicActions;

public class LockFaceDirectionLogicAction(bool lockFace, int forceDirection = 0) : LogicAction
{
    public override void Execute()
    {
        if (ParentLogicSet is not { IsActive: true })
        {
            return;
        }

        if (ParentLogicSet.ParentGameObj is CharacterObj obj)
        {
            obj.LockFlip = lockFace;
            obj.Flip = forceDirection switch
            {
                > 0 => SpriteEffects.None,
                < 0 => SpriteEffects.FlipHorizontally,
                _   => obj.Flip,
            };
        }

        base.Execute();
    }

    public override object Clone()
    {
        return new LockFaceDirectionLogicAction(lockFace, forceDirection);
    }
}
