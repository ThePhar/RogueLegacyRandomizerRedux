﻿namespace RogueCastle.EVs;

public static class LevelEV
{
    public const string GAME_VERSION = "v1.4.1";
    public const string RLRX_VERSION = "v1.0.0-dev";
    
    // The number of levels an enemy needs to be before he goes to the next difficulty.
    public const int ENEMY_LEVEL_DIFFICULTY_MOD = 32; //30;//40;//30;//15;//6;//12;

    // This multiplies the enemy level by a fake amount in order to keep it more in par with how close we want the
    // player to be when engaging.
    public const float ENEMY_LEVEL_FAKE_MULTIPLIER = 2.75f; //3.0f;//2.75f;//2.5f;//2.25f;

    // The number of rooms that have to be generated before the room raises a difficulty level (raising enemy lvl).
    public const int ROOM_LEVEL_MOD = 4; //6;

    // This needs to be kept up-to-date.
    public const byte TOTAL_JOURNAL_ENTRIES = 25; //5;//19;//15;

    // The number of bonus levels an expert enemy gains over the current room level mod.
    public const int ENEMY_EXPERT_LEVEL_MOD = 4; //3;

    // The number of bonus levels the BOSSES GAIN over the current room level mod.
    public const int ENEMY_MINIBOSS_LEVEL_MOD = 7; //8;//5;

    // ADDS DIRECTLY TO THE LEVEL, IGNORING ROOM_LEVEL_MOD.
    public const int LAST_BOSS_MODE1_LEVEL_MOD = 8;
    public const int LAST_BOSS_MODE2_LEVEL_MOD = 10;

    // EVs related to New Game Plus
    //////////////////////////////////////////////////////////////
    
    // These are room levels prior to dividing by ROOM_LEVEL_MOD.
    public const int CASTLE_ROOM_LEVEL_BOOST = 0;
    public const int GARDEN_ROOM_LEVEL_BOOST = 2; //3;//5;//10;
    public const int TOWER_ROOM_LEVEL_BOOST = 4; //6;//10;//20;
    public const int DUNGEON_ROOM_LEVEL_BOOST = 6; //9;//15;//30;

    public const int NEWGAMEPLUS_LEVEL_BASE = 128; //120;//120;//10;
    public const int NEWGAMEPLUS_LEVEL_APPRECIATION = 128; //80; //60;
    public const int NEWGAMEPLUS_MINIBOSS_LEVEL_BASE = 0;
    public const int NEWGAMEPLUS_MINIBOSS_LEVEL_APPRECIATION = 0;

    //////////////////////////////////////////////////////////////

    public const bool LINK_TO_CASTLE_ONLY = true;
    public const byte CASTLE_BOSS_ROOM = BossRoomType.EyeballBossRoom;
    public const byte TOWER_BOSS_ROOM = BossRoomType.FireballBossRoom;
    public const byte DUNGEON_BOSS_ROOM = BossRoomType.BlobBossRoom;
    public const byte GARDEN_BOSS_ROOM = BossRoomType.FairyBossRoom;
    public const byte LAST_BOSS_ROOM = BossRoomType.LastBossRoom;

    // Percent chance the door direction will be open or closed when procedurally generating a room.
    ///////////////////////////////////////////////////////////////
    
    public const int LEVEL_CASTLE_LEFTDOOR = 90; //70;
    public const int LEVEL_CASTLE_RIGHTDOOR = 90; //70;
    public const int LEVEL_CASTLE_TOPDOOR = 90; //70;
    public const int LEVEL_CASTLE_BOTTOMDOOR = 90; //70;

    public const int LEVEL_GARDEN_LEFTDOOR = 70; //80;
    public const int LEVEL_GARDEN_RIGHTDOOR = 100; //80;
    public const int LEVEL_GARDEN_TOPDOOR = 45; //85;//40;
    public const int LEVEL_GARDEN_BOTTOMDOOR = 45; //85;//40;

    public const int LEVEL_TOWER_LEFTDOOR = 45; //65;
    public const int LEVEL_TOWER_RIGHTDOOR = 45; //65;
    public const int LEVEL_TOWER_TOPDOOR = 100; //90;
    public const int LEVEL_TOWER_BOTTOMDOOR = 60; //40;

    public const int LEVEL_DUNGEON_LEFTDOOR = 55; //50;
    public const int LEVEL_DUNGEON_RIGHTDOOR = 55; //50;
    public const int LEVEL_DUNGEON_TOPDOOR = 45; //40;
    public const int LEVEL_DUNGEON_BOTTOMDOOR = 100; //75; //100;

    // The list of enemies that each area will randomly add enemies to.
    ///////////////////////////////////////////////////////////////

    public static readonly byte[] DementiaFlightList =
    [
        EnemyType.FireWizard, EnemyType.IceWizard, EnemyType.Eyeball, EnemyType.Fairy, EnemyType.BouncySpike,
        EnemyType.Fireball, EnemyType.Starburst,
    ];

    public static readonly byte[] DementiaGroundList =
    [
        EnemyType.Skeleton, EnemyType.Knight, EnemyType.Blob, EnemyType.BallAndChain, EnemyType.SwordKnight,
        EnemyType.Zombie, EnemyType.Ninja, EnemyType.Plant, EnemyType.HomingTurret, EnemyType.Horse,
    ];

    public static readonly byte[] CastleEnemyList =
    [
        EnemyType.Skeleton, EnemyType.Knight, EnemyType.FireWizard, EnemyType.IceWizard, EnemyType.Eyeball,
        EnemyType.BouncySpike, EnemyType.SwordKnight, EnemyType.Zombie, EnemyType.Fireball, EnemyType.Portrait,
        EnemyType.Starburst, EnemyType.HomingTurret,
    ];

    public static readonly byte[] GardenEnemyList =
    [
        EnemyType.Skeleton, EnemyType.Blob, EnemyType.BallAndChain, EnemyType.EarthWizard, EnemyType.FireWizard,
        EnemyType.Eyeball, EnemyType.Fairy, EnemyType.ShieldKnight, EnemyType.BouncySpike, EnemyType.Wolf,
        EnemyType.Plant, EnemyType.SkeletonArcher, EnemyType.Starburst, EnemyType.Horse,
    ];

    public static readonly byte[] TowerEnemyList =
    [
        EnemyType.Knight, EnemyType.BallAndChain, EnemyType.IceWizard, EnemyType.Eyeball, EnemyType.Fairy,
        EnemyType.ShieldKnight, EnemyType.BouncySpike, EnemyType.Wolf, EnemyType.Ninja, EnemyType.Plant,
        EnemyType.Fireball, EnemyType.SkeletonArcher, EnemyType.Portrait, EnemyType.Starburst, EnemyType.HomingTurret,
        EnemyType.Mimic,
    ];

    public static readonly byte[] DungeonEnemyList =
    [
        EnemyType.Skeleton, EnemyType.Knight, EnemyType.Blob, EnemyType.BallAndChain, EnemyType.EarthWizard,
        EnemyType.FireWizard, EnemyType.IceWizard, EnemyType.Eyeball, EnemyType.Fairy, EnemyType.BouncySpike,
        EnemyType.SwordKnight, EnemyType.Zombie, EnemyType.Ninja, EnemyType.Plant, EnemyType.Fireball,
        EnemyType.Starburst, EnemyType.HomingTurret, EnemyType.Horse,
    ];

    public static readonly byte[] CastleEnemyDifficultyList = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public static readonly byte[] GardenEnemyDifficultyList = { 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public static readonly byte[] TowerEnemyDifficultyList = { 1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1, 1, 1, 0 };
    public static readonly byte[] DungeonEnemyDifficultyList = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

    // Every implemented enemy type.
    // {
    //     EnemyType.BallAndChain, EnemyType.Blob, EnemyType.BouncySpike, EnemyType.Eagle, EnemyType.EarthWizard,
    //     EnemyType.Energon, EnemyType.Eyeball, EnemyType.Fairy, EnemyType.Fireball, EnemyType.FireWizard,
    //     EnemyType.HomingTurret, EnemyType.Horse, EnemyType.IceWizard, EnemyType.Knight, EnemyType.Ninja,
    //     EnemyType.Plant, EnemyType.ShieldKnight, EnemyType.Skeleton, EnemyType.SkeletonArcher, EnemyType.Spark,
    //     EnemyType.SpikeTrap, EnemyType.SwordKnight, EnemyType.Wolf, EnemyType.Zombie
    // };

    public static readonly string[] CastleAssetSwapList =
    [
        "BreakableBarrel1_Character", "BreakableBarrel2_Character", "CastleAssetKnightStatue_Character",
        "CastleAssetWindow1_Sprite", "CastleAssetWindow2_Sprite", "CastleBGPillar_Character", "CastleAssetWeb1_Sprite",
        "CastleAssetWeb2_Sprite", "CastleAssetBackTorch_Character", "CastleAssetSideTorch_Character",
        "CastleAssetChandelier1_Character", "CastleAssetChandelier2_Character", "CastleAssetCandle1_Character",
        "CastleAssetCandle2_Character", "CastleAssetFireplace_Character", "CastleAssetBookcase_Sprite",
        "CastleAssetBookCase2_Sprite", "CastleAssetBookCase3_Sprite", "CastleAssetUrn1_Character",
        "CastleAssetUrn2_Character", "BreakableChair1_Character", "BreakableChair2_Character",
        "CastleAssetTable1_Character", "CastleAssetTable2_Character", "CastleDoorOpen_Sprite",
        "CastleAssetFrame_Sprite",
    ];

    public static readonly string[] DungeonAssetSwapList =
    [
        "BreakableCrate1_Character", "BreakableBarrel1_Character", "CastleAssetDemonStatue_Character",
        "DungeonSewerGrate1_Sprite", "DungeonSewerGrate2_Sprite", "", "CastleAssetWeb1_Sprite",
        "CastleAssetWeb2_Sprite", "DungeonChainRANDOM2_Character", "", "DungeonHangingCell1_Character",
        "DungeonHangingCell2_Character", "DungeonTorch1_Character", "DungeonTorch2_Character",
        "DungeonMaidenRANDOM3_Character", "DungeonPrison1_Sprite", "DungeonPrison2_Sprite", "DungeonPrison3_Sprite", "",
        "", "DungeonBucket1_Character", "DungeonBucket2_Character", "DungeonTable1_Character",
        "DungeonTable2_Character", "DungeonDoorOpen_Sprite", "",
    ];

    public static readonly string[] TowerAssetSwapList =
    [
        "BreakableCrate1_Character", "BreakableCrate2_Character", "CastleAssetAngelStatue_Character",
        "TowerHoleRANDOM9_Sprite", "TowerHoleRANDOM9_Sprite", "", "TowerLever1_Sprite", "TowerLever2_Sprite",
        "CastleAssetBackTorchUnlit_Character", "CastleAssetSideTorchUnlit_Character", "DungeonChain1_Character",
        "DungeonChain2_Character", "TowerTorch_Character", "TowerPedestal2_Character",
        "CastleAssetFireplaceNoFire_Character", "BrokenBookcase1_Sprite", "BrokenBookcase2_Sprite", "",
        "TowerBust1_Character", "TowerBust2_Character", "TowerChair1_Character", "TowerChair2_Character",
        "TowerTable1_Character", "TowerTable2_Character", "TowerDoorOpen_Sprite", "CastleAssetFrame_Sprite",
    ];

    public static readonly string[] GardenAssetSwapList =
    [
        "GardenUrn1_Character", "GardenUrn2_Character", "CherubStatue_Character", "GardenFloatingRockRANDOM5_Sprite",
        "GardenFloatingRockRANDOM5_Sprite", "GardenPillar_Character", "", "", "GardenFairy_Character", "",
        "GardenVine1_Character", "GardenVine2_Character", "GardenLampPost1_Character", "GardenLampPost2_Character",
        "GardenFountain_Character", "GardenBush1_Sprite", "GardenBush2_Sprite", "", "", "", "GardenMushroom1_Character",
        "GardenMushroom2_Character", "GardenTrunk1_Character", "GardenTrunk2_Character", "GardenDoorOpen_Sprite", "",
    ];
    
    // Debugging EVs
    ////////////////////////////////////////////////////////////////
    
    public static bool ShowEnemyRadii = false;
    public static bool EnableDebugInput = true;
    public static bool UnlockAllAbilities = false;
    public static bool UnlockAllDiaryEntries = false;

    public static GameTypes.LevelType TestRoomLevelType = GameTypes.LevelType.CASTLE;
    public static bool TestRoomReverse = false;
    public static bool RunTestRoom = false;
    public static bool ShowDebugText = false;
    public static bool LoadTitleScreen = true;
    public static bool LoadSplashScreen = false;
    public static bool ShowSaveLoadDebugText = false;

    public static bool DeleteSaveFile = false;
    
    public static bool CloseTestRoomDoors = false;
    public static bool RunTutorial = false;
    public static bool RunDemoVersion = false;
    public static bool DisableSaving = false;
    public static bool RunCrashLogs = false;
    public static bool WeakenBosses = false;
    public static bool EnableOffscreenControl = false;
    public static bool EnableBackupSaving = true;
    public static bool EnableBlitworksSplash = false;

    // This EV overrides all the other Level EVs to create a retail build of the game.
    public static bool CreateRetailVersion = true; 

    // Setting this true also turns vsync off (so that you can get an FPS greater than 60).
    public static bool ShowFps = false; 

    public static bool SaveFrames = false;
    public static int SaveFileRevisionNumber = 1;
}
