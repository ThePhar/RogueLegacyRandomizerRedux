//#define OLD_CONSOLE_CREDITS
//#define SWITCH_CREDITS

using System;
using System.Collections.Generic;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueCastle.EnvironmentVariables;
using RogueCastle.GameObjects.RoomObjs;
using RogueCastle.GameStructs;
using RogueCastle.Managers;
using RogueCastle.Objects;
using RogueCastle.Screens.BaseScreens;
using Tweener;
using Tweener.Ease;

namespace RogueCastle.Screens;

public class ProceduralLevelScreen : Screen
{
    private const byte INPUT_TOGGLEMAP = 0;
    private const byte INPUT_TOGGLEZOOM = 1;
    private const byte INPUT_LEFTCONTROL = 2;
    private const byte INPUT_LEFT = 3;
    private const byte INPUT_RIGHT = 4;
    private const byte INPUT_UP = 5;
    private const byte INPUT_DOWN = 6;
    private const byte INPUT_DISPLAYROOMINFO = 7;
    private const SurfaceFormat fgTargetFormat = SurfaceFormat.Color;
    private const SurfaceFormat effectTargetFormat = SurfaceFormat.Color;
    protected int BottomDoorPercent = 80;

    public TextObj DebugTextObj;

    protected int LeftDoorPercent = 80;
    public bool LoadGameData = false;
    private int _bagsCollected;
    private RenderTarget2D _bgRenderTarget; // The render target that the background is drawn on.
    private int _bigDiamondsCollected;

    // Black borders for cinematic scenes.
    private SpriteObj _blackBorder1;
    private SpriteObj _blackBorder2;
    private int _blueprintsCollected;
    private int _borderSize;
    protected int _bottomMostBorder = -int.MaxValue;

    private Texture2D _castleBorderTexture, _towerBorderTexture, _dungeonBorderTexture, _gardenBorderTexture;

    private int _coinsCollected;
    private SpriteObj _compass;

    // Variables for the compass.
    private SpriteObj _compassBG;
    private bool _compassDisplayed;
    private DoorObj _compassDoor;
    private int _creditsIndex;

    // Variables for the credits that appear in the beginning.
    private TextObj _creditsText;
    private string[] _creditsTextList;
    private string[] _creditsTextTitleList;
    private TextObj _creditsTitleText;

    protected RoomObj _currentRoom;

    private int _diamondsCollected;
    // Created in level instead of having each room create their own, to save massive VRAM.

    // Effects for dynamic lighting ////
    private SpriteObj _dungeonLight;

    private float _elapsedScreenShake;

    private EnemyHUDObj _enemyHUD;
    private float _enemyHUDCounter;
    private readonly float _enemyHUDDuration = 2.0f;

    // Code needed for spells
    private float _enemyPauseDuration;

    private List<Vector2> _enemyStartPositions;

    private readonly float _fakeElapsedTotalHour = 1f / 60f / (60f * 60f);
    private RenderTarget2D _fgRenderTarget; // The render target that the foreground is drawn on.
    private SpriteObj _filmGrain;

    private BackgroundObj _foregroundSprite, _backgroundSprite, _backgroundParallaxSprite, _gardenParallaxFG;

    private bool _horizontalShake;

    ////////////////////////////////////

    private InputMap _inputMap;
    protected ItemDropManager _itemDropManager;

    private List<EnemyObj> _killedEnemyObjList;
    private EnemyObj _lastEnemyHit;

    protected int _leftMostBorder = int.MaxValue;

    private RenderTarget2D _lightSourceRenderTarget; // Also used to calculate shadows.  Maybe these can be merged somehow.

    private SpriteObj _mapBG;
    protected MapObj MiniMapDisplay;
    private Texture2D _neoBorderTexture;

    // Code for objective plate.
    private ObjContainer _objectivePlate;
    private TweenObject _objectivePlateTween;

    private GameObj _objKilledPlayer;
    protected PhysicsManager _physicsManager;

    private PlayerHUDObj _playerHUD;
    private ProjectileIconPool _projectileIconPool;
    protected ProjectileManager _projectileManager;
    protected int _rightMostBorder = -int.MaxValue;

    private RenderTarget2D _roomBWRenderTarget; // A special render target that is created so that rooms can draw their backgrounds.

    private TextObj _roomEnteringTitle;

    private TextObj _roomTitle;
    private float _screenShakeMagnitude;

    // Special pixel shader render targets.
    private RenderTarget2D _shadowRenderTarget; // The render target used to drawn the shadows in the dungeon.
    private bool _shakeScreen;

    public SkyObj _sky;
    private RenderTarget2D _skyRenderTarget; // The sky is drawn on this render target.
    private List<Vector2> _tempEnemyStartPositions;

    protected TextManager _textManager;

    private bool _toggleMagentaBG;
    protected int _topMostBorder = int.MaxValue;
    private SpriteObj _traitAura;
    private RenderTarget2D _traitAuraRenderTarget; // A render target used to draw trait effects like near sighted.
    private bool _verticalShake;
    private SpriteObj _whiteBG;
    protected int RightDoorPercent = 80;
    protected int TopDoorPercent = 80;

    public ProceduralLevelScreen()
    {
        DisableRoomTransitioning = false;
        RoomList = new List<RoomObj>();
        _textManager = new TextManager(700); //200 TEDDY RAISING POOL TO 500
        _projectileManager = new ProjectileManager(this, 700);
        _enemyStartPositions = new List<Vector2>();
        _tempEnemyStartPositions = new List<Vector2>();

        ImpactEffectPool = new ImpactEffectPool(2000);
        CameraLockedToPlayer = true;

        _roomTitle = new TextObj();
        _roomTitle.Font = Game.JunicodeLargeFont;
        //m_roomTitle.Align = Types.TextAlign.Centre;
        _roomTitle.Align = Types.TextAlign.Right;
        _roomTitle.Opacity = 0;
        _roomTitle.FontSize = 40;
        _roomTitle.Position = new Vector2(1320 - 50, 720 - 150);
        //m_roomTitle.Position = new Vector2(1320 / 2, 720 / 2 - 150);
        _roomTitle.OutlineWidth = 2;
        //m_roomTitle.DropShadow = new Vector2(4, 4);

        _roomEnteringTitle = _roomTitle.Clone() as TextObj;
        _roomEnteringTitle.Text = "LOC_ID_LEVEL_SCREEN_1".GetString(_roomEnteringTitle); //"Now Entering"
        _roomEnteringTitle.FontSize = 24;
        _roomEnteringTitle.Y -= 50;

        _inputMap = new InputMap(PlayerIndex.One, false);
        _inputMap.AddInput(INPUT_TOGGLEMAP, Keys.Y);
        _inputMap.AddInput(INPUT_TOGGLEZOOM, Keys.U);
        _inputMap.AddInput(INPUT_LEFTCONTROL, Keys.LeftControl);
        _inputMap.AddInput(INPUT_LEFT, Keys.Left);
        _inputMap.AddInput(INPUT_RIGHT, Keys.Right);
        _inputMap.AddInput(INPUT_UP, Keys.Up);
        _inputMap.AddInput(INPUT_DOWN, Keys.Down);
        _inputMap.AddInput(INPUT_DISPLAYROOMINFO, Keys.OemTilde);

        ChestList = new List<ChestObj>();
        MiniMapDisplay =
            new MapObj(true,
                this); // Must be called before CheckForRoomTransition() since rooms are added to the map during that call.

        _killedEnemyObjList = new List<EnemyObj>();
    }

    /// Death Animation Variables ///
    public float BackBufferOpacity { get; set; }

    public bool CameraLockedToPlayer { get; set; }

    public float ShoutMagnitude { get; set; }

    public bool DisableRoomOnEnter { get; set; } // Sometimes I need to currentRoom.OnEnter().

    public bool DisableSongUpdating { get; set; }
    public bool DisableRoomTransitioning { get; set; }

    public bool JukeboxEnabled { get; set; }

    public List<RoomObj> MapRoomsUnveiled
    {
        get => MiniMapDisplay.AddedRoomsList;
        set
        {
            MiniMapDisplay.ClearRoomsAdded();
            MiniMapDisplay.AddAllRooms(value);
            //(ScreenManager as RCScreenManager).AddRoomsToMap(value);
        }
    }

    public List<RoomObj> MapRoomsAdded => MiniMapDisplay.AddedRoomsList;

    public PlayerObj Player { get; set; }

    public List<RoomObj> RoomList { get; private set; }

    public PhysicsManager PhysicsManager => _physicsManager;

    public RoomObj CurrentRoom => _currentRoom;

    public ProjectileManager ProjectileManager => _projectileManager;

    public List<EnemyObj> EnemyList => CurrentRoom.EnemyList;

    public List<ChestObj> ChestList { get; private set; }

    public TextManager TextManager => _textManager;

    public ImpactEffectPool ImpactEffectPool { get; private set; }

    public ItemDropManager ItemDropManager => _itemDropManager;

    public GameTypes.LevelType CurrentLevelType => _currentRoom.LevelType;

    public int LeftBorder => _leftMostBorder;

    public int RightBorder => _rightMostBorder;

    public int TopBorder => _topMostBorder;

    public int BottomBorder => _bottomMostBorder;

    public RenderTarget2D RenderTarget { get; private set; }

    public bool EnemiesPaused { get; private set; }

    public override void LoadContent()
    {
        DebugTextObj = new TextObj(Game.JunicodeFont);
        DebugTextObj.FontSize = 26;
        DebugTextObj.Align = Types.TextAlign.Centre;
        DebugTextObj.Text = "";
        DebugTextObj.ForceDraw = true;

        _projectileIconPool = new ProjectileIconPool(200, _projectileManager, ScreenManager as RCScreenManager);
        _projectileIconPool.Initialize();

        _textManager.Initialize();

        ImpactEffectPool.Initialize();

        _physicsManager = (ScreenManager.Game as Game).PhysicsManager;
        _physicsManager.SetGravity(0, -GlobalEV.GRAVITY);

        _projectileManager.Initialize();
        _physicsManager.Initialize(ScreenManager.Camera);

        _itemDropManager = new ItemDropManager(600, _physicsManager);
        _itemDropManager.Initialize();

        _playerHUD = new PlayerHUDObj();
        _playerHUD.SetPosition(new Vector2(20, 40));

        _enemyHUD = new EnemyHUDObj();
        _enemyHUD.Position = new Vector2((GlobalEV.SCREEN_WIDTH / 2) - (_enemyHUD.Width / 2), 20);

        MiniMapDisplay.SetPlayer(Player);
        MiniMapDisplay.InitializeAlphaMap(new Rectangle(1320 - 250, 50, 200, 100), Camera);

        InitializeAllRooms(true); // Required to initialize all the render targets for each room. Must be called before InitializeEnemies/Chests() so that the room's level is set.
        InitializeEnemies();
        InitializeChests(true);
        InitializeRenderTargets();

        _mapBG = new SpriteObj("MinimapBG_Sprite");
        _mapBG.Position = new Vector2(1320 - 250, 50);
        _mapBG.ForceDraw = true;

        UpdateCamera();

        _borderSize = 100;
        _blackBorder1 = new SpriteObj("Blank_Sprite");
        _blackBorder1.TextureColor = Color.Black;
        _blackBorder1.Scale = new Vector2(1340f / _blackBorder1.Width, _borderSize / _blackBorder1.Height);
        _blackBorder2 = new SpriteObj("Blank_Sprite");
        _blackBorder2.TextureColor = Color.Black;
        _blackBorder2.Scale = new Vector2(1340f / _blackBorder2.Width, _borderSize / _blackBorder2.Height);
        _blackBorder1.ForceDraw = true;
        _blackBorder2.ForceDraw = true;
        _blackBorder1.Y = -_borderSize;
        _blackBorder2.Y = 720;

        _dungeonLight = new SpriteObj("LightSource_Sprite");
        _dungeonLight.ForceDraw = true;
        _dungeonLight.Scale = new Vector2(12, 12);
        _traitAura = new SpriteObj("LightSource_Sprite");
        _traitAura.ForceDraw = true;

        // Objective plate
        _objectivePlate = new ObjContainer("DialogBox_Character");
        _objectivePlate.ForceDraw = true;
        var objTitle = new TextObj(Game.JunicodeFont);
        objTitle.Position = new Vector2(-400, -60);
        objTitle.OverrideParentScale = true;
        objTitle.FontSize = 10;
        objTitle.Text = "LOC_ID_LEVEL_SCREEN_2".GetString(objTitle); //"Fairy Chest Objective:"
        objTitle.TextureColor = Color.Red;
        objTitle.OutlineWidth = 2;
        _objectivePlate.AddChild(objTitle);

        var objDescription = new TextObj(Game.JunicodeFont);
        objDescription.OverrideParentScale = true;
        objDescription.Position = new Vector2(objTitle.X, objTitle.Y + 40);
        objDescription.ForceDraw = true;
        objDescription.FontSize = 9;
        objDescription.Text = "LOC_ID_LEVEL_SCREEN_3".GetString(objDescription); //"Reach the chest in 15 seconds:"
        objDescription.WordWrap(250);
        objDescription.OutlineWidth = 2;
        _objectivePlate.AddChild(objDescription);

        var objProgress = new TextObj(Game.JunicodeFont);
        objProgress.OverrideParentScale = true;
        objProgress.Position = new Vector2(objDescription.X, objDescription.Y + 35);
        objProgress.ForceDraw = true;
        objProgress.FontSize = 9;
        objProgress.Text = "LOC_ID_LEVEL_SCREEN_4".GetString(objProgress); //"Time Remaining:"
        objProgress.WordWrap(250);
        objProgress.OutlineWidth = 2;
        _objectivePlate.AddChild(objProgress);

        _objectivePlate.Scale = new Vector2(250f / _objectivePlate.GetChildAt(0).Width,
            130f / _objectivePlate.GetChildAt(0).Height);
        _objectivePlate.Position = new Vector2(1170 + 300, 250);

        var objectiveLine1 = new SpriteObj("Blank_Sprite");
        objectiveLine1.TextureColor = Color.Red;
        objectiveLine1.Position = new Vector2(objDescription.X, objDescription.Y + 20);
        objectiveLine1.ForceDraw = true;
        objectiveLine1.OverrideParentScale = true;
        objectiveLine1.ScaleY = 0.5f;
        _objectivePlate.AddChild(objectiveLine1);

        var objectiveLine2 = new SpriteObj("Blank_Sprite");
        objectiveLine2.TextureColor = Color.Red;
        objectiveLine2.Position = new Vector2(objDescription.X, objectiveLine1.Y + 35);
        objectiveLine2.ForceDraw = true;
        objectiveLine2.OverrideParentScale = true;
        objectiveLine2.ScaleY = 0.5f;
        _objectivePlate.AddChild(objectiveLine2);
        base.LoadContent(); // Doesn't do anything.

        _sky = new SkyObj(this);
        _sky.LoadContent(Camera);

        _whiteBG = new SpriteObj("Blank_Sprite");
        _whiteBG.Opacity = 0;
        _whiteBG.Scale = new Vector2(1320f / _whiteBG.Width, 720f / _whiteBG.Height);

        _filmGrain = new SpriteObj("FilmGrain_Sprite");
        _filmGrain.ForceDraw = true;
        _filmGrain.Scale = new Vector2(2.015f, 2.05f);
        _filmGrain.X -= 5;
        _filmGrain.Y -= 5;
        _filmGrain.PlayAnimation();
        _filmGrain.AnimationDelay = 1 / 30f;

        _compassBG = new SpriteObj("CompassBG_Sprite");
        _compassBG.ForceDraw = true;
        _compassBG.Position = new Vector2(1320 / 2f, 90);
        _compassBG.Scale = Vector2.Zero;
        _compass = new SpriteObj("Compass_Sprite");
        _compass.Position = _compassBG.Position;
        _compass.ForceDraw = true;
        _compass.Scale = Vector2.Zero;

        InitializeCreditsText();
    }

    private void InitializeCreditsText()
    {
        _creditsTextTitleList = new[]
        {
            "LOC_ID_TUTORIAL_CREDITS_TITLE_1", "LOC_ID_TUTORIAL_CREDITS_TITLE_2", "LOC_ID_TUTORIAL_CREDITS_TITLE_3",
            "LOC_ID_TUTORIAL_CREDITS_TITLE_4", "LOC_ID_TUTORIAL_CREDITS_TITLE_5", "LOC_ID_TUTORIAL_CREDITS_TITLE_6",
            "LOC_ID_TUTORIAL_CREDITS_TITLE_7", "LOC_ID_TUTORIAL_CREDITS_TITLE_8",
        };

        _creditsTextList = new[]
        {
            "Cellar Door Games", "Teddy Lee", "Kenny Lee", "Marie-Christine Bourdua", "Glauber Kotaki",
            "Gordon McGladdery", "Judson Cowan", "Rogue Legacy",
        };

        _creditsText = new TextObj(Game.JunicodeFont);
        _creditsText.FontSize = 20;
        _creditsText.Text = "Cellar Door Games";
        _creditsText.DropShadow = new Vector2(2, 2);
        _creditsText.Opacity = 0;

        _creditsTitleText = _creditsText.Clone() as TextObj;
        _creditsTitleText.FontSize = 14;
        _creditsTitleText.Position = new Vector2(50, 580);

        _creditsText.Position = _creditsTitleText.Position;
        _creditsText.Y += 35;
        _creditsTitleText.X += 5;
    }

    public void DisplayCreditsText(bool resetIndex)
    {
        if (resetIndex)
        {
            _creditsIndex = 0;
        }

        _creditsTitleText.Opacity = 0;
        _creditsText.Opacity = 0;

        if (_creditsIndex < _creditsTextList.Length)
        {
            _creditsTitleText.Opacity = 0;
            _creditsText.Opacity = 0;

            _creditsTitleText.Text = _creditsTextTitleList[_creditsIndex].GetString(_creditsTitleText);
            _creditsText.Text = _creditsTextList[_creditsIndex];

            // Tween text in.
            Tween.To(_creditsTitleText, 0.5f, Tween.EaseNone, "Opacity", "1");
            Tween.To(_creditsText, 0.5f, Tween.EaseNone, "delay", "0.2", "Opacity", "1");
            _creditsTitleText.Opacity = 1;
            _creditsText.Opacity = 1;

            // Tween text out.
            Tween.To(_creditsTitleText, 0.5f, Tween.EaseNone, "delay", "4", "Opacity", "0");
            Tween.To(_creditsText, 0.5f, Tween.EaseNone, "delay", "4.2", "Opacity", "0");
            _creditsTitleText.Opacity = 0;
            _creditsText.Opacity = 0;

            _creditsIndex++;
            Tween.RunFunction(8, this, "DisplayCreditsText", false);
        }
    }

    public void StopCreditsText()
    {
        _creditsIndex = 0;
        Tween.StopAllContaining(_creditsTitleText, false);
        Tween.StopAllContaining(_creditsText, false);
        Tween.StopAllContaining(this, false);
        _creditsTitleText.Opacity = 0;
    }

    public override void ReinitializeRTs()
    {
        _sky.ReinitializeRT(Camera);
        MiniMapDisplay.InitializeAlphaMap(new Rectangle(1320 - 250, 50, 200, 100), Camera);
        InitializeRenderTargets();
        InitializeAllRooms(false);

        if (CurrentRoom == null || CurrentRoom.Name != "Start")
        {
            if (CurrentRoom.Name == "ChallengeBoss")
            {
                //m_foregroundSprite.TextureColor = Color.Black;
                //m_backgroundSprite.TextureColor = Color.Black;
                _backgroundSprite.Scale = Vector2.One;
                _backgroundSprite.ChangeSprite("NeoBG_Sprite", ScreenManager.Camera);
                _backgroundSprite.Scale = new Vector2(2, 2);

                _foregroundSprite.Scale = Vector2.One;
                _foregroundSprite.ChangeSprite("NeoFG_Sprite", ScreenManager.Camera);
                _foregroundSprite.Scale = new Vector2(2, 2);
            }
            else
            {
                switch (CurrentRoom.LevelType)
                {
                    case GameTypes.LevelType.Castle:
                        _backgroundSprite.Scale = Vector2.One;
                        _foregroundSprite.Scale = Vector2.One;
                        _backgroundSprite.ChangeSprite("CastleBG1_Sprite", ScreenManager.Camera);
                        _foregroundSprite.ChangeSprite("CastleFG1_Sprite", ScreenManager.Camera);
                        _backgroundSprite.Scale = new Vector2(2, 2);
                        _foregroundSprite.Scale = new Vector2(2, 2);
                        break;
                    case GameTypes.LevelType.Tower:
                        _backgroundSprite.Scale = Vector2.One;
                        _foregroundSprite.Scale = Vector2.One;
                        _backgroundSprite.ChangeSprite("TowerBG2_Sprite", ScreenManager.Camera);
                        _foregroundSprite.ChangeSprite("TowerFG2_Sprite", ScreenManager.Camera);
                        _backgroundSprite.Scale = new Vector2(2, 2);
                        _foregroundSprite.Scale = new Vector2(2, 2);
                        break;
                    case GameTypes.LevelType.Dungeon:
                        _backgroundSprite.Scale = Vector2.One;
                        _foregroundSprite.Scale = Vector2.One;
                        _backgroundSprite.ChangeSprite("DungeonBG1_Sprite", ScreenManager.Camera);
                        _foregroundSprite.ChangeSprite("DungeonFG1_Sprite", ScreenManager.Camera);
                        _backgroundSprite.Scale = new Vector2(2, 2);
                        _foregroundSprite.Scale = new Vector2(2, 2);
                        break;
                    case GameTypes.LevelType.Garden:
                        _backgroundSprite.Scale = Vector2.One;
                        _foregroundSprite.Scale = Vector2.One;
                        _backgroundSprite.ChangeSprite("GardenBG_Sprite", ScreenManager.Camera);
                        _foregroundSprite.ChangeSprite("GardenFG_Sprite", ScreenManager.Camera);
                        _backgroundSprite.Scale = new Vector2(2, 2);
                        _foregroundSprite.Scale = new Vector2(2, 2);
                        break;
                }
            }

            if (Game.PlayerStats.HasTrait(TraitType.THE_ONE))
            {
                _foregroundSprite.Scale = Vector2.One;
                _foregroundSprite.ChangeSprite("NeoFG_Sprite", ScreenManager.Camera);
                _foregroundSprite.Scale = new Vector2(2, 2);
            }
        }

        _backgroundSprite.Position = CurrentRoom.Position;
        _foregroundSprite.Position = CurrentRoom.Position;

        base.ReinitializeRTs();
    }

    // Removes the previous room's objects from the physics manager and adds the new room's objects.
    // Not performance heavy because removing and adding objects to physics manager is O(1), with the exception of Clear(), which is O(n).
    private void LoadPhysicsObjects(RoomObj room)
    {
        var expandedRoomRect =
            new Rectangle((int)room.X - 100, (int)room.Y - 100, room.Width + 200,
                room.Height + 200); // An expanded bounds of the room.
        //m_physicsManager.ObjectList.Clear();
        _physicsManager.RemoveAllObjects();

        foreach (var obj in CurrentRoom.TerrainObjList)
        {
            _physicsManager.AddObject(obj);
        }

        foreach (var obj in _projectileManager.ActiveProjectileList)
        {
            _physicsManager.AddObject(obj);
        }

        foreach (var obj in CurrentRoom.GameObjList)
        {
            var physicsObj = obj as IPhysicsObj;
            if (physicsObj != null &&
                obj.Bounds.Intersects(expandedRoomRect)) // Not sure why we're doing a bounds intersect check.
            {
                var breakable = obj as BreakableObj;
                if (breakable != null && breakable.Broken) // Don't add broken breakables to the list.
                {
                    continue;
                }

                _physicsManager.AddObject(physicsObj);
            }
        }

        // This is needed for entering boss doors.
        foreach (var door in CurrentRoom.DoorList)
        {
            _physicsManager.AddObject(door);
        }

        foreach (var enemy in CurrentRoom.EnemyList)
        {
            _physicsManager.AddObject(enemy);

            if (enemy is EnemyObj_BallAndChain) // Special handling to add the separate entity ball for the ball and chain dude.
            {
                if (enemy.IsKilled == false)
                {
                    _physicsManager.AddObject((enemy as EnemyObj_BallAndChain).BallAndChain);
                    if (enemy.Difficulty > GameTypes.EnemyDifficulty.Basic)
                    {
                        _physicsManager.AddObject((enemy as EnemyObj_BallAndChain).BallAndChain2);
                    }
                }
            }
        }

        foreach (var enemy in CurrentRoom.TempEnemyList)
        {
            _physicsManager.AddObject(enemy);
        }

        _physicsManager.AddObject(Player);
    }

    public void InitializeEnemies()
    {
        //int enemyLevel = 1;
        //int enemyDifficulty = (int)GameTypes.EnemyDifficulty.BASIC;
        //int levelCounter = 0;

        var terrainCollList = new List<TerrainObj>();

        foreach (var room in RoomList)
        {
            foreach (var enemy in room.EnemyList)
            {
                enemy.SetPlayerTarget(Player);
                enemy.SetLevelScreen(this); // Must be called before enemy.Initialize().

                var roomLevel = room.Level;
                // Special handling for boss rooms.
                if (room.Name == "Boss" && room.LinkedRoom != null)
                {
                    //int roomLevel = room.LinkedRoom.RoomNumber;
                    roomLevel = room.LinkedRoom.Level;
                    var bossEnemyLevel = (int)(roomLevel / (LevelEV.ROOM_LEVEL_MOD +
                                                            (Game.PlayerStats.GetNumberOfEquippedRunes(
                                                                 EquipmentAbilityType.ROOM_LEVEL_DOWN) *
                                                             GameEV.RUNE_GRACE_ROOM_LEVEL_LOSS)));
                    enemy.Level = bossEnemyLevel;
                }
                else
                {
                    var enemyLevel = (int)(roomLevel / (LevelEV.ROOM_LEVEL_MOD +
                                                        (Game.PlayerStats.GetNumberOfEquippedRunes(EquipmentAbilityType
                                                            .ROOM_LEVEL_DOWN) * GameEV.RUNE_GRACE_ROOM_LEVEL_LOSS)));
                    if (enemyLevel < 1)
                    {
                        enemyLevel = 1;
                    }

                    enemy.Level =
                        enemyLevel; // Call this before Initialize(), since Initialie sets their starting health and so on.
                }

                var enemyDifficulty = enemy.Level / LevelEV.ENEMY_LEVEL_DIFFICULTY_MOD;
                if (enemyDifficulty > (int)GameTypes.EnemyDifficulty.Expert)
                {
                    enemyDifficulty = (int)GameTypes.EnemyDifficulty.Expert;
                }

                if (enemy.IsProcedural)
                {
                    if (enemy.Difficulty == GameTypes.EnemyDifficulty.Expert)
                    {
                        enemy.Level += LevelEV.ENEMY_EXPERT_LEVEL_MOD;
                    }

                    if ((int)enemy.Difficulty < enemyDifficulty)
                    {
                        enemy.SetDifficulty((GameTypes.EnemyDifficulty)enemyDifficulty, false);
                    }
                }
                else
                {
                    if (enemy.Difficulty == GameTypes.EnemyDifficulty.Miniboss)
                    {
                        if (room is ArenaBonusRoom) // Level up arena room enemies by expert level instead of miniboss.
                        {
                            enemy.Level += LevelEV.ENEMY_EXPERT_LEVEL_MOD;
                        }
                        else
                        {
                            enemy.Level += LevelEV.ENEMY_MINIBOSS_LEVEL_MOD;
                        }
                    }
                }

                //if (enemy.Difficulty == GameTypes.EnemyDifficulty.EXPERT)
                //    enemy.Level += LevelEV.ENEMY_EXPERT_LEVEL_MOD; // If an enemy is already expert, then he is a yellow orb, and should gain these extra level.
                //else if (enemy.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
                //    enemy.Level += LevelEV.ENEMY_MINIBOSS_LEVEL_MOD; // Minibosses gain these extra levels.
                //else if (enemy.IsProcedural == true && (int)enemy.Difficulty < enemyDifficulty) // Only change the difficulty of procedural enemies and if they're not yellows and they're lower difficulty.
                //    enemy.SetDifficulty((GameTypes.EnemyDifficulty)enemyDifficulty, false);

                enemy.Initialize();

                // Positioning each enemy to the ground closest below them. But don't do it if they fly.
                if (enemy.IsWeighted)
                {
                    var closestGround = float.MaxValue;
                    TerrainObj closestTerrain = null;
                    terrainCollList.Clear();
                    var enemyBoundsRect = new Rectangle((int)enemy.X, enemy.TerrainBounds.Bottom, 1, 5000);
                    foreach (var terrainObj in room.TerrainObjList)
                    {
                        if (terrainObj.Rotation == 0)
                        {
                            if (terrainObj.Bounds.Top >= enemy.TerrainBounds.Bottom &&
                                CollisionMath.Intersects(terrainObj.Bounds, enemyBoundsRect))
                            {
                                terrainCollList.Add(terrainObj);
                            }
                        }
                        else
                        {
                            if (CollisionMath.RotatedRectIntersects(enemyBoundsRect, 0, Vector2.Zero,
                                    terrainObj.TerrainBounds, terrainObj.Rotation, Vector2.Zero))
                            {
                                terrainCollList.Add(terrainObj);
                            }
                        }
                    }

                    foreach (var terrain in terrainCollList)
                    {
                        var collides = false;
                        var groundDist = 0;
                        if (terrain.Rotation == 0)
                        {
                            collides = true;
                            groundDist = terrain.TerrainBounds.Top - enemy.TerrainBounds.Bottom;
                        }
                        else
                        {
                            Vector2 pt1, pt2;
                            if (terrain.Width > terrain.Height) // If rotated objects are done correctly.
                            {
                                pt1 = CollisionMath.UpperLeftCorner(terrain.TerrainBounds, terrain.Rotation,
                                    Vector2.Zero);
                                pt2 = CollisionMath.UpperRightCorner(terrain.TerrainBounds, terrain.Rotation,
                                    Vector2.Zero);
                            }
                            else // If rotated objects are done Teddy's incorrect way.
                            {
                                if (terrain.Rotation > 0) // ROTCHECK
                                {
                                    pt1 = CollisionMath.LowerLeftCorner(terrain.TerrainBounds, terrain.Rotation,
                                        Vector2.Zero);
                                    pt2 = CollisionMath.UpperLeftCorner(terrain.TerrainBounds, terrain.Rotation,
                                        Vector2.Zero);
                                }
                                else
                                {
                                    pt1 = CollisionMath.UpperRightCorner(terrain.TerrainBounds, terrain.Rotation,
                                        Vector2.Zero);
                                    pt2 = CollisionMath.LowerRightCorner(terrain.TerrainBounds, terrain.Rotation,
                                        Vector2.Zero);
                                }
                            }

                            // A check to make sure the enemy collides with the correct slope.
                            if (enemy.X > pt1.X && enemy.X < pt2.X)
                            {
                                collides = true;
                            }

                            var u = pt2.X - pt1.X;
                            var v = pt2.Y - pt1.Y;
                            var x = pt1.X;
                            var y = pt1.Y;
                            var x1 = enemy.X;

                            groundDist = (int)(y + ((x1 - x) * (v / u))) - enemy.TerrainBounds.Bottom;
                        }

                        if (collides && groundDist < closestGround && groundDist > 0)
                        {
                            closestGround = groundDist;
                            closestTerrain = terrain;
                        }
                    }

                    //foreach (TerrainObj terrainObj in room.TerrainObjList)
                    //{
                    //    if (terrainObj.Y >= enemy.Y)
                    //    {
                    //        if (terrainObj.Y - enemy.Y < closestGround && CollisionMath.Intersects(terrainObj.Bounds, new Rectangle((int)enemy.X, (int)(enemy.Y + (terrainObj.Y - enemy.Y) + 5), enemy.Width, (int)(enemy.Height / 2))))
                    //        {
                    //            closestGround = terrainObj.Y - enemy.Y;
                    //            closestTerrain = terrainObj;
                    //        }
                    //    }
                    //}

                    if (closestTerrain != null)
                    {
                        enemy.UpdateCollisionBoxes();
                        if (closestTerrain.Rotation == 0)
                        {
                            enemy.Y = closestTerrain.Y - (enemy.TerrainBounds.Bottom - enemy.Y);
                        }
                        else
                        {
                            HookEnemyToSlope(enemy, closestTerrain);
                        }
                    }
                }
            }
        }
    }

    private void HookEnemyToSlope(IPhysicsObj enemy, TerrainObj terrain)
    {
        var y1 = float.MaxValue;
        Vector2 pt1, pt2;
        if (terrain.Width > terrain.Height) // If rotated objects are done correctly.
        {
            pt1 = CollisionMath.UpperLeftCorner(terrain.TerrainBounds, terrain.Rotation, Vector2.Zero);
            pt2 = CollisionMath.UpperRightCorner(terrain.TerrainBounds, terrain.Rotation, Vector2.Zero);
        }
        else // If rotated objects are done Teddy's incorrect way.
        {
            if (terrain.Rotation > 0) // ROTCHECK
            {
                pt1 = CollisionMath.LowerLeftCorner(terrain.TerrainBounds, terrain.Rotation, Vector2.Zero);
                pt2 = CollisionMath.UpperLeftCorner(terrain.TerrainBounds, terrain.Rotation, Vector2.Zero);
            }
            else
            {
                pt1 = CollisionMath.UpperRightCorner(terrain.TerrainBounds, terrain.Rotation, Vector2.Zero);
                pt2 = CollisionMath.LowerRightCorner(terrain.TerrainBounds, terrain.Rotation, Vector2.Zero);
            }
        }

        //if (enemy.X > pt1.X + 10 && enemy.X < pt2.X - 10)
        {
            var u = pt2.X - pt1.X;
            var v = pt2.Y - pt1.Y;
            var x = pt1.X;
            var y = pt1.Y;
            var x1 = enemy.X;

            y1 = y + ((x1 - x) * (v / u));

            enemy.UpdateCollisionBoxes();
            y1 -= enemy.Bounds.Bottom - enemy.Y + (5 * (enemy as GameObj).ScaleX);
            enemy.Y = (float)Math.Round(y1, MidpointRounding.ToEven);
        }
    }

    public void InitializeChests(bool resetChests)
    {
        ChestList.Clear();

        //int chestLevel = 1;
        //int levelCounter = 0; // Every 5 times a room is iterated, the chest's level goes up.

        foreach (var room in RoomList)
        {
            //if (room.Name != "Secret") // Do not modify chests for secret rooms yet.
            {
                foreach (var obj in room.GameObjList)
                {
                    var chest = obj as ChestObj;
                    if (chest != null &&
                        chest.ChestType !=
                        ChestType
                            .FAIRY) // && room.Name != "Bonus") // Do not modify chests for bonus rooms or fairy chests.
                    {
                        //chest.Level = chestLevel;  // Setting the chest level.
                        chest.Level = (int)(room.Level / (LevelEV.ROOM_LEVEL_MOD +
                                                          (Game.PlayerStats.GetNumberOfEquippedRunes(
                                                               EquipmentAbilityType.ROOM_LEVEL_DOWN) *
                                                           GameEV.RUNE_GRACE_ROOM_LEVEL_LOSS)));

                        if (chest.IsProcedural) // Ensures chests loaded from a save file are not overwritten.
                        {
                            // Closes the chests.
                            if (resetChests)
                            {
                                chest.ResetChest();
                            }


                            // Turning the chest into a brown, silver, or gold chest.

                            var chestRoll = CDGMath.RandomInt(1, 100);
                            var chestType = 0;
                            for (var i = 0; i < GameEV.ChestTypeChance.Length; i++)
                            {
                                chestType += GameEV.ChestTypeChance[i];
                                if (chestRoll <= chestType)
                                {
                                    if (i == 0)
                                    {
                                        chest.ChestType = ChestType.BROWN;
                                    }
                                    else if (i == 1)
                                    {
                                        chest.ChestType = ChestType.SILVER;
                                    }
                                    else
                                    {
                                        chest.ChestType = ChestType.GOLD;
                                    }

                                    break;
                                }
                            }
                            //////////////////////////////////////////////////////////
                        }

                        ChestList.Add(chest);
                    }
                    else if (chest != null && chest.ChestType == ChestType.FAIRY)
                    {
                        var fairyChest = chest as FairyChestObj;
                        if (fairyChest != null)
                        {
                            if (chest.IsProcedural)
                            {
                                if (resetChests)
                                {
                                    fairyChest.ResetChest();
                                }
                            }

                            //fairyChest.SetPlayer(m_player);
                            fairyChest.SetConditionType();
                        }
                    }

                    ChestList.Add(chest);

                    // Code to properly recentre chests (since their anchor points were recently modified in the spritesheet.
                    if (chest != null)
                    {
                        chest.X += chest.Width / 2;
                        chest.Y += 60; // The height of a tile.
                    }
                }
            }

            //if (room.Level % LevelEV.ROOM_LEVEL_MOD == 0)
            //    chestLevel++;

            //levelCounter++;
            //if (levelCounter >= LevelEV.ROOM_LEVEL_MOD)
            //{
            //    levelCounter = 0;
            //    chestLevel++;
            //}
        }
    }

    public void InitializeAllRooms(bool loadContent)
    {
        _castleBorderTexture =
            new SpriteObj("CastleBorder_Sprite") { Scale = new Vector2(2, 2) }.ConvertToTexture(Camera, true,
                SamplerState.PointWrap);
        var castleCornerTextureString = "CastleCorner_Sprite";
        var castleCornerLTextureString = "CastleCornerL_Sprite";

        _towerBorderTexture =
            new SpriteObj("TowerBorder2_Sprite") { Scale = new Vector2(2, 2) }.ConvertToTexture(Camera, true,
                SamplerState.PointWrap);
        var towerCornerTextureString = "TowerCorner_Sprite";
        var towerCornerLTextureString = "TowerCornerL_Sprite";

        _dungeonBorderTexture =
            new SpriteObj("DungeonBorder_Sprite") { Scale = new Vector2(2, 2) }.ConvertToTexture(Camera, true,
                SamplerState.PointWrap);
        var dungeonCornerTextureString = "DungeonCorner_Sprite";
        var dungeonCornerLTextureString = "DungeonCornerL_Sprite";

        _gardenBorderTexture =
            new SpriteObj("GardenBorder_Sprite") { Scale = new Vector2(2, 2) }.ConvertToTexture(Camera, true,
                SamplerState.PointWrap);
        var gardenCornerTextureString = "GardenCorner_Sprite";
        var gardenCornerLTextureString = "GardenCornerL_Sprite";

        _neoBorderTexture =
            new SpriteObj("NeoBorder_Sprite") { Scale = new Vector2(2, 2) }.ConvertToTexture(Camera, true,
                SamplerState.PointWrap);
        var futureCornerTextureString = "NeoCorner_Sprite";
        var futureCornerLTextureString = "NeoCornerL_Sprite";

        if (Game.PlayerStats.HasTrait(TraitType.THE_ONE))
        {
            castleCornerLTextureString = dungeonCornerLTextureString =
                towerCornerLTextureString = gardenCornerLTextureString = futureCornerLTextureString;
            castleCornerTextureString = dungeonCornerTextureString =
                towerCornerTextureString = gardenCornerTextureString = futureCornerTextureString;
        }
        // These textures need to be stored and released during dispose().

        var roomLevel = 0;
        roomLevel = Game.PlayerStats.GetNumberOfEquippedRunes(EquipmentAbilityType.ROOM_LEVEL_UP) *
                    GameEV.RUNE_CURSE_ROOM_LEVEL_GAIN;

        if (_roomBWRenderTarget != null)
        {
            _roomBWRenderTarget.Dispose();
        }

        _roomBWRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, GlobalEV.SCREEN_WIDTH, GlobalEV.SCREEN_HEIGHT,
            false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

        foreach (var room in RoomList)
        {
            var roomLevelMod = 0;
            switch (room.LevelType)
            {
                case GameTypes.LevelType.Castle:
                    roomLevelMod = LevelEV.CASTLE_ROOM_LEVEL_BOOST;
                    break;
                case GameTypes.LevelType.Garden:
                    roomLevelMod =
                        LevelEV.GARDEN_ROOM_LEVEL_BOOST -
                        2; // Subtracting 2 for each subsequent number of Linker rooms there are on the map.
                    break;
                case GameTypes.LevelType.Tower:
                    roomLevelMod = LevelEV.TOWER_ROOM_LEVEL_BOOST - 4;
                    break;
                case GameTypes.LevelType.Dungeon:
                    roomLevelMod = LevelEV.DUNGEON_ROOM_LEVEL_BOOST - 6;
                    break;
            }

            if (Game.PlayerStats.TimesCastleBeaten == 0)
            {
                room.Level = roomLevel + roomLevelMod;
            }
            else
            {
                room.Level = roomLevel + roomLevelMod + LevelEV.NEWGAMEPLUS_LEVEL_BASE +
                             ((Game.PlayerStats.TimesCastleBeaten - 1) *
                              LevelEV
                                  .NEWGAMEPLUS_LEVEL_APPRECIATION); //TEDDY DELETING 1 from TimesCastleBeaten CAUSE APPRECIATION SHOULDNT KICK IN.
            }

            roomLevel++;

            if (loadContent)
            {
                room.LoadContent(Camera.GraphicsDevice);
            }

            room.InitializeRenderTarget(_roomBWRenderTarget);

            if (room.Name == "ChallengeBoss")
            {
                foreach (var border in room.BorderList)
                {
                    border.SetBorderTextures(_neoBorderTexture, futureCornerTextureString, futureCornerLTextureString);
                    border.NeoTexture = _neoBorderTexture;
                }
            }
            else
            {
                foreach (var border in room.BorderList)
                {
                    switch (room.LevelType)
                    {
                        case GameTypes.LevelType.Tower:
                            border.SetBorderTextures(_towerBorderTexture, towerCornerTextureString,
                                towerCornerLTextureString);
                            break;
                        case GameTypes.LevelType.Dungeon:
                            border.SetBorderTextures(_dungeonBorderTexture, dungeonCornerTextureString,
                                dungeonCornerLTextureString);
                            break;
                        case GameTypes.LevelType.Garden:
                            border.SetBorderTextures(_gardenBorderTexture, gardenCornerTextureString,
                                gardenCornerLTextureString);
                            border.TextureOffset = new Vector2(0, -18);
                            break;
                        case GameTypes.LevelType.Castle:
                        default:
                            border.SetBorderTextures(_castleBorderTexture, castleCornerTextureString,
                                castleCornerLTextureString);
                            break;
                    }

                    border.NeoTexture = _neoBorderTexture;
                }
            }

            var addTerrainBoxToBreakables = false;
            if (Game.PlayerStats.HasTrait(TraitType.NO_FURNITURE))
            {
                addTerrainBoxToBreakables = true;
            }

            foreach (var obj in room.GameObjList)
            {
                var hazard = obj as HazardObj;
                if (hazard != null)
                {
                    hazard.InitializeTextures(Camera);
                }

                var hoverObj = obj as HoverObj;
                if (hoverObj != null)
                {
                    hoverObj.SetStartingPos(hoverObj.Position);
                }

                if (addTerrainBoxToBreakables)
                {
                    var breakableObj = obj as BreakableObj;

                    if (breakableObj != null && breakableObj.HitBySpellsOnly == false &&
                        breakableObj.HasTerrainHitBox == false)
                    {
                        breakableObj.CollisionBoxes.Add(new CollisionBox(breakableObj.RelativeBounds.X,
                            breakableObj.RelativeBounds.Y, breakableObj.Width, breakableObj.Height,
                            Consts.TERRAIN_HITBOX, breakableObj));
                        breakableObj.DisableHitboxUpdating = true;
                        breakableObj.UpdateTerrainBox();
                    }
                }
            }

            if (LevelEV.RunTestRoom && loadContent)
            {
                foreach (var obj in room.GameObjList)
                {
                    if (obj is PlayerStartObj)
                    {
                        Player.Position = obj.Position;
                    }
                }
            }

            if ((room.Name == "Boss" || room.Name == "ChallengeBoss") && room.LinkedRoom != null)
            {
                CloseBossDoor(room.LinkedRoom, room.LevelType);
                //OpenChallengeBossDoor(room.LinkedRoom, room.LevelType); // Extra content added to link challenge boss rooms.
                //if (Game.PlayerStats.ChallengeLastBossBeaten == false && Game.PlayerStats.ChallengeLastBossUnlocked == true)
                //    OpenLastBossChallengeDoors();
            }
        }
    }

    public void CloseBossDoor(RoomObj linkedRoom, GameTypes.LevelType levelType)
    {
        var closeDoor = false;

        switch (levelType)
        {
            case GameTypes.LevelType.Castle:
                if (Game.PlayerStats.EyeballBossBeaten)
                {
                    closeDoor = true;
                }

                break;
            case GameTypes.LevelType.Dungeon:
                if (Game.PlayerStats.BlobBossBeaten)
                {
                    closeDoor = true;
                }

                break;
            case GameTypes.LevelType.Garden:
                if (Game.PlayerStats.FairyBossBeaten)
                {
                    closeDoor = true;
                }

                break;
            case GameTypes.LevelType.Tower:
                if (Game.PlayerStats.FireballBossBeaten)
                {
                    closeDoor = true;
                }

                break;
        }

        if (closeDoor)
        {
            foreach (var door in linkedRoom.DoorList)
            {
                if (door.IsBossDoor)
                {
                    // Change the door graphic to closed.
                    foreach (var obj in linkedRoom.GameObjList)
                    {
                        if (obj.Name == "BossDoor")
                        {
                            obj.ChangeSprite((obj as SpriteObj).SpriteName.Replace("Open", ""));
                            obj.TextureColor = Color.White;
                            obj.Opacity = 1;
                            linkedRoom.LinkedRoom = null;
                            break;
                        }
                    }

                    // Lock the door.
                    door.Locked = true;
                    break;
                }
            }
        }

        OpenChallengeBossDoor(linkedRoom, levelType); // Extra content added to link challenge boss rooms.
        //if (Game.PlayerStats.ChallengeLastBossBeaten == false && Game.PlayerStats.ChallengeLastBossUnlocked == true)
        if (Game.PlayerStats.ChallengeLastBossUnlocked)
        {
            OpenLastBossChallengeDoors();
        }
    }

    public void OpenLastBossChallengeDoors()
    {
        LastBossChallengeRoom lastBossChallengeRoom = null;
        foreach (var room in RoomList)
        {
            if (room.Name == "ChallengeBoss")
            {
                if (room is LastBossChallengeRoom)
                {
                    lastBossChallengeRoom = room as LastBossChallengeRoom;
                    break;
                }
            }
        }

        foreach (var room in RoomList)
        {
            if (room.Name == "EntranceBoss")
            {
                var linkChallengeBossRoom = false;

                // Make sure to only link rooms with bosses that are beaten.
                if (room.LevelType == GameTypes.LevelType.Castle && Game.PlayerStats.EyeballBossBeaten)
                {
                    linkChallengeBossRoom = true;
                }
                else if (room.LevelType == GameTypes.LevelType.Dungeon && Game.PlayerStats.BlobBossBeaten)
                {
                    linkChallengeBossRoom = true;
                }
                else if (room.LevelType == GameTypes.LevelType.Garden && Game.PlayerStats.FairyBossBeaten)
                {
                    linkChallengeBossRoom = true;
                }
                else if (room.LevelType == GameTypes.LevelType.Tower && Game.PlayerStats.FireballBossBeaten)
                {
                    linkChallengeBossRoom = true;
                }

                if (linkChallengeBossRoom)
                {
                    foreach (var door in room.DoorList)
                    {
                        if (door.IsBossDoor)
                        {
                            room.LinkedRoom = lastBossChallengeRoom;

                            foreach (var obj in room.GameObjList)
                            {
                                if (obj.Name == "BossDoor")
                                {
                                    // Change the door graphic to close
                                    if (Game.PlayerStats.ChallengeLastBossBeaten)
                                    {
                                        if ((obj as SpriteObj).SpriteName.Contains("Open"))
                                        {
                                            obj.ChangeSprite((obj as SpriteObj).SpriteName.Replace("Open", ""));
                                        }

                                        //obj.TextureColor = new Color(0, 255, 255);
                                        //obj.Opacity = 0.6f;
                                        obj.TextureColor = Color.White;
                                        obj.Opacity = 1;
                                        room.LinkedRoom = null;
                                        door.Locked = true;
                                    }
                                    else
                                    {
                                        // Change the door graphic to open
                                        if ((obj as SpriteObj).SpriteName.Contains("Open") == false)
                                        {
                                            obj.ChangeSprite(
                                                (obj as SpriteObj).SpriteName.Replace("_Sprite", "Open_Sprite"));
                                        }

                                        obj.TextureColor = new Color(0, 255, 255);
                                        obj.Opacity = 0.6f;

                                        // Unlock the door. It now leads to the challenge room.
                                        door.Locked = false;
                                    }

                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void OpenChallengeBossDoor(RoomObj linkerRoom, GameTypes.LevelType levelType)
    {
        var openSpecialBossDoor = false;

        switch (levelType)
        {
            case GameTypes.LevelType.Castle:
                if (Game.PlayerStats.EyeballBossBeaten && Game.PlayerStats.ChallengeEyeballBeaten == false &&
                    Game.PlayerStats.ChallengeEyeballUnlocked)
                {
                    openSpecialBossDoor = true;
                }

                break;
            case GameTypes.LevelType.Dungeon:
                if (Game.PlayerStats.BlobBossBeaten && Game.PlayerStats.ChallengeBlobBeaten == false &&
                    Game.PlayerStats.ChallengeBlobUnlocked)
                {
                    openSpecialBossDoor = true;
                }

                break;
            case GameTypes.LevelType.Garden:
                if (Game.PlayerStats.FairyBossBeaten && Game.PlayerStats.ChallengeSkullBeaten == false &&
                    Game.PlayerStats.ChallengeSkullUnlocked)
                {
                    openSpecialBossDoor = true;
                }

                break;
            case GameTypes.LevelType.Tower:
                if (Game.PlayerStats.FireballBossBeaten && Game.PlayerStats.ChallengeFireballBeaten == false &&
                    Game.PlayerStats.ChallengeFireballUnlocked)
                {
                    openSpecialBossDoor = true;
                }

                break;
        }

        if (openSpecialBossDoor)
        {
            var linkedRoom = LevelBuilder2.GetChallengeBossRoomFromRoomList(levelType, RoomList);
            linkerRoom.LinkedRoom = linkedRoom;

            foreach (var door in linkerRoom.DoorList)
            {
                if (door.IsBossDoor)
                {
                    // Change the door graphic to open
                    foreach (var obj in linkerRoom.GameObjList)
                    {
                        if (obj.Name == "BossDoor")
                        {
                            obj.ChangeSprite((obj as SpriteObj).SpriteName.Replace("_Sprite", "Open_Sprite"));
                            obj.TextureColor = new Color(0, 255, 255);
                            obj.Opacity = 0.6f;
                            break;
                        }
                    }

                    // Unlock the door. It now leads to the challenge room.
                    door.Locked = false;
                    break;
                }
            }
        }
    }

    public void AddRooms(List<RoomObj> roomsToAdd)
    {
        foreach (var room in roomsToAdd)
        {
            RoomList.Add(room);
            if (room.X < _leftMostBorder)
            {
                _leftMostBorder = (int)room.X;
            }

            if (room.X + room.Width > _rightMostBorder)
            {
                _rightMostBorder = (int)room.X + room.Width;
            }

            if (room.Y < _topMostBorder)
            {
                _topMostBorder = (int)room.Y;
            }

            if (room.Y + room.Height > _bottomMostBorder)
            {
                _bottomMostBorder = (int)room.Y + room.Height;
            }
        }
    }

    public void AddRoom(RoomObj room)
    {
        RoomList.Add(room);
        if (room.X < _leftMostBorder)
        {
            _leftMostBorder = (int)room.X;
        }

        if (room.X + room.Width > _rightMostBorder)
        {
            _rightMostBorder = (int)room.X + room.Width;
        }

        if (room.Y < _topMostBorder)
        {
            _topMostBorder = (int)room.Y;
        }

        if (room.Y + room.Height > _bottomMostBorder)
        {
            _bottomMostBorder = (int)room.Y + room.Height;
        }
    }

    private void CheckForRoomTransition()
    {
        if (Player != null)
        {
            foreach (var roomObj in RoomList)
            {
                if (roomObj != CurrentRoom)
                {
                    if (roomObj.Bounds.Contains((int)Player.X, (int)Player.Y))
                    {
                        // This was moved here. If causing problems, remove this one and uncomment the one lower in this function.
                        ResetEnemyPositions(); // Must be called before the current room is set. Resets the positions of all enemies in the previous room.

                        // Before changing rooms, reset enemy logic.
                        if (CurrentRoom != null)
                        {
                            foreach (var enemy in EnemyList)
                            {
                                enemy.ResetState();
                            }
                        }

                        if (EnemiesPaused)
                        {
                            UnpauseAllEnemies();
                        }

                        Player.RoomTransitionReset();

                        MiniMapDisplay.AddRoom(roomObj); // Add the room to the map display the moment you enter it.
                        // Save the player data and map data upon transition to new room.
                        if (roomObj.Name != "Start")
                        {
                            (ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.PlayerData, SaveType.MapData);
                        }

                        // Override texture colour if challenge room
                        if (roomObj.Name == "ChallengeBoss")
                        {
                            //m_foregroundSprite.TextureColor = Color.Black;
                            //m_backgroundSprite.TextureColor = Color.Black;
                            _backgroundSprite.Scale = Vector2.One;
                            _backgroundSprite.ChangeSprite("NeoBG_Sprite", ScreenManager.Camera);
                            _backgroundSprite.Scale = new Vector2(2, 2);

                            _foregroundSprite.Scale = Vector2.One;
                            _foregroundSprite.ChangeSprite("NeoFG_Sprite", ScreenManager.Camera);
                            _foregroundSprite.Scale = new Vector2(2, 2);
                        }
                        //else
                        //{
                        //    m_foregroundSprite.TextureColor = Color.White;
                        //    m_backgroundSprite.TextureColor = Color.White;
                        //}

                        // This code only happens if the level type you are entering is different from the previous one you were in.
                        if ((CurrentRoom == null || CurrentLevelType != roomObj.LevelType ||
                             (CurrentRoom != null && CurrentRoom.Name == "ChallengeBoss")) && roomObj.Name != "Start")
                        {
                            if (roomObj.Name != "ChallengeBoss")
                            {
                                switch (roomObj.LevelType)
                                {
                                    case GameTypes.LevelType.Castle:
                                        _backgroundSprite.Scale = Vector2.One;
                                        _foregroundSprite.Scale = Vector2.One;
                                        _backgroundSprite.ChangeSprite("CastleBG1_Sprite", ScreenManager.Camera);
                                        _foregroundSprite.ChangeSprite("CastleFG1_Sprite", ScreenManager.Camera);
                                        _backgroundSprite.Scale = new Vector2(2, 2);
                                        _foregroundSprite.Scale = new Vector2(2, 2);
                                        break;
                                    case GameTypes.LevelType.Tower:
                                        _backgroundSprite.Scale = Vector2.One;
                                        _foregroundSprite.Scale = Vector2.One;
                                        _backgroundSprite.ChangeSprite("TowerBG2_Sprite", ScreenManager.Camera);
                                        _foregroundSprite.ChangeSprite("TowerFG2_Sprite", ScreenManager.Camera);
                                        _backgroundSprite.Scale = new Vector2(2, 2);
                                        _foregroundSprite.Scale = new Vector2(2, 2);
                                        break;
                                    case GameTypes.LevelType.Dungeon:
                                        _backgroundSprite.Scale = Vector2.One;
                                        _foregroundSprite.Scale = Vector2.One;
                                        _backgroundSprite.ChangeSprite("DungeonBG1_Sprite", ScreenManager.Camera);
                                        _foregroundSprite.ChangeSprite("DungeonFG1_Sprite", ScreenManager.Camera);
                                        _backgroundSprite.Scale = new Vector2(2, 2);
                                        _foregroundSprite.Scale = new Vector2(2, 2);
                                        break;
                                    case GameTypes.LevelType.Garden:
                                        _backgroundSprite.Scale = Vector2.One;
                                        _foregroundSprite.Scale = Vector2.One;
                                        _backgroundSprite.ChangeSprite("GardenBG_Sprite", ScreenManager.Camera);
                                        _foregroundSprite.ChangeSprite("GardenFG_Sprite", ScreenManager.Camera);
                                        _backgroundSprite.Scale = new Vector2(2, 2);
                                        _foregroundSprite.Scale = new Vector2(2, 2);
                                        break;
                                }
                            }

                            if (Game.PlayerStats.HasTrait(TraitType.THE_ONE))
                            {
                                _foregroundSprite.Scale = Vector2.One;
                                _foregroundSprite.ChangeSprite("NeoFG_Sprite", ScreenManager.Camera);
                                _foregroundSprite.Scale = new Vector2(2, 2);
                            }

                            // Setting shadow intensity.
                            if (roomObj.LevelType == GameTypes.LevelType.Dungeon ||
                                Game.PlayerStats.HasTrait(TraitType.GLAUCOMA) || roomObj.Name == "Compass")
                            {
                                Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0.7f);
                            }
                            else
                            {
                                Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0);
                            }

                            // Tower frame parallaxing effect.
                            //if (roomObj.LevelType == GameTypes.LevelType.TOWER)
                            //{
                            //    m_gameObjStartPos.Clear();
                            //    foreach (GameObj obj in roomObj.GameObjList)
                            //        m_gameObjStartPos.Add(obj.Position);
                            //}

                            //m_roomTitle.Text = "Now Entering\n" + WordBuilder.BuildDungeonName(roomObj.LevelType);
                            _roomTitle.Text = WordBuilder.BuildDungeonNameLocID(roomObj.LevelType)
                                .GetString(_roomTitle);
                            if (Game.PlayerStats.HasTrait(TraitType.DYSLEXIA))
                            {
                                _roomTitle.RandomizeSentence(false);
                            }

                            _roomTitle.Opacity = 0;

                            if (roomObj.Name != "Boss" && roomObj.Name != "Tutorial" && roomObj.Name != "Ending" &&
                                roomObj.Name != "ChallengeBoss") // && roomObj.Name != "CastleEntrance")
                            {
                                Tween.StopAllContaining(_roomEnteringTitle, false);
                                Tween.StopAllContaining(_roomTitle, false);
                                _roomTitle.Opacity = 0;
                                _roomEnteringTitle.Opacity = 0;

                                if (Player.X > roomObj.Bounds.Center.X)
                                {
                                    _roomTitle.X = 50;
                                    _roomTitle.Align = Types.TextAlign.Left;
                                    _roomEnteringTitle.X = 70;
                                    _roomEnteringTitle.Align = Types.TextAlign.Left;
                                }
                                else
                                {
                                    _roomTitle.X = 1320 - 50;
                                    _roomTitle.Align = Types.TextAlign.Right;
                                    _roomEnteringTitle.X = 1320 - 70;
                                    _roomEnteringTitle.Align = Types.TextAlign.Right;
                                }

                                Tween.To(_roomTitle, 0.5f, Linear.EaseNone, "delay", "0.2", "Opacity", "1");
                                _roomTitle.Opacity =
                                    1; // This is necessary because the tweener stores the initial value of the property when it is called.
                                Tween.To(_roomTitle, 0.5f, Linear.EaseNone, "delay", "2.2", "Opacity", "0");
                                _roomTitle.Opacity = 0;

                                Tween.To(_roomEnteringTitle, 0.5f, Linear.EaseNone, "Opacity", "1");
                                _roomEnteringTitle.Opacity =
                                    1; // This is necessary because the tweener stores the initial value of the property when it is called.
                                Tween.To(_roomEnteringTitle, 0.5f, Linear.EaseNone, "delay", "2", "Opacity", "0");
                                _roomEnteringTitle.Opacity = 0;
                            }
                            else
                            {
                                Tween.StopAllContaining(_roomEnteringTitle, false);
                                Tween.StopAllContaining(_roomTitle, false);
                                _roomTitle.Opacity = 0;
                                _roomEnteringTitle.Opacity = 0;
                            }

                            JukeboxEnabled = false;
                            Console.WriteLine("Now entering " + roomObj.LevelType);
                        }

                        //ResetEnemyPositions(); // Must be called before the current room is set. Resets the positions of all enemies in the previous room.

                        if (_currentRoom != null)
                        {
                            _currentRoom
                                .OnExit(); // Call on exit if exiting from a room. This also removes all dementia enemies from the room. IMPORTANT.
                        }

                        _currentRoom = roomObj; // Sets to newly entered room to be the current room.

                        // Necessary to keep track of which room the player is in otherwise it won't load in the correct room at start up.
                        //if (m_currentRoom.Name == "Boss" && LevelEV.RUN_TESTROOM == false)
                        //    Game.PlayerStats.RespawnPos = m_currentRoom.LinkedRoom.Position;
                        //(ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.PlayerData); // Saving player data.

                        //if (m_currentRoom.Name != "Start" && m_currentRoom.Name != "CastleEntrance")
                        //  (ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.MapData); // Saving map data.

                        _backgroundSprite.Position = CurrentRoom.Position;
                        _foregroundSprite.Position = CurrentRoom.Position;
                        _gardenParallaxFG.Position = CurrentRoom.Position;

                        if (SoundManager.IsMusicPaused)
                        {
                            SoundManager.ResumeMusic();
                        }

                        if (DisableSongUpdating == false && JukeboxEnabled == false)
                        {
                            UpdateLevelSong();
                        }

                        if (_currentRoom.Player == null)
                        {
                            _currentRoom.Player = Player;
                        }
                        //m_currentRoom.OnEnter();

                        if (_currentRoom.Name != "Start" && _currentRoom.Name != "Tutorial" &&
                            _currentRoom.Name != "Ending" &&
                            _currentRoom.Name != "CastleEntrance" && _currentRoom.Name != "Bonus" &&
                            _currentRoom.Name != "Throne" &&
                            _currentRoom.Name != "Secret" && _currentRoom.Name != "Boss" &&
                            _currentRoom.LevelType != GameTypes.LevelType.None &&
                            _currentRoom.Name != "ChallengeBoss")
                        {
                            if (Game.PlayerStats.HasTrait(TraitType.DEMENTIA))
                            {
                                if (CDGMath.RandomFloat(0, 1) < GameEV.TRAIT_DEMENTIA_SPAWN_CHANCE)
                                {
                                    SpawnDementiaEnemy();
                                }
                            }
                        }

                        if (_currentRoom.HasFairyChest) // && ScreenManager.GetScreens().Length > 0)
                        {
                            _currentRoom.DisplayFairyChestInfo();
                        }

                        _tempEnemyStartPositions.Clear();
                        _enemyStartPositions.Clear(); // Clear out the start position array.
                        foreach (var enemy in
                                 CurrentRoom
                                     .EnemyList) // Saves all enemy positions in the new room to an array for when the player exits the room.
                        {
                            _enemyStartPositions.Add(enemy.Position);
                        }

                        foreach (var enemy in
                                 CurrentRoom
                                     .TempEnemyList) // Saves all enemy positions in the new room to an array for when the player exits the room.
                        {
                            _tempEnemyStartPositions.Add(enemy.Position);
                        }

                        _projectileManager.DestroyAllProjectiles(false);
                        LoadPhysicsObjects(roomObj);
                        //m_miniMapDisplay.AddRoom(roomObj); // Add the room to the map display.
                        _itemDropManager.DestroyAllItemDrops();
                        _projectileIconPool.DestroyAllIcons(); // Destroys all icons for projectiles in the room.

                        _enemyPauseDuration =
                            0; // Resets the enemy pause counter. Don't unpause all enemies because they will unpause when Enemy.ResetState() is called.


                        if (LevelEV.ShowEnemyRadii)
                        {
                            foreach (var enemy in roomObj.EnemyList)
                            {
                                enemy.InitializeDebugRadii();
                            }
                        }

                        _lastEnemyHit = null; // Clear out last enemy hit.

                        foreach (var obj in _currentRoom.GameObjList)
                        {
                            var chest = obj as FairyChestObj;
                            if (chest != null && chest.IsOpen == false) // Always reset chests.
                            {
                                //chest.State = ChestConditionChecker.STATE_LOCKED;
                                //chest.TextureColor = Color.White;
                                //chest.ResetChest();
                            }

                            //ObjContainer objContainer = obj as ObjContainer;
                            var objContainer = obj as IAnimateableObj;
                            // This is the code that sets the frame rate and whether to animate objects in the room.
                            if (objContainer != null && objContainer.TotalFrames > 1 && !(objContainer is ChestObj) &&
                                !(obj is BreakableObj)) // What's this code for?
                            {
                                objContainer.AnimationDelay = 1 / 10f;
                                objContainer.PlayAnimation();
                            }
                        }

                        if (DisableRoomOnEnter == false)
                        {
                            _currentRoom.OnEnter();
                        }

                        break;
                    }
                }
            }
        }
    }

    private void UpdateLevelSong()
    {
        //if (!(m_currentRoom is StartingRoomObj) && !(m_currentRoom is IntroRoomObj) && SoundManager.IsMusicPlaying == false)
        if (CurrentRoom.Name != "Start" && CurrentRoom.Name != "Tutorial" && CurrentRoom.Name != "Ending" &&
            SoundManager.IsMusicPlaying == false)
        {
            if (_currentRoom is CarnivalShoot1BonusRoom || _currentRoom is CarnivalShoot2BonusRoom)
            {
                SoundManager.PlayMusic("PooyanSong", true, 1);
            }
            else
            {
                switch (_currentRoom.LevelType)
                {
                    default:
                    case GameTypes.LevelType.Castle:
                        SoundManager.PlayMusic("CastleSong", true, 1);
                        break;
                    case GameTypes.LevelType.Garden:
                        SoundManager.PlayMusic("GardenSong", true, 1);
                        break;
                    case GameTypes.LevelType.Tower:
                        SoundManager.PlayMusic("TowerSong", true, 1);
                        break;
                    case GameTypes.LevelType.Dungeon:
                        SoundManager.PlayMusic("DungeonSong", true, 1);
                        break;
                }
            }
        }
        else if (!(_currentRoom is StartingRoomObj) && SoundManager.IsMusicPlaying)
        {
            if ((_currentRoom is CarnivalShoot1BonusRoom || _currentRoom is CarnivalShoot2BonusRoom) &&
                SoundManager.GetCurrentMusicName() != "PooyanSong")
            {
                SoundManager.PlayMusic("PooyanSong", true, 1);
            }
            else
            {
                if (_currentRoom.LevelType == GameTypes.LevelType.Castle &&
                    SoundManager.GetCurrentMusicName() != "CastleSong")
                {
                    SoundManager.PlayMusic("CastleSong", true, 1);
                }
                else if (_currentRoom.LevelType == GameTypes.LevelType.Garden &&
                         SoundManager.GetCurrentMusicName() != "GardenSong")
                {
                    SoundManager.PlayMusic("GardenSong", true, 1);
                }
                else if (_currentRoom.LevelType == GameTypes.LevelType.Dungeon &&
                         SoundManager.GetCurrentMusicName() != "DungeonSong")
                {
                    SoundManager.PlayMusic("DungeonSong", true, 1);
                }
                else if (_currentRoom.LevelType == GameTypes.LevelType.Tower &&
                         SoundManager.GetCurrentMusicName() != "TowerSong")
                {
                    SoundManager.PlayMusic("TowerSong", true, 1);
                }
            }
        }
    }

    public override void Update(GameTime gameTime)
    {
        //if (InputManager.JustPressed(Keys.L, PlayerIndex.One))
        //{
        //    if (Camera.Zoom != 2)
        //        Camera.Zoom = 2;
        //    else
        //        Camera.Zoom = 1;
        //}
        //if (InputManager.JustPressed(Keys.K, PlayerIndex.One))
        //{
        //    if (Camera.Zoom != 0.5)
        //        Camera.Zoom = 0.5f;
        //    else
        //        Camera.Zoom = 1;
        //}

        //if (InputManager.JustPressed(Keys.B, null))
        //    this.ResetEnemyPositions();

        var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
        _projectileIconPool.Update(Camera);

        if (IsPaused == false)
        {
            //TotalGameTimeHours = (float)gameTime.TotalGameTime.TotalHours;
            var elapsedTotalHours = (float)gameTime.ElapsedGameTime.TotalHours;
            if (elapsedTotalHours <= 0) // This is a check to ensure total GameTime is always incremented.
            {
                elapsedTotalHours = _fakeElapsedTotalHour;
            }

            Game.HoursPlayedSinceLastSave += elapsedTotalHours;

            _sky.Update(gameTime);

            if (_enemyPauseDuration > 0)
            {
                _enemyPauseDuration -= elapsed;
                if (_enemyPauseDuration <= 0)
                {
                    StopTimeStop();
                }
            }

            CurrentRoom.Update(gameTime);

            if (Player != null)
            {
                Player.Update(gameTime);
            }

            _enemyHUD.Update(gameTime);
            _playerHUD.Update(Player);

            _projectileManager.Update(gameTime);
            _physicsManager.Update(gameTime);

            // Only check for room transitions if the player steps out of the camera zone.
            if (DisableRoomTransitioning == false &&
                CollisionMath.Intersects(new Rectangle((int)Player.X, (int)Player.Y, 1, 1), Camera.Bounds) == false)
            {
                CheckForRoomTransition();
            }

            if ((_inputMap.Pressed(INPUT_LEFTCONTROL) == false ||
                 (_inputMap.Pressed(INPUT_LEFTCONTROL) && LevelEV.CreateRetailVersion)) && CameraLockedToPlayer)
            {
                UpdateCamera(); // Must be called AFTER the PhysicsManager Update() because the PhysicsManager changes the player's position depending on what he/she is colliding with.
            }

            if (Game.PlayerStats.SpecialItem == SpecialItemType.COMPASS && CurrentRoom.Name != "Start" &&
                CurrentRoom.Name != "Tutorial" && CurrentRoom.Name != "Boss" && CurrentRoom.Name != "Throne" &&
                CurrentRoom.Name != "ChallengeBoss")
            {
                if (_compassDisplayed == false) // Display compass here
                {
                    DisplayCompass();
                }
                else
                {
                    UpdateCompass();
                }
            }
            else
            {
                if (_compassDisplayed && CurrentRoom.Name != "Compass")
                {
                    HideCompass();
                }
            }

            // This means the objective plate is displayed. Now we are checking to make sure if any enemy of player collides with it, change its opacity.
            if (_objectivePlate.X == 1170)
            {
                var objectivePlateCollides = false;
                var objectivePlateAbsRect = _objectivePlate.Bounds;
                objectivePlateAbsRect.X += (int)Camera.TopLeftCorner.X;
                objectivePlateAbsRect.Y += (int)Camera.TopLeftCorner.Y;

                if (CollisionMath.Intersects(Player.Bounds, objectivePlateAbsRect))
                {
                    objectivePlateCollides = true;
                }

                if (objectivePlateCollides == false)
                {
                    foreach (var enemy in CurrentRoom.EnemyList)
                    {
                        if (CollisionMath.Intersects(enemy.Bounds, objectivePlateAbsRect))
                        {
                            objectivePlateCollides = true;
                            break;
                        }
                    }
                }

                if (objectivePlateCollides)
                {
                    _objectivePlate.Opacity = 0.5f;
                }
                else
                {
                    _objectivePlate.Opacity = 1;
                }
            }

            if (CurrentRoom != null && CurrentRoom is BonusRoomObj == false)
            {
                if (_elapsedScreenShake > 0)
                {
                    _elapsedScreenShake -= elapsed;
                    if (_elapsedScreenShake <= 0)
                    {
                        if (Game.PlayerStats.HasTrait(TraitType.CLONUS))
                        {
                            ShakeScreen(1);
                            GamePad.SetVibration(PlayerIndex.One, 0.25f, 0.25f);
                            Tween.RunFunction(CDGMath.RandomFloat(1, 1.5f), this, "StopScreenShake");
                            _elapsedScreenShake =
                                CDGMath.RandomFloat(GameEV.TRAIT_CLONUS_MIN, GameEV.TRAIT_CLONUS_MAX);
                        }
                    }
                }

                if (_shakeScreen)
                {
                    UpdateShake();
                }
            }
        }

        base.Update(gameTime); // Necessary to update the ScreenManager.
    }

    public void UpdateCamera()
    {
        if (Player != null)
        {
            ScreenManager.Camera.X = (int)(Player.Position.X + GlobalEV.CameraXOffset);
            ScreenManager.Camera.Y = (int)(Player.Position.Y + GlobalEV.CameraYOffset);
        }

        if (_currentRoom != null)
        {
            //Constrain the X-Axis of the camera to the current room.
            if (ScreenManager.Camera.Width < _currentRoom.Width)
            {
                if (ScreenManager.Camera.Bounds.Left < _currentRoom.Bounds.Left)
                {
                    ScreenManager.Camera.X = (int)(_currentRoom.Bounds.Left + (ScreenManager.Camera.Width * 0.5f));
                }
                else if (ScreenManager.Camera.Bounds.Right > _currentRoom.Bounds.Right)
                {
                    ScreenManager.Camera.X = (int)(_currentRoom.Bounds.Right - (ScreenManager.Camera.Width * 0.5f));
                }
            }
            else
            {
                ScreenManager.Camera.X = (int)(_currentRoom.X + (_currentRoom.Width * 0.5f));
            }

            //Constrain the Y-Axis of the camera to the current room.
            if (ScreenManager.Camera.Height < _currentRoom.Height)
            {
                if (ScreenManager.Camera.Bounds.Top < _currentRoom.Bounds.Top)
                {
                    ScreenManager.Camera.Y = (int)(_currentRoom.Bounds.Top + (ScreenManager.Camera.Height * 0.5f));
                }
                else if (ScreenManager.Camera.Bounds.Bottom > _currentRoom.Bounds.Bottom)
                {
                    ScreenManager.Camera.Y = (int)(_currentRoom.Bounds.Bottom - (ScreenManager.Camera.Height * 0.5f));
                }
            }
            else
            {
                ScreenManager.Camera.Y = (int)(_currentRoom.Y + (_currentRoom.Height * 0.5f));
            }
        }
    }

    //HandleInput is called AFTER Update().
    public override void HandleInput()
    {
        if (Game.GlobalInput.JustPressed(InputMapType.MENU_PAUSE) && CurrentRoom.Name != "Ending")
        {
            (ScreenManager as RCScreenManager).DisplayScreen(ScreenType.PAUSE, true);
        }

        if (LevelEV.EnableDebugInput)
        {
            HandleDebugInput();
        }

        if (Player != null &&
            (_inputMap.Pressed(INPUT_LEFTCONTROL) == false ||
             (_inputMap.Pressed(INPUT_LEFTCONTROL) && LevelEV.CreateRetailVersion)) && Player.IsKilled == false)
        {
            Player.HandleInput();
        }

        base.HandleInput();
    }

    private void HandleDebugInput()
    {
        if (InputManager.JustPressed(Keys.RightControl, null))
        {
            if (SoundManager.GetCurrentMusicName() == "CastleSong")
            {
                SoundManager.PlayMusic("TowerSong", true, 0.5f);
            }
            else if (SoundManager.GetCurrentMusicName() == "TowerSong")
            {
                SoundManager.PlayMusic("DungeonBoss", true, 0.5f);
            }
            else
            {
                SoundManager.PlayMusic("CastleSong", true, 0.5f);
            }
        }

        if (_inputMap.JustPressed(INPUT_TOGGLEMAP))
        {
            MiniMapDisplay.AddAllRooms(RoomList);
            //(ScreenManager as RCScreenManager).AddRoomsToMap(m_miniMapDisplay.AddedRoomsList);
            //(ScreenManager as RCScreenManager).DisplayScreen(ScreenType.Map, true, null);
        }

        if (_inputMap.JustPressed(INPUT_DISPLAYROOMINFO))
        {
            LevelEV.ShowDebugText = !LevelEV.ShowDebugText;
        }

        if (_inputMap.JustPressed(INPUT_TOGGLEZOOM))
        {
            //CameraLockedToPlayer = false;
            if (Camera.Zoom < 1)
            {
                Camera.Zoom = 1;
            }
            else
                //Tween.To(Camera, 4, Quad.EaseInOut, "Zoom", "0.05");
            {
                Camera.Zoom = 0.05f;
            }
        }

        float debugCameraSpeed = 2000;
        if (_inputMap.Pressed(INPUT_LEFTCONTROL) && _inputMap.Pressed(INPUT_LEFT))
        {
            Camera.X -= debugCameraSpeed * (float)Camera.GameTime.ElapsedGameTime.TotalSeconds;
        }
        else if (_inputMap.Pressed(INPUT_LEFTCONTROL) && _inputMap.Pressed(INPUT_RIGHT))
        {
            Camera.X += debugCameraSpeed * (float)Camera.GameTime.ElapsedGameTime.TotalSeconds;
        }

        if (_inputMap.Pressed(INPUT_LEFTCONTROL) && _inputMap.Pressed(INPUT_UP))
        {
            Camera.Y -= debugCameraSpeed * (float)Camera.GameTime.ElapsedGameTime.TotalSeconds;
        }
        else if (_inputMap.Pressed(INPUT_LEFTCONTROL) && _inputMap.Pressed(INPUT_DOWN))
        {
            Camera.Y += debugCameraSpeed * (float)Camera.GameTime.ElapsedGameTime.TotalSeconds;
        }

        if (InputManager.JustPressed(Keys.C, null))
        {
            ToggleMagentaBG();
        }

        //if (InputManager.JustPressed(Keys.H, null))
        //    ZoomOutAllObjects();
    }

    private void UpdateCompass()
    {
        if (_compassDoor == null && CurrentRoom.Name != "Ending" && CurrentRoom.Name != "Boss" &&
            CurrentRoom.Name != "Start" && CurrentRoom.Name != "Tutorial" && CurrentRoom.Name != " ChallengeBoss")
        {
            Console.WriteLine("Creating new bonus room for compass");
            RoomObj roomToLink = null;
            EnemyObj enemyToLink = null;

            var acceptableRooms = new List<RoomObj>();
            foreach (var room in RoomList)
            {
                var hasEnemies = false;
                foreach (var enemy in room.EnemyList)
                {
                    if (enemy.IsWeighted)
                    {
                        hasEnemies = true;
                        break;
                    }
                }

                // No need to check for CastleEntrance or linker because they have no enemies in them.
                if (room.Name != "Ending" && room.Name != "Tutorial" && room.Name != "Boss" && room.Name != "Secret" &&
                    room.Name != "Bonus" && hasEnemies && room.Name != "ChallengeBoss")
                {
                    acceptableRooms.Add(room);
                }
            }

            if (acceptableRooms.Count > 0)
            {
                roomToLink = acceptableRooms[CDGMath.RandomInt(0, acceptableRooms.Count - 1)];
                var counter = 0;
                while (enemyToLink == null || enemyToLink.IsWeighted == false)
                {
                    enemyToLink = roomToLink.EnemyList[counter];
                    counter++;
                }

                var door = new DoorObj(roomToLink, 120, 180, GameTypes.DoorType.Open);
                door.Position = enemyToLink.Position;
                door.IsBossDoor = true;
                door.DoorPosition = "None";
                door.AddCollisionBox(0, 0, door.Width, door.Height,
                    Consts.TERRAIN_HITBOX); // This adds the terrain collision box for terrain objects.
                door.AddCollisionBox(0, 0, door.Width, door.Height,
                    Consts.BODY_HITBOX); // This adds a body collision box to terrain objects.

                var closestGround = float.MaxValue;
                TerrainObj closestTerrain = null;
                foreach (var terrainObj in roomToLink.TerrainObjList)
                {
                    if (terrainObj.Y >= door.Y)
                    {
                        if (terrainObj.Y - door.Y < closestGround && CollisionMath.Intersects(terrainObj.Bounds,
                                new Rectangle((int)door.X, (int)(door.Y + (terrainObj.Y - door.Y) + 5), door.Width,
                                    door.Height / 2)))
                        {
                            closestGround = terrainObj.Y - door.Y;
                            closestTerrain = terrainObj;
                        }
                    }
                }

                if (closestTerrain != null)
                {
                    door.UpdateCollisionBoxes();
                    if (closestTerrain.Rotation == 0)
                    {
                        door.Y = closestTerrain.Y - (door.TerrainBounds.Bottom - door.Y);
                    }
                    else
                    {
                        HookEnemyToSlope(door, closestTerrain);
                    }
                }

                roomToLink.DoorList.Add(door);

                roomToLink.LinkedRoom = RoomList[RoomList.Count - 1]; // The last room is always the compass room.
                roomToLink.LinkedRoom.LinkedRoom = roomToLink;
                roomToLink.LinkedRoom.LevelType = roomToLink.LevelType;

                var castleCornerTextureString = "CastleCorner_Sprite";
                var castleCornerLTextureString = "CastleCornerL_Sprite";
                var towerCornerTextureString = "TowerCorner_Sprite";
                var towerCornerLTextureString = "TowerCornerL_Sprite";
                var dungeonCornerTextureString = "DungeonCorner_Sprite";
                var dungeonCornerLTextureString = "DungeonCornerL_Sprite";
                var gardenCornerTextureString = "GardenCorner_Sprite";
                var gardenCornerLTextureString = "GardenCornerL_Sprite";

                if (Game.PlayerStats.HasTrait(TraitType.THE_ONE))
                {
                    var futureCornerTextureString = "NeoCorner_Sprite";
                    var futureCornerLTextureString = "NeoCornerL_Sprite";
                    castleCornerLTextureString = dungeonCornerLTextureString = towerCornerLTextureString =
                        gardenCornerLTextureString = futureCornerLTextureString;
                    castleCornerTextureString = dungeonCornerTextureString =
                        towerCornerTextureString = gardenCornerTextureString = futureCornerTextureString;
                }

                foreach (var border in roomToLink.LinkedRoom.BorderList)
                {
                    switch (roomToLink.LinkedRoom.LevelType)
                    {
                        case GameTypes.LevelType.Tower:
                            border.SetBorderTextures(_towerBorderTexture, towerCornerTextureString,
                                towerCornerLTextureString);
                            break;
                        case GameTypes.LevelType.Dungeon:
                            border.SetBorderTextures(_dungeonBorderTexture, dungeonCornerTextureString,
                                dungeonCornerLTextureString);
                            break;
                        case GameTypes.LevelType.Garden:
                            border.SetBorderTextures(_gardenBorderTexture, gardenCornerTextureString,
                                gardenCornerLTextureString);
                            border.TextureOffset = new Vector2(0, -18);
                            break;
                        case GameTypes.LevelType.Castle:
                        default:
                            border.SetBorderTextures(_castleBorderTexture, castleCornerTextureString,
                                castleCornerLTextureString);
                            break;
                    }
                }

                _compassDoor = door;
            }
        }

        if (_compassDoor != null)
        {
            _compass.Rotation = CDGMath.AngleBetweenPts(Player.Position,
                new Vector2(_compassDoor.Bounds.Center.X, _compassDoor.Bounds.Center.Y));
        }
    }

    public void RemoveCompassDoor()
    {
        if (_compassDoor != null)
        {
            _compassDoor.Room.DoorList.Remove(_compassDoor);
            _compassDoor.Dispose();
            _compassDoor = null;
        }
    }

    private void DisplayCompass()
    {
        Tween.StopAllContaining(_compassBG, false);
        Tween.StopAllContaining(_compass, false);
        Tween.To(_compassBG, 0.5f, Back.EaseOutLarge, "ScaleX", "1", "ScaleY", "1");
        Tween.To(_compass, 0.5f, Back.EaseOutLarge, "ScaleX", "1", "ScaleY", "1");
        _compassDisplayed = true;
    }

    private void HideCompass()
    {
        Tween.StopAllContaining(_compassBG, false);
        Tween.StopAllContaining(_compass, false);
        Tween.To(_compassBG, 0.5f, Back.EaseInLarge, "ScaleX", "0", "ScaleY", "0");
        Tween.To(_compass, 0.5f, Back.EaseInLarge, "ScaleX", "0", "ScaleY", "0");
        _compassDisplayed = false;
        RemoveCompassDoor();
    }

    public void InitializeRenderTargets()
    {
        var screenWidth = GlobalEV.SCREEN_WIDTH;
        var screenHeight = GlobalEV.SCREEN_HEIGHT;

        if (LevelEV.SaveFrames)
        {
            screenWidth /= 2;
            screenHeight /= 2;
        }

        // Initializing foreground render target.
        if (_fgRenderTarget != null)
        {
            _fgRenderTarget.Dispose();
        }

        _fgRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, screenWidth, screenHeight, false, fgTargetFormat,
            DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

        if (_shadowRenderTarget != null)
        {
            _shadowRenderTarget.Dispose();
        }

        _shadowRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, screenWidth, screenHeight, false,
            effectTargetFormat, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

        Camera.Begin();
        Camera.GraphicsDevice.SetRenderTarget(_shadowRenderTarget);
        Camera.GraphicsDevice.Clear(Color.Black); // Requires no wrap.
        Camera.End();

        if (_lightSourceRenderTarget != null)
        {
            _lightSourceRenderTarget.Dispose();
        }

        _lightSourceRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, screenWidth, screenHeight, false,
            effectTargetFormat, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

        if (RenderTarget != null)
        {
            RenderTarget.Dispose();
        }

        RenderTarget = new RenderTarget2D(Camera.GraphicsDevice, GlobalEV.SCREEN_WIDTH, GlobalEV.SCREEN_HEIGHT, false,
            SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

        if (_skyRenderTarget != null)
        {
            _skyRenderTarget.Dispose();
        }

        _skyRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, screenWidth, screenHeight, false,
            SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

        if (_bgRenderTarget != null)
        {
            _bgRenderTarget.Dispose();
        }

        _bgRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, GlobalEV.SCREEN_WIDTH, GlobalEV.SCREEN_HEIGHT,
            false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

        if (_traitAuraRenderTarget != null)
        {
            _traitAuraRenderTarget.Dispose();
        }

        _traitAuraRenderTarget = new RenderTarget2D(Camera.GraphicsDevice, screenWidth, screenHeight, false,
            SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);

        InitializeBackgroundObjs();
    }

    public void InitializeBackgroundObjs()
    {
        if (_foregroundSprite != null)
        {
            _foregroundSprite.Dispose();
        }

        _foregroundSprite = new BackgroundObj("CastleFG1_Sprite");
        _foregroundSprite.SetRepeated(true, true, Camera, SamplerState.PointWrap);
        _foregroundSprite.Scale = new Vector2(2, 2);

        /////////////////////////////////////////////////////////

        // Initializing background render target.
        if (_backgroundSprite != null)
        {
            _backgroundSprite.Dispose();
        }

        _backgroundSprite = new BackgroundObj("CastleBG1_Sprite");
        _backgroundSprite.SetRepeated(true, true, Camera,
            SamplerState.PointWrap); // Must be called before anything else.
        _backgroundSprite.Scale = new Vector2(2f, 2f);

        if (_backgroundParallaxSprite != null)
        {
            _backgroundParallaxSprite.Dispose();
        }

        _backgroundParallaxSprite = new BackgroundObj("TowerBGFrame_Sprite");
        _backgroundParallaxSprite.SetRepeated(true, true, Camera, SamplerState.PointWrap);
        _backgroundParallaxSprite.Scale = new Vector2(2, 2);

        /////////////////////////////////////////////////////////
        // Initializing the parallaxing background render target.
        if (_gardenParallaxFG != null)
        {
            _gardenParallaxFG.Dispose();
        }

        _gardenParallaxFG = new BackgroundObj("ParallaxDifferenceClouds_Sprite");
        _gardenParallaxFG.SetRepeated(true, true, Camera, SamplerState.LinearWrap);
        _gardenParallaxFG.TextureColor = Color.White;
        //m_gardenParallaxFG.ForceDraw = true;
        _gardenParallaxFG.Scale = new Vector2(3, 3);
        _gardenParallaxFG.Opacity = 0.7f;
        _gardenParallaxFG.ParallaxSpeed = new Vector2(0.3f, 0);
    }

    // All objects that need to be drawn on a render target before being drawn the back buffer go here.
    public void DrawRenderTargets()
    {
        // This happens if graphics virtualization fails two times in a row.  Buggy XNA RenderTarget2Ds.
        if (_backgroundSprite.Texture.IsContentLost)
        {
            ReinitializeRTs();
        }

        Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null,
            Camera.GetTransformation());
        // Drawing the B/W outline of the room to wallpaper on the FG and BG later.
        if (CurrentRoom != null)
        {
            CurrentRoom.DrawRenderTargets(Camera); // Requires LinearWrap.
        }

        Camera.End();

        ///////// ALL DRAW CALLS THAT REQUIRE A MATRIX TRANSFORMATION GO HERE /////////////////
        Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null,
            Camera.GetTransformation());

        // Drawing the tiled foreground onto m_fgRenderTarget.
        Camera.GraphicsDevice.SetRenderTarget(_fgRenderTarget);
        _foregroundSprite.Draw(Camera); // Requires PointWrap.

        // Setting sampler state to Linear Wrap since most RTs below require it.
        //Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

        // Drawing the trait aura onto m_traitAuraRenderTarget (used for trait effects).
        if (EnemiesPaused == false)
        {
            if (Game.PlayerStats.HasTrait(TraitType.NEAR_SIGHTED))
            {
                _traitAura.Scale = new Vector2(15, 15);
            }
            else if (Game.PlayerStats.HasTrait(TraitType.FAR_SIGHTED))
            {
                _traitAura.Scale = new Vector2(8, 8);
            }
            else
            {
                _traitAura.Scale = new Vector2(10, 10);
            }
        }

        Camera.GraphicsDevice.SetRenderTarget(_traitAuraRenderTarget);
        Camera.GraphicsDevice.Clear(Color.Transparent);
        if (CurrentRoom != null)
        {
            _traitAura.Position = Player.Position;
            _traitAura.Draw(Camera); // Requires LinearWrap.
        }

        // Drawing a light source onto a transparent m_lightSourceRenderTarget (used for dungeon lighting).
        Camera.GraphicsDevice.SetRenderTarget(_lightSourceRenderTarget);
        Camera.GraphicsDevice.Clear(Color.Transparent);
        if (CurrentRoom != null)
        {
            _dungeonLight.Position = Player.Position;
            _dungeonLight.Draw(Camera); // Requires LinearWrap.
        }

        // Drawing a completely black RT onto m_shadowRenderTarget for the shadows in the dungeon.
        //Camera.GraphicsDevice.SetRenderTarget(m_shadowRenderTarget);
        //Camera.GraphicsDevice.Clear(Color.Black); // Requires no wrap.
        Camera.End();

        // Separated the mini map draw calls to deferred to speed up performance. Had to separate them from the sky render target.
        ///////// ALL DRAW CALLS THAT DO NOT REQUIRE A MATRIX TRANSFORMATION GO HERE /////////////////
        Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);

        // Making the map render targets.
        MiniMapDisplay.DrawRenderTargets(Camera); // Requires PointClamp
        Camera.End();

        // Drawing the sky parallax background to m_skyRenderTarget.
        Camera.GraphicsDevice.SetRenderTarget(_skyRenderTarget);
        Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null);
        //Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
        _sky.Draw(Camera); // Requires PointWrap.
        Camera.End();

        // Setting the render target back to the main render target.
        //Camera.GraphicsDevice.SetRenderTarget(m_finalRenderTarget);
    }

    private static Vector2 MoveInCircle(GameTime gameTime, float speed)
    {
        double time = Game.TotalGameTime * speed;

        var x = (float)Math.Cos(time);
        var y = (float)Math.Sin(time);

        return new Vector2(x, y);
    }

    private void ToggleMagentaBG()
    {
        _toggleMagentaBG = !_toggleMagentaBG;
    }

    public override void Draw(GameTime gameTime)
    {
        //Camera.Zoom = 2;
        //BackBufferOpacity = 1; //TEDDY - ADDED FOR THE BLACKENING OF THE BG FOR SNAPSHOTS
        DrawRenderTargets();

        Camera.GraphicsDevice.SetRenderTarget(_bgRenderTarget);
        ///////// DRAWING BACKGROUND /////////////////////////
        // If the foreground and background effect are merged into one effect, this draw call can be removed.
        Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null,
            Camera.GetTransformation());
        _backgroundSprite.Draw(Camera);

        if (CurrentRoom != null && Camera.Zoom == 1 && (_inputMap.Pressed(INPUT_LEFTCONTROL) == false ||
                                                        (_inputMap.Pressed(INPUT_LEFTCONTROL) &&
                                                         LevelEV.CreateRetailVersion)))
        {
            CurrentRoom.DrawBGObjs(Camera);
            // This line isn't being drawn anyway for some reason.
            //if (CurrentRoom.LevelType == GameTypes.LevelType.TOWER)
            //    m_backgroundParallaxSprite.Draw(Camera);
        }
        else
        {
            // Debug drawing. Holding control allows you to zoom around.
            foreach (var room in RoomList)
            {
                room.DrawBGObjs(Camera);
            }
        }

        Camera.End();

        Camera.GraphicsDevice.SetRenderTarget(RenderTarget);
        Camera.GraphicsDevice.Clear(Color.Black);
        if (EnemiesPaused)
        {
            Camera.GraphicsDevice.Clear(Color.White);
        }

        Camera.GraphicsDevice.Textures[1] = _skyRenderTarget;
        Camera.GraphicsDevice.Textures[1].GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
        Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
            Game.ParallaxEffect); // Parallax Effect has been disabled in favour of ripple effect for now.
        if (EnemiesPaused == false)
        {
            Camera.Draw(_bgRenderTarget, Vector2.Zero, Color.White);
        }

        Camera.End();
        //////////////////////////////////////////////////////

        //////// DRAWING FOREGROUND///////////
        Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null,
            RasterizerState.CullNone, Game.BWMaskEffect, Camera.GetTransformation());
        Camera.GraphicsDevice.Textures[1] = _fgRenderTarget;
        Camera.GraphicsDevice.Textures[1].GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
        Camera.Draw(CurrentRoom.BGRender, Camera.TopLeftCorner, Color.White);
        Camera.End();
        ///////////////////////////////////////////

        //////// IMPORTANT!!!! //////////////////
        // At this point in time, m_fgRenderTarget, m_bgRenderTarget, and m_skyRenderTarget are no longer needed.
        // They can now be (and should be) re-used for whatever rendertarget processes you need.
        // This will cut down immensely on the render targets needed for the game.
        ////////////////////////////////////////

        ////// DRAWING ACTUAL LEVEL ///////////////////////////////////
        if (LevelEV.ShowEnemyRadii == false)
        {
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null,
                Camera.GetTransformation()); // Set SpriteSortMode to immediate to allow instant changes to samplerstates.
        }
        else
        {
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null, null,
                Camera.GetTransformation());
        }

        // IT is currently necessary to draw all rooms for debug purposes.         
        //if (CurrentRoom != null && Camera.Zoom == 1 && (m_inputMap.Pressed(INPUT_LEFTCONTROL) == false || (m_inputMap.Pressed(INPUT_LEFTCONTROL) == true && LevelEV.RUN_DEMO_VERSION == true)))
        CurrentRoom.Draw(Camera);
        //else
        //{
        //    foreach (RoomObj room in m_roomList)
        //        room.Draw(Camera);
        //}


        if (LevelEV.ShowEnemyRadii)
        {
            foreach (var enemy in _currentRoom.EnemyList)
            {
                enemy.DrawDetectionRadii(Camera);
            }
        }

        _projectileManager.Draw(Camera);

        if (EnemiesPaused)
        {
            Camera.End();
            //Camera.GraphicsDevice.SetRenderTarget(m_invertRenderTarget); // Removing m_invertRenderTarget by re-using m_bgRenderTarget.
            Camera.GraphicsDevice.SetRenderTarget(_bgRenderTarget);
            Camera.GraphicsDevice.Textures[1] = _traitAuraRenderTarget;
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                Game.InvertShader);
            Camera.Draw(RenderTarget, Vector2.Zero, Color.White);
            Camera.End();

            Game.HSVEffect.Parameters["Saturation"].SetValue(0);
            Game.HSVEffect.Parameters["UseMask"].SetValue(true);
            Camera.GraphicsDevice.SetRenderTarget(RenderTarget);
            Camera.GraphicsDevice.Textures[1] = _traitAuraRenderTarget;
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                Game.HSVEffect);
            Camera.Draw(_bgRenderTarget, Vector2.Zero, Color.White);
            //Camera.Draw(m_invertRenderTarget, Vector2.Zero, Color.White); // Removing m_invertRenderTarget by re-using m_bgRenderTarget.
            //Camera.End();
        }

        Camera.End();

        if (_toggleMagentaBG)
        {
            Camera.GraphicsDevice.Clear(Color.Magenta);
        }

        // SpriteSortMode changed to deferred.
        Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null,
            Camera.GetTransformation());

        //////  Death animation sprites.
        Camera.Draw(Game.GenericTexture,
            new Rectangle((int)Camera.TopLeftCorner.X, (int)Camera.TopLeftCorner.Y, 1320, 720),
            Color.Black * BackBufferOpacity);

        // Player
        //Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        if (Player.IsKilled == false)
        {
            Player.Draw(Camera);
        }

        if (LevelEV.CreateRetailVersion == false)
        {
            DebugTextObj.Position = new Vector2(Camera.X, Camera.Y - 300);
            DebugTextObj.Draw(Camera);
        }

        _itemDropManager.Draw(Camera);
        ImpactEffectPool.Draw(Camera);
        Camera.End();

        Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null, null,
            Camera.GetTransformation());
        //Camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        _textManager.Draw(Camera);

        //// Special code for parallaxing the Garden FG.
        if (CurrentRoom.LevelType == GameTypes.LevelType.Tower)
        {
            //m_gardenParallaxFG.Position = CurrentRoom.Position - Camera.Position;
            _gardenParallaxFG.Draw(Camera);
        }

        //Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        _whiteBG.Draw(Camera);

        //ScreenManager.Camera.Draw_CameraBox();
        //m_physicsManager.DrawAllCollisionBoxes(Camera, Game.GenericTexture, Consts.TERRAIN_HITBOX);
        //m_physicsManager.DrawAllCollisionBoxes(ScreenManager.Camera, Game.GenericTexture, Consts.WEAPON_HITBOX);
        //m_physicsManager.DrawAllCollisionBoxes(ScreenManager.Camera, Game.GenericTexture, Consts.BODY_HITBOX);

        Camera.End();

        /////////// DRAWING THE SHADOWS & LIGHTING //////////////////////////////
        if ((CurrentLevelType == GameTypes.LevelType.Dungeon || Game.PlayerStats.HasTrait(TraitType.GLAUCOMA))
            && (Game.PlayerStats.Class != ClassType.BANKER2 ||
                (Game.PlayerStats.Class == ClassType.BANKER2 && Player.LightOn == false)))
        {
            // Can't do this because switching from a rendertarget and back is a bug in XNA that causes a purple screen.  Might work with Monogame.
            //Camera.GraphicsDevice.SetRenderTarget(m_bgRenderTarget);
            //Camera.GraphicsDevice.Clear(Color.Black);
            //Camera.GraphicsDevice.SetRenderTarget(m_finalRenderTarget);
            Camera.GraphicsDevice.Textures[1] = _lightSourceRenderTarget;
            Camera.GraphicsDevice.Textures[1].GraphicsDevice.SamplerStates[1] = SamplerState.LinearClamp;
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                Game.ShadowEffect);
            if (LevelEV.SaveFrames)
            {
                Camera.Draw(_shadowRenderTarget, Vector2.Zero, null, Color.White, 0, Vector2.Zero, new Vector2(2, 2),
                    SpriteEffects.None, 1);
            }
            else
            {
                Camera.Draw(_shadowRenderTarget, Vector2.Zero, Color.White);
            }

            Camera.End();
        }

        // Myopia effect.
        if (CurrentRoom.Name != "Ending")
        {
            if (Game.PlayerStats.HasTrait(TraitType.NEAR_SIGHTED) &&
                Game.PlayerStats.SpecialItem != SpecialItemType.GLASSES)
            {
                Game.GaussianBlur.InvertMask = true;
                Game.GaussianBlur.Draw(RenderTarget, Camera, _traitAuraRenderTarget);
            }
            // Hyperopia effect.
            else if (Game.PlayerStats.HasTrait(TraitType.FAR_SIGHTED) &&
                     Game.PlayerStats.SpecialItem != SpecialItemType.GLASSES)
            {
                Game.GaussianBlur.InvertMask = false;
                Game.GaussianBlur.Draw(RenderTarget, Camera, _traitAuraRenderTarget);
            }
        }

        /////////// DRAWING MINIMAP & ENEMY HUD//////////////////////////////////
        Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null);

        _projectileIconPool.Draw(Camera);

        _playerHUD.Draw(Camera);

        if (_lastEnemyHit != null && _enemyHUDCounter > 0)
        {
            _enemyHUD.Draw(Camera);
        }

        if (_enemyHUDCounter > 0)
        {
            _enemyHUDCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        if (CurrentRoom.Name != "Start" && CurrentRoom.Name != "Boss" && CurrentRoom.Name != "ChallengeBoss" &&
            MiniMapDisplay.Visible)
        {
            _mapBG.Draw(Camera);
            MiniMapDisplay.Draw(Camera);
        }

        if (CurrentRoom.Name != "Boss" && CurrentRoom.Name != "Ending")
        {
            _compassBG.Draw(Camera);
            _compass.Draw(Camera);
        }

        _objectivePlate.Draw(Camera);
        _roomEnteringTitle.Draw(Camera);
        _roomTitle.Draw(Camera);

        if (CurrentRoom.Name != "Ending")
        {
            if ((Game.PlayerStats.TutorialComplete == false || Game.PlayerStats.HasTrait(TraitType.NOSTALGIC)) &&
                Game.PlayerStats.SpecialItem != SpecialItemType.GLASSES)
            {
                _filmGrain.Draw(Camera);
            }
        }

        Camera.End();

        Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
        _blackBorder1.Draw(Camera);
        _blackBorder2.Draw(Camera);

        Camera.End();
        //////////////////////////////////////////////////////////////

        // This is where you apply all the rendertarget effects.

        // Applying the Fus Ro Dah ripple effect, applying it to m_finalRenderTarget, and saving it to another RT.
        //Camera.GraphicsDevice.SetRenderTarget(m_rippleRenderTarget);
        Camera.GraphicsDevice.SetRenderTarget(_bgRenderTarget);
        Game.RippleEffect.Parameters["width"].SetValue(ShoutMagnitude);

        var playerPos = Player.Position - Camera.TopLeftCorner;
        if (Game.PlayerStats.Class == ClassType.BARBARIAN || Game.PlayerStats.Class == ClassType.BARBARIAN2)
        {
            Game.RippleEffect.Parameters["xcenter"].SetValue(playerPos.X / 1320f);
            Game.RippleEffect.Parameters["ycenter"].SetValue(playerPos.Y / 720f);
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                Game.RippleEffect);
        }
        else
        {
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
        }

        Camera.Draw(RenderTarget, Vector2.Zero, Color.White);
        Camera.End();

        // Changing to the final Screen manager RenderTarget. This is where the final drawing goes.
        Camera.GraphicsDevice.SetRenderTarget((ScreenManager as RCScreenManager).RenderTarget);

        if (CurrentRoom.Name != "Ending")
        {
            // Colour blind effect.
            if (Game.PlayerStats.HasTrait(TraitType.COLOR_BLIND) &&
                Game.PlayerStats.SpecialItem != SpecialItemType.GLASSES)
            {
                Game.HSVEffect.Parameters["Saturation"].SetValue(0);
                Game.HSVEffect.Parameters["Brightness"].SetValue(0);
                Game.HSVEffect.Parameters["Contrast"].SetValue(0);
                Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                    Game.HSVEffect);
            }
            else if ((Game.PlayerStats.TutorialComplete == false || Game.PlayerStats.HasTrait(TraitType.NOSTALGIC)) &&
                     Game.PlayerStats.SpecialItem != SpecialItemType.GLASSES)
            {
                Camera.GraphicsDevice.SetRenderTarget(RenderTarget);
                Game.HSVEffect.Parameters["Saturation"].SetValue(0.2f);
                Game.HSVEffect.Parameters["Brightness"].SetValue(0.1f);
                Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                    Game.HSVEffect);
                Camera.Draw(_bgRenderTarget, Vector2.Zero, Color.White);
                Camera.End();

                Camera.GraphicsDevice.SetRenderTarget(_bgRenderTarget);
                Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                    null);
                var sepia = new Color(180, 150, 80);
                Camera.Draw(RenderTarget, Vector2.Zero, sepia);
                _creditsText.Draw(Camera);
                _creditsTitleText.Draw(Camera);
                Camera.End();

                Camera.GraphicsDevice.SetRenderTarget((ScreenManager as RCScreenManager).RenderTarget);
                Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                    null);
            }
            else
            {
                Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null,
                    null);
            }
        }
        else
        {
            Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null);
        }

        Camera.Draw(_bgRenderTarget, Vector2.Zero, Color.White);

        Camera.End();

        base.Draw(gameTime); // Doesn't do anything.
    }

    public void RunWhiteSlashEffect()
    {
        _whiteBG.Position = CurrentRoom.Position;
        _whiteBG.Scale = Vector2.One;
        _whiteBG.Scale = new Vector2(CurrentRoom.Width / _whiteBG.Width, _currentRoom.Height / _whiteBG.Height);
        _whiteBG.Opacity = 1;
        Tween.To(_whiteBG, 0.2f, Tween.EaseNone, "Opacity", "0");
        Tween.RunFunction(0.2f, this, "RunWhiteSlash2");
    }

    public void RunWhiteSlash2()
    {
        _whiteBG.Position = CurrentRoom.Position;
        _whiteBG.Scale = Vector2.One;
        _whiteBG.Scale = new Vector2(CurrentRoom.Width / _whiteBG.Width, _currentRoom.Height / _whiteBG.Height);
        _whiteBG.Opacity = 1;
        Tween.To(_whiteBG, 0.2f, Tween.EaseNone, "Opacity", "0");
    }

    // Same as RunWhiteSlash but calls a different sfx.
    public void LightningEffectTwice()
    {
        _whiteBG.Position = CurrentRoom.Position;
        _whiteBG.Scale = Vector2.One;
        _whiteBG.Scale = new Vector2(CurrentRoom.Width / _whiteBG.Width, _currentRoom.Height / _whiteBG.Height);
        _whiteBG.Opacity = 1;
        Tween.To(_whiteBG, 0.2f, Tween.EaseNone, "Opacity", "0");
        Tween.RunFunction(0.2f, this, "LightningEffectOnce");
    }

    public void LightningEffectOnce()
    {
        _whiteBG.Position = CurrentRoom.Position;
        _whiteBG.Scale = Vector2.One;
        _whiteBG.Scale = new Vector2(CurrentRoom.Width / _whiteBG.Width, _currentRoom.Height / _whiteBG.Height);
        _whiteBG.Opacity = 1;
        Tween.To(_whiteBG, 1, Tween.EaseNone, "Opacity", "0");
        SoundManager.PlaySound("LightningClap1", "LightningClap2");
    }

    public void SpawnDementiaEnemy()
    {
        var enemyObjList = new List<EnemyObj>();

        foreach (var enemy in _currentRoom.EnemyList)
        {
            if (enemy.Type != EnemyType.TURRET && enemy.Type != EnemyType.SPIKE_TRAP &&
                enemy.Type != EnemyType.PLATFORM &&
                enemy.Type != EnemyType.PORTRAIT && enemy.Type != EnemyType.EYEBALL &&
                enemy.Type != EnemyType.STARBURST)
            {
                enemyObjList.Add(enemy);
            }
        }

        if (enemyObjList.Count > 0)
        {
            var enemy = enemyObjList[CDGMath.RandomInt(0, enemyObjList.Count - 1)];
            byte[] enemyList = null;

            if (enemy.IsWeighted)
            {
                enemyList = LevelEV.DementiaGroundList;
            }
            else
            {
                enemyList = LevelEV.DementiaFlightList;
            }

            var newEnemy = EnemyBuilder.BuildEnemy(enemyList[CDGMath.RandomInt(0, enemyList.Length - 1)], null, null,
                null, GameTypes.EnemyDifficulty.Basic, true);
            newEnemy.Position = enemy.Position; // Make sure this is set before calling AddEnemyToCurrentRoom()
            newEnemy.SaveToFile = false;
            newEnemy.IsDemented = true;
            newEnemy.NonKillable = true;
            newEnemy.GivesLichHealth = false;
            AddEnemyToCurrentRoom(newEnemy);
        }
    }

    public void AddEnemyToCurrentRoom(EnemyObj enemy)
    {
        //m_currentRoom.EnemyList.Add(enemy);
        _currentRoom.TempEnemyList.Add(enemy); // Add enemy to the temp list instead of the real one.
        _physicsManager.AddObject(enemy);
        //m_enemyStartPositions.Add(enemy.Position);
        _tempEnemyStartPositions.Add(enemy.Position);
        enemy.SetPlayerTarget(Player);
        enemy.SetLevelScreen(this);
        enemy.Initialize();
    }

    public void RemoveEnemyFromCurrentRoom(EnemyObj enemy, Vector2 startingPos)
    {
        _currentRoom.TempEnemyList.Remove(enemy);
        _physicsManager.RemoveObject(enemy);
        _tempEnemyStartPositions.Remove(startingPos);
    }

    public void RemoveEnemyFromRoom(EnemyObj enemy, RoomObj room, Vector2 startingPos)
    {
        room.TempEnemyList.Remove(enemy);
        _physicsManager.RemoveObject(enemy);
        _tempEnemyStartPositions.Remove(startingPos);
    }

    public void RemoveEnemyFromRoom(EnemyObj enemy, RoomObj room)
    {
        var enemyIndex = room.TempEnemyList.IndexOf(enemy);
        if (enemyIndex != -1)
        {
            room.TempEnemyList.RemoveAt(enemyIndex);
            _physicsManager.RemoveObject(enemy);
            _tempEnemyStartPositions.RemoveAt(enemyIndex);
        }
    }

    public void ResetEnemyPositions()
    {
        for (var i = 0; i < _enemyStartPositions.Count; i++)
        {
            CurrentRoom.EnemyList[i].Position = _enemyStartPositions[i];
        }

        for (var i = 0; i < _tempEnemyStartPositions.Count; i++)
        {
            CurrentRoom.TempEnemyList[i].Position = _tempEnemyStartPositions[i];
        }
    }

    public override void PauseScreen()
    {
        if (IsPaused == false)
        {
            Tween.PauseAll();
            CurrentRoom.PauseRoom();
            ItemDropManager.PauseAllAnimations();
            ImpactEffectPool.PauseAllAnimations();
            if (EnemiesPaused == false) // Only pause the projectiles if they aren't already paused via time stop.
            {
                _projectileManager.PauseAllProjectiles(true);
            }

            SoundManager.PauseAllSounds("Pauseable");

            Player.PauseAnimation();
            GamePad.SetVibration(PlayerIndex.One, 0, 0);

            base.PauseScreen();
        }
    }

    public override void UnpauseScreen()
    {
        if (IsPaused)
        {
            Tween.ResumeAll();
            CurrentRoom.UnpauseRoom();
            ItemDropManager.ResumeAllAnimations();
            ImpactEffectPool.ResumeAllAnimations();
            if (EnemiesPaused == false) // Only unpause all projectiles if enemies are paused.
            {
                _projectileManager.UnpauseAllProjectiles();
            }

            SoundManager.ResumeAllSounds("Pauseable");

            Player.ResumeAnimation();
            base.UnpauseScreen();
        }
    }

    public void RunGameOver()
    {
        Player.Opacity = 1;
        _killedEnemyObjList.Clear();
        List<Vector2> enemiesKilledInRun = Game.PlayerStats.EnemiesKilledInRun;

        var roomSize = RoomList.Count;
        for (var i = 0; i < enemiesKilledInRun.Count; i++)
        {
            if (enemiesKilledInRun[i].X != -1 && enemiesKilledInRun[i].Y != -1)
            {
                if ((int)enemiesKilledInRun[i].X < roomSize)
                {
                    var room = RoomList[(int)enemiesKilledInRun[i].X];
                    var numEnemies = room.EnemyList.Count;
                    if ((int)enemiesKilledInRun[i].Y < numEnemies)
                    {
                        var enemy = RoomList[(int)enemiesKilledInRun[i].X].EnemyList[(int)enemiesKilledInRun[i].Y];
                        _killedEnemyObjList.Add(enemy);
                    }
                }
            }

            //EnemyObj enemy = m_roomList[(int)enemiesKilledInRun[i].X].EnemyList[(int)enemiesKilledInRun[i].Y];
            //m_killedEnemyObjList.Add(enemy);
        }

        var dataList = new List<object>();
        dataList.Add(Player);
        dataList.Add(_killedEnemyObjList);
        dataList.Add(_coinsCollected);
        dataList.Add(_bagsCollected);
        dataList.Add(_diamondsCollected);
        dataList.Add(_bigDiamondsCollected);
        dataList.Add(_objKilledPlayer);

        Tween.RunFunction(0, ScreenManager, "DisplayScreen", ScreenType.GAME_OVER, true, dataList);
    }

    public void RunCinematicBorders(float duration)
    {
        StopCinematicBorders();
        _blackBorder1.Opacity = 1;
        _blackBorder2.Opacity = 1;
        _blackBorder1.Y = 0;
        _blackBorder2.Y = 720 - _borderSize;
        var fadeSpeed = 1f;
        Tween.By(_blackBorder1, fadeSpeed, Quad.EaseInOut, "delay", (duration - fadeSpeed).ToString(), "Y",
            (-_borderSize).ToString());
        Tween.By(_blackBorder2, fadeSpeed, Quad.EaseInOut, "delay", (duration - fadeSpeed).ToString(), "Y",
            _borderSize.ToString());
        Tween.To(_blackBorder1, fadeSpeed, Linear.EaseNone, "delay", (duration - fadeSpeed + 0.2f).ToString(),
            "Opacity", "0");
        Tween.To(_blackBorder2, fadeSpeed, Linear.EaseNone, "delay", (duration - fadeSpeed + 0.2f).ToString(),
            "Opacity", "0");
    }

    public void StopCinematicBorders()
    {
        Tween.StopAllContaining(_blackBorder1, false);
        Tween.StopAllContaining(_blackBorder2, false);
    }

    public void DisplayMap(bool isTeleporterScreen)
    {
        //m_miniMapDisplay.AddAllRooms(m_roomList);
        (ScreenManager as RCScreenManager).AddRoomsToMap(MiniMapDisplay.AddedRoomsList);
        if (isTeleporterScreen)
        {
            (ScreenManager as RCScreenManager).ActivateMapScreenTeleporter();
        }

        (ScreenManager as RCScreenManager).DisplayScreen(ScreenType.MAP, true);
    }

    public void PauseAllEnemies()
    {
        EnemiesPaused = true;
        CurrentRoom.PauseRoom();
        foreach (var enemy in CurrentRoom.EnemyList)
        {
            enemy.PauseEnemy();
        }

        foreach (var enemy in CurrentRoom.TempEnemyList)
        {
            enemy.PauseEnemy();
        }

        _projectileManager.PauseAllProjectiles(false);
    }

    public void CastTimeStop(float duration)
    {
        SoundManager.PlaySound("Cast_TimeStart");
        SoundManager.PauseMusic();
        _enemyPauseDuration = duration;
        PauseAllEnemies();
        Tween.To(_traitAura, 0.2f, Tween.EaseNone, "ScaleX", "100", "ScaleY", "100");
    }

    public void StopTimeStop()
    {
        SoundManager.PlaySound("Cast_TimeStop");
        SoundManager.ResumeMusic();
        Tween.To(_traitAura, 0.2f, Tween.EaseNone, "ScaleX", "0", "ScaleY", "0");
        Tween.AddEndHandlerToLastTween(this, "UnpauseAllEnemies");
    }

    public void UnpauseAllEnemies()
    {
        Game.HSVEffect.Parameters["UseMask"].SetValue(false);
        EnemiesPaused = false;

        CurrentRoom.UnpauseRoom();

        foreach (var enemy in CurrentRoom.EnemyList)
        {
            enemy.UnpauseEnemy();
        }

        foreach (var enemy in CurrentRoom.TempEnemyList)
        {
            enemy.UnpauseEnemy();
        }

        _projectileManager.UnpauseAllProjectiles();
    }

    public void DamageAllEnemies(int damage)
    {
        // Do temp enemies first otherwise one of them will get hit twice.
        var tempEnemyList =
            new List<EnemyObj>(); // Necessary because TempEnemyList is a list that is continually modified.
        tempEnemyList.AddRange(CurrentRoom.TempEnemyList);
        foreach (var enemy in tempEnemyList)
        {
            if (enemy.IsDemented == false && enemy.IsKilled == false)
            {
                enemy.HitEnemy(damage, enemy.Position, true);
            }
        }

        tempEnemyList.Clear();
        tempEnemyList = null;

        foreach (var enemy in CurrentRoom.EnemyList)
        {
            if (enemy.IsDemented == false && enemy.IsKilled == false)
            {
                enemy.HitEnemy(damage, enemy.Position, true);
            }
        }
    }

    public virtual void Reset()
    {
        BackBufferOpacity = 0;

        _killedEnemyObjList.Clear();

        _bigDiamondsCollected = 0;
        _diamondsCollected = 0;
        _coinsCollected = 0;
        _bagsCollected = 0;
        _blueprintsCollected = 0;

        if (Player != null)
        {
            Player.Reset();
            Player.ResetLevels();
            Player.Position = new Vector2(200, 200);
            //UpdatePlayerHUDHP();
            //UpdatePlayerHUDMP();
        }

        ResetEnemyPositions();

        foreach (var room in RoomList)
        {
            room.Reset();
        }

        InitializeChests(false);


        foreach (var room in RoomList)
        {
            foreach (var obj in room.GameObjList)
            {
                var breakableObj = obj as BreakableObj;
                if (breakableObj != null)
                {
                    breakableObj.Reset();
                }
            }
        }

        _projectileManager.DestroyAllProjectiles(true);
        Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0);
    }

    public override void DisposeRTs()
    {
        _fgRenderTarget.Dispose();
        _fgRenderTarget = null;
        _bgRenderTarget.Dispose();
        _bgRenderTarget = null;
        _skyRenderTarget.Dispose();
        _skyRenderTarget = null;
        RenderTarget.Dispose();
        RenderTarget = null;

        _shadowRenderTarget.Dispose();
        _shadowRenderTarget = null;
        _lightSourceRenderTarget.Dispose();
        _lightSourceRenderTarget = null;
        _traitAuraRenderTarget.Dispose();
        _traitAuraRenderTarget = null;

        _foregroundSprite.Dispose();
        _foregroundSprite = null;
        _backgroundSprite.Dispose();
        _backgroundSprite = null;
        _backgroundParallaxSprite.Dispose();
        _backgroundParallaxSprite = null;
        _gardenParallaxFG.Dispose();
        _gardenParallaxFG = null;

        _roomBWRenderTarget.Dispose();
        _roomBWRenderTarget = null;

        MiniMapDisplay.DisposeRTs();
        base.DisposeRTs();
    }

    public override void Dispose()
    {
        if (IsDisposed == false)
        {
            Console.WriteLine("Disposing Procedural Level Screen");

            Tween.StopAll(false);

            _currentRoom = null;
            DisposeRTs();
            foreach (var room in RoomList)
            {
                room.Dispose();
            }

            RoomList.Clear();
            RoomList = null;
            _enemyStartPositions.Clear();
            _enemyStartPositions = null;
            _tempEnemyStartPositions.Clear();
            _tempEnemyStartPositions = null;
            _textManager.Dispose();
            _textManager = null;
            //m_physicsManager.Dispose(); // Don't dispose the Physics manager since it's created in Game.cs and needs to remain persistent.
            _physicsManager = null;
            _projectileManager.Dispose();
            _projectileManager = null;
            _itemDropManager.Dispose();
            _itemDropManager = null;
            _currentRoom = null;
            MiniMapDisplay.Dispose();
            MiniMapDisplay = null;
            _mapBG.Dispose();
            _mapBG = null;
            _inputMap.Dispose();
            _inputMap = null;
            _lastEnemyHit = null;
            _playerHUD.Dispose();
            _playerHUD = null;
            Player = null;
            _enemyHUD.Dispose();
            _enemyHUD = null;
            ImpactEffectPool.Dispose();
            ImpactEffectPool = null;

            _blackBorder1.Dispose();
            _blackBorder1 = null;
            _blackBorder2.Dispose();
            _blackBorder2 = null;

            ChestList.Clear();
            ChestList = null;

            _projectileIconPool.Dispose();
            _projectileIconPool = null;

            _objKilledPlayer = null;

            _dungeonLight.Dispose();
            _dungeonLight = null;
            _traitAura.Dispose();
            _traitAura = null;

            _killedEnemyObjList.Clear();
            _killedEnemyObjList = null;

            _roomEnteringTitle.Dispose();
            _roomEnteringTitle = null;
            _roomTitle.Dispose();
            _roomTitle = null;

            _creditsText.Dispose();
            _creditsText = null;
            _creditsTitleText.Dispose();
            _creditsTitleText = null;
            Array.Clear(_creditsTextTitleList, 0, _creditsTextTitleList.Length);
            Array.Clear(_creditsTextList, 0, _creditsTextList.Length);
            _creditsTextTitleList = null;
            _creditsTextList = null;
            _filmGrain.Dispose();
            _filmGrain = null;

            _objectivePlate.Dispose();
            _objectivePlate = null;
            _objectivePlateTween = null;

            _sky.Dispose();
            _sky = null;
            _whiteBG.Dispose();
            _whiteBG = null;

            _compassBG.Dispose();
            _compassBG = null;
            _compass.Dispose();
            _compass = null;

            if (_compassDoor != null)
            {
                _compassDoor.Dispose();
            }

            _compassDoor = null;

            _castleBorderTexture.Dispose();
            _gardenBorderTexture.Dispose();
            _towerBorderTexture.Dispose();
            _dungeonBorderTexture.Dispose();
            _neoBorderTexture.Dispose();

            _castleBorderTexture = null;
            _gardenBorderTexture = null;
            _towerBorderTexture = null;
            _dungeonBorderTexture = null;

            DebugTextObj.Dispose();
            DebugTextObj = null;

            base.Dispose(); // Sets the IsDisposed flag to true.
        }
    }

    public void SetLastEnemyHit(EnemyObj enemy)
    {
        _lastEnemyHit = enemy;
        _enemyHUDCounter = _enemyHUDDuration;
        //m_enemyHUD.UpdateEnemyInfo(m_lastEnemyHit.Name, m_lastEnemyHit.Level, m_lastEnemyHit.CurrentHealth / (float)m_lastEnemyHit.MaxHealth);
        _enemyHUD.UpdateEnemyInfo(_lastEnemyHit.LocStringID, _lastEnemyHit.Level,
            _lastEnemyHit.CurrentHealth / (float)_lastEnemyHit.MaxHealth);
    }

    public void KillEnemy(EnemyObj enemy)
    {
        if (enemy.SaveToFile)
        {
            var killedEnemy = new Vector2(RoomList.IndexOf(CurrentRoom), CurrentRoom.EnemyList.IndexOf(enemy));

            if (killedEnemy.X < 0 || killedEnemy.Y < 0)
            {
                throw new Exception(
                    "Could not find killed enemy in either CurrentRoom or CurrentRoom.EnemyList. This may be because the enemy was a blob");
            }

            Game.PlayerStats.EnemiesKilledInRun.Add(killedEnemy);
        }
    }

    public void ItemDropCollected(int itemDropType)
    {
        switch (itemDropType)
        {
            case ItemDropType.COIN:
                _coinsCollected++;
                break;
            case ItemDropType.MONEY_BAG:
                _bagsCollected++;
                break;
            case ItemDropType.DIAMOND:
                _diamondsCollected++;
                break;
            case ItemDropType.BIG_DIAMOND:
                _bigDiamondsCollected++;
                break;
            case ItemDropType.BLUEPRINT:
            case ItemDropType.REDPRINT:
                _blueprintsCollected++;
                break;
        }
    }

    public void RefreshMapChestIcons()
    {
        MiniMapDisplay.RefreshChestIcons(CurrentRoom);
        (ScreenManager as RCScreenManager).RefreshMapScreenChestIcons(CurrentRoom);
    }

    public void DisplayObjective(
        string objectiveTitleID,
        string objectiveDescriptionID,
        string objectiveProgressID,
        bool tween
    )
    {
        //SoundManager.Play3DSound(this, Game.ScreenManager.Player,"FairyChest_Start");
        // Objective Lines.
        (_objectivePlate.GetChildAt(4) as SpriteObj).ScaleX = 0;
        (_objectivePlate.GetChildAt(5) as SpriteObj).ScaleX = 0;

        _objectivePlate.GetChildAt(2).Opacity = 1f;
        _objectivePlate.GetChildAt(3).Opacity = 1f;
        _objectivePlate.X = 1170 + 300;

        if (_objectivePlateTween != null && _objectivePlateTween.TweenedObject == _objectivePlate &&
            _objectivePlateTween.Active)
        {
            _objectivePlateTween.StopTween(false);
        }

        (_objectivePlate.GetChildAt(1) as TextObj).Text =
            objectiveTitleID.GetString(_objectivePlate.GetChildAt(1) as TextObj);
        (_objectivePlate.GetChildAt(2) as TextObj).Text =
            objectiveDescriptionID.GetString(_objectivePlate.GetChildAt(2) as TextObj);
        (_objectivePlate.GetChildAt(3) as TextObj).Text =
            objectiveProgressID.GetString(_objectivePlate.GetChildAt(3) as TextObj);

        if (tween)
        {
            _objectivePlateTween = Tween.By(_objectivePlate, 0.5f, Back.EaseOut, "X", "-300");
        }
        else
        {
            _objectivePlate.X -= 300;
        }
    }

    public void ResetObjectivePlate(bool tween)
    {
        if (_objectivePlate != null)
        {
            _objectivePlate.X = 1170;

            if (_objectivePlateTween != null && _objectivePlateTween.TweenedObject == _objectivePlate &&
                _objectivePlateTween.Active)
            {
                _objectivePlateTween.StopTween(false);
            }

            if (tween)
            {
                Tween.By(_objectivePlate, 0.5f, Back.EaseIn, "X", "300");
            }
            else
            {
                _objectivePlate.X += 300;
            }
        }
    }

    // progress parameter is actual display string
    public void UpdateObjectiveProgress(string progress)
    {
        (_objectivePlate.GetChildAt(3) as TextObj).Text = progress;
    }

    public void ObjectiveFailed()
    {
        (_objectivePlate.GetChildAt(1) as TextObj).Text =
            "LOC_ID_LEVEL_SCREEN_6".GetString(_objectivePlate.GetChildAt(1) as TextObj); //"Objective Failed"
        _objectivePlate.GetChildAt(2).Opacity = 0.3f;
        _objectivePlate.GetChildAt(3).Opacity = 0.3f;
    }

    public void ObjectiveComplete()
    {
        // objective lines
        //Tween.By(m_objectivePlate.GetChildAt(4), 0.3f, Tween.EaseNone, "ScaleX", (m_objectivePlate.GetChildAt(2).Width / 5).ToString());
        //if ((m_objectivePlate.GetChildAt(3) as TextObj).Text != "")
        //    Tween.By(m_objectivePlate.GetChildAt(5), 0.3f, Tween.EaseNone, "delay", "0.2", "ScaleX", (m_objectivePlate.GetChildAt(3).Width / 5).ToString());

        _objectivePlate.GetChildAt(2).Opacity = 0.3f;
        _objectivePlate.GetChildAt(3).Opacity = 0.3f;

        _objectivePlate.X = 1170;

        if (_objectivePlateTween != null && _objectivePlateTween.TweenedObject == _objectivePlate &&
            _objectivePlateTween.Active)
        {
            _objectivePlateTween.StopTween(false);
        }

        (_objectivePlate.GetChildAt(1) as TextObj).Text =
            "LOC_ID_LEVEL_SCREEN_5".GetString(_objectivePlate.GetChildAt(1) as TextObj); //"Objective Complete!"
        //m_objectivePlateTween = Tween.By(m_objectivePlate, 0.5f, Back.EaseIn, "delay", "1", "X", "300");
    }

    public override void OnEnter()
    {
        (ScreenManager.Game as Game).SaveManager.ResetAutosave();
        Player.DisableAllWeight =
            false; // Fixes bug where you translocate right before enter the castle, resulting in screwed up gravity.
        Player.StopAllSpells();
        StopScreenShake();

        ShoutMagnitude = 3;

        // Setting up player.
        if (Game.PlayerStats.HasTrait(TraitType.GIGANTISM))
        {
            Player.Scale = new Vector2(GameEV.TRAIT_GIGANTISM, GameEV.TRAIT_GIGANTISM); //(3.5f, 3.5f);
        }
        else if (Game.PlayerStats.HasTrait(TraitType.DWARFISM))
        {
            Player.Scale = new Vector2(GameEV.TRAIT_DWARFISM, GameEV.TRAIT_DWARFISM);
        }
        else
        {
            Player.Scale = new Vector2(2, 2);
        }

        // Modifying the player's scale based on traits.
        if (Game.PlayerStats.HasTrait(TraitType.ECTOMORPH))
        {
            Player.ScaleX *= 0.825f;
            Player.ScaleY *= 1.15f;
            //m_player.Scale = new Vector2(1.8f, 2.2f);
        }
        else if (Game.PlayerStats.HasTrait(TraitType.ENDOMORPH))
        {
            Player.ScaleX *= 1.25f;
            Player.ScaleY *= 1.175f;
            //m_player.Scale = new Vector2(2.5f, 2f);
        }

        if (Game.PlayerStats.HasTrait(TraitType.CLONUS))
        {
            _elapsedScreenShake = CDGMath.RandomFloat(GameEV.TRAIT_CLONUS_MIN, GameEV.TRAIT_CLONUS_MAX);
        }

        Player.CurrentHealth = Game.PlayerStats.CurrentHealth;
        Player.CurrentMana = Game.PlayerStats.CurrentMana;

        if (LevelEV.RunTestRoom)
        {
            Game.ScreenManager.Player.CurrentHealth = Game.ScreenManager.Player.MaxHealth;
            Game.ScreenManager.Player.CurrentMana = Game.ScreenManager.Player.MaxMana;
        }

        Player.UpdateInternalScale();

        CheckForRoomTransition();
        UpdateCamera();
        UpdatePlayerHUDAbilities();
        Player.UpdateEquipmentColours();
        Player.StopAllSpells();

        // Adding treasure chest icons to map for Spelunker.
        if (Game.PlayerStats.Class == ClassType.BANKER2)
        {
            MiniMapDisplay.AddAllIcons(RoomList);
            (ScreenManager as RCScreenManager).AddIconsToMap(RoomList);
        }

        //// Adding teleporters to all bosses already beaten.
        //if (Game.PlayerStats.EyeballBossBeaten)
        //    m_miniMapDisplay.AddLinkerRoom(GameTypes.LevelType.CASTLE, this.RoomList);
        //if (Game.PlayerStats.FairyBossBeaten)
        //    m_miniMapDisplay.AddLinkerRoom(GameTypes.LevelType.GARDEN, this.RoomList);
        //if (Game.PlayerStats.FireballBossBeaten)
        //    m_miniMapDisplay.AddLinkerRoom(GameTypes.LevelType.TOWER, this.RoomList);
        //if (Game.PlayerStats.BlobBossBeaten)
        //    m_miniMapDisplay.AddLinkerRoom(GameTypes.LevelType.DUNGEON, this.RoomList);

        if (Game.PlayerStats.EyeballBossBeaten)
        {
            GameUtil.UnlockAchievement("FEAR_OF_EYES");
        }

        if (Game.PlayerStats.FairyBossBeaten)
        {
            GameUtil.UnlockAchievement("FEAR_OF_GHOSTS");
        }

        if (Game.PlayerStats.BlobBossBeaten)
        {
            GameUtil.UnlockAchievement("FEAR_OF_SLIME");
        }

        if (Game.PlayerStats.FireballBossBeaten)
        {
            GameUtil.UnlockAchievement("FEAR_OF_FIRE");
        }

        if (Game.PlayerStats.LastbossBeaten || Game.PlayerStats.TimesCastleBeaten > 0)
        {
            GameUtil.UnlockAchievement("FEAR_OF_FATHERS");
        }

        if (Game.PlayerStats.TimesCastleBeaten > 1)
        {
            GameUtil.UnlockAchievement("FEAR_OF_TWINS");
        }

        if (Game.PlayerStats.ChallengeEyeballBeaten)
        {
            GameUtil.UnlockAchievement("FEAR_OF_BLINDNESS");
        }

        if (Game.PlayerStats.ChallengeSkullBeaten)
        {
            GameUtil.UnlockAchievement("FEAR_OF_BONES");
        }

        if (Game.PlayerStats.ChallengeFireballBeaten)
        {
            GameUtil.UnlockAchievement("FEAR_OF_CHEMICALS");
        }

        if (Game.PlayerStats.ChallengeBlobBeaten)
        {
            GameUtil.UnlockAchievement("FEAR_OF_SPACE");
        }

        if (Game.PlayerStats.ChallengeLastBossBeaten)
        {
            GameUtil.UnlockAchievement("FEAR_OF_RELATIVES");
        }

        var skeletonMBKilled = false;
        var plantMBKilled = false;
        var paintingMBKilled = false;
        var knightMBKilled = false;
        var wizardMBKilled = false;
        if (Game.PlayerStats.EnemiesKilledList[EnemyType.SKELETON].W > 0)
        {
            skeletonMBKilled = true;
        }

        if (Game.PlayerStats.EnemiesKilledList[EnemyType.PLANT].W > 0)
        {
            plantMBKilled = true;
        }

        if (Game.PlayerStats.EnemiesKilledList[EnemyType.PORTRAIT].W > 0)
        {
            paintingMBKilled = true;
        }

        if (Game.PlayerStats.EnemiesKilledList[EnemyType.KNIGHT].W > 0)
        {
            knightMBKilled = true;
        }

        if (Game.PlayerStats.EnemiesKilledList[EnemyType.EARTH_WIZARD].W > 0)
        {
            wizardMBKilled = true;
        }

        if (skeletonMBKilled && plantMBKilled && paintingMBKilled && knightMBKilled && wizardMBKilled)
        {
            GameUtil.UnlockAchievement("FEAR_OF_ANIMALS");
        }

        if (Game.PlayerStats.TotalHoursPlayed + Game.HoursPlayedSinceLastSave >= 20)
        {
            GameUtil.UnlockAchievement("FEAR_OF_SLEEP");
        }

        if (Game.PlayerStats.TotalRunesFound > 10)
        {
            GameUtil.UnlockAchievement("LOVE_OF_MAGIC");
        }

        base.OnEnter();
    }

    public override void OnExit()
    {
        StopScreenShake();
        if (_currentRoom != null)
        {
            _currentRoom.OnExit(); // Call on exit if exiting from a room.
        }

        SoundManager.StopAllSounds("Default");
        SoundManager.StopAllSounds("Pauseable");
        base.OnExit();
    }

    public void RevealMorning()
    {
        _sky.MorningOpacity = 0;
        Tween.To(_sky, 2, Tween.EaseNone, "MorningOpacity", "1");
    }

    public void ZoomOutAllObjects()
    {
        var centrePt = new Vector2(CurrentRoom.Bounds.Center.X, CurrentRoom.Bounds.Center.Y);
        var objPositions = new List<Vector2>();
        float delay = 0;

        foreach (var obj in CurrentRoom.GameObjList)
        {
            var zoomXAmount = 0;
            var zoomYAmount = 0;

            if (obj.Y < centrePt.Y)
            {
                zoomYAmount = CurrentRoom.Bounds.Top - (obj.Bounds.Top + obj.Bounds.Height);
            }
            else
            {
                zoomYAmount = CurrentRoom.Bounds.Bottom - obj.Bounds.Top;
            }

            if (obj.X < centrePt.X)
            {
                zoomXAmount = CurrentRoom.Bounds.Left - (obj.Bounds.Left + obj.Bounds.Width);
            }
            else
            {
                zoomXAmount = CurrentRoom.Bounds.Right - obj.Bounds.Left;
            }

            if (Math.Abs(zoomXAmount) > Math.Abs(zoomYAmount))
            {
                objPositions.Add(new Vector2(0, zoomYAmount));
                Tween.By(obj, 0.5f, Back.EaseIn, "delay", delay.ToString(), "Y", zoomYAmount.ToString());
            }
            else
            {
                objPositions.Add(new Vector2(zoomXAmount, 0));
                Tween.By(obj, 0.5f, Back.EaseIn, "delay", delay.ToString(), "X", zoomXAmount.ToString());
            }

            delay += 0.05f;
        }

        Tween.RunFunction(delay + 0.5f, this, "ZoomInAllObjects", objPositions);
        //Tween.AddEndHandlerToLastTween(this, "ZoomInAllObjects", objPositions);
    }

    public void ZoomInAllObjects(List<Vector2> objPositions)
    {
        var counter = 0;
        float delay = 1;
        foreach (var obj in CurrentRoom.GameObjList)
        {
            Tween.By(obj, 0.5f, Back.EaseOut, "delay", delay.ToString(), "X", (-objPositions[counter].X).ToString(),
                "Y", (-objPositions[counter].Y).ToString());
            counter++;
            delay += 0.05f;
        }
    }

    public void UpdateLevel(GameTypes.LevelType levelType)
    {
        switch (levelType)
        {
            case GameTypes.LevelType.Castle:
                _backgroundSprite.Scale = Vector2.One;
                _foregroundSprite.Scale = Vector2.One;
                _backgroundSprite.ChangeSprite("CastleBG1_Sprite", ScreenManager.Camera);
                _foregroundSprite.ChangeSprite("CastleFG1_Sprite", ScreenManager.Camera);
                _backgroundSprite.Scale = new Vector2(2, 2);
                _foregroundSprite.Scale = new Vector2(2, 2);
                break;
            case GameTypes.LevelType.Tower:
                _backgroundSprite.Scale = Vector2.One;
                _foregroundSprite.Scale = Vector2.One;
                _backgroundSprite.ChangeSprite("TowerBG2_Sprite", ScreenManager.Camera);
                _foregroundSprite.ChangeSprite("TowerFG2_Sprite", ScreenManager.Camera);
                _backgroundSprite.Scale = new Vector2(2, 2);
                _foregroundSprite.Scale = new Vector2(2, 2);
                break;
            case GameTypes.LevelType.Dungeon:
                _backgroundSprite.Scale = Vector2.One;
                _foregroundSprite.Scale = Vector2.One;
                _backgroundSprite.ChangeSprite("DungeonBG1_Sprite", ScreenManager.Camera);
                _foregroundSprite.ChangeSprite("DungeonFG1_Sprite", ScreenManager.Camera);
                _backgroundSprite.Scale = new Vector2(2, 2);
                _foregroundSprite.Scale = new Vector2(2, 2);
                break;
            case GameTypes.LevelType.Garden:
                _backgroundSprite.Scale = Vector2.One;
                _foregroundSprite.Scale = Vector2.One;
                _backgroundSprite.ChangeSprite("GardenBG_Sprite", ScreenManager.Camera);
                _foregroundSprite.ChangeSprite("GardenFG_Sprite", ScreenManager.Camera);
                _backgroundSprite.Scale = new Vector2(2, 2);
                _foregroundSprite.Scale = new Vector2(2, 2);
                break;
        }

        // Setting shadow intensity.
        if (levelType == GameTypes.LevelType.Dungeon)
        {
            Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0.7f);
        }
        else
        {
            Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0);
        }
    }

    public void ShakeScreen(float magnitude, bool horizontalShake = true, bool verticalShake = true)
    {
        _screenShakeMagnitude = magnitude;
        _horizontalShake = horizontalShake;
        _verticalShake = verticalShake;
        _shakeScreen = true;
    }

    // This shake is specific to the Clonus trait.
    public void UpdateShake()
    {
        if (_horizontalShake)
        {
            Player.AttachedLevel.Camera.X +=
                CDGMath.RandomPlusMinus() * (CDGMath.RandomFloat(0, 1) * _screenShakeMagnitude);
        }

        if (_verticalShake)
        {
            Player.AttachedLevel.Camera.Y +=
                CDGMath.RandomPlusMinus() * (CDGMath.RandomFloat(0, 1) * _screenShakeMagnitude);
        }
    }

    public void StopScreenShake()
    {
        GamePad.SetVibration(PlayerIndex.One, 0, 0);
        _shakeScreen = false;
    }

    public void RefreshPlayerHUDPos()
    {
        _playerHUD.SetPosition(new Vector2(20, 40));
    }

    public void UpdatePlayerHUD()
    {
        _playerHUD.Update(Player);
    }

    public void ForcePlayerHUDLevel(int level)
    {
        _playerHUD.forcedPlayerLevel = level;
    }

    public void UpdatePlayerHUDAbilities()
    {
        _playerHUD.UpdateAbilityIcons();
    }

    public void UpdatePlayerHUDSpecialItem()
    {
        _playerHUD.UpdateSpecialItemIcon();
    }

    public void UpdatePlayerSpellIcon()
    {
        _playerHUD.UpdateSpellIcon();
    }

    public void SetMapDisplayVisibility(bool visible)
    {
        MiniMapDisplay.Visible = visible;
    }

    public void SetPlayerHUDVisibility(bool visible)
    {
        _playerHUD.Visible = visible;
    }

    public void SetObjectKilledPlayer(GameObj obj)
    {
        _objKilledPlayer = obj;
    }

    public override void RefreshTextObjs()
    {
        foreach (var room in RoomList)
        {
            room.RefreshTextObjs();
        }

        _playerHUD.RefreshTextObjs();
        _enemyHUD.RefreshTextObjs();
        base.RefreshTextObjs();
    }
}
