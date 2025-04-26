using DS2DEngine;
using RogueCastle.Screens;

namespace RogueCastle.GameObjects.OptionsObjs;

// The base object only likes OptionsScreen, and I'm too lazy to create another base for this menu.
public class ConnectOptionsObj(RandomizerMenuScreen parentScreen, string nameLocID) : OptionsObj(null, nameLocID)
{
    public override void Initialize()
    {
        NameText.Align = Types.TextAlign.Centre;
        base.Initialize();
    }

    public override bool IsActive
    {
        get => base.IsActive;
        set
        {
            base.IsActive = value;
            if (IsActive)
            {
                parentScreen.StartConnect();
            }
        }
    }
}
