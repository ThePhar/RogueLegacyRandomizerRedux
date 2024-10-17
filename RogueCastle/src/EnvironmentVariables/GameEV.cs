namespace RogueCastle.EnvironmentVariables;

public static class GameEV
{
    #region Miscellanous

    /// <summary>
    ///     The cost increase of ALL traits. This value is multiplied off of the player's level.
    /// </summary>
    public const int SKILL_LEVEL_COST_INCREASE = 10; //5;//10; //20;//50;//25;

    /// <summary>
    ///     The PERCENT of your money that the gatekeeper takes when you restart a castle.
    /// </summary>
    public const float GATEKEEPER_TOLL_COST = 1;

    /// <summary>
    ///     The rate that mana drain tics on a per second basis.
    /// </summary>
    public const float MANA_OVER_TIME_TIC_RATE = 0.33f;

    /// <summary>
    ///     Speed increase the player gets when they fly.
    /// </summary>
    public const float FLIGHT_SPEED_MOD = 0.15f; //0.25f;//0.35f;

    /// <summary>
    ///     LoL Equation. Player_Armor / (ARMOR_DIVIDER + Player_Armor)
    /// </summary>
    public const int ARMOR_DIVIDER = 200;

    /// <summary>
    ///      Player gets 10% gold increase per times castle beaten.
    /// </summary>
    public const float NEWGAMEPLUS_GOLD_BOUNTY = 0.5f; //0.1f;

    /// <summary>
    ///     The percentage of health that hazards deal to player.
    /// </summary>
    public const float HAZARD_DAMAGE_PERCENT = 0.2f;

    /// <summary>
    ///     The higher the number, the lower the chance of farting.
    /// </summary>
    public const int FART_CHANCE = 91; //88; //82 //70

    /// <summary>
    ///     Total percentage of gold gained (not lost).
    /// </summary>
    public const float ARCHITECT_FEE = 0.6f; //0.7f;

    #endregion

    #region Runes

    /// <summary>
    ///     The amount of life-drain you get per enemy kill per vampirism rune.
    /// </summary>
    public const int RUNE_VAMPIRISM_HEALTH_GAIN = 2; //3;//2;

    /// <summary>
    ///     The amount of managain you get per enemy kill per mana rune.
    /// </summary>
    public const int RUNE_MANA_GAIN = 2; //3; //2;

    /// <summary>
    ///     The number of seconds of flight you get per rune.
    /// </summary>
    public const float RUNE_FLIGHT = 0.6f; //0.5f;//1.0f;//1.5f; //2.0f; //1.5f;

    public const float RUNE_MOVEMENT_SPEED_MOD = 0.2f; //0.1f;

    public const float RUNE_DAMAGE_RETURN_MOD = 0.5f; //0.3f;//0.50f;

    public const float RUNE_GOLDGAIN_MOD = 0.1f;

    public const int RUNE_MANA_HP_GAIN = 1;

    /// <summary>
    ///     The amount of mana and health you get back from the vita chambers.
    /// </summary>
    public const float ROOM_VITA_CHAMBER_REGEN_AMOUNT = 0.3f; //0.35f;

    /// <summary>
    ///     The amount of levels that is added to each room.
    /// </summary>
    public const int RUNE_CURSE_ROOM_LEVEL_GAIN = 8; //5;

    /// <summary>
    ///     The amount of levels that is added to the required room level to level up an enemy.
    /// </summary>
    public const float RUNE_GRACE_ROOM_LEVEL_LOSS = 0.75f; //1;

    public const int BASE_ERA = 700;

    #endregion

    #region Classes
    
    public const float SPELLSWORD_SPELL_DAMAGE_MOD = 2.0f; //1.5f;
    public const float SPELLSWORD_MANACOST_MOD = 2.0f;
    public const float SPELLSWORD_SPELL_SCALE = 1.75f;
    public const float SPELLSWORD_ATTACK_MANA_CONVERSION = 0.3f;

    public const int LICH_HEALTH_GAIN_PER_KILL = 4; //6;//3;
    public const int LICH_HEALTH_GAIN_PER_KILL_LESSER = 4; //3;//3;
    public const float LICH_HEALTH_CONVERSION_PERCENT = 0.5f;
    public const float LICH_MAX_HP_OFF_BASE = 1.0f; //1.25f;//2.0f;
    public const float LICH_MAX_MP_OFF_BASE = 2.0f; //1.75f;//1.25f;

    /// <summary>
    ///     The amount of mana that is drained per half second while the assassin spell is active.
    /// </summary>
    public const float ASSASSIN_ACTIVE_MANA_DRAIN = 7; //6;//5;//3;//5;

    /// <summary>
    ///     The initial cost of mana to cast the assassin's active.
    /// </summary>
    public const float ASSASSIN_ACTIVE_INITIAL_COST = 5; //10;

    public const int KNIGHT_BLOCK_DRAIN = 25;
    public const float TANOOKI_ACTIVE_MANA_DRAIN = 6; //3;
    public const float TANOOKI_ACTIVE_INITIAL_COST = 25;

    public const float BARBARIAN_SHOUT_INITIAL_COST = 20; //15;
    public const float BARBARIAN_SHOUT_KNOCKBACK_MOD = 3;

    /// <summary>
    ///     The amount of mana the dragon regains per mana over time tic rate.
    /// </summary>
    public const float DRAGON_MANA_GAIN = 4;

    /// <summary>
    ///      The amount of mana the spelunker loses while the light is on PER SECOND.
    /// </summary>
    /// <remarks>DOES NOT USE TICK RATE.</remarks>
    public const float SPELUNKER_LIGHT_DRAIN = 0; //1;

    /// <summary>
    ///     The amount of mana the Mage gets from killing enemies.
    /// </summary>
    public const int MAGE_MANA_GAIN = 6; //12;//10;//15;

    public const int NINJA_TELEPORT_COST = 5;
    public const int NINJA_TELEPORT_DISTANCE = 350; //300;

    #endregion

    #region Spells

    public const float TIMESTOP_ACTIVE_MANA_DRAIN = 8; //7; //5;
    public const float DAMAGESHIELD_ACTIVE_MANA_DRAIN = 6; //5;//7;

    #endregion

    #region Item Drops

    // ITEM DROP CHANCES

    /// <summary>
    ///     Percent chance of enemy dropping health or mana on death.
    /// </summary>
    public const int ENEMY_ITEMDROP_CHANCE = 2;

    /// <summary>
    ///     Chance of breakable dropping HP/MP/COIN/MONEYBAG/NOTHING.
    /// </summary>
    public static readonly int[] BreakableItemdropChance = [3, 4, 36, 1, 56];

    /// <summary>
    ///     Chance of chest being bronze, silver, or gold.
    /// </summary>
    public static readonly int[] ChestTypeChance = [87, 13, 0]; //{ 83, 14, 3 };

    /// <summary>
    ///     Chance of bronze chest dropping MONEY/STAT DROP/BLUEPRINT.
    /// </summary>
    public static readonly int[] BronzeChestItemdropChance = [85, 0, 15];
    
    /// <summary>
    ///     Chance of silver chest dropping MONEY/STAT DROP/BLUEPRINT.
    /// </summary>
    public static readonly int[] SilverChestItemdropChance = [22, 5, 73];
    
    /// <summary>
    ///     Chance of gold chest dropping MONEY/STAT DROP/BLUEPRINT.
    /// </summary>
    public static readonly int[] GoldChestItemdropChance = [0, 20, 80]; //{4, 9, 87 };

    /// <summary>
    ///     When you get a stat drop, the chance of it being STR/MAG/DEF/HP/MP/WEIGHT.
    /// </summary>
    public static readonly int[] StatDropChance = [15, 15, 15, 25, 25, 5];

    // ITEM DROP AMOUNTS

    /// <summary>
    ///     Percent amount of mana you get from a mana item drop.
    /// </summary>
    public const float ITEM_MANADROP_AMOUNT = 0.1f;

    /// <summary>
    ///     Percent amount of health you get from a health item drop.
    /// </summary>
    public const float ITEM_HEALTHDROP_AMOUNT = 0.1f;

    /// <summary>
    ///     Amount of strength you get for picking up a strength item drop.
    /// </summary>
    public const int ITEM_STAT_STRENGTH_AMOUNT = 1;

    /// <summary>
    ///     Amount of magic you get for picking up a magic item drop.
    /// </summary>
    public const int ITEM_STAT_MAGIC_AMOUNT = 1;

    /// <summary>
    ///     Amount of armor you get for picking up an armor item drop.
    /// </summary>
    public const int ITEM_STAT_ARMOR_AMOUNT = 2;

    /// <summary>
    ///     Amount of max HP you get for picking up a max HP item drop.
    /// </summary>
    public const int ITEM_STAT_MAXHP_AMOUNT = 5; //3;

    /// <summary>
    ///     Amount of max MP you get for picking up a max MP item drop.
    /// </summary>
    public const int ITEM_STAT_MAXMP_AMOUNT = 5; //3;

    /// <summary>
    ///     Amount of weight you get for picking up a weight item drop.
    /// </summary>
    public const int ITEM_STAT_WEIGHT_AMOUNT = 5; //3;

    #endregion

    #region Traits

    /// <summary>
    ///     The amount the player is scaled.
    /// </summary>
    public const float TRAIT_GIGANTISM = 3.0f; //2.5f;//3.5f;

    /// <summary>
    ///     The amount the player is scaled.
    /// </summary>
    public const float TRAIT_DWARFISM = 1.35f; //1.25f;

    /// <summary>
    ///     The amount the player will knock enemies back with Hypergonadism.
    /// </summary>
    public const float TRAIT_HYPERGONADISM = 2.0f;

    /// <summary>
    ///     The amount the player gets knocked if Skinny.
    /// </summary>
    public const float TRAIT_ECTOMORPH = 1.85f; //2.0f;

    /// <summary>
    ///     The amount the player gets knocked if Fat.
    /// </summary>
    public const float TRAIT_ENDOMORPH = 0.5f;

    /// <summary>
    ///     The amount of movespeed you get for the hyperactive trait.
    /// </summary>
    public const float TRAIT_MOVESPEED_AMOUNT = 0.3f; //0.5f; //0.25f;

    /// <summary>
    ///     The chance of dementia spawning a fake enemy.
    /// </summary>
    public const float TRAIT_DEMENTIA_SPAWN_CHANCE = 0.2f;

    /// <summary>
    ///     The min time it takes for clonus to trigger again in seconds.
    /// </summary>
    public const float TRAIT_CLONUS_MIN = 12;

    /// <summary>
    ///     The max time it takes for clonus to trigger again in seconds.
    /// </summary>
    public const float TRAIT_CLONUS_MAX = 35;

    #endregion

    // NEW PLACEHOLDER TEXT
    public static readonly string[] GameHints =
    [
        "LOC_ID_HINT_1",      // The Forest is always to the right side of the Castle.
        "LOC_ID_HINT_2",      // The Maya is always at the top of the Castle.
        "LOC_ID_HINT_3",      // The Darkness is always at the bottom of the Castle.
        "LOC_ID_HINT_4",      // If you're having trouble with a boss, try using different runes.
        "LOC_ID_HINT_5_NEW",  // Vault runes let you to jump in the air with [Input:PLAYER_JUMP1]
        "LOC_ID_HINT_6",      // Sprint runes let you dash with...
        "LOC_ID_HINT_7",      // Each class has pros and cons.  Make sure to change your playstyle accordingly.
        "LOC_ID_HINT_8",      // Exploring and finding chests is the best way to earn gold.
        "LOC_ID_HINT_9",      // Harder areas offer greater rewards.
        "LOC_ID_HINT_10_NEW", // Sky runes let you fly by pressing [Input:PLAYER_JUMP1] while in the air.
        "LOC_ID_HINT_11",     // Vampirism and Siphon runes are very powerful when stacked.
        "LOC_ID_HINT_12",     // Mastering mobility runes makes you awesome.
        "LOC_ID_HINT_13",     // Make sure to expand your manor. You never know what new skills can be revealed.
        "LOC_ID_HINT_14",     // All classes can be upgraded with unique class abilities.
        "LOC_ID_HINT_15_NEW", // Unlocked class abilities can be activated with [Input:PLAYER_BLOCK]
        "LOC_ID_HINT_16",     // Upgrade your classes early to obtain powerful class abilities.
        "LOC_ID_HINT_17",     // If you are having trouble with a room, see if you can bypass it instead.
        "LOC_ID_HINT_18",     // Buying equipment is the fastest way to raise your stats.
        "LOC_ID_HINT_19",     // Purchasing equipment is cheaper and more flexible than raising your base stats.
        "LOC_ID_HINT_20",     // You should have picked the other child.
        "LOC_ID_HINT_21",     // Runes are very powerful. Equip runes at the Enchantress, and don't forget to use them!
        "LOC_ID_HINT_22",     // Learn the nuances of your spell to maximize their potential.
        "LOC_ID_HINT_23",     // Try to hit enemies near the apex of the axe's arc in order to hit them multiple times.
        "LOC_ID_HINT_24",     // Avoid picking up the conflux orbs after casting it to maximize damage.
        "LOC_ID_HINT_25",     // Dodge the chakrams return trip in order to maximize its damage.
        "LOC_ID_HINT_26",     // Better to use mana to kill enemies than to take unnecessary damage.
        "LOC_ID_HINT_27",     // Learning enemy 'tells' is integral to surviving the castle.
        "LOC_ID_HINT_28",     // Spike traps check for a pulse to tell the dead from the living.
        "LOC_ID_HINT_29_NEW", // Press [Input:MENU_MAP] to open the map.
        "LOC_ID_HINT_30",     // Fairy chests hold all the runes in the game. Runes will help you immensely.
        "LOC_ID_HINT_31",     // If you fail a Fairy chest room, the Architect can give you a second chance.
        "LOC_ID_HINT_32",     // The Architect has a hefty fee for those who use his service.
        "LOC_ID_HINT_33",     // Bosses drop large amounts of gold on their death.
        "LOC_ID_HINT_34",     // Bury me with my money.
        "LOC_ID_HINT_35",     // If you are having trouble, try equipping Grace runes.
        "LOC_ID_HINT_36_NEW", // In options you can enable Quick Drop to downstrike and drop with [Input:PLAYER_DOWN1]
        "LOC_ID_HINT_37",     // The architect is very useful for practicing against bosses.
        "LOC_ID_HINT_38",     // The third row of equipment usually has major tradeoffs. Be careful.
        "LOC_ID_HINT_39",     // Certain runes work better with certain bosses.
        "LOC_ID_HINT_40",     // You should practice fighting bosses using the architect.
        "LOC_ID_HINT_41",     // Health is a very important stat to raise.
        "LOC_ID_HINT_42",     // Retribution runes can damage invulnerable objects.
        "LOC_ID_HINT_43",     // Class abilities are very powerful if used correctly.
        "LOC_ID_HINT_44",     // Some classes have advantages over certain bosses.
    ];
}
