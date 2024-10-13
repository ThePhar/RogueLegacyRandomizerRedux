using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.Screens;

namespace RogueCastle.GameObjects.OptionsObjs;

public class TextBoxOptionsObj : RandomizerOptionsObj
{
    private readonly string  _placeholder;
    private          string  _currentValue = string.Empty;
    private          bool    _ready;
    private          TextObj _toggleText;

    public int CursorIndex;

    public TextBoxOptionsObj(RandomizerMenuScreen parentScreen, string name, string placeholder)
        : base(parentScreen, name)
    {
        _placeholder = placeholder;

        // Toggle Text Highlighting.
        _toggleText = _nameText.Clone() as TextObj;
        _toggleText!.X = OPTIONS_TEXT_OFFSET;
        _toggleText.Text = _placeholder;
        _toggleText.TextureColor = Color.Gray;

        base.AddChild(_toggleText);
    }

    public string GetValue => _currentValue != string.Empty ? _currentValue : _placeholder;

    public override bool IsActive
    {
        get => base.IsActive;
        set
        {
            base.IsActive = value;
            if (value)
            {
                _toggleText.TextureColor = Color.Yellow;
                CursorIndex = _currentValue.Length;
                return;
            }

            _ready = false;
            _toggleText.TextureColor = _currentValue.Length == 0 ? Color.Gray : Color.White;
        }
    }

    public override void Initialize()
    {
        _currentValue = string.Empty;
        base.Initialize();
    }

    public void HandleInput(object sender)
    {
        base.HandleInput();
    }

    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        _toggleText = null;
        base.Dispose();
    }
}
