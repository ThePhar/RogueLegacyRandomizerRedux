namespace RogueCastle.GameStructs;

public static class ChestConditionType
{
    public const byte NONE                 = 0;
    public const byte KILL_ALL_ENEMIES     = 1;
    public const byte HEALTH_BELOW15       = 2;
    public const byte DONT_LOOK            = 3;
    public const byte NO_JUMPING           = 4;
    public const byte NO_SOUND             = 5;
    public const byte NO_FLOOR             = 6;
    public const byte NO_ATTACKING_ENEMIES = 7;
    public const byte REACH_IN5_SECONDS    = 8;
    public const byte TAKE_NO_DAMAGE       = 9;
    public const byte INVISIBLE_CHEST      = 10;
}
