using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.Screens;

namespace RogueCastle.GameObjects.OptionsObjs;

public abstract class RandomizerOptionsObj : ObjContainer
{
    protected const int OPTIONS_TEXT_OFFSET = 250;

    protected bool                 _isActive;
    protected bool                 _isSelected;
    protected TextObj              _nameText;
    protected RandomizerMenuScreen _parentScreen;

    protected RandomizerOptionsObj(RandomizerMenuScreen parentScreen, string name)
    {
        _parentScreen = parentScreen;
        _nameText = new TextObj(Game.JunicodeFont)
        {
            FontSize = 12f,
            Text = name,
            DropShadow = new Vector2(2f, 2f),
        };

        AddChild(_nameText);
        ForceDraw = true;
    }

    public virtual bool IsActive
    {
        get => _isActive;
        set
        {
            IsSelected = !value;

            _isActive = value;
            if (!value)
            {
                (_parentScreen.ScreenManager.Game as Game)!.SaveConfig();
            }
        }
    }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            if (value)
            {
                _nameText.TextureColor = Color.Yellow;
                return;
            }

            _nameText.TextureColor = Color.White;
        }
    }

    public virtual void Initialize() { }

    public virtual void HandleInput()
    {
        if (Game.GlobalInput.JustPressed(InputMapType.MENU_CANCEL1) ||
            Game.GlobalInput.JustPressed(InputMapType.MENU_CANCEL2) ||
            Game.GlobalInput.JustPressed(InputMapType.MENU_CANCEL3))
        {
            IsActive = false;
        }
    }

    public virtual void Update(GameTime gameTime) { }

    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        _parentScreen = null;
        _nameText = null;
        base.Dispose();
    }
}
