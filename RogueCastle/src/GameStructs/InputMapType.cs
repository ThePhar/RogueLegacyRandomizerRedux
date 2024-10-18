using InputSystem;

namespace RogueCastle.GameStructs;

public static class InputMapType
{
    // Menu Input
    public const byte MENU_CONFIRM1 = 0;
    public const byte MENU_CONFIRM2 = 1;
    public const byte MENU_CANCEL1 = 2;
    public const byte MENU_CANCEL2 = 3;
    public const byte MENU_OPTIONS = 4;
    public const byte MENU_ROGUEMODE = 5;
    public const byte MENU_CREDITS = 6;
    public const byte MENU_PROFILECARD = 7;
    public const byte MENU_PAUSE = 8;
    public const byte MENU_MAP = 9;

    // Player Input
    public const byte PLAYER_JUMP1 = 10;
    public const byte PLAYER_JUMP2 = 11;
    public const byte PLAYER_ATTACK = 12;
    public const byte PLAYER_BLOCK = 13;
    public const byte PLAYER_DASHLEFT = 14;
    public const byte PLAYER_DASHRIGHT = 15;
    public const byte PLAYER_UP1 = 16;
    public const byte PLAYER_UP2 = 17;
    public const byte PLAYER_DOWN1 = 18;
    public const byte PLAYER_DOWN2 = 19;
    public const byte PLAYER_LEFT1 = 20;
    public const byte PLAYER_LEFT2 = 21;
    public const byte PLAYER_RIGHT1 = 22;
    public const byte PLAYER_RIGHT2 = 23;
    public const byte PLAYER_SPELL1 = 24;

    public const byte MENU_PROFILESELECT = 25;
    public const byte MENU_DELETEPROFILE = 26;

    public const byte MENU_CONFIRM3 = 27;
    public const byte MENU_CANCEL3 = 28;

    // Helper extension methods to prevent `||`ing every possible action button.
    public static bool PressedCancel(this InputMap map)
    {
        return map.JustPressed(MENU_CANCEL1) || map.JustPressed(MENU_CANCEL2) || map.JustPressed(MENU_CANCEL3);
    }

    public static bool PressedConfirm(this InputMap map)
    {
        return map.JustPressed(MENU_CONFIRM1) || map.JustPressed(MENU_CONFIRM2) || map.JustPressed(MENU_CONFIRM3);
    }

    public static bool PressedUp(this InputMap map)
    {
        return map.JustPressed(PLAYER_UP1) || map.JustPressed(PLAYER_UP2);
    }

    public static bool PressedDown(this InputMap map)
    {
        return map.JustPressed(PLAYER_DOWN1) || map.JustPressed(PLAYER_DOWN2);
    }

    public static bool PressedLeft(this InputMap map)
    {
        return map.JustPressed(PLAYER_LEFT1) || map.JustPressed(PLAYER_LEFT2);
    }

    public static bool PressedRight(this InputMap map)
    {
        return map.JustPressed(PLAYER_RIGHT1) || map.JustPressed(PLAYER_RIGHT2);
    }
}
