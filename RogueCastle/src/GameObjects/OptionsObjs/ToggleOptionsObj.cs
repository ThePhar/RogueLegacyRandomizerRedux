using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.GameStructs;

namespace RogueCastle.GameObjects.OptionsObjs;

public class ToggleOptionsObj : OptionsObj
{
    private TextObj _toggleText;
    private bool _value;
    private bool _prevValue;

    public ToggleOptionsObj(string nameLocID, bool value = false) : base(null, nameLocID)
    {
        _toggleText = NameText.Clone() as TextObj;
        _toggleText!.X = OPTIONS_TEXT_OFFSET;
        Value = value;
        _prevValue = Value;

        base.AddChild(_toggleText);
    }

    public bool Value
    {
        get => _value;
        set
        {
            _value = value;
            _toggleText.Text = _value
                ? "LOC_ID_QUICKDROP_OPTIONS_3".GetString(_toggleText)
                : "LOC_ID_QUICKDROP_OPTIONS_2".GetString(_toggleText);
        }
    }

    public override bool IsActive
    {
        get => base.IsActive;
        set
        {
            base.IsActive = value;
            _toggleText.TextureColor = value ? Color.Yellow : Color.White;
        }
    }

    public override void HandleInput()
    {
        if (Game.GlobalInput.PressedLeft() || Game.GlobalInput.PressedRight())
        {
            SoundManager.PlaySound("frame_swap");
            Value = !Value;
        }
        else if (Game.GlobalInput.PressedConfirm())
        {
            SoundManager.PlaySound("Option_Menu_Select");
            _prevValue = Value;
            IsActive = false;
        }
        else if (Game.GlobalInput.PressedCancel())
        {
            Value = _prevValue;
            IsActive = false;
        }

        base.HandleInput();
    }

    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        _toggleText?.Dispose();
        _toggleText = null;

        base.Dispose();
    }
}
