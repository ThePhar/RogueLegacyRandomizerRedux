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
using RogueCastle.EnvironmentVariables;
using RogueCastle.GameStructs;
using RogueCastle.Managers;
using RogueCastle.Randomizer;
using RogueCastle.Screens;
using RogueCastle.Screens.BaseScreens;
using SpriteSystem;
using Tweener;

namespace RogueCastle;

public class Game : Microsoft.Xna.Framework.Game
{
    private const float FrameLimit = 1 / 40f;

    public static Texture2D GenericTexture;
    public static Effect MaskEffect;
    public static Effect BWMaskEffect;
    public static Effect ShadowEffect;
    public static Effect ParallaxEffect;
    public static Effect RippleEffect;
    public static GaussianBlur GaussianBlur;
    public static Effect HSVEffect;
    public static Effect InvertShader;
    public static Effect ColourSwapShader;
    public static EquipmentSystem EquipmentSystem;
    public static PlayerStats PlayerStats = new();
    public static RandomizerStats RandomizerStats = new();
    public static SpriteFont PixelArtFont;
    public static SpriteFont PixelArtFontBold;
    public static SpriteFont JunicodeFont;
    public static SpriteFont EnemyLevelFont;
    public static SpriteFont PlayerLevelFont;
    public static SpriteFont GoldFont;
    public static SpriteFont HerzogFont;
    public static SpriteFont JunicodeLargeFont;
    public static SpriteFont CinzelFont;
    public static SpriteFont BitFont;
    public static SpriteFont NotoSansSCFont; // Noto Sans Simplified Chinese
    public static SpriteFont RobotoSlabFont;
    public static Cue LineageSongCue;
    public static InputMap GlobalInput;
    public static SettingStruct GameConfig;
    public static List<string> NameArray;
    public static List<string> FemaleNameArray;
    public static float TotalGameTime;
    public static bool GameIsCorrupt;

    public GraphicsDeviceManager Graphics { get; }

    private bool _contentLoaded;
    private bool _femaleChineseNamesLoaded;
    private GameTime _forcedGameTime1, _forcedGameTime2;
    private bool _frameLimitSwap;
    private bool _gameLoaded;
    private bool _maleChineseNamesLoaded;

    // This makes sure your very first inputs upon returning after leaving the screen does not register (no accidental inputs happen).
    private float _previouslyActiveCounter;

    public int GraphicsToggle { get; set; }

    public static AreaStruct[] Area1List { get; private set; }

    public Game(string filePath = "")
    {
        // Make sure to remove reference from LocaleBuilder's text refresh list when a TextObj is disposed
        TextObj.disposeMethod = LocaleBuilder.RemoveFromTextRefreshList;

        if (filePath.Contains("-t"))
        {
            LevelEV.TestRoomLevelType = GameTypes.LevelType.Tower;
        }
        else if (filePath.Contains("-d"))
        {
            LevelEV.TestRoomLevelType = GameTypes.LevelType.Dungeon;
        }
        else if (filePath.Contains("-g"))
        {
            LevelEV.TestRoomLevelType = GameTypes.LevelType.Garden;
        }

        Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
        Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

        Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";

        // Very important. Tells the engine if the game is running at a fixed resolution (which it is).
        EngineEV.ScreenWidth = GlobalEV.SCREEN_WIDTH;
        EngineEV.ScreenHeight = GlobalEV.SCREEN_HEIGHT;

        Window.Title = "Rogue Legacy Randomizer Redux";
        ScreenManager = new RCScreenManager(this);
        SaveManager = new SaveGameManager(this);
        ArchipelagoManager = new ArchipelagoManager();

        IsFixedTimeStep = false;
        Graphics.SynchronizeWithVerticalRetrace = !LevelEV.ShowFps;
        Window.AllowUserResizing = true;

        if (LevelEV.EnableOffscreenControl == false)
        {
            InactiveSleepTime = new TimeSpan(); // Overrides sleep time, which disables the lag when losing focus.
        }

        PhysicsManager = new PhysicsManager();
        EquipmentSystem = new EquipmentSystem();
        EquipmentSystem.InitializeEquipmentData();
        EquipmentSystem.InitializeAbilityCosts();

        GameConfig = new SettingStruct();
        GraphicsDeviceManager.PreparingDeviceSettings += ChangeGraphicsSettings;
        SleepUtil.DisableScreensaver();
    }

    public static RCScreenManager ScreenManager { get; internal set; }

    public static float HoursPlayedSinceLastSave { get; set; }

    public PhysicsManager PhysicsManager { get; }

    public ContentManager ContentManager => Content;

    public SaveGameManager SaveManager { get; }

    public ArchipelagoManager ArchipelagoManager { get; }

    public GraphicsDeviceManager GraphicsDeviceManager => Graphics;

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
        Tween.Initialize(7000);
        InputManager.Initialize();
        InputManager.InitializeDXManager(Services, Window);
        Buttons[] buttonList =
        [
            Buttons.X, Buttons.A, Buttons.B, Buttons.Y, Buttons.LeftShoulder, Buttons.RightShoulder,
            Buttons.LeftTrigger, Buttons.RightTrigger, Buttons.Back, Buttons.Start, Buttons.LeftStick,
            Buttons.RightStick,
        ];
        InputManager.RemapDXPad(buttonList);

        SpriteLibrary.Init();
        DialogueManager.Initialize();

        // Default to english language
        LocaleBuilder.LanguageType = LanguageType.English;

        SaveManager.Initialize();

        PhysicsManager.Initialize(ScreenManager.Camera);
        PhysicsManager.TerminalVelocity = 2000;

        // Necessary to manually call screen-manager initialize otherwise its LoadContent() method will be called first.
        ScreenManager.Initialize();
        InitializeGlobalInput();
        LoadConfig(); // Loads the config file, override language if specified in config file
        InitializeScreenConfig(); // Applies the screen config data.

        // Must be called after config file is loaded so that the correct language name array is loaded.
        InitializeMaleNameArray(false);
        InitializeFemaleNameArray(false);

        if (LevelEV.ShowFps)
        {
            var fpsCounter = new FrameRateCounter(this);
            Components.Add(fpsCounter);
            fpsCounter.Initialize();
        }

        // Code used to handle game chop.
        _forcedGameTime1 = new GameTime(new TimeSpan(), new TimeSpan(0, 0, 0, 0, (int)(FrameLimit * 1000)));
        _forcedGameTime2 = new GameTime(new TimeSpan(), new TimeSpan(0, 0, 0, 0, (int)(FrameLimit * 1050)));

        base.Initialize(); // Must be called before the enemy list is created so that their content is loaded.

        // Everything below this line can be disabled for release.

        if (LevelEV.CreateRetailVersion == false)
        {
            //Steps for adding enemies to editor.
            //1. Add a new EnemyEditorData object to enemyList with the name of the enemy class as the constructor (right below).
            //2. In the Builder class, add a case statement for the enemy string in BuildEnemy().
            //3. Press F5 to build and run, which should create the EnemyList.xml file that the map editor reads.
            var enemyList = new List<EnemyEditorData>
            {
                new(EnemyType.SKELETON),
                new(EnemyType.KNIGHT),
                new(EnemyType.FIREBALL),
                new(EnemyType.FAIRY),
                new(EnemyType.TURRET),
                new(EnemyType.NINJA),
                new(EnemyType.HORSE),
                new(EnemyType.ZOMBIE),
                new(EnemyType.WOLF),
                new(EnemyType.BALL_AND_CHAIN),
                new(EnemyType.EYEBALL),
                new(EnemyType.BLOB),
                new(EnemyType.SWORD_KNIGHT),
                new(EnemyType.EAGLE),
                new(EnemyType.SHIELD_KNIGHT),
                new(EnemyType.FIRE_WIZARD),
                new(EnemyType.ICE_WIZARD),
                new(EnemyType.EARTH_WIZARD),
                new(EnemyType.BOUNCY_SPIKE),
                new(EnemyType.SPIKE_TRAP),
                new(EnemyType.PLANT),
                new(EnemyType.ENERGON),
                new(EnemyType.SPARK),
                new(EnemyType.SKELETON_ARCHER),
                new(EnemyType.CHICKEN),
                new(EnemyType.PLATFORM),
                new(EnemyType.HOMING_TURRET),
                new(EnemyType.LAST_BOSS),
                new(EnemyType.DUMMY),
                new(EnemyType.STARBURST),
                new(EnemyType.PORTRAIT),
                new(EnemyType.MIMIC),
            };

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

        // KEYBOARD INPUT MAP
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

        // GAMEPAD INPUT MAP
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

    private static void InitializeDefaultConfig()
    {
        GameConfig.FullScreen = false;
        GameConfig.ScreenWidth = 1360;
        GameConfig.ScreenHeight = 768;
        GameConfig.MusicVolume = 1;
        GameConfig.SFXVolume = 0.8f;
        GameConfig.EnableDirectInput = true;
        InputManager.Deadzone = 10;
        GameConfig.ProfileSlot = 0;
        GameConfig.EnableSteamCloud = false;
        GameConfig.ReduceQuality = false;

        InitializeGlobalInput();
    }

    /// <summary>
    ///     LoadContent will be called once per game and is the place to load all of your content.
    /// </summary>
    protected override void LoadContent()
    {
        if (_contentLoaded)
        {
            return;
        }

        _contentLoaded = true;
        LoadAllSpriteFonts();
        LoadAllEffects();
        LoadAllSpritesheets();

        // Initializing Sound Manager.
        SoundManager.Initialize(@"Content\Audio\RogueCastleXACTProj.xgs");
        SoundManager.LoadWaveBank(@"Content\Audio\SFXWaveBank.xwb");
        SoundManager.LoadWaveBank(@"Content\Audio\MusicWaveBank.xwb", true);
        SoundManager.LoadSoundBank(@"Content\Audio\SFXSoundBank.xsb");
        SoundManager.LoadSoundBank(@"Content\Audio\MusicSoundBank.xsb", true);
        SoundManager.GlobalMusicVolume = GameConfig.MusicVolume;
        SoundManager.GlobalSFXVolume = GameConfig.SFXVolume;

        if (InputManager.GamePadIsConnected(PlayerIndex.One))
        {
            InputManager.SetPadType(PlayerIndex.One, PadTypes.GamePad);
        }

        // Creating a generic texture for use.
        GenericTexture = new Texture2D(GraphicsDevice, 1, 1);
        GenericTexture.SetData([Color.White]);

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

        // Must be initialized after the sprites are loaded because the MiscSpritesheet is needed.
        SkillSystem.Initialize();

        var castleZone = new AreaStruct
        {
            Name = "The Grand Entrance",
            LevelType = GameTypes.LevelType.Castle,
            TotalRooms = new Vector2(24, 28), //(17,19),//(20, 22),//(25,35),//(20,25),//(15, 25),
            BossInArea = true,
            SecretRooms = new Vector2(1, 3), //(2, 3),
            BonusRooms = new Vector2(2, 3),
            Color = Color.White,
        };

        var gardenZone = new AreaStruct
        {
            LevelType = GameTypes.LevelType.Garden,
            TotalRooms = new Vector2(23, 27), //(25,29),//(25, 35),//(15, 25),
            BossInArea = true,
            SecretRooms = new Vector2(1, 3),
            BonusRooms = new Vector2(2, 3),
            Color = Color.Green,
        };

        var towerZone = new AreaStruct
        {
            LevelType = GameTypes.LevelType.Tower,
            TotalRooms = new Vector2(23, 27), //(27,31),//(25,29),//(25, 35),//(15, 25),
            BossInArea = true,
            SecretRooms = new Vector2(1, 3),
            BonusRooms = new Vector2(2, 3),
            Color = Color.DarkBlue,
        };

        var dungeonZone = new AreaStruct
        {
            LevelType = GameTypes.LevelType.Dungeon,
            TotalRooms = new Vector2(23, 27), //(29,33),//(25, 29),//(25, 35),//(15, 25),
            BossInArea = true,
            SecretRooms = new Vector2(1, 3),
            BonusRooms = new Vector2(2, 3),
            Color = Color.Red,
        };

        Area1List = [castleZone, gardenZone, towerZone, dungeonZone];
    }

    public void LoadAllSpriteFonts()
    {
        SpriteFontArray.SpriteFontList.Clear();
        PixelArtFont = Content.Load<SpriteFont>(@"Fonts\Arial12");
        SpriteFontArray.SpriteFontList.Add(PixelArtFont);
        PixelArtFontBold = Content.Load<SpriteFont>(@"Fonts\PixelArtFontBold");
        SpriteFontArray.SpriteFontList.Add(PixelArtFontBold);
        EnemyLevelFont = Content.Load<SpriteFont>(@"Fonts\EnemyLevelFont");
        SpriteFontArray.SpriteFontList.Add(EnemyLevelFont);
        EnemyLevelFont.Spacing = -5;
        PlayerLevelFont = Content.Load<SpriteFont>(@"Fonts\PlayerLevelFont");
        SpriteFontArray.SpriteFontList.Add(PlayerLevelFont);
        PlayerLevelFont.Spacing = -7;
        GoldFont = Content.Load<SpriteFont>(@"Fonts\GoldFont");
        SpriteFontArray.SpriteFontList.Add(GoldFont);
        GoldFont.Spacing = -5;
        JunicodeFont = Content.Load<SpriteFont>(@"Fonts\Junicode");
        SpriteFontArray.SpriteFontList.Add(JunicodeFont);
        JunicodeLargeFont = Content.Load<SpriteFont>(@"Fonts\JunicodeLarge");
        SpriteFontArray.SpriteFontList.Add(JunicodeLargeFont);
        JunicodeLargeFont.Spacing = -1;
        HerzogFont = Content.Load<SpriteFont>(@"Fonts\HerzogVonGraf24");
        SpriteFontArray.SpriteFontList.Add(HerzogFont);
        CinzelFont = Content.Load<SpriteFont>(@"Fonts\CinzelFont");
        SpriteFontArray.SpriteFontList.Add(CinzelFont);
        BitFont = Content.Load<SpriteFont>(@"Fonts\BitFont");
        BitFont.DefaultCharacter = '?';
        SpriteFontArray.SpriteFontList.Add(BitFont);
        NotoSansSCFont = Content.Load<SpriteFont>(@"Fonts\NotoSansSC");
        SpriteFontArray.SpriteFontList.Add(NotoSansSCFont);
        RobotoSlabFont = Content.Load<SpriteFont>(@"Fonts\RobotoSlab");
        SpriteFontArray.SpriteFontList.Add(RobotoSlabFont);
    }

    public void LoadAllSpritesheets()
    {
        SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\blacksmithUISpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\enemyFinal2Spritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\enemyFinalSpritesheetBig", false);
        SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\miscSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\traitsCastleSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\castleTerrainSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\playerSpritesheetBig", false);
        SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\titleScreen3Spritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\mapSpritesheetBig", false);
        SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\startingRoomSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\towerTerrainSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\dungeonTerrainSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\profileCardSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\portraitSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\gardenTerrainSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\parallaxBGSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\getItemScreenSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\neoTerrainSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\languageSpritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\language2Spritesheet", false);
        SpriteLibrary.LoadSpritesheet(Content, @"GameSpritesheets\language3Spritesheet", false);

        // Randomizer Specific Sheets
        SpriteLibrary.LoadSpritesheet(Content, @"Randomizer\randomizerSpritesheet", false);
    }

    public void LoadAllEffects()
    {
        // Necessary stuff to create a 2D shader.
        MaskEffect = Content.Load<Effect>(@"Shaders\AlphaMaskShader");

        ShadowEffect = Content.Load<Effect>(@"Shaders\ShadowFX");
        ParallaxEffect = Content.Load<Effect>(@"Shaders\ParallaxFX");
        HSVEffect = Content.Load<Effect>(@"Shaders\HSVShader");
        InvertShader = Content.Load<Effect>(@"Shaders\InvertShader");
        ColourSwapShader = Content.Load<Effect>(@"Shaders\ColourSwapShader");
        RippleEffect = Content.Load<Effect>(@"Shaders\Shockwave");

        RippleEffect.Parameters["mag"].SetValue(2);

        GaussianBlur = new GaussianBlur(this, 1320, 720) {
            Amount = 2f,
            Radius = 7,
            InvertMask = true,
        };

        GaussianBlur.ComputeKernel();
        GaussianBlur.ComputeOffsets();

        // Necessary stuff to create Black/White mask shader.
        BWMaskEffect = Content.Load<Effect>(@"Shaders\BWMaskShader");
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
        if (_gameLoaded == false)
        {
            _gameLoaded = true;
            if (LevelEV.DeleteSaveFile)
            {
                SaveManager.ClearAllFileTypes(true);
                SaveManager.ClearAllFileTypes(false);
            }

            if (LevelEV.LoadSplashScreen)
            {
                ScreenManager.DisplayScreen(ScreenType.CDG_SPLASH, true);
            }
            else
            {
                if (LevelEV.LoadTitleScreen == false)
                {
                    if (LevelEV.RunTestRoom)
                    {
                        ScreenManager.DisplayScreen(ScreenType.LEVEL, true);
                    }
                    else
                    {
                        ScreenManager.DisplayScreen(ScreenType.STARTING_ROOM, true);
                    }
                }
                else
                {
                    ScreenManager.DisplayScreen(ScreenType.TITLE, true);
                }
            }
        }

        // This code forces the game to slow down (instead of chop) if it drops below the frame limit.
        TotalGameTime = (float)gameTime.TotalGameTime.TotalSeconds;

        var gameTimeToUse = gameTime;
        if (gameTime.ElapsedGameTime.TotalSeconds > FrameLimit)
        {
            if (_frameLimitSwap == false)
            {
                _frameLimitSwap = true;
                gameTimeToUse = _forcedGameTime1;
            }
            else
            {
                _frameLimitSwap = false;
                gameTimeToUse = _forcedGameTime2;
            }
        }

        // The screen-manager is updated via the Components.Add call in the game constructor. It is called after this Update() call.
        SoundManager.Update(gameTimeToUse);
        if ((_previouslyActiveCounter <= 0 && IsActive) || LevelEV.EnableOffscreenControl)
        {
            // Only accept input if you have screen focus.
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
            _previouslyActiveCounter = 0.25f;
        }

        if (_previouslyActiveCounter > 0)
        {
            _previouslyActiveCounter -= 0.016f;
        }
    }

    private void HandleDebugInput()
    {
        var languageType = (int)LocaleBuilder.LanguageType;

        // temp disabled since i keep pressing : to enter AP info, then it changes my language to chinese
        // if (InputManager.JustPressed(Keys.OemQuotes, null))
        // {
        //     languageType++;
        //     if (languageType >= (int)LanguageType.MAX)
        //     {
        //         languageType = 0;
        //     }
        //
        //     LocaleBuilder.LanguageType = (LanguageType)languageType;
        //     LocaleBuilder.RefreshAllText();
        //     Console.WriteLine($@"Changing to language type: {(LanguageType)languageType}");
        // }
        // else if (InputManager.JustPressed(Keys.OemSemicolon, null))
        // {
        //     languageType--;
        //     if (languageType < 0)
        //     {
        //         languageType = (int)LanguageType.MAX - 1;
        //     }
        //
        //     LocaleBuilder.LanguageType = (LanguageType)languageType;
        //     LocaleBuilder.RefreshAllText();
        //     Console.WriteLine($@"Changing to language type: {(LanguageType)languageType}");
        // }

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
            GraphicsToggle++;
            if (GraphicsToggle > 1)
            {
                GraphicsToggle = 0;
            }

            switch (GraphicsToggle)
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
        // The screen manager is drawn via the Components.Add call in the game constructor. It is called after this Draw() call.
        ScreenManager.Draw(gameTime);
        base.Draw(gameTime);
    }

    public void InitializeMaleNameArray(bool forceCreate)
    {
        // The name list needs to be reloaded every time the language is from Chinese to another language or vice versa.
        if ((_maleChineseNamesLoaded == false && LocaleBuilder.LanguageType == LanguageType.ChineseSimple) ||
            (_maleChineseNamesLoaded && LocaleBuilder.LanguageType != LanguageType.ChineseSimple))
        {
            forceCreate = true;
        }

        if (NameArray != null && NameArray.Count > 0 && forceCreate == false)
        {
            return;
        }

        if (NameArray != null)
        {
            NameArray.Clear();
        }
        else
        {
            NameArray = new List<string>();
        }

        // Logographic fonts cannot make use of the name change system, otherwise the bitmap spritesheet
        // generated by the system would have to include every single glyph.
        if (LocaleBuilder.LanguageType != LanguageType.ChineseSimple)
        {
            _maleChineseNamesLoaded = false;

            using (var sr = new StreamReader(TitleContainer.OpenStream(Path.Combine(Content.RootDirectory, "HeroNames.txt"))))
            {
                // A test to make sure no special characters are used in the game.
                var junicode = Content.Load<SpriteFont>("Fonts\\Junicode");
                SpriteFontArray.SpriteFontList.Add(junicode);
                var specialCharTest = new TextObj(junicode);

                while (!sr.EndOfStream)
                {
                    var name = sr.ReadLine();
                    var hasSpecialChar = false;

                    try
                    {
                        specialCharTest.Text = name;
                    }
                    catch
                    {
                        hasSpecialChar = true;
                    }

                    if (!name.Contains("//") && hasSpecialChar == false)
                    {
                        NameArray.Add(name);
                    }
                }

                specialCharTest.Dispose();
                SpriteFontArray.SpriteFontList.Remove(junicode);
            }
        }
        else
        {
            _maleChineseNamesLoaded = true;

            // List of male Chinese names.
            NameArray.Add("马如龙");
            NameArray.Add("常遇春");
            NameArray.Add("胡不归");
            NameArray.Add("何千山");
            NameArray.Add("方日中");
            NameArray.Add("谢南山");
            NameArray.Add("慕江南");
            NameArray.Add("赵寒江");
            NameArray.Add("宋乔木");
            NameArray.Add("应楚山");
            NameArray.Add("江山月");
            NameArray.Add("赵慕寒");
            NameArray.Add("万重山");
            NameArray.Add("郭百鸣");
            NameArray.Add("谢武夫");
            NameArray.Add("关中林");
            NameArray.Add("吴深山");
            NameArray.Add("向春风");
            NameArray.Add("牛始旦");
            NameArray.Add("卫东方");
            NameArray.Add("萧北辰");
            NameArray.Add("黃鹤年");
            NameArray.Add("王石柱");
            NameArray.Add("胡江林");
            NameArray.Add("周宇浩");
            NameArray.Add("程向阳");
            NameArray.Add("魏海风");
            NameArray.Add("龚剑辉");
            NameArray.Add("周宇浩");
            NameArray.Add("何汝平");
        }

        // Ensures the name array is greater than 0.
        if (NameArray.Count < 1)
        {
            NameArray.Add("Lee");
            NameArray.Add("Charles");
            NameArray.Add("Lancelot");
        }
    }

    public void InitializeFemaleNameArray(bool forceCreate)
    {
        // The name list needs to be reloaded every time the language is from Chinese to another language or vice versa.
        if ((_femaleChineseNamesLoaded == false && LocaleBuilder.LanguageType == LanguageType.ChineseSimple) ||
            (_femaleChineseNamesLoaded && LocaleBuilder.LanguageType != LanguageType.ChineseSimple))
        {
            forceCreate = true;
        }

        if (FemaleNameArray != null && FemaleNameArray.Count > 0 && forceCreate == false)
        {
            return;
        }

        if (FemaleNameArray != null)
        {
            FemaleNameArray.Clear();
        }
        else
        {
            FemaleNameArray = new List<string>();
        }

        // Logographic fonts cannot make use of the name change system, otherwise the bitmap spritesheet
        // generated by the system would have to include every single glyph.
        if (LocaleBuilder.LanguageType != LanguageType.ChineseSimple)
        {
            _femaleChineseNamesLoaded = false;

            using (var sr = new StreamReader(TitleContainer.OpenStream(Path.Combine(Content.RootDirectory, "HeroineNames.txt"))))
            {
                // A test to make sure no special characters are used in the game.
                var junicode = Content.Load<SpriteFont>("Fonts\\Junicode");
                SpriteFontArray.SpriteFontList.Add(junicode);
                var specialCharTest = new TextObj(junicode);

                while (!sr.EndOfStream)
                {
                    var name = sr.ReadLine();
                    var hasSpecialChar = false;

                    try
                    {
                        specialCharTest.Text = name;
                    }
                    catch
                    {
                        hasSpecialChar = true;
                    }

                    if (!name.Contains("//") && hasSpecialChar == false)
                    {
                        FemaleNameArray.Add(name);
                    }
                }

                specialCharTest.Dispose();
                SpriteFontArray.SpriteFontList.Remove(junicode);
            }
        }
        else
        {
            _femaleChineseNamesLoaded = true;

            // List of female Chinese names.
            FemaleNameArray.Add("水一方");
            FemaleNameArray.Add("刘妙音");
            FemaleNameArray.Add("郭釆薇");
            FemaleNameArray.Add("颜如玉");
            FemaleNameArray.Add("陈巧雅");
            FemaleNameArray.Add("萧玉旋");
            FemaleNameArray.Add("花可秀");
            FemaleNameArray.Add("董小婉");
            FemaleNameArray.Add("李诗诗");
            FemaleNameArray.Add("唐秋香");
            FemaleNameArray.Add("方美人");
            FemaleNameArray.Add("金喜儿");
            FemaleNameArray.Add("达莉萍");
            FemaleNameArray.Add("蔡靜语");
            FemaleNameArray.Add("郭玲玲");
            FemaleNameArray.Add("黃晓莺");
            FemaleNameArray.Add("杜秋娘");
            FemaleNameArray.Add("高媛媛");
            FemaleNameArray.Add("林靜妤");
            FemaleNameArray.Add("凤雨婷");
            FemaleNameArray.Add("徐瑶瑶");
            FemaleNameArray.Add("祝台英");
            FemaleNameArray.Add("郭燕秋");
            FemaleNameArray.Add("江小满");
            FemaleNameArray.Add("项月芳");
            FemaleNameArray.Add("郑云云");
            FemaleNameArray.Add("王琼琼");
            FemaleNameArray.Add("李瓶儿");
            FemaleNameArray.Add("周楚红");
            FemaleNameArray.Add("叶秋菊");
        }

        // Ensures the female name array is greater than 0.
        if (FemaleNameArray.Count < 1)
        {
            FemaleNameArray.Add("Jenny");
            FemaleNameArray.Add("Shanoa");
            FemaleNameArray.Add("Chun Li");
        }
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
        var romanNumeralIndex = playerName.Trim().LastIndexOf(" ", StringComparison.Ordinal);
        if (romanNumeralIndex <= 0)
        {
            return;
        }

        var romanNumeralString = playerName.Substring(romanNumeralIndex + 1);
        // Can't check them all, so only fix the first 40 name duplicates.
        string[] romanNumeralCheckArray =
        [
            "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "XIII", "XIV", "XV",
            "XVI", "XVII", "XVIII", "XIX", "XX", "XXI", "XXII", "XXIII", "XXIV", "XXV", "XXVI", "XXVII",
            "XXVIII", "XXIX", "XXX", "XXXI", "XXXII", "XXXIII", "XXXIV", "XXXV", "XXXVI", "XXXVII", "XXXVIII",
            "XXXIX", "XXXX",
        ];
        foreach (var numeral in romanNumeralCheckArray)
        {
            if (romanNumeralString == numeral)
            {
                playerName = playerName.Substring(0, playerName.Length - romanNumeralString.Length).Trim();
                romanNumeral = romanNumeralString.Trim();
                break;
            }
        }
    }

    public static string NameHelper(
        string playerName,
        string romanNumerals,
        bool isFemale,
        bool forceConversionCheck = false
    )
    {
        if (PlayerStats.RevisionNumber <= 0 || forceConversionCheck)
        {
            ConvertPlayerNameFormat(ref playerName, ref romanNumerals);
        }

        if (isFemale)
        {
            if (LocaleBuilder.LanguageType == LanguageType.ChineseSimple && string.IsNullOrEmpty(romanNumerals))
            {
                return string.Format("LOC_ID_LINEAGE_OBJ_14_NEW_SINGULAR_ZH".GetResourceString(), playerName, "").Trim();
            }

            return string.Format("LOC_ID_LINEAGE_OBJ_14_NEW".GetResourceString(), playerName, romanNumerals).Trim();
        }

        if (LocaleBuilder.LanguageType == LanguageType.ChineseSimple && string.IsNullOrEmpty(romanNumerals))
        {
            return string.Format("LOC_ID_LINEAGE_OBJ_12_NEW_SINGULAR_ZH".GetResourceString(), playerName, "").Trim();
        }

        return string.Format("LOC_ID_LINEAGE_OBJ_12_NEW".GetResourceString(), playerName, romanNumerals).Trim();
    }

    public static string NameHelper()
    {
        return NameHelper(PlayerStats.PlayerName, PlayerStats.RomanNumeral, PlayerStats.IsFemale);
    }

    public void SaveOnExit()
    {
        if (GameIsCorrupt)
        {
            return;
        }

        // Quick hack to fix bug where save file is deleted on closing during splash screen.
        if (ScreenManager.CurrentScreen is CDGSplashScreen or DemoStartScreen)
        {
            return;
        }

        var level = ScreenManager.GetLevelScreen();
        //Special handling to revert your spell if you are in a carnival room.
        if (level is { CurrentRoom: CarnivalShoot1BonusRoom or CarnivalShoot2BonusRoom })
        {
            switch (level.CurrentRoom)
            {
                case CarnivalShoot1BonusRoom room:
                    room.UnequipPlayer();
                    break;
                case CarnivalShoot2BonusRoom room:
                    room.UnequipPlayer();
                    break;
            }
        }

        // A check to make sure challenge rooms do not override player save data.
        if (level?.CurrentRoom is ChallengeBossRoomObj challengeRoom)
        {
            // Make sure this is loaded before upgrade data, otherwise player equipment will be overridden.
            challengeRoom.LoadPlayerData();
            SaveManager.LoadFiles(level, SaveType.UpgradeData);
            level.Player.CurrentHealth = challengeRoom.StoredHP;
            level.Player.CurrentMana = challengeRoom.StoredMP;
        }

        // Special check in case the user closes the program while in the game over screen to reset the traits.
        if (ScreenManager.CurrentScreen is GameOverScreen)
        {
            PlayerStats.Traits = (TraitType.NONE, TraitType.NONE);
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
            if (
                PlayerStats.TutorialComplete &&
                level != null &&
                level.CurrentRoom.Name != "Start" &&
                level.CurrentRoom.Name != "Ending" &&
                level.CurrentRoom.Name != "Tutorial"
            )
            {
                SaveManager.SaveFiles(SaveType.MapData, SaveType.Archipelago);
            }
        }
    }

    public List<Vector2> GetSupportedResolutions()
    {
        var list = new List<Vector2>();
        foreach (var mode in GraphicsDevice.Adapter.SupportedDisplayModes)
        {
            //if (mode.AspectRatio > 1.7f)
            // Restricts the resolution to below 2048 (which is max supported texture size).
            if (mode.Width >= 2000 || mode.Height >= 2000)
            {
                continue;
            }

            var res = new Vector2(mode.Width, mode.Height);
            if (list.Contains(res) == false)
            {
                list.Add(new Vector2(mode.Width, mode.Height));
            }
        }

        return list;
    }

    public static void SaveConfig()
    {
        Console.WriteLine(@"Saving Config file");

        if (!Directory.Exists(Program.OSDir))
        {
            Directory.CreateDirectory(Program.OSDir);
        }

        var configFilePath = Path.Combine(Program.OSDir, "GameConfig.ini");

        using var writer = new StreamWriter(configFilePath, false);
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
        writer.WriteLine("MusicVol=" + $"{GameConfig.MusicVolume:F2}");
        writer.WriteLine("SFXVol=" + $"{GameConfig.SFXVolume:F2}");
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
        writer.WriteLine("Language=" + LocaleBuilder.LanguageType);
        writer.WriteLine();
        if (GameConfig.UnlockTraitor > 0)
        {
            writer.WriteLine("UnlockTraitor=" + GameConfig.UnlockTraitor);
        }

        writer.Close();
    }

    public static void LoadConfig()
    {
        Console.WriteLine(@"Loading Config file");
        InitializeDefaultConfig(); // Initialize a default config first in case new config data is added in the future.
        try
        {
            var configFilePath = Path.Combine(Program.OSDir, "GameConfig.ini");

            using var reader = new StreamReader(configFilePath);
            var ci = CultureInfo.InvariantCulture;

            while (reader.ReadLine() is { } line)
            {
                var titleIndex = line.IndexOf("=", StringComparison.Ordinal);
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
                            GameConfig.ProfileSlot = 0;
                            break;
                        case "KeyUP":
                            GlobalInput.KeyList[InputMapType.PLAYER_UP1] =
                                (Keys)Enum.Parse(typeof(Keys), lineValue);
                            break;
                        case "KeyDOWN":
                            GlobalInput.KeyList[InputMapType.PLAYER_DOWN1] =
                                (Keys)Enum.Parse(typeof(Keys), lineValue);
                            break;
                        case "KeyLEFT":
                            GlobalInput.KeyList[InputMapType.PLAYER_LEFT1] =
                                (Keys)Enum.Parse(typeof(Keys), lineValue);
                            break;
                        case "KeyRIGHT":
                            GlobalInput.KeyList[InputMapType.PLAYER_RIGHT1] =
                                (Keys)Enum.Parse(typeof(Keys), lineValue);
                            break;
                        case "KeyATTACK":
                            GlobalInput.KeyList[InputMapType.PLAYER_ATTACK] =
                                (Keys)Enum.Parse(typeof(Keys), lineValue);
                            break;
                        case "KeyJUMP":
                            GlobalInput.KeyList[InputMapType.PLAYER_JUMP1] =
                                (Keys)Enum.Parse(typeof(Keys), lineValue);
                            break;
                        case "KeySPECIAL":
                            GlobalInput.KeyList[InputMapType.PLAYER_BLOCK] =
                                (Keys)Enum.Parse(typeof(Keys), lineValue);
                            break;
                        case "KeyDASHLEFT":
                            GlobalInput.KeyList[InputMapType.PLAYER_DASHLEFT] =
                                (Keys)Enum.Parse(typeof(Keys), lineValue);
                            break;
                        case "KeyDASHRIGHT":
                            GlobalInput.KeyList[InputMapType.PLAYER_DASHRIGHT] =
                                (Keys)Enum.Parse(typeof(Keys), lineValue);
                            break;
                        case "KeySPELL1":
                            GlobalInput.KeyList[InputMapType.PLAYER_SPELL1] =
                                (Keys)Enum.Parse(typeof(Keys), lineValue);
                            break;
                        case "ButtonUP":
                            GlobalInput.ButtonList[InputMapType.PLAYER_UP1] =
                                (Buttons)Enum.Parse(typeof(Buttons), lineValue);
                            break;
                        case "ButtonDOWN":
                            GlobalInput.ButtonList[InputMapType.PLAYER_DOWN1] =
                                (Buttons)Enum.Parse(typeof(Buttons), lineValue);
                            break;
                        case "ButtonLEFT":
                            GlobalInput.ButtonList[InputMapType.PLAYER_LEFT1] =
                                (Buttons)Enum.Parse(typeof(Buttons), lineValue);
                            break;
                        case "ButtonRIGHT":
                            GlobalInput.ButtonList[InputMapType.PLAYER_RIGHT1] =
                                (Buttons)Enum.Parse(typeof(Buttons), lineValue);
                            break;
                        case "ButtonATTACK":
                            GlobalInput.ButtonList[InputMapType.PLAYER_ATTACK] =
                                (Buttons)Enum.Parse(typeof(Buttons), lineValue);
                            break;
                        case "ButtonJUMP":
                            GlobalInput.ButtonList[InputMapType.PLAYER_JUMP1] =
                                (Buttons)Enum.Parse(typeof(Buttons), lineValue);
                            break;
                        case "ButtonSPECIAL":
                            GlobalInput.ButtonList[InputMapType.PLAYER_BLOCK] =
                                (Buttons)Enum.Parse(typeof(Buttons), lineValue);
                            break;
                        case "ButtonDASHLEFT":
                            GlobalInput.ButtonList[InputMapType.PLAYER_DASHLEFT] =
                                (Buttons)Enum.Parse(typeof(Buttons), lineValue);
                            break;
                        case "ButtonDASHRIGHT":
                            GlobalInput.ButtonList[InputMapType.PLAYER_DASHRIGHT] =
                                (Buttons)Enum.Parse(typeof(Buttons), lineValue);
                            break;
                        case "ButtonSPELL1":
                            GlobalInput.ButtonList[InputMapType.PLAYER_SPELL1] =
                                (Buttons)Enum.Parse(typeof(Buttons), lineValue);
                            break;
                        case "Language":
                            LocaleBuilder.LanguageType = (LanguageType)Enum.Parse(typeof(LanguageType), lineValue);
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
        catch
        {
            //If exception occurred, then no file was found and default config must be created.
            Console.WriteLine(@"Config File Not Found. Creating Default Config File.");
            InitializeDefaultConfig();
            SaveConfig();
        }
    }

    public void InitializeScreenConfig()
    {
        if (Environment.GetEnvironmentVariable("SteamTenfoot") == "1" || Environment.GetEnvironmentVariable("SteamDeck") == "1")
        {
            // We are asked to override resolution settings in Big Picture modes
            var mode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            Graphics.PreferredBackBufferWidth = mode.Width;
            Graphics.PreferredBackBufferHeight = mode.Height;
            Graphics.IsFullScreen = true;
            Graphics.ApplyChanges();
        }
        else
        {
            Graphics.PreferredBackBufferWidth = GameConfig.ScreenWidth;
            Graphics.PreferredBackBufferHeight = GameConfig.ScreenHeight;
            if ((Graphics.IsFullScreen && GameConfig.FullScreen == false) || (Graphics.IsFullScreen == false && GameConfig.FullScreen))
            {
                Graphics.ToggleFullScreen();
            }
            else
            {
                Graphics.ApplyChanges();
            }
        }

        // No need to call Graphics.ApplyChanges() since ToggleFullScreen() implicitly calls it.
        ScreenManager.ForceResolutionChangeCheck();
    }

    public static void ChangeBitmapLanguage(SpriteObj sprite, string spriteName)
    {
        switch (LocaleBuilder.LanguageType)
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
            case LanguageType.PortugueseBrazil:
                sprite.ChangeSprite(spriteName + "_BR");
                break;
            case LanguageType.SpanishSpain:
                sprite.ChangeSprite(spriteName + "_SP");
                break;
            case LanguageType.ChineseSimple:
                sprite.ChangeSprite(spriteName + "_ZH");
                break;
        }
    }

    public struct SettingStruct
    {
        public int ScreenWidth;
        public int ScreenHeight;
        public bool FullScreen;
        public float MusicVolume;
        public float SFXVolume;
        public bool QuickDrop;
        public bool EnableDirectInput;
        public byte ProfileSlot;
        public bool ReduceQuality;
        public bool EnableSteamCloud;
        public byte UnlockTraitor;
    }
}
