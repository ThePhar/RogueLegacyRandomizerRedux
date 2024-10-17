using System;
using DS2DEngine;
using RogueCastle.GameStructs;

namespace RogueCastle;

public static class EnemyBuilder
{
    public static EnemyObj BuildEnemy(
        byte enemyType,
        PlayerObj player,
        PhysicsManager physMgr,
        ProceduralLevelScreen levelToAttachTo,
        GameTypes.EnemyDifficulty difficulty,
        bool doNotInitialize = false
    )
    {
        EnemyObj objToReturn = enemyType switch
        {
            EnemyType.SKELETON        => new EnemyObj_Skeleton(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.KNIGHT          => new EnemyObj_Knight(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.FIREBALL        => new EnemyObj_Fireball(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.FAIRY           => new EnemyObj_Fairy(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.TURRET          => new EnemyObj_Turret(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.WALL            => new EnemyObj_Wall(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.HORSE           => new EnemyObj_Horse(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.ZOMBIE          => new EnemyObj_Zombie(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.WOLF            => new EnemyObj_Wolf(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.BALL_AND_CHAIN  => new EnemyObj_BallAndChain(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.EYEBALL         => new EnemyObj_Eyeball(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.NINJA           => new EnemyObj_Ninja(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.BLOB            => new EnemyObj_Blob(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.SWORD_KNIGHT    => new EnemyObj_SwordKnight(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.EAGLE           => new EnemyObj_Eagle(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.SHIELD_KNIGHT   => new EnemyObj_ShieldKnight(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.FIRE_WIZARD     => new EnemyObj_FireWizard(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.ICE_WIZARD      => new EnemyObj_IceWizard(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.EARTH_WIZARD    => new EnemyObj_EarthWizard(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.BOUNCY_SPIKE    => new EnemyObj_BouncySpike(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.SPIKE_TRAP      => new EnemyObj_SpikeTrap(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.PLANT           => new EnemyObj_Plant(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.ENERGON         => new EnemyObj_Energon(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.SPARK           => new EnemyObj_Spark(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.SKELETON_ARCHER => new EnemyObj_SkeletonArcher(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.CHICKEN         => new EnemyObj_Chicken(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.PLATFORM        => new EnemyObj_Platform(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.HOMING_TURRET   => new EnemyObj_HomingTurret(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.LAST_BOSS       => new EnemyObj_LastBoss(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.DUMMY           => new EnemyObj_Dummy(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.STARBURST       => new EnemyObj_Starburst(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.PORTRAIT        => new EnemyObj_Portrait(player, physMgr, levelToAttachTo, difficulty),
            EnemyType.MIMIC           => new EnemyObj_Mimic(player, physMgr, levelToAttachTo, difficulty),
            _                         => throw new NotImplementedException($"Unsupported enemy type: {enemyType}"),
        };

        if (player == null && doNotInitialize == false)
        {
            objToReturn.Initialize();
        }

        return objToReturn;
    }
}
