using RogueCastle.Screens;

namespace RogueCastle.GameObjects.OptionsObjs;

public sealed class ConnectArchipelagoOptionsObj : RandomizerOptionsObj
{
    private readonly RandomizerScreen _parentScreen;
    
    public ConnectArchipelagoOptionsObj(string title, RandomizerScreen parentScreen) : base(title)
    {
        _parentScreen = parentScreen;

        AddChild(TitleText);
        ForceDraw = true;
    }

    public override bool IsActive
    {
        get => base.IsActive;
        set
        {
            base.IsActive = value;
            if (IsActive)
            {
                var rcs = Game.ScreenManager;
                
                rcs.DialogueScreen.SetDialogue("Multiworld Connect");
                rcs.DialogueScreen.SetDialogueChoice("ConfirmTest1");
                rcs.DialogueScreen.SetConfirmEndHandler(this, "StartGame");
                rcs.DialogueScreen.SetCancelEndHandler(this, "CancelCommand");
                rcs.DisplayScreen(ScreenType.Dialogue, false);
            }
        }
    }

    public void StartGame()
    {
        IsActive = false;
        _parentScreen.StartGame();
    }

    public void CancelCommand()
    {
        IsActive = false;
    }
}
