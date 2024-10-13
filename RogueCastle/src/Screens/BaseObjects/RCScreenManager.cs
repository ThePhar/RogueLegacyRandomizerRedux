using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.EVs;
using Tweener;
using Tweener.Ease;

namespace RogueCastle.Screens.BaseObjects;

public class RCScreenManager(Game game) : ScreenManager(game)
{
    private readonly List<Screen>         _screenCleanupList = new();
    private          BlacksmithScreen     _blacksmithScreen;
    private          SpriteObj            _blackTransitionIn, _blackScreen, _blackTransitionOut;
    private          CreditsScreen        _creditsScreen;
    private          DeathDefiedScreen    _deathDefyScreen;
    private          DiaryEntryScreen     _diaryEntryScreen;
    private          EnchantressScreen    _enchantressScreen;
    private          DiaryFlashbackScreen _flashbackScreen;
    private          GameOverBossScreen   _gameOverBossScreen;
    private          GameOverScreen       _gameOverScreen;
    private          GetItemScreen        _getItemScreen;
    private          bool                 _isWipeTransitioning;
    private          MapScreen            _mapScreen;
    private          OptionsScreen        _optionsScreen;
    private          PauseScreen          _pauseScreen;
    private          ProfileCardScreen    _profileCardScreen;
    private          ProfileSelectScreen  _profileSelectScreen;

    private RandomizerScreen  _randomizerScreen;
    private SkillUnlockScreen _skillUnlockScreen;
    private TextScreen        _textScreen;
    private VirtualScreen     _virtualScreen;

    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public int CurrentScreenType { get; private set; }

    public bool InventoryVisible { get; } = false;

    public RenderTarget2D RenderTarget => _virtualScreen.RenderTarget;

    public DialogueScreen DialogueScreen { get; private set; }

    public SkillScreen SkillScreen { get; private set; }

    public PlayerObj Player { get; private set; }

    public bool IsTransitioning { get; private set; }

    public override void Initialize()
    {
        InitializeScreens();
        base.Initialize(); // Camera gets initialized here.

        _virtualScreen = new VirtualScreen(GlobalEV.SCREEN_WIDTH, GlobalEV.SCREEN_HEIGHT, Camera.GraphicsDevice);
        Game.Window.ClientSizeChanged += Window_ClientSizeChanged;
        Game.Deactivated += PauseGame;
    }

    public void PauseGame(object sender, EventArgs e)
    {
        if (CurrentScreen is ProceduralLevelScreen level && level.CurrentRoom is EndingRoomObj == false)
        {
            DisplayScreen(ScreenType.Pause, true);
        }
    }

    public void ReinitializeContent(object sender, EventArgs e)
    {
        _virtualScreen.ReinitializeRTs(Game.GraphicsDevice);

        foreach (var screen in m_screenArray)
        {
            screen.DisposeRTs();
        }

        foreach (var screen in m_screenArray)
        {
            screen.ReinitializeRTs();
        }
    }

    public void ReinitializeCamera(GraphicsDevice graphicsDevice)
    {
        m_camera.Dispose();
        m_camera = new Camera2D(graphicsDevice, EngineEV.ScreenWidth, EngineEV.ScreenHeight);
    }

    public void InitializeScreens()
    {
        if (_gameOverScreen != null)
        {
            _screenCleanupList.Add(_gameOverScreen);
        }

        _gameOverScreen = new GameOverScreen();

        if (SkillScreen != null)
        {
            _screenCleanupList.Add(SkillScreen);
        }

        SkillScreen = new SkillScreen();

        if (_blacksmithScreen != null)
        {
            _screenCleanupList.Add(_blacksmithScreen);
        }

        _blacksmithScreen = new BlacksmithScreen();

        if (_getItemScreen != null)
        {
            _screenCleanupList.Add(_getItemScreen);
        }

        _getItemScreen = new GetItemScreen();

        if (_enchantressScreen != null)
        {
            _screenCleanupList.Add(_enchantressScreen);
        }

        _enchantressScreen = new EnchantressScreen();

        if (DialogueScreen != null)
        {
            _screenCleanupList.Add(DialogueScreen);
        }

        DialogueScreen = new DialogueScreen();

        if (_pauseScreen != null)
        {
            _screenCleanupList.Add(_pauseScreen);
        }

        _pauseScreen = new PauseScreen();

        if (_optionsScreen != null)
        {
            _screenCleanupList.Add(_optionsScreen);
        }

        _optionsScreen = new OptionsScreen();

        if (_profileCardScreen != null)
        {
            _screenCleanupList.Add(_profileCardScreen);
        }

        _profileCardScreen = new ProfileCardScreen();

        if (_creditsScreen != null)
        {
            _screenCleanupList.Add(_creditsScreen);
        }

        _creditsScreen = new CreditsScreen();

        if (_skillUnlockScreen != null)
        {
            _screenCleanupList.Add(_skillUnlockScreen);
        }

        _skillUnlockScreen = new SkillUnlockScreen();

        if (_diaryEntryScreen != null)
        {
            _screenCleanupList.Add(_diaryEntryScreen);
        }

        _diaryEntryScreen = new DiaryEntryScreen();

        if (_deathDefyScreen != null)
        {
            _screenCleanupList.Add(_deathDefyScreen);
        }

        _deathDefyScreen = new DeathDefiedScreen();

        if (_textScreen != null)
        {
            _screenCleanupList.Add(_textScreen);
        }

        _textScreen = new TextScreen();

        if (_flashbackScreen != null)
        {
            _screenCleanupList.Add(_flashbackScreen);
        }

        _flashbackScreen = new DiaryFlashbackScreen();

        if (_gameOverBossScreen != null)
        {
            _screenCleanupList.Add(_gameOverBossScreen);
        }

        _gameOverBossScreen = new GameOverBossScreen();

        if (_profileSelectScreen != null)
        {
            _screenCleanupList.Add(_profileSelectScreen);
        }

        _profileSelectScreen = new ProfileSelectScreen();

        if (_randomizerScreen != null)
        {
            _screenCleanupList.Add(_randomizerScreen);
        }

        _randomizerScreen = new RandomizerScreen();
    }

    public override void LoadContent()
    {
        _gameOverScreen.LoadContent();
        SkillScreen.LoadContent();
        _blacksmithScreen.LoadContent();
        _getItemScreen.LoadContent();
        _enchantressScreen.LoadContent();
        DialogueScreen.LoadContent();
        _pauseScreen.LoadContent();
        _optionsScreen.LoadContent();
        _profileCardScreen.LoadContent();
        _creditsScreen.LoadContent();
        _skillUnlockScreen.LoadContent();
        _diaryEntryScreen.LoadContent();
        _deathDefyScreen.LoadContent();
        _textScreen.LoadContent();
        _flashbackScreen.LoadContent();
        _gameOverBossScreen.LoadContent();
        _profileSelectScreen.LoadContent();
        _randomizerScreen.LoadContent();

        if (IsContentLoaded == false)
        {
            _blackTransitionIn = new SpriteObj("Blank_Sprite")
            {
                Rotation = 15,
                TextureColor = Color.Black,
                ForceDraw = true,
            };
            _blackTransitionIn.Scale = new Vector2(1320 / _blackTransitionIn.Width, 2000 / _blackTransitionIn.Height);

            _blackScreen = new SpriteObj("Blank_Sprite")
            {
                TextureColor = Color.Black,
                ForceDraw = true,
            };
            _blackScreen.Scale = new Vector2(1320 / _blackScreen.Width, 720 / _blackScreen.Height);

            _blackTransitionOut = new SpriteObj("Blank_Sprite")
            {
                Rotation = 15,
                TextureColor = Color.Black,
                ForceDraw = true,
            };
            _blackTransitionOut.Scale = new Vector2(1320 / _blackTransitionOut.Width, 2000 / _blackTransitionOut.Height);

            _blackTransitionIn.X = 0;
            _blackTransitionIn.X = 1320 - _blackTransitionIn.Bounds.Left;
            _blackScreen.X = _blackTransitionIn.X;
            _blackTransitionOut.X = _blackScreen.X + _blackScreen.Width;
            _blackTransitionIn.Visible = false;
            _blackScreen.Visible = false;
            _blackTransitionOut.Visible = false;

            LoadPlayer();
        }

        base.LoadContent();
    }

    private void Window_ClientSizeChanged(object sender, EventArgs e)
    {
        _virtualScreen.PhysicalResolutionChanged();
        EngineEV.RefreshEngine(Camera.GraphicsDevice);
        Console.WriteLine(@"resolution changed");
    }

    private void LoadPlayer()
    {
        if (Player != null)
        {
            return;
        }

        Player = new PlayerObj("PlayerIdle_Character", PlayerIndex.One, (Game as Game)!.PhysicsManager, null, Game as Game)
        {
            Position = new Vector2(200, 200),
        };
        Player.Initialize();
    }

    public void DisplayScreen(int screenType, bool pauseOtherScreens, List<object> objList = null)
    {
        LoadPlayer();

        if (pauseOtherScreens)
        {
            // This needs to be changed so that the ScreenManager holds a reference to the ProceduralLevelScreen.
            foreach (var screen in GetScreens())
            {
                if (screen == CurrentScreen)
                {
                    screen.PauseScreen();
                    break;
                }
            }
        }

        CurrentScreenType = screenType;
        //if (CurrentScreen != null && !(CurrentScreen is ProceduralLevelScreen))
        //    RemoveScreen(CurrentScreen, false);

        // This currently has no checks to see if the screen is already in the screen manager's screen list.
        switch (screenType)
        {
            case ScreenType.CDGSplash:
            case ScreenType.BlitWorks:
            case ScreenType.Title:
            case ScreenType.TitleWhite:
            case ScreenType.StartingRoom:
            case ScreenType.DemoStart:
            case ScreenType.DemoEnd:
            case ScreenType.Lineage:
                LoadScreen((byte) screenType, true);
                break;
            case ScreenType.Level:
                if (RogueCastle.Game.PlayerStats.LockCastle || CurrentScreen is ProceduralLevelScreen == false)
                {
                    LoadScreen((byte) screenType, true);
                }
                else
                {
                    LoadScreen((byte) screenType, false);
                }

                break;
            case ScreenType.Skill:
                AddScreen(SkillScreen, null);
                break;
            case ScreenType.GameOver:
                _gameOverScreen.PassInData(objList);
                AddScreen(_gameOverScreen, null);
                break;
            case ScreenType.GameOverBoss:
                _gameOverBossScreen.PassInData(objList);
                AddScreen(_gameOverBossScreen, null);
                break;
            case ScreenType.Blacksmith:
                _blacksmithScreen.Player = Player;
                AddScreen(_blacksmithScreen, null);
                break;
            case ScreenType.GetItem:
                _getItemScreen.PassInData(objList);
                AddScreen(_getItemScreen, null);
                break;
            case ScreenType.Enchantress:
                _enchantressScreen.Player = Player;
                AddScreen(_enchantressScreen, null);
                break;
            case ScreenType.Dialogue:
                AddScreen(DialogueScreen, null);
                break;
            case ScreenType.Map:
                _mapScreen.SetPlayer(Player);
                AddScreen(_mapScreen, null);
                break;
            case ScreenType.Pause:
                GetLevelScreen().CurrentRoom.DarkenRoom();
                AddScreen(_pauseScreen, null);
                break;
            case ScreenType.Options:
                _optionsScreen.PassInData(objList);
                AddScreen(_optionsScreen, null);
                break;
            case ScreenType.ProfileCard:
                AddScreen(_profileCardScreen, null);
                break;
            case ScreenType.Credits:
                LoadScreen(ScreenType.Credits, true);
                break;
            case ScreenType.SkillUnlock:
                _skillUnlockScreen.PassInData(objList);
                AddScreen(_skillUnlockScreen, null);
                break;
            case ScreenType.DiaryEntry:
                AddScreen(_diaryEntryScreen, null);
                break;
            case ScreenType.DeathDefy:
                AddScreen(_deathDefyScreen, null);
                break;
            case ScreenType.Text:
                _textScreen.PassInData(objList);
                AddScreen(_textScreen, null);
                break;
            case ScreenType.TutorialRoom:
                LoadScreen(ScreenType.TutorialRoom, true);
                break;
            case ScreenType.Ending:
                GetLevelScreen().CameraLockedToPlayer = false;
                GetLevelScreen().DisableRoomTransitioning = true;
                Player.Position = new Vector2(100, 100); // HHHAACCK
                LoadScreen(ScreenType.Ending, true);
                break;
            case ScreenType.DiaryFlashback:
                AddScreen(_flashbackScreen, null);
                break;
            case ScreenType.ProfileSelect:
                AddScreen(_profileSelectScreen, null);
                break;
            case ScreenType.Randomizer:
                AddScreen(_randomizerScreen, null);
                break;
        }

        if (_isWipeTransitioning)
        {
            EndWipeTransition();
        }
    }

    public void AddRoomsToMap(List<RoomObj> roomList)
    {
        _mapScreen.AddRooms(roomList);
    }

    public void AddIconsToMap(List<RoomObj> roomList)
    {
        _mapScreen.AddAllIcons(roomList);
    }

    public void RefreshMapScreenChestIcons(RoomObj room)
    {
        _mapScreen.RefreshMapChestIcons(room);
    }

    public void ActivateMapScreenTeleporter()
    {
        _mapScreen.IsTeleporter = true;
    }

    public void HideCurrentScreen()
    {
        RemoveScreen(CurrentScreen, false);

        if (CurrentScreen is ProceduralLevelScreen level)
        {
            level.UnpauseScreen();
        }

        if (_isWipeTransitioning)
        {
            EndWipeTransition();
        }
    }

    public void ForceResolutionChangeCheck()
    {
        _virtualScreen.PhysicalResolutionChanged();
        EngineEV.RefreshEngine(Game.GraphicsDevice);
    }

    // This is overridden so that a custom LoadScreen can be passed in.
    private void LoadScreen(byte screenType, bool wipeTransition)
    {
        CurrentScreenType = ScreenType.Loading;
        foreach (var screen in m_screenArray)
        {
            screen.DrawIfCovered = true;
            if (screen is ProceduralLevelScreen levelScreen)
            {
                Player.AttachLevel(levelScreen);
                levelScreen.Player = Player;

                AttachMap(levelScreen);
            }
        }

        // Double check this.  This doesn't seem right.
        if (_gameOverScreen != null)
        {
            InitializeScreens();
            LoadContent(); // Since all screens are disposed, their content needs to be loaded again. Hacked.
        }

        // Create and activate the loading screen.
        var loadingScreen = new LoadingScreen(screenType, wipeTransition);
        IsTransitioning = true;

        AddScreen(loadingScreen, PlayerIndex.One);

        // This has been moved to PerformCleanUp().
        //GC.Collect(); // Should this be called? Most people say don't, but I want to ensure that collection occurs during the loading screen, not some random moment later.
    }

    public void PerformCleanUp()
    {
        foreach (var screen in _screenCleanupList)
        {
            if (screen.IsDisposed == false && screen.IsContentLoaded)
            {
                screen.Dispose();
            }
        }

        _screenCleanupList.Clear();

        GC.Collect(); // Should this be called? Most people say don't, but I want to ensure that collection occurs during the loading screen, not some random moment later.
    }

    public void LoadingComplete(int screenType)
    {
        CurrentScreenType = screenType;
    }

    public override void RemoveScreen(Screen screen, bool disposeScreen)
    {
        if (screen is LoadingScreen)
        {
            IsTransitioning = false;
        }

        base.RemoveScreen(screen, disposeScreen);
    }

    public void AttachMap(ProceduralLevelScreen level)
    {
        if (_mapScreen != null)
        {
            _mapScreen.Dispose();
        }

        _mapScreen = new MapScreen(level);
    }

    public override void Update(GameTime gameTime)
    {
        _virtualScreen.Update();
        if (IsTransitioning == false)
        {
            base.Update(gameTime);
        }
        else
        {
            Camera.GameTime = gameTime;
            if (CurrentScreen != null)
            {
                CurrentScreen.Update(gameTime);
                CurrentScreen.HandleInput();
            }
        }
    }

    public override void Draw(GameTime gameTime)
    {
        _virtualScreen.BeginCapture();
        Camera.GraphicsDevice.Clear(Color.Black);

        // Must be called after BeginCapture(), in case graphics virtualization fails and the device is hard reset.
        Camera.GameTime ??= gameTime;

        base.Draw(gameTime);

        _virtualScreen.EndCapture();
        Camera.GraphicsDevice.Clear(Color.Black);
        Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
        _virtualScreen.Draw(Camera);
        Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        _blackTransitionIn.Draw(Camera);
        _blackTransitionOut.Draw(Camera);
        _blackScreen.Draw(Camera);
        Camera.End();

        if (LevelEV.EnableDebugInput)
        {
            Camera.Begin();
            var forcedGenderString = "None";
            switch (RogueCastle.Game.PlayerStats.ForceLanguageGender)
            {
                case 1:
                    forcedGenderString = "Male";
                    break;
                case 2:
                    forcedGenderString = "Female";
                    break;
            }

            var godModeString = "Off";
            if (RogueCastle.Game.PlayerStats.GodMode)
            {
                godModeString = "On";
            }

            Camera.DrawString(RogueCastle.Game.PixelArtFont, "Forced Gender Language: " + forcedGenderString,
                new Vector2(10, 10), Color.White);
            Camera.DrawString(RogueCastle.Game.PixelArtFont, "God Mode: " + godModeString, new Vector2(10, 30),
                Color.White);
            Camera.End();
        }
    }

    public void StartWipeTransition()
    {
        _isWipeTransitioning = true;
        _blackTransitionIn.Visible = true;
        _blackScreen.Visible = true;
        _blackTransitionOut.Visible = true;

        _blackTransitionIn.X = 0;
        _blackTransitionOut.Y = -500;
        _blackTransitionIn.X = 1320 - _blackTransitionIn.Bounds.Left;
        _blackScreen.X = _blackTransitionIn.X;
        _blackTransitionOut.X = _blackScreen.X + _blackScreen.Width;

        Tween.By(_blackTransitionIn, 0.15f, Quad.EaseInOut, "X", $"{-_blackTransitionIn.X}");
        Tween.By(_blackScreen, 0.15f, Quad.EaseInOut, "X", $"{-_blackTransitionIn.X}");
        Tween.By(_blackTransitionOut, 0.15f, Quad.EaseInOut, "X", $"{-_blackTransitionIn.X}");
        //Tween.AddEndHandlerToLastTween(this, "EndWipeTransition");
    }

    public void EndWipeTransition()
    {
        _isWipeTransitioning = false;

        _blackTransitionOut.Y = -500;
        Tween.By(_blackTransitionIn, 0.25f, Quad.EaseInOut, "X", "-3000");
        Tween.By(_blackScreen, 0.25f, Quad.EaseInOut, "X", "-3000");
        Tween.By(_blackTransitionOut, 0.25f, Quad.EaseInOut, "X", "-3000");
    }

    public void UpdatePauseScreenIcons()
    {
        _pauseScreen.UpdatePauseScreenIcons();
    }

    public ProceduralLevelScreen GetLevelScreen()
    {
        foreach (var screen in GetScreens())
        {
            if (screen is ProceduralLevelScreen level)
            {
                return level;
            }
        }

        return null;
    }
}
