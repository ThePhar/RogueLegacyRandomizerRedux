using System;
using System.Collections.Generic;

namespace RogueCastle.Randomizer;

public record SlotDataV1
{
    // Counts
    public readonly int ChestsPerZone;
    public readonly int FairiesPerZone;
    public readonly int NewGamePlus;
    public readonly int ChildrenCount;
    public readonly int ArchitectFee;
    public readonly byte StartClass;
    public readonly float GoldMultiplier;
    
    // Booleans
    public readonly bool ChallengeKhidr;
    public readonly bool ChallengeAlexander;
    public readonly bool ChallengeLeon;
    public readonly bool ChallengeHerodotus;
    public readonly bool AllowDefaultNames;
    public readonly bool DeathLink;
    public readonly bool DisabledCharon;
    public readonly bool DisabledArchitect;
    public readonly bool FreeDiary;
    public readonly bool ProgressiveBlueprints;
    public readonly bool RequirePurchasing;
    public readonly bool StartIsFemale;
    public readonly bool UniversalChests;
    public readonly bool UniversalFairies;
    
    // Lists
    public readonly IEnumerable<string> NamesMale;
    public readonly IEnumerable<string> NamesFemale;
    
    public SlotDataV1(Dictionary<string, object> raw)
    {
        ChestsPerZone = Convert.ToInt32(raw["chests_per_zone"]);
        FairiesPerZone = Convert.ToInt32(raw["fairy_chests_per_zone"]);
        NewGamePlus = Convert.ToInt32(raw["new_game_plus"]);
        ChildrenCount = Convert.ToInt32(raw["number_of_children"]);
        ArchitectFee = Convert.ToInt32(raw["architect_fee"]);
        StartClass = Convert.ToByte(raw["starting_class"]);
        StartIsFemale = Convert.ToBoolean(raw["starting_gender"]);
        ChallengeKhidr = Convert.ToBoolean(raw["khidr"]);
        ChallengeAlexander = Convert.ToBoolean(raw["alexander"]);
        ChallengeLeon = Convert.ToBoolean(raw["leon"]);
        ChallengeHerodotus = Convert.ToBoolean(raw["herodotus"]);
        AllowDefaultNames = Convert.ToBoolean(raw["allow_default_names"]);
        DeathLink = Convert.ToBoolean(raw["death_link"]);
        DisabledArchitect = Convert.ToInt32(raw["architect"]) == 3;
        DisabledCharon = Convert.ToBoolean(raw["disable_charon"]);
        FreeDiary = Convert.ToBoolean(raw["free_diary_on_generation"]);
        ProgressiveBlueprints = Convert.ToBoolean(raw["progressive_blueprints"]);
        RequirePurchasing = Convert.ToBoolean(raw["require_purchasing"]);
        UniversalChests = Convert.ToBoolean(raw["universal_chests"]);
        UniversalFairies = Convert.ToBoolean(raw["universal_fairy_chests"]);
        NamesMale = raw["additional_sir_names"] as IEnumerable<string>;
        NamesFemale = raw["additional_lady_names"] as IEnumerable<string>;
        
        // Gold more complicated.
        GoldMultiplier = Convert.ToInt32(raw["gold_gain_multiplier"]) switch
        {
            1 => 0.25f,
            2 => 0.5f,
            3 => 2.0f,
            4 => 4.0f,
            _ => 1.0f,
        };
    }
}
