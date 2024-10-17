using DS2DEngine;

namespace RogueCastle.LogicActions;

public class FireProjectileLogicAction(ProjectileManager projectileManager, ProjectileData data) : LogicAction
{
    private ProjectileData _data = data.Clone();
    private ProjectileManager _projectileManager = projectileManager;

    public override void Execute()
    {
        if (ParentLogicSet is not { IsActive: true })
        {
            return;
        }

        var enemy = ParentLogicSet.ParentGameObj as EnemyObj;
        var obj = _projectileManager.FireProjectile(_data);

        base.Execute();
    }

    public override object Clone()
    {
        return new FireProjectileLogicAction(_projectileManager, _data);
    }

    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        _projectileManager = null;
        _data = null;
        base.Dispose();
    }
}
