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

namespace RogueCastle.Screens;

public class PauseScreen : Screen
{
    private TextObj            _apDebugText;
    private sbyte              _classDebugCounter;
    private List<PauseInfoObj> _infoObjList;
    private float              _inputDelay;
    private SpriteObj          _optionsIcon;
    private KeyIconTextObj     _optionsKey;
    private SpriteObj          _profileCard;
    private KeyIconTextObj     _profileCardKey;
    private SpriteObj          _titleText;

    public PauseScreen()
    {
        DrawIfCovered = true;
    }

    public override void LoadContent()
    {
        _titleText = new SpriteObj("GamePausedTitleText_Sprite");
        _titleText.X = GlobalEV.SCREEN_WIDTH / 2;
        _titleText.Y = GlobalEV.SCREEN_HEIGHT * 0.1f;
        _titleText.ForceDraw = true;

        _apDebugText = new TextObj(Game.BitFont)
        {
            X = 32,
            Y = 32,
            OutlineColour = Color.Black,
            OutlineWidth = 2,
            ForceDraw = true,
        };

        _infoObjList = new List<PauseInfoObj>();
        _infoObjList.Add(new PauseInfoObj()); // Adding an info obj for the player.

        _profileCard = new SpriteObj("TitleProfileCard_Sprite");
        _profileCard.OutlineWidth = 2;
        _profileCard.Scale = new Vector2(2, 2);
        _profileCard.Position = new Vector2(_profileCard.Width, 720 - _profileCard.Height);
        _profileCard.ForceDraw = true;

        _optionsIcon = new SpriteObj("TitleOptionsIcon_Sprite");
        _optionsIcon.Scale = new Vector2(2, 2);
        _optionsIcon.OutlineWidth = _profileCard.OutlineWidth;
        _optionsIcon.Position = new Vector2(1320 - _optionsIcon.Width * 2 + 120, _profileCard.Y);
        _optionsIcon.ForceDraw = true;

        _profileCardKey = new KeyIconTextObj(Game.JunicodeFont);
        _profileCardKey.Align = Types.TextAlign.Centre;
        _profileCardKey.FontSize = 12;
        _profileCardKey.Text = "[Input:" + InputMapType.MENU_PROFILECARD + "]";
        _profileCardKey.Position = new Vector2(_profileCard.X, _profileCard.Bounds.Top - _profileCardKey.Height - 10);
        _profileCardKey.ForceDraw = true;

        _optionsKey = new KeyIconTextObj(Game.JunicodeFont);
        _optionsKey.Align = Types.TextAlign.Centre;
        _optionsKey.FontSize = 12;
        _optionsKey.Text = "[Input:" + InputMapType.MENU_OPTIONS + "]";
        _optionsKey.Position = new Vector2(_optionsIcon.X, _optionsIcon.Bounds.Top - _optionsKey.Height - 10);
        _optionsKey.ForceDraw = true;

        base.LoadContent();
    }

    public override void OnEnter()
    {
        _classDebugCounter = 0;
        UpdatePauseScreenIcons();

        _inputDelay = 0.5f;

        if (SoundManager.IsMusicPlaying)
        {
            SoundManager.PauseMusic();
        }

        SoundManager.PlaySound("Pause_Toggle");

        var level = (ScreenManager as RCScreenManager).GetLevelScreen();

        foreach (var infoObj in _infoObjList)
        {
            infoObj.Reset();
            infoObj.Visible = false;
        }

        var player = (ScreenManager as RCScreenManager).Player;
        var playerInfo = _infoObjList[0];
        playerInfo.Visible = true;

        playerInfo.AddItem("LOC_ID_PAUSE_SCREEN_1", ClassType.ToStringID(Game.PlayerStats.Class, Game.PlayerStats.IsFemale), true);
        playerInfo.AddItem("LOC_ID_PAUSE_SCREEN_2", player.Damage.ToString());
        playerInfo.AddItem("LOC_ID_PAUSE_SCREEN_3", player.TotalMagicDamage.ToString());
        playerInfo.AddItem("LOC_ID_PAUSE_SCREEN_4", player.TotalArmor.ToString());
        playerInfo.ResizePlate();

        playerInfo.X = player.X - Camera.TopLeftCorner.X;
        playerInfo.Y = player.Bounds.Bottom - Camera.TopLeftCorner.Y + playerInfo.Height / 2f - 20;

        if (Game.PlayerStats.TutorialComplete == false)
        {
            playerInfo.SetName("LOC_ID_PAUSE_SCREEN_8", true); // ????? (no name yet)
        }
        else
        {
            playerInfo.SetName(Game.NameHelper());
        }

        playerInfo.SetNamePosition(new Vector2(playerInfo.X, player.Bounds.Top - Camera.TopLeftCorner.Y - 40));

        playerInfo.Visible = player.Visible;

        // Adding more pause info objs to the screen if the current room has more enemies than the previous one.
        var infoObjListCount = _infoObjList.Count - 1;
        //for (int i = infoObjListCount; i < level.CurrentRoom.EnemyList.Count; i++)
        for (var i = infoObjListCount; i < level.CurrentRoom.EnemyList.Count + level.CurrentRoom.TempEnemyList.Count; i++)
        {
            _infoObjList.Add(new PauseInfoObj { Visible = false });
        }

        for (var i = 1; i < level.CurrentRoom.EnemyList.Count + 1; i++) // +1 because the first infoObjList object is the player's data.
        {
            var enemy = level.CurrentRoom.EnemyList[i - 1];

            if (enemy.NonKillable == false && enemy.IsKilled == false && enemy.Visible)
            {
                var enemyInfo = _infoObjList[i];
                enemyInfo.Visible = true;
                //enemyInfo.AddItem("Name: ", enemy.Name);
                if (LevelEV.CreateRetailVersion == false)
                {
                    enemyInfo.AddItem("LOC_ID_PAUSE_SCREEN_5", enemy.Level.ToString());
                }
                else
                {
                    enemyInfo.AddItem("LOC_ID_PAUSE_SCREEN_5", ((int) (enemy.Level * LevelEV.ENEMY_LEVEL_FAKE_MULTIPLIER)).ToString());
                }

                enemyInfo.AddItem("LOC_ID_PAUSE_SCREEN_2", enemy.Damage.ToString());
                enemyInfo.AddItem("LOC_ID_PAUSE_SCREEN_6", enemy.CurrentHealth + "/" + enemy.MaxHealth);
                enemyInfo.ResizePlate();

                enemyInfo.X = enemy.X - Camera.TopLeftCorner.X;
                enemyInfo.Y = enemy.Bounds.Bottom - Camera.TopLeftCorner.Y + enemyInfo.Height / 2f - 20;

                enemyInfo.SetName(enemy.LocStringID, true);
                enemyInfo.SetNamePosition(new Vector2(enemyInfo.X, enemy.Bounds.Top - Camera.TopLeftCorner.Y - 40));
            }
        }

        var tempEnemyIndex = level.CurrentRoom.EnemyList.Count;
        for (var i = 0; i < level.CurrentRoom.TempEnemyList.Count; i++)
        {
            var enemy = level.CurrentRoom.TempEnemyList[i];

            if (enemy.NonKillable == false && enemy.IsKilled == false)
            {
                var enemyInfo = _infoObjList[i + 1 + tempEnemyIndex];
                enemyInfo.Visible = true;
                //enemyInfo.AddItem("Name: ", enemy.Name);
                if (LevelEV.CreateRetailVersion == false)
                {
                    enemyInfo.AddItem("LOC_ID_PAUSE_SCREEN_5", enemy.Level.ToString());
                }
                else
                {
                    enemyInfo.AddItem("LOC_ID_PAUSE_SCREEN_5", ((int) (enemy.Level * LevelEV.ENEMY_LEVEL_FAKE_MULTIPLIER)).ToString());
                }

                enemyInfo.AddItem("LOC_ID_PAUSE_SCREEN_2", enemy.Damage.ToString());
                enemyInfo.AddItem("LOC_ID_PAUSE_SCREEN_6", enemy.CurrentHealth + "/" + enemy.MaxHealth);
                enemyInfo.ResizePlate();

                enemyInfo.X = enemy.X - Camera.TopLeftCorner.X;
                enemyInfo.Y = enemy.Bounds.Bottom - Camera.TopLeftCorner.Y + enemyInfo.Height / 2f - 20;

                enemyInfo.SetName(enemy.LocStringID, true);
                enemyInfo.SetNamePosition(new Vector2(enemyInfo.X, enemy.Bounds.Top - Camera.TopLeftCorner.Y - 40));
            }
        }

        Game.ChangeBitmapLanguage(_titleText, "GamePausedTitleText_Sprite");
        base.OnEnter();
    }

    public void UpdatePauseScreenIcons()
    {
        _profileCardKey.Text = "[Input:" + InputMapType.MENU_PROFILECARD + "]";
        _optionsKey.Text = "[Input:" + InputMapType.MENU_OPTIONS + "]";
    }

    public override void OnExit()
    {
        if (SoundManager.IsMusicPaused)
        {
            SoundManager.ResumeMusic();
        }

        SoundManager.PlaySound("Resume_Toggle");

        foreach (var obj in _infoObjList)
        {
            obj.Visible = false;
        }

        base.OnExit();
    }

    public override void HandleInput()
    {
        if (_inputDelay <= 0)
        {
            if (Game.GlobalInput.JustPressed(InputMapType.MENU_PROFILECARD) && Game.PlayerStats.TutorialComplete) // this needs to be unified.
            {
                (ScreenManager as RCScreenManager).DisplayScreen(ScreenType.ProfileCard, true);
            }

            if (Game.GlobalInput.JustPressed(InputMapType.MENU_OPTIONS))
            {
                var optionsData = new List<object>();
                optionsData.Add(false);
                (ScreenManager as RCScreenManager).DisplayScreen(ScreenType.Options, false, optionsData);
            }

            if (Game.GlobalInput.JustPressed(InputMapType.MENU_PAUSE))
            {
                (ScreenManager as RCScreenManager).GetLevelScreen().UnpauseScreen();
                (ScreenManager as RCScreenManager).HideCurrentScreen();
            }

            if (LevelEV.EnableDebugInput)
            {
                HandleDebugInput();
            }

            base.HandleInput();
        }
    }

    private void HandleDebugInput()
    {
        var currentDebugClass = (sbyte) (Game.PlayerStats.Class + _classDebugCounter);
        var previousDebugClass = currentDebugClass;

        if (InputManager.JustPressed(Keys.OemOpenBrackets, PlayerIndex.One))
        {
            if (currentDebugClass == ClassType.Knight)
            {
                _classDebugCounter = (sbyte) (ClassType.Traitor - Game.PlayerStats.Class);
            }
            else
            {
                _classDebugCounter--;
            }

            currentDebugClass = (sbyte) (Game.PlayerStats.Class + _classDebugCounter);
        }
        else if (InputManager.JustPressed(Keys.OemCloseBrackets, PlayerIndex.One))
        {
            if (currentDebugClass == ClassType.Traitor)
            {
                _classDebugCounter = (sbyte) -Game.PlayerStats.Class;
            }
            else
            {
                _classDebugCounter++;
            }

            currentDebugClass = (sbyte) (Game.PlayerStats.Class + _classDebugCounter);
        }

        if (currentDebugClass != previousDebugClass)
        {
            var player = (ScreenManager as RCScreenManager).Player;
            var playerInfo = _infoObjList[0];
            playerInfo.Visible = true;
            (playerInfo.GetChildAt(2) as TextObj).Text = LocaleBuilder.getString(ClassType.ToStringID((byte) currentDebugClass, Game.PlayerStats.IsFemale), playerInfo.GetChildAt(2) as TextObj);
            playerInfo.ResizePlate();

            playerInfo.X = player.X - Camera.TopLeftCorner.X;
            playerInfo.Y = player.Bounds.Bottom - Camera.TopLeftCorner.Y + playerInfo.Height / 2f - 20;
            //playerInfo.SetNamePosition(new Vector2(playerInfo.X, player.Bounds.Top - Camera.TopLeftCorner.Y - 40));
        }
    }

    public override void Update(GameTime gameTime)
    {
        if (_inputDelay > 0)
        {
            _inputDelay -= (float) gameTime.ElapsedGameTime.TotalSeconds;
        }

        base.Update(gameTime);
    }

    public override void Draw(GameTime gameTime)
    {
        Camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, null, null); // Anything that is affected by the godray should be drawn here.
        _titleText.Draw(Camera);

        if (LevelEV.ShowAPDebugText)
        {
            _apDebugText.Text = (Game.ScreenManager.Game as Game)!.ArchipelagoManager.SlotData.ToString();
            _apDebugText.Draw(Camera);
        }

        foreach (var infoObj in _infoObjList)
        {
            infoObj.Draw(Camera);
        }

        if (Game.PlayerStats.TutorialComplete)
        {
            _profileCardKey.Draw(Camera);
        }

        _optionsKey.Draw(Camera);
        Camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        _optionsIcon.Draw(Camera);

        if (Game.PlayerStats.TutorialComplete)
        {
            _profileCard.Draw(Camera);
        }

        Camera.End();
        base.Draw(gameTime);
    }

    public override void RefreshTextObjs()
    {
        Game.ChangeBitmapLanguage(_titleText, "GamePausedTitleText_Sprite");

        var playerInfo = _infoObjList[0];
        if (Game.PlayerStats.TutorialComplete == false)
        {
            playerInfo.SetName("LOC_ID_PAUSE_SCREEN_8", true); // ????? (no name yet)
        }
        else
        {
            playerInfo.SetName(Game.NameHelper());
        }

        foreach (var infoObj in _infoObjList)
        {
            if (infoObj.Visible)
            {
                infoObj.ResizePlate();
            }
        }

        base.RefreshTextObjs();
    }

    public override void Dispose()
    {
        if (IsDisposed == false)
        {
            Console.WriteLine("Disposing Pause Screen");

            foreach (var obj in _infoObjList)
            {
                obj.Dispose();
            }

            _infoObjList.Clear();
            _infoObjList = null;

            _titleText.Dispose();
            _titleText = null;

            _apDebugText.Dispose();
            _apDebugText = null;

            _profileCard.Dispose();
            _profileCard = null;
            _optionsIcon.Dispose();
            _optionsIcon = null;

            _profileCardKey.Dispose();
            _profileCardKey = null;
            _optionsKey.Dispose();
            _optionsKey = null;
            base.Dispose();
        }
    }

    private class PauseInfoObj : ObjContainer
    {
        private int     _arrayIndex;
        private TextObj _name;

        private ObjContainer  _namePlate;
        private List<TextObj> _textDataList; // The data for the text
        private List<TextObj> _textList; // The title for the text

        public PauseInfoObj()
            : base("GameOverStatPlate_Character")
        {
            ForceDraw = true;
            _textList = new List<TextObj>();
            _textDataList = new List<TextObj>();

            _namePlate = new ObjContainer("DialogBox_Character");
            _namePlate.ForceDraw = true;

            _name = new TextObj(Game.JunicodeFont);
            _name.Align = Types.TextAlign.Centre;
            _name.Text = "<noname>";
            _name.FontSize = 8;
            _name.Y -= 45;
            _name.OverrideParentScale = true;
            _name.DropShadow = new Vector2(2, 2);
            _namePlate.AddChild(_name);
        }

        public void SetName(string name, bool isLocalized = false)
        {
            if (isLocalized)
            {
                try
                {
                    _name.ChangeFontNoDefault(LocaleBuilder.GetLanguageFont(_name));
                    _name.Text = LocaleBuilder.getString(name, _name);
                    if (LocaleBuilder.languageType != LanguageType.Chinese_Simp && Regex.IsMatch(_name.Text, @"\p{IsCyrillic}"))
                    {
                        _name.ChangeFontNoDefault(Game.RobotoSlabFont);
                    }
                }
                catch
                {
                    _name.ChangeFontNoDefault(Game.NotoSansSCFont);
                    _name.Text = LocaleBuilder.getString(name, _name);
                }
            }
            else
            {
                try
                {
                    _name.ChangeFontNoDefault(LocaleBuilder.GetLanguageFont(_name));
                    _name.Text = name;
                    if (LocaleBuilder.languageType != LanguageType.Chinese_Simp && Regex.IsMatch(_name.Text, @"\p{IsCyrillic}"))
                    {
                        _name.ChangeFontNoDefault(Game.RobotoSlabFont);
                    }
                }
                catch
                {
                    _name.ChangeFontNoDefault(Game.NotoSansSCFont);
                    _name.Text = name;
                }
            }

            _namePlate.Scale = Vector2.One;
            _namePlate.Scale = new Vector2((_name.Width + 70f) / _namePlate.Width, (_name.Height + 20f) / _namePlate.Height);
        }

        public void SetNamePosition(Vector2 pos)
        {
            _namePlate.Position = pos;
        }

        public void AddItem(string title, string data, bool localizedData = false)
        {
            TextObj titleText;
            if (_textList.Count <= _arrayIndex)
            {
                titleText = new TextObj(Game.JunicodeFont);
            }
            else
            {
                titleText = _textList[_arrayIndex];
            }

            titleText.FontSize = 8;
            titleText.Text = LocaleBuilder.getString(title, titleText);
            titleText.Align = Types.TextAlign.Right;
            titleText.Y = _objectList[0].Bounds.Top + titleText.Height + _arrayIndex * 20;
            titleText.DropShadow = new Vector2(2, 2);
            if (_textList.Count <= _arrayIndex)
            {
                AddChild(titleText);
                _textList.Add(titleText);
            }

            TextObj dataText;
            if (_textDataList.Count <= _arrayIndex)
            {
                dataText = new TextObj(Game.JunicodeFont);
            }
            else
            {
                dataText = _textDataList[_arrayIndex];
            }

            dataText.FontSize = 8;
            if (localizedData)
            {
                dataText.Text = LocaleBuilder.getString(data, dataText);
            }
            else
            {
                dataText.Text = data;
            }

            dataText.Y = titleText.Y;
            dataText.DropShadow = new Vector2(2, 2);
            if (_textDataList.Count <= _arrayIndex)
            {
                AddChild(dataText);
                _textDataList.Add(dataText);
            }

            _arrayIndex++;
        }

        // Should be called once all items have been added.
        public void ResizePlate()
        {
            _objectList[0].ScaleY = 1;
            _objectList[0].ScaleY = _objectList[1].Height * (_objectList.Count + 1) / 2 / (float) _objectList[0].Height;

            var longestTitle = 0;
            foreach (var obj in _textList)
            {
                if (obj.Width > longestTitle)
                {
                    longestTitle = obj.Width;
                }
            }

            var longestData = 0;
            foreach (var obj in _textDataList)
            {
                if (obj.Width > longestData)
                {
                    longestData = obj.Width;
                }
            }

            _objectList[0].ScaleX = 1;
            _objectList[0].ScaleX = (longestTitle + longestData + 50) / (float) _objectList[0].Width;

            var newTitleXPos = (int) (-(_objectList[0].Width / 2f) + longestTitle) + 25;
            var newTitleYPos = _objectList[0].Height / (_textList.Count + 2);

            for (var i = 0; i < _textList.Count; i++)
            {
                _textList[i].X = newTitleXPos;
                _textList[i].Y = _objectList[0].Bounds.Top + newTitleYPos + newTitleYPos * i;

                _textDataList[i].X = newTitleXPos;
                _textDataList[i].Y = _textList[i].Y;
            }
        }

        public override void Draw(Camera2D camera)
        {
            if (Visible)
            {
                _namePlate.Draw(camera);
                _name.Draw(camera);
            }

            base.Draw(camera);
        }

        public void Reset()
        {
            foreach (var obj in _textList)
            {
                obj.Text = "";
            }

            foreach (var obj in _textDataList)
            {
                obj.Text = "";
            }

            _arrayIndex = 0;
        }

        public override void Dispose()
        {
            if (IsDisposed == false)
            {
                _textList.Clear();
                _textList = null;

                _textDataList.Clear();
                _textDataList = null;

                _namePlate.Dispose();
                _namePlate = null;
                _name = null;
                base.Dispose();
            }
        }
    }
}
