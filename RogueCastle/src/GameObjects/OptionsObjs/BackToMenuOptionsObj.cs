﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.GameObjects.OptionsObjs;
using RogueCastle.GameStructs;
using RogueCastle.Managers;
using RogueCastle.Screens;
using RogueCastle.Screens.BaseScreens;

namespace RogueCastle
{
    public class BackToMenuOptionsObj : OptionsObj
    {
        public BackToMenuOptionsObj(OptionsScreen parentScreen)
            : base(parentScreen, "LOC_ID_BACK_TO_MENU_OPTIONS_1") //"Quit to Title Screen"
        {
        }

        public override void Initialize()
        {
            if (Game.PlayerStats.TutorialComplete == true)
                NameText.Text = LocaleBuilder.GetString("LOC_ID_BACK_TO_MENU_OPTIONS_1", NameText); //"Quit to Title Screen"
            else
                NameText.Text = LocaleBuilder.GetString("LOC_ID_BACK_TO_MENU_OPTIONS_2", NameText); //"Quit to Title Screen (skip tutorial)"

            base.Initialize();
        }

        public void GoBackToTitle()
        {
            IsActive = false;

            ProceduralLevelScreen level = Game.ScreenManager.GetLevelScreen();
            //Special handling to revert your spell if you are in a carnival room.
            if (level != null && (level.CurrentRoom is CarnivalShoot1BonusRoom || level.CurrentRoom is CarnivalShoot2BonusRoom))
            {
                if (level.CurrentRoom is CarnivalShoot1BonusRoom)
                    (level.CurrentRoom as CarnivalShoot1BonusRoom).UnequipPlayer();
                if (level.CurrentRoom is CarnivalShoot2BonusRoom)
                    (level.CurrentRoom as CarnivalShoot2BonusRoom).UnequipPlayer();
            }

            // A check to make sure challenge rooms do not override player save data.
            if (level != null)
            {
                ChallengeBossRoomObj challengeRoom = level.CurrentRoom as ChallengeBossRoomObj;
                if (challengeRoom != null)
                {
                    challengeRoom.LoadPlayerData(); // Make sure this is loaded before upgrade data, otherwise player equipment will be overridden.
                    (ParentScreen.ScreenManager.Game as Game).SaveManager.LoadFiles(level, SaveType.UpgradeData);
                    level.Player.CurrentHealth = challengeRoom.StoredHP;
                    level.Player.CurrentMana = challengeRoom.StoredMP;
                }
            }

            // This code is needed otherwise the lineage data will still be on Revision 0 when the game exits, but player data is Rev1
            // which results in a mismatch.
            if (Game.PlayerStats.RevisionNumber <= 0)
                 (ParentScreen.ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.Lineage);
            (ParentScreen.ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.PlayerData, SaveType.UpgradeData, SaveType.Archipelago);

            if (Game.PlayerStats.TutorialComplete == true && level != null && level.CurrentRoom.Name != "Start" && level.CurrentRoom.Name != "Ending" && level.CurrentRoom.Name != "Tutorial")
                (ParentScreen.ScreenManager.Game as Game).SaveManager.SaveFiles(SaveType.MapData);

            Game.ScreenManager.DisplayScreen(ScreenType.TITLE, true);
        }

        public void CancelCommand()
        {
            IsActive = false;
        }

        public override bool IsActive
        {
            get { return base.IsActive; }
            set
            {
                base.IsActive = value;
                if (IsActive == true)
                {
                    RCScreenManager manager = ParentScreen.ScreenManager as RCScreenManager;
                    manager.DialogueScreen.SetDialogue("Back to Menu");
                    manager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
                    manager.DialogueScreen.SetConfirmEndHandler(this, "GoBackToTitle");
                    manager.DialogueScreen.SetCancelEndHandler(this, "CancelCommand");
                    manager.DisplayScreen(ScreenType.DIALOGUE, false, null);
                }
            }
        }
    }
}
