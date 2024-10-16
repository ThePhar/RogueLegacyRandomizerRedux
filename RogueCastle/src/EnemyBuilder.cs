using System;
using DS2DEngine;
using RogueCastle.Enumerations;

namespace RogueCastle;

public static class EnemyBuilder
{
    public static EnemyObj BuildEnemy(
        EnemyType enemyType,
        PlayerObj player,
        PhysicsManager physMgr,
        ProceduralLevelScreen levelToAttachTo,
        GameTypes.EnemyDifficulty difficulty,
        bool doNotInitialize = false
    )
    {
        EnemyObj objToReturn = enemyType switch
        {
            EnemyType.Skeleton       => new EnemyObj_Skeleton(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.Knight         => new EnemyObj_Knight(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.Fireball       => new EnemyObj_Fireball(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.Fairy          => new EnemyObj_Fairy(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.Turret         => new EnemyObj_Turret(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.Wall           => new EnemyObj_Wall(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.Horse          => new EnemyObj_Horse(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.Zombie         => new EnemyObj_Zombie(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.Wolf           => new EnemyObj_Wolf(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.BallAndChain   => new EnemyObj_BallAndChain(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.Eyeball        => new EnemyObj_Eyeball(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.Ninja          => new EnemyObj_Ninja(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.Blob           => new EnemyObj_Blob(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.SwordKnight    => new EnemyObj_SwordKnight(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.Eagle          => new EnemyObj_Eagle(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.ShieldKnight   => new EnemyObj_ShieldKnight(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.FireWizard     => new EnemyObj_FireWizard(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.IceWizard      => new EnemyObj_IceWizard(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.EarthWizard    => new EnemyObj_EarthWizard(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.BouncySpike    => new EnemyObj_BouncySpike(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.SpikeTrap      => new EnemyObj_SpikeTrap(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.Plant          => new EnemyObj_Plant(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.Energon        => new EnemyObj_Energon(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.Spark          => new EnemyObj_Spark(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.SkeletonArcher => new EnemyObj_SkeletonArcher(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.Chicken        => new EnemyObj_Chicken(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.Platform       => new EnemyObj_Platform(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.HomingTurret   => new EnemyObj_HomingTurret(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.LastBoss       => new EnemyObj_LastBoss(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.Dummy          => new EnemyObj_Dummy(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.Starburst      => new EnemyObj_Starburst(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.Portrait       => new EnemyObj_Portrait(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.Mimic          => new EnemyObj_Mimic(player, physMgr, levelToAttachTo, difficulty),
            _                        => throw new NotImplementedException($"Unsupported enemy type: {enemyType}"),
        };

        if (player == null && doNotInitialize == false)
        {
            objToReturn.Initialize();
        }

        return objToReturn;
    }
}
