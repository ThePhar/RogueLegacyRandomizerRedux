using System;
using System.Collections.Generic;

namespace RogueCastle.Randomizer;

public record RandoSettings {
    // public readonly bool DeathLink { get; init; }

    public static (bool, string) TryParse(Dictionary<string, object> data, out RandoSettings settings) {
        settings = null;

        // Data version 1 or 2 usually, which is not supported.
        if (!data.TryCastValue("data_version", out long version)) {
            return (false, "LOC_ID_RANDO_PARSE_OUTDATED".GetResourceString());
        }

        try {
            // Determine the supported data version and build the settings accordingly.
            switch (version) {
                case 3:
                    settings = BuildVersion3(data);
                    return (true, null);

                // Some newer version that I'm not aware of?
                default:
                    return (false, "LOC_ID_RANDO_PARSE_INCOMPATIBLE".GetResourceString());
            }
        } catch (Exception e) {
            // Corrupted APWorld?
            Console.WriteLine(e);
            return (false, "LOC_ID_RANDO_PARSE_CORRUPTED".GetResourceString());
        }
    }

    private static RandoSettings BuildVersion3(Dictionary<string, object> data) {
        return new RandoSettings();
    }
}

file static class SlotDataParseExtensions {
    public static bool TryCastValue<TK, TV, TVCast>(this IDictionary<TK, TV> dict, TK key, out TVCast value) where TVCast : TV {
        if (dict.TryGetValue(key, out var obj) && obj is TVCast valueCast) {
            value = valueCast;
            return true;
        }

        value = default;
        return false;
    }
}
