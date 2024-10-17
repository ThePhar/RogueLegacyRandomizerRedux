global using Traits = (byte Trait1, byte Trait2);

using System;
using System.Collections.Generic;
using DS2DEngine;

namespace RogueCastle.GameStructs;

public static class TraitType
{
    public const byte TOTAL = 38;

    public const byte NONE           = 0;
    public const byte COLOR_BLIND    = 1;  // Done
    public const byte GAY            = 2;  // Done
    public const byte NEAR_SIGHTED   = 3;  // Done
    public const byte FAR_SIGHTED    = 4;  // Done
    public const byte DYSLEXIA       = 5;  // Done
    public const byte GIGANTISM      = 6;  // Done
    public const byte DWARFISM       = 7;  // Done
    public const byte BALDNESS       = 8;  // Done
    public const byte ENDOMORPH      = 9;  // Done
    public const byte ECTOMORPH      = 10; // Done
    public const byte ALZHEIMERS     = 11; // Done
    public const byte DEXTROCARDIA   = 12; // Done
    public const byte TOURETTES      = 13; // Done
    public const byte HYPERACTIVE    = 14; // Done
    public const byte OCD            = 15; // Done
    public const byte HYPERGONADISM  = 16; // Done
    public const byte HYPOGONADISM   = 17; // Done
    public const byte STEREO_BLIND   = 18; // Done
    public const byte IBS            = 19; // Done
    public const byte VERTIGO        = 20; // Done
    public const byte TUNNEL_VISION  = 21; // Done
    public const byte AMBILEVOUS     = 22; // Done
    public const byte PAD            = 23; // Done
    public const byte ALEKTOROPHOBIA = 24; // Done
    public const byte HYPOCHONDRIAC  = 25; // Done
    public const byte DEMENTIA       = 26; // Done
    public const byte HYPERMOBILITY  = 27; // Done
    public const byte EIDETIC_MEMORY = 28; // Done
    public const byte NOSTALGIC      = 29; // Done
    public const byte CIP            = 30; // Done
    public const byte SAVANT         = 31; // Done
    public const byte THE_ONE        = 32; // Done
    public const byte NO_FURNITURE   = 33; // Done
    public const byte PLATFORMS_OPEN = 34; // Done
    public const byte GLAUCOMA       = 35; // Done
    public const byte CLONUS         = 36; // Done
    public const byte PROSOPAGNOSIA  = 37; // Done
    public const byte ADOPTED        = 100;

    public static string ToStringID(byte traitType)
    {
        return traitType switch
        {
            COLOR_BLIND    => "LOC_ID_TRAIT_TYPE_1",
            GAY            => "LOC_ID_TRAIT_TYPE_2",
            NEAR_SIGHTED   => "LOC_ID_TRAIT_TYPE_3",
            FAR_SIGHTED    => "LOC_ID_TRAIT_TYPE_4",
            DYSLEXIA       => "LOC_ID_TRAIT_TYPE_5",
            GIGANTISM      => "LOC_ID_TRAIT_TYPE_6",
            DWARFISM       => "LOC_ID_TRAIT_TYPE_7",
            BALDNESS       => "LOC_ID_TRAIT_TYPE_8",
            ENDOMORPH      => "LOC_ID_TRAIT_TYPE_9",
            ECTOMORPH      => "LOC_ID_TRAIT_TYPE_10",
            ALZHEIMERS     => "LOC_ID_TRAIT_TYPE_11",
            DEXTROCARDIA   => "LOC_ID_TRAIT_TYPE_12",
            TOURETTES      => "LOC_ID_TRAIT_TYPE_13",
            HYPERACTIVE    => "LOC_ID_TRAIT_TYPE_14",
            OCD            => "LOC_ID_TRAIT_TYPE_15",
            HYPERGONADISM  => "LOC_ID_TRAIT_TYPE_16",
            HYPOGONADISM   => "LOC_ID_TRAIT_TYPE_17",
            STEREO_BLIND   => "LOC_ID_TRAIT_TYPE_18",
            IBS            => "LOC_ID_TRAIT_TYPE_19",
            VERTIGO        => "LOC_ID_TRAIT_TYPE_20",
            TUNNEL_VISION  => "LOC_ID_TRAIT_TYPE_21",
            AMBILEVOUS     => "LOC_ID_TRAIT_TYPE_22",
            PAD            => "LOC_ID_TRAIT_TYPE_23",
            ALEKTOROPHOBIA => "LOC_ID_TRAIT_TYPE_24",
            HYPOCHONDRIAC  => "LOC_ID_TRAIT_TYPE_25",
            DEMENTIA       => "LOC_ID_TRAIT_TYPE_26",
            HYPERMOBILITY  => "LOC_ID_TRAIT_TYPE_27",
            EIDETIC_MEMORY => "LOC_ID_TRAIT_TYPE_28",
            NOSTALGIC      => "LOC_ID_TRAIT_TYPE_29",
            CIP            => "LOC_ID_TRAIT_TYPE_30",
            SAVANT         => "LOC_ID_TRAIT_TYPE_31",
            THE_ONE        => "LOC_ID_TRAIT_TYPE_32",
            NO_FURNITURE   => "LOC_ID_TRAIT_TYPE_33",
            PLATFORMS_OPEN => "LOC_ID_TRAIT_TYPE_34",
            GLAUCOMA       => "LOC_ID_TRAIT_TYPE_35",
            CLONUS         => "LOC_ID_TRAIT_TYPE_36",
            PROSOPAGNOSIA  => "LOC_ID_TRAIT_TYPE_37",
            _              => "NULL",
        };
    }

    public static string DescriptionID(byte traitType, bool isFemale)
    {
        return traitType switch
        {
            COLOR_BLIND    => "LOC_ID_TRAIT_DESC_1",
            GAY            => isFemale ? "LOC_ID_TRAIT_DESC_2" : "LOC_ID_TRAIT_DESC_2b",
            NEAR_SIGHTED   => "LOC_ID_TRAIT_DESC_3",
            FAR_SIGHTED    => "LOC_ID_TRAIT_DESC_4",
            DYSLEXIA       => "LOC_ID_TRAIT_DESC_5",
            GIGANTISM      => "LOC_ID_TRAIT_DESC_6",
            DWARFISM       => "LOC_ID_TRAIT_DESC_7",
            BALDNESS       => "LOC_ID_TRAIT_DESC_8",
            ENDOMORPH      => "LOC_ID_TRAIT_DESC_9",
            ECTOMORPH      => "LOC_ID_TRAIT_DESC_10",
            ALZHEIMERS     => "LOC_ID_TRAIT_DESC_11",
            DEXTROCARDIA   => "LOC_ID_TRAIT_DESC_12",
            TOURETTES      => "LOC_ID_TRAIT_DESC_13",
            HYPERACTIVE    => "LOC_ID_TRAIT_DESC_14",
            OCD            => "LOC_ID_TRAIT_DESC_15",
            HYPERGONADISM  => "LOC_ID_TRAIT_DESC_16",
            HYPOGONADISM   => "LOC_ID_TRAIT_DESC_17",
            STEREO_BLIND   => "LOC_ID_TRAIT_DESC_18",
            IBS            => "LOC_ID_TRAIT_DESC_19",
            VERTIGO        => "LOC_ID_TRAIT_DESC_20",
            TUNNEL_VISION  => "LOC_ID_TRAIT_DESC_21",
            AMBILEVOUS     => "LOC_ID_TRAIT_DESC_22",
            PAD            => "LOC_ID_TRAIT_DESC_23",
            ALEKTOROPHOBIA => "LOC_ID_TRAIT_DESC_24",
            HYPOCHONDRIAC  => "LOC_ID_TRAIT_DESC_25",
            DEMENTIA       => "LOC_ID_TRAIT_DESC_26",
            HYPERMOBILITY  => "LOC_ID_TRAIT_DESC_27",
            EIDETIC_MEMORY => "LOC_ID_TRAIT_DESC_28",
            NOSTALGIC      => "LOC_ID_TRAIT_DESC_29",
            CIP            => "LOC_ID_TRAIT_DESC_30",
            SAVANT         => "LOC_ID_TRAIT_DESC_31",
            THE_ONE        => "LOC_ID_TRAIT_DESC_32",
            NO_FURNITURE   => "LOC_ID_TRAIT_DESC_33",
            PLATFORMS_OPEN => "LOC_ID_TRAIT_DESC_34",
            GLAUCOMA       => "LOC_ID_TRAIT_DESC_35",
            CLONUS         => "LOC_ID_TRAIT_DESC_36",
            PROSOPAGNOSIA  => "LOC_ID_TRAIT_DESC_37",
            _              => "NULL",
        };
    }

    public static string ProfileCardDescriptionID(byte traitType)
    {
        return traitType switch
        {
            COLOR_BLIND    => "LOC_ID_TRAIT_PROF_1",
            GAY            => Game.PlayerStats.IsFemale ? "LOC_ID_TRAIT_PROF_2" : "LOC_ID_TRAIT_PROF_2b",
            NEAR_SIGHTED   => "LOC_ID_TRAIT_PROF_3",
            FAR_SIGHTED    => "LOC_ID_TRAIT_PROF_4",
            DYSLEXIA       => "LOC_ID_TRAIT_PROF_5",
            GIGANTISM      => "LOC_ID_TRAIT_PROF_6",
            DWARFISM       => "LOC_ID_TRAIT_PROF_7",
            BALDNESS       => "LOC_ID_TRAIT_PROF_8",
            ENDOMORPH      => "LOC_ID_TRAIT_PROF_9",
            ECTOMORPH      => "LOC_ID_TRAIT_PROF_10",
            ALZHEIMERS     => "LOC_ID_TRAIT_PROF_11",
            DEXTROCARDIA   => "LOC_ID_TRAIT_PROF_12",
            TOURETTES      => "LOC_ID_TRAIT_PROF_13",
            HYPERACTIVE    => "LOC_ID_TRAIT_PROF_14",
            OCD            => "LOC_ID_TRAIT_PROF_15",
            HYPERGONADISM  => "LOC_ID_TRAIT_PROF_16",
            HYPOGONADISM   => "LOC_ID_TRAIT_PROF_17",
            STEREO_BLIND   => "LOC_ID_TRAIT_PROF_18",
            IBS            => "LOC_ID_TRAIT_PROF_19",
            VERTIGO        => "LOC_ID_TRAIT_PROF_20",
            TUNNEL_VISION  => "LOC_ID_TRAIT_PROF_21",
            AMBILEVOUS     => "LOC_ID_TRAIT_PROF_22",
            PAD            => "LOC_ID_TRAIT_PROF_23",
            ALEKTOROPHOBIA => "LOC_ID_TRAIT_PROF_24",
            HYPOCHONDRIAC  => "LOC_ID_TRAIT_PROF_25",
            DEMENTIA       => "LOC_ID_TRAIT_PROF_26",
            HYPERMOBILITY  => "LOC_ID_TRAIT_PROF_27",
            EIDETIC_MEMORY => "LOC_ID_TRAIT_PROF_28",
            NOSTALGIC      => "LOC_ID_TRAIT_PROF_29",
            CIP            => "LOC_ID_TRAIT_PROF_30",
            SAVANT         => "LOC_ID_TRAIT_PROF_31",
            THE_ONE        => "LOC_ID_TRAIT_PROF_32",
            NO_FURNITURE   => "LOC_ID_TRAIT_PROF_33",
            PLATFORMS_OPEN => "LOC_ID_TRAIT_PROF_34",
            GLAUCOMA       => "LOC_ID_TRAIT_PROF_35",
            CLONUS         => "LOC_ID_TRAIT_DESC_36",
            PROSOPAGNOSIA  => "LOC_ID_TRAIT_DESC_37",
            _              => "NULL",
        };
    }

    public static Traits CreateRandomTraits()
    {
        // The percent chance of getting a consecutive trait.
        const int dropRateChanceForTrait = 39; //45;//40;
        // Minimum chance for the player to get a trait.
        const int minChanceForTrait = 1;

        byte[] traits = [NONE, NONE];

        var numTraits = 0;
        var baseChanceForTrait = 94; //60;//100;//90; // The Base chance of the player getting at least 1 trait.
        var traitChance = CDGMath.RandomInt(0, 100);

        // Start by getting the number of traits this lineage has.
        for (var i = 0; i < 2; i++)
        {
            if (traitChance < baseChanceForTrait)
            {
                numTraits++;
            }

            baseChanceForTrait = Math.Max(baseChanceForTrait - dropRateChanceForTrait, minChanceForTrait);
        }

        for (var i = 0; i < numTraits; i++)
        {
            byte rarity = CDGMath.RandomInt(0, 100) switch
            {
                <= 48 => 1, // Common
                <= 85 => 2, // Uncommon
                _     => 3, // Rare
            };

            List<byte> options = TraitsByRarity(rarity);
            traits[i] = options[CDGMath.RandomInt(0, options.Count - 1)];

            if (i > 0 && TraitConflict((traits[0], traits[1])))
            {
                i--; // Will cause this to retry again.
            }
        }

        return (traits[0], traits[1]);
    }

    private static byte Rarity(byte traitType)
    {
        return traitType switch
        {
            COLOR_BLIND    => 2,
            GAY            => 1,
            NEAR_SIGHTED   => 2,
            FAR_SIGHTED    => 3,
            DYSLEXIA       => 3,
            GIGANTISM      => 1,
            DWARFISM       => 1,
            BALDNESS       => 1,
            ENDOMORPH      => 1,
            ECTOMORPH      => 2,
            ALZHEIMERS     => 3,
            DEXTROCARDIA   => 2,
            TOURETTES      => 1,
            HYPERACTIVE    => 1,
            OCD            => 1,
            HYPERGONADISM  => 1,
            HYPOGONADISM   => 3,
            STEREO_BLIND   => 1,
            IBS            => 2,
            VERTIGO        => 3,
            TUNNEL_VISION  => 2,
            AMBILEVOUS     => 2,
            PAD            => 2,
            ALEKTOROPHOBIA => 2,
            HYPOCHONDRIAC  => 3,
            DEMENTIA       => 3,
            HYPERMOBILITY  => 2,
            EIDETIC_MEMORY => 2,
            NOSTALGIC      => 3,
            CIP            => 3,
            SAVANT         => 2,
            THE_ONE        => 3,
            NO_FURNITURE   => 2,
            PLATFORMS_OPEN => 2,
            GLAUCOMA       => 2,
            CLONUS         => 2,
            PROSOPAGNOSIA  => 2,
            _              => 0,
        };
    }

    private static List<byte> TraitsByRarity(byte rarity)
    {
        List<byte> traits = [];
        for (byte i = 0; i < TOTAL; i++)
        {
            if (rarity == Rarity(i))
            {
                traits.Add(i);
            }
        }

        return traits;
    }

    /// <summary>
    ///     Calculates whether there is a conflict between two traits.
    /// </summary>
    /// <param name="traits">A tuple pair of traits to check for conflicts.</param>
    private static bool TraitConflict(Traits traits)
    {
        if (traits is { Trait1: NONE, Trait2: NONE })
        {
            return false;
        }

        if (traits.Trait1 == traits.Trait2)
        {
            return true;
        }

        return traits switch
        {
            (HYPERGONADISM, HYPOGONADISM) or (HYPOGONADISM, HYPERGONADISM) => true,
            (ENDOMORPH, ECTOMORPH) or (ECTOMORPH, ENDOMORPH)               => true,
            (GIGANTISM, DWARFISM) or (DWARFISM, GIGANTISM)                 => true,
            (NEAR_SIGHTED, FAR_SIGHTED) or (FAR_SIGHTED, NEAR_SIGHTED)     => true,
            (COLOR_BLIND, NOSTALGIC) or (NOSTALGIC, COLOR_BLIND)           => true,
            _                                                              => false,
        };
    }
}
