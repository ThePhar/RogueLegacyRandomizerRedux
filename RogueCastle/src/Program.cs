using System;
using System.IO;
using RogueCastle.EVs;
using SteamWorksWrapper;
using SDL3;

namespace RogueCastle
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Environment.SetEnvironmentVariable("FNA_PLATFORM_BACKEND", "SDL3");

            bool loadGame = true;

            if (LevelEV.CreateRetailVersion == true)// && LevelEV.CREATE_INSTALLABLE == false)
            {
                Steamworks.Init();
                loadGame = Steamworks.WasInit;
            }

            // Don't really need this anymore... -flibit
            //if (loadGame == true)
            {
#if true
                // Dave's custom EV settings for localization testing
                //LevelEV.RUN_TESTROOM = true;// false; // true; // false;
                //LevelEV.LOAD_SPLASH_SCREEN = false; // true; // false;
                //LevelEV.CREATE_RETAIL_VERSION = false;
                //LevelEV.SHOW_DEBUG_TEXT = false; // true;
#endif

                if (LevelEV.CreateRetailVersion == true)
                {
                    LevelEV.ShowEnemyRadii = false;
                    LevelEV.EnableDebugInput = false;
                    LevelEV.UnlockAllAbilities = false;
                    LevelEV.TestRoomLevelType = GameTypes.LevelType.CASTLE;
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
                    LevelEV.RunCrashLogs = true;
                    LevelEV.WeakenBosses = false;
                    LevelEV.EnableBackupSaving = true;
                    LevelEV.EnableOffscreenControl = false;
                    LevelEV.ShowFps = false;
                    LevelEV.SaveFrames = false;
                    LevelEV.UnlockAllDiaryEntries = false;
                    LevelEV.EnableBlitworksSplash = false;
                }

                if (args.Length == 1 && LevelEV.CreateRetailVersion == false)
                {
                    using (Game game = new Game(args[0]))
                    {
                        LevelEV.RunTestRoom = true;
                        LevelEV.DisableSaving = true;
                        game.Run();
                    }
                }
                else
                {
                    if (LevelEV.RunCrashLogs == true)
                    {
                        try
                        {
                            using (Game game = new Game())
                            {
                                game.Run();
                            }
                        }
                        catch (Exception e)
                        {
                            string date = DateTime.Now.ToString("dd-mm-yyyy_HH-mm-ss");
                            if (!Directory.Exists(Program.OSDir))
                                Directory.CreateDirectory(Program.OSDir);
                            string configFilePath = Path.Combine(Program.OSDir, "CrashLog_" + date + ".log");

                            //using (StreamWriter writer = new StreamWriter("CrashLog_" + date + ".log", false))
                            using (StreamWriter writer = new StreamWriter(configFilePath, false))
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
                        using (Game game = new Game())
                        {
                            game.Run();
                        }
                    }
                }
            }
            //else
            //{
            //    #if STEAM
            //    SDL.SDL_ShowSimpleMessageBox(
            //        SDL.SDL_MessageBoxFlags.SDL_MESSAGEBOX_ERROR,
            //        "Launch Error",
            //        "Please load Rogue Legacy from the Steam client",
            //        IntPtr.Zero
            //    );
            //    #endif
            //}
            Steamworks.Shutdown();
        }

        public static readonly string OSDir = GetOSDir();
        private static string GetOSDir()
        {
            string os = SDL.SDL_GetPlatform();
            if (    os.Equals("Linux") ||
                    os.Equals("FreeBSD") ||
                    os.Equals("OpenBSD") ||
                    os.Equals("NetBSD") )
            {
                string osDir = Environment.GetEnvironmentVariable("XDG_CONFIG_HOME");
                if (string.IsNullOrEmpty(osDir))
                {
                    osDir = Environment.GetEnvironmentVariable("HOME");
                    if (string.IsNullOrEmpty(osDir))
                    {
                        return "."; // Oh well.
                    }
                    else
                    {
                        return Path.Combine(osDir, ".config", "RogueLegacy");
                    }
                }
                return Path.Combine(osDir, "RogueLegacy");
            }
            else if (os.Equals("macOS"))
            {
                string osDir = Environment.GetEnvironmentVariable("HOME");
                if (string.IsNullOrEmpty(osDir))
                {
                    return "."; // Oh well.
                }
                return Path.Combine(osDir, "Library/Application Support/RogueLegacy");
            }
            else if (!os.Equals("Windows"))
            {
                throw new NotSupportedException("Unhandled SDL3 platform!");
            }
            else
            {
                string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return Path.Combine(appdata, "Rogue Legacy");
            }
        }
    }
}

