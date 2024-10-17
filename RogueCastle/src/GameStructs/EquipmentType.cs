using System;
using Microsoft.Xna.Framework;

namespace RogueCastle.GameStructs;

public static class EquipmentCategoryType
{
    public const int TOTAL = 5;

    public const int SWORD = 0;
    public const int HELM  = 1;
    public const int CHEST = 2;
    public const int LIMBS = 3;
    public const int CAPE  = 4;

    // English-only string needed by BlacksmithScreen for sprite name generation
    public static string ToStringEN(int equipmentType)
    {
        return equipmentType switch
        {
            SWORD => "Sword",
            CAPE  => "Cape",
            LIMBS => "Limbs",
            HELM  => "Helm",
            CHEST => "Chest",
            _     => "None",
        };
    }

    public static string ToStringID(int equipmentType)
    {
        return equipmentType switch
        {
            SWORD => "LOC_ID_EQUIPMENT_CAT_1",
            CAPE  => "LOC_ID_EQUIPMENT_CAT_2",
            LIMBS => "LOC_ID_EQUIPMENT_CAT_3",
            HELM  => "LOC_ID_EQUIPMENT_CAT_4",
            CHEST => "LOC_ID_EQUIPMENT_CAT_5",
            _     => "LOC_ID_EQUIPMENT_CAT_6",
        };
    }

    public static string ToStringID2(int equipmentType) // Sigh, my horrid architecture caused this.
    {
        return equipmentType switch
        {
            SWORD => "LOC_ID_EQUIPMENT_CAT2_1",
            CAPE  => "LOC_ID_EQUIPMENT_CAT2_2",
            LIMBS => "LOC_ID_EQUIPMENT_CAT2_3",
            HELM  => "LOC_ID_EQUIPMENT_CAT2_4",
            CHEST => "LOC_ID_EQUIPMENT_CAT2_5",
            _     => "LOC_ID_EQUIPMENT_CAT2_6",
        };
    }
}

public static class EquipmentState
{
    public const int NOT_FOUND              = 0;
    public const int FOUND_BUT_NOT_SEEN     = 1;
    public const int FOUND_AND_SEEN         = 2;
    public const int PURCHASED              = 3;
    public const int PURCHASED_AND_EQUIPPED = 4;
}

public static class EquipmentAbilityType
{
    public const int TOTAL = 11;

    public const int DOUBLE_JUMP     = 0;
    public const int DASH            = 1;
    public const int VAMPIRISM       = 2;
    public const int FLIGHT          = 3;
    public const int MANA_GAIN       = 4;
    public const int DAMAGE_RETURN   = 5;
    public const int GOLD_GAIN       = 6;
    public const int MOVEMENT_SPEED  = 7;
    public const int ROOM_LEVEL_UP   = 8;
    public const int ROOM_LEVEL_DOWN = 9;
    public const int MANA_HP_GAIN    = 10;

    // Special rune gained only through locking the castle.
    public const int ARCHITECT_FEE = 20;

    // Special rune gained only through beating the castle at least once.
    public const int NEW_GAME_PLUS_GOLD_BONUS = 21;

    public static string ToStringID(int type)
    {
        return type switch
        {
            DOUBLE_JUMP              => "LOC_ID_EQUIPMENT_ABILITY_1",
            DASH                     => "LOC_ID_EQUIPMENT_ABILITY_2",
            VAMPIRISM                => "LOC_ID_EQUIPMENT_ABILITY_3",
            FLIGHT                   => "LOC_ID_EQUIPMENT_ABILITY_4",
            MANA_GAIN                => "LOC_ID_EQUIPMENT_ABILITY_5",
            MANA_HP_GAIN             => "LOC_ID_EQUIPMENT_ABILITY_6",
            DAMAGE_RETURN            => "LOC_ID_EQUIPMENT_ABILITY_7",
            GOLD_GAIN                => "LOC_ID_EQUIPMENT_ABILITY_8",
            MOVEMENT_SPEED           => "LOC_ID_EQUIPMENT_ABILITY_9",
            ROOM_LEVEL_UP            => "LOC_ID_EQUIPMENT_ABILITY_10",
            ROOM_LEVEL_DOWN          => "LOC_ID_EQUIPMENT_ABILITY_11",
            ARCHITECT_FEE            => "LOC_ID_EQUIPMENT_ABILITY_12",
            NEW_GAME_PLUS_GOLD_BONUS => "LOC_ID_EQUIPMENT_ABILITY_13",
            _                        => "",
        };
    }

    public static string ToStringID2(int type)
    {
        return type switch
        {
            DOUBLE_JUMP              => "LOC_ID_EQUIPMENT_ABILITY2_1",
            DASH                     => "LOC_ID_EQUIPMENT_ABILITY2_2",
            VAMPIRISM                => "LOC_ID_EQUIPMENT_ABILITY2_3",
            FLIGHT                   => "LOC_ID_EQUIPMENT_ABILITY2_4",
            MANA_GAIN                => "LOC_ID_EQUIPMENT_ABILITY2_5",
            MANA_HP_GAIN             => "LOC_ID_EQUIPMENT_ABILITY2_6",
            DAMAGE_RETURN            => "LOC_ID_EQUIPMENT_ABILITY2_7",
            GOLD_GAIN                => "LOC_ID_EQUIPMENT_ABILITY2_8",
            MOVEMENT_SPEED           => "LOC_ID_EQUIPMENT_ABILITY2_9",
            ROOM_LEVEL_UP            => "LOC_ID_EQUIPMENT_ABILITY2_10",
            ROOM_LEVEL_DOWN          => "LOC_ID_EQUIPMENT_ABILITY2_11",
            ARCHITECT_FEE            => "LOC_ID_EQUIPMENT_ABILITY2_12",
            NEW_GAME_PLUS_GOLD_BONUS => "LOC_ID_EQUIPMENT_ABILITY2_13",
            _                        => "",
        };
    }

    public static string DescriptionID(int type)
    {
        return type switch
        {
            DOUBLE_JUMP     => "LOC_ID_EQUIPMENT_DESC_1",
            DASH            => "LOC_ID_EQUIPMENT_DESC_2",
            VAMPIRISM       => "LOC_ID_EQUIPMENT_DESC_3",
            FLIGHT          => "LOC_ID_EQUIPMENT_DESC_4",
            MANA_GAIN       => "LOC_ID_EQUIPMENT_DESC_5",
            MANA_HP_GAIN    => "LOC_ID_EQUIPMENT_DESC_6",
            DAMAGE_RETURN   => "LOC_ID_EQUIPMENT_DESC_7",
            GOLD_GAIN       => "LOC_ID_EQUIPMENT_DESC_8",
            MOVEMENT_SPEED  => "LOC_ID_EQUIPMENT_DESC_9",
            ROOM_LEVEL_UP   => "LOC_ID_EQUIPMENT_DESC_10",
            ROOM_LEVEL_DOWN => "LOC_ID_EQUIPMENT_DESC_11",
            _               => "",
        };
    }

    // Returns localized string. This is safe because calling function LoadBackCardStats()
    // is refreshed on language change.
    public static string ShortDescription(int type, float amount)
    {
        return type switch
        {
            DOUBLE_JUMP => amount > 1
                ? "LOC_ID_EQUIPMENT_SHORT_1_NEW_A".GetResourceString()
                : "LOC_ID_EQUIPMENT_SHORT_1_NEW_B".FormatResourceString(amount),
            DASH => amount > 1
                ? "LOC_ID_EQUIPMENT_SHORT_2_NEW_A".GetResourceString()
                : "LOC_ID_EQUIPMENT_SHORT_2_NEW_B".FormatResourceString(amount),
            FLIGHT => amount > 1
                ? "LOC_ID_EQUIPMENT_SHORT_4_NEW_A".GetResourceString()
                : "LOC_ID_EQUIPMENT_SHORT_4_NEW_B".FormatResourceString(amount),
            ROOM_LEVEL_DOWN => amount > 1
                ? "LOC_ID_EQUIPMENT_SHORT_13_NEW_A".GetResourceString()
                : "LOC_ID_EQUIPMENT_SHORT_13_NEW_B".FormatResourceString(amount),

            MANA_GAIN                => "LOC_ID_EQUIPMENT_SHORT_5_NEW".FormatResourceString(amount),
            MANA_HP_GAIN             => "LOC_ID_EQUIPMENT_SHORT_6".GetResourceString(),
            DAMAGE_RETURN            => "LOC_ID_EQUIPMENT_SHORT_7_NEW".FormatResourceString(amount),
            GOLD_GAIN                => "LOC_ID_EQUIPMENT_SHORT_8_NEW".FormatResourceString(amount),
            MOVEMENT_SPEED           => "LOC_ID_EQUIPMENT_SHORT_9_NEW".FormatResourceString(amount),
            ARCHITECT_FEE            => "LOC_ID_EQUIPMENT_SHORT_10".GetResourceString(),
            NEW_GAME_PLUS_GOLD_BONUS => "LOC_ID_EQUIPMENT_SHORT_11_NEW".FormatResourceString(amount),
            ROOM_LEVEL_UP            => "LOC_ID_EQUIPMENT_SHORT_12_NEW".FormatResourceString(amount),
            VAMPIRISM                => "LOC_ID_EQUIPMENT_SHORT_3_NEW".FormatResourceString(amount),
            _                        => "",
        };
    }

    // Returns localized string. This is safe because calling function UpdateEquipmentDataText()
    // is refreshed on language change.
    public static string Instructions(int type)
    {
        return type switch
        {
            DOUBLE_JUMP     => "LOC_ID_EQUIPMENT_INST_1_NEW".GetResourceString(),
            DASH            => "LOC_ID_EQUIPMENT_INST_2_NEW".GetResourceString(),
            VAMPIRISM       => "LOC_ID_EQUIPMENT_INST_3".GetResourceString(),
            FLIGHT          => "LOC_ID_EQUIPMENT_INST_4_NEW".GetResourceString(),
            MANA_GAIN       => "LOC_ID_EQUIPMENT_INST_5".GetResourceString(),
            MANA_HP_GAIN    => "LOC_ID_EQUIPMENT_INST_6".GetResourceString(),
            DAMAGE_RETURN   => "LOC_ID_EQUIPMENT_INST_7".GetResourceString(),
            GOLD_GAIN       => "LOC_ID_EQUIPMENT_INST_8".GetResourceString(),
            MOVEMENT_SPEED  => "LOC_ID_EQUIPMENT_INST_9".GetResourceString(),
            ROOM_LEVEL_UP   => "LOC_ID_EQUIPMENT_INST_10".GetResourceString(),
            ROOM_LEVEL_DOWN => "LOC_ID_EQUIPMENT_INST_11".GetResourceString(),
            _               => "",
        };
    }

    public static string Icon(int type)
    {
        return type switch
        {
            DOUBLE_JUMP     => "EnchantressUI_DoubleJumpIcon_Sprite",
            DASH            => "EnchantressUI_DashIcon_Sprite",
            VAMPIRISM       => "EnchantressUI_VampirismIcon_Sprite",
            FLIGHT          => "EnchantressUI_FlightIcon_Sprite",
            MANA_GAIN       => "EnchantressUI_ManaGainIcon_Sprite",
            MANA_HP_GAIN    => "EnchantressUI_BalanceIcon_Sprite",
            DAMAGE_RETURN   => "EnchantressUI_DamageReturnIcon_Sprite",
            GOLD_GAIN       => "Icon_Gold_Gain_Up_Sprite",
            MOVEMENT_SPEED  => "EnchantressUI_SpeedUpIcon_Sprite",
            ROOM_LEVEL_UP   => "EnchantressUI_CurseIcon_Sprite",
            ROOM_LEVEL_DOWN => "EnchantressUI_BlessingIcon_Sprite",
            _               => "",
        };
    }
}

public static class EquipmentBaseType
{
    public const int TOTAL = 15;

    public const int BRONZE   = 0;
    public const int SILVER   = 1;
    public const int GOLD     = 2;
    public const int IMPERIAL = 3;
    public const int ROYAL    = 4;
    public const int KNIGHT   = 5;
    public const int EARTHEN  = 6;
    public const int SKY      = 7;
    public const int DRAGON   = 8;
    public const int ETERNAL  = 9;
    public const int BLOOD    = 10;
    public const int AMETHYST = 11;
    public const int SPIKE    = 12;
    public const int HOLY     = 13;
    public const int DARK     = 14;

    public static string ToStringID(int equipmentBaseType)
    {
        return equipmentBaseType switch
        {
            BRONZE   => "LOC_ID_EQUIPMENT_BASE_1",
            SILVER   => "LOC_ID_EQUIPMENT_BASE_2",
            GOLD     => "LOC_ID_EQUIPMENT_BASE_3",
            IMPERIAL => "LOC_ID_EQUIPMENT_BASE_4",
            ROYAL    => "LOC_ID_EQUIPMENT_BASE_5",
            KNIGHT   => "LOC_ID_EQUIPMENT_BASE_6",
            EARTHEN  => "LOC_ID_EQUIPMENT_BASE_7",
            SKY      => "LOC_ID_EQUIPMENT_BASE_8",
            DRAGON   => "LOC_ID_EQUIPMENT_BASE_9",
            ETERNAL  => "LOC_ID_EQUIPMENT_BASE_10",
            AMETHYST => "LOC_ID_EQUIPMENT_BASE_11",
            BLOOD    => "LOC_ID_EQUIPMENT_BASE_12",
            SPIKE    => "LOC_ID_EQUIPMENT_BASE_13",
            HOLY     => "LOC_ID_EQUIPMENT_BASE_14",
            DARK     => "LOC_ID_EQUIPMENT_BASE_15",
            _        => "",
        };
    }
}

public class EquipmentData
{
    public int BonusArmor;
    public int BonusDamage;
    public int BonusHealth;
    public int BonusMagic;
    public int BonusMana;
    public byte ChestColourRequirement = 0;
    public int Cost = 9999;
    public Color FirstColour = Color.White;
    public byte LevelRequirement = 0;
    public Vector2[] SecondaryAttribute;
    public Color SecondColour = Color.White;
    public int Weight;

    public void Dispose()
    {
        if (SecondaryAttribute != null)
        {
            Array.Clear(SecondaryAttribute, 0, SecondaryAttribute.Length);
        }

        SecondaryAttribute = null;
    }
}

public static class EquipmentSecondaryDataType
{
    public const int TOTAL = 16;

    public const int NONE               = 0;
    public const int CRIT_CHANCE        = 1;
    public const int CRIT_DAMAGE        = 2;
    public const int GOLD_BONUS         = 3;
    public const int DAMAGE_RETURN      = 4;

    public const int XP_BONUS           = 5;

    // Anything above this line will be displayed as a percent.
    public const int AIR_ATTACK         = 6;
    public const int VAMPIRISM          = 7;
    public const int MANA_DRAIN         = 8;
    public const int DOUBLE_JUMP        = 9;
    public const int MOVE_SPEED         = 10;
    public const int AIR_DASH           = 11;
    public const int BLOCK              = 12;
    public const int FLOAT              = 13;
    public const int ATTACK_PROJECTILES = 14;
    public const int FLIGHT             = 15;

    public static string ToStringID(int equipmentSecondaryDataType)
    {
        return equipmentSecondaryDataType switch
        {
            CRIT_CHANCE        => "LOC_ID_EQUIPMENT_SEC_1",
            CRIT_DAMAGE        => "LOC_ID_EQUIPMENT_SEC_2",
            VAMPIRISM          => "LOC_ID_EQUIPMENT_SEC_3",
            GOLD_BONUS         => "LOC_ID_EQUIPMENT_SEC_4",
            MANA_DRAIN         => "LOC_ID_EQUIPMENT_SEC_5",
            XP_BONUS           => "LOC_ID_EQUIPMENT_SEC_6",
            AIR_ATTACK         => "LOC_ID_EQUIPMENT_SEC_7",
            DOUBLE_JUMP        => "LOC_ID_EQUIPMENT_SEC_8",
            DAMAGE_RETURN      => "LOC_ID_EQUIPMENT_SEC_9",
            AIR_DASH           => "LOC_ID_EQUIPMENT_SEC_10",
            BLOCK              => "LOC_ID_EQUIPMENT_SEC_11",
            FLOAT              => "LOC_ID_EQUIPMENT_SEC_12",
            ATTACK_PROJECTILES => "LOC_ID_EQUIPMENT_SEC_13",
            FLIGHT             => "LOC_ID_EQUIPMENT_SEC_14",
            MOVE_SPEED         => "LOC_ID_EQUIPMENT_SEC_15",
            _                  => "LOC_ID_EQUIPMENT_SEC_16",
        };
    }
}
