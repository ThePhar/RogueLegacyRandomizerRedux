using System.Linq;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using RogueCastle.GameStructs;

namespace RogueCastle.GameObjects.OptionsObjs;

public sealed class TextInputOptionsObj : OptionsObj
{
    private readonly string _placeholder;
    private string _currentValue = "";
    private int _cursorIndex;
    private TextObj _valueText;
    private bool _hidden;

    public TextInputOptionsObj(string nameLocID, string placeholder = "") : base(null, nameLocID)
    {
        _placeholder = placeholder;

        NameText.Align = Types.TextAlign.Centre;

        // Toggle text highlighting.
        _valueText = NameText.Clone() as TextObj;
        _valueText!.Text = _placeholder;
        _valueText.TextureColor = Color.Gray;
        _valueText.Align = Types.TextAlign.Centre;

        NameText.FontSize = 10;

        AddChild(_valueText);
        ForceDraw = true;
    }

    public string GetValue => _currentValue != string.Empty ? _currentValue : _placeholder;

    public bool Hidden
    {
        get => _hidden;
        set
        {
            if (_hidden == value)
            {
                return;
            }

            _hidden = value;
            UpdateText();
        }
    }

    public override void Update(GameTime gameTime)
    {
        _valueText.Position = NameText.Position + new Vector2(0, 28f);

        base.Update(gameTime);
    }

    public override bool IsActive
    {
        get => base.IsActive;
        set
        {
            base.IsActive = value;
            if (value)
            {
                _valueText.TextureColor = Color.Yellow;
                _cursorIndex = _currentValue.Length;
                _valueText.Text = Hidden
                    ? string.Concat(Enumerable.Repeat("*", _currentValue.Length))
                    : _currentValue;

                TextInputEXT.TextInput += InterceptKey;
                TextInputEXT.StartTextInput();
                return;
            }

            _valueText.TextureColor = _currentValue.Length > 0 ? Color.White : Color.Gray;

            UpdateText();
        }
    }

    public override void HandleInput()
    {
        // Do not leave early if we're still typing.
        if (!TextInputEXT.IsTextInputActive() && (Game.GlobalInput.PressedCancel() || Game.GlobalInput.PressedConfirm()))
        {
            SoundManager.PlaySound("Options_Menu_Deselect");
            IsActive = false;
        }

        if (IsActive)
        {
            UpdateText();
            _valueText.Text = _valueText.Text.Insert(_cursorIndex, "|");
        }
        else
        {
            _currentValue = _currentValue.Trim();
        }
    }

    private void InterceptKey(char chr)
    {
        if (!char.IsControl(chr))
        {
            _currentValue = _currentValue.Insert(_cursorIndex, $"{chr}");
            _cursorIndex++;
            return;
        }

        // Control character.
        switch (chr)
        {
            // Backspace
            case '\b':
            {
                if (_cursorIndex != 0)
                {
                    _currentValue = _currentValue.Remove(_cursorIndex - 1, 1);
                    _cursorIndex -= 1;
                }

                break;
            }

            // Enter/Return (to confirm input)
            case '\n':
            case '\r':
                TextInputEXT.StopTextInput();
                TextInputEXT.TextInput -= InterceptKey;
                break;
        }
    }

    private void UpdateText()
    {
        if (Hidden && _currentValue.Length > 0)
        {
            _valueText.Text = string.Concat(Enumerable.Repeat("*", _currentValue.Length));
        }
        else
        {
            _valueText.Text = _currentValue.Length > 0 || IsActive ? _currentValue : _placeholder;
        }
    }

    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        _valueText?.Dispose();
        _valueText = null;
        base.Dispose();
    }
}
