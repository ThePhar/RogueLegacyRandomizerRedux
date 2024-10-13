using RogueCastle.Screens.BaseObjects;

namespace RogueCastle.GameObjects.OptionsObjs;

public class BackToMenuOptionsObj(OptionsScreen parentScreen) : OptionsObj(parentScreen, "LOC_ID_BACK_TO_MENU_OPTIONS_1")
{
    public override bool IsActive
    {
        get => base.IsActive;
        set
        {
            base.IsActive = value;
            if (IsActive)
            {
                var rcs = m_parentScreen.ScreenManager as RCScreenManager;
                rcs!.DialogueScreen.SetDialogue("Back to Menu");
                rcs.DialogueScreen.SetDialogueChoice("ConfirmTest1");
                rcs.DialogueScreen.SetConfirmEndHandler(this, "GoBackToTitle");
                rcs.DialogueScreen.SetCancelEndHandler(this, "CancelCommand");
                rcs.DisplayScreen(ScreenType.Dialogue, false);
            }
        }
    }

    public override void Initialize()
    {
        if (Game.PlayerStats.TutorialComplete)
        {
            //"Quit to Title Screen"
            m_nameText.Text = LocaleBuilder.getString("LOC_ID_BACK_TO_MENU_OPTIONS_1", m_nameText);
        }
        else
        {
            //"Quit to Title Screen (skip tutorial)"
            m_nameText.Text = LocaleBuilder.getString("LOC_ID_BACK_TO_MENU_OPTIONS_2", m_nameText); 
        }

        base.Initialize();
    }

    public void GoBackToTitle()
    {
        IsActive = false;

        // Disconnect from AP.
        (Game.ScreenManager.Game as Game)!.ArchipelagoManager.Disconnect();

        var level = Game.ScreenManager.GetLevelScreen();
        //Special handling to revert your spell if you are in a carnival room.
        if (level != null && (level.CurrentRoom is CarnivalShoot1BonusRoom || level.CurrentRoom is CarnivalShoot2BonusRoom))
        {
            if (level.CurrentRoom is CarnivalShoot1BonusRoom carnivalRoom1)
            {
                carnivalRoom1.UnequipPlayer();
            }

            if (level.CurrentRoom is CarnivalShoot2BonusRoom carnivalRoom2)
            {
                carnivalRoom2.UnequipPlayer();
            }
        }

        // A check to make sure challenge rooms do not override player save data.
        if (level is { CurrentRoom: ChallengeBossRoomObj challengeRoom })
        {
            challengeRoom.LoadPlayerData(); // Make sure this is loaded before upgrade data, otherwise player equipment will be overridden.
            (m_parentScreen.ScreenManager.Game as Game)!.SaveManager.LoadFiles(level, SaveType.UpgradeData);
            level.Player.CurrentHealth = challengeRoom.StoredHP;
            level.Player.CurrentMana = challengeRoom.StoredMP;
        }

        // This code is needed otherwise the lineage data will still be on Revision 0 when the game exits, but player data is Rev1
        // which results in a mismatch.
        if (Game.PlayerStats.RevisionNumber <= 0)
        {
            (m_parentScreen.ScreenManager.Game as Game)!.SaveManager.SaveFiles(SaveType.Lineage);
        }

        (m_parentScreen.ScreenManager.Game as Game)!.SaveManager.SaveFiles(SaveType.PlayerData, SaveType.UpgradeData);

        if (Game.PlayerStats.TutorialComplete && level != null && level.CurrentRoom.Name != "Start" && level.CurrentRoom.Name != "Ending" && level.CurrentRoom.Name != "Tutorial")
        {
            (m_parentScreen.ScreenManager.Game as Game)!.SaveManager.SaveFiles(SaveType.MapData);
        }

        Game.ScreenManager.DisplayScreen(ScreenType.Title, true);
    }

    public void CancelCommand()
    {
        IsActive = false;
    }
}
