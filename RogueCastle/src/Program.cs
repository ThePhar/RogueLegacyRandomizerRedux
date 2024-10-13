using System;
using System.IO;
using SDL3;
using SteamWorksWrapper;

namespace RogueCastle;

public static class Program
{
    public static readonly string OSDir = GetOSDir();

    /// <summary>
    ///     The main entry point for the application.
    /// </summary>
    private static void Main(string[] args)
    {
        Environment.SetEnvironmentVariable("FNA_PLATFORM_BACKEND", "SDL3");

        if (LevelEV.CREATE_RETAIL_VERSION)
        {
            Steamworks.Init();
            
            LevelEV.SHOW_ENEMY_RADII = false;
            LevelEV.ENABLE_DEBUG_INPUT = false;
            LevelEV.UNLOCK_ALL_ABILITIES = false;
            LevelEV.TESTROOM_LEVELTYPE = GameTypes.LevelType.CASTLE;
            LevelEV.TESTROOM_REVERSE = false;
            LevelEV.RUN_TESTROOM = false;
            LevelEV.SHOW_DEBUG_TEXT = false;
            LevelEV.LOAD_TITLE_SCREEN = false;
            LevelEV.LOAD_SPLASH_SCREEN = true;
            LevelEV.SHOW_SAVELOAD_DEBUG_TEXT = false;
            LevelEV.DELETE_SAVEFILE = false;
            LevelEV.CLOSE_TESTROOM_DOORS = false;
            LevelEV.RUN_TUTORIAL = false;
            LevelEV.RUN_DEMO_VERSION = false;
            LevelEV.DISABLE_SAVING = false;
            LevelEV.RUN_CRASH_LOGS = true;
            LevelEV.WEAKEN_BOSSES = false;
            LevelEV.ENABLE_BACKUP_SAVING = true;
            LevelEV.ENABLE_OFFSCREEN_CONTROL = false;
            LevelEV.SHOW_FPS = false;
            LevelEV.SAVE_FRAMES = false;
            LevelEV.UNLOCK_ALL_DIARY_ENTRIES = false;
            LevelEV.ENABLE_BLITWORKS_SPLASH = false;
        }

        if (args.Length == 1 && LevelEV.CREATE_RETAIL_VERSION == false)
        {
            using var game = new Game(args[0]);
            LevelEV.RUN_TESTROOM = true;
            LevelEV.DISABLE_SAVING = true;
            game.Run();
        }
        else
        {
            if (LevelEV.RUN_CRASH_LOGS)
            {
                try
                {
                    using var game = new Game();
                    game.Run();
                }
                catch (Exception e)
                {
                    var date = DateTime.Now.ToString("dd-mm-yyyy_HH-mm-ss");
                    if (!Directory.Exists(OSDir))
                    {
                        Directory.CreateDirectory(OSDir);
                    }

                    var configFilePath = Path.Combine(OSDir, "CrashLog_" + date + ".log");
                    using (var writer = new StreamWriter(configFilePath, false))
                    {
                        writer.WriteLine(e.ToString());
                    }

                    Console.WriteLine(e.ToString());
                    SDL.SDL_ShowSimpleMessageBox(
                        SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR,
                        "Rogue Legacy Randomizer: An Error Occurred",
                        "Rogue Legacy Randomizer has run into an unrecoverable situation and must close. If you are\n" +
                        "not Phar, please make a bug report on the Rogue Legacy Randomizer repository below and\n" +
                        "include any crash logs so they can investigate this problem.\n\n" +
                        "If you are Phar, stop breaking things and fix your mistakes. #blamePhar\n\n" +
                        "Repository: https://github.com/ThePhar/RogueLegacyRandomizerRedux\n" +
                        $"Crash Logs: {configFilePath}\n\n" +
                        "Exception:\n" +
                        e.Message,
                        IntPtr.Zero
                    );
                }
            }
            else
            {
                using var game = new Game();
                game.Run();
            }
        }

        Steamworks.Shutdown();
    }

    private static string GetOSDir()
    {
        var os = SDL.SDL_GetPlatform();
        switch (os)
        {
            case "Linux":
            case "FreeBSD":
            case "OpenBSD":
            case "NetBSD":
            {
                var osDir = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
                if (!string.IsNullOrEmpty(osDir))
                {
                    return Path.Combine(osDir, "RogueLegacy");
                }

                osDir = Environment.GetEnvironmentVariable("HOME");
                if (string.IsNullOrEmpty(osDir))
                {
                    return "."; // Oh well.
                }

                return Path.Combine(osDir, ".config", "RogueLegacyRandomizer");

            }
            
            case "Mac OS X":
            {
                var osDir = Environment.GetEnvironmentVariable("HOME");
                if (string.IsNullOrEmpty(osDir))
                {
                    return "."; // Oh well.
                }

                return Path.Combine(osDir, "Library/Application Support/RogueLegacyRandomizer");
            }
            
            case "Windows":
            {
                var osDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return Path.Combine(osDir, "Rogue Legacy Randomizer");
            }
            
            // Unsupported SDL3 platform.
            default:
                throw new NotSupportedException("Unhandled SDL3 platform!");
        }
    }
}
