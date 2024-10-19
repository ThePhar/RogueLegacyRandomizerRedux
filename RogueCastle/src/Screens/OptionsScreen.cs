using System;
using System.Collections.Generic;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using RogueCastle.EnvironmentVariables;
using RogueCastle.GameObjects.OptionsObjs;
using RogueCastle.GameStructs;
using RogueCastle.Screens.BaseScreens;
using Tweener;
using Tweener.Ease;

namespace RogueCastle.Screens;

public class OptionsScreen : Screen
{
    private OptionsObj _backToMenuObj;
    private ObjContainer _bgSprite;
    private KeyIconTextObj _cancelText;
    private SpriteObj _changeControlsTitle;
    private bool _changingControls;
    private KeyIconTextObj _confirmText;
    private OptionsObj _enableSteamCloudObj;
    private KeyIconTextObj _navigationText;
    private List<OptionsObj> _optionsArray = [];
    private SpriteObj _optionsBar;
    private SpriteObj _optionsTitle;
    private OptionsObj _quickDropObj;
    private TextObj _quickDropText;
    private OptionsObj _reduceQualityObj;
    private OptionsObj _selectedOption;
    private int _selectedOptionIndex;
    private bool _titleScreenOptions = true;
    private bool _transitioning;

    public OptionsScreen()
    {
        UpdateIfCovered = true;
        DrawIfCovered = true;
    }

    public float BackBufferOpacity { get; set; }

    public override void LoadContent()
    {
        _bgSprite = new ObjContainer("SkillUnlockPlate_Character");
        _bgSprite.ForceDraw = true;

        _optionsTitle = new SpriteObj("OptionsScreenTitle_Sprite");
        _bgSprite.AddChild(_optionsTitle);
        _optionsTitle.Position = new Vector2(0, (-_bgSprite.Width / 2f) + 60);

        _changeControlsTitle = new SpriteObj("OptionsScreenChangeControls_Sprite");
        _bgSprite.AddChild(_changeControlsTitle);
        _changeControlsTitle.Position = new Vector2(1320, _optionsTitle.Y);

        if (!(Environment.GetEnvironmentVariable("SteamTenfoot") == "1" ||
              Environment.GetEnvironmentVariable("SteamDeck") == "1"))
        {
            _optionsArray.Add(new ResolutionOptionsObj(this));
            _optionsArray.Add(new FullScreenOptionsObj(this));
        }

        _reduceQualityObj = new ReduceQualityOptionsObj(this);
        _optionsArray.Add(_reduceQualityObj);
        _optionsArray.Add(new MusicVolOptionsObj(this));
        _optionsArray.Add(new SFXVolOptionsObj(this));
        _quickDropObj = new QuickDropOptionsObj(this);
        _optionsArray.Add(_quickDropObj);
        _optionsArray.Add(new DeadZoneOptionsObj(this));
        _optionsArray.Add(new ChangeControlsOptionsObj(this));

        _optionsArray.Add(new LanguageOptionsObj(this));

        _optionsArray.Add(new ExitProgramOptionsObj(this));
        _backToMenuObj = new BackToMenuOptionsObj(this);
        _backToMenuObj.X = 420;

        for (var i = 0; i < _optionsArray.Count; i++)
        {
            _optionsArray[i].X = 420;
            _optionsArray[i].Y = 160 + (i * 30);
        }

        _optionsBar = new SpriteObj("OptionsBar_Sprite");
        _optionsBar.ForceDraw = true;
        _optionsBar.Position = new Vector2(_optionsArray[0].X - 20, _optionsArray[0].Y);

        _confirmText = new KeyIconTextObj(Game.JunicodeFont);
        _confirmText.Text =
            "LOC_ID_CLASS_NAME_1_MALE".GetString(_confirmText); // dummy locID to add TextObj to language refresh list
        _confirmText.DropShadow = new Vector2(2, 2);
        _confirmText.FontSize = 12;
        _confirmText.Align = Types.TextAlign.Right;
        _confirmText.Position = new Vector2(1290, 570);
        _confirmText.ForceDraw = true;

        _cancelText = new KeyIconTextObj(Game.JunicodeFont);
        _cancelText.Text =
            "LOC_ID_CLASS_NAME_1_MALE".GetString(_cancelText); // dummy locID to add TextObj to language refresh list
        _cancelText.Align = Types.TextAlign.Right;
        _cancelText.DropShadow = new Vector2(2, 2);
        _cancelText.FontSize = 12;
        _cancelText.Position = new Vector2(_confirmText.X, _confirmText.Y + 40);
        _cancelText.ForceDraw = true;

        _navigationText = new KeyIconTextObj(Game.JunicodeFont);
        _navigationText.Text =
            "LOC_ID_CLASS_NAME_1_MALE"
                .GetString(_navigationText); // dummy locID to add TextObj to language refresh list
        _navigationText.Align = Types.TextAlign.Right;
        _navigationText.DropShadow = new Vector2(2, 2);
        _navigationText.FontSize = 12;
        _navigationText.Position = new Vector2(_confirmText.X, _confirmText.Y + 80);
        _navigationText.ForceDraw = true;

        _quickDropText = new TextObj(Game.JunicodeFont);
        _quickDropText.FontSize = 8;
        _quickDropText.Text =
            "*Quick drop allows you to drop down ledges and down-attack in \nthe air by pressing DOWN";
        _quickDropText.Position = new Vector2(420, 620);
        _quickDropText.ForceDraw = true;
        _quickDropText.DropShadow = new Vector2(2, 2);
        base.LoadContent();
    }

    public override void PassInData(List<object> objList)
    {
        _titleScreenOptions = (bool)objList[0];
        base.PassInData(objList);
    }

    public override void OnEnter()
    {
        RefreshTextObjs();

        _quickDropText.Visible = false;

        if (InputManager.GamePadIsConnected(PlayerIndex.One))
        {
            _confirmText.ForcedScale = new Vector2(0.7f, 0.7f);
            _cancelText.ForcedScale = new Vector2(0.7f, 0.7f);
            _navigationText.Text = "LOC_ID_OPTIONS_SCREEN_2_NEW".GetString(_navigationText);
        }
        else
        {
            _confirmText.ForcedScale = new Vector2(1f, 1f);
            _cancelText.ForcedScale = new Vector2(1f, 1f);
            _navigationText.Text = "LOC_ID_OPTIONS_SCREEN_3".GetString(_navigationText);
        }

        _confirmText.Text = "LOC_ID_OPTIONS_SCREEN_4_NEW".GetString(_confirmText);
        _cancelText.Text = "LOC_ID_OPTIONS_SCREEN_5_NEW".GetString(_cancelText);

        _confirmText.Opacity = 0;
        _cancelText.Opacity = 0;
        _navigationText.Opacity = 0;

        Tween.To(_confirmText, 0.2f, Tween.EaseNone, "Opacity", "1");
        Tween.To(_cancelText, 0.2f, Tween.EaseNone, "Opacity", "1");
        Tween.To(_navigationText, 0.2f, Tween.EaseNone, "Opacity", "1");

        Tween.RunFunction(0.1f, typeof(SoundManager), "PlaySound", "DialogueMenuOpen");

        if (_optionsArray.Contains(_backToMenuObj) == false)
        {
            _optionsArray.Insert(_optionsArray.Count - 1, _backToMenuObj);
        }

        if (_titleScreenOptions)
        {
            _optionsArray.RemoveAt(_optionsArray.Count -
                                    2); // Remove the second last entry because the last entry is "Exit Program"
        }

        _transitioning = true;
        Tween.To(this, 0.2f, Tween.EaseNone, "BackBufferOpacity", "0.8");

        //(_optionsArray[0] as ResolutionOptionsObj).Initialize(); // The resolutionObj needs to be initialized every time.

        _selectedOptionIndex = 0;
        _selectedOption = _optionsArray[_selectedOptionIndex];
        _selectedOption.IsActive = false;

        _bgSprite.Position = new Vector2(1320 / 2f, 0);
        _bgSprite.Opacity = 0;
        Tween.To(_bgSprite, 0.5f, Quad.EaseOut, "Y", (720 / 2f).ToString());
        Tween.AddEndHandlerToLastTween(this, "EndTransition");
        Tween.To(_bgSprite, 0.2f, Tween.EaseNone, "Opacity", "1");

        var counter = 0;
        foreach (var obj in _optionsArray)
        {
            obj.Y = 160 + (counter * 30) - (720 / 2f);
            obj.Opacity = 0;
            Tween.By(obj, 0.5f, Quad.EaseOut, "Y", (720 / 2f).ToString());
            Tween.To(obj, 0.2f, Tween.EaseNone, "Opacity", "1");
            obj.Initialize();
            counter++;
        }

        _optionsBar.Opacity = 0;
        Tween.To(_optionsBar, 0.2f, Tween.EaseNone, "Opacity", "1");

        base.OnEnter();
    }

    public void EndTransition()
    {
        _transitioning = false;
    }

    private void ExitTransition()
    {
        SoundManager.PlaySound("DialogMenuClose");

        _transitioning = true;


        Tween.To(_confirmText, 0.2f, Tween.EaseNone, "Opacity", "0");
        Tween.To(_cancelText, 0.2f, Tween.EaseNone, "Opacity", "0");
        Tween.To(_navigationText, 0.2f, Tween.EaseNone, "Opacity", "0");

        Tween.To(this, 0.2f, Tween.EaseNone, "BackBufferOpacity", "0");
        Tween.To(_optionsBar, 0.2f, Tween.EaseNone, "Opacity", "0");

        _bgSprite.Position = new Vector2(1320 / 2f, 720 / 2f);
        _bgSprite.Opacity = 1;
        Tween.To(_bgSprite, 0.5f, Quad.EaseOut, "Y", "0");
        Tween.To(_bgSprite, 0.2f, Tween.EaseNone, "Opacity", "0");

        var counter = 0;
        foreach (var obj in _optionsArray)
        {
            obj.Y = 160 + (counter * 30);
            obj.Opacity = 1;
            Tween.By(obj, 0.5f, Quad.EaseOut, "Y", (-(720 / 2f)).ToString());
            Tween.To(obj, 0.2f, Tween.EaseNone, "Opacity", "0");
            counter++;
        }

        Tween.AddEndHandlerToLastTween(ScreenManager, "HideCurrentScreen");
    }

    public override void OnExit()
    {
        _selectedOption.IsActive = false;
        _selectedOption.IsSelected = false;
        _selectedOption = null;
        Game.SaveConfig();
        (ScreenManager as RCScreenManager).UpdatePauseScreenIcons();

        base.OnExit();
    }

    public override void HandleInput()
    {
        if (_transitioning == false) // No input until the screen is fully displayed.
        {
            if (_selectedOption.IsActive)
            {
                _selectedOption.HandleInput();
            }
            else
            {
                if (_selectedOption.IsActive == false)
                {
                    var previousSelectedOptionIndex = _selectedOptionIndex;

                    if (Game.GlobalInput.JustPressed(InputMapType.PLAYER_UP1) ||
                        Game.GlobalInput.JustPressed(InputMapType.PLAYER_UP2))
                    {
                        if (_selectedOptionIndex > 0)
                        {
                            SoundManager.PlaySound("frame_swap");
                        }

                        _selectedOptionIndex--;
                    }
                    else if (Game.GlobalInput.JustPressed(InputMapType.PLAYER_DOWN1) ||
                             Game.GlobalInput.JustPressed(InputMapType.PLAYER_DOWN2))
                    {
                        if (_selectedOptionIndex < _optionsArray.Count - 1)
                        {
                            SoundManager.PlaySound("frame_swap");
                        }

                        _selectedOptionIndex++;
                    }

                    if (_selectedOptionIndex < 0)
                    {
                        _selectedOptionIndex = _optionsArray.Count - 1;
                    }

                    if (_selectedOptionIndex > _optionsArray.Count - 1)
                    {
                        _selectedOptionIndex = 0;
                    }

                    if (previousSelectedOptionIndex != _selectedOptionIndex)
                    {
                        if (_selectedOption != null)
                        {
                            _selectedOption.IsSelected = false;
                        }

                        _selectedOption = _optionsArray[_selectedOptionIndex];
                        _selectedOption.IsSelected = true;
                    }
                }

                if (Game.GlobalInput.JustPressed(InputMapType.MENU_CONFIRM1) ||
                    Game.GlobalInput.JustPressed(InputMapType.MENU_CONFIRM2) ||
                    Game.GlobalInput.JustPressed(InputMapType.MENU_CONFIRM3))
                {
                    SoundManager.PlaySound("Option_Menu_Select");
                    _selectedOption.IsActive = true;
                }

                if (Game.GlobalInput.JustPressed(InputMapType.MENU_CANCEL1) ||
                    Game.GlobalInput.JustPressed(InputMapType.MENU_CANCEL2) ||
                    Game.GlobalInput.JustPressed(InputMapType.MENU_OPTIONS)
                    || Game.GlobalInput.JustPressed(InputMapType.MENU_CANCEL3))
                {
                    ExitTransition();
                }
            }


            if (_selectedOption == _quickDropObj)
            {
                _quickDropText.Visible = true;
                _quickDropText.Text = "LOC_ID_OPTIONS_SCREEN_1".GetString(_quickDropText, true);
            }
            else if (_selectedOption == _reduceQualityObj)
            {
                _quickDropText.Visible = true;
                _quickDropText.Text = "LOC_ID_OPTIONS_SCREEN_8".GetString(_quickDropText, true);
            }
            else if (_selectedOption == _enableSteamCloudObj)
            {
                _quickDropText.Visible = true;
                _quickDropText.Text = "LOC_ID_OPTIONS_SCREEN_9".GetString(_quickDropText, true);
            }
            else
            {
                _quickDropText.Visible = false;
            }
        }
        else
        {
            _quickDropText.Visible = false;
        }

        base.HandleInput();
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var obj in _optionsArray)
        {
            obj.Update(gameTime);
        }

        _optionsBar.Position = new Vector2(_selectedOption.X - 15, _selectedOption.Y);

        base.Update(gameTime);
    }

    public void ToggleControlsConfig()
    {
        if (_changingControls == false)
        {
            foreach (var obj in _optionsArray)
            {
                Tween.By(obj, 0.3f, Quad.EaseInOut, "X", "-1320");
            }

            Tween.By(_optionsTitle, 0.3f, Quad.EaseInOut, "X", "-1320");
            Tween.By(_changeControlsTitle, 0.3f, Quad.EaseInOut, "X", "-1320");
            _changingControls = true;
        }
        else
        {
            foreach (var obj in _optionsArray)
            {
                Tween.By(obj, 0.3f, Quad.EaseInOut, "X", "1320");
            }

            Tween.By(_optionsTitle, 0.3f, Quad.EaseInOut, "X", "1320");
            Tween.By(_changeControlsTitle, 0.3f, Quad.EaseInOut, "X", "1320");
            _changingControls = false;
        }
    }

    public override void Draw(GameTime gametime)
    {
        Camera.Begin();
        Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, GlobalEV.SCREEN_WIDTH, GlobalEV.SCREEN_HEIGHT),
            Color.Black * BackBufferOpacity);
        _bgSprite.Draw(Camera);
        foreach (var obj in _optionsArray)
        {
            obj.Draw(Camera);
        }

        _quickDropText.Draw(Camera);

        _confirmText.Draw(Camera);
        _cancelText.Draw(Camera);
        _navigationText.Draw(Camera);
        _optionsBar.Draw(Camera);
        Camera.End();

        base.Draw(gametime);
    }

    public override void Dispose()
    {
        if (IsDisposed == false)
        {
            Console.WriteLine("Disposing Options Screen");

            foreach (var obj in _optionsArray)
            {
                obj.Dispose();
            }

            _optionsArray.Clear();
            _optionsArray = null;
            _bgSprite.Dispose();
            _bgSprite = null;
            _optionsTitle = null;
            _changeControlsTitle = null;
            //_deleteSaveObj = null;
            _backToMenuObj = null;

            _confirmText.Dispose();
            _confirmText = null;
            _cancelText.Dispose();
            _cancelText = null;
            _navigationText.Dispose();
            _navigationText = null;

            _optionsBar.Dispose();
            _optionsBar = null;

            _selectedOption = null;

            _quickDropText.Dispose();
            _quickDropText = null;
            _quickDropObj = null;
            _enableSteamCloudObj = null;
            _reduceQualityObj = null;
            base.Dispose();
        }
    }

    public override void RefreshTextObjs()
    {
        /*
        if (InputManager.GamePadIsConnected(PlayerIndex.One))
            _navigationText.Text = "[Button:LeftStick] " + LocaleBuilder.getResourceString("LOC_ID_OPTIONS_SCREEN_2");
        else
            _navigationText.Text = LocaleBuilder.getResourceString("LOC_ID_OPTIONS_SCREEN_3");
        _confirmText.Text = "[Input:" + InputMapType.MENU_CONFIRM1 + "] " + LocaleBuilder.getResourceString("LOC_ID_OPTIONS_SCREEN_4");
        _cancelText.Text = "[Input:" + InputMapType.MENU_CANCEL1 + "] " + LocaleBuilder.getResourceString("LOC_ID_OPTIONS_SCREEN_5");
         */

        foreach (var obj in _optionsArray)
        {
            obj.RefreshTextObjs();
        }

        Game.ChangeBitmapLanguage(_optionsTitle, "OptionsScreenTitle_Sprite");
        Game.ChangeBitmapLanguage(_changeControlsTitle, "OptionsScreenChangeControls_Sprite");

        _quickDropText.ScaleX = 1;
        switch (LocaleBuilder.LanguageType)
        {
            case LanguageType.Russian:
            case LanguageType.German:
                _quickDropText.ScaleX = 0.9f;
                break;
        }

        base.RefreshTextObjs();
    }
}
