using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.EVs;
using RogueCastle.GameObjects.OptionsObjs;
using Tweener;

namespace RogueCastle.Screens;

public class RandomizerScreen : Screen
{
    private bool                      _transitioning;
    private bool                      _lockControls;
    private ObjContainer              _container;
    private TextObj                   _title;
    private TextInputOptionsObj       _hostname;
    private TextInputOptionsObj       _slotname;
    private TextInputOptionsObj       _password;
    private List<TextInputOptionsObj> _options = [];
    private int                       _selectedIndex;

    public RandomizerScreen()
    {
        UpdateIfCovered = true;
        DrawIfCovered = true;
    }

    public bool                LockedControls    => _lockControls || _transitioning;
    public TextInputOptionsObj SelectedOption    => _options[_selectedIndex];
    public float               BackBufferOpacity { get; set; }

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
            Text = "Connect to Archipelago",
            FontSize = 20f,
            Align = Types.TextAlign.Centre,
            Position = new Vector2(GlobalEV.SCREEN_WIDTH / 2, 24f),
        };

        _slotname = new TextInputOptionsObj("Slot Name:", "Sir Phar");
        _hostname = new TextInputOptionsObj("Room Hostname:", "wss://archipelago.gg:38281");
        _password = new TextInputOptionsObj("Room Password:", hidden: true);
        _options.Add(_slotname);
        _options.Add(_hostname);
        _options.Add(_password);
        
        _container.AddChild(_title);
        for (var i = 0; i < _options.Count; i++)
        {
            _options[i].X = 624f;
            _options[i].Y = (i + 1) * 36 + 80;
            
            _container.AddChild(_options[i]);
        }
        
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
}
