using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.EVs;
using RogueCastle.GameObjects.OptionsObjs;
using RogueCastle.Screens.BaseObjects;
using Tweener;

namespace RogueCastle.Screens;

public class RandomizerScreen : Screen
{
    private bool                       _transitioning;
    private bool                       _lockControls;
    private ObjContainer               _container;
    private TextObj                    _title;
    private TextInputOptionsObj        _hostname;
    private TextInputOptionsObj        _slotname;
    private TextInputOptionsObj        _password;
    private List<RandomizerOptionsObj> _options = [];
    private int                        _selectedIndex;

    public RandomizerScreen()
    {
        UpdateIfCovered = true;
        DrawIfCovered = true;
    }

    public bool                 LockedControls    => _lockControls || _transitioning;
    public RandomizerOptionsObj SelectedOption    => _options[_selectedIndex];
    public float                BackBufferOpacity { get; set; }

    public override void OnEnter()
    {
        SoundManager.PlaySound("DialogOpen");
        _transitioning = true;

        _title.Opacity = 0;

        Tween.To(this, 0.2f, Tween.EaseNone, "BackBufferOpacity", "0.95");
        Tween.To(_title, 0.2f, Tween.EaseNone, "delay", "0.2", "Opacity", "1");
        foreach (var option in _options)
        {
            Tween.To(option, 0.2f, Tween.EaseNone, "delay", "0.2", "Opacity", "1");
        }
        Tween.RunFunction(0.5f, this, "UnlockControls");

        base.OnEnter();
    }

    public override void LoadContent()
    {
        _container = new ObjContainer()
        {
            ForceDraw = true,
        };
        
        _title = new TextObj(Game.JunicodeLargeFont)
        {
            FontSize = 20f,
            Align = Types.TextAlign.Centre,
            Position = new Vector2(GlobalEV.SCREEN_WIDTH / 2, 24f),
        };
        _title.Text = LocaleBuilder.getString("LOC_ID_RANDOMIZER_CONNECT_TITLE", _title);

        _slotname = new TextInputOptionsObj("LOC_ID_RANDOMIZER_SLOT_NAME", "Sir Phar");
        _hostname = new TextInputOptionsObj("LOC_ID_RANDOMIZER_ROOM_HOSTNAME", "archipelago.gg:38281");
        _password = new TextInputOptionsObj("LOC_ID_RANDOMIZER_ROOM_PASSWORD", hidden: true);
        _options.Add(_slotname);
        _options.Add(_hostname);
        _options.Add(_password);
        _options.Add(new ConnectArchipelagoOptionsObj("LOC_ID_RANDOMIZER_CONNECT", this));
        
        _container.AddChild(_title);
        for (var i = 0; i < _options.Count; i++)
        {
            _options[i].X = 624f;
            _options[i].Y = (i + 1) * 36 + 80;
            
            _container.AddChild(_options[i]);
        }
        
        // Mark the first option as selected.
        _options[0].IsSelected = true;
        
        // Move last option.
        _options[_options.Count - 1].X += 24;
        _options[_options.Count - 1].Y += 80 + 36;

        base.LoadContent();
    }
    
    public void UnlockControls()
    {
        _lockControls = false;
        _transitioning = false;
    }

    public override void OnExit()
    {
        UnlockControls();
        base.OnExit();
    }

    public override void HandleInput()
    {
        if (LockedControls)
        {
            base.HandleInput();
            return;
        }

        if (SelectedOption.IsActive)
        {
            SelectedOption.HandleInput();
            return;
        }

        if (InputHelper.PressedUp())
        {
            SoundManager.PlaySound("frame_swap");
            SelectedOption.IsSelected = false;
            _selectedIndex--;
            if (_selectedIndex < 0)
            {
                _selectedIndex = _options.Count - 1;
            }
            
            SelectedOption.IsSelected = true;
        }
        else if (InputHelper.PressedDown())
        {
            SoundManager.PlaySound("frame_swap");
            SelectedOption.IsSelected = false;
            _selectedIndex++;
            if (_selectedIndex >= _options.Count)
            {
                _selectedIndex = 0;
            }
            
            SelectedOption.IsSelected = true;
        }

        if (InputHelper.PressedConfirm())
        {
            SoundManager.PlaySound("Option_Menu_Select");
            SelectedOption.IsActive = true;
        }
        else if (InputHelper.PressedCancel())
        {
            ExitTransition();
        }
        
        base.HandleInput();
    }

    public override void Draw(GameTime gametime)
    {
        Camera.Begin();
        Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, GlobalEV.SCREEN_WIDTH, GlobalEV.SCREEN_HEIGHT), Color.Black * BackBufferOpacity);

        _container.Draw(Camera);
        _title.Draw(Camera);
        foreach (var option in _options)
        {
            option.Draw(Camera);
        }
        
        Camera.End();
        base.Draw(gametime);
    }

    private void ExitTransition()
    {
        SoundManager.PlaySound("DialogMenuClose");

        _transitioning = true;

        Tween.To(this, 0.2f, Tween.EaseNone, "delay", "0.2", "BackBufferOpacity", "0");
        Tween.To(_title, 0.2f, Tween.EaseNone, "Opacity", "0");
        foreach (var option in _options)
        {
            Tween.To(option, 0.2f, Tween.EaseNone, "Opacity", "0");
        }

        Tween.AddEndHandlerToLastTween(ScreenManager, "HideCurrentScreen");
    }
    
    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        foreach (var option in _options)
        {
            option.Dispose();
        }
        
        _container?.Dispose();
        _container = null;
        _title?.Dispose();
        _title = null;
        _hostname?.Dispose();
        _hostname = null;
        _slotname?.Dispose();
        _slotname = null;
        _password?.Dispose();
        _password = null;
        
        base.Dispose();
    }
    
    public void StartGame()
    {
        SoundManager.PlaySound("Game_Start");

        var hostname = _hostname.GetValue;
        var username = _slotname.GetValue;
        var password = _password.GetValue;
        
        var result = (ScreenManager.Game as Game)!.ArchipelagoManager.TryConnect(hostname, username, password);
        if (result is not null)
        {
            throw new Exception(result.ToString());
        }
        
        Game.PlayerStats.CharacterFound = true;
        Game.PlayerStats.Gold = 0;
        
        // Necessary to change his headpiece so he doesn't look like the first dude.
        Game.PlayerStats.HeadPiece = (byte) CDGMath.RandomInt(1, PlayerPart.NumHeadPieces);
        Game.PlayerStats.EnemiesKilledInRun.Clear();
        
        // Create new player, lineage, and upgrade data.
        (ScreenManager.Game as Game)!.SaveManager.SaveFiles(SaveType.PlayerData, SaveType.Lineage, SaveType.UpgradeData);
        (ScreenManager as RCScreenManager)!.DisplayScreen(ScreenType.StartingRoom, true);

        SoundManager.StopMusic(0.2f);
    }
}
