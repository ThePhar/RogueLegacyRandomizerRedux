using System;
using System.Collections.Generic;
using System.Linq;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.EnvironmentVariables;
using RogueCastle.GameStructs;
using Tweener;
using Tweener.Ease;

namespace RogueCastle.Screens.BaseScreens;

public class RCScreenManager(Game game) : ScreenManager(game) {
    private readonly List<Screen> _screenCleanupList = [];
    private BlacksmithScreen _blacksmithScreen;
    private SpriteObj _blackTransitionIn, _blackScreen, _blackTransitionOut;
    private CreditsScreen _creditsScreen;
    private DeathDefiedScreen _deathDefyScreen;
    private DiaryEntryScreen _diaryEntryScreen;
    private EnchantressScreen _enchantressScreen;
    private DiaryFlashbackScreen _flashbackScreen;
    private GameOverBossScreen _gameOverBossScreen;
    private GameOverScreen _gameOverScreen;
    private GetItemScreen _getItemScreen;
    private bool _isWipeTransitioning;
    private MapScreen _mapScreen;
    private OptionsScreen _optionsScreen;
    private PauseScreen _pauseScreen;
    private ProfileCardScreen _profileCardScreen;
    private ProfileSelectScreen _profileSelectScreen;
    private RandomizerMenuScreen _randomizerMenuScreen;
    private SkillUnlockScreen _skillUnlockScreen;
    private TextScreen _textScreen;
    private VirtualScreen _virtualScreen;

    public int CurrentScreenType { get; private set; }

    public RenderTarget2D RenderTarget => _virtualScreen.RenderTarget;

    public DialogueScreen DialogueScreen { get; private set; }

    public SkillScreen SkillScreen { get; private set; }

    public PlayerObj Player { get; private set; }

    public bool IsTransitioning { get; private set; }

    public override void Initialize() {
        InitializeScreens();
        base.Initialize(); // Camera gets initialized here.

        _virtualScreen = new VirtualScreen(GlobalEV.SCREEN_WIDTH, GlobalEV.SCREEN_HEIGHT, Camera.GraphicsDevice);
        Game.Window.ClientSizeChanged += Window_ClientSizeChanged;
        Game.Deactivated += PauseGame;
    }

    public void PauseGame(object sender, EventArgs e) {
        if (CurrentScreen is ProceduralLevelScreen level && level.CurrentRoom is EndingRoomObj == false) {
            DisplayScreen(ScreenType.PAUSE, true);
        }
    }

    public void ReinitializeContent(object sender, EventArgs e) {
        _virtualScreen.ReinitializeRTs(Game.GraphicsDevice);

        foreach (var screen in m_screenArray) {
            screen.DisposeRTs();
        }

        foreach (var screen in m_screenArray) {
            screen.ReinitializeRTs();
        }
    }

    public void ReinitializeCamera(GraphicsDevice graphicsDevice) {
        m_camera.Dispose();
        m_camera = new Camera2D(graphicsDevice, EngineEV.ScreenWidth, EngineEV.ScreenHeight);
    }

    public void InitializeScreens() {
        if (_gameOverScreen != null) {
            _screenCleanupList.Add(_gameOverScreen);
        }

        _gameOverScreen = new GameOverScreen();

        if (SkillScreen != null) {
            _screenCleanupList.Add(SkillScreen);
        }

        SkillScreen = new SkillScreen();

        if (_blacksmithScreen != null) {
            _screenCleanupList.Add(_blacksmithScreen);
        }

        _blacksmithScreen = new BlacksmithScreen();

        if (_getItemScreen != null) {
            _screenCleanupList.Add(_getItemScreen);
        }

        _getItemScreen = new GetItemScreen();

        if (_enchantressScreen != null) {
            _screenCleanupList.Add(_enchantressScreen);
        }

        _enchantressScreen = new EnchantressScreen();

        if (DialogueScreen != null) {
            _screenCleanupList.Add(DialogueScreen);
        }

        DialogueScreen = new DialogueScreen();

        if (_pauseScreen != null) {
            _screenCleanupList.Add(_pauseScreen);
        }

        _pauseScreen = new PauseScreen();

        if (_optionsScreen != null) {
            _screenCleanupList.Add(_optionsScreen);
        }

        _optionsScreen = new OptionsScreen();

        if (_profileCardScreen != null) {
            _screenCleanupList.Add(_profileCardScreen);
        }

        _profileCardScreen = new ProfileCardScreen();

        if (_creditsScreen != null) {
            _screenCleanupList.Add(_creditsScreen);
        }

        _creditsScreen = new CreditsScreen();

        if (_skillUnlockScreen != null) {
            _screenCleanupList.Add(_skillUnlockScreen);
        }

        _skillUnlockScreen = new SkillUnlockScreen();

        if (_diaryEntryScreen != null) {
            _screenCleanupList.Add(_diaryEntryScreen);
        }

        _diaryEntryScreen = new DiaryEntryScreen();

        if (_deathDefyScreen != null) {
            _screenCleanupList.Add(_deathDefyScreen);
        }

        _deathDefyScreen = new DeathDefiedScreen();

        if (_textScreen != null) {
            _screenCleanupList.Add(_textScreen);
        }

        _textScreen = new TextScreen();

        if (_flashbackScreen != null) {
            _screenCleanupList.Add(_flashbackScreen);
        }

        _flashbackScreen = new DiaryFlashbackScreen();

        if (_gameOverBossScreen != null) {
            _screenCleanupList.Add(_gameOverBossScreen);
        }

        _gameOverBossScreen = new GameOverBossScreen();

        if (_profileSelectScreen != null) {
            _screenCleanupList.Add(_profileSelectScreen);
        }

        _profileSelectScreen = new ProfileSelectScreen();

        if (_randomizerMenuScreen != null) {
            _screenCleanupList.Add(_randomizerMenuScreen);
        }

        _randomizerMenuScreen = new RandomizerMenuScreen();
    }

    public override void LoadContent() {
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
        _randomizerMenuScreen.LoadContent();

        if (IsContentLoaded == false) {
            _blackTransitionIn = new SpriteObj("Blank_Sprite") {
                Rotation = 15,
                TextureColor = Color.Black,
                ForceDraw = true,
            };
            _blackTransitionIn.Scale = new Vector2(1320 / _blackTransitionIn.Width, 2000 / _blackTransitionIn.Height);

            _blackScreen = new SpriteObj("Blank_Sprite") {
                TextureColor = Color.Black,
                ForceDraw = true,
            };
            _blackScreen.Scale = new Vector2(1320 / _blackScreen.Width, 720 / _blackScreen.Height);

            _blackTransitionOut = new SpriteObj("Blank_Sprite") {
                Rotation = 15,
                TextureColor = Color.Black,
                ForceDraw = true,
            };
            _blackTransitionOut.Scale = new Vector2(1320 / _blackTransitionOut.Width, 2000 / _blackTransitionOut.Height);

            _blackTransitionIn.X = 0;
            _blackTransitionIn.X = 1320 - _blackTransitionIn.Bounds.Left;
            _blackTransitionOut.X = _blackScreen.X + _blackScreen.Width;
            _blackScreen.X = _blackTransitionIn.X;
            _blackTransitionIn.Visible = false;
            _blackScreen.Visible = false;
            _blackTransitionOut.Visible = false;

            LoadPlayer();
        }

        base.LoadContent();
    }

    private void Window_ClientSizeChanged(object sender, EventArgs e) {
        _virtualScreen.PhysicalResolutionChanged();
        EngineEV.RefreshEngine(Camera.GraphicsDevice);
        Console.WriteLine(@"Resolution changed.");
    }

    private void LoadPlayer() {
        if (Player != null) {
            return;
        }

        Player = new PlayerObj("PlayerIdle_Character", PlayerIndex.One, (Game as Game)!.PhysicsManager, null, Game as Game) {
            Position = new Vector2(200, 200),
        };
        Player.Initialize();
    }

    public void DisplayScreen(int screenType, bool pauseOtherScreens, List<object> objList = null) {
        LoadPlayer();

        if (pauseOtherScreens) {
            // This needs to be changed so that the ScreenManager holds a reference to the ProceduralLevelScreen.
            foreach (var screen in GetScreens()) {
                if (screen != CurrentScreen) {
                    continue;
                }

                screen.PauseScreen();
                break;
            }
        }

        CurrentScreenType = screenType;

        // This currently has no checks to see if the screen is already in the screen-manager's screen list.
        switch (screenType) {
            case ScreenType.CDG_SPLASH:
            case ScreenType.TITLE:
            case ScreenType.TITLE_WHITE:
            case ScreenType.STARTING_ROOM:
            case ScreenType.DEMO_START:
            case ScreenType.DEMO_END:
            case ScreenType.LINEAGE:
                LoadScreen((byte)screenType, true);
                break;

            case ScreenType.LEVEL:
                if (RogueCastle.Game.PlayerStats.LockCastle || CurrentScreen is ProceduralLevelScreen == false) {
                    LoadScreen((byte)screenType, true);
                } else {
                    LoadScreen((byte)screenType, false);
                }

                break;

            case ScreenType.SKILL:
                AddScreen(SkillScreen, null);
                break;

            case ScreenType.GAME_OVER:
                _gameOverScreen.PassInData(objList);
                AddScreen(_gameOverScreen, null);
                break;

            case ScreenType.GAME_OVER_BOSS:
                _gameOverBossScreen.PassInData(objList);
                AddScreen(_gameOverBossScreen, null);
                break;

            case ScreenType.BLACKSMITH:
                _blacksmithScreen.Player = Player;
                AddScreen(_blacksmithScreen, null);
                break;

            case ScreenType.GET_ITEM:
                _getItemScreen.PassInData(objList);
                AddScreen(_getItemScreen, null);
                break;

            case ScreenType.ENCHANTRESS:
                _enchantressScreen.Player = Player;
                AddScreen(_enchantressScreen, null);
                break;

            case ScreenType.DIALOGUE:
                AddScreen(DialogueScreen, null);
                break;

            case ScreenType.MAP:
                _mapScreen.SetPlayer(Player);
                AddScreen(_mapScreen, null);
                break;

            case ScreenType.PAUSE:
                GetLevelScreen().CurrentRoom.DarkenRoom();
                AddScreen(_pauseScreen, null);
                break;

            case ScreenType.OPTIONS:
                _optionsScreen.PassInData(objList);
                AddScreen(_optionsScreen, null);
                break;

            case ScreenType.PROFILE_CARD:
                AddScreen(_profileCardScreen, null);
                break;

            case ScreenType.CREDITS:
                LoadScreen(ScreenType.CREDITS, true);
                break;

            case ScreenType.SKILL_UNLOCK:
                _skillUnlockScreen.PassInData(objList);
                AddScreen(_skillUnlockScreen, null);
                break;

            case ScreenType.DIARY_ENTRY:
                AddScreen(_diaryEntryScreen, null);
                break;

            case ScreenType.DEATH_DEFY:
                AddScreen(_deathDefyScreen, null);
                break;

            case ScreenType.TEXT:
                _textScreen.PassInData(objList);
                AddScreen(_textScreen, null);
                break;

            case ScreenType.TUTORIAL_ROOM:
                LoadScreen(ScreenType.TUTORIAL_ROOM, true);
                break;

            case ScreenType.ENDING:
                GetLevelScreen().CameraLockedToPlayer = false;
                GetLevelScreen().DisableRoomTransitioning = true;
                Player.Position = new Vector2(100, 100); // HHHAACCK
                LoadScreen(ScreenType.ENDING, true);
                break;

            case ScreenType.DIARY_FLASHBACK:
                AddScreen(_flashbackScreen, null);
                break;

            case ScreenType.PROFILE_SELECT:
                AddScreen(_profileSelectScreen, null);
                break;

            case ScreenType.RANDOMIZER_MENU:
                _randomizerMenuScreen.PassInData(objList);
                AddScreen(_randomizerMenuScreen, null);
                break;
        }

        if (_isWipeTransitioning) {
            EndWipeTransition();
        }
    }

    public void AddRoomsToMap(List<RoomObj> roomList) {
        _mapScreen.AddRooms(roomList);
    }

    public void AddIconsToMap(List<RoomObj> roomList) {
        _mapScreen.AddAllIcons(roomList);
    }

    public void RefreshMapScreenChestIcons(RoomObj room) {
        _mapScreen.RefreshMapChestIcons(room);
    }

    public void ActivateMapScreenTeleporter() {
        _mapScreen.IsTeleporter = true;
    }

    public void HideCurrentScreen() {
        RemoveScreen(CurrentScreen, false);

        if (CurrentScreen is ProceduralLevelScreen level) {
            level.UnpauseScreen();
        }

        if (_isWipeTransitioning) {
            EndWipeTransition();
        }
    }

    public void ForceResolutionChangeCheck() {
        _virtualScreen.PhysicalResolutionChanged();
        EngineEV.RefreshEngine(Game.GraphicsDevice);
    }

    // This is overridden so that a custom LoadScreen can be passed in.
    private void LoadScreen(byte screenType, bool wipeTransition) {
        CurrentScreenType = ScreenType.LOADING;
        foreach (var screen in m_screenArray) {
            screen.DrawIfCovered = true;
            if (screen is not ProceduralLevelScreen levelScreen) {
                continue;
            }

            Player.AttachLevel(levelScreen);
            levelScreen.Player = Player;
            AttachMap(levelScreen);
        }

        // Double check this.  This doesn't seem right.
        if (_gameOverScreen != null) {
            InitializeScreens();
            LoadContent(); // Since all screens are disposed, their content needs to be loaded again. Hacked.
        }

        // Create and activate the loading screen.
        var loadingScreen = new LoadingScreen(screenType, wipeTransition);
        IsTransitioning = true;

        AddScreen(loadingScreen, PlayerIndex.One);
    }

    public void PerformCleanUp() {
        foreach (var screen in _screenCleanupList.Where(screen => screen.IsDisposed == false && screen.IsContentLoaded)) {
            screen.Dispose();
        }

        _screenCleanupList.Clear();

        // Should this be called? Most people say don't, but I want to ensure that collection occurs during the loading screen, not some random moment later.
        GC.Collect();
    }

    public void LoadingComplete(int screenType) {
        CurrentScreenType = screenType;
    }

    public override void RemoveScreen(Screen screen, bool disposeScreen) {
        if (screen is LoadingScreen) {
            IsTransitioning = false;
        }

        base.RemoveScreen(screen, disposeScreen);
    }

    public void AttachMap(ProceduralLevelScreen level) {
        _mapScreen?.Dispose();
        _mapScreen = new MapScreen(level);
    }

    public override void Update(GameTime gameTime) {
        _virtualScreen.Update();
        if (IsTransitioning == false) {
            base.Update(gameTime);
            return;
        }

        Camera.GameTime = gameTime;
        if (CurrentScreen == null) {
            return;
        }

        CurrentScreen.Update(gameTime);
        CurrentScreen.HandleInput();
    }

    public override void Draw(GameTime gameTime) {
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

        if (LevelEV.EnableDebugInput) {
            Camera.Begin();
            var forcedGenderString = RogueCastle.Game.PlayerStats.ForceLanguageGender switch {
                1 => "Male",
                2 => "Female",
                _ => "None",
            };

            var godModeString = "Off";
            if (RogueCastle.Game.PlayerStats.GodMode) {
                godModeString = "On";
            }

            Camera.DrawString(RogueCastle.Game.PixelArtFont, $"Forced Gender Language: {forcedGenderString}", new Vector2(10, 10), Color.White);
            Camera.DrawString(RogueCastle.Game.PixelArtFont, $"God Mode: {godModeString}", new Vector2(10, 50), Color.White);
            Camera.End();
        }
    }

    public void StartWipeTransition() {
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
    }

    public void EndWipeTransition() {
        _isWipeTransitioning = false;

        _blackTransitionOut.Y = -500;
        Tween.By(_blackTransitionIn, 0.25f, Quad.EaseInOut, "X", "-3000");
        Tween.By(_blackScreen, 0.25f, Quad.EaseInOut, "X", "-3000");
        Tween.By(_blackTransitionOut, 0.25f, Quad.EaseInOut, "X", "-3000");
    }

    public void UpdatePauseScreenIcons() {
        _pauseScreen.UpdatePauseScreenIcons();
    }

    public ProceduralLevelScreen GetLevelScreen() {
        foreach (var screen in GetScreens()) {
            if (screen is ProceduralLevelScreen level) {
                return level;
            }
        }

        return null;
    }
}
