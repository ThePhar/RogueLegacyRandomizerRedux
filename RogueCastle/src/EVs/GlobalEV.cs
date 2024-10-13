namespace RogueCastle.EVs;

public static class GlobalEV
{
    // Base screen resolution size for the game. 1320x720 == 60x60 tiles
    public const int SCREEN_WIDTH = 1320; //1366; 
    public const int SCREEN_HEIGHT = 720; //768;

    public const float GRAVITY = -1830; //-30.5f;//-28.5f;//-27f; //-15f;

    // The amount the camera is offset by when following the player.
    public static float Camera_YOffset = -22; //2; //-23;//50; 
    public static float Camera_XOffset = 0;
}
