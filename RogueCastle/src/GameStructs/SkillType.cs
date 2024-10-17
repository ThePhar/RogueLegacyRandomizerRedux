using RogueCastle.EnvironmentVariables;

namespace RogueCastle.GameStructs;

public enum SkillType
{
    Null = 0,
    Filler,
    HealthUp, // Done
    InvulnTimeUp, // Done
    DeathDodge, // Done
    AttackUp, // Done
    DownStrikeUp, // Done
    CritChanceUp, // Done
    CritDamageUp, // Done
    MagicDamageUp, // Done
    ManaUp, // Done
    ManaCostDown, // Done
    Smithy, // Done
    Enchanter, // Done
    Architect, //Done
    EquipUp, // Done
    ArmorUp, // Done
    GoldGainUp, // Done
    PricesDown,
    PotionUp, // Done
    RandomizeChildren,
    LichUnlock,
    BankerUnlock,
    SpellswordUnlock,
    NinjaUnlock,
    KnightUp,
    MageUp,
    AssassinUp,
    BankerUp,
    BarbarianUp,
    LichUp,
    NinjaUp,
    SpellSwordUp,
    SuperSecret,

    Divider, // NO LONGER USED // This separates the traits from the skills.

    AttackSpeedUp, // Done
    InvulnAttackUp, // Done
    HealthUpFinal,
    EquipUpFinal,
    DamageUpFinal,
    ManaUpFinal,
    XPGainUp, // Done
    GoldFlatBonus, // Done
    ManaRegenUp,
    Run,
    Block,
    Cartographer,
    EnvDamageDown,
    GoldLossDown,
    VampireUp,
    StoutHeart, // Done
    QuickOfBreath, // Done
    BornToRun, // Done
    OutTheGate, // Done HP portion only since MP isn't implemented yet.
    Perfectionist, // Done
    Guru,
    IronLung,
    SwordMaster,
    Tank,
    Vampire,
    SecondChance,
    PeaceOfMind,
    CartographyNinja,
    StrongMan,
    Suicidalist,
    CritBarbarian,
    Magician,
    Keymaster,
    OneTimeOnly,
    CuttingOutEarly,
    Quaffer,
    SpellSword,
    Sorcerer,
    WellEndowed,
    TreasureHunter,
    MortarMaster,
    ExplosiveExpert,
    Icicle,
    Ender,
}

public static class TraitState
{
    public const byte INVISIBLE = 0;
    public const byte PURCHASABLE = 1;
    public const byte PURCHASED = 2;
    public const byte MAXED_OUT = 3;
}

public static class TraitStatType
{
    // A dummy variable used for property in TraitObj called StatType that is no longer used.
    public const int PLAYER_MAX_HEALTH = 0;

    public static float GetTraitStat(SkillType traitType)
    {
        return traitType switch
        {
            SkillType.HealthUpFinal => Game.ScreenManager.Player.MaxHealth,
            SkillType.HealthUp      => Game.ScreenManager.Player.MaxHealth,
            SkillType.InvulnTimeUp  => Game.ScreenManager.Player.InvincibilityTime,
            SkillType.DeathDodge    => SkillSystem.GetSkill(SkillType.DeathDodge).ModifierAmount * 100,
            SkillType.DamageUpFinal => Game.ScreenManager.Player.Damage,
            SkillType.AttackUp      => Game.ScreenManager.Player.Damage,
            SkillType.CritChanceUp  => Game.ScreenManager.Player.TotalCritChance,
            SkillType.CritDamageUp  => Game.ScreenManager.Player.TotalCriticalDamage * 100,
            SkillType.ManaUpFinal   => Game.ScreenManager.Player.MaxMana,
            SkillType.ManaUp        => Game.ScreenManager.Player.MaxMana,
            SkillType.ManaRegenUp   => Game.ScreenManager.Player.ManaGain,
            SkillType.EquipUpFinal  => Game.ScreenManager.Player.MaxWeight,
            SkillType.EquipUp       => Game.ScreenManager.Player.MaxWeight,
            SkillType.ArmorUp       => Game.ScreenManager.Player.TotalArmor,
            SkillType.GoldGainUp    => Game.ScreenManager.Player.TotalGoldBonus,
            SkillType.XPGainUp      => Game.ScreenManager.Player.TotalXPBonus,
            SkillType.ManaCostDown  => SkillSystem.GetSkill(SkillType.ManaCostDown).ModifierAmount * 100,
            SkillType.AttackSpeedUp => SkillSystem.GetSkill(SkillType.AttackSpeedUp).ModifierAmount * 10,
            SkillType.MagicDamageUp => Game.ScreenManager.Player.TotalMagicDamage,
            SkillType.PotionUp      => (GameEV.ITEM_HEALTHDROP_AMOUNT + SkillSystem.GetSkill(SkillType.PotionUp).ModifierAmount) * 100,
            SkillType.PricesDown    => SkillSystem.GetSkill(SkillType.PricesDown).ModifierAmount * 100,
            SkillType.DownStrikeUp  => SkillSystem.GetSkill(SkillType.DownStrikeUp).ModifierAmount * 100,
            _                       => -1,
        };
    }
}
