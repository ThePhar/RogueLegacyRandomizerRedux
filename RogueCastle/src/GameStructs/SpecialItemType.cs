namespace RogueCastle.GameStructs;

public static class SpecialItemType
{
    public const byte TOTAL = 7;

    public const byte NONE            = 0;
    public const byte FREE_ENTRANCE  = 1; // Done
    public const byte LOSE_COINS     = 2; // Done
    public const byte REVIVE         = 3; // Done
    public const byte SPIKE_IMMUNITY = 4; // Done
    public const byte GOLD_PER_KILL  = 5; // Done
    public const byte COMPASS        = 6; // Done

    // Don't include glasses on the list because it needs to be hard coded in.
    public const byte GLASSES         = 8; // Done

    public const byte EYEBALL_TOKEN   = 9;
    public const byte SKULL_TOKEN     = 10;
    public const byte FIREBALL_TOKEN  = 11;
    public const byte BLOB_TOKEN      = 12;
    public const byte LAST_BOSS_TOKEN = 13;

    public static string ToStringID(byte itemType)
    {
        return itemType switch
        {
            REVIVE          => "LOC_ID_SPECIAL_ITEM_TYPE_1",
            SPIKE_IMMUNITY  => "LOC_ID_SPECIAL_ITEM_TYPE_2",
            LOSE_COINS      => "LOC_ID_SPECIAL_ITEM_TYPE_3",
            FREE_ENTRANCE   => "LOC_ID_SPECIAL_ITEM_TYPE_4",
            COMPASS         => "LOC_ID_SPECIAL_ITEM_TYPE_5",
            GOLD_PER_KILL   => "LOC_ID_SPECIAL_ITEM_TYPE_6",
            GLASSES         => "LOC_ID_SPECIAL_ITEM_TYPE_7",
            EYEBALL_TOKEN   => "LOC_ID_SPECIAL_ITEM_TYPE_8",
            SKULL_TOKEN     => "LOC_ID_SPECIAL_ITEM_TYPE_9",
            FIREBALL_TOKEN  => "LOC_ID_SPECIAL_ITEM_TYPE_10",
            BLOB_TOKEN      => "LOC_ID_SPECIAL_ITEM_TYPE_11",
            LAST_BOSS_TOKEN => "LOC_ID_SPECIAL_ITEM_TYPE_12",
            _               => "",
        };
    }

    public static string SpriteName(byte itemType)
    {
        return itemType switch
        {
            REVIVE          => "BonusRoomRingIcon_Sprite",
            SPIKE_IMMUNITY  => "BonusRoomBootsIcon_Sprite",
            LOSE_COINS      => "BonusRoomHedgehogIcon_Sprite",
            FREE_ENTRANCE   => "BonusRoomObolIcon_Sprite",
            COMPASS         => "BonusRoomCompassIcon_Sprite",
            GOLD_PER_KILL   => "BonusRoomBlessingIcon_Sprite",
            GLASSES         => "BonusRoomGlassesIcon_Sprite",
            EYEBALL_TOKEN   => "ChallengeIcon_Eyeball_Sprite",
            SKULL_TOKEN     => "ChallengeIcon_Skull_Sprite",
            FIREBALL_TOKEN  => "ChallengeIcon_Fireball_Sprite",
            BLOB_TOKEN      => "ChallengeIcon_Blob_Sprite",
            LAST_BOSS_TOKEN => "ChallengeIcon_LastBoss_Sprite",
            _               => "",
        };
    }
}
