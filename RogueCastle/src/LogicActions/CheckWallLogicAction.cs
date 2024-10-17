using DS2DEngine;

namespace RogueCastle.LogicActions;

public class CheckWallLogicAction : LogicAction
{
    private EnemyObj _obj;

    public override void Execute()
    {
        if (ParentLogicSet is not { IsActive: true })
        {
            return;
        }

        _obj = ParentLogicSet.ParentGameObj as EnemyObj;
        SequenceType = Types.Sequence.Serial;
        base.Execute();
    }

    public override object Clone()
    {
        return new CheckWallLogicAction();
    }

    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        _obj?.Dispose();
        _obj = null;

        base.Dispose();
    }
}
