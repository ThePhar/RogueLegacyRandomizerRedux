using System;
using System.IO;
using RogueCastle.EnvironmentVariables;
using RogueCastle.GameStructs;
using SDL3;
using SteamWorksWrapper;

namespace RogueCastle;

public static class Program {
    public static readonly string OSDir = GetOSDir();

    public static Game Game { get; } = new();

    /// <summary>The main entry point for the application.</summary>
#if NET
    private static void Main(string[] realArgs) {
        args = realArgs;
        SDL.SDL_RunApp(0, IntPtr.Zero, RealMain, IntPtr.Zero);
    }
    
    private static string[] args;
    private static int RealMain(int argc, IntPtr argv) {
#else
    private static void Main(string[] args) {
#endif
        Environment.SetEnvironmentVariable("FNA_PLATFORM_BACKEND", "SDL3");

        if (LevelEV.CreateRetailVersion) {
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

        // Parse command line arguments to modify game flags.
        foreach (var arg in args) {
            switch (arg) {
                case "--debug":
                    LevelEV.EnableDebugInput = true;
                    LevelEV.ShowDebugText = false;
                    LevelEV.RunCrashLogs = false;
                    LevelEV.ShowFps = true;
                    break;

                case "--no_save":
                    LevelEV.DisableSaving = true;
                    LevelEV.EnableBackupSaving = false;
                    break;

                case "--no_splash":
                    LevelEV.LoadSplashScreen = false;
                    break;
            }
        }

        try {
            Game.Run();
        } catch (Exception e) when (LevelEV.RunCrashLogs) {
            RunCrashLogger(e);
        }

        Steamworks.Shutdown();
        
#if NET
        return 0;
#endif
    }

    private static string GetOSDir() {
        switch (SDL.SDL_GetPlatform()) {
            case "Linux":
            case "FreeBSD":
            case "OpenBSD":
            case "NetBSD": {
                var homePath = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
                if (!string.IsNullOrEmpty(homePath)) {
                    return Path.Combine(homePath, "RogueLegacyRandomizer");
                }

                // Fallback home.
                homePath = Environment.GetEnvironmentVariable("HOME");
                return string.IsNullOrEmpty(homePath)
                    ? "."
                    : Path.Combine(homePath, ".config", "RogueLegacyRandomizer");
            }

            case "macOS": {
                var homePath = Environment.GetEnvironmentVariable("HOME");
                return string.IsNullOrEmpty(homePath)
                    ? "."
                    : Path.Combine(homePath, "Library/Application Support/RogueLegacyRandomizer");
            }

            case "Windows":
                return Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Rogue Legacy Randomizer"
                );

            // Any other unknown platform.
            default:
                return SDL.SDL_GetPrefPath("Cellar Door Games", "Rogue Legacy Randomizer");
        }
    }

    private static void RunCrashLogger(Exception e) {
        var datetime = DateTime.Now.ToString(@"yyyyMMddTHHmmss");
        if (!Directory.Exists(OSDir)) {
            Directory.CreateDirectory(OSDir);
        }

        var configFilePath = Path.Combine(OSDir, $"CrashLog_{datetime}.log");
        using (var writer = new StreamWriter(configFilePath, false)) {
            writer.WriteLine(e.ToString());
        }

        Console.WriteLine(e.ToString());
        SDL.SDL_ShowSimpleMessageBox(
            SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR,
            "RLRandomizer Unhandled Exception",
            "Sorry, the Rogue Legacy Randomizer has encountered an unknown error and ended up crashing. If you are\n" +
            "continuing to see this error, please make a bug report with the following information on the project\n" +
            "repository here: https://github.com/ThePhar/RogueLegacyRandomizer\n\n" +
            $"Version: {LevelEV.RLRX_VERSION} ({SDL.SDL_GetPlatform()})\n\n" +
            $"Please include the following crash log:\n{configFilePath}\n\n" +
            "Finally, if you are the mod developer, stop breaking things. Thanks.",
            IntPtr.Zero
        );
    }
}
