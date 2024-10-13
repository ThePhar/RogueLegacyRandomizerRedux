using DS2DEngine;
using Microsoft.Xna.Framework;

namespace RogueCastle.GameObjects;

public class SaveSlotObj : ObjContainer
{
    private TextObj _nameTextObj;
    private TextObj _seedTextObj;
    private TextObj _levelTextObj;
    private TextObj _generationTextObj;
    private TextObj _lastAccessTextObj;
    
    public SaveSlotObj(TextObj baseText) : base("ProfileSlotBG_Container")
    {
        Scale = new Vector2(1.5f, 1.5f);

        // Initialize text objects.
        _nameTextObj = baseText.Clone() as TextObj;
        _seedTextObj = baseText.Clone() as TextObj;
        _levelTextObj = baseText.Clone() as TextObj;
        _generationTextObj = baseText.Clone() as TextObj;
        // _lastAccessTextObj = baseText.Clone() as TextObj;
        
        // Placeholder text.
        _nameTextObj!.Text = LocaleBuilder.getString("LOC_ID_CLASS_NAME_1_MALE", _nameTextObj);
        // _generationTextObj!.Text = LocaleBuilder.getString("LOC_ID_CLASS_NAME_1_MALE", _generationTextObj);
        // _lastAccessTextObj!.Text = LocaleBuilder.getString("LOC_ID_CLASS_NAME_1_MALE", _lastAccessTextObj);

        // Positions
        _seedTextObj!.Text = LocaleBuilder.getString("LOC_ID_CLASS_NAME_1_MALE", _seedTextObj);
        _seedTextObj.Position = new Vector2(0, -35);
        _seedTextObj.Align = Types.TextAlign.Left;
        
        _levelTextObj!.Text = LocaleBuilder.getString("LOC_ID_CLASS_NAME_1_MALE", _levelTextObj);
        _levelTextObj.Position = new Vector2(160, 15);
        _levelTextObj.Align = Types.TextAlign.Right;

        AddChild(_nameTextObj);
        AddChild(_levelTextObj);
        AddChild(_seedTextObj);
        ForceDraw = true;
    }

    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }
        
        _nameTextObj?.Dispose();
        _seedTextObj?.Dispose();
        _levelTextObj?.Dispose();
        _generationTextObj?.Dispose();
        _lastAccessTextObj?.Dispose();
        
        _nameTextObj = null;
        _seedTextObj = null;
        _levelTextObj = null;
        _generationTextObj = null;
        _lastAccessTextObj = null;
        
        base.Dispose();
    }
}
