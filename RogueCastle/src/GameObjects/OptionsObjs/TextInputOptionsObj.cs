using System;
using System.Linq;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RogueCastle.GameObjects.OptionsObjs;

public sealed class TextInputOptionsObj : ObjContainer
{
    private const int OptionsTextOffset = 24;

    private bool             _isActive;
    private bool             _isSelected;
    private bool             _ready;
    private bool             _hidden;
    private string           _placeholder;
    private string           _currentValue = "";
    private TextObj          _titleText;
    private TextObj          _valueText;
    private int              _cursorIndex;
    
    public TextInputOptionsObj(string title, string placeholder = "", bool hidden = false)
    {
        _titleText = new TextObj(Game.JunicodeFont)
        {
            FontSize = 12f,
            Text = title,
            DropShadow = new Vector2(2f, 2f),
            Align = Types.TextAlign.Right,
        };

        _placeholder = placeholder;
        _hidden = hidden;
        
        // Toggle text highlighting.
        _valueText = _titleText.Clone() as TextObj;
        _valueText!.X = OptionsTextOffset;
        _valueText.Text = _placeholder;
        _valueText.TextureColor = Color.Gray;
        _valueText.Align = Types.TextAlign.Left;

        AddChild(_titleText);
        AddChild(_valueText);

        ForceDraw = true;
    }

    public string GetValue => _currentValue != string.Empty ? _currentValue : _placeholder;

    public bool IsActive
    {
        get => _isActive;
        set
        {
            _isActive = value;
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

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            if (value)
            {
                _titleText.TextureColor = Color.Yellow;
                return;
            }

            _titleText.TextureColor = Color.White;
        }
    }
    
    public void HandleInput()
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
            
            // Tab, Enter, Return (to cancel input)
            case '\n':
            case '\r':
                TextInputEXT.StopTextInput();
                TextInputEXT.TextInput -= InterceptKey;
                break;
            
            default:
                Console.WriteLine($@"Ignored key: {chr} ({char.GetNumericValue(chr)})");
                break;
        }
    }
    
    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }
        
        _titleText?.Dispose();
        _titleText = null;
        _valueText?.Dispose();
        _valueText = null;
    }
}
