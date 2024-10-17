namespace RogueCastle.GameStructs;

public static class SkillUnlockType
{
    public const byte NONE           = 0;
    public const byte BLACKSMITH     = 1;
    public const byte ENCHANTRESS    = 2;
    public const byte ARCHITECT      = 3;
    public const byte NINJA          = 4;
    public const byte BANKER         = 5;
    public const byte SPELL_SWORD    = 6;
    public const byte LICH           = 7;

    public const byte KNIGHT_UP      = 8;
    public const byte WIZARD_UP      = 9;
    public const byte BARBARIAN_UP   = 10;
    public const byte NINJA_UP       = 11;
    public const byte ASSASSIN_UP    = 12;
    public const byte BANKER_UP      = 13;
    public const byte SPELL_SWORD_UP = 14;
    public const byte LICH_UP        = 15;

    public const byte DRAGON         = 16;
    public const byte TRAITOR        = 17;

    public static string DescriptionID(byte unlockType)
    {
        return unlockType switch
        {
            BLACKSMITH     => "LOC_ID_SKILL_UNLOCK_1",
            ENCHANTRESS    => "LOC_ID_SKILL_UNLOCK_2",
            ARCHITECT      => "LOC_ID_SKILL_UNLOCK_3",
            NINJA          => "LOC_ID_SKILL_UNLOCK_4",
            BANKER         => "LOC_ID_SKILL_UNLOCK_5",
            SPELL_SWORD    => "LOC_ID_SKILL_UNLOCK_6",
            LICH           => "LOC_ID_SKILL_UNLOCK_7",
            KNIGHT_UP      => "LOC_ID_SKILL_UNLOCK_8",
            WIZARD_UP      => "LOC_ID_SKILL_UNLOCK_9",
            BARBARIAN_UP   => "LOC_ID_SKILL_UNLOCK_10",
            NINJA_UP       => "LOC_ID_SKILL_UNLOCK_11",
            ASSASSIN_UP    => "LOC_ID_SKILL_UNLOCK_12",
            BANKER_UP      => "LOC_ID_SKILL_UNLOCK_13",
            SPELL_SWORD_UP => "LOC_ID_SKILL_UNLOCK_14",
            LICH_UP        => "LOC_ID_SKILL_UNLOCK_15",
            DRAGON         => "LOC_ID_SKILL_UNLOCK_16",
            TRAITOR        => "LOC_ID_SKILL_UNLOCK_17",
            _              => "",
        };
    }
}
