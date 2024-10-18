using System.Collections.Generic;
using System.Text.RegularExpressions;
using DS2DEngine;
using InputSystem;
using Microsoft.Xna.Framework;
using RogueCastle.EnvironmentVariables;
using RogueCastle.GameStructs;
using RogueCastle.Managers;
using RogueCastle.Screens.BaseScreens;
using Tweener;
using Tweener.Ease;

namespace RogueCastle.Screens;

public class ProfileSelectScreen : Screen
{
    private const int SlotCount = 3;

    private KeyIconTextObj _cancelText;
    private KeyIconTextObj _confirmText;
    private KeyIconTextObj _deleteProfileText;
    private bool _lockControls;
    private KeyIconTextObj _navigationText;
    private int _selectedIndex;
    private ObjContainer _selectedSlot;
    private List<ObjContainer> _slotArray = [];
    private SpriteObj _title;

    public ProfileSelectScreen()
    {
        DrawIfCovered = true;
    }

    public float BackBufferOpacity { get; set; }

    public override void LoadContent()
    {
        _title = new SpriteObj("ProfileSelectTitle_Sprite") { ForceDraw = true };

        // Template for slot text
        var slotText = new TextObj(Game.JunicodeFont) {
            Align = Types.TextAlign.Centre,
            TextureColor = Color.White,
            OutlineWidth = 2,
            FontSize = 10,
        };
        slotText.Text = "LOC_ID_PROFILE_SEL_SCREEN_1".GetString(slotText);
        slotText.Position = new Vector2(0, -(slotText.Height / 2f));

        for (var i = 0; i < SlotCount; i++)
        {
            var iSlotContainer = new ObjContainer("ProfileSlotBG_Container") { ForceDraw = true };
            var iSlotText = slotText.Clone() as TextObj;
            var iSlotLvlText = slotText.Clone() as TextObj;
            var iSlot1NGText = slotText.Clone() as TextObj;
            var iSlotTitle = new SpriteObj($"ProfileSlot{i + 1}Text_Sprite") { Position = new Vector2(-130, 35) };

            iSlotText!.Text = "LOC_ID_CLASS_NAME_1_MALE".GetString(iSlotText);
            iSlotLvlText!.Text = "LOC_ID_CLASS_NAME_1_MALE".GetString(iSlotLvlText);
            iSlot1NGText!.Text = "LOC_ID_CLASS_NAME_1_MALE".GetString(iSlot1NGText);

            iSlotLvlText.Position = new Vector2(120, 15);
            iSlot1NGText.Position = new Vector2(-120, 15);

            iSlotContainer.AddChild(iSlotText);
            iSlotContainer.AddChild(iSlotTitle);
            iSlotContainer.AddChild(iSlotLvlText);
            iSlotContainer.AddChild(iSlot1NGText);

            _slotArray.Add(iSlotContainer);
        }

        _confirmText = new KeyIconTextObj(Game.JunicodeFont);
        _confirmText.Text = "LOC_ID_PROFILE_SEL_SCREEN_4".GetString(_confirmText);
        _confirmText.DropShadow = new Vector2(2, 2);
        _confirmText.FontSize = 12;
        _confirmText.Align = Types.TextAlign.Right;
        _confirmText.Position = new Vector2(1290, 570);
        _confirmText.ForceDraw = true;

        _cancelText = new KeyIconTextObj(Game.JunicodeFont);
        _cancelText.Text = "LOC_ID_PROFILE_SEL_SCREEN_5".GetString(_cancelText);
        _cancelText.Align = Types.TextAlign.Right;
        _cancelText.DropShadow = new Vector2(2, 2);
        _cancelText.FontSize = 12;
        _cancelText.Position = new Vector2(_confirmText.X, _confirmText.Y + 40);
        _cancelText.ForceDraw = true;

        _navigationText = new KeyIconTextObj(Game.JunicodeFont);
        _navigationText.Text = "LOC_ID_PROFILE_SEL_SCREEN_2".GetString(_navigationText);
        _navigationText.Align = Types.TextAlign.Right;
        _navigationText.DropShadow = new Vector2(2, 2);
        _navigationText.FontSize = 12;
        _navigationText.Position = new Vector2(_confirmText.X, _confirmText.Y + 80);
        _navigationText.ForceDraw = true;

        _deleteProfileText = new KeyIconTextObj(Game.JunicodeFont);
        _deleteProfileText.Text = "LOC_ID_PROFILE_SEL_SCREEN_6".GetString(_deleteProfileText);
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

        for (var i = 0; i < SlotCount; i++)
        {
            CheckSaveHeaders(_slotArray[i], (byte)(i + 1));
        }

        _deleteProfileText.Visible = true;
        if (_slotArray[_selectedIndex].ID == 0)
        {
            _deleteProfileText.Visible = false;
        }

        Tween.To(this, 0.2f, Tween.EaseNone, "BackBufferOpacity", "0.9");

        _title.Position = new Vector2(1320 / 2f, 100);
        TweenInText(_title, 0);

        for (var i = 0; i < SlotCount; i++)
        {
            _slotArray[i].Position = new Vector2(1320 / 2f, 300 + i * 120);
            TweenInText(_slotArray[i], 0.05f * (i + 1));
        }

        Tween.RunFunction(0.5f, this, "UnlockControls");

        if (InputManager.GamePadIsConnected(PlayerIndex.One))
        {
            _confirmText.ForcedScale = new Vector2(0.7f, 0.7f);
            _cancelText.ForcedScale = new Vector2(0.7f, 0.7f);
            _navigationText.Text = "LOC_ID_PROFILE_SEL_SCREEN_2_NEW".GetString(_navigationText);
        }
        else
        {
            _confirmText.ForcedScale = new Vector2(1f, 1f);
            _cancelText.ForcedScale = new Vector2(1f, 1f);
            _navigationText.Text = "LOC_ID_PROFILE_SEL_SCREEN_3".GetString(_navigationText);
        }

        _confirmText.Text = "LOC_ID_PROFILE_SEL_SCREEN_4_NEW".GetString(_confirmText);
        _cancelText.Text = "LOC_ID_PROFILE_SEL_SCREEN_5_NEW".GetString(_cancelText);
        _deleteProfileText.Text = "LOC_ID_PROFILE_SEL_SCREEN_6_NEW".GetString(_deleteProfileText);

        _confirmText.Opacity = 0;
        _cancelText.Opacity = 0;
        _navigationText.Opacity = 0;
        _deleteProfileText.Opacity = 0;

        Tween.To(_confirmText, 0.2f, Tween.EaseNone, "Opacity", "1");
        Tween.To(_cancelText, 0.2f, Tween.EaseNone, "Opacity", "1");
        Tween.To(_navigationText, 0.2f, Tween.EaseNone, "Opacity", "1");
        Tween.To(_deleteProfileText, 0.2f, Tween.EaseNone, "Opacity", "1");

        Game.ChangeBitmapLanguage(_title, "ProfileSelectTitle_Sprite");
        for (var i = 0; i < SlotCount; i++)
        {
            Game.ChangeBitmapLanguage(_slotArray[i].GetChildAt(2) as SpriteObj, $"ProfileSlot{i + 1}Text_Sprite");
        }

        base.OnEnter();
    }

    private void CheckSaveHeaders(ObjContainer container, byte profile)
    {
        var slotText = container.GetChildAt(1) as TextObj;
        var slotLvlText = container.GetChildAt(3) as TextObj;
        var slotNGText = container.GetChildAt(4) as TextObj;
        slotLvlText!.Text = "";
        slotNGText!.Text = "";

        try
        {
            (ScreenManager.Game as Game)!.SaveManager.GetSaveHeader(
                profile,
                out var playerClass,
                out var playerName,
                out var playerLevel,
                out var isDead,
                out var timesCastleBeaten,
                out var isFemale);

            if (playerName == null)
            {
                slotText!.Text = "LOC_ID_PROFILE_SEL_SCREEN_1".GetResourceString();
                container.ID = 0; // Container with ID == 0 means it has no save file.
            }
            else
            {
                // This call to Game.NameHelper forces a name conversion check every time.  This is necessary because it is possible for different profile slots to have different
                // save revision numbers.  So you have to do the check every time in case that happens only in this scenario (since the check can be a little expensive).
                playerName = Game.NameHelper(playerName, "", isFemale, true);

                try
                {
                    slotText!.ChangeFontNoDefault(slotText.GetLanguageFont());
                    if (isDead == false)
                    {
                        slotText.Text = !isFemale
                            ? "LOC_ID_PROFILE_SEL_SCREEN_7_MALE_NEW".FormatResourceString(playerName, ClassType.ToStringID(playerClass, false).GetResourceString())
                            : "LOC_ID_PROFILE_SEL_SCREEN_7_FEMALE_NEW".FormatResourceString(playerName, ClassType.ToStringID(playerClass, true).GetResourceString());
                    }
                    else
                    {
                        slotText.Text = !isFemale
                            ? "LOC_ID_PROFILE_SEL_SCREEN_8_MALE_NEW".FormatResourceString(playerName)
                            : "LOC_ID_PROFILE_SEL_SCREEN_8_FEMALE_NEW".FormatResourceString(playerName);
                    }

                    if (LocaleBuilder.LanguageType != LanguageType.ChineseSimple && Regex.IsMatch(slotText.Text, @"\p{IsCyrillic}"))
                    {
                        slotText.ChangeFontNoDefault(Game.RobotoSlabFont);
                    }
                }
                catch
                {
                    slotText!.ChangeFontNoDefault(Game.NotoSansSCFont);
                    if (isDead == false)
                    {
                        slotText.Text = !isFemale
                            ? "LOC_ID_PROFILE_SEL_SCREEN_7_MALE_NEW".FormatResourceString(playerName, ClassType.ToStringID(playerClass, false).GetResourceString())
                            : "LOC_ID_PROFILE_SEL_SCREEN_7_FEMALE_NEW".FormatResourceString(playerName, ClassType.ToStringID(playerClass, true).GetResourceString());
                    }
                    else
                    {
                        slotText.Text = !isFemale
                            ? "LOC_ID_PROFILE_SEL_SCREEN_8_MALE_NEW".FormatResourceString(playerName)
                            : "LOC_ID_PROFILE_SEL_SCREEN_8_FEMALE_NEW".FormatResourceString(playerName);
                    }
                }

                slotLvlText.Text = $"{"LOC_ID_PROFILE_SEL_SCREEN_9".GetResourceString()} {playerLevel}";
                if (timesCastleBeaten > 0)
                {
                    slotNGText.Text = $"{"LOC_ID_PROFILE_SEL_SCREEN_10".GetResourceString()} {timesCastleBeaten}";
                }

                container.ID = 1; // Container with ID == 1 means it has a save file.
            }
        }
        catch
        {
            slotText!.Text = "LOC_ID_PROFILE_SEL_SCREEN_1".GetString(slotText);
            container.ID = 0; // Container with ID == 0 means it has no save file.
        }
    }

    public void UnlockControls()
    {
        _lockControls = false;
    }

    private static void TweenInText(GameObj obj, float delay)
    {
        obj.Opacity = 0;
        obj.Y -= 50;
        Tween.To(obj, 0.5f, Tween.EaseNone, "delay", $"{delay}", "Opacity", "1");
        Tween.By(obj, 0.5f, Quad.EaseOut, "delay", $"{delay}", "Y", "50");
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
        for (var i = 0; i < SlotCount; i++)
        {
            TweenOutText(_slotArray[i], 0.05f * (i + 1));
        }

        Tween.To(this, 0.2f, Tween.EaseNone, "delay", "0.5", "BackBufferOpacity", "0");
        Tween.AddEndHandlerToLastTween(ScreenManager, "HideCurrentScreen");
    }

    private static void TweenOutText(GameObj obj, float delay)
    {
        Tween.To(obj, 0.5f, Tween.EaseNone, "delay", $"{delay}", "Opacity", "0");
        Tween.By(obj, 0.5f, Quad.EaseInOut, "delay", $"{delay}", "Y", "-50");
    }

    public override void OnExit()
    {
        for (var i = 0; i < SlotCount; i++)
        {
            _slotArray[i].TextureColor = Color.White;
        }

        _lockControls = false;
        base.OnExit();
    }

    public override void HandleInput()
    {
        if (_lockControls == false)
        {
            var selectedSlot = _selectedSlot;

            if (Game.GlobalInput.PressedDown())
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

            if (Game.GlobalInput.PressedUp())
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

            if (Game.GlobalInput.PressedCancel())
            {
                ExitTransition();
            }

            if (Game.GlobalInput.PressedConfirm())
            {
                SoundManager.PlaySound("Map_On");

                Game.GameConfig.ProfileSlot = (byte)(_selectedIndex + 1);
                var game = ScreenManager.Game as Game;
                var manager = ScreenManager as RCScreenManager;
                Game.SaveConfig();

                if (game!.SaveManager.FileExists(SaveType.PlayerData))
                {
                    manager!.DisplayScreen(ScreenType.TITLE, true);
                }
                else
                {
                    SkillSystem.ResetAllTraits();
                    Game.PlayerStats.Dispose();
                    Game.PlayerStats = new PlayerStats();
                    manager!.Player.Reset();
                    Game.ScreenManager.Player.CurrentHealth = Game.PlayerStats.CurrentHealth;
                    Game.ScreenManager.Player.CurrentMana = Game.PlayerStats.CurrentMana;

                    ExitTransition();
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
        Camera.Draw(Game.GenericTexture, new Rectangle(0, 0, GlobalEV.SCREEN_WIDTH, GlobalEV.SCREEN_HEIGHT), Color.Black * BackBufferOpacity);

        _title.Draw(Camera);
        for (var i = 0; i < SlotCount; i++)
        {
            _slotArray[i].Draw(Camera);
        }

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
        manager!.DialogueScreen.SetDialogue("Delete Save");
        manager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
        manager.DialogueScreen.SetConfirmEndHandler(this, "DeleteSaveAskAgain");
        manager.DisplayScreen(ScreenType.DIALOGUE, false);
    }

    public void DeleteSaveAskAgain()
    {
        var manager = ScreenManager as RCScreenManager;
        manager!.DialogueScreen.SetDialogue("Delete Save2");
        manager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
        manager.DialogueScreen.SetConfirmEndHandler(this, "DeleteSave");
        manager.DisplayScreen(ScreenType.DIALOGUE, false);
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
        Game.GameConfig.ProfileSlot = (byte)(_selectedIndex + 1);
        var game = ScreenManager.Game as Game;
        var manager = ScreenManager as RCScreenManager;
        //game.SaveConfig();

        game!.SaveManager.ClearAllFileTypes(false);
        game.SaveManager.ClearAllFileTypes(true);

        // Reverting profile slot back to stored slot.
        Game.GameConfig.ProfileSlot = storedProfile;

        // Tutorial is bypassed in Rando, so we just use this to clear all values.
        if (runTutorial)
        {
            Game.PlayerStats.Dispose();
            SkillSystem.ResetAllTraits();
            Game.PlayerStats = new PlayerStats();
            manager!.Player.Reset();

            ExitTransition();
        }
        else
        {
            _deleteProfileText.Visible = false;
            CheckSaveHeaders(_slotArray[_selectedIndex], (byte)(_selectedIndex + 1));
        }
    }

    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        _title.Dispose();
        _title = null;

        foreach (var container in _slotArray)
        {
            container.Dispose();
        }

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

    public override void RefreshTextObjs()
    {
        Game.ChangeBitmapLanguage(_title, "ProfileSelectTitle_Sprite");
        for (var i = 0; i < SlotCount; i++)
        {
            Game.ChangeBitmapLanguage(_slotArray[i].GetChildAt(2) as SpriteObj, $"ProfileSlot{i + 1}Text_Sprite");
            CheckSaveHeaders(_slotArray[i], (byte)(i + 1));
        }

        base.RefreshTextObjs();
    }
}
