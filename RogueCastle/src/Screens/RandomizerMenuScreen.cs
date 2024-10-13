using System;
using System.Collections.Generic;
using System.Globalization;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using RogueCastle.GameObjects.OptionsObjs;
using Tweener;
using Tweener.Ease;

namespace RogueCastle.Screens;

public class RandomizerMenuScreen : Screen
{
    private ObjContainer               _bgSprite;
    private KeyIconTextObj             _cancelText;
    private KeyIconTextObj             _confirmText;
    private SpriteObj                  _downArrow;
    private TextBoxOptionsObj          _hostname;
    private List<RandomizerOptionsObj> _multiRandomizerOptions;
    private KeyIconTextObj             _navigationText;
    private TextBoxOptionsObj          _password;
    private SpriteObj                  _randomizerBar;
    private SpriteObj                  _randomizerTitle;
    private RandomizerOptionsObj       _selectedOption;
    private int                        _selectedOptionIndex;
    private TextBoxOptionsObj          _slot;
    private bool                       _transitioning;
    private SpriteObj                  _upArrow;

    public RandomizerMenuScreen()
    {
        _multiRandomizerOptions = [];
        UpdateIfCovered = true;
        DrawIfCovered = true;
    }

    public float BackBufferOpacity { get; set; }
    public bool  LockControls      { get; set; }

    public override void LoadContent()
    {
        // Background Image
        _bgSprite = new ObjContainer("SkillUnlockPlate_Character")
        {
            ForceDraw = true,
        };

        // Randomizer Menu Title
        _randomizerTitle = new SpriteObj("OptionsScreenTitle_Sprite");
        _bgSprite.AddChild(_randomizerTitle);
        _randomizerTitle.Position = new Vector2(0f, -(float) _bgSprite.Width / 2f + 60f);

        // Arrows
        _upArrow = new SpriteObj("ContinueTextIcon_Sprite");
        _downArrow = new SpriteObj("ContinueTextIcon_Sprite");
        _bgSprite.AddChild(_upArrow);
        _bgSprite.AddChild(_downArrow);
        _upArrow.Visible = false;

        _upArrow.Position = new Vector2(0 - _upArrow.Width / 2, -150);
        _upArrow.Rotation = 180;
        _downArrow.Position = new Vector2(0 - _downArrow.Width / 2, 190);

        // MultiWorld Randomizer Options
        _hostname = new TextBoxOptionsObj(this, "Hostname", "ws://localhost:38281");
        _slot = new TextBoxOptionsObj(this, "Slot Name", "Phar");
        _password = new TextBoxOptionsObj(this, "Password", "");

        _multiRandomizerOptions.Add(_hostname);
        _multiRandomizerOptions.Add(_slot);
        _multiRandomizerOptions.Add(_password);
        // _multiRandomizerOptions.Add(new ConnectArchipelagoOptionObj(this));
        // _multiRandomizerOptions.Add(new BackToMenuArchipelagoObj(this));

        for (var i = 0; i < _multiRandomizerOptions.Count; i++)
        {
            _multiRandomizerOptions[i].X = 420f;
            _multiRandomizerOptions[i].Y = 180 + (i + 1) * 30;
        }

        // Scrollbar
        _randomizerBar = new SpriteObj("OptionsBar_Sprite")
        {
            ForceDraw = true,
            Position = new Vector2(_multiRandomizerOptions[0].X - 20f, _multiRandomizerOptions[0].Y),
        };

        // Menu Help-text
        _confirmText = new KeyIconTextObj(Game.JunicodeFont)
        {
            Text = "to select option",
            DropShadow = new Vector2(2f, 2f),
            FontSize = 12f,
            Align = Types.TextAlign.Right,
            Position = new Vector2(1290f, 570f),
            ForceDraw = true,
        };
        _cancelText = new KeyIconTextObj(Game.JunicodeFont)
        {
            Text = "to exit options",
            Align = Types.TextAlign.Right,
            DropShadow = new Vector2(2f, 2f),
            FontSize = 12f,
            Position = new Vector2(_confirmText.X, _confirmText.Y + 40f),
            ForceDraw = true,
        };
        _navigationText = new KeyIconTextObj(Game.JunicodeFont)
        {
            Text = "to navigate options",
            Align = Types.TextAlign.Right,
            DropShadow = new Vector2(2f, 2f),
            FontSize = 12f,
            Position = new Vector2(_confirmText.X, _confirmText.Y + 80f),
            ForceDraw = true,
        };

        base.LoadContent();
    }

    // public void Connect()
    // {
    //     LockControls = true;
    //
    //     // Game.GameConfig.APServer = _hostname.GetValue;
    //     // Game.GameConfig.APSlot = _slot.GetValue;
    //
    //     Program.Game.SaveConfig();
    //
    //     // Parse port and connect.
    //     var manager = new ArchipelagoManager(new()
    //     {
    //         Url = _hostname.GetValue,
    //         SlotName = _slot.GetValue,
    //         Password = _password.GetValue,
    //     });
    //
    //     Program.Game.ArchipelagoManager = manager;
    //     var result = manager.TryConnect().Result;
    //     if (result != null)
    //     {
    //         // TODO: Make this into a standardized message handler?
    //         var screenManager = Game.ScreenManager;
    //         var errorUuid = Guid.NewGuid().ToString();
    //         var errors = result.Errors;
    //         var speakers = Enumerable.Repeat("Error Connecting to Archipelago", errors.Length).ToArray();
    //
    //         DialogueManager.AddText(errorUuid, speakers, errors);
    //         screenManager.DialogueScreen.SetDialogue(errorUuid);
    //         screenManager.DisplayScreen(ScreenType.Dialogue, true);
    //         LockControls = false;
    //     }
    // }

    public override void OnEnter()
    {
        // Show correct icons based on input device.
        if (InputManager.GamePadIsConnected(PlayerIndex.One))
        {
            _confirmText.ForcedScale = new Vector2(0.7f, 0.7f);
            _cancelText.ForcedScale = new Vector2(0.7f, 0.7f);
            _navigationText.Text = "[Button:LeftStick] to navigate options";
        }
        else
        {
            _confirmText.ForcedScale = new Vector2(1f, 1f);
            _cancelText.ForcedScale = new Vector2(1f, 1f);
            _navigationText.Text = "Arrow keys to navigate options";
        }

        _confirmText.Text = "[Input:" + 0 + "] to select option";
        _cancelText.Text = "[Input:" + 2 + "] to exit options";
        _confirmText.Opacity = 0f;
        _cancelText.Opacity = 0f;
        _navigationText.Opacity = 0f;
        Tween.To(_confirmText, 0.2f, Tween.EaseNone, "Opacity", "1");
        Tween.To(_cancelText, 0.2f, Tween.EaseNone, "Opacity", "1");
        Tween.To(_navigationText, 0.2f, Tween.EaseNone, "Opacity", "1");
        Tween.RunFunction(0.1f, typeof(SoundManager), "PlaySound", "DialogueMenuOpen");
        _transitioning = true;
        Tween.To(this, 0.2f, Tween.EaseNone, "BackBufferOpacity", "0.8");
        _selectedOptionIndex = 0;
        _selectedOption = _multiRandomizerOptions[_selectedOptionIndex];
        _selectedOption.IsActive = false;
        _bgSprite.Position = new Vector2(660f, 0f);
        _bgSprite.Opacity = 0f;
        Tween.To(_bgSprite, 0.5f, Quad.EaseOut, "Y", 360f.ToString(CultureInfo.InvariantCulture));
        Tween.AddEndHandlerToLastTween(this, "EndTransition");
        Tween.To(_bgSprite, 0.2f, Tween.EaseNone, "Opacity", "1");
        var num = 0;
        foreach (var current in _multiRandomizerOptions)
        {
            current.Y = 180 + num * 30 - 360f;

            if (num < 12)
            {
                current.Visible = true;
            }
            else
            {
                current.Visible = false;
            }

            current.Opacity = 0f;
            Tween.By(current, 0.5f, Quad.EaseOut, "Y", 360f.ToString(CultureInfo.InvariantCulture));
            Tween.To(current, 0.2f, Tween.EaseNone, "Opacity", "1");
            current.Initialize();
            num++;
        }

        _randomizerBar.Opacity = 0f;
        Tween.To(_randomizerBar, 0.2f, Tween.EaseNone, "Opacity", "1");
        base.OnEnter();
    }

    public void EndTransition()
    {
        _transitioning = false;
    }

    public void ExitTransition()
    {
        SoundManager.PlaySound("DialogMenuClose");
        _transitioning = true;
        Tween.To(_confirmText, 0.2f, Tween.EaseNone, "Opacity", "0");
        Tween.To(_cancelText, 0.2f, Tween.EaseNone, "Opacity", "0");
        Tween.To(_navigationText, 0.2f, Tween.EaseNone, "Opacity", "0");
        Tween.To(this, 0.2f, Tween.EaseNone, "BackBufferOpacity", "0");
        Tween.To(_randomizerBar, 0.2f, Tween.EaseNone, "Opacity", "0");
        _bgSprite.Position = new Vector2(660f, 360f);
        _bgSprite.Opacity = 1f;
        Tween.To(_bgSprite, 0.5f, Quad.EaseOut, "Y", "0");
        Tween.To(_bgSprite, 0.2f, Tween.EaseNone, "Opacity", "0");
        foreach (var current in _multiRandomizerOptions)
        {
            current.Opacity = 1f;
            Tween.By(current, 0.5f, Quad.EaseOut, "Y", (-360f).ToString(CultureInfo.InvariantCulture));
            Tween.To(current, 0.2f, Tween.EaseNone, "Opacity", "0");
        }

        Tween.AddEndHandlerToLastTween(ScreenManager, "HideCurrentScreen");
    }

    public override void OnExit()
    {
        _selectedOption.IsActive = false;
        _selectedOption.IsSelected = false;
        _selectedOption = null;
        (ScreenManager.Game as Game)!.SaveConfig();
        (ScreenManager as RCScreenManager)!.UpdatePauseScreenIcons();
        base.OnExit();
    }

    public override void HandleInput()
    {
        if (!_transitioning)
        {
            if (_selectedOption.IsActive)
            {
                _selectedOption.HandleInput();
                return;
            }

            var selectedOptionIndex = _selectedOptionIndex;
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
                if (_selectedOptionIndex < _multiRandomizerOptions.Count - 1)
                {
                    SoundManager.PlaySound("frame_swap");
                }

                _selectedOptionIndex++;
            }

            if (_selectedOptionIndex < 0)
            {
                _selectedOptionIndex = _multiRandomizerOptions.Count - 1;
            }

            if (_selectedOptionIndex > _multiRandomizerOptions.Count - 1)
            {
                _selectedOptionIndex = 0;
            }

            if (selectedOptionIndex != _selectedOptionIndex)
            {
                if (_selectedOption != null)
                {
                    _selectedOption.IsSelected = false;
                }

                _selectedOption = _multiRandomizerOptions[_selectedOptionIndex];
                _selectedOption.IsSelected = true;
            }

            if (Game.GlobalInput.JustPressed(InputMapType.MENU_CONFIRM1) ||
                Game.GlobalInput.JustPressed(InputMapType.MENU_CONFIRM2) ||
                Game.GlobalInput.JustPressed(InputMapType.MENU_CONFIRM3))
            {
                SoundManager.PlaySound("Option_Menu_Select");
                LockControls = true;
                _selectedOption.IsActive = true;
            }

            if (Game.GlobalInput.JustPressed(InputMapType.MENU_CANCEL1) ||
                Game.GlobalInput.JustPressed(InputMapType.MENU_CANCEL2) ||
                Game.GlobalInput.JustPressed(InputMapType.MENU_CANCEL3))
            {
                ExitTransition();
            }
        }

        // Hide arrows if on Archipelago settings.
        // TODO: Add additional randomizer options.
        if (true)
        {
            _upArrow.Visible = false;
            _downArrow.Visible = false;
        }
        else
        {
            if (_selectedOptionIndex <= 6)
            {
                _upArrow.Visible = false;
                _downArrow.Visible = true;
            }
            else if (_selectedOptionIndex >= _multiRandomizerOptions.Count - 6)
            {
                _upArrow.Visible = true;
                _downArrow.Visible = false;
            }
            else
            {
                _upArrow.Visible = true;
                _downArrow.Visible = true;
            }
        }

        base.HandleInput();
    }

    public override void Update(GameTime gameTime)
    {
        foreach (var current in _multiRandomizerOptions)
        {
            current.Update(gameTime);
        }

        _randomizerBar.Position = new Vector2(_selectedOption.X - 15f, _selectedOption.Y);
        base.Update(gameTime);
    }

    public override void Draw(GameTime gametime)
    {
        Camera.Begin();
        Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320, 720), Color.Black * BackBufferOpacity);
        _bgSprite.Draw(Camera);
        foreach (var current in _multiRandomizerOptions)
        {
            current.Draw(Camera);
            if (current is TextBoxOptionsObj text)
            {
                Console.WriteLine($@"[{current.Name}]: {current.Width} ({text.GetValue})");
            }
        }

        _confirmText.Draw(Camera);
        _cancelText.Draw(Camera);
        _navigationText.Draw(Camera);
        _randomizerBar.Draw(Camera);
        Camera.End();
        base.Draw(gametime);
    }

    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        Console.WriteLine("Disposing Randomizer Screen");
        foreach (var current in _multiRandomizerOptions)
        {
            current.Dispose();
        }

        _multiRandomizerOptions.Clear();
        _multiRandomizerOptions = null;
        _bgSprite.Dispose();
        _bgSprite = null;
        _randomizerTitle = null;
        _confirmText.Dispose();
        _confirmText = null;
        _cancelText.Dispose();
        _cancelText = null;
        _navigationText.Dispose();
        _navigationText = null;
        _randomizerBar.Dispose();
        _randomizerBar = null;
        _selectedOption = null;
        base.Dispose();
    }
}
