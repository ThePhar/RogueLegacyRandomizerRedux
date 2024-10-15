using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RogueCastle.EVs;
using RogueCastle.Screens.BaseObjects;
using Tweener;
using Tweener.Ease;

namespace RogueCastle.Screens;

public class GameOverScreen : Screen
{
    private readonly int            _debugTotalEnemies = 127;
    private          int            _bagsCollected;
    private          int            _bigDiamondsCollected;
    private          int            _coinsCollected;
    private          KeyIconTextObj _continueText;
    private          int            _debugEnemyLocID;
    private          int            _debugGameHint = -1;
    private          ObjContainer   _dialoguePlate;
    private          int            _diamondsCollected;
    private          bool           _droppingStats;
    private          List<EnemyObj> _enemyList;
    private          List<Vector2>  _enemyStoredPositions = [];
    private          int            _gameHint;
    private          bool           _lockControls;
    private          GameObj        _objKilledPlayer;
    private          PlayerObj      _player;
    private          FrameSoundObj  _playerFallSound;
    private          LineageObj     _playerFrame;
    private          SpriteObj      _playerGhost;
    private          FrameSoundObj  _playerSwordFallSound;
    private          FrameSoundObj  _playerSwordSpinSound;
    private          SpriteObj      _spotlight;

    public float BackBufferOpacity { get; set; }

    public override void PassInData(List<object> objList)
    {
        if (objList != null)
        {
            _player = objList[0] as PlayerObj;

            if (_playerFallSound == null)
            {
                _playerFallSound = new FrameSoundObj(_player, 14, "Player_Death_BodyFall");
                _playerSwordSpinSound = new FrameSoundObj(_player, 2, "Player_Death_SwordTwirl");
                _playerSwordFallSound = new FrameSoundObj(_player, 9, "Player_Death_SwordLand");
            }

            _enemyList = objList[1] as List<EnemyObj>;
            _coinsCollected = (int) objList[2];
            _bagsCollected = (int) objList[3];
            _diamondsCollected = (int) objList[4];
            _bigDiamondsCollected = (int) objList[5];
            if (objList[6] != null)
            {
                _objKilledPlayer = objList[6] as GameObj;
            }

            SetObjectKilledPlayerText();

            _enemyStoredPositions.Clear();
            base.PassInData(objList);
        }
    }

    public override void LoadContent()
    {
        _continueText = new KeyIconTextObj(Game.JunicodeFont);
        _continueText.FontSize = 14;
        _continueText.Align = Types.TextAlign.Right;
        _continueText.Opacity = 0;
        _continueText.Position = new Vector2(1320 - 50, 30);
        _continueText.ForceDraw = true;
        _continueText.Text = LocaleBuilder.getString("LOC_ID_CLASS_NAME_1_MALE", _continueText); // dummy locID to add TextObj to language refresh list

        var shadowOffset = new Vector2(2, 2);
        var textColour = new Color(255, 254, 128);

        _dialoguePlate = new ObjContainer("DialogBox_Character");
        _dialoguePlate.Position = new Vector2(1320 / 2, 610);
        _dialoguePlate.ForceDraw = true;

        var deathDescription = new TextObj(Game.JunicodeFont);
        deathDescription.Align = Types.TextAlign.Centre;
        deathDescription.Text = LocaleBuilder.getString("LOC_ID_CLASS_NAME_1_MALE", deathDescription); // dummy locID to add TextObj to language refresh list
        deathDescription.FontSize = 17;
        deathDescription.DropShadow = shadowOffset;
        deathDescription.Position = new Vector2(0, -_dialoguePlate.Height / 2 + 25);
        _dialoguePlate.AddChild(deathDescription);

        var partingWords = new KeyIconTextObj(Game.JunicodeFont);
        partingWords.FontSize = 12;
        partingWords.Align = Types.TextAlign.Centre;
        partingWords.Text = LocaleBuilder.getString("LOC_ID_CLASS_NAME_1_MALE", partingWords); // dummy locID to add TextObj to language refresh list
        partingWords.DropShadow = shadowOffset;
        partingWords.Y = 0;
        partingWords.TextureColor = textColour;
        _dialoguePlate.AddChild(partingWords);

        var partingWordsTitle = new TextObj(Game.JunicodeFont);
        partingWordsTitle.FontSize = 8;
        partingWordsTitle.Text = ""; // LocaleBuilder.getString("LOC_ID_CLASS_NAME_1_MALE", partingWordsTitle); // dummy locID to add TextObj to language refresh list
        partingWordsTitle.Y = partingWords.Y;
        partingWordsTitle.Y += 40;
        partingWordsTitle.X += 20;
        partingWordsTitle.DropShadow = shadowOffset;
        _dialoguePlate.AddChild(partingWordsTitle);

        _playerGhost = new SpriteObj("PlayerGhost_Sprite");
        _playerGhost.AnimationDelay = 1 / 10f;

        _spotlight = new SpriteObj("GameOverSpotlight_Sprite");
        _spotlight.Rotation = 90;
        _spotlight.ForceDraw = true;
        _spotlight.Position = new Vector2(1320 / 2, 40 + _spotlight.Height);

        _playerFrame = new LineageObj(null, true);
        _playerFrame.DisablePlaque = true;

        base.LoadContent();
    }

    public override void OnEnter()
    {
        _debugEnemyLocID = -1;

        // Setting the player frame.  This needs to be done before the stats are erased for the next play.
        _playerFrame.Opacity = 0;
        _playerFrame.Position = _player.Position;
        _playerFrame.SetTraits(Game.PlayerStats.Traits);
        _playerFrame.IsFemale = Game.PlayerStats.IsFemale;
        _playerFrame.Class = Game.PlayerStats.Class;
        _playerFrame.Y -= 120;
        _playerFrame.SetPortrait(Game.PlayerStats.HeadPiece, Game.PlayerStats.ShoulderPiece, Game.PlayerStats.ChestPiece);
        _playerFrame.UpdateData();
        Tween.To(_playerFrame, 1f, Tween.EaseNone, "delay", "4", "Opacity", "1");

        // Creating a new family tree node and saving.
        var newNode = new FamilyTreeNode
        {
            Name = Game.PlayerStats.PlayerName,
            Age = Game.PlayerStats.Age,
            ChildAge = Game.PlayerStats.ChildAge,
            Class = Game.PlayerStats.Class,
            HeadPiece = Game.PlayerStats.HeadPiece,
            ChestPiece = Game.PlayerStats.ChestPiece,
            ShoulderPiece = Game.PlayerStats.ShoulderPiece,
            NumEnemiesBeaten = Game.PlayerStats.NumEnemiesBeaten,
            BeatenABoss = Game.PlayerStats.NewBossBeaten,
            Traits = Game.PlayerStats.Traits,
            IsFemale = Game.PlayerStats.IsFemale,
            RomanNumeral = Game.PlayerStats.RomanNumeral,
        };

        var storedTraits = Game.PlayerStats.Traits;
        Game.PlayerStats.FamilyTreeArray.Add(newNode);
        if (Game.PlayerStats.CurrentBranches != null)
        {
            Game.PlayerStats.CurrentBranches.Clear();
        }

        // Setting necessary after-death flags and saving.
        Game.PlayerStats.IsDead = true;
        Game.PlayerStats.Traits = Vector2.Zero;
        Game.PlayerStats.NewBossBeaten = false;
        Game.PlayerStats.RerolledChildren = false;
        Game.PlayerStats.HasArchitectFee = false;
        Game.PlayerStats.NumEnemiesBeaten = 0;
        Game.PlayerStats.LichHealth = 0;
        Game.PlayerStats.LichMana = 0;
        Game.PlayerStats.LichHealthMod = 1;
        Game.PlayerStats.TimesDead++;
        Game.PlayerStats.LoadStartingRoom = true;
        Game.PlayerStats.EnemiesKilledInRun.Clear();

        if (Game.PlayerStats.SpecialItem != SpecialItemType.FreeEntrance &&
            Game.PlayerStats.SpecialItem != SpecialItemType.EyeballToken &&
            Game.PlayerStats.SpecialItem != SpecialItemType.SkullToken &&
            Game.PlayerStats.SpecialItem != SpecialItemType.FireballToken &&
            Game.PlayerStats.SpecialItem != SpecialItemType.BlobToken &&
            Game.PlayerStats.SpecialItem != SpecialItemType.LastBossToken)
        {
            Game.PlayerStats.SpecialItem = SpecialItemType.None;
        }

        // Ensures the prosopagnosia effect kicks in when selecting an heir.
        if (storedTraits.X == TraitType.Prosopagnosia || storedTraits.Y == TraitType.Prosopagnosia)
        {
            Game.PlayerStats.HasProsopagnosia = true;
        }

        (ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.PlayerData, SaveType.Lineage, SaveType.MapData);
        (ScreenManager.Game as Game).SaveManager.SaveAllFileTypes(true); // Save the backup the moment the player dies.

        // The player's traits need to be restored to so that his death animation matches the player.
        Game.PlayerStats.Traits = storedTraits;

        // Setting achievements.
        if (Game.PlayerStats.TimesDead >= 20)
        {
            GameUtil.UnlockAchievement("FEAR_OF_LIFE");
        }

        ////////////////////////////////////////////////////////////////////////////////

        SoundManager.StopMusic(0.5f);
        _droppingStats = false;
        _lockControls = false;
        SoundManager.PlaySound("Player_Death_FadeToBlack");
        _continueText.Text = LocaleBuilder.getString("LOC_ID_GAME_OVER_SCREEN_1_NEW", _continueText);

        _player.Visible = true;
        _player.Opacity = 1;

        _continueText.Opacity = 0;
        _dialoguePlate.Opacity = 0;
        _playerGhost.Opacity = 0;
        _spotlight.Opacity = 0;

        // Player ghost animation.
        _playerGhost.Position = new Vector2(_player.X - _playerGhost.Width / 2, _player.Bounds.Top - 20);
        Tween.RunFunction(3, typeof(SoundManager), "PlaySound", "Player_Ghost");
        //_ghostSoundTween = Tween.RunFunction(5, typeof(SoundManager), "PlaySound", "Player_Ghost");
        Tween.To(_playerGhost, 0.5f, Linear.EaseNone, "delay", "3", "Opacity", "0.4");
        Tween.By(_playerGhost, 2, Linear.EaseNone, "delay", "3", "Y", "-150");
        _playerGhost.Opacity = 0.4f;
        Tween.To(_playerGhost, 0.5f, Linear.EaseNone, "delay", "4", "Opacity", "0");
        _playerGhost.Opacity = 0;
        _playerGhost.PlayAnimation();

        // Spotlight, Player slain text, and Backbuffer animation.
        Tween.To(this, 0.5f, Linear.EaseNone, "BackBufferOpacity", "1");
        Tween.To(_spotlight, 0.1f, Linear.EaseNone, "delay", "1", "Opacity", "1");
        Tween.AddEndHandlerToLastTween(typeof(SoundManager), "PlaySound", "Player_Death_Spotlight");
        Tween.RunFunction(1.2f, typeof(SoundManager), "PlayMusic", "GameOverStinger", false, 0.5f);
        Tween.To(Camera, 1, Quad.EaseInOut, "X", _player.AbsX.ToString(), "Y", (_player.Bounds.Bottom - 10).ToString(), "Zoom", "1");
        Tween.RunFunction(2f, _player, "RunDeathAnimation1");

        // Setting the dialogue plate info.
        //1 = slain text
        //2 = parting words
        //3 = parting words title.

        if (Game.PlayerStats.Traits.X == TraitType.Tourettes || Game.PlayerStats.Traits.Y == TraitType.Tourettes)
        {
            (_dialoguePlate.GetChildAt(2) as TextObj).Text = "#)!(%*#@!%^"; // not localized
            (_dialoguePlate.GetChildAt(2) as TextObj).RandomizeSentence(true);
        }
        else
        {
            _gameHint = CDGMath.RandomInt(0, GameEV.GameHints.GetLength(0) - 1);
            (_dialoguePlate.GetChildAt(2) as TextObj).Text = LocaleBuilder.getResourceString(GameEV.GameHints[_gameHint]);
            FixHintTextSize();
            //(_dialoguePlate.GetChildAt(2) as TextObj).Text =
            //    LocaleBuilder.getResourceString(GameEV.GAME_HINTS[_gameHint, 0]) +
            //    GameEV.GAME_HINTS[_gameHint, 1] +
            //    LocaleBuilder.getResourceString(GameEV.GAME_HINTS[_gameHint, 2]);
        }

        try
        {
            (_dialoguePlate.GetChildAt(3) as TextObj).ChangeFontNoDefault((_dialoguePlate.GetChildAt(3) as TextObj).defaultFont);
            (_dialoguePlate.GetChildAt(3) as TextObj).Text = string.Format(LocaleBuilder.getResourceString("LOC_ID_GAME_OVER_SCREEN_8_NEW"), Game.NameHelper()); //"'s Parting Words"
            if (LocaleBuilder.languageType != LanguageType.Chinese_Simp && Regex.IsMatch((_dialoguePlate.GetChildAt(3) as TextObj).Text, @"\p{IsCyrillic}"))
            {
                (_dialoguePlate.GetChildAt(3) as TextObj).ChangeFontNoDefault(Game.RobotoSlabFont);
            }
        }
        catch
        {
            (_dialoguePlate.GetChildAt(3) as TextObj).ChangeFontNoDefault(Game.NotoSansSCFont);
            (_dialoguePlate.GetChildAt(3) as TextObj).Text = string.Format(LocaleBuilder.getResourceString("LOC_ID_GAME_OVER_SCREEN_8_NEW"), Game.NameHelper()); //"'s Parting Words"
        }
        //(_dialoguePlate.GetChildAt(3) as TextObj).Text = "-" + Game.PlayerStats.PlayerName + LocaleBuilder.getResourceString("LOC_ID_GAME_OVER_SCREEN_8");

        Tween.To(_dialoguePlate, 0.5f, Tween.EaseNone, "delay", "2", "Opacity", "1");
        Tween.RunFunction(4f, this, "DropStats");
        Tween.To(_continueText, 0.4f, Linear.EaseNone, "delay", "4", "Opacity", "1");

        base.OnEnter();
    }

    public override void OnExit()
    {
        Tween.StopAll(false);
        if (_enemyList != null)
        {
            _enemyList.Clear();
            _enemyList = null;
        }

        Game.PlayerStats.Traits = Vector2.Zero;

        BackBufferOpacity = 0;

        base.OnExit();
    }

    public void DropStats()
    {
        _droppingStats = true;
        var randPos = Vector2.Zero;
        float delay = 0;

        var startingPos = Camera.TopLeftCorner;
        startingPos.X += 200;
        startingPos.Y += 450;
        // Dropping enemies

        //_enemyList = new List<EnemyObj>();
        //for (int i = 0; i < 120; i++)
        //{
        //    EnemyObj_Skeleton enemy = new EnemyObj_Skeleton(_player, null, _player.AttachedLevel, GameTypes.EnemyDifficulty.BASIC);
        //    enemy.Initialize();
        //    _enemyList.Add(enemy);
        //}

        //_enemyList = new List<EnemyObj>();
        //EnemyObj_Fireball fireBall = new EnemyObj_Fireball(_player, null, _player.AttachedLevel, GameTypes.EnemyDifficulty.MINIBOSS);
        //fireBall.Initialize();
        //_enemyList.Add(fireBall);

        //EnemyObj_Eyeball eyeball = new EnemyObj_Eyeball(_player, null, _player.AttachedLevel, GameTypes.EnemyDifficulty.MINIBOSS);
        //eyeball.Initialize();
        //_enemyList.Add(eyeball);

        //EnemyObj_Fairy fairy = new EnemyObj_Fairy(_player, null, _player.AttachedLevel, GameTypes.EnemyDifficulty.MINIBOSS);
        //fairy.Initialize();
        //_enemyList.Add(fairy);

        //EnemyObj_Blob blob = new EnemyObj_Blob(_player, null, _player.AttachedLevel, GameTypes.EnemyDifficulty.MINIBOSS);
        //blob.Initialize();
        //_enemyList.Add(blob);

        if (_enemyList != null)
        {
            foreach (var enemy in _enemyList)
            {
                _enemyStoredPositions.Add(enemy.Position);
                enemy.Position = startingPos;
                enemy.ChangeSprite(enemy.ResetSpriteName);
                if (enemy.SpriteName == "EnemyZombieRise_Character")
                {
                    enemy.ChangeSprite("EnemyZombieWalk_Character");
                }

                enemy.Visible = true;
                enemy.Flip = SpriteEffects.FlipHorizontally;
                Tween.StopAllContaining(enemy, false);
                enemy.Scale = enemy.InternalScale;
                enemy.Scale /= 2;
                enemy.Opacity = 0;
                delay += 0.05f;

                // Special handling for the eyeball boss's pupil.
                var eyeBoss = enemy as EnemyObj_Eyeball;
                if (eyeBoss != null && eyeBoss.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
                {
                    eyeBoss.ChangeToBossPupil();
                }

                Tween.To(enemy, 0f, Tween.EaseNone, "delay", delay.ToString(), "Opacity", "1");
                Tween.RunFunction(delay, this, "PlayEnemySound");
                startingPos.X += 25;
                if (enemy.X + enemy.Width > Camera.TopLeftCorner.X + 200 + 950)
                {
                    startingPos.Y += 30;
                    startingPos.X = Camera.TopLeftCorner.X + 200;
                }
            }
        }
    }

    public void PlayEnemySound()
    {
        SoundManager.PlaySound("Enemy_Kill_Plant");
    }

    private void SetObjectKilledPlayerText()
    {
        var playerSlainText = _dialoguePlate.GetChildAt(1) as TextObj;

        try
        {
            playerSlainText.ChangeFontNoDefault(LocaleBuilder.GetLanguageFont(playerSlainText));
            playerSlainText.Text = Game.PlayerStats.PlayerName;
            if (LocaleBuilder.languageType != LanguageType.Chinese_Simp && Regex.IsMatch(playerSlainText.Text, @"\p{IsCyrillic}"))
            {
                playerSlainText.ChangeFontNoDefault(Game.RobotoSlabFont);
            }
        }
        catch
        {
            playerSlainText.ChangeFontNoDefault(Game.NotoSansSCFont);
        }

        if (_debugEnemyLocID > 0)
        {
            playerSlainText.Text = string.Format(LocaleBuilder.getResourceString("LOC_ID_GAME_OVER_SCREEN_4_NEW"), Game.NameHelper(), LocaleBuilder.getResourceString("LOC_ID_ENEMY_NAME_" + _debugEnemyLocID));
        }
        else
        {
            if (_objKilledPlayer != null)
            {
                var enemy = _objKilledPlayer as EnemyObj;
                var projectile = _objKilledPlayer as ProjectileObj;

                if (enemy != null)
                {
                    if (enemy.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS || enemy is EnemyObj_LastBoss)
                    {
                        playerSlainText.Text = string.Format(LocaleBuilder.getResourceString("LOC_ID_GAME_OVER_SCREEN_3_NEW"), Game.NameHelper(), LocaleBuilder.getResourceString(enemy.LocStringID));
                    }
                    //playerSlainText.Text = Game.PlayerStats.PlayerName + " " + LocaleBuilder.getResourceString("LOC_ID_GAME_OVER_SCREEN_3") + " " + LocaleBuilder.getResourceString(enemy.LocStringID);
                    else
                    {
                        playerSlainText.Text = string.Format(LocaleBuilder.getResourceString("LOC_ID_GAME_OVER_SCREEN_4_NEW"), Game.NameHelper(), LocaleBuilder.getResourceString(enemy.LocStringID));
                    }
                    //playerSlainText.Text = Game.PlayerStats.PlayerName + " " + LocaleBuilder.getResourceString("LOC_ID_GAME_OVER_SCREEN_4") + " " + LocaleBuilder.getResourceString(enemy.LocStringID);
                }
                else if (projectile != null)
                {
                    enemy = projectile.Source as EnemyObj;
                    if (enemy != null)
                    {
                        if (enemy.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS || enemy is EnemyObj_LastBoss)
                        {
                            playerSlainText.Text = string.Format(LocaleBuilder.getResourceString("LOC_ID_GAME_OVER_SCREEN_3_NEW"), Game.NameHelper(), LocaleBuilder.getResourceString(enemy.LocStringID));
                        }
                        //playerSlainText.Text = Game.PlayerStats.PlayerName + " " + LocaleBuilder.getResourceString("LOC_ID_GAME_OVER_SCREEN_3") + " " + LocaleBuilder.getResourceString(enemy.LocStringID);
                        else
                        {
                            playerSlainText.Text = string.Format(LocaleBuilder.getResourceString("LOC_ID_GAME_OVER_SCREEN_4_NEW"), Game.NameHelper(), LocaleBuilder.getResourceString(enemy.LocStringID));
                        }
                        //playerSlainText.Text = Game.PlayerStats.PlayerName + " " + LocaleBuilder.getResourceString("LOC_ID_GAME_OVER_SCREEN_4") + " " + LocaleBuilder.getResourceString(enemy.LocStringID);
                    }
                    else
                    {
                        playerSlainText.Text = string.Format(LocaleBuilder.getResourceString("LOC_ID_GAME_OVER_SCREEN_5_NEW"), Game.NameHelper());
                    }
                    //playerSlainText.Text = Game.PlayerStats.PlayerName + " " + LocaleBuilder.getResourceString("LOC_ID_GAME_OVER_SCREEN_5");
                }

                var hazard = _objKilledPlayer as HazardObj;
                if (hazard != null)
                {
                    playerSlainText.Text = string.Format(LocaleBuilder.getResourceString("LOC_ID_GAME_OVER_SCREEN_6_NEW"), Game.NameHelper());
                }
                //playerSlainText.Text = Game.PlayerStats.PlayerName + " " + LocaleBuilder.getResourceString("LOC_ID_GAME_OVER_SCREEN_6");
            }
            else
            {
                playerSlainText.Text = string.Format(LocaleBuilder.getResourceString("LOC_ID_GAME_OVER_SCREEN_7_NEW"), Game.NameHelper());
            }
            //playerSlainText.Text = Game.PlayerStats.PlayerName + " " + LocaleBuilder.getResourceString("LOC_ID_GAME_OVER_SCREEN_7");
        }
    }

    public override void HandleInput()
    {
        if (_lockControls || !_droppingStats || !(InputHelper.PressedConfirm() || InputHelper.PressedCancel()))
        {
            base.HandleInput();
            return;
        }

        if (_enemyList.Count > 0 && _enemyList[_enemyList.Count - 1].Opacity != 1)
        {
            foreach (var enemy in _enemyList)
            {
                Tween.StopAllContaining(enemy, false);
                enemy.Opacity = 1;
            }

            Tween.StopAllContaining(this, false);
            PlayEnemySound();
        }
        else //if (_continueText.Opacity == 1)
        {
            SkillSystem.ResetAllTraits();
            Game.PlayerStats.Dispose();
            Game.PlayerStats = new PlayerStats();

            (ScreenManager as RCScreenManager)!.Player.Reset();
            (ScreenManager.Game as Game)!.SaveManager.LoadFiles(null, SaveType.PlayerData, SaveType.Lineage, SaveType.UpgradeData);
            Game.ScreenManager.Player.CurrentHealth = Game.PlayerStats.CurrentHealth;
            Game.ScreenManager.Player.CurrentMana = Game.PlayerStats.CurrentMana;

            (ScreenManager as RCScreenManager)!.DisplayScreen(ScreenType.Lineage, true);
            _lockControls = true;
        }

        base.HandleInput();
    }

    public override void Update(GameTime gameTime)
    {
        if (LevelEV.EnableDebugInput)
        {
            HandleDebugInput();
        }

        if (_player.SpriteName == "PlayerDeath_Character")
        {
            _playerFallSound.Update();
            _playerSwordFallSound.Update();
            _playerSwordSpinSound.Update();
        }

        base.Update(gameTime);
    }

    private void HandleDebugInput()
    {
        if (InputManager.JustPressed(Keys.Space, PlayerIndex.One))
        {
            _gameHint = _debugGameHint;
            Console.WriteLine("Changing to game hint index: " + _debugGameHint);

            (_dialoguePlate.GetChildAt(2) as TextObj).Text = LocaleBuilder.getResourceString(GameEV.GameHints[_gameHint]);
            //(_dialoguePlate.GetChildAt(2) as TextObj).Text =
            //    LocaleBuilder.getString(GameEV.GAME_HINTS[_gameHint, 0], _dialoguePlate.GetChildAt(2) as TextObj) +
            //    GameEV.GAME_HINTS[_gameHint, 1] +
            //    LocaleBuilder.getString(GameEV.GAME_HINTS[_gameHint, 2], _dialoguePlate.GetChildAt(2) as TextObj);
            _debugGameHint++;
            if (_debugGameHint >= GameEV.GameHints.GetLength(0))
            {
                _debugGameHint = 0;
            }
        }

        var previousEnemyLocID = _debugEnemyLocID;
        if (InputManager.JustPressed(Keys.OemOpenBrackets, null))
        {
            _debugEnemyLocID--;
        }
        else if (InputManager.JustPressed(Keys.OemCloseBrackets, null))
        {
            _debugEnemyLocID++;
        }

        if (_debugEnemyLocID <= 0 && _debugEnemyLocID != -1)
        {
            _debugEnemyLocID = _debugTotalEnemies;
        }
        else if (_debugEnemyLocID > _debugTotalEnemies)
        {
            _debugEnemyLocID = 1;
        }

        if (_debugEnemyLocID != previousEnemyLocID)
        {
            SetObjectKilledPlayerText();
        }
    }

    public override void Draw(GameTime gameTime)
    {
        //Camera.GraphicsDevice.Clear(Color.Black);
        Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Camera.GetTransformation()); // Parallax Effect has been disabled in favour of ripple effect for now.
        Camera.Draw(Game.GenericTexture, new Rectangle((int) Camera.TopLeftCorner.X - 10, (int) Camera.TopLeftCorner.Y - 10, 1420, 820), Color.Black * BackBufferOpacity);
        foreach (var enemy in _enemyList)
        {
            enemy.Draw(Camera);
        }

        _playerFrame.Draw(Camera);
        _player.Draw(Camera);
        if (_playerGhost.Opacity > 0)
        {
            _playerGhost.X += (float) Math.Sin(Game.TotalGameTime * 5) * 60 * (float) gameTime.ElapsedGameTime.TotalSeconds;
        }

        _playerGhost.Draw(Camera);
        Camera.End();

        Camera.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null, null); // Parallax Effect has been disabled in favour of ripple effect for now.
        _spotlight.Draw(Camera);
        _dialoguePlate.Draw(Camera);
        _continueText.Draw(Camera);
        Camera.End();

        base.Draw(gameTime);
    }

    public override void Dispose()
    {
        if (IsDisposed == false)
        {
            Console.WriteLine("Disposing Game Over Screen");

            _player = null;
            _dialoguePlate.Dispose();
            _dialoguePlate = null;
            _continueText.Dispose();
            _continueText = null;
            _playerGhost.Dispose();
            _playerGhost = null;
            _spotlight.Dispose();
            _spotlight = null;

            if (_playerFallSound != null)
            {
                _playerFallSound.Dispose();
            }

            _playerFallSound = null;
            if (_playerSwordFallSound != null)
            {
                _playerSwordFallSound.Dispose();
            }

            _playerSwordFallSound = null;
            if (_playerSwordSpinSound != null)
            {
                _playerSwordSpinSound.Dispose();
            }

            _playerSwordSpinSound = null;

            _objKilledPlayer = null;

            if (_enemyList != null)
            {
                _enemyList.Clear();
            }

            _enemyList = null;
            if (_enemyStoredPositions != null)
            {
                _enemyStoredPositions.Clear();
            }

            _enemyStoredPositions = null;

            _playerFrame.Dispose();
            _playerFrame = null;
            base.Dispose();
        }
    }

    public override void RefreshTextObjs()
    {
        //_continueText.Text = LocaleBuilder.getResourceString("LOC_ID_GAME_OVER_SCREEN_1") + " [Input:" + InputMapType.MENU_CONFIRM1 + "] " + LocaleBuilder.getResourceString("LOC_ID_GAME_OVER_SCREEN_2");
        (_dialoguePlate.GetChildAt(2) as TextObj).ChangeFontNoDefault(LocaleBuilder.GetLanguageFont(_dialoguePlate.GetChildAt(2) as TextObj));
        (_dialoguePlate.GetChildAt(3) as TextObj).ChangeFontNoDefault(LocaleBuilder.GetLanguageFont(_dialoguePlate.GetChildAt(3) as TextObj));

        if (Game.PlayerStats.Traits.X == TraitType.Tourettes || Game.PlayerStats.Traits.Y == TraitType.Tourettes)
        {
            (_dialoguePlate.GetChildAt(2) as TextObj).Text = "#)!(%*#@!%^"; // not localized
            (_dialoguePlate.GetChildAt(2) as TextObj).RandomizeSentence(true);
        }
        else
        {
            (_dialoguePlate.GetChildAt(2) as TextObj).Text = LocaleBuilder.getResourceString(GameEV.GameHints[_gameHint]);
            //(_dialoguePlate.GetChildAt(2) as TextObj).Text =
            //    LocaleBuilder.getResourceString(GameEV.GAME_HINTS[_gameHint, 0]) +
            //    GameEV.GAME_HINTS[_gameHint, 1] +
            //    LocaleBuilder.getResourceString(GameEV.GAME_HINTS[_gameHint, 2]);
        }

        try
        {
            (_dialoguePlate.GetChildAt(3) as TextObj).ChangeFontNoDefault(LocaleBuilder.GetLanguageFont(_dialoguePlate.GetChildAt(3) as TextObj));
            (_dialoguePlate.GetChildAt(3) as TextObj).Text = string.Format(LocaleBuilder.getResourceString("LOC_ID_GAME_OVER_SCREEN_8_NEW"), Game.NameHelper()); //"'s Parting Words"
            if (LocaleBuilder.languageType != LanguageType.Chinese_Simp && Regex.IsMatch((_dialoguePlate.GetChildAt(3) as TextObj).Text, @"\p{IsCyrillic}"))
            {
                (_dialoguePlate.GetChildAt(3) as TextObj).ChangeFontNoDefault(Game.RobotoSlabFont);
            }
        }
        catch
        {
            (_dialoguePlate.GetChildAt(3) as TextObj).ChangeFontNoDefault(Game.NotoSansSCFont);
            (_dialoguePlate.GetChildAt(3) as TextObj).Text = string.Format(LocaleBuilder.getResourceString("LOC_ID_GAME_OVER_SCREEN_8_NEW"), Game.NameHelper()); //"'s Parting Words"
        }
        //(_dialoguePlate.GetChildAt(3) as TextObj).Text = "-" + Game.PlayerStats.PlayerName + LocaleBuilder.getResourceString("LOC_ID_GAME_OVER_SCREEN_8");

        FixHintTextSize();
        SetObjectKilledPlayerText();
        base.RefreshTextObjs();
    }

    private void FixHintTextSize()
    {
        var partingWords = _dialoguePlate.GetChildAt(2) as TextObj;
        partingWords.FontSize = 12;
        partingWords.ScaleX = 1;

        switch (LocaleBuilder.languageType)
        {
            case LanguageType.Russian:
                if (_gameHint == 6)
                {
                    partingWords.FontSize = 11;
                    partingWords.ScaleX = 0.9f;
                }

                break;
            case LanguageType.French:
                if (_gameHint == 12 || _gameHint == 20)
                {
                    partingWords.ScaleX = 0.9f;
                }
                else if (_gameHint == 35)
                {
                    partingWords.FontSize = 10;
                    partingWords.ScaleX = 0.9f;
                }

                break;
            case LanguageType.German:
                switch (_gameHint)
                {
                    case 18:
                    case 27:
                    case 29:
                    case 30:
                    case 35:
                        partingWords.ScaleX = 0.9f;
                        break;
                }

                break;
            case LanguageType.Portuguese_Brazil:
                if (_gameHint == 18)
                {
                    partingWords.ScaleX = 0.9f;
                }

                break;
            case LanguageType.Polish:
                if (_gameHint == 18)
                {
                    partingWords.ScaleX = 0.9f;
                }

                break;
            case LanguageType.Spanish_Spain:
                if (_gameHint == 29)
                {
                    partingWords.ScaleX = 0.9f;
                }
                else if (_gameHint == 35)
                {
                    partingWords.FontSize = 11;
                    partingWords.ScaleX = 0.9f;
                }

                break;
        }
    }
}
