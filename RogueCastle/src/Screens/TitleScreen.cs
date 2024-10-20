using System;
using System.Collections.Generic;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueCastle.EnvironmentVariables;
using RogueCastle.GameStructs;
using RogueCastle.Managers;
using RogueCastle.RandomChaos2DGodRays;
using RogueCastle.Screens.BaseScreens;
using Tweener;
using Tweener.Ease;

namespace RogueCastle.Screens;

public class TitleScreen : Screen
{
    private SpriteObj _bg;
    private SpriteObj _castle;
    private TextObj _copyrightText;
    private SpriteObj _creditsIcon;
    private KeyIconTextObj _creditsKey;
    private SpriteObj _crown;
    private SpriteObj _dlcIcon;
    private CrepuscularRays _godRay;
    private RenderTarget2D _godRayTexture;
    private float _hardCoreModeOpacity;
    private bool _heroIsDead;
    private SpriteObj _largeCloud1, _largeCloud2, _largeCloud3, _largeCloud4;
    private bool _loadStartingRoom;
    private SpriteObj _logo;
    private SpriteObj _logo2;
    private bool _optionsEntered;
    private SpriteObj _optionsIcon;
    private KeyIconTextObj _optionsKey;
    private PostProcessingManager _ppm;
    private KeyIconTextObj _pressStartText;
    private TextObj _pressStartText2;
    private SpriteObj _profileCard;
    private KeyIconTextObj _profileCardKey;
    private KeyIconTextObj _profileSelectKey;
    private float _randomSeagullSFX;
    private Cue _seagullCue;
    private SpriteObj _smallCloud1, _smallCloud2, _smallCloud3, _smallCloud4, _smallCloud5;
    private bool _startNewGamePlus;
    private bool _startNewLegacy;
    private bool _startPressed;
    private TextObj _titleText;
    private TextObj _versionNumber;

    public override void LoadContent()
    {
        _ppm = new PostProcessingManager(ScreenManager.Game, ScreenManager.Camera);
        _godRay = new CrepuscularRays(ScreenManager.Game, Vector2.One * .5f, "GameSpritesheets/flare3", 2f, .97f, .97f, .5f, 1.25f);
        _ppm.AddEffect(_godRay);

        _godRayTexture = new RenderTarget2D(Camera.GraphicsDevice, 1320, 720, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
        _godRay.lightSource = new Vector2(0.495f, 0.3f);

        _bg = new SpriteObj("TitleBG_Sprite");
        _bg.Scale = new Vector2(1320f / _bg.Width, 720f / _bg.Height);
        _bg.TextureColor = Color.Red;
        _hardCoreModeOpacity = 0f;

        _logo = new SpriteObj("TitleLogo_Sprite") {
            Position = new Vector2(1320 / 2, 720 / 2),
            DropShadow = new Vector2(0, 5),
        };
        _logo2 = new SpriteObj("RandomizerLogo_Sprite")
        {
            Position = _logo.Position + new Vector2(-80, 100),
            DropShadow = new Vector2(0, 5),
        };

        _castle = new SpriteObj("TitleCastle_Sprite") { Scale = new Vector2(2, 2) };
        _castle.Position = new Vector2((1320 / 2) - 30, 720 - (_castle.Height / 2));

        _smallCloud1 = new SpriteObj("TitleSmallCloud1_Sprite") { Position = new Vector2(1320 / 2, 0) };
        _smallCloud2 = new SpriteObj("TitleSmallCloud2_Sprite") { Position = _smallCloud1.Position };
        _smallCloud3 = new SpriteObj("TitleSmallCloud3_Sprite") { Position = _smallCloud1.Position };
        _smallCloud4 = new SpriteObj("TitleSmallCloud4_Sprite") { Position = _smallCloud1.Position };
        _smallCloud5 = new SpriteObj("TitleSmallCloud5_Sprite") { Position = _smallCloud1.Position };

        _largeCloud1 = new SpriteObj("TitleLargeCloud1_Sprite");
        _largeCloud1.Position = new Vector2(0, 720 - _largeCloud1.Height);
        _largeCloud2 = new SpriteObj("TitleLargeCloud2_Sprite");
        _largeCloud2.Position = new Vector2(1320 / 3, 720 - _largeCloud2.Height + 130);
        _largeCloud3 = new SpriteObj("TitleLargeCloud1_Sprite");
        _largeCloud3.Position = new Vector2(1320 / 3 * 2, 720 - _largeCloud3.Height + 50);
        _largeCloud3.Flip = SpriteEffects.FlipHorizontally;
        _largeCloud4 = new SpriteObj("TitleLargeCloud2_Sprite");
        _largeCloud4.Position = new Vector2(1320, 720 - _largeCloud4.Height);
        _largeCloud4.Flip = SpriteEffects.FlipHorizontally;

        _titleText = new TextObj {
            Font = Game.JunicodeFont,
            FontSize = 45,
            Text = "ROGUE CASTLE",
            Position = new Vector2(1320 / 2, (720 / 2) - 300),
            Align = Types.TextAlign.Centre,
        };

        _copyrightText = new TextObj(Game.JunicodeFont) {
            FontSize = 8,
            Text = "Copyright(C) 2013-2018, Cellar Door Games Inc. Rogue Legacy(TM) is a trademark or registered trademark of Cellar Door Games Inc. All Rights Reserved.",
            Align = Types.TextAlign.Centre,
            DropShadow = new Vector2(1, 2),
        };
        _copyrightText.Position = new Vector2(1320 / 2, 720 - _copyrightText.Height - 10);

        _versionNumber = _copyrightText.Clone() as TextObj;
        _versionNumber!.Align = Types.TextAlign.Right;
        _versionNumber.FontSize = 8;
        _versionNumber.Position = new Vector2(1320 - 15, 5);
        _versionNumber.Text = $"RL   {LevelEV.GAME_VERSION}\nRLR {LevelEV.RLRX_VERSION}";

        _pressStartText = new KeyIconTextObj(Game.JunicodeFont) {
            FontSize = 20,
            Text = "Press Enter to begin",
            Align = Types.TextAlign.Centre,
            Position = new Vector2(1320 / 2, (720 / 2) + 200),
            DropShadow = new Vector2(2, 2),
        };

        _pressStartText2 = new TextObj(Game.JunicodeFont) {
            FontSize = 20,
            Align = Types.TextAlign.Centre,
            Position = _pressStartText.Position,
            DropShadow = new Vector2(2, 2),
        };
        _pressStartText2.Y -= _pressStartText.Height - 5;
        _pressStartText2.Text = "LOC_ID_CLASS_NAME_1_MALE".GetString(_pressStartText2);

        _profileCard = new SpriteObj("TitleProfileCard_Sprite") {
            OutlineWidth = 2,
            Scale = new Vector2(2, 2),
            ForceDraw = true,
        };
        _profileCard.Position = new Vector2(_profileCard.Width, 720 - _profileCard.Height);

        _optionsIcon = new SpriteObj("TitleOptionsIcon_Sprite") {
            Scale = new Vector2(2, 2),
            OutlineWidth = _profileCard.OutlineWidth,
            ForceDraw = true,
        };
        _optionsIcon.Position = new Vector2(1320 - (_optionsIcon.Width * 2), _profileCard.Y);

        _creditsIcon = new SpriteObj("TitleCreditsIcon_Sprite") {
            Scale = new Vector2(2, 2),
            OutlineWidth = _profileCard.OutlineWidth,
            ForceDraw = true,
            Position = new Vector2(_optionsIcon.X + 120, _profileCard.Y),
        };

        _profileCardKey = new KeyIconTextObj(Game.JunicodeFont) {
            Align = Types.TextAlign.Centre,
            FontSize = 12,
            Text = "[Input:" + InputMapType.MENU_PROFILECARD + "]",
            ForceDraw = true,
        };
        _profileCardKey.Position = new Vector2(_profileCard.X, _profileCard.Bounds.Top - _profileCardKey.Height - 10);

        _optionsKey = new KeyIconTextObj(Game.JunicodeFont) {
            Align = Types.TextAlign.Centre,
            FontSize = 12,
            Text = "[Input:" + InputMapType.MENU_OPTIONS + "]",
            ForceDraw = true,
        };
        _optionsKey.Position = new Vector2(_optionsIcon.X, _optionsIcon.Bounds.Top - _optionsKey.Height - 10);

        _creditsKey = new KeyIconTextObj(Game.JunicodeFont) {
            Align = Types.TextAlign.Centre,
            FontSize = 12,
            Text = "[Input:" + InputMapType.MENU_CREDITS + "]",
            ForceDraw = true,
        };
        _creditsKey.Position = new Vector2(_creditsIcon.X, _creditsIcon.Bounds.Top - _creditsKey.Height - 10);

        _profileSelectKey = new KeyIconTextObj(Game.JunicodeFont) {
            Align = Types.TextAlign.Left,
            FontSize = 10,
            Position = new Vector2(30, 15),
            ForceDraw = true,
            DropShadow = new Vector2(2, 2),
        };
        _profileSelectKey.Text = string.Format("LOC_ID_BACK_TO_MENU_OPTIONS_3_NEW".GetString(_profileSelectKey), Game.GameConfig.ProfileSlot);

        _crown = new SpriteObj("Crown_Sprite") {
            ForceDraw = true,
            Scale = new Vector2(0.7f, 0.7f),
            Rotation = -30,
            OutlineWidth = 2,
        };

        _dlcIcon = new SpriteObj("MedallionPiece5_Sprite") {
            Position = new Vector2(950, 310),
            ForceDraw = true,
            TextureColor = Color.Yellow,
        };

        base.LoadContent();
    }

    public override void OnEnter()
    {
        Game.HoursPlayedSinceLastSave = 0;

        Camera.Zoom = 1;
        _profileSelectKey.Text = Game.GameConfig.ProfileSlot == 0
            ? "LOC_ID_BACK_TO_MENU_OPTIONS_4_NEW".GetResourceString()
            : "LOC_ID_BACK_TO_MENU_OPTIONS_3_NEW".FormatResourceString(Game.GameConfig.ProfileSlot);

        // Setting initial data.
        SoundManager.PlayMusic("TitleScreenSong", true, 1f);
        Game.ScreenManager.Player.ForceInvincible = false; //TEDDY - DISABLE INVINCIBILITY WHEN YOU ENTER GAME PROPER.

        _optionsEntered = false;
        _startNewLegacy = false;
        _heroIsDead = false;
        _startNewGamePlus = false;
        _loadStartingRoom = false;

        _bg.TextureColor = Color.Red;
        _crown.Visible = false;

        _randomSeagullSFX = CDGMath.RandomInt(1, 5);
        _startPressed = false;
        Tween.By(_godRay, 5, Quad.EaseInOut, "Y", "-0.23");
        _logo.Opacity = 0;
        _logo.Position = new Vector2(1320 / 2, (720 / 2) - 50);
        Tween.To(_logo, 2, Linear.EaseNone, "Opacity", "1");
        Tween.To(_logo, 3, Quad.EaseInOut, "Y", "360");

        _logo2.Opacity = 0;
        _logo2.Position = _logo.Position + new Vector2(-245, 50);
        Tween.To(_logo2, 2, Linear.EaseNone, "Opacity", "1");
        Tween.To(_logo2, 3, Quad.EaseInOut, "Y", "430");

        _crown.Opacity = 0;
        _crown.Position = new Vector2(390, 250 - 50);
        Tween.To(_crown, 2, Linear.EaseNone, "Opacity", "1");
        Tween.By(_crown, 3, Quad.EaseInOut, "Y", "50");

        _dlcIcon.Opacity = 0;
        _dlcIcon.Visible = Game.PlayerStats.ChallengeLastBossBeaten;

        _dlcIcon.Position = new Vector2(898, 317 - 50);
        Tween.To(_dlcIcon, 2, Linear.EaseNone, "Opacity", "1");
        Tween.By(_dlcIcon, 3, Quad.EaseInOut, "Y", "50");

        Camera.Position = new Vector2(1320 / 2, 720 / 2);

        _pressStartText.Text = "[Input:" + InputMapType.MENU_CONFIRM1 + "]";

        // Setting up save data.
        LoadSaveData();
        Game.PlayerStats.TutorialComplete =
            true; // Force this to true since you can't be in the title screen without going through the tutorial first.

        _startNewLegacy = !Game.PlayerStats.CharacterFound;
        _heroIsDead = Game.PlayerStats.IsDead;
        _startNewGamePlus = Game.PlayerStats.LastbossBeaten;
        _loadStartingRoom = Game.PlayerStats.LoadStartingRoom;

        if (Game.PlayerStats.TimesCastleBeaten > 0)
        {
            _crown.Visible = true;
            _bg.TextureColor = Color.White;
        }

        InitializeStartingText();

        UpdateCopyrightText();

        base.OnEnter();
    }

    public void UpdateCopyrightText()
    {
        _copyrightText.ChangeFontNoDefault(_copyrightText.GetLanguageFont());
        _copyrightText.Text = "LOC_ID_COPYRIGHT_GENERIC".GetResourceString(true) + " " +
                               string.Format("LOC_ID_TRADEMARK_GENERIC".GetResourceString(true), "Rogue Legacy");
    }

    public override void OnExit()
    {
        if (_seagullCue != null && _seagullCue.IsPlaying)
        {
            _seagullCue.Stop(AudioStopOptions.Immediate);
            _seagullCue.Dispose();
        }

        base.OnExit();
    }

    public void LoadSaveData()
    {
        SkillSystem.ResetAllTraits();
        Game.PlayerStats.Dispose();
        Game.PlayerStats = new PlayerStats();
        (ScreenManager as RCScreenManager).Player.Reset();
        (ScreenManager.Game as Game).SaveManager.LoadFiles(null, SaveType.PlayerData, SaveType.Lineage,
            SaveType.UpgradeData);
        // Special circumstance where you should override player's current HP/MP
        Game.ScreenManager.Player.CurrentHealth = Game.PlayerStats.CurrentHealth;
        Game.ScreenManager.Player.CurrentMana = Game.PlayerStats.CurrentMana;
    }

    public void InitializeStartingText()
    {
        _pressStartText2.ChangeFontNoDefault(_pressStartText.GetLanguageFont());

        // Slot not picked.
        if (Game.GameConfig.ProfileSlot == 0)
        {
            _pressStartText2.Text = "LOC_ID_TITLE_SCREEN_1".GetResourceString();
        }
        else if (_startNewLegacy == false)
        {
            // You have an active character who is not dead. Therefore begin the game like normal.
            if (_heroIsDead == false)
            {
                if (Game.PlayerStats.TimesCastleBeaten == 1)
                {
                    _pressStartText2.Text = "LOC_ID_TITLE_SCREEN_2".GetResourceString() + " +";
                }
                else if (Game.PlayerStats.TimesCastleBeaten > 1)
                {
                    _pressStartText2.Text = "LOC_ID_TITLE_SCREEN_2".GetResourceString() + " +" +
                                             Game.PlayerStats.TimesCastleBeaten;
                }
                else
                {
                    _pressStartText2.Text = "LOC_ID_TITLE_SCREEN_2".GetResourceString();
                }
            }
            else // You have an active character but he died. Go to legacy screen.
            {
                if (Game.PlayerStats.TimesCastleBeaten == 1)
                {
                    _pressStartText2.Text = "LOC_ID_TITLE_SCREEN_3".GetResourceString(true) + " +";
                }
                else if (Game.PlayerStats.TimesCastleBeaten > 1)
                {
                    _pressStartText2.Text = "LOC_ID_TITLE_SCREEN_3".GetResourceString(true) + " +" +
                                             Game.PlayerStats.TimesCastleBeaten;
                }
                else
                {
                    _pressStartText2.Text = "LOC_ID_TITLE_SCREEN_3".GetResourceString(true);
                }
            }
        }
        else
        {
            // No character was found, and the castle was never beaten. Therefore you are starting a new game.
            if (_startNewGamePlus == false)
            {
                _pressStartText2.Text = "LOC_ID_TITLE_SCREEN_4".GetResourceString(true);
            }
            else // You've beaten the castle at least once, which means it's new game plus.
            {
                if (Game.PlayerStats.TimesCastleBeaten == 1)
                {
                    _pressStartText2.Text = "LOC_ID_TITLE_SCREEN_4".GetResourceString(true) + " +";
                }
                else
                {
                    _pressStartText2.Text = "LOC_ID_TITLE_SCREEN_4".GetResourceString(true) + " +" +
                                             Game.PlayerStats.TimesCastleBeaten;
                }
            }
        }
    }

    public void StartPressed()
    {
        var game = ScreenManager.Game as Game;

        // Need to be connected and choose a slot.
        if (game!.ArchipelagoManager.Status != ConnectionStatus.Ready || Game.GameConfig.ProfileSlot == 0)
        {
            Game.ScreenManager.DisplayScreen(ScreenType.PROFILE_SELECT, false);
            return;
        }

        SoundManager.PlaySound("Game_Start");

        if (_startNewLegacy == false)
        {
            if (_heroIsDead == false) // Loading a previous file.
            {
                if (_loadStartingRoom)
                {
                    (ScreenManager as RCScreenManager).DisplayScreen(ScreenType.STARTING_ROOM, true);
                }
                else
                {
                    (ScreenManager as RCScreenManager).DisplayScreen(ScreenType.LEVEL, true);
                }
            }
            else // Selecting a new heir.
            {
                (ScreenManager as RCScreenManager).DisplayScreen(ScreenType.LINEAGE, true);
            }
        }
        else
        {
            Game.PlayerStats.CharacterFound = true; // Signifies that a new game is being started.
            // Start a brand new game.
            if (_startNewGamePlus) // If start new game plus is true, erase these flags.
            {
                Game.PlayerStats.LastbossBeaten = false;
                Game.PlayerStats.BlobBossBeaten = false;
                Game.PlayerStats.EyeballBossBeaten = false;
                Game.PlayerStats.FairyBossBeaten = false;
                Game.PlayerStats.FireballBossBeaten = false;
                Game.PlayerStats.FinalDoorOpened = false;
                if ((ScreenManager.Game as Game).SaveManager.FileExists(SaveType.Map))
                {
                    (ScreenManager.Game as Game).SaveManager.ClearFiles(SaveType.Map, SaveType.MapData);
                    (ScreenManager.Game as Game).SaveManager.ClearBackupFiles(SaveType.Map, SaveType.MapData);
                }
            }
            else
            {
                Game.PlayerStats.Gold = 0;
            }

            // Default data that needs to be restarted when starting a new game.
            //Game.PlayerStats.Gold = 0;
            Game.PlayerStats.HeadPiece =
                (byte)CDGMath.RandomInt(1,
                    PlayerPart.NUM_HEAD_PIECES); // Necessary to change his headpiece so he doesn't look like the first dude.
            Game.PlayerStats.EnemiesKilledInRun.Clear();

            (ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.PlayerData, SaveType.Lineage,
                SaveType.UpgradeData); // Create new player, lineage, and upgrade data.
            (ScreenManager as RCScreenManager).DisplayScreen(ScreenType.STARTING_ROOM, true);
        }

        SoundManager.StopMusic(0.2f);
    }

    public override void Update(GameTime gameTime)
    {
        //_castle.Position = new Vector2(InputSystem.InputManager.MouseX, InputSystem.InputManager.MouseY);
        if (_randomSeagullSFX > 0)
        {
            _randomSeagullSFX -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_randomSeagullSFX <= 0)
            {
                if (_seagullCue != null && _seagullCue.IsPlaying)
                {
                    _seagullCue.Stop(AudioStopOptions.Immediate);
                    _seagullCue.Dispose();
                }

                _seagullCue = SoundManager.PlaySound("Wind1");
                _randomSeagullSFX = CDGMath.RandomInt(10, 15);
            }
        }

        var elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

        //_smallCloud1.Rotation += 0.03f;
        //_smallCloud2.Rotation += 0.02f;
        //_smallCloud3.Rotation += 0.05f;
        //_smallCloud4.Rotation -= 0.01f;
        //_smallCloud5.Rotation -= 0.03f;

        _smallCloud1.Rotation += 1.8f * elapsedSeconds;
        _smallCloud2.Rotation += 1.2f * elapsedSeconds;
        _smallCloud3.Rotation += 3 * elapsedSeconds;
        _smallCloud4.Rotation -= 0.6f * elapsedSeconds;
        _smallCloud5.Rotation -= 1.8f * elapsedSeconds;


        //_largeCloud2.X += 0.04f;
        _largeCloud2.X += 2.4f * elapsedSeconds;
        if (_largeCloud2.Bounds.Left > 1320)
        {
            _largeCloud2.X = 0 - (_largeCloud2.Width / 2);
        }

        //_largeCloud3.X -= 0.05f;
        _largeCloud3.X -= 3 * elapsedSeconds;
        if (_largeCloud3.Bounds.Right < 0)
        {
            _largeCloud3.X = 1320 + (_largeCloud3.Width / 2);
        }

        if (_startPressed == false)
        {
            _pressStartText.Opacity = (float)Math.Abs(Math.Sin(Game.TotalGameTime * 1));
            //if (_pressStartText.Opacity < 0.5f) _pressStartText.Opacity = 0.5f;
        }

        // Gives it a glow effect by growing and shrinking.
        _godRay.LightSourceSize = 1 + ((float)Math.Abs(Math.Sin(Game.TotalGameTime * 0.5f)) * 0.5f);

        // Needed to refresh the save state in case the player entered the options screen and deleted his/her file.
        if (_optionsEntered && Game.ScreenManager.CurrentScreen == this)
        {
            _optionsEntered = false;
            // Recheck these buttons.
            _optionsKey.Text = "[Input:" + InputMapType.MENU_OPTIONS + "]";
            _profileCardKey.Text = "[Input:" + InputMapType.MENU_PROFILECARD + "]";
            _creditsKey.Text = "[Input:" + InputMapType.MENU_CREDITS + "]";
            _profileSelectKey.Text = string.Format("LOC_ID_BACK_TO_MENU_OPTIONS_3_NEW".GetString(_profileSelectKey),
                Game.GameConfig.ProfileSlot);
            //_profileSelectKey.Text = "[Input:" + InputMapType.MENU_PROFILESELECT + "] " + LocaleBuilder.getString("LOC_ID_BACK_TO_MENU_OPTIONS_3", _profileSelectKey) + " (" + Game.GameConfig.ProfileSlot + ")";

            // Recheck save data. This might not be needed any longer. Not sure.
            //InitializeSaveData();
        }

        base.Update(gameTime);
    }

    public override void HandleInput()
    {
        HandleAchievementInput();

        //ChangeRay();
        if (Game.GlobalInput.PressedConfirm())
        {
            StartPressed();
        }

        if (_startNewLegacy == false)
        {
            if (Game.GlobalInput.JustPressed(InputMapType.MENU_PROFILECARD))
            {
                (ScreenManager as RCScreenManager).DisplayScreen(ScreenType.PROFILE_CARD, false);
            }
        }

        if (Game.GlobalInput.JustPressed(InputMapType.MENU_OPTIONS))
        {
            _optionsEntered = true;
            var optionsData = new List<object>();
            optionsData.Add(true);
            (ScreenManager as RCScreenManager).DisplayScreen(ScreenType.OPTIONS, false, optionsData);
        }

        if (Game.GlobalInput.JustPressed(InputMapType
                .MENU_CREDITS)) // && InputManager.Pressed(Keys.LeftAlt, PlayerIndex.One) == false && InputManager.Pressed(Keys.RightAlt, PlayerIndex.One) == false) // Make sure not to load credits if alttabbing.                
        {
            (ScreenManager as RCScreenManager).DisplayScreen(ScreenType.CREDITS, false);
        }

        if (Game.GlobalInput.JustPressed(InputMapType.MENU_PROFILESELECT))
        {
            (ScreenManager as RCScreenManager).DisplayScreen(ScreenType.PROFILE_SELECT, false);
        }

        base.HandleInput();
    }

    public void HandleAchievementInput()
    {
        if (InputManager.Pressed(Keys.LeftAlt, PlayerIndex.One) && InputManager.Pressed(Keys.CapsLock, PlayerIndex.One))
        {
            if (InputManager.JustPressed(Keys.T, PlayerIndex.One))
            {
                if (GameUtil.IsAchievementUnlocked("FEAR_OF_LIFE") &&
                    GameUtil.IsAchievementUnlocked("FEAR_OF_DECISIONS") &&
                    GameUtil.IsAchievementUnlocked("FEAR_OF_WEALTH") &&
                    GameUtil.IsAchievementUnlocked("LOVE_OF_GOLD") &&
                    GameUtil.IsAchievementUnlocked("FEAR_OF_NUDITY") &&
                    GameUtil.IsAchievementUnlocked("FEAR_OF_THROWING_STUFF_OUT") &&
                    GameUtil.IsAchievementUnlocked("LOVE_OF_MAGIC") &&
                    GameUtil.IsAchievementUnlocked("LOVE_OF_CHANGE") &&
                    GameUtil.IsAchievementUnlocked("FEAR_OF_EYES") &&
                    GameUtil.IsAchievementUnlocked("FEAR_OF_GHOSTS") &&
                    GameUtil.IsAchievementUnlocked("FEAR_OF_FIRE") &&
                    GameUtil.IsAchievementUnlocked("FEAR_OF_SLIME") &&
                    GameUtil.IsAchievementUnlocked("FEAR_OF_FATHERS") &&
                    GameUtil.IsAchievementUnlocked("FEAR_OF_ANIMALS") &&
                    GameUtil.IsAchievementUnlocked("FEAR_OF_TWINS") &&
                    GameUtil.IsAchievementUnlocked("LOVE_OF_BOOKS") &&
                    GameUtil.IsAchievementUnlocked("FEAR_OF_CHICKENS") &&
                    GameUtil.IsAchievementUnlocked("FEAR_OF_GRAVITY") &&
                    GameUtil.IsAchievementUnlocked("FEAR_OF_IMPERFECTIONS") &&
                    GameUtil.IsAchievementUnlocked("FEAR_OF_SLEEP") &&
                    GameUtil.IsAchievementUnlocked("LOVE_OF_CLOWNS") &&
                    GameUtil.IsAchievementUnlocked("LOVE_OF_KNOWLEDGE") &&
                    GameUtil.IsAchievementUnlocked("FEAR_OF_RELATIVES") &&
                    GameUtil.IsAchievementUnlocked("FEAR_OF_CHEMICALS") &&
                    GameUtil.IsAchievementUnlocked("FEAR_OF_BONES") &&
                    GameUtil.IsAchievementUnlocked("FEAR_OF_BLINDNESS") &&
                    GameUtil.IsAchievementUnlocked("FEAR_OF_SPACE") &&
                    GameUtil.IsAchievementUnlocked("LOVE_OF_LAUGHING_AT_OTHERS"))
                {
                    Console.WriteLine("UNLOCKED THANATOPHOBIA");
                    GameUtil.UnlockAchievement("FEAR_OF_DYING");
                }
            }
            else if (InputManager.JustPressed(Keys.S, PlayerIndex.One))
            {
                Console.WriteLine("UNLOCKED SOMNIPHOBIA");
                GameUtil.UnlockAchievement("FEAR_OF_SLEEP");
            }
        }
    }

    public void ChangeRay()
    {
        //if (Keyboard.GetState().IsKeyDown(Keys.F1))
        //    _godRay.lightTexture = ScreenManager.Game.Content.Load<Texture2D>("GameSpritesheets/flare");
        //if (Keyboard.GetState().IsKeyDown(Keys.F2))
        //    _godRay.lightTexture = ScreenManager.Game.Content.Load<Texture2D>("GameSpritesheets/flare2");
        //if (Keyboard.GetState().IsKeyDown(Keys.F3))
        //    _godRay.lightTexture = ScreenManager.Game.Content.Load<Texture2D>("GameSpritesheets/flare3");
        //if (Keyboard.GetState().IsKeyDown(Keys.F4))
        //    _godRay.lightTexture = ScreenManager.Game.Content.Load<Texture2D>("GameSpritesheets/flare4");

        if (Keyboard.GetState().IsKeyDown(Keys.Up))
        {
            _godRay.lightSource = new Vector2(_godRay.lightSource.X, _godRay.lightSource.Y - .01f);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Down))
        {
            _godRay.lightSource = new Vector2(_godRay.lightSource.X, _godRay.lightSource.Y + .01f);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            _godRay.lightSource = new Vector2(_godRay.lightSource.X - .01f, _godRay.lightSource.Y);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            _godRay.lightSource = new Vector2(_godRay.lightSource.X + .01f, _godRay.lightSource.Y);
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Y))
        {
            _godRay.Exposure += .01f;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.H))
        {
            _godRay.Exposure -= .01f;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.U))
        {
            _godRay.LightSourceSize += .01f;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.J))
        {
            _godRay.LightSourceSize -= .01f;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.I))
        {
            _godRay.Density += .01f;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.K))
        {
            _godRay.Density -= .01f;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.O))
        {
            _godRay.Decay += .01f;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.L))
        {
            _godRay.Decay -= .01f;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.P))
        {
            _godRay.Weight += .01f;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.OemSemicolon))
        {
            _godRay.Weight -= .01f;
        }
    }

    public override void Draw(GameTime gameTime)
    {
        Camera.GraphicsDevice.SetRenderTarget(_godRayTexture);
        Camera.GraphicsDevice.Clear(Color.White);
        Camera.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, null,
            null); // Anything that is affected by the godray should be drawn here.
        _smallCloud1.DrawOutline(Camera);
        _smallCloud3.DrawOutline(Camera);
        _smallCloud4.DrawOutline(Camera);
        Camera.End();

        Camera.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null);
        _castle.DrawOutline(Camera);
        Camera.End();

        Camera.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, null, null);
        _smallCloud2.DrawOutline(Camera);
        _smallCloud5.DrawOutline(Camera);
        _logo.DrawOutline(Camera);
        _logo2.DrawOutline(Camera);
        _dlcIcon.DrawOutline(Camera);
        _crown.DrawOutline(Camera);
        //_largeCloud1.DrawOutline(Camera);
        //_largeCloud2.DrawOutline(Camera);
        //_largeCloud3.DrawOutline(Camera);
        //_largeCloud4.DrawOutline(Camera);
        Camera.End();

        // Draw the post-processed stuff to the godray render target
        _ppm.Draw(gameTime, _godRayTexture);

        //Anything not affected by god ray should get drawn here.
        Camera.GraphicsDevice.SetRenderTarget(_godRayTexture);
        Camera.GraphicsDevice.Clear(Color.Black);
        Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
        _bg.Draw(Camera);
        _smallCloud1.Draw(Camera);
        _smallCloud3.Draw(Camera);
        _smallCloud4.Draw(Camera);
        Camera.End();

        Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
        _castle.Draw(Camera);
        Camera.End();

        Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
        _smallCloud2.Draw(Camera);
        _smallCloud5.Draw(Camera);
        _largeCloud1.Draw(Camera);
        _largeCloud2.Draw(Camera);
        _largeCloud3.Draw(Camera);
        _largeCloud4.Draw(Camera);
        Camera.End();

        Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null);
        Camera.Draw(Game.GenericTexture, new Rectangle(-10, -10, 1400, 800), Color.Black * _hardCoreModeOpacity);
        Camera.End();

        Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null);
        _logo.Draw(Camera);
        _logo2.Draw(Camera);
        _crown.Draw(Camera);
        _copyrightText.Draw(Camera);
        _versionNumber.Draw(Camera);

        _pressStartText2.Opacity = _pressStartText.Opacity;
        _pressStartText.Draw(Camera);
        _pressStartText2.Draw(Camera);

        if (_startNewLegacy == false)
        {
            _profileCardKey.Draw(Camera);
        }

        _creditsKey.Draw(Camera);
        _optionsKey.Draw(Camera);
        _profileSelectKey.Draw(Camera);
        Camera.End();

        Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, null, null);
        if (_startNewLegacy == false)
        {
            _profileCard.Draw(Camera);
        }

        _dlcIcon.Draw(Camera);
        _optionsIcon.Draw(Camera);
        _creditsIcon.Draw(Camera);
        Camera.End();

        // Draw the render targets to the screen
        Camera.GraphicsDevice.SetRenderTarget((ScreenManager as RCScreenManager).RenderTarget);
        Camera.GraphicsDevice.Clear(Color.Black);

        Camera.Begin(SpriteSortMode.Immediate, BlendState.Additive);
        Camera.Draw(_ppm.Scene,
            new Rectangle(0, 0, Camera.GraphicsDevice.Viewport.Width, Camera.GraphicsDevice.Viewport.Height),
            Color.White);
        Camera.Draw(_godRayTexture,
            new Rectangle(0, 0, Camera.GraphicsDevice.Viewport.Width, Camera.GraphicsDevice.Viewport.Height),
            Color.White);
        Camera.End();

        base.Draw(gameTime);
    }

    public override void Dispose()
    {
        if (IsDisposed == false)
        {
            Console.WriteLine("Disposing Title Screen");
            _godRayTexture.Dispose();
            _godRayTexture = null;

            _bg.Dispose();
            _bg = null;
            _logo.Dispose();
            _logo = null;
            _logo2.Dispose();
            _logo2 = null;
            _castle.Dispose();
            _castle = null;

            _smallCloud1.Dispose();
            _smallCloud2.Dispose();
            _smallCloud3.Dispose();
            _smallCloud4.Dispose();
            _smallCloud5.Dispose();
            _smallCloud1 = null;
            _smallCloud2 = null;
            _smallCloud3 = null;
            _smallCloud4 = null;
            _smallCloud5 = null;

            _largeCloud1.Dispose();
            _largeCloud1 = null;
            _largeCloud2.Dispose();
            _largeCloud2 = null;
            _largeCloud3.Dispose();
            _largeCloud3 = null;
            _largeCloud4.Dispose();
            _largeCloud4 = null;

            _pressStartText.Dispose();
            _pressStartText = null;
            _pressStartText2.Dispose();
            _pressStartText2 = null;
            _copyrightText.Dispose();
            _copyrightText = null;
            _versionNumber.Dispose();
            _versionNumber = null;
            _titleText.Dispose();
            _titleText = null;

            _profileCard.Dispose();
            _profileCard = null;
            _optionsIcon.Dispose();
            _optionsIcon = null;
            _creditsIcon.Dispose();
            _creditsIcon = null;

            _profileCardKey.Dispose();
            _profileCardKey = null;
            _optionsKey.Dispose();
            _optionsKey = null;
            _creditsKey.Dispose();
            _creditsKey = null;
            _crown.Dispose();
            _crown = null;
            _profileSelectKey.Dispose();
            _profileSelectKey = null;

            _dlcIcon.Dispose();
            _dlcIcon = null;

            _seagullCue = null;
            base.Dispose();
        }
    }

    public override void RefreshTextObjs()
    {
        UpdateCopyrightText();
        _profileSelectKey.Text = string.Format("LOC_ID_BACK_TO_MENU_OPTIONS_3_NEW".GetResourceString(),
            Game.GameConfig.ProfileSlot);
        //_profileSelectKey.Text = "[Input:" + InputMapType.MENU_PROFILESELECT + "] " + LocaleBuilder.getResourceString("LOC_ID_BACK_TO_MENU_OPTIONS_3") + " (" + Game.GameConfig.ProfileSlot + ")";
        InitializeStartingText(); // refreshes _pressStartText2
        base.RefreshTextObjs();
    }
}
