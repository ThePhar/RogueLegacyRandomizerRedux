using RogueCastle.GameStructs;
using RogueCastle.Screens;

namespace RogueCastle.GameObjects.OptionsObjs;

// The base object only likes OptionsScreen, and I'm too lazy to create another base for this menu.
public class ConnectOptionsObj(RandomizerMenuScreen parentScreen, string nameLocID) : OptionsObj(null, nameLocID)
{
    private RandomizerMenuScreen _parentScreen = parentScreen;

    public override bool IsActive
    {
        get => base.IsActive;
        set
        {
            base.IsActive = value;
            if (IsActive)
            {
                var rcs = Game.ScreenManager;

                rcs.DialogueScreen.SetDialogue("MultiworldConnect");
                rcs.DialogueScreen.SetDialogueChoice("ConfirmTest1");
                rcs.DialogueScreen.SetConfirmEndHandler(this, "Connect");
                rcs.DialogueScreen.SetCancelEndHandler(this, "Cancel");
                rcs.DisplayScreen(ScreenType.DIALOGUE, false);
            }
        }
    }

    public void Connect()
    {
        IsActive = false;
        Game.ScreenManager.DisplayScreen(ScreenType.TITLE, true);
    }

    public void Cancel()
    {
        IsActive = false;
    }
}
