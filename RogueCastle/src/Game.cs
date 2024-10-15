using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueCastle.EVs;
using RogueCastle.Randomizer;
using RogueCastle.Screens.BaseObjects;
using SpriteSystem;
using Tweener;

namespace RogueCastle;

/// <summary>
///     This is the main type for your game
/// </summary>
public class Game : Microsoft.Xna.Framework.Game
{
    //Generic textures used for multiple objects.
    public static Texture2D       GenericTexture;
    public static Effect          MaskEffect;
    public static Effect          BWMaskEffect;
    public static Effect          ShadowEffect;
    public static Effect          ParallaxEffect;
    public static Effect          RippleEffect;
    public static GaussianBlur    GaussianBlur;
    public static Effect          HSVEffect;
    public static Effect          InvertShader;
    public static Effect          ColourSwapShader;
    public static AreaStruct[]    Area1List;
    public static EquipmentSystem EquipmentSystem;
    public static PlayerStats     PlayerStats = new();
    public static SpriteFont      PixelArtFont;
    public static SpriteFont      PixelArtFontBold;
    public static SpriteFont      JunicodeFont;
    public static SpriteFont      EnemyLevelFont;
    public static SpriteFont      PlayerLevelFont;
    public static SpriteFont      GoldFont;
    public static SpriteFont      HerzogFont;
    public static SpriteFont      JunicodeLargeFont;
    public static SpriteFont      CinzelFont;
    public static SpriteFont      BitFont;
    public static SpriteFont      NotoSansSCFont; // Noto Sans Simplified Chinese
    public static SpriteFont      RobotoSlabFont;
    public static Cue             LineageSongCue;
    public static InputMap        GlobalInput;
    public static SettingStruct   GameConfig;
    public static List<string>    NameArray;
    public static List<string>    FemaleNameArray;
    public static float           TotalGameTime;
    public static bool            gameIsCorrupt;

    private readonly float                 m_frameLimit = 1 / 40f;
    private          WeakReference         gcTracker    = new(new object());
    public           GraphicsDeviceManager graphics;

    /// <summary>
    ///     Allows the game to run logic such as updating the world,
    ///     checking for collisions, gathering input, and playing audio.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public int graphicsToggle;

    private string   m_commandLineFilePath = "";
    private bool     m_contentLoaded;
    private bool     m_femaleChineseNamesLoaded;
    private GameTime m_forcedGameTime1, m_forcedGameTime2;
    private bool     m_frameLimitSwap;
    private bool     m_gameLoaded;
    private bool     m_maleChineseNamesLoaded;

    // This makes sure your very first inputs upon returning after leaving the screen does not register (no accidental inputs happen).
    private float m_previouslyActiveCounter;

    public Game(string filePath = "")
    {
        // Make sure to remove reference from LocaleBuilder's text refresh list when a TextObj is disposed
        TextObj.disposeMethod = LocaleBuilder.RemoveFromTextRefreshList;

        if (filePath.Contains("-t"))
        {
            LevelEV.TestRoomLevelType = GameTypes.LevelType.TOWER;
            filePath = filePath.Replace("-t", "");
        }
        else if (filePath.Contains("-d"))
        {
            LevelEV.TestRoomLevelType = GameTypes.LevelType.DUNGEON;
            filePath = filePath.Replace("-d", "");
        }
        else if (filePath.Contains("-g"))
        {
            LevelEV.TestRoomLevelType = GameTypes.LevelType.GARDEN;
            filePath = filePath.Replace("-g", "");
        }

        /* flibit didn't like this
        if (Thread.CurrentThread.CurrentCulture.Name != "en-US")
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US", false);
        }
        */
        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

        m_commandLineFilePath = filePath;
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        //this.graphics.PreferredBackBufferWidth = 1360;// GlobalEV.ScreenWidth;
        //this.graphics.PreferredBackBufferHeight = 768;//GlobalEV.ScreenHeight;

        EngineEV.ScreenWidth =
            GlobalEV.SCREEN_WIDTH; // Very important. Tells the engine if the game is running at a fixed resolution (which it is).
        EngineEV.ScreenHeight = GlobalEV.SCREEN_HEIGHT;

        //this.graphics.IsFullScreen = true;
        Window.Title = "Rogue Legacy Randomizer";
        ScreenManager = new RCScreenManager(this);
        SaveManager = new SaveGameManager(this);
        ArchipelagoManager = new ArchipelagoManager(this);

        // Set first to false and last to true for targetelapsedtime to work.
        IsFixedTimeStep = false; // Sets game to slow down instead of frame skip if set to false.
        graphics.SynchronizeWithVerticalRetrace =
            !LevelEV.ShowFps; // Disables setting the FPS to your screen's refresh rate.
        // WARNING, if you turn off frame limiting, if the framerate goes over 1000 then the elapsed time will be too small a number for a float to carry and things will break.
        //this.TargetElapsedTime = TimeSpan.FromSeconds(1.0f / 60.0f);// Sets the frame rate to 30 fps.
        Window.AllowUserResizing = true;

        if (LevelEV.EnableOffscreenControl == false)
        {
            InactiveSleepTime = new TimeSpan(); // Overrides sleep time, which disables the lag when losing focus.
        }

        PhysicsManager = new PhysicsManager();
        EquipmentSystem = new EquipmentSystem();
        EquipmentSystem.InitializeEquipmentData();
        EquipmentSystem.InitializeAbilityCosts();

        //TraitSystem = new RogueCastle.TraitSystem();
        //TraitSystem.Initialize();

        GameConfig = new SettingStruct();

        GraphicsDeviceManager.PreparingDeviceSettings += ChangeGraphicsSettings;

        SleepUtil.DisableScreensaver();
    }

    public static RCScreenManager ScreenManager { get; internal set; }

    public static float HoursPlayedSinceLastSave { get; set; }

    //protected override void OnExiting(object sender, EventArgs args)
    //{
    //    // Quick hack to fix bug where save file is deleted on closing during splash screen.
    //    if (ScreenManager.CurrentScreen is CDGSplashScreen == false && ScreenManager.CurrentScreen is DemoStartScreen == false)
    //    {
    //        UpdatePlaySessionLength();

    //        ProceduralLevelScreen level = Game.ScreenManager.GetLevelScreen();
    //        //Special handling to revert your spell if you are in a carnival room.
    //        if (level != null && (level.CurrentRoom is CarnivalShoot1BonusRoom || level.CurrentRoom is CarnivalShoot2BonusRoom))
    //        {
    //            if (level.CurrentRoom is CarnivalShoot1BonusRoom)
    //                (level.CurrentRoom as CarnivalShoot1BonusRoom).UnequipPlayer();
    //            if (level.CurrentRoom is CarnivalShoot2BonusRoom)
    //                (level.CurrentRoom as CarnivalShoot2BonusRoom).UnequipPlayer();
    //        }

    //        // Special check in case the user closes the program while in the game over screen to reset the traits.
    //        if (ScreenManager.CurrentScreen is GameOverScreen)
    //            Game.PlayerStats.Traits = Vector2.Zero;

    //        if (SaveManager.FileExists(SaveType.PlayerData))
    //        {
    //            SaveManager.SaveFiles(SaveType.PlayerData, SaveType.UpgradeData);

    //            // IMPORTANT!! Only save map data if you are actually in the castle. Not at the title screen, starting room, or anywhere else. Also make sure not to save during the intro scene.
    //            if (Game.PlayerStats.TutorialComplete == true && level != null && level.CurrentRoom.Name != "Start" && level.CurrentRoom.Name != "Ending" && level.CurrentRoom.Name != "Tutorial")
    //                SaveManager.SaveFiles(SaveType.MapData);
    //        }
    //    }

    //    SWManager.instance().shutdown();
    //    base.OnExiting(sender, args);
    //}

    public PhysicsManager PhysicsManager { get; }

    public ContentManager ContentManager => Content;

    public SaveGameManager SaveManager { get; }

    public ArchipelagoManager ArchipelagoManager { get; }

    public GraphicsDeviceManager GraphicsDeviceManager => graphics;

    protected void ChangeGraphicsSettings(object sender, PreparingDeviceSettingsEventArgs e)
    {
        e.GraphicsDeviceInformation.PresentationParameters.DepthStencilFormat = DepthFormat.None;
        e.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
    }

    /// <summary>
    ///     Allows the game to perform any initialization it needs to before starting to run.
    ///     This is where it can query for any required services and load any non-graphic
    ///     related content.  Calling base.Initialize will enumerate through any components
    ///     and initialize them as well.
    /// </summary>
    protected override void Initialize()
    {
        // TODO: Add your initialization logic here.
        Tween.Initialize(7000);
        InputManager.Initialize();
        InputManager.InitializeDXManager(Services, Window);
        Buttons[] buttonList =
        {
            Buttons.X,
            Buttons.A,
            Buttons.B,
            Buttons.Y,
            Buttons.LeftShoulder,
            Buttons.RightShoulder,
            Buttons.LeftTrigger,
            Buttons.RightTrigger,
            Buttons.Back,
            Buttons.Start,
            Buttons.LeftStick,
            Buttons.RightStick,
        };
        InputManager.RemapDXPad(buttonList);

        SpriteLibrary.Init();
        DialogueManager.Initialize();

        // Default to english language
        LocaleBuilder.languageType = LanguageType.English;
#if false
            // Remove the comment tags for these language documents creating a release build.
            //TxtToBinConverter.Convert("Content\\Languages\\Diary_En.txt");
            //TxtToBinConverter.Convert("Content\\Languages\\Text_En.txt");
            
            // Comment out these language documents when creating a release build.
            // Don't forget to copy/paste the created bin files to your project's language folder!
            if (LevelEV.CREATE_RETAIL_VERSION == false)
            {
                DialogueManager.LoadLanguageDocument(Content, "Languages\\Text_En");
                DialogueManager.LoadLanguageDocument(Content, "Languages\\Diary_En");
            }
            else
            {
                DialogueManager.LoadLanguageBinFile("Content\\Languages\\Text_En.bin");
                DialogueManager.LoadLanguageBinFile("Content\\Languages\\Diary_En.bin");
            }
            DialogueManager.SetLanguage("English");
#endif

        SaveManager.Initialize();

        PhysicsManager.Initialize(ScreenManager.Camera);
        PhysicsManager.TerminalVelocity = 2000;
        //this.IsMouseVisible = true;

        //Components.Add(ScreenManager);
        // Necessary to manually call screen-manager initialize otherwise its LoadContent() method will be called first.
        ScreenManager.Initialize();
        InitializeGlobalInput();
        LoadConfig(); // Loads the config file, override language if specified in config file
        InitializeScreenConfig(); // Applies the screen config data.

        if (LevelEV.ShowFps)
        {
            var fpsCounter = new FrameRateCounter(this);
            Components.Add(fpsCounter);
            fpsCounter.Initialize();
        }

        // Code used to handle game chop.
        m_forcedGameTime1 = new GameTime(new TimeSpan(), new TimeSpan(0, 0, 0, 0, (int) (m_frameLimit * 1000)));
        m_forcedGameTime2 = new GameTime(new TimeSpan(), new TimeSpan(0, 0, 0, 0, (int) (m_frameLimit * 1050)));

        // Initializes the global input map.
        //InitializeGlobalInput();
        //LoadConfig(); // Loads the config file.
        //InitializeScreenConfig(); // Applies the screen config data.
        base.Initialize(); // Must be called before the enemylist is created so that their content is loaded.

        // Everything below this line can be disabled for release.

        if (LevelEV.CreateRetailVersion == false)
        {
            //Steps for adding enemies to editor.
            //1. Add a new EnemyEditorData object to enemyList with the name of the enemy class as the constructor (right below).
            //2. In the Builder class, add a case statement for the enemy string in BuildEnemy().
            //3. Press F5 to build and run, which should create the EnemyList.xml file that the map editor reads.
            var enemyList = new List<EnemyEditorData>();
            enemyList.Add(new EnemyEditorData(EnemyType.Skeleton));
            enemyList.Add(new EnemyEditorData(EnemyType.Knight));
            enemyList.Add(new EnemyEditorData(EnemyType.Fireball));
            enemyList.Add(new EnemyEditorData(EnemyType.Fairy));
            enemyList.Add(new EnemyEditorData(EnemyType.Turret));
            enemyList.Add(new EnemyEditorData(EnemyType.Ninja));
            enemyList.Add(new EnemyEditorData(EnemyType.Horse));
            enemyList.Add(new EnemyEditorData(EnemyType.Zombie));
            enemyList.Add(new EnemyEditorData(EnemyType.Wolf));
            enemyList.Add(new EnemyEditorData(EnemyType.BallAndChain));
            enemyList.Add(new EnemyEditorData(EnemyType.Eyeball));
            enemyList.Add(new EnemyEditorData(EnemyType.Blob));
            enemyList.Add(new EnemyEditorData(EnemyType.SwordKnight));
            enemyList.Add(new EnemyEditorData(EnemyType.Eagle));
            enemyList.Add(new EnemyEditorData(EnemyType.ShieldKnight));
            enemyList.Add(new EnemyEditorData(EnemyType.FireWizard));
            enemyList.Add(new EnemyEditorData(EnemyType.IceWizard));
            enemyList.Add(new EnemyEditorData(EnemyType.EarthWizard));
            enemyList.Add(new EnemyEditorData(EnemyType.BouncySpike));
            enemyList.Add(new EnemyEditorData(EnemyType.SpikeTrap));
            enemyList.Add(new EnemyEditorData(EnemyType.Plant));
            enemyList.Add(new EnemyEditorData(EnemyType.Energon));
            enemyList.Add(new EnemyEditorData(EnemyType.Spark));
            enemyList.Add(new EnemyEditorData(EnemyType.SkeletonArcher));
            enemyList.Add(new EnemyEditorData(EnemyType.Chicken));
            enemyList.Add(new EnemyEditorData(EnemyType.Platform));
            enemyList.Add(new EnemyEditorData(EnemyType.HomingTurret));
            enemyList.Add(new EnemyEditorData(EnemyType.LastBoss));
            enemyList.Add(new EnemyEditorData(EnemyType.Dummy));
            enemyList.Add(new EnemyEditorData(EnemyType.Starburst));
            enemyList.Add(new EnemyEditorData(EnemyType.Portrait));
            enemyList.Add(new EnemyEditorData(EnemyType.Mimic));

            // Take this out when building release version.
            XMLCompiler.CompileEnemies(enemyList, Directory.GetCurrentDirectory());
        }
    }

    public static void InitializeGlobalInput()
    {
        if (GlobalInput != null)
        {
            GlobalInput.ClearAll();
        }
        else
        {
            GlobalInput = new InputMap(PlayerIndex.One, true);
        }

        //////////// KEYBOARD INPUT MAP
        GlobalInput.AddInput(InputMapType.MENU_CONFIRM1, Keys.Enter);
        GlobalInput.AddInput(InputMapType.MENU_CANCEL1, Keys.Escape);
        GlobalInput.AddInput(InputMapType.MENU_CREDITS, Keys.LeftControl);
        GlobalInput.AddInput(InputMapType.MENU_OPTIONS, Keys.Tab);
        GlobalInput.AddInput(InputMapType.MENU_PROFILECARD, Keys.LeftShift);
        GlobalInput.AddInput(InputMapType.MENU_ROGUEMODE, Keys.Back);
        GlobalInput.AddInput(InputMapType.MENU_PAUSE, Keys.Escape);
        GlobalInput.AddInput(InputMapType.MENU_MAP, Keys.Tab);

        GlobalInput.AddInput(InputMapType.PLAYER_JUMP1, Keys.S);
        GlobalInput.AddInput(InputMapType.PLAYER_JUMP2, Keys.Space);
        GlobalInput.AddInput(InputMapType.PLAYER_SPELL1, Keys.W);
        GlobalInput.AddInput(InputMapType.PLAYER_ATTACK, Keys.D);
        GlobalInput.AddInput(InputMapType.PLAYER_BLOCK, Keys.A);
        GlobalInput.AddInput(InputMapType.PLAYER_DASHLEFT, Keys.Q);
        GlobalInput.AddInput(InputMapType.PLAYER_DASHRIGHT, Keys.E);
        GlobalInput.AddInput(InputMapType.PLAYER_UP1, Keys.I);
        GlobalInput.AddInput(InputMapType.PLAYER_UP2, Keys.Up);
        GlobalInput.AddInput(InputMapType.PLAYER_DOWN1, Keys.K);
        GlobalInput.AddInput(InputMapType.PLAYER_DOWN2, Keys.Down);
        GlobalInput.AddInput(InputMapType.PLAYER_LEFT1, Keys.J);
        GlobalInput.AddInput(InputMapType.PLAYER_LEFT2, Keys.Left);
        GlobalInput.AddInput(InputMapType.PLAYER_RIGHT1, Keys.L);
        GlobalInput.AddInput(InputMapType.PLAYER_RIGHT2, Keys.Right);

        //////////// GAMEPAD INPUT MAP

        GlobalInput.AddInput(InputMapType.MENU_CONFIRM1, Buttons.A);
        GlobalInput.AddInput(InputMapType.MENU_CONFIRM2, Buttons.Start);
        GlobalInput.AddInput(InputMapType.MENU_CANCEL1, Buttons.B);
        GlobalInput.AddInput(InputMapType.MENU_CANCEL2, Buttons.Back);
        GlobalInput.AddInput(InputMapType.MENU_CREDITS, Buttons.RightTrigger);
        GlobalInput.AddInput(InputMapType.MENU_OPTIONS, Buttons.Y);
        GlobalInput.AddInput(InputMapType.MENU_PROFILECARD, Buttons.X);
        GlobalInput.AddInput(InputMapType.MENU_ROGUEMODE, Buttons.Back);
        GlobalInput.AddInput(InputMapType.MENU_PAUSE, Buttons.Start);
        GlobalInput.AddInput(InputMapType.MENU_MAP, Buttons.Back);

        GlobalInput.AddInput(InputMapType.PLAYER_JUMP1, Buttons.A);
        GlobalInput.AddInput(InputMapType.PLAYER_ATTACK, Buttons.X);
        GlobalInput.AddInput(InputMapType.PLAYER_BLOCK, Buttons.Y);
        GlobalInput.AddInput(InputMapType.PLAYER_DASHLEFT, Buttons.LeftTrigger);
        GlobalInput.AddInput(InputMapType.PLAYER_DASHRIGHT, Buttons.RightTrigger);
        GlobalInput.AddInput(InputMapType.PLAYER_UP1, Buttons.DPadUp);
        //GlobalInput.AddInput(InputMapType.PLAYER_UP2, Buttons.LeftThumbstickUp);
        GlobalInput.AddInput(InputMapType.PLAYER_UP2, ThumbStick.LeftStick, -90, 30);
        GlobalInput.AddInput(InputMapType.PLAYER_DOWN1, Buttons.DPadDown);
        GlobalInput.AddInput(InputMapType.PLAYER_DOWN2, ThumbStick.LeftStick, 90, 37);
        GlobalInput.AddInput(InputMapType.PLAYER_LEFT1, Buttons.DPadLeft);
        GlobalInput.AddInput(InputMapType.PLAYER_LEFT2, Buttons.LeftThumbstickLeft);
        GlobalInput.AddInput(InputMapType.PLAYER_RIGHT1, Buttons.DPadRight);
        GlobalInput.AddInput(InputMapType.PLAYER_RIGHT2, Buttons.LeftThumbstickRight);
        GlobalInput.AddInput(InputMapType.PLAYER_SPELL1, Buttons.B);

        GlobalInput.AddInput(InputMapType.MENU_PROFILESELECT, Keys.Escape);
        GlobalInput.AddInput(InputMapType.MENU_PROFILESELECT, Buttons.Back);
        GlobalInput.AddInput(InputMapType.MENU_DELETEPROFILE, Keys.Back);
        GlobalInput.AddInput(InputMapType.MENU_DELETEPROFILE, Buttons.Y);

        // Adding mouse confirm/cancel controls
        GlobalInput.AddInput(InputMapType.MENU_CONFIRM3, Keys.F13);
        GlobalInput.AddInput(InputMapType.MENU_CANCEL3, Keys.F14);

        // Special code so that player attack acts as the second confirm and player jump acts as the second cancel.
        GlobalInput.KeyList[InputMapType.MENU_CONFIRM2] = GlobalInput.KeyList[InputMapType.PLAYER_ATTACK];
        GlobalInput.KeyList[InputMapType.MENU_CANCEL2] = GlobalInput.KeyList[InputMapType.PLAYER_JUMP1];
    }

    private void InitializeDefaultConfig()
    {
        GameConfig.FullScreen = false;
        GameConfig.ScreenWidth = 1360;
        GameConfig.ScreenHeight = 768;
        GameConfig.MusicVolume = 1;
        GameConfig.SFXVolume = 0.8f;
        GameConfig.EnableDirectInput = true;
        InputManager.Deadzone = 10;
        GameConfig.ProfileSlot = 1;
        GameConfig.EnableSteamCloud = false;
        GameConfig.ReduceQuality = false;

        InitializeGlobalInput();
    }

    /// <summary>
    ///     LoadContent will be called once per game and is the place to load
    ///     all of your content.
    /// </summary>
    protected override void LoadContent()
    {
        if (m_contentLoaded == false)
        {
            m_contentLoaded = true;
            LoadAllSpriteFonts();
            LoadAllEffects();
            LoadAllSpritesheets();

            // Initializing Sound Manager.
            SoundManager.Initialize("Content\\Audio\\RogueCastleXACTProj.xgs");
            SoundManager.LoadWaveBank("Content\\Audio\\SFXWaveBank.xwb");
            SoundManager.LoadWaveBank("Content\\Audio\\MusicWaveBank.xwb", true);
            SoundManager.LoadSoundBank("Content\\Audio\\SFXSoundBank.xsb");
            SoundManager.LoadSoundBank("Content\\Audio\\MusicSoundBank.xsb", true);
            SoundManager.GlobalMusicVolume = GameConfig.MusicVolume;
            SoundManager.GlobalSFXVolume = GameConfig.SFXVolume;

            if (InputManager.GamePadIsConnected(PlayerIndex.One))
            {
                InputManager.SetPadType(PlayerIndex.One, PadTypes.GamePad);
            }

            // Creating a generic texture for use.
            GenericTexture = new Texture2D(GraphicsDevice, 1, 1);
            GenericTexture.SetData(new[] { Color.White });

            // This causes massive slowdown on load.
            if (LevelEV.LoadSplashScreen == false)
            {
                LevelBuilder2.Initialize();
                LevelParser.ParseRooms("Map_1x1", Content);
                LevelParser.ParseRooms("Map_1x2", Content);
                LevelParser.ParseRooms("Map_1x3", Content);
                LevelParser.ParseRooms("Map_2x1", Content);
                LevelParser.ParseRooms("Map_2x2", Content);
                LevelParser.ParseRooms("Map_2x3", Content);
                LevelParser.ParseRooms("Map_3x1", Content);
                LevelParser.ParseRooms("Map_3x2", Content);
                LevelParser.ParseRooms("Map_Special", Content);
                LevelParser.ParseRooms("Map_DLC1", Content, true);
                LevelBuilder2.IndexRoomList();
            }

            SkillSystem.Initialize(); // Must be initialized after the sprites are loaded because the MiscSpritesheet is needed.

            var CastleZone = new AreaStruct
            {
                Name = "The Grand Entrance",
                LevelType = GameTypes.LevelType.CASTLE,
                TotalRooms = new Vector2(24, 28), //(17,19),//(20, 22),//(25,35),//(20,25),//(15, 25),
                BossInArea = true,
                SecretRooms = new Vector2(1, 3), //(2, 3),
                BonusRooms = new Vector2(2, 3),
                Color = Color.White,
            };

            var GardenZone = new AreaStruct
            {
                LevelType = GameTypes.LevelType.GARDEN,
                TotalRooms = new Vector2(23, 27), //(25,29),//(25, 35),//(15, 25),
                BossInArea = true,
                SecretRooms = new Vector2(1, 3),
                BonusRooms = new Vector2(2, 3),
                Color = Color.Green,
            };

            var TowerZone = new AreaStruct
            {
                LevelType = GameTypes.LevelType.TOWER,
                TotalRooms = new Vector2(23, 27), //(27,31),//(25,29),//(25, 35),//(15, 25),
                BossInArea = true,
                SecretRooms = new Vector2(1, 3),
                BonusRooms = new Vector2(2, 3),
                Color = Color.DarkBlue,
            };

            var DungeonZone = new AreaStruct
            {
                LevelType = GameTypes.LevelType.DUNGEON,
                TotalRooms = new Vector2(23, 27), //(29,33),//(25, 29),//(25, 35),//(15, 25),
                BossInArea = true,
                SecretRooms = new Vector2(1, 3),
                BonusRooms = new Vector2(2, 3),
                Color = Color.Red,
            };

            #region Demo Levels

            var CastleZoneDemo = new AreaStruct
            {
                Name = "The Grand Entrance",
                LevelType = GameTypes.LevelType.CASTLE,
                TotalRooms = new Vector2(24, 27), //(25,35),//(20,25),//(15, 25),
                BossInArea = true,
                SecretRooms = new Vector2(2, 3),
                BonusRooms = new Vector2(2, 3),
                Color = Color.White,
            };

            var GardenZoneDemo = new AreaStruct
            {
                Name = "The Grand Entrance",
                LevelType = GameTypes.LevelType.GARDEN,
                TotalRooms = new Vector2(12, 14), //(25, 35),//(15, 25),
                BossInArea = true,
                SecretRooms = new Vector2(2, 3),
                BonusRooms = new Vector2(1, 2),
                Color = Color.Green,
            };

            var DungeonZoneDemo = new AreaStruct
            {
                Name = "The Grand Entrance",
                LevelType = GameTypes.LevelType.DUNGEON,
                TotalRooms = new Vector2(12, 14), //(25, 35),//(15, 25),
                BossInArea = true,
                SecretRooms = new Vector2(2, 3),
                BonusRooms = new Vector2(1, 2),
                Color = Color.Red,
            };

            var TowerZoneDemo = new AreaStruct
            {
                Name = "The Grand Entrance",
                LevelType = GameTypes.LevelType.TOWER,
                TotalRooms = new Vector2(12, 14), //(25, 35),//(15, 25),
                BossInArea = true,
                SecretRooms = new Vector2(2, 3),
                BonusRooms = new Vector2(1, 2),
                Color = Color.DarkBlue,
            };

            #endregion

            Area1List = new[] { CastleZone, GardenZone, TowerZone, DungeonZone }; //DUNGEON IS LAST AREA

            if (LevelEV.RunDemoVersion)
            {
                Area1List = new[] { CastleZoneDemo }; //DUNGEON IS LAST AREA
            }
            //Area1List = new AreaStruct[] { CastleZoneDemo, GardenZoneDemo, TowerZoneDemo, DungeonZoneDemo }; //DUNGEON IS LAST AREA
        }

        //ScreenManager.LoadContent(); // What is this doing here?
    }

    public void LoadAllSpriteFonts()
    {
        SpriteFontArray.SpriteFontList.Clear();
        PixelArtFont = Content.Load<SpriteFont>("Fonts\\Arial12");
        SpriteFontArray.SpriteFontList.Add(PixelArtFont);
        PixelArtFontBold = Content.Load<SpriteFont>("Fonts\\PixelArtFontBold");
        SpriteFontArray.SpriteFontList.Add(PixelArtFontBold);
        EnemyLevelFont = Content.Load<SpriteFont>("Fonts\\EnemyLevelFont");
        SpriteFontArray.SpriteFontList.Add(EnemyLevelFont);
        EnemyLevelFont.Spacing = -5;
        PlayerLevelFont = Content.Load<SpriteFont>("Fonts\\PlayerLevelFont");
        SpriteFontArray.SpriteFontList.Add(PlayerLevelFont);
        PlayerLevelFont.Spacing = -7;
        GoldFont = Content.Load<SpriteFont>("Fonts\\GoldFont");
        SpriteFontArray.SpriteFontList.Add(GoldFont);
        GoldFont.Spacing = -5;
        JunicodeFont = Content.Load<SpriteFont>("Fonts\\Junicode");
        JunicodeFont.DefaultCharacter = 'П';
        SpriteFontArray.SpriteFontList.Add(JunicodeFont);
        //JunicodeFont.Spacing = -1;
        JunicodeLargeFont = Content.Load<SpriteFont>("Fonts\\JunicodeLarge");
        SpriteFontArray.SpriteFontList.Add(JunicodeLargeFont);
        JunicodeLargeFont.Spacing = -1;
        HerzogFont = Content.Load<SpriteFont>("Fonts\\HerzogVonGraf24");
        SpriteFontArray.SpriteFontList.Add(HerzogFont);
        CinzelFont = Content.Load<SpriteFont>("Fonts\\CinzelFont");
        SpriteFontArray.SpriteFontList.Add(CinzelFont);
        BitFont = Content.Load<SpriteFont>("Fonts\\BitFont");
        BitFont.DefaultCharacter = '?';
        SpriteFontArray.SpriteFontList.Add(BitFont);
        NotoSansSCFont = Content.Load<SpriteFont>("Fonts\\NotoSansSC");
        SpriteFontArray.SpriteFontList.Add(NotoSansSCFont);
        RobotoSlabFont = Content.Load<SpriteFont>("Fonts\\RobotoSlab");
        SpriteFontArray.SpriteFontList.Add(RobotoSlabFont);
    }

    public void LoadAllSpritesheets()
    {
        SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\blacksmithUISpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\enemyFinal2Spritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\enemyFinalSpritesheetBig", false);
        SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\miscSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\traitsCastleSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\castleTerrainSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\playerSpritesheetBig", false);
        SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\titleScreen3Spritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\mapSpritesheetBig", false);
        SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\startingRoomSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\towerTerrainSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\dungeonTerrainSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\profileCardSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\portraitSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\gardenTerrainSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\parallaxBGSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\getItemScreenSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\neoTerrainSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\languageSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\language2Spritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\language3Spritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, "GameSpritesheets\\blitworksSpritesheet", false);
    }

    public void LoadAllEffects()
    {
        // Necessary stuff to create a 2D shader.
        MaskEffect = Content.Load<Effect>("Shaders\\AlphaMaskShader");

        ShadowEffect = Content.Load<Effect>("Shaders\\ShadowFX");
        ParallaxEffect = Content.Load<Effect>("Shaders\\ParallaxFX");
        HSVEffect = Content.Load<Effect>("Shaders\\HSVShader");
        InvertShader = Content.Load<Effect>("Shaders\\InvertShader");
        ColourSwapShader = Content.Load<Effect>("Shaders\\ColourSwapShader");
        RippleEffect = Content.Load<Effect>("Shaders\\Shockwave");

        RippleEffect.Parameters["mag"].SetValue(2);

        GaussianBlur = new GaussianBlur(this, 1320, 720);
        GaussianBlur.Amount = 2f;
        GaussianBlur.Radius = 7;
        GaussianBlur.ComputeKernel();
        GaussianBlur.ComputeOffsets();
        GaussianBlur.InvertMask = true;

        // Necessary stuff to create Black/White mask shader.
        BWMaskEffect = Content.Load<Effect>("Shaders\\BWMaskShader");
    }

    /// <summary>
    ///     UnloadContent will be called once per game and is the place to unload
    ///     all content.
    /// </summary>
    protected override void UnloadContent()
    {
        // TODO: Unload any non ContentManager content here
    }

    protected override void Update(GameTime gameTime)
    {
        if (m_gameLoaded == false)
        {
            m_gameLoaded = true;
            if (LevelEV.DeleteSaveFile)
            {
                SaveManager.ClearAllFileTypes(true);
                SaveManager.ClearAllFileTypes(false);
            }

            if (LevelEV.LoadSplashScreen)
            {
                if (LevelEV.RunDemoVersion)
                {
                    ScreenManager.DisplayScreen(ScreenType.DemoStart, true);
                }
                else
                {
                    ScreenManager.DisplayScreen(ScreenType.CDGSplash, true);
                }
            }
            else
            {
                if (LevelEV.LoadTitleScreen == false)
                {
                    if (LevelEV.RunTestRoom)
                    {
                        ScreenManager.DisplayScreen(ScreenType.Level, true);
                    }
                    else
                    {
                        if (LevelEV.RunTutorial)
                        {
                            ScreenManager.DisplayScreen(ScreenType.TutorialRoom, true);
                        }
                        else
                            //ScreenManager.DisplayScreen(ScreenType.Lineage, true, null); // Just for testing lineages.
                        {
                            ScreenManager.DisplayScreen(ScreenType.StartingRoom, true);
                        }
                        //ScreenManager.DisplayScreen(ScreenType.Ending, true, null);
                        //ScreenManager.DisplayScreen(ScreenType.Credits, true, null);
                    }
                }
                else
                {
                    ScreenManager.DisplayScreen(ScreenType.Title, true);
                }
            }
        }

        // This code forces the game to slow down (instead of chop) if it drops below the frame limit.
        TotalGameTime = (float) gameTime.TotalGameTime.TotalSeconds;

        var gameTimeToUse = gameTime;
        if (gameTime.ElapsedGameTime.TotalSeconds > m_frameLimit)
        {
            if (m_frameLimitSwap == false)
            {
                m_frameLimitSwap = true;
                gameTimeToUse = m_forcedGameTime1;
            }
            else
            {
                m_frameLimitSwap = false;
                gameTimeToUse = m_forcedGameTime2;
            }
        }

        //if (!gcTracker.IsAlive)
        //{
        //    Console.WriteLine("A garbage collection occurred!");
        //    gcTracker = new WeakReference(new object());
        //}

        // The screenmanager is updated via the Components.Add call in the game constructor. It is called after this Update() call.
        SoundManager.Update(gameTimeToUse);
        if ((m_previouslyActiveCounter <= 0 && IsActive) ||
            LevelEV.EnableOffscreenControl) // Only accept input if you have screen focus.
        {
            InputManager.Update(gameTimeToUse);
        }

        if (LevelEV.EnableDebugInput)
        {
            HandleDebugInput();
        }

        Tween.Update(gameTimeToUse);
        ScreenManager.Update(gameTimeToUse);
        SoundManager.Update3DSounds(); // Special method to handle 3D sound overrides. Must go after enemy update.
        base.Update(gameTime);

        if (IsActive == false)
        {
            m_previouslyActiveCounter = 0.25f;
        }

        // Prevents mouse from accidentally leaving game while active.
        //if (IsActive == true)
        //    Mouse.SetPosition((int)(GlobalEV.ScreenWidth / 2f), (int)(GlobalEV.ScreenHeight / 2f));

        if (m_previouslyActiveCounter > 0)
        {
            m_previouslyActiveCounter -= 0.016f;
        }
    }

    private void HandleDebugInput()
    {
        var languageType = (int) LocaleBuilder.languageType;

        if (InputManager.JustPressed(Keys.OemQuotes, null))
        {
            languageType++;
            if (languageType >= (int) LanguageType.MAX)
            {
                languageType = 0;
            }

            LocaleBuilder.languageType = (LanguageType) languageType;
            LocaleBuilder.RefreshAllText();
            Console.WriteLine("Changing to language type: " + (LanguageType) languageType);
        }
        else if (InputManager.JustPressed(Keys.OemSemicolon, null))
        {
            languageType--;
            if (languageType < 0)
            {
                languageType = (int) LanguageType.MAX - 1;
            }

            LocaleBuilder.languageType = (LanguageType) languageType;
            LocaleBuilder.RefreshAllText();
            Console.WriteLine("Changing to language type: " + (LanguageType) languageType);
        }

        if (InputManager.JustPressed(Keys.OemPipe, null))
        {
            PlayerStats.ForceLanguageGender++;
            if (PlayerStats.ForceLanguageGender > 2)
            {
                PlayerStats.ForceLanguageGender = 0;
            }

            LocaleBuilder.RefreshAllText();
        }

        if (InputManager.JustPressed(Keys.Z, null))
        {
            graphicsToggle++;
            if (graphicsToggle > 1)
            {
                graphicsToggle = 0;
            }

            switch (graphicsToggle)
            {
                case 0:
                    ScreenManager.GetLevelScreen().SetPlayerHUDVisibility(true);
                    ScreenManager.GetLevelScreen().SetMapDisplayVisibility(true);
                    break;
                case 1:
                    ScreenManager.GetLevelScreen().SetPlayerHUDVisibility(false);
                    ScreenManager.GetLevelScreen().SetMapDisplayVisibility(false);
                    break;
            }
        }
    }

    /// <summary>
    ///     This is called when the game should draw itself.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    protected override void Draw(GameTime gameTime)
    {
        // The screen-manager is drawn via the Components.Add call in the game constructor. It is called after this
        // Draw() call.
        ScreenManager.Draw(gameTime);
        base.Draw(gameTime);
    }

    public void InitializeNameArray(bool isFemale, HashSet<string> names, bool forceCreate = false)
    {
        if (LocaleBuilder.languageType == LanguageType.Chinese_Simp)
        {
            throw new NotImplementedException("Chinese name entry is not supported by the randomizer at this time.");
        }

        var array = isFemale ? FemaleNameArray : NameArray;
        var dataset = isFemale ? "HeroineNames.txt" : "HeroNames.txt";

        if (array is { Count: > 0 } && !forceCreate)
        {
            return;
        }

        // Initialize array.
        if (array != null)
        {
            array.Clear();
        }
        else if (isFemale)
        {
            FemaleNameArray = [];
            array = FemaleNameArray;
        }
        else
        {
            NameArray = [];
            array = NameArray;
        }

        var loadContent = names.Contains("__default");
        if (loadContent)
        {
            names.Remove("__default");

            // Load default names.
            using var stream = new StreamReader(Path.Combine("Content", dataset));
            var junicode = Content.Load<SpriteFont>(@"Fonts\Junicode");
            SpriteFontArray.SpriteFontList.Add(junicode);
            var invalidCharacterTest = new TextObj(junicode);

            while (!stream.EndOfStream)
            {
                var name = stream.ReadLine()!.Trim();
                var hasInvalidChar = false;

                try
                {
                    invalidCharacterTest.Text = name;
                }
                catch
                {
                    hasInvalidChar = true;
                }

                if (name.Length > 0 && !name.StartsWith("//") && !hasInvalidChar)
                {
                    names.Add(name);
                }
            }

            invalidCharacterTest.Dispose();
            SpriteFontArray.SpriteFontList.Remove(junicode);
        }

        // Ensure we always have enough names for the amount of offspring that can exist to be chosen from.
        if (names.Count < 5)
        {
            string[] defaults = isFemale
                ? ["Jenny", "Shanoa", "Chun Li", "Dorian", "Sasha"]
                : ["Lee", "Charles", "Lancelot", "Phar", "Travis"];

            for (var i = 0; names.Count < 5; i++)
            {
                names.Add(defaults[i]);
            }
        }

        // Convert HashSet to List
        array.AddRange(names);
    }

    public static void ConvertPlayerNameFormat(ref string playerName, ref string romanNumeral)
    {
        if (playerName.Length < 3)
        {
            return;
        }

        // Remove the Sir or Lady title in the player name.
        if (playerName.Substring(0, 3) == "Sir")
        {
            if (playerName.Length > 3)
            {
                playerName = playerName.Substring(4); // Removing "Sir ".
            }
        }
        else if (playerName.Length > 3 && playerName.Substring(0, 4) == "Lady")
        {
            if (playerName.Length > 4)
            {
                playerName = playerName.Substring(5); // Removing "Lady ".
            }
        }

        // Remove the roman numerals in the player name.
        var romanNumeralIndex = playerName.Trim().LastIndexOf(" ");
        if (romanNumeralIndex > 0)
        {
            var romanNumeralString = playerName.Substring(romanNumeralIndex + 1);
            // Can't check them all, so only fix the first 40 name duplicates.
            string[] romanNumeralCheckArray =
            {
                "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV", "XVI",
                "XVII", "XVIII", "XIX", "XX",
                "XXI", "XXII", "XXIII", "XXIV", "XXV", "XXVI", "XXVII", "XXVIII", "XXIX", "XXX", "XXXI", "XXXII",
                "XXXIII", "XXXIV", "XXXV", "XXXVI",
                "XXXVII", "XXXVIII", "XXXIX", "XXXX",
            };
            for (var i = 0; i < romanNumeralCheckArray.Length; i++)
            {
                if (romanNumeralString == romanNumeralCheckArray[i])
                {
                    playerName = playerName.Substring(0, playerName.Length - romanNumeralString.Length).Trim();
                    romanNumeral = romanNumeralString.Trim();
                    break;
                }
            }
        }
    }

    public static string NameHelper(string playerName, string romanNumerals, bool isFemale,
        bool forceConversionCheck = false)
    {
        if (PlayerStats.RevisionNumber <= 0 || forceConversionCheck)
        {
            ConvertPlayerNameFormat(ref playerName, ref romanNumerals);
        }

        if (isFemale)
        {
            if (LocaleBuilder.languageType == LanguageType.Chinese_Simp &&
                (romanNumerals == "" || romanNumerals == null))
            {
                return string.Format(LocaleBuilder.getResourceString("LOC_ID_LINEAGE_OBJ_14_NEW_SINGULAR_ZH"),
                    playerName, "").Trim();
            }

            return string.Format(LocaleBuilder.getResourceString("LOC_ID_LINEAGE_OBJ_14_NEW"), playerName,
                romanNumerals).Trim();
        }

        if (LocaleBuilder.languageType == LanguageType.Chinese_Simp && (romanNumerals == "" || romanNumerals == null))
        {
            return string.Format(LocaleBuilder.getResourceString("LOC_ID_LINEAGE_OBJ_12_NEW_SINGULAR_ZH"), playerName,
                "").Trim();
        }

        return string.Format(LocaleBuilder.getResourceString("LOC_ID_LINEAGE_OBJ_12_NEW"), playerName, romanNumerals)
            .Trim();
    }

    public static string NameHelper()
    {
        return NameHelper(PlayerStats.PlayerName, PlayerStats.RomanNumeral, PlayerStats.IsFemale);
    }

    public void SaveOnExit()
    {
        if (gameIsCorrupt == false)
            // Quick hack to fix bug where save file is deleted on closing during splash screen.
        {
            if (ScreenManager.CurrentScreen is CDGSplashScreen == false &&
                ScreenManager.CurrentScreen is DemoStartScreen == false)
            {
                var level = ScreenManager.GetLevelScreen();
                //Special handling to revert your spell if you are in a carnival room.
                if (level != null && (level.CurrentRoom is CarnivalShoot1BonusRoom ||
                                      level.CurrentRoom is CarnivalShoot2BonusRoom))
                {
                    if (level.CurrentRoom is CarnivalShoot1BonusRoom)
                    {
                        (level.CurrentRoom as CarnivalShoot1BonusRoom).UnequipPlayer();
                    }

                    if (level.CurrentRoom is CarnivalShoot2BonusRoom)
                    {
                        (level.CurrentRoom as CarnivalShoot2BonusRoom).UnequipPlayer();
                    }
                }

                // A check to make sure challenge rooms do not override player save data.
                if (level != null)
                {
                    var challengeRoom = level.CurrentRoom as ChallengeBossRoomObj;
                    if (challengeRoom != null)
                    {
                        challengeRoom
                            .LoadPlayerData(); // Make sure this is loaded before upgrade data, otherwise player equipment will be overridden.
                        SaveManager.LoadFiles(level, SaveType.UpgradeData);
                        level.Player.CurrentHealth = challengeRoom.StoredHP;
                        level.Player.CurrentMana = challengeRoom.StoredMP;
                    }
                }

                // Special check in case the user closes the program while in the game over screen to reset the traits.
                if (ScreenManager.CurrentScreen is GameOverScreen)
                {
                    PlayerStats.Traits = Vector2.Zero;
                }

                if (SaveManager.FileExists(SaveType.PlayerData))
                {
                    SaveManager.SaveFiles(SaveType.PlayerData, SaveType.UpgradeData);

                    // This code is needed otherwise the lineage data will still be on Revision 0 when the game exits, but player data is Rev1
                    // which results in a mismatch.
                    if (PlayerStats.RevisionNumber <= 0)
                    {
                        SaveManager.SaveFiles(SaveType.Lineage);
                    }

                    // IMPORTANT!! Only save map data if you are actually in the castle. Not at the title screen, starting room, or anywhere else. Also make sure not to save during the intro scene.
                    if (PlayerStats.TutorialComplete && level != null && level.CurrentRoom.Name != "Start" &&
                        level.CurrentRoom.Name != "Ending" && level.CurrentRoom.Name != "Tutorial")
                    {
                        SaveManager.SaveFiles(SaveType.MapData);
                    }
                }
            }
        }
    }

    public List<Vector2> GetSupportedResolutions()
    {
        var list = new List<Vector2>();
        foreach (var mode in GraphicsDevice.Adapter.SupportedDisplayModes)
            //if (mode.AspectRatio > 1.7f)
        {
            if (mode.Width < 2000 &&
                mode.Height < 2000) // Restricts the resolution to below 2048 (which is max supported texture size).
            {
                var res = new Vector2(mode.Width, mode.Height);
                if (list.Contains(res) == false)
                {
                    list.Add(new Vector2(mode.Width, mode.Height));
                }
            }
        }

        //list.Sort(delegate(Vector2 obj1, Vector2 obj2) { return obj1.X.CompareTo(obj2.X); }); // Why did I do this? It just screwed up the ordering.

        return list;
    }

    public void SaveConfig()
    {
        Console.WriteLine("Saving Config file");

        if (!Directory.Exists(Program.OSDir))
        {
            Directory.CreateDirectory(Program.OSDir);
        }

        var configFilePath = Path.Combine(Program.OSDir, "GameConfig.ini");

        using (var writer = new StreamWriter(configFilePath, false))
        {
            writer.WriteLine("[Screen Resolution]");
            writer.WriteLine("ScreenWidth=" + GameConfig.ScreenWidth);
            writer.WriteLine("ScreenHeight=" + GameConfig.ScreenHeight);
            writer.WriteLine();
            writer.WriteLine("[Fullscreen]");
            writer.WriteLine("Fullscreen=" + GameConfig.FullScreen);
            writer.WriteLine();
            writer.WriteLine("[QuickDrop]");
            writer.WriteLine("QuickDrop=" + GameConfig.QuickDrop);
            writer.WriteLine();
            writer.WriteLine("[Game Volume]");
            writer.WriteLine("MusicVol=" + string.Format("{0:F2}", GameConfig.MusicVolume));
            writer.WriteLine("SFXVol=" + string.Format("{0:F2}", GameConfig.SFXVolume));
            writer.WriteLine();
            writer.WriteLine("[Joystick Dead Zone]");
            writer.WriteLine("DeadZone=" + InputManager.Deadzone);
            writer.WriteLine();
            writer.WriteLine("[Enable DirectInput Gamepads]");
            writer.WriteLine("EnableDirectInput=" + GameConfig.EnableDirectInput);
            writer.WriteLine();
            writer.WriteLine("[Reduce Shader Quality]");
            writer.WriteLine("ReduceQuality=" + GameConfig.ReduceQuality);
            writer.WriteLine();
            //writer.WriteLine("[Enable Steam Cloud]");
            //writer.WriteLine("EnableSteamCloud=" + Game.GameConfig.EnableSteamCloud);
            //writer.WriteLine();
            writer.WriteLine("[Profile]");
            writer.WriteLine("Slot=" + GameConfig.ProfileSlot);
            writer.WriteLine();
            writer.WriteLine("[Keyboard Config]");
            writer.WriteLine("KeyUP=" + GlobalInput.KeyList[InputMapType.PLAYER_UP1]);
            writer.WriteLine("KeyDOWN=" + GlobalInput.KeyList[InputMapType.PLAYER_DOWN1]);
            writer.WriteLine("KeyLEFT=" + GlobalInput.KeyList[InputMapType.PLAYER_LEFT1]);
            writer.WriteLine("KeyRIGHT=" + GlobalInput.KeyList[InputMapType.PLAYER_RIGHT1]);
            writer.WriteLine("KeyATTACK=" + GlobalInput.KeyList[InputMapType.PLAYER_ATTACK]);
            writer.WriteLine("KeyJUMP=" + GlobalInput.KeyList[InputMapType.PLAYER_JUMP1]);
            writer.WriteLine("KeySPECIAL=" + GlobalInput.KeyList[InputMapType.PLAYER_BLOCK]);
            writer.WriteLine("KeyDASHLEFT=" + GlobalInput.KeyList[InputMapType.PLAYER_DASHLEFT]);
            writer.WriteLine("KeyDASHRIGHT=" + GlobalInput.KeyList[InputMapType.PLAYER_DASHRIGHT]);
            writer.WriteLine("KeySPELL1=" + GlobalInput.KeyList[InputMapType.PLAYER_SPELL1]);
            writer.WriteLine();
            writer.WriteLine("[Gamepad Config]");
            writer.WriteLine("ButtonUP=" + GlobalInput.ButtonList[InputMapType.PLAYER_UP1]);
            writer.WriteLine("ButtonDOWN=" + GlobalInput.ButtonList[InputMapType.PLAYER_DOWN1]);
            writer.WriteLine("ButtonLEFT=" + GlobalInput.ButtonList[InputMapType.PLAYER_LEFT1]);
            writer.WriteLine("ButtonRIGHT=" + GlobalInput.ButtonList[InputMapType.PLAYER_RIGHT1]);
            writer.WriteLine("ButtonATTACK=" + GlobalInput.ButtonList[InputMapType.PLAYER_ATTACK]);
            writer.WriteLine("ButtonJUMP=" + GlobalInput.ButtonList[InputMapType.PLAYER_JUMP1]);
            writer.WriteLine("ButtonSPECIAL=" + GlobalInput.ButtonList[InputMapType.PLAYER_BLOCK]);
            writer.WriteLine("ButtonDASHLEFT=" + GlobalInput.ButtonList[InputMapType.PLAYER_DASHLEFT]);
            writer.WriteLine("ButtonDASHRIGHT=" + GlobalInput.ButtonList[InputMapType.PLAYER_DASHRIGHT]);
            writer.WriteLine("ButtonSPELL1=" + GlobalInput.ButtonList[InputMapType.PLAYER_SPELL1]);
            writer.WriteLine();
            writer.WriteLine("[Language]");
            writer.WriteLine("Language=" + LocaleBuilder.languageType);
            writer.WriteLine();
            if (GameConfig.UnlockTraitor > 0)
            {
                writer.WriteLine("UnlockTraitor=" + GameConfig.UnlockTraitor);
            }

            writer.Close();
        }
    }

    public void LoadConfig()
    {
        Console.WriteLine("Loading Config file");
        InitializeDefaultConfig(); // Initialize a default config first in case new config data is added in the future.
        try
        {
            var configFilePath = Path.Combine(Program.OSDir, "GameConfig.ini");

            using (var reader = new StreamReader(configFilePath))
            {
                // flibit didn't like this
                // CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                // ci.NumberFormat.CurrencyDecimalSeparator = ".";
                var ci = CultureInfo.InvariantCulture;

                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    var titleIndex = line.IndexOf("=");

                    if (titleIndex != -1)
                    {
                        var lineTitle = line.Substring(0, titleIndex);
                        var lineValue = line.Substring(titleIndex + 1);

                        switch (lineTitle)
                        {
                            case "ScreenWidth":
                                GameConfig.ScreenWidth = int.Parse(lineValue, NumberStyles.Any, ci);
                                break;
                            case "ScreenHeight":
                                GameConfig.ScreenHeight = int.Parse(lineValue, NumberStyles.Any, ci);
                                break;
                            case "Fullscreen":
                                GameConfig.FullScreen = bool.Parse(lineValue);
                                break;
                            case "QuickDrop":
                                GameConfig.QuickDrop = bool.Parse(lineValue);
                                break;
                            case "MusicVol":
                                GameConfig.MusicVolume = float.Parse(lineValue);
                                break;
                            case "SFXVol":
                                GameConfig.SFXVolume = float.Parse(lineValue);
                                break;
                            case "DeadZone":
                                InputManager.Deadzone = int.Parse(lineValue, NumberStyles.Any, ci);
                                break;
                            case "EnableDirectInput":
                                GameConfig.EnableDirectInput = bool.Parse(lineValue);
                                break;
                            case "ReduceQuality":
                                GameConfig.ReduceQuality = bool.Parse(lineValue);
                                LevelEV.SaveFrames = GameConfig.ReduceQuality;
                                break;
                            case "EnableSteamCloud":
                                GameConfig.EnableSteamCloud = bool.Parse(lineValue);
                                break;
                            case "Slot":
                                GameConfig.ProfileSlot = byte.Parse(lineValue, NumberStyles.Any, ci);
                                break;
                            case "KeyUP":
                                GlobalInput.KeyList[InputMapType.PLAYER_UP1] =
                                    (Keys) Enum.Parse(typeof(Keys), lineValue);
                                break;
                            case "KeyDOWN":
                                GlobalInput.KeyList[InputMapType.PLAYER_DOWN1] =
                                    (Keys) Enum.Parse(typeof(Keys), lineValue);
                                break;
                            case "KeyLEFT":
                                GlobalInput.KeyList[InputMapType.PLAYER_LEFT1] =
                                    (Keys) Enum.Parse(typeof(Keys), lineValue);
                                break;
                            case "KeyRIGHT":
                                GlobalInput.KeyList[InputMapType.PLAYER_RIGHT1] =
                                    (Keys) Enum.Parse(typeof(Keys), lineValue);
                                break;
                            case "KeyATTACK":
                                GlobalInput.KeyList[InputMapType.PLAYER_ATTACK] =
                                    (Keys) Enum.Parse(typeof(Keys), lineValue);
                                break;
                            case "KeyJUMP":
                                GlobalInput.KeyList[InputMapType.PLAYER_JUMP1] =
                                    (Keys) Enum.Parse(typeof(Keys), lineValue);
                                break;
                            case "KeySPECIAL":
                                GlobalInput.KeyList[InputMapType.PLAYER_BLOCK] =
                                    (Keys) Enum.Parse(typeof(Keys), lineValue);
                                break;
                            case "KeyDASHLEFT":
                                GlobalInput.KeyList[InputMapType.PLAYER_DASHLEFT] =
                                    (Keys) Enum.Parse(typeof(Keys), lineValue);
                                break;
                            case "KeyDASHRIGHT":
                                GlobalInput.KeyList[InputMapType.PLAYER_DASHRIGHT] =
                                    (Keys) Enum.Parse(typeof(Keys), lineValue);
                                break;
                            case "KeySPELL1":
                                GlobalInput.KeyList[InputMapType.PLAYER_SPELL1] =
                                    (Keys) Enum.Parse(typeof(Keys), lineValue);
                                break;
                            case "ButtonUP":
                                GlobalInput.ButtonList[InputMapType.PLAYER_UP1] =
                                    (Buttons) Enum.Parse(typeof(Buttons), lineValue);
                                break;
                            case "ButtonDOWN":
                                GlobalInput.ButtonList[InputMapType.PLAYER_DOWN1] =
                                    (Buttons) Enum.Parse(typeof(Buttons), lineValue);
                                break;
                            case "ButtonLEFT":
                                GlobalInput.ButtonList[InputMapType.PLAYER_LEFT1] =
                                    (Buttons) Enum.Parse(typeof(Buttons), lineValue);
                                break;
                            case "ButtonRIGHT":
                                GlobalInput.ButtonList[InputMapType.PLAYER_RIGHT1] =
                                    (Buttons) Enum.Parse(typeof(Buttons), lineValue);
                                break;
                            case "ButtonATTACK":
                                GlobalInput.ButtonList[InputMapType.PLAYER_ATTACK] =
                                    (Buttons) Enum.Parse(typeof(Buttons), lineValue);
                                break;
                            case "ButtonJUMP":
                                GlobalInput.ButtonList[InputMapType.PLAYER_JUMP1] =
                                    (Buttons) Enum.Parse(typeof(Buttons), lineValue);
                                break;
                            case "ButtonSPECIAL":
                                GlobalInput.ButtonList[InputMapType.PLAYER_BLOCK] =
                                    (Buttons) Enum.Parse(typeof(Buttons), lineValue);
                                break;
                            case "ButtonDASHLEFT":
                                GlobalInput.ButtonList[InputMapType.PLAYER_DASHLEFT] =
                                    (Buttons) Enum.Parse(typeof(Buttons), lineValue);
                                break;
                            case "ButtonDASHRIGHT":
                                GlobalInput.ButtonList[InputMapType.PLAYER_DASHRIGHT] =
                                    (Buttons) Enum.Parse(typeof(Buttons), lineValue);
                                break;
                            case "ButtonSPELL1":
                                GlobalInput.ButtonList[InputMapType.PLAYER_SPELL1] =
                                    (Buttons) Enum.Parse(typeof(Buttons), lineValue);
                                break;
                            case "Language":
                                LocaleBuilder.languageType = (LanguageType) Enum.Parse(typeof(LanguageType), lineValue);
                                break;
                            case "UnlockTraitor":
                                GameConfig.UnlockTraitor = byte.Parse(lineValue, NumberStyles.Any, ci);
                                break;
                        }
                    }
                }

                // Special code so that player attack acts as the second confirm and player jump acts as the second cancel.
                GlobalInput.KeyList[InputMapType.MENU_CONFIRM2] = GlobalInput.KeyList[InputMapType.PLAYER_ATTACK];
                GlobalInput.KeyList[InputMapType.MENU_CANCEL2] = GlobalInput.KeyList[InputMapType.PLAYER_JUMP1];

                reader.Close();

                // Game config file was not loaded properly. Throw an exception.
                if (GameConfig.ScreenHeight <= 0 || GameConfig.ScreenWidth <= 0)
                {
                    throw new Exception("Blank Config File");
                }
            }
        }
        catch
        {
            //If exception occurred, then no file was found and default config must be created.
            Console.WriteLine("Config File Not Found. Creating Default Config File.");
            InitializeDefaultConfig();
            SaveConfig();
        }
    }

    public void InitializeScreenConfig()
    {
        if (Environment.GetEnvironmentVariable("SteamTenfoot") == "1" ||
            Environment.GetEnvironmentVariable("SteamDeck") == "1")
        {
            // We are asked to override resolution settings in Big Picture modes
            var mode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            graphics.PreferredBackBufferWidth = mode.Width;
            graphics.PreferredBackBufferHeight = mode.Height;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
        }
        else
        {
            graphics.PreferredBackBufferWidth = GameConfig.ScreenWidth;
            graphics.PreferredBackBufferHeight = GameConfig.ScreenHeight;
            if ((graphics.IsFullScreen && GameConfig.FullScreen == false) ||
                (graphics.IsFullScreen == false && GameConfig.FullScreen))
            {
                graphics.ToggleFullScreen();
            }
            else
            {
                graphics.ApplyChanges();
            }
        }

        // No need to call Graphics.ApplyChanges() since ToggleFullScreen() implicitly calls it.
        ScreenManager.ForceResolutionChangeCheck();
    }

    public static void ChangeBitmapLanguage(SpriteObj sprite, string spriteName)
    {
        switch (LocaleBuilder.languageType)
        {
            case LanguageType.English:
                sprite.ChangeSprite(spriteName);
                break;
            case LanguageType.German:
                sprite.ChangeSprite(spriteName + "_DE");
                break;
            case LanguageType.Russian:
                sprite.ChangeSprite(spriteName + "_RU");
                break;
            case LanguageType.French:
                sprite.ChangeSprite(spriteName + "_FR");
                break;
            case LanguageType.Polish:
                sprite.ChangeSprite(spriteName + "_PO");
                break;
            case LanguageType.Portuguese_Brazil:
                sprite.ChangeSprite(spriteName + "_BR");
                break;
            case LanguageType.Spanish_Spain:
                sprite.ChangeSprite(spriteName + "_SP");
                break;
            case LanguageType.Chinese_Simp:
                sprite.ChangeSprite(spriteName + "_ZH");
                break;
        }
    }

    public struct SettingStruct
    {
        public int   ScreenWidth;
        public int   ScreenHeight;
        public bool  FullScreen;
        public float MusicVolume;
        public float SFXVolume;
        public bool  QuickDrop;
        public bool  EnableDirectInput;
        public byte  ProfileSlot;
        public bool  ReduceQuality;
        public bool  EnableSteamCloud;
        public byte  UnlockTraitor;
    }
}
