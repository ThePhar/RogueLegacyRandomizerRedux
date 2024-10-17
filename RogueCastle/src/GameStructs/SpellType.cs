namespace RogueCastle.GameStructs;

public enum SpellType
{
    None          = 0,
    Dagger        = 1,   // DONE
    Axe           = 2,   // DONE
    TimeBomb      = 3,
    TimeStop      = 4,   // DONE - Just needs art.
    Nuke          = 5,   // DONE - Needs art.
    Translocator  = 6,   // DONE - Just needs effect.
    Displacer     = 7,   // DONE - But buggy.
    Boomerang     = 8,   // DONE
    DualBlades    = 9,   // DONE
    Close         = 10,  // DONE
    DamageShield  = 11,  // DONE
    Bounce        = 12,  // DONE
    DragonFire    = 13,  // Special spell for the dragon.
    RapidDagger   = 14,
    DragonFireNeo = 15,

    Shout         = 20,  // Special spell for the barbarian.
    Laser         = 100, // DONE - Needs art. // Disabled for now.
}
