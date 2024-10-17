using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueCastle.LogicActions;

public class GroundCheckLogicAction : LogicAction
{
    private CharacterObj _obj;

    public override void Execute()
    {
        if (ParentLogicSet is not { IsActive: true })
        {
            return;
        }

        _obj = ParentLogicSet.ParentGameObj as CharacterObj;
        SequenceType = Types.Sequence.Serial;
        base.Execute();
    }

    public override void Update(GameTime gameTime)
    {
        ExecuteNext();
        base.Update(gameTime);
    }

    public override void ExecuteNext()
    {
        if (_obj.IsTouchingGround)
        {
            base.ExecuteNext();
        }
    }

    public override object Clone()
    {
        return new GroundCheckLogicAction();
    }

    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        _obj = null;
        base.Dispose();
    }
}
