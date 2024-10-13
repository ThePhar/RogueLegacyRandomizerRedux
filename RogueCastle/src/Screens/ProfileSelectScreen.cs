using System.Collections.Generic;
using System.Text.RegularExpressions;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using RogueCastle.EVs;
using RogueCastle.GameObjects;
using Tweener;
using Tweener.Ease;

namespace RogueCastle.Screens;

public class ProfileSelectScreen : Screen
{
    private KeyIconTextObj     _cancelText;
    private KeyIconTextObj     _confirmText;
    private KeyIconTextObj     _deleteProfileText;
    private bool               _lockControls;
    private KeyIconTextObj     _navigationText;
    private int                _selectedIndex;
    private ObjContainer       _selectedSlot;
    private ObjContainer       _slot1Container; //, _slot2Container, _slot3Container;
    private List<ObjContainer> _slotArray;
    private SpriteObj          _title;

    public ProfileSelectScreen()
    {
        _slotArray = new List<ObjContainer>();
        DrawIfCovered = true;
    }

    public float BackBufferOpacity { get; set; }

    public override void LoadContent()
    {
        _title = new SpriteObj("ProfileSelectTitle_Sprite")
        {
            ForceDraw = true,
        };

        // Template for slot text
        var slotText = new TextObj(Game.JunicodeFont)
        {
            Align = Types.TextAlign.Centre,
        };
        slotText.Text = LocaleBuilder.getString("LOC_ID_PROFILE_SEL_SCREEN_1", slotText);
        slotText.TextureColor = Color.White;
        slotText.OutlineWidth = 2;
        slotText.FontSize = 10;
        slotText.Position = new Vector2(0, -(slotText.Height / 2f));

        _slot1Container = new SaveSlotObj(slotText);
        
        // _slot1Container = new ObjContainer("ProfileSlotBG_Container");
        // _slot1Container.Scale = new Vector2(1.5f, 1.5f);
        // var slot1Text = slotText.Clone() as TextObj;
        // slot1Text!.Text = LocaleBuilder.getString("LOC_ID_CLASS_NAME_1_MALE", slot1Text);
        // _slot1Container.AddChild(slot1Text);
        // // var slot1Title = new SpriteObj("ProfileSlot1Text_Sprite")
        // // {
        // //     Position = new Vector2(-130, -35),
        // // };
        // // _slot1Container.AddChild(slot1Title);
        // var slot1LvlText = slotText.Clone() as TextObj;
        // slot1LvlText!.Text = LocaleBuilder.getString("LOC_ID_CLASS_NAME_1_MALE", slot1LvlText); // dummy locID to add TextObj to language refresh list
        // slot1LvlText.Position = new Vector2(120, 15);
        // _slot1Container.AddChild(slot1LvlText);
        // var slot1NGText = slotText.Clone() as TextObj;
        // slot1NGText!.Text = LocaleBuilder.getString("LOC_ID_CLASS_NAME_1_MALE", slot1NGText); // dummy locID to add TextObj to language refresh list
        // slot1NGText.Position = new Vector2(-120, 15);
        // _slot1Container.AddChild(slot1NGText);
        // _slot1Container.ForceDraw = true;

        // _slot2Container = new ObjContainer("ProfileSlotBG_Container");
        // var slot2Text = slotText.Clone() as TextObj;
        // slot2Text!.Text =
        //     LocaleBuilder.getString("LOC_ID_CLASS_NAME_1_MALE",
        //         slot2Text); // dummy locID to add TextObj to language refresh list
        // _slot2Container.AddChild(slot2Text);
        // var slot2Title = new SpriteObj("ProfileSlot2Text_Sprite")
        // {
        //     Position = new Vector2(-130, -35),
        // };
        // _slot2Container.AddChild(slot2Title);
        // var slot2LvlText = slotText.Clone() as TextObj;
        // slot2LvlText!.Text =
        //     LocaleBuilder.getString("LOC_ID_CLASS_NAME_1_MALE",
        //         slot2LvlText); // dummy locID to add TextObj to language refresh list
        // slot2LvlText.Position = new Vector2(120, 15);
        // _slot2Container.AddChild(slot2LvlText);
        // var slot2NGText = slotText.Clone() as TextObj;
        // slot2NGText!.Text =
        //     LocaleBuilder.getString("LOC_ID_CLASS_NAME_1_MALE",
        //         slot2NGText); // dummy locID to add TextObj to language refresh list
        // slot2NGText.Position = new Vector2(-120, 15);
        // _slot2Container.AddChild(slot2NGText);
        // _slot2Container.ForceDraw = true;
        //
        // _slot3Container = new ObjContainer("ProfileSlotBG_Container");
        // var slot3Text = slotText.Clone() as TextObj;
        // slot3Text!.Text =
        //     LocaleBuilder.getString("LOC_ID_CLASS_NAME_1_MALE",
        //         slot3Text); // dummy locID to add TextObj to language refresh list
        // _slot3Container.AddChild(slot3Text);
        // var slot3Title = new SpriteObj("ProfileSlot3Text_Sprite")
        // {
        //     Position = new Vector2(-130, -35),
        // };
        // _slot3Container.AddChild(slot3Title);
        // var slot3LvlText = slotText.Clone() as TextObj;
        // slot3LvlText!.Text =
        //     LocaleBuilder.getString("LOC_ID_CLASS_NAME_1_MALE",
        //         slot3LvlText); // dummy locID to add TextObj to language refresh list
        // slot3LvlText.Position = new Vector2(120, 15);
        // _slot3Container.AddChild(slot3LvlText);
        // var slot3NGText = slotText.Clone() as TextObj;
        // slot3NGText!.Text =
        //     LocaleBuilder.getString("LOC_ID_CLASS_NAME_1_MALE",
        //         slot3NGText); // dummy locID to add TextObj to language refresh list
        // slot3NGText.Position = new Vector2(-120, 15);
        // _slot3Container.AddChild(slot3NGText);
        // _slot3Container.ForceDraw = true;

        _slotArray.Add(_slot1Container);
        // _slotArray.Add(_slot2Container);
        // _slotArray.Add(_slot3Container);

        _confirmText = new KeyIconTextObj(Game.JunicodeFont);
        _confirmText.Text = LocaleBuilder.getString("LOC_ID_PROFILE_SEL_SCREEN_4", _confirmText);
        _confirmText.DropShadow = new Vector2(2, 2);
        _confirmText.FontSize = 12;
        _confirmText.Align = Types.TextAlign.Right;
        _confirmText.Position = new Vector2(1290, 570);
        _confirmText.ForceDraw = true;

        _cancelText = new KeyIconTextObj(Game.JunicodeFont);
        _cancelText.Text = LocaleBuilder.getString("LOC_ID_PROFILE_SEL_SCREEN_5", _cancelText);
        _cancelText.Align = Types.TextAlign.Right;
        _cancelText.DropShadow = new Vector2(2, 2);
        _cancelText.FontSize = 12;
        _cancelText.Position = new Vector2(_confirmText.X, _confirmText.Y + 40);
        _cancelText.ForceDraw = true;

        _navigationText = new KeyIconTextObj(Game.JunicodeFont);
        _navigationText.Text = LocaleBuilder.getString("LOC_ID_PROFILE_SEL_SCREEN_2", _navigationText);
        _navigationText.Align = Types.TextAlign.Right;
        _navigationText.DropShadow = new Vector2(2, 2);
        _navigationText.FontSize = 12;
        _navigationText.Position = new Vector2(_confirmText.X, _confirmText.Y + 80);
        _navigationText.ForceDraw = true;

        _deleteProfileText = new KeyIconTextObj(Game.JunicodeFont);
        _deleteProfileText.Text = LocaleBuilder.getString("LOC_ID_PROFILE_SEL_SCREEN_6", _deleteProfileText);
        _deleteProfileText.Align = Types.TextAlign.Left;
        _deleteProfileText.DropShadow = new Vector2(2, 2);
        _deleteProfileText.FontSize = 12;
        _deleteProfileText.Position = new Vector2(20, _confirmText.Y + 80);
        _deleteProfileText.ForceDraw = true;

        base.LoadContent();
    }

    public override void OnEnter()
    {
        SoundManager.PlaySound("DialogOpen");
        _lockControls = true;
        _selectedIndex = Game.GameConfig.ProfileSlot - 1;
        _selectedSlot = _slotArray[_selectedIndex];
        _selectedSlot.TextureColor = Color.Yellow;

        CheckSaveHeaders(_slot1Container, 1);
        // CheckSaveHeaders(_slot2Container, 2);
        // CheckSaveHeaders(_slot3Container, 3);

        _deleteProfileText.Visible = true;
        if (_slotArray[_selectedIndex].ID == 0)
        {
            _deleteProfileText.Visible = false;
        }

        Tween.To(this, 0.2f, Tween.EaseNone, "BackBufferOpacity", "0.9");

        _title.Position = new Vector2(1320 / 2f, 100);
        _slot1Container.Position = new Vector2(1320 / 2f, 300);
        // _slot2Container.Position = new Vector2(1320 / 2f, 420);
        // _slot3Container.Position = new Vector2(1320 / 2f, 540);

        TweenInText(_title, 0);
        TweenInText(_slot1Container, 0.05f);
        // TweenInText(_slot2Container, 0.1f);
        // TweenInText(_slot3Container, 0.15f);

        Tween.RunFunction(0.5f, this, "UnlockControls");

        if (InputManager.GamePadIsConnected(PlayerIndex.One))
        {
            _confirmText.ForcedScale = new Vector2(0.7f, 0.7f);
            _cancelText.ForcedScale = new Vector2(0.7f, 0.7f);
            _navigationText.Text = LocaleBuilder.getString("LOC_ID_PROFILE_SEL_SCREEN_2_NEW", _navigationText);
        }
        else
        {
            _confirmText.ForcedScale = new Vector2(1f, 1f);
            _cancelText.ForcedScale = new Vector2(1f, 1f);
            _navigationText.Text = LocaleBuilder.getString("LOC_ID_PROFILE_SEL_SCREEN_3", _navigationText);
        }

        _confirmText.Text = LocaleBuilder.getString("LOC_ID_PROFILE_SEL_SCREEN_4_NEW", _confirmText);
        _cancelText.Text = LocaleBuilder.getString("LOC_ID_PROFILE_SEL_SCREEN_5_NEW", _cancelText);
        _deleteProfileText.Text = LocaleBuilder.getString("LOC_ID_PROFILE_SEL_SCREEN_6_NEW", _deleteProfileText);

        _confirmText.Opacity = 0;
        _cancelText.Opacity = 0;
        _navigationText.Opacity = 0;
        _deleteProfileText.Opacity = 0;

        Tween.To(_confirmText, 0.2f, Tween.EaseNone, "Opacity", "1");
        Tween.To(_cancelText, 0.2f, Tween.EaseNone, "Opacity", "1");
        Tween.To(_navigationText, 0.2f, Tween.EaseNone, "Opacity", "1");
        Tween.To(_deleteProfileText, 0.2f, Tween.EaseNone, "Opacity", "1");

        Game.ChangeBitmapLanguage(_title, "ProfileSelectTitle_Sprite");
        // Game.ChangeBitmapLanguage(_slot1Container.GetChildAt(2) as SpriteObj, "ProfileSlot1Text_Sprite");
        // Game.ChangeBitmapLanguage(_slot2Container.GetChildAt(2) as SpriteObj, "ProfileSlot2Text_Sprite");
        // Game.ChangeBitmapLanguage(_slot3Container.GetChildAt(2) as SpriteObj, "ProfileSlot3Text_Sprite");

        base.OnEnter();
    }

    private void CheckSaveHeaders(ObjContainer container, byte profile)
    {
        var slotName = container.GetChildAt(1) as TextObj;
        var slotLevel = container.GetChildAt(2) as TextObj;
        var slotSeed = container.GetChildAt(3) as TextObj;
        slotLevel!.Text = "";
        slotSeed!.Text = "Seed #############";
        string playerName = null;
        byte playerClass = 0;
        var playerLevel = 0;
        var isDead = false;
        var timesCastleBeaten = 0;
        var isFemale = false;

        try
        {
            (ScreenManager.Game as Game).SaveManager.GetSaveHeader(profile, out playerClass, out playerName,
                out playerLevel, out isDead, out timesCastleBeaten, out isFemale);

            if (playerName == null)
            {
                slotName.Text = LocaleBuilder.getResourceString("LOC_ID_PROFILE_SEL_SCREEN_1");
                container.ID = 0; // Container with ID == 0 means it has no save file.
            }
            else
            {
                // This call to Game.NameHelper forces a name conversion check every time.  This is necessary because it is possible for different profile slots to have different
                // save revision numbers.  So you have to do the check every time in case that happens only in this scenario (since the check can be a little expensive).
                playerName = Game.NameHelper(playerName, "", isFemale, true);

                try
                {
                    slotName.ChangeFontNoDefault(LocaleBuilder.GetLanguageFont(slotName));
                    if (isDead == false)
                    {
                        slotName.Text = string.Format(
                            LocaleBuilder.getResourceString(!isFemale
                                ? "LOC_ID_PROFILE_SEL_SCREEN_7_MALE_NEW"
                                : "LOC_ID_PROFILE_SEL_SCREEN_7_FEMALE_NEW"), playerName,
                            LocaleBuilder.getResourceString(ClassType.ToStringID(playerClass,
                                isFemale))); // {0} the {1}
                    }
                    else
                    {
                        slotName.Text =
                            string.Format(
                                LocaleBuilder.getResourceString(!isFemale
                                    ? "LOC_ID_PROFILE_SEL_SCREEN_8_MALE_NEW"
                                    : "LOC_ID_PROFILE_SEL_SCREEN_8_FEMALE_NEW"), playerName); // {0} the deceased
                    }

                    if (LocaleBuilder.languageType != LanguageType.Chinese_Simp &&
                        Regex.IsMatch(slotName.Text, @"\p{IsCyrillic}"))
                    {
                        slotName.ChangeFontNoDefault(Game.RobotoSlabFont);
                    }
                }
                catch
                {
                    slotName.ChangeFontNoDefault(Game.NotoSansSCFont);
                    if (isDead == false)
                    {
                        slotName.Text = string.Format(
                            LocaleBuilder.getResourceString(!isFemale
                                ? "LOC_ID_PROFILE_SEL_SCREEN_7_MALE_NEW"
                                : "LOC_ID_PROFILE_SEL_SCREEN_7_FEMALE_NEW"), playerName,
                            LocaleBuilder.getResourceString(ClassType.ToStringID(playerClass,
                                isFemale))); // {0} the {1}
                    }
                    else
                    {
                        slotName.Text =
                            string.Format(
                                LocaleBuilder.getResourceString(!isFemale
                                    ? "LOC_ID_PROFILE_SEL_SCREEN_8_MALE_NEW"
                                    : "LOC_ID_PROFILE_SEL_SCREEN_8_FEMALE_NEW"), playerName); // {0} the deceased
                    }
                }

                slotLevel.Text = LocaleBuilder.getResourceString("LOC_ID_PROFILE_SEL_SCREEN_9") + " " + playerLevel;
                if (timesCastleBeaten > 0)
                {
                    slotSeed.Text = LocaleBuilder.getResourceString("LOC_ID_PROFILE_SEL_SCREEN_10") + " " +
                                      timesCastleBeaten;
                }

                container.ID = 1; // Container with ID == 1 means it has a save file.
            }
        }
        catch
        {
            slotName.Text = LocaleBuilder.getString("LOC_ID_PROFILE_SEL_SCREEN_1", slotName);
            container.ID = 0; // Container with ID == 0 means it has no save file.
        }
    }

    public void UnlockControls()
    {
        _lockControls = false;
    }

    private void TweenInText(GameObj obj, float delay)
    {
        obj.Opacity = 0;
        obj.Y -= 50;
        Tween.To(obj, 0.5f, Tween.EaseNone, "delay", delay.ToString(), "Opacity", "1");
        Tween.By(obj, 0.5f, Quad.EaseOut, "delay", delay.ToString(), "Y", "50");
    }

    private void ExitTransition()
    {
        SoundManager.PlaySound("DialogMenuClose");

        Tween.To(_confirmText, 0.2f, Tween.EaseNone, "Opacity", "0");
        Tween.To(_cancelText, 0.2f, Tween.EaseNone, "Opacity", "0");
        Tween.To(_navigationText, 0.2f, Tween.EaseNone, "Opacity", "0");
        Tween.To(_deleteProfileText, 0.2f, Tween.EaseNone, "Opacity", "0");

        _lockControls = true;

        TweenOutText(_title, 0);
        TweenOutText(_slot1Container, 0.05f);
        // TweenOutText(_slot2Container, 0.1f);
        // TweenOutText(_slot3Container, 0.15f);

        Tween.To(this, 0.2f, Tween.EaseNone, "delay", "0.5", "BackBufferOpacity", "0");
        Tween.AddEndHandlerToLastTween(ScreenManager, "HideCurrentScreen");
    }

    private void TweenOutText(GameObj obj, float delay)
    {
        Tween.To(obj, 0.5f, Tween.EaseNone, "delay", delay.ToString(), "Opacity", "0");
        Tween.By(obj, 0.5f, Quad.EaseInOut, "delay", delay.ToString(), "Y", "-50");
    }

    public override void OnExit()
    {
        _slot1Container.TextureColor = Color.White;
        // _slot2Container.TextureColor = Color.White;
        // _slot3Container.TextureColor = Color.White;
        _lockControls = false;
        base.OnExit();
    }

    public override void HandleInput()
    {
        if (_lockControls == false)
        {
            var selectedSlot = _selectedSlot;

            if (Game.GlobalInput.JustPressed(InputMapType.PLAYER_DOWN1) ||
                Game.GlobalInput.JustPressed(InputMapType.PLAYER_DOWN2))
            {
                _selectedIndex++;
                if (_selectedIndex >= _slotArray.Count)
                {
                    _selectedIndex = 0;
                }

                _selectedSlot = _slotArray[_selectedIndex];
                SoundManager.PlaySound("frame_swap");

                _deleteProfileText.Visible = true;
                if (_selectedSlot.ID == 0)
                {
                    _deleteProfileText.Visible = false;
                }
            }

            if (Game.GlobalInput.JustPressed(InputMapType.PLAYER_UP1) ||
                Game.GlobalInput.JustPressed(InputMapType.PLAYER_UP2))
            {
                _selectedIndex--;
                if (_selectedIndex < 0)
                {
                    _selectedIndex = _slotArray.Count - 1;
                }

                _selectedSlot = _slotArray[_selectedIndex];
                SoundManager.PlaySound("frame_swap");

                _deleteProfileText.Visible = true;
                if (_selectedSlot.ID == 0)
                {
                    _deleteProfileText.Visible = false;
                }
            }

            if (_selectedSlot != selectedSlot)
            {
                selectedSlot.TextureColor = Color.White;
                _selectedSlot.TextureColor = Color.Yellow;
            }

            if (Game.GlobalInput.JustPressed(InputMapType.MENU_CANCEL1) ||
                Game.GlobalInput.JustPressed(InputMapType.MENU_CANCEL2) ||
                Game.GlobalInput.JustPressed(InputMapType.MENU_CANCEL3))
            {
                ExitTransition();
            }

            if (Game.GlobalInput.JustPressed(InputMapType.MENU_CONFIRM1) ||
                Game.GlobalInput.JustPressed(InputMapType.MENU_CONFIRM2) ||
                Game.GlobalInput.JustPressed(InputMapType.MENU_CONFIRM3))
            {
                SoundManager.PlaySound("Map_On");

                Game.GameConfig.ProfileSlot = (byte) (_selectedIndex + 1);
                var game = ScreenManager.Game as Game;
                game.SaveConfig();

                if (game.SaveManager.FileExists(SaveType.PlayerData))
                {
                    (ScreenManager as RCScreenManager).DisplayScreen(ScreenType.Title, true);
                }
                else
                {
                    SkillSystem.ResetAllTraits();
                    Game.PlayerStats.Dispose();
                    Game.PlayerStats = new PlayerStats();
                    (ScreenManager as RCScreenManager).Player.Reset();
                    Game.ScreenManager.Player.CurrentHealth = Game.PlayerStats.CurrentHealth;
                    Game.ScreenManager.Player.CurrentMana = Game.PlayerStats.CurrentMana;
                    (ScreenManager as RCScreenManager).DisplayScreen(ScreenType.TutorialRoom, true);
                }
            }

            if (Game.GlobalInput.JustPressed(InputMapType.MENU_DELETEPROFILE) && _deleteProfileText.Visible)
            {
                SoundManager.PlaySound("Map_On");
                DeleteSaveAsk();
            }
        }

        base.HandleInput();
    }

    public override void Draw(GameTime gametime)
    {
        Camera.Begin();
        Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, GlobalEV.SCREEN_WIDTH, GlobalEV.SCREEN_HEIGHT),
            Color.Black * BackBufferOpacity);

        _title.Draw(Camera);
        _slot1Container.Draw(Camera);
        // _slot2Container.Draw(Camera);
        // _slot3Container.Draw(Camera);

        _confirmText.Draw(Camera);
        _cancelText.Draw(Camera);
        _navigationText.Draw(Camera);
        _deleteProfileText.Draw(Camera);

        Camera.End();
        base.Draw(gametime);
    }

    public void DeleteSaveAsk()
    {
        var manager = ScreenManager as RCScreenManager;
        manager.DialogueScreen.SetDialogue("Delete Save");
        manager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
        manager.DialogueScreen.SetConfirmEndHandler(this, "DeleteSaveAskAgain");
        manager.DisplayScreen(ScreenType.Dialogue, false);
    }

    public void DeleteSaveAskAgain()
    {
        var manager = ScreenManager as RCScreenManager;
        manager.DialogueScreen.SetDialogue("Delete Save2");
        manager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
        manager.DialogueScreen.SetConfirmEndHandler(this, "DeleteSave");
        manager.DisplayScreen(ScreenType.Dialogue, false);
    }

    public void DeleteSave()
    {
        var runTutorial = false;
        var storedProfile = Game.GameConfig.ProfileSlot;

        if (Game.GameConfig.ProfileSlot == _selectedIndex + 1)
        {
            runTutorial = true;
        }

        // Doing this to delete the correct profile slot.  Will be reverted once the file is deleted.
        Game.GameConfig.ProfileSlot = (byte) (_selectedIndex + 1);
        //Game game = (ScreenManager.Game as Game);
        //game.SaveConfig();

        (ScreenManager.Game as Game).SaveManager.ClearAllFileTypes(false);
        (ScreenManager.Game as Game).SaveManager.ClearAllFileTypes(true);

        // Reverting profile slot back to stored slot.
        Game.GameConfig.ProfileSlot = storedProfile;

        if (runTutorial)
        {
            Game.PlayerStats.Dispose();
            SkillSystem.ResetAllTraits();
            Game.PlayerStats = new PlayerStats();
            (ScreenManager as RCScreenManager).Player.Reset();

            SoundManager.StopMusic(1);

            (ScreenManager as RCScreenManager).DisplayScreen(ScreenType.TutorialRoom, true);
        }
        else
        {
            _deleteProfileText.Visible = false;
            CheckSaveHeaders(_slotArray[_selectedIndex], (byte) (_selectedIndex + 1));
        }
    }

    public override void Dispose()
    {
        if (IsDisposed == false)
        {
            //Console.WriteLine("Disposing Profile Select Screen");
            _title.Dispose();
            _title = null;

            _slot1Container.Dispose();
            _slot1Container = null;
            // _slot2Container.Dispose();
            // _slot2Container = null;
            // _slot3Container.Dispose();
            // _slot3Container = null;

            _slotArray.Clear();
            _slotArray = null;

            _selectedSlot = null;

            _confirmText.Dispose();
            _confirmText = null;
            _cancelText.Dispose();
            _cancelText = null;
            _navigationText.Dispose();
            _navigationText = null;
            _deleteProfileText.Dispose();
            _deleteProfileText = null;
            base.Dispose();
        }
    }

    public override void RefreshTextObjs()
    {
        Game.ChangeBitmapLanguage(_title, "ProfileSelectTitle_Sprite");
        // Game.ChangeBitmapLanguage(_slot1Container.GetChildAt(2) as SpriteObj, "ProfileSlot1Text_Sprite");
        // Game.ChangeBitmapLanguage(_slot2Container.GetChildAt(2) as SpriteObj, "ProfileSlot2Text_Sprite");
        // Game.ChangeBitmapLanguage(_slot3Container.GetChildAt(2) as SpriteObj, "ProfileSlot3Text_Sprite");

        // Update save slot text
        CheckSaveHeaders(_slot1Container, 1);
        // CheckSaveHeaders(_slot2Container, 2);
        // CheckSaveHeaders(_slot3Container, 3);

        base.RefreshTextObjs();
    }
}
