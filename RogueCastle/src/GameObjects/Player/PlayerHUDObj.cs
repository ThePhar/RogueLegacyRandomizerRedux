using System;
using Archipelago.MultiClient.Net.Models;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.EnvironmentVariables;
using RogueCastle.GameStructs;

namespace RogueCastle.GameObjects.Player;

public class PlayerHUDObj : SpriteObj {
    private const int MAX_BAR_LENGTH = 360;

    private SpriteObj[] _abilitiesSpriteArray;
    private SpriteObj _coin;
    private TextObj _goldText;
    private SpriteObj _hpBar;
    private ObjContainer _hpBarContainer;
    private TextObj _hpText;
    private SpriteObj _iconHolder1, _iconHolder2;
    private SpriteObj _mpBar;
    private ObjContainer _mpBarContainer;
    private TextObj _mpText;
    private TextObj _playerLevelText;
    private SpriteObj _specialItemIcon;
    private TextObj _spellCost;
    private SpriteObj _spellIcon;
    private ItemsReceivedHUD _itemsReceivedHUD;

    public PlayerHUDObj() : base("PlayerHUDLvlText_Sprite") {
        ForceDraw = true;
        ForcedPlayerLevel = -1;

        _playerLevelText = new TextObj();
        _playerLevelText.Text = Game.PlayerStats.CurrentLevel.ToString();
        _playerLevelText.Font = Game.PlayerLevelFont;

        _coin = new SpriteObj("PlayerUICoin_Sprite") { ForceDraw = true };
        _goldText = new TextObj {
            Text = "0",
            Font = Game.GoldFont,
            FontSize = 25,
        };
        _hpBar = new SpriteObj("HPBar_Sprite") { ForceDraw = true };
        _mpBar = new SpriteObj("MPBar_Sprite") { ForceDraw = true };
        _hpText = new TextObj(Game.JunicodeFont) {
            FontSize = 8,
            DropShadow = new Vector2(1, 1),
            ForceDraw = true,
        };
        _mpText = new TextObj(Game.JunicodeFont) {
            FontSize = 8,
            DropShadow = new Vector2(1, 1),
            ForceDraw = true,
        };

        _abilitiesSpriteArray = new SpriteObj[5]; // Can only have 5 abilities equipped at a time.
        var startPos = new Vector2(130, 690);
        const int xOffset = 35;
        for (var i = 0; i < _abilitiesSpriteArray.Length; i++) {
            _abilitiesSpriteArray[i] = new SpriteObj("Blank_Sprite") {
                ForceDraw = true,
                Position = startPos,
                Scale = new Vector2(0.5f, 0.5f),
            };
            startPos.X += xOffset;
        }

        _hpBarContainer = new ObjContainer("PlayerHUDHPBar_Character") { ForceDraw = true };
        _mpBarContainer = new ObjContainer("PlayerHUDMPBar_Character") { ForceDraw = true };
        _itemsReceivedHUD = new ItemsReceivedHUD { ForceDraw = true };
        _specialItemIcon = new SpriteObj("Blank_Sprite") {
            ForceDraw = true,
            OutlineWidth = 1,
            Scale = new Vector2(1.7f, 1.7f),
            Visible = false,
        };
        _spellIcon = new SpriteObj(SpellEV.Icon(SpellType.NONE)) {
            ForceDraw = true,
            OutlineWidth = 1,
            Visible = false,
        };
        _iconHolder1 = new SpriteObj("BlacksmithUI_IconBG_Sprite") {
            ForceDraw = true,
            Opacity = 0.5f,
            Scale = new Vector2(0.8f, 0.8f),
        };
        _iconHolder2 = _iconHolder1.Clone() as SpriteObj;

        _spellCost = new TextObj(Game.JunicodeFont) {
            Align = Types.TextAlign.Centre,
            ForceDraw = true,
            OutlineWidth = 2,
            FontSize = 8,
            Visible = false,
        };

        UpdateSpecialItemIcon();
        UpdateSpellIcon();
    }

    public bool ShowBarsOnly { get; set; }

    public int ForcedPlayerLevel { get; set; }

    public void SetPosition(Vector2 position) {
        SpriteObj mpBar, hpBar;
        ObjContainer mpContainer, hpContainer;

        if (Game.PlayerStats.HasTrait(TraitType.DEXTROCARDIA)) {
            mpBar = _hpBar;
            hpBar = _mpBar;
            mpContainer = _hpBarContainer;
            hpContainer = _mpBarContainer;
        } else {
            mpBar = _mpBar;
            hpBar = _hpBar;
            mpContainer = _mpBarContainer;
            hpContainer = _hpBarContainer;
        }

        Position = position;
        mpBar.Position = new Vector2(X + 7, Y + 60);
        hpBar.Position = new Vector2(X + 8, Y + 29);
        _playerLevelText.Position = new Vector2(X + 30, Y - 20);

        if (Game.PlayerStats.HasTrait(TraitType.DEXTROCARDIA)) {
            _mpText.Position = new Vector2(X + 5, Y + 16);
            _mpText.X += 8;

            _hpText.Position = _mpText.Position;
            _hpText.Y += 28;
        } else {
            _hpText.Position = new Vector2(X + 5, Y + 16);
            _hpText.X += 8;
            _hpText.Y += 5;

            _mpText.Position = _hpText.Position;
            _mpText.Y += 30;
        }

        hpContainer.Position = new Vector2(X, Y + 17);
        hpBar.Position = hpBar == _hpBar
            ? new Vector2(hpContainer.X + 2, hpContainer.Y + 7)
            : new Vector2(hpContainer.X + 2, hpContainer.Y + 6); // Small hack to properly align dextrocardia

        mpContainer.Position = new Vector2(X, hpContainer.Bounds.Bottom);
        mpBar.Position = mpBar == _mpBar
            ? new Vector2(mpContainer.X + 2, mpContainer.Y + 6)
            : new Vector2(mpContainer.X + 2, mpContainer.Y + 7); // Small hack to properly align dextrocardia

        _coin.Position = new Vector2(X, mpContainer.Bounds.Bottom + 2);
        _goldText.Position = new Vector2(_coin.X + 28, _coin.Y - 2);
        _iconHolder1.Position = new Vector2(_coin.X + 25, _coin.Y + 60);
        _iconHolder2.Position = new Vector2(_iconHolder1.X + 55, _iconHolder1.Y);
        _spellIcon.Position = _iconHolder1.Position;
        _specialItemIcon.Position = _iconHolder2.Position;
        _spellCost.Position = new Vector2(_spellIcon.X, _spellIcon.Bounds.Bottom + 10);

        // Received items view goes on the other side, just below where the mini map would be.
        _itemsReceivedHUD.Position = new Vector2(1080, 84);
        _itemsReceivedHUD.AnchorX = 200;
    }

    public void Update(PlayerObj player) {
        var playerLevel = Game.PlayerStats.CurrentLevel;
        if (playerLevel < 0) {
            playerLevel = 0;
        }

        if (ForcedPlayerLevel >= 0) {
            playerLevel = ForcedPlayerLevel;
        }

        _playerLevelText.Text = playerLevel.ToString();

        var playerGold = Game.PlayerStats.Gold;
        if (playerGold < 0) {
            playerGold = 0;
        }

        _goldText.Text = playerGold.ToString();

        _hpText.Text = player.CurrentHealth + "/" + player.MaxHealth;
        _mpText.Text = player.CurrentMana + "/" + player.MaxMana;

        UpdatePlayerHP(player);
        UpdatePlayerMP(player);

        _itemsReceivedHUD.Update();
    }

    private void UpdatePlayerHP(PlayerObj player) {
        // Each piece is 32 pixels in width.
        // Total bar length is 88;
        var hpBarIncreaseAmount = player.MaxHealth - player.BaseHealth; // The amount of bonus HP the player has compared to his base health.
        var hpPercent = player.CurrentHealth / (float)player.MaxHealth; // The current percent of health player has compared to his max health.

        var hpBarIncreaseWidth = (int)(88 + (hpBarIncreaseAmount / 5f));
        if (hpBarIncreaseWidth > MAX_BAR_LENGTH) {
            hpBarIncreaseWidth = MAX_BAR_LENGTH;
        }

        var midBarScaleX = (hpBarIncreaseWidth - 28 - 28) / 32f;
        _hpBarContainer.GetChildAt(1).ScaleX = midBarScaleX;
        _hpBarContainer.GetChildAt(2).X = _hpBarContainer.GetChildAt(1).Bounds.Right;
        _hpBarContainer.CalculateBounds();

        _hpBar.ScaleX = 1;
        _hpBar.ScaleX = (_hpBarContainer.Width - 8) / (float)_hpBar.Width * hpPercent;
    }

    private void UpdatePlayerMP(PlayerObj player) {
        var mpBarIncreaseAmount = (int)(player.MaxMana - player.BaseMana);
        var mpPercent = player.CurrentMana / player.MaxMana;

        var mpBarIncreaseWidth = (int)(88 + (mpBarIncreaseAmount / 5f));
        if (mpBarIncreaseWidth > MAX_BAR_LENGTH) {
            mpBarIncreaseWidth = MAX_BAR_LENGTH;
        }

        var midBarScaleX = (mpBarIncreaseWidth - 28 - 28) / 32f;
        _mpBarContainer.GetChildAt(1).ScaleX = midBarScaleX;
        _mpBarContainer.GetChildAt(2).X = _mpBarContainer.GetChildAt(1).Bounds.Right;
        _mpBarContainer.CalculateBounds();

        _mpBar.ScaleX = 1;
        _mpBar.ScaleX = (_mpBarContainer.Width - 8) / (float)_mpBar.Width * mpPercent;
    }

    public void UpdatePlayerLevel() {
        _playerLevelText.Text = Game.PlayerStats.CurrentLevel.ToString();
    }

    public void UpdateAbilityIcons() {
        foreach (var sprite in _abilitiesSpriteArray) {
            sprite.ChangeSprite("Blank_Sprite"); // Zeroing out each sprite.
        }

        var spriteArrayIndex = 0;
        foreach (var index in Game.PlayerStats.GetEquippedRuneArray) {
            if (index == -1) {
                continue;
            }

            _abilitiesSpriteArray[spriteArrayIndex].ChangeSprite(EquipmentAbilityType.Icon(index));
            spriteArrayIndex++;
        }
    }

    public void UpdateSpecialItemIcon() {
        _specialItemIcon.Visible = false;
        _iconHolder2.Opacity = 0.5f;
        if (Game.PlayerStats.SpecialItem == SpecialItemType.NONE) {
            return;
        }

        _specialItemIcon.Visible = true;
        _specialItemIcon.ChangeSprite(SpecialItemType.SpriteName(Game.PlayerStats.SpecialItem));
        _iconHolder2.Opacity = 1;
    }

    public void UpdateSpellIcon() {
        _spellIcon.Visible = false;
        _iconHolder1.Opacity = 0.5f;
        _spellCost.Visible = false;

        if (Game.PlayerStats.Spell == SpellType.NONE) {
            return;
        }

        _spellIcon.ChangeSprite(SpellEV.Icon(Game.PlayerStats.Spell));
        _spellIcon.Visible = true;
        _iconHolder1.Opacity = 1;
        _spellCost.ChangeFontNoDefault(_spellCost.GetLanguageFont());
        _spellCost.Text = (int)(SpellEV.GetManaCost(Game.PlayerStats.Spell) * (1 - SkillSystem.GetSkill(SkillType.ManaCostDown).ModifierAmount)) + " " + "LOC_ID_SKILL_SCREEN_15".GetString(null);
        _spellCost.Visible = true;
    }

    public void AddReceivedItem(int type, ItemInfo item, int sender, params object[] args) {
        _itemsReceivedHUD.Elements.Add(new ItemsReceivedElement(_itemsReceivedHUD, type, item, sender, args));
    }

    public override void Draw(Camera2D camera) {
        if (!Visible) {
            return;
        }

        if (ShowBarsOnly == false) {
            base.Draw(camera);
            _coin.Draw(camera);

            _playerLevelText.Draw(camera);
            _goldText.Draw(camera);

            camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            foreach (var sprite in _abilitiesSpriteArray) {
                sprite.Draw(camera);
            }

            _iconHolder1.Draw(camera);
            _iconHolder2.Draw(camera);
            _spellIcon.Draw(camera);
            _specialItemIcon.Draw(camera);

            camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            _spellCost.Draw(camera);
        }

        _mpBar.Draw(camera);
        _mpText.Draw(camera);
        if (!Game.PlayerStats.HasTrait(TraitType.CIP)) {
            _hpBar.Draw(camera);
            _hpText.Draw(camera);
        }

        _mpBarContainer.Draw(camera);
        _hpBarContainer.Draw(camera);
        _itemsReceivedHUD.Draw(camera);
    }

    public void RefreshTextObjs() {
        if (Game.PlayerStats.Spell == SpellType.NONE) {
            return;
        }

        _spellCost.ChangeFontNoDefault(_spellCost.GetLanguageFont());
        _spellCost.Text = (int)(SpellEV.GetManaCost(Game.PlayerStats.Spell) * (1 - SkillSystem.GetSkill(SkillType.ManaCostDown).ModifierAmount)) + " " + "LOC_ID_SKILL_SCREEN_15".GetString(null);
    }

    public override void Dispose() {
        if (IsDisposed) {
            return;
        }

        foreach (var sprite in _abilitiesSpriteArray) {
            sprite.Dispose();
        }

        Array.Clear(_abilitiesSpriteArray, 0, _abilitiesSpriteArray.Length);
        _abilitiesSpriteArray = null;

        _coin.Dispose();
        _coin = null;
        _mpBar.Dispose();
        _mpBar = null;
        _hpBar.Dispose();
        _hpBar = null;
        _playerLevelText.Dispose();
        _playerLevelText = null;
        _goldText.Dispose();
        _goldText = null;
        _hpText.Dispose();
        _hpText = null;
        _mpText.Dispose();
        _mpText = null;
        _hpBarContainer.Dispose();
        _hpBarContainer = null;
        _mpBarContainer.Dispose();
        _mpBarContainer = null;
        _specialItemIcon.Dispose();
        _specialItemIcon = null;
        _spellIcon.Dispose();
        _spellIcon = null;
        _spellCost.Dispose();
        _spellCost = null;
        _iconHolder1.Dispose();
        _iconHolder1 = null;
        _iconHolder2.Dispose();
        _iconHolder2 = null;
        _itemsReceivedHUD.Dispose();
        _itemsReceivedHUD = null;
        base.Dispose();
    }
}
