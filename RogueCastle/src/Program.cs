using System;
using System.IO;
using RogueCastle.EnvironmentVariables;
using RogueCastle.GameStructs;
using SDL3;
using SteamWorksWrapper;

namespace RogueCastle;

public static class Program
{
    public static readonly string OSDir = GetOSDir();

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
#if NET
    static void Main(string[] realArgs)
    {
        args = realArgs;
        SDL.SDL_RunApp(0, IntPtr.Zero, RealMain, IntPtr.Zero);
    }

    static string[] args;

    static int RealMain(int argc, IntPtr argv)
#else
    private static void Main(string[] args)
#endif
    {
        Environment.SetEnvironmentVariable("FNA_PLATFORM_BACKEND", "SDL3");

        // Parse command line arguments.
        foreach (var arg in args)
        {
            switch (arg)
            {
                case "--show_enemy_radii":
                    LevelEV.ShowEnemyRadii = true;
                    LevelEV.CreateRetailVersion = false;
                    break;

                case "--debug":
                    LevelEV.EnableDebugInput = true;
                    LevelEV.ShowDebugText = true;
                    LevelEV.ShowSaveLoadDebugText = true;
                    LevelEV.RunCrashLogs = false;
                    LevelEV.CreateRetailVersion = false;
                    break;

                case "--no_save":
                    LevelEV.DisableSaving = true;
                    LevelEV.EnableBackupSaving = false;
                    LevelEV.CreateRetailVersion = false;
                    break;

                case "--no_splash":
                    LevelEV.LoadSplashScreen = false;
                    LevelEV.CreateRetailVersion = false;
                    break;

                case "--weaken_bosses":
                    LevelEV.WeakenBosses = true;
                    LevelEV.CreateRetailVersion = false;
                    break;

                case "--fps":
                    LevelEV.ShowFps = true;
                    LevelEV.CreateRetailVersion = false;
                    break;
            }
        }

        if (LevelEV.CreateRetailVersion)
        {
            Steamworks.Init();

            LevelEV.ShowEnemyRadii = false;
            LevelEV.EnableDebugInput = false;
            LevelEV.UnlockAllAbilities = false;
            LevelEV.UnlockAllDiaryEntries = false;
            LevelEV.TestRoomLevelType = GameTypes.LevelType.Castle;
            LevelEV.TestRoomReverse = false;
            LevelEV.RunTestRoom = false;
            LevelEV.ShowDebugText = false;
            LevelEV.LoadTitleScreen = true;
            LevelEV.LoadSplashScreen = true;
            LevelEV.ShowSaveLoadDebugText = false;
            LevelEV.DeleteSaveFile = false;
            LevelEV.CloseTestRoomDoors = false;
            LevelEV.DisableSaving = false;
            LevelEV.RunCrashLogs = true;
            LevelEV.WeakenBosses = false;
            LevelEV.EnableOffscreenControl = false;
            LevelEV.EnableBackupSaving = true;
            LevelEV.SaveFrames = false;
        }

        // if (args.Length == 1 && LevelEV.CreateRetailVersion == false)
        // {
        //     using var game = new Game(args[0]);
        //
        //     LevelEV.RunTestRoom = true;
        //     LevelEV.DisableSaving = true;
        //     game.Run();
        // }

        if (LevelEV.RunCrashLogs)
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
                    "SAVE THIS MESSAGE!",
                    e.ToString(),
                    IntPtr.Zero
                );
            }
        }
        else
        {
            using var game = new Game();

            game.Run();
        }

        Steamworks.Shutdown();
        
#if NET
        return 0;
#endif
    }

    private static string GetOSDir()
    {
        switch (SDL.SDL_GetPlatform())
        {
            case "Linux":
            case "FreeBSD":
            case "OpenBSD":
            case "NetBSD":
            {
                var homePath = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
                if (!string.IsNullOrEmpty(homePath))
                {
                    return Path.Combine(homePath, "RogueLegacyRandomizer");
                }

                homePath = Environment.GetEnvironmentVariable("HOME");
                return string.IsNullOrEmpty(homePath)
                    ? "." // Oh, well.
                    : Path.Combine(homePath, ".config", "RogueLegacyRandomizer");

            }
            case "macOS":
            {
                var homePath = Environment.GetEnvironmentVariable("HOME");
                return string.IsNullOrEmpty(homePath)
                    ? "." // Oh, well.
                    : Path.Combine(homePath, "Library/Application Support/RogueLegacyRandomizer");
            }

            case "Windows":
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Rogue Legacy");
            
            // Any other unknown platform.
            default:
                return SDL.SDL_GetPrefPath("Cellar Door Games", "Rogue Legacy");
        }
    }
}
