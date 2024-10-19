using DS2DEngine;

namespace RogueCastle.Screens;

public class RandomizerMenuScreen : Screen
{
    private ObjContainer _bgSprite;

    public RandomizerMenuScreen()
    {
        UpdateIfCovered = true;
        DrawIfCovered = true;
    }

    public float BackBufferOpacity { get; set; }

    public override void LoadContent()
    {
        _bgSprite = new ObjContainer("SkillUnlockPlate_Character") { ForceDraw = true };
    }
}
