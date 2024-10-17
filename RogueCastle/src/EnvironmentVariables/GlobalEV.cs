namespace RogueCastle.EnvironmentVariables;

public static class GlobalEV
{
    //Base screen resolution size for the game. 1320x720 == 60x60 tiles
    public const int SCREEN_WIDTH = 1320; //1366;
    public const int SCREEN_HEIGHT = 720; //768;

    public const float GRAVITY = -1830; //-30.5f;//-28.5f;//-27f; //-15f;
    
    /// <summary>
    ///     The amount the camera is X-offset by when following the player.
    /// </summary>
    public static readonly float CameraXOffset = 0;

    /// <summary>
    ///     The amount the camera is Y-offset by when following the player.
    /// </summary>
    public static readonly float CameraYOffset = -22; //2; //-23;//50;
}
