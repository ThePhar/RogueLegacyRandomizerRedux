using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using RogueCastle.EnvironmentVariables;
using RogueCastle.GameObjects.OptionsObjs;
using RogueCastle.GameStructs;
using RogueCastle.Managers;
using RogueCastle.Screens.BaseScreens;
using Tweener;
using Tweener.Ease;

namespace RogueCastle.Screens;

public class RandomizerMenuScreen : Screen
{
    private ObjContainer _bgSprite;
    private List<OptionsObj> _optionsArray = [];
    private OptionsObj _selectedOption;
    private SpriteObj _optionsBar;
    private int _selectedOptionIndex;
    private bool _transitioning;
    private TextObj _contextText;
    private KeyIconTextObj _cancelText;
    private KeyIconTextObj _confirmText;
    private KeyIconTextObj _navigationText;
    private TextObj _title;

    private TextInputOptionsObj _hostname;
    private TextInputOptionsObj _slotname;
    private TextInputOptionsObj _password;
    private ToggleOptionsObj _showPassword;
    private ConnectOptionsObj _connectOption;

    private Task<bool> _connectionSuccess;
    private int _profile;

    public RandomizerMenuScreen()
    {
        UpdateIfCovered = true;
        DrawIfCovered = true;
    }

    public float BackBufferOpacity { get; set; }

    public override void LoadContent()
    {
        _bgSprite = new ObjContainer("SkillUnlockPlate_Character") { ForceDraw = true };

        _hostname = new TextInputOptionsObj("LOC_ID_RANDOMIZER_OPTION_HOSTNAME", "archipelago.gg:38281");
        _slotname = new TextInputOptionsObj("LOC_ID_RANDOMIZER_OPTION_SLOTNAME", "Phar");
        _password = new TextInputOptionsObj("LOC_ID_RANDOMIZER_OPTION_PASSWORD", "");
        _showPassword = new ToggleOptionsObj("LOC_ID_RANDOMIZER_OPTION_SHOW_PASSWORD");
        _connectOption = new ConnectOptionsObj(this, "LOC_ID_RANDOMIZER_OPTION_CONNECT");

        _optionsArray.Add(_hostname);
        _optionsArray.Add(_password);
        _optionsArray.Add(_slotname);
        _optionsArray.Add(_showPassword);
        _optionsArray.Add(null);
        _optionsArray.Add(_connectOption);

        _optionsBar = new SpriteObj("OptionsBar_Sprite") { ForceDraw = true };

        for (var i = 0; i < _optionsArray.Count; i++)
        {
            if (_optionsArray[i] is null)
            {
                continue;
            }

            if (i < 3 || _optionsArray[i] is ConnectOptionsObj)
            {
                _optionsArray[i].X = 420 + _optionsBar.Width / 2f;
            }
            else
            {
                _optionsArray[i].X = 420 + 15;
            }
        }

        _title = new TextObj(Game.JunicodeFont)
        {
            FontSize = 14,
            Align = Types.TextAlign.Centre,
            Position = new Vector2(0, (-_bgSprite.Width / 2f) + 42),
            OutlineWidth = 2,
            TextureColor = Color.Yellow,
        };
        _title.Text = "LOC_ID_RANDOMIZER_MENU_TITLE".GetString(_title);
        _bgSprite.AddChild(_title);

        _confirmText = new KeyIconTextObj(Game.JunicodeFont)
        {
            DropShadow = new Vector2(2, 2),
            FontSize = 12,
            Align = Types.TextAlign.Right,
            Position = new Vector2(1290, 570),
            ForceDraw = true,
        };
        _confirmText.Text = "LOC_ID_CLASS_NAME_1_MALE".GetString(_confirmText);

        _cancelText = new KeyIconTextObj(Game.JunicodeFont)
        {
            Align = Types.TextAlign.Right,
            DropShadow = new Vector2(2, 2),
            FontSize = 12,
            Position = new Vector2(_confirmText.X, _confirmText.Y + 40),
            ForceDraw = true,
        };
        _cancelText.Text = "LOC_ID_CLASS_NAME_1_MALE".GetString(_cancelText);

        _navigationText = new KeyIconTextObj(Game.JunicodeFont)
        {
            Align = Types.TextAlign.Right,
            DropShadow = new Vector2(2, 2),
            FontSize = 12,
            Position = new Vector2(_confirmText.X, _confirmText.Y + 80),
            ForceDraw = true,
        };
        _navigationText.Text = "LOC_ID_CLASS_NAME_1_MALE".GetString(_navigationText);

        _contextText = new TextObj(Game.JunicodeFont) {
            FontSize = 9,
            Text = "",
            Align = Types.TextAlign.Centre,
            Position = new Vector2(_optionsArray[0].X, 620),
            OutlineWidth = 2,
            ForceDraw = true,
        };

        base.LoadContent();
    }

    public override void OnEnter()
    {
        RefreshTextObjs();

        _contextText.Visible = false;
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

        _transitioning = true;
        Tween.To(this, 0.2f, Tween.EaseNone, "BackBufferOpacity", "0.8");

        _bgSprite.Position = new Vector2(1320 / 2f, 0);
        _bgSprite.Opacity = 0;
        Tween.To(_bgSprite, 0.5f, Quad.EaseOut, "Y", $"{720 / 2f}");
        Tween.AddEndHandlerToLastTween(this, "EndTransition");
        Tween.To(_bgSprite, 0.2f, Tween.EaseNone, "Opacity", "1");

        _selectedOptionIndex = 0;
        _selectedOption = _optionsArray[_selectedOptionIndex];
        _selectedOption.IsActive = false;

        for (var i = 0; i < _optionsArray.Count; i++)
        {
            if (_optionsArray[i] is null)
            {
                continue;
            }

            var obj = _optionsArray[i];
            var offset = Math.Min(3, i) * 50 + i * 30;

            obj.Y = 160 + offset - (720 / 2f);
            obj.Opacity = 0;
            Tween.By(obj, 0.5f, Quad.EaseOut, "Y", $"{720 / 2f}");
            Tween.To(obj, 0.2f, Tween.EaseNone, "Opacity", "1");
            obj.Initialize();
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

        for (var i = 0; i < _optionsArray.Count; i++)
        {
            if (_optionsArray[i] is null)
            {
                continue;
            }

            var obj = _optionsArray[i];
            var offset = Math.Min(3, i) * 50 + i * 30;

            obj.Y = 160 + offset;
            obj.Opacity = 1;
            Tween.By(obj, 0.5f, Quad.EaseOut, "Y", $"{-(720 / 2f)}");
            Tween.To(obj, 0.2f, Tween.EaseNone, "Opacity", "0");
        }

        Tween.AddEndHandlerToLastTween(ScreenManager, "HideCurrentScreen");
    }

    public override void OnExit()
    {
        _selectedOption.IsActive = false;
        _selectedOption.IsSelected = false;
        _selectedOption = null;
        (ScreenManager as RCScreenManager)!.UpdatePauseScreenIcons();

        base.OnExit();
    }

    public override void PassInData(List<object> objList)
    {
        _profile = (int)objList[0];
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var obj in _optionsArray)
        {
            obj?.Update(gameTime);
        }

        // todo probably not the most efficient way to do this, and should be changed later
        _password.Hidden = !_showPassword.Value;

        _optionsBar.Position = _selectedOption switch
        {
            TextInputOptionsObj => new Vector2(_selectedOption.X - _optionsBar.Width / 2f, _selectedOption.Y + 28),
            ConnectOptionsObj   => new Vector2(_selectedOption.X - _optionsBar.Width / 2f, _selectedOption.Y),
            _                   => new Vector2(_selectedOption.X - 15, _selectedOption.Y),
        };

        if (_connectionSuccess is { IsCompleted: true })
        {
            // New Game Time!
            if (_connectionSuccess.Result)
            {
                Game.GameConfig.ProfileSlot = (byte)_profile;

                // Reset stats.
                SkillSystem.ResetAllTraits();
                Game.PlayerStats.Dispose();
                Game.PlayerStats = new PlayerStats();
                Game.ScreenManager.Player.Reset();
                Game.ScreenManager.Player.CurrentHealth = Game.PlayerStats.CurrentHealth;
                Game.ScreenManager.Player.CurrentMana = Game.PlayerStats.CurrentMana;

                // Save data.
                (ScreenManager.Game as Game)!.SaveManager.SaveFiles(SaveType.Archipelago, SaveType.PlayerData, SaveType.UpgradeData);

                // Return to title with new save.
                Game.ScreenManager.DisplayScreen(ScreenType.TITLE, true);
                _connectOption.IsActive = false;
            }

            _contextText.Visible = false;
            _connectionSuccess = null;
        }

        base.Update(gameTime);
    }

    public void StartConnect()
    {
        _connectionSuccess = (ScreenManager.Game as Game)!.ArchipelagoManager.ConnectAsync(
            _hostname.GetValue.Trim(),
            _slotname.GetValue.Trim(),
            _password.GetValue);

        _contextText.Visible = true;
        _contextText.Text = "Attempting to connect to server...";
        _connectOption.IsActive = false;
    }

    public override void Draw(GameTime gametime)
    {
        Camera.Begin();
        Camera.Draw(
            Game.GenericTexture,
            new Rectangle(0, 0, GlobalEV.SCREEN_WIDTH, GlobalEV.SCREEN_HEIGHT),
            Color.Black * BackBufferOpacity
        );

        _bgSprite.Draw(Camera);
        foreach (var obj in _optionsArray)
        {
            obj?.Draw(Camera);
        }

        _contextText.Draw(Camera);
        _confirmText.Draw(Camera);
        _cancelText.Draw(Camera);
        _navigationText.Draw(Camera);
        _optionsBar.Draw(Camera);

        Camera.End();
        base.Draw(gametime);
    }

    public override void HandleInput()
    {
        if (_transitioning)
        {
            _contextText.Visible = false;
            base.HandleInput();
            return;
        }

        if (_selectedOption.IsActive)
        {
            _selectedOption.HandleInput();
            return;
        }

        // Input Handling
        var previousSelectedOptionIndex = _selectedOptionIndex;
        if (Game.GlobalInput.PressedUp())
        {
            SoundManager.PlaySound("frame_swap");
            do
            {
                _selectedOptionIndex--;

                if (_selectedOptionIndex < 0)
                {
                    _selectedOptionIndex = _optionsArray.Count - 1;
                }
            } while (_optionsArray[_selectedOptionIndex] == null);
        }
        else if (Game.GlobalInput.PressedDown())
        {
            SoundManager.PlaySound("frame_swap");
            do
            {
                _selectedOptionIndex++;

                if (_selectedOptionIndex >= _optionsArray.Count)
                {
                    _selectedOptionIndex = 0;
                }
            } while (_optionsArray[_selectedOptionIndex] == null);
        }

        if (previousSelectedOptionIndex != _selectedOptionIndex)
        {
            _selectedOption.IsSelected = false;
            _selectedOption = _optionsArray[_selectedOptionIndex];
            _selectedOption.IsSelected = true;
        }

        if (Game.GlobalInput.PressedConfirm())
        {
            SoundManager.PlaySound("Option_Menu_Select");
            _selectedOption.IsActive = true;
        }
        else if (Game.GlobalInput.PressedCancel() || Game.GlobalInput.JustPressed(InputMapType.MENU_OPTIONS))
        {
            ExitTransition();
        }

        base.HandleInput();
    }

    public override void RefreshTextObjs()
    {
        foreach (var obj in _optionsArray)
        {
            obj?.RefreshTextObjs();
        }

        _contextText.ScaleX = LocaleBuilder.LanguageType switch
        {
            LanguageType.Russian or LanguageType.German => 0.9f,
            _                                           => 1,
        };

        base.RefreshTextObjs();
    }

    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        foreach (var option in _optionsArray)
        {
            option?.Dispose();
        }

        _optionsArray.Clear();
        _optionsArray = null;
        _selectedOption?.Dispose();
        _selectedOption = null;
        _bgSprite?.Dispose();
        _bgSprite = null;

        _hostname = null;
        _slotname = null;
        _password = null;

        _contextText?.Dispose();
        _contextText = null;
        _confirmText?.Dispose();
        _confirmText = null;
        _cancelText?.Dispose();
        _cancelText = null;
        _navigationText?.Dispose();
        _navigationText = null;

        base.Dispose();
    }
}
