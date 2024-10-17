namespace RogueCastle.GameStructs;

public static class SpellType
{
    public const byte TOTAL = 16;

    public const byte NONE            = 0;
    public const byte DAGGER          = 1;   // DONE
    public const byte AXE             = 2;   // DONE
    public const byte TIME_BOMB       = 3;
    public const byte TIME_STOP       = 4;   // DONE - Just needs art.
    public const byte NUKE            = 5;   // DONE - Needs art.
    public const byte TRANSLOCATOR    = 6;   // DONE - Just needs effect.
    public const byte DISPLACER       = 7;   // DONE - But buggy.
    public const byte BOOMERANG       = 8;   // DONE
    public const byte DUAL_BLADES     = 9;   // DONE
    public const byte CLOSE           = 10;  // DONE
    public const byte DAMAGE_SHIELD   = 11;  // DONE
    public const byte BOUNCE          = 12;  // DONE
    public const byte DRAGON_FIRE     = 13;  // Special spell for the dragon.
    public const byte RAPID_DAGGER    = 14;
    public const byte DRAGON_FIRE_NEO = 15;

    public const byte SHOUT           = 20;  // Special spell for the barbarian.
    public const byte LASER           = 100; // DONE - Needs art. // Disabled for now.
}
