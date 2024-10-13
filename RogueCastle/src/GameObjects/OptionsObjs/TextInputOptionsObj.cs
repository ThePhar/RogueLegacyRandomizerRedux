using System;
using System.Linq;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RogueCastle.GameObjects.OptionsObjs;

public sealed class TextInputOptionsObj : RandomizerOptionsObj
{

    private bool             _ready;
    private bool             _hidden;
    private string           _placeholder;
    private string           _currentValue = "";

    private TextObj          _valueText;
    private int              _cursorIndex;
    
    public TextInputOptionsObj(string title, string placeholder = "", bool hidden = false) : base(title)
    {
        _placeholder = placeholder;
        _hidden = hidden;

        TitleText.Align = Types.TextAlign.Right;
        
        // Toggle text highlighting.
        _valueText = TitleText.Clone() as TextObj;
        _valueText!.X = OPTIONS_TEXT_OFFSET;
        _valueText.Text = _placeholder;
        _valueText.TextureColor = Color.Gray;
        _valueText.Align = Types.TextAlign.Left;

        AddChild(TitleText);
        AddChild(_valueText);

        ForceDraw = true;
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
                _valueText.TextureColor = Color.Yellow;
                _cursorIndex = _currentValue.Length;
                _valueText.Text = _hidden 
                    ? string.Concat(Enumerable.Repeat("*", _currentValue.Length)) 
                    : _currentValue;
                
                TextInputEXT.TextInput += InterceptKey;
                TextInputEXT.StartTextInput();
                return;
            }
            
            _ready = false;
            _valueText.TextureColor = _currentValue.Length > 0 ? Color.White : Color.Gray;

            if (_hidden)
            {
                _valueText.Text = string.Concat(Enumerable.Repeat("*", _currentValue.Length));
            }
            else
            {
                _valueText.Text = _currentValue.Length > 0 ? _currentValue : _placeholder;
            }
        }
    }
    
    public override void HandleInput()
    {
        // Do not leave early if we're still typing.
        if (!TextInputEXT.IsTextInputActive() && (InputHelper.PressedCancel() || InputHelper.PressedConfirm()))
        {
            SoundManager.PlaySound("Options_Menu_Deselect");
            IsActive = false;
        }
        
        if (IsActive)
        {
            if (_hidden)
            {
                var obscured = string.Concat(Enumerable.Repeat("*", _currentValue.Length));
                _valueText.Text = obscured.Insert(_cursorIndex, "|");
            }
            else
            {
                _valueText.Text = _currentValue.Insert(_cursorIndex, "|");
            }
        }
        else
        {
            _currentValue = _currentValue.Trim();
        }
    }

    public void InterceptKey(char chr)
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
