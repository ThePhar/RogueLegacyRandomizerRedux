using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.GameObjects.OptionsObjs;
using RogueCastle.GameStructs;
using RogueCastle.Screens;
using RogueCastle.Screens.BaseScreens;

namespace RogueCastle
{
    public class ExitProgramOptionsObj : OptionsObj
    {
        public ExitProgramOptionsObj(OptionsScreen parentScreen)
            : base(parentScreen, "LOC_ID_EXIT_ROGUE_LEGACY_OPTIONS_1") //"Quit Rogue Legacy"
        {
        }

        public void QuitProgram()
        {
            (ParentScreen.ScreenManager.Game as Game).SaveOnExit();
            ParentScreen.ScreenManager.Game.Exit();
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
                    manager.DialogueScreen.SetDialogue("Quit Rogue Legacy");
                    manager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
                    manager.DialogueScreen.SetConfirmEndHandler(this, "QuitProgram");
                    manager.DialogueScreen.SetCancelEndHandler(this, "CancelCommand");
                    manager.DisplayScreen(ScreenType.DIALOGUE, false, null);
                }
            }
        }
    }
}
