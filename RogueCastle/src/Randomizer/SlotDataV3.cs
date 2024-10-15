using System.Collections.Generic;
using Newtonsoft.Json;
using RogueCastle.Randomizer.Types;

namespace RogueCastle.Randomizer;

[JsonObject(MemberSerialization.OptIn)]
public record SlotDataV3
{
    [JsonProperty("children")]
    public Children Children { get; init; }

    [JsonProperty("level_limit")]
    public bool LimitLevels { get; init; }

    [JsonProperty("shuffle_blacksmith")]
    public bool ShuffleBlacksmith { get; init; }

    [JsonProperty("shuffle_enchantress")]
    public bool ShuffleEnchantress { get; init; }

    [JsonProperty("chests_brown")]
    public int ChestsBrown { get; init; }

    [JsonProperty("chests_silver")]
    public int ChestsSilver { get; init; }

    [JsonProperty("chests_gold")]
    public int ChestsGold { get; init; }

    [JsonProperty("chests_fairy")]
    public int ChestsFairy { get; init; }

    [JsonProperty("diary_entries")]
    public int DiaryEntries { get; init; }

    [JsonProperty("neo_bosses")]
    public NeoBosses NeoBosses { get; init; }

    [JsonProperty("additional_challenges")]
    public bool AdditionalChallenges { get; init; }

    [JsonProperty("enemy_scaling")]
    public float EnemyScalingFactor { get; init; }

    [JsonProperty("castle_scaling")]
    public float CastleScalingFactor { get; init; }

    [JsonProperty("ngplus_requirement")]
    public int NewGamePlusRequirement { get; init; }

    [JsonProperty("gold_gain")]
    public float GoldGainFactor { get; init; }

    [JsonProperty("charon")]
    public bool CharonEnabled { get; init; }

    [JsonProperty("character_names_sir")]
    public HashSet<string> CharacterNamesSir { get; init; }

    [JsonProperty("character_names_lady")]
    public HashSet<string> CharacterNamesLady { get; init; }

    [JsonProperty("max_health")]
    public int MaxHealth { get; init; }

    [JsonProperty("max_mana")]
    public int MaxMana { get; init; }

    [JsonProperty("max_attack")]
    public int MaxAttack { get; init; }

    [JsonProperty("max_magic_damage")]
    public int MaxMagicDamage { get; init; }

    [JsonProperty("death_link")]
    public DeathLink DeathLink { get; init; }

    [JsonProperty("boss_order")]
    public List<string> BossOrder { get; init; }

    [JsonProperty("fountain_pieces_required")]
    public int FountainPiecesRequired { get; init; }
}
