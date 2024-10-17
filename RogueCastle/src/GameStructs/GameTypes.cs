namespace RogueCastle.GameStructs;

public static class GameTypes
{
    public enum ArmorType
    {
        None,
        Head,
        Body,
        Ring,
        Foot,
        Hand,
        All,
    }

    public enum DoorType
    {
        Null,
        Open,
        Locked,
        Blocked,
    }

    public enum EnemyDifficulty
    {
        Basic,
        Advanced,
        Expert,
        Miniboss,
    }

    public enum EquipmentType
    {
        None,
        Weapon,
        Armor,
    }

    public enum LevelType
    {
        None,
        Castle,
        Garden,
        Dungeon,
        Tower,
    }

    public enum SkillType
    {
        Strength = 0,
        Health,
        Defense,
    }

    public enum StatType
    {
        Strength = 0,
        Health = 1,
        Endurance = 2,
        EquipLoad = 3,
    }

    public enum WeaponType
    {
        None,
        Dagger,
        Sword,
        Spear,
        Axe,
    }

    // CollisionType represents the type of object you are colliding with.
    public const int COLLISION_TYPE_NULL = 0;
    public const int COLLISION_TYPE_WALL = 1;
    public const int COLLISION_TYPE_PLAYER = 2;
    public const int COLLISION_TYPE_ENEMY = 3;
    public const int COLLISION_TYPE_ENEMY_WALL = 4; // An enemy that you cannot walk through while invincible.
    public const int COLLISION_TYPE_WALL_FOR_PLAYER = 5;
    public const int COLLISION_TYPE_WALL_FOR_ENEMY = 6;
    public const int COLLISION_TYPE_PLAYER_TRIGGER = 7;
    public const int COLLISION_TYPE_ENEMY_TRIGGER = 8;
    public const int COLLISION_TYPE_GLOBAL_TRIGGER = 9;
    public const int COLLISION_TYPE_GLOBAL_DAMAGE_WALL = 10;

    public const int LOGIC_SET_TYPE_NULL = 0;
    public const int LOGIC_SET_TYPE_NON_ATTACK = 1;
    public const int LOGIC_SET_TYPE_ATTACK = 2;
    public const int LOGIC_SET_TYPE_CD = 3;
}
