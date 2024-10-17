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
    ///     The main entry point for the application.
    /// </summary>
    private static void Main(string[] args)
    {
        Environment.SetEnvironmentVariable("FNA_PLATFORM_BACKEND", "SDL3");

        if (LevelEV.CreateRetailVersion)
        {
            Steamworks.Init();

            LevelEV.ShowEnemyRadii = false;
            LevelEV.EnableDebugInput = false;
            LevelEV.UnlockAllAbilities = false;
            LevelEV.TestRoomLevelType = GameTypes.LevelType.Castle;
            LevelEV.TestRoomReverse = false;
            LevelEV.RunTestRoom = false;
            LevelEV.ShowDebugText = false;
            LevelEV.LoadTitleScreen = false;
            LevelEV.LoadSplashScreen = true;
            LevelEV.ShowSaveLoadDebugText = false;
            LevelEV.DeleteSaveFile = false;
            LevelEV.CloseTestRoomDoors = false;
            LevelEV.RunTutorial = false;
            LevelEV.RunDemoVersion = false;
            LevelEV.DisableSaving = false;
            LevelEV.RunCrashLogs = false; // todo
            LevelEV.WeakenBosses = true; // todo
            LevelEV.EnableBackupSaving = true;
            LevelEV.EnableOffscreenControl = false;
            LevelEV.ShowFps = false;
            LevelEV.SaveFrames = false;
            LevelEV.UnlockAllDiaryEntries = false;
            LevelEV.EnableBlitworksSplash = false;
        }

        if (args.Length == 1 && LevelEV.CreateRetailVersion == false)
        {
            using var game = new Game(args[0]);
            LevelEV.RunTestRoom = true;
            LevelEV.DisableSaving = true;
            game.Run();
        }
        else
        {
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
        }

        Steamworks.Shutdown();
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
                var osDir = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
                if (!string.IsNullOrEmpty(osDir))
                {
                    return Path.Combine(osDir, "RogueLegacy");
                }

                osDir = Environment.GetEnvironmentVariable("HOME");
                return string.IsNullOrEmpty(osDir)
                    ? "." // Oh, well.
                    : Path.Combine(osDir, ".config", "RogueLegacy");

            }
            case "macOS":
            {
                var osDir = Environment.GetEnvironmentVariable("HOME");
                return string.IsNullOrEmpty(osDir)
                    ? "." // Oh, well.
                    : Path.Combine(osDir, "Library/Application Support/RogueLegacy");
            }

            case "Windows":
            {
                var osDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return Path.Combine(osDir, "Rogue Legacy");
            }

            default:
                throw new NotSupportedException("Unhandled SDL3 platform!");
        }
    }
}
