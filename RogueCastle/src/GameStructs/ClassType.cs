using System.Collections.Generic;
using DS2DEngine;
using RogueCastle.EnvironmentVariables;

namespace RogueCastle.GameStructs;

public static class ClassType
{
    public const byte TOTAL_UNIQUES = 8;
    public const byte TOTAL = 16;

    // Starter classes.
    public const byte KNIGHT       = 0;
    public const byte WIZARD       = 1;
    public const byte BARBARIAN    = 2;
    public const byte ASSASSIN     = 3;

    // Requires skill to be purchased.
    public const byte NINJA        = 4;
    public const byte BANKER       = 5;
    public const byte SPELL_SWORD  = 6;
    public const byte LICH         = 7;

    // Upgraded classes.
    public const byte KNIGHT2      = 8;
    public const byte WIZARD2      = 9;
    public const byte BARBARIAN2   = 10;
    public const byte ASSASSIN2    = 11;
    public const byte NINJA2       = 12;
    public const byte BANKER2      = 13;
    public const byte SPELL_SWORD2 = 14;
    public const byte LICH2        = 15;

    // Special classes with no upgrade variant.
    public const byte DRAGON       = 16;
    public const byte TRAITOR      = 17;

    public static string ToStringID(byte classType, bool isFemale)
    {
        return classType switch
        {
            KNIGHT       => !isFemale ? "LOC_ID_CLASS_NAME_1_MALE" : "LOC_ID_CLASS_NAME_1_FEMALE",
            KNIGHT2      => !isFemale ? "LOC_ID_CLASS_NAME_2_MALE" : "LOC_ID_CLASS_NAME_2_FEMALE",
            ASSASSIN     => !isFemale ? "LOC_ID_CLASS_NAME_3_MALE" : "LOC_ID_CLASS_NAME_3_FEMALE",
            ASSASSIN2    => !isFemale ? "LOC_ID_CLASS_NAME_4_MALE" : "LOC_ID_CLASS_NAME_4_FEMALE",
            BANKER       => !isFemale ? "LOC_ID_CLASS_NAME_5_MALE" : "LOC_ID_CLASS_NAME_5_FEMALE",
            BANKER2      => !isFemale ? "LOC_ID_CLASS_NAME_6_MALE" : "LOC_ID_CLASS_NAME_6_FEMALE",
            WIZARD       => !isFemale ? "LOC_ID_CLASS_NAME_7_MALE" : "LOC_ID_CLASS_NAME_7_FEMALE",
            WIZARD2      => !isFemale ? "LOC_ID_CLASS_NAME_8_MALE" : "LOC_ID_CLASS_NAME_8_FEMALE",
            BARBARIAN    => !isFemale ? "LOC_ID_CLASS_NAME_9_MALE" : "LOC_ID_CLASS_NAME_9_FEMALE",
            BARBARIAN2   => !isFemale ? "LOC_ID_CLASS_NAME_10_MALE" : "LOC_ID_CLASS_NAME_10_FEMALE",
            NINJA        => "LOC_ID_CLASS_NAME_11",
            NINJA2       => "LOC_ID_CLASS_NAME_12",
            SPELL_SWORD  => !isFemale ? "LOC_ID_CLASS_NAME_13_MALE" : "LOC_ID_CLASS_NAME_13_FEMALE",
            SPELL_SWORD2 => !isFemale ? "LOC_ID_CLASS_NAME_14_MALE" : "LOC_ID_CLASS_NAME_14_FEMALE",
            LICH         => "LOC_ID_CLASS_NAME_15",
            LICH2        => !isFemale ? "LOC_ID_CLASS_NAME_16_MALE" : "LOC_ID_CLASS_NAME_16_FEMALE",
            DRAGON       => !isFemale ? "LOC_ID_CLASS_NAME_17_MALE" : "LOC_ID_CLASS_NAME_17_FEMALE",
            TRAITOR      => !isFemale ? "LOC_ID_CLASS_NAME_18_MALE" : "LOC_ID_CLASS_NAME_18_FEMALE",
            _            => "",
        };
    }

    public static string DescriptionID(byte classType)
    {
        return classType switch
        {
            KNIGHT       => "LOC_ID_CLASS_DESC_1",
            KNIGHT2      => "LOC_ID_CLASS_DESC_2",
            ASSASSIN     => "LOC_ID_CLASS_DESC_3",
            ASSASSIN2    => "LOC_ID_CLASS_DESC_4",
            BANKER       => "LOC_ID_CLASS_DESC_5",
            BANKER2      => "LOC_ID_CLASS_DESC_6",
            WIZARD       => "LOC_ID_CLASS_DESC_7",
            WIZARD2      => "LOC_ID_CLASS_DESC_8",
            BARBARIAN    => "LOC_ID_CLASS_DESC_9",
            BARBARIAN2   => "LOC_ID_CLASS_DESC_10",
            NINJA        => "LOC_ID_CLASS_DESC_11",
            NINJA2       => "LOC_ID_CLASS_DESC_12",
            SPELL_SWORD  => "LOC_ID_CLASS_DESC_13",
            SPELL_SWORD2 => "LOC_ID_CLASS_DESC_14",
            LICH         => "LOC_ID_CLASS_DESC_15",
            LICH2        => "LOC_ID_CLASS_DESC_16",
            DRAGON       => "LOC_ID_CLASS_DESC_17",
            TRAITOR      => "LOC_ID_CLASS_DESC_18",
            _            => "",
        };
    }

   
    public static string ProfileCardDescription(byte classType)
    {
        return classType switch
        {
            KNIGHT       => "LOC_ID_PROFILE_DESC_1".GetResourceString(),
            KNIGHT2      => "LOC_ID_PROFILE_DESC_2".GetResourceString(),
            ASSASSIN     => "LOC_ID_PROFILE_DESC_3_NEW".FormatResourceString(PlayerEV.ASSASSIN_CRITCHANCE_MOD * 100, PlayerEV.ASSASSIN_CRITDAMAGE_MOD * 100),
            ASSASSIN2    => "LOC_ID_PROFILE_DESC_4_NEW".FormatResourceString(PlayerEV.ASSASSIN_CRITCHANCE_MOD * 100, PlayerEV.ASSASSIN_CRITDAMAGE_MOD * 100),
            BANKER       => "LOC_ID_PROFILE_DESC_5_NEW".FormatResourceString(PlayerEV.BANKER_GOLDGAIN_MOD * 100),
            BANKER2      => "LOC_ID_PROFILE_DESC_6_NEW".FormatResourceString(PlayerEV.BANKER_GOLDGAIN_MOD * 100),
            WIZARD       => "LOC_ID_PROFILE_DESC_7_NEW".FormatResourceString(GameEV.MAGE_MANA_GAIN),
            WIZARD2      => "LOC_ID_PROFILE_DESC_8_NEW".FormatResourceString(GameEV.MAGE_MANA_GAIN),
            BARBARIAN    => "LOC_ID_PROFILE_DESC_9".GetResourceString(),
            BARBARIAN2   => "LOC_ID_PROFILE_DESC_10".GetResourceString(),
            NINJA        => "LOC_ID_PROFILE_DESC_11_NEW".FormatResourceString(PlayerEV.NINJA_MOVESPEED_MOD * 100),
            NINJA2       => "LOC_ID_PROFILE_DESC_12_NEW".FormatResourceString(PlayerEV.NINJA_MOVESPEED_MOD * 100),
            SPELL_SWORD  => "LOC_ID_PROFILE_DESC_13_NEW".FormatResourceString(GameEV.SPELLSWORD_ATTACK_MANA_CONVERSION * 100),
            SPELL_SWORD2 => "LOC_ID_PROFILE_DESC_14_NEW".FormatResourceString(GameEV.SPELLSWORD_ATTACK_MANA_CONVERSION * 100),
            LICH         => "LOC_ID_PROFILE_DESC_15".GetResourceString(),
            LICH2        => "LOC_ID_PROFILE_DESC_16".GetResourceString(),
            DRAGON       => "LOC_ID_PROFILE_DESC_17".GetResourceString(),
            TRAITOR      => "LOC_ID_PROFILE_DESC_18".GetResourceString(),
            _            => "",
        };
    }

    public static byte GetRandomClass()
    {
        var randomClassList = new List<byte>
        {
            KNIGHT,
            WIZARD,
            BARBARIAN,
            ASSASSIN,
        };

        if (SkillSystem.GetSkill(SkillType.NinjaUnlock).ModifierAmount > 0)
        {
            randomClassList.Add(NINJA);
        }

        if (SkillSystem.GetSkill(SkillType.BankerUnlock).ModifierAmount > 0)
        {
            randomClassList.Add(BANKER);
        }

        if (SkillSystem.GetSkill(SkillType.SpellswordUnlock).ModifierAmount > 0)
        {
            randomClassList.Add(SPELL_SWORD);
        }

        if (SkillSystem.GetSkill(SkillType.LichUnlock).ModifierAmount > 0)
        {
            randomClassList.Add(LICH);
        }

        if (SkillSystem.GetSkill(SkillType.SuperSecret).ModifierAmount > 0)
        {
            randomClassList.Add(DRAGON);
        }

        if (Game.PlayerStats.ChallengeLastBossBeaten || Game.GameConfig.UnlockTraitor == 2)
        {
            randomClassList.Add(TRAITOR);
        }
       
        var randClass = randomClassList[CDGMath.RandomInt(0, randomClassList.Count - 1)];

        if (Upgraded(randClass))
        {
            randClass += TOTAL_UNIQUES;
        }

        return randClass;
    }

    public static bool Upgraded(byte classType)
    {
        return classType switch
        {
            KNIGHT      => SkillSystem.GetSkill(SkillType.KnightUp).ModifierAmount > 0,
            WIZARD      => SkillSystem.GetSkill(SkillType.MageUp).ModifierAmount > 0,
            BARBARIAN   => SkillSystem.GetSkill(SkillType.BarbarianUp).ModifierAmount > 0,
            NINJA       => SkillSystem.GetSkill(SkillType.NinjaUp).ModifierAmount > 0,
            ASSASSIN    => SkillSystem.GetSkill(SkillType.AssassinUp).ModifierAmount > 0,
            BANKER      => SkillSystem.GetSkill(SkillType.BankerUp).ModifierAmount > 0,
            SPELL_SWORD => SkillSystem.GetSkill(SkillType.SpellSwordUp).ModifierAmount > 0,
            LICH        => SkillSystem.GetSkill(SkillType.LichUp).ModifierAmount > 0,
            _           => false,
        };
    }

    public static byte[] GetSpellList(byte classType)
    {
        return classType switch
        {
            KNIGHT or KNIGHT2 =>
            [
                SpellType.AXE,
                SpellType.DAGGER,
                SpellType.BOOMERANG,
                SpellType.DUAL_BLADES,
                SpellType.CLOSE,
                SpellType.BOUNCE,
            ],
            BARBARIAN or BARBARIAN2 =>
            [
                SpellType.AXE,
                SpellType.DAGGER,
                SpellType.BOOMERANG,
                SpellType.DUAL_BLADES,
                SpellType.CLOSE,
            ],
            ASSASSIN or ASSASSIN2 =>
            [
                SpellType.AXE,
                SpellType.DAGGER,
                SpellType.TRANSLOCATOR,
                SpellType.BOOMERANG,
                SpellType.DUAL_BLADES,
                SpellType.BOUNCE,
            ],
            BANKER or BANKER2 =>
            [
                SpellType.AXE,
                SpellType.DAGGER,
                SpellType.BOOMERANG,
                SpellType.DUAL_BLADES,
                SpellType.DAMAGE_SHIELD,
                SpellType.BOUNCE,
            ],
            LICH or LICH2 =>
            [
                SpellType.NUKE,
                SpellType.DAMAGE_SHIELD,
                SpellType.BOUNCE,
            ],
            SPELL_SWORD or SPELL_SWORD2 =>
            [
                SpellType.AXE,
                SpellType.DAGGER,
                SpellType.BOOMERANG,
                SpellType.DUAL_BLADES,
                SpellType.CLOSE,
                SpellType.DAMAGE_SHIELD,
            ],
            WIZARD or WIZARD2 =>
            [
                SpellType.AXE,
                SpellType.DAGGER,
                SpellType.TIME_STOP,
                SpellType.BOOMERANG,
                SpellType.DUAL_BLADES,
                SpellType.CLOSE,
                SpellType.DAMAGE_SHIELD,
                SpellType.BOUNCE,
            ],
            NINJA or NINJA2 =>
            [
                SpellType.AXE,
                SpellType.DAGGER,
                SpellType.TRANSLOCATOR,
                SpellType.BOOMERANG,
                SpellType.DUAL_BLADES,
                SpellType.CLOSE,
                SpellType.BOUNCE,
            ],
            DRAGON  => [SpellType.DRAGON_FIRE],
            TRAITOR => [SpellType.RAPID_DAGGER],
            _       => null,
        };
    }
}
