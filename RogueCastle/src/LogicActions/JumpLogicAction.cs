using DS2DEngine;

namespace RogueCastle.LogicActions;

public class JumpLogicAction(float overriddenHeight = 0) : LogicAction
{
    public override void Execute()
    {
        if (ParentLogicSet is not { IsActive: true })
        {
            return;
        }

        if (ParentLogicSet.ParentGameObj is CharacterObj character)
        {
            if (overriddenHeight > 0)
            {
                character.AccelerationY = -overriddenHeight;
            }
            else
            {
                character.AccelerationY = -character.JumpHeight;
            }
        }

        base.Execute();
    }

    public override object Clone()
    {
        return new JumpLogicAction(overriddenHeight);
    }
}
