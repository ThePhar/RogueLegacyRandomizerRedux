using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.GameStructs;
using RogueCastle.Screens;

namespace RogueCastle.GameObjects.OptionsObjs;

public abstract class OptionsObj : ObjContainer
{
    protected const int OPTIONS_TEXT_OFFSET = 300;

    protected TextObj NameText;
    protected OptionsScreen ParentScreen;

    private bool _isActive;
    private bool _isSelected;

    protected OptionsObj(OptionsScreen parentScreen, string nameLocID)
    {
        ParentScreen = parentScreen;

        NameText = new TextObj(Game.JunicodeFont)
        {
            FontSize = 12,
            DropShadow = new Vector2(2, 2),
        };
        NameText.Text = nameLocID.GetString(NameText, true);

        AddChild(NameText);
        ForceDraw = true;
    }

    public virtual bool IsActive
    {
        get => _isActive;
        set
        {
            IsSelected = !value;

            _isActive = value;
        }
    }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            NameText.TextureColor = value ? Color.Yellow : Color.White;
        }
    }

    public virtual void Initialize() { }

    public virtual void HandleInput()
    {
        if (Game.GlobalInput.PressedCancel())
        {
            SoundManager.PlaySound("Options_Menu_Deselect");
        }
    }

    public virtual void Update(GameTime gameTime) { }

    public virtual void RefreshTextObjs() { }

    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        // Don't accidentally dispose the parent screen; might still be used lol
        ParentScreen = null;

        NameText.Dispose();
        NameText = null;
        base.Dispose();
    }
}
