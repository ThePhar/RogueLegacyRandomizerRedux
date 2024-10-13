using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueCastle.GameObjects.OptionsObjs;

public abstract class RandomizerOptionsObj : ObjContainer
{
    protected const int     OPTIONS_TEXT_OFFSET = 24;
    private         bool    _isSelected;
    protected       TextObj TitleText;

    protected RandomizerOptionsObj(string title)
    {
        TitleText = new TextObj(Game.JunicodeFont)
        {
            FontSize = 12f,
            DropShadow = new Vector2(2f, 2f),
            Align = Types.TextAlign.Centre,
        };
        TitleText.Text = LocaleBuilder.getString(title, TitleText);
    }

    public virtual bool IsActive { get; set; }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            if (value)
            {
                TitleText.TextureColor = Color.Yellow;
                return;
            }

            TitleText.TextureColor = Color.White;
        }
    }

    public virtual void HandleInput() { }
    
    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }
        
        TitleText?.Dispose();
        TitleText = null;
        base.Dispose();
    }
}
