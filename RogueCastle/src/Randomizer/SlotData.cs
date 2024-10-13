using Newtonsoft.Json;

namespace RogueCastle.Randomizer;

[JsonObject(MemberSerialization.OptIn)]
public class SlotData
{
    // Misc.
    [JsonProperty("starting_gender")]
    public bool StartingGenderFemale { get; init; }
    [JsonProperty("starting_class")]
    public int StartingClass { get; init; }
    [JsonProperty("number_of_children")]
    public int OffspringCount { get; init; }
    
    // Vendors
    [JsonProperty("architect")]
    public int Architect { get; init; }
    [JsonProperty("architect_fee")]
    public int ArchitectFee { get; init; }
    [JsonProperty("vendors")]
    public int Vendors { get; init; }
    [JsonProperty("require_purchasing")]
    public bool RequirePurchasingEquipment { get; init; }
    
    // Castle Manipulation
    [JsonProperty("free_diary_on_generation")]
    public bool FreeDiaryPerGeneration { get; init; }
    [JsonProperty("new_game_plus")]
    public int NewGamePlusLevel { get; init; }
    [JsonProperty("universal_chests")]
    public bool UniversalChests { get; init; }
    [JsonProperty("universal_fairy_chests")]
    public bool UniversalFairies { get; init; }
    
    // Gold Manipulation
    [JsonProperty("disable_charon")]
    public bool DisableCharon { get; init; }
    [JsonProperty("gold_gain_multiplier")]
    public int GoldGainMultiplier { get; init; }
    
    // Challenge Bosses
    [JsonProperty("khidr")]
    public bool ChallengeBossKhidr { get; init; }
    [JsonProperty("alexander")]
    public bool ChallengeBossAlexander { get; init; }
    [JsonProperty("leon")]
    public bool ChallengeBossLeon { get; init; }
    [JsonProperty("herodotus")]
    public bool ChallengeBossHerodotus { get; init; }
    
    // DeathLink
    [JsonProperty("death_link")]
    public bool DeathLink { get; init; }
}
