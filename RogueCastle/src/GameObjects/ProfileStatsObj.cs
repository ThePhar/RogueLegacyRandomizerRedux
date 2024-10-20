using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.GameStructs;
using RogueCastle.Managers;

namespace RogueCastle.GameObjects;

public class ProfileStatsObj : ObjContainer
{
    private TextObj _seedText;
    private TextObj _versionText;
    private TextObj _architectFeeText;
    private TextObj _gatekepperEnabledText;
    private TextObj[] _counts = new TextObj[5];
    private TextObj _money;

    private ObjContainer _skills;
    private ObjContainer[,] _blueprints = new ObjContainer[5, 15];
    private SpriteObj[,] _runes = new SpriteObj[5, 11];

    private SkillType[,] _skillArray =
    {
        { SkillType.HealthUp,     SkillType.ManaUp,       SkillType.AttackUp,     SkillType.MagicDamageUp, SkillType.EquipUp,    SkillType.ArmorUp,    SkillType.CritChanceUp, SkillType.CritDamageUp },
        { SkillType.DownStrikeUp, SkillType.InvulnTimeUp, SkillType.ManaCostDown, SkillType.PotionUp,      SkillType.GoldGainUp, SkillType.DeathDodge, SkillType.PricesDown,   SkillType.RandomizeChildren },
        { SkillType.KnightUp,     SkillType.BarbarianUp,  SkillType.MageUp,       SkillType.AssassinUp,    SkillType.BankerUp,   SkillType.NinjaUp,    SkillType.LichUp,       SkillType.SpellSwordUp },
        { SkillType.SuperSecret,  SkillType.Null,         SkillType.Null,         SkillType.Null,          SkillType.Null,       SkillType.Null,       SkillType.Null,         SkillType.Null},
    };

    public void LoadContent()
    {
        _seedText = new TextObj(Game.JunicodeFont)
        {
            FontSize = 12,
            OutlineWidth = 2,
            Text = "AP TEST",
        };

        _versionText = _seedText.Clone() as TextObj;
        _versionText!.Position = new Vector2(682, _seedText.Y);
        _versionText.Text = "(AP 0.5.1) RLR 2.0.0-dev";
        _versionText.Align = Types.TextAlign.Right;

        AddChild(_seedText);
        AddChild(_versionText);

        // Blueprints
        for (var row = 0; row < Game.PlayerStats.GetBlueprintArray.Count; row++)
        for (var bp = 0; bp < Game.PlayerStats.GetBlueprintArray[row].Length; bp++)
        {
            var position = new Vector2(610f, 354f);
            const float scale = 0.5f;
            const int offset = 4;

            var icon = new ObjContainer("BlacksmithUI_QuestionMarkIcon_Character")
            {
                Position = new Vector2(bp * (scale * 60 + offset), row * (scale * 60 + offset)) + position,
                Scale = new Vector2(scale),
                ForceDraw = true,
            };

            var state = Game.PlayerStats.GetBlueprintArray[row][bp];
            if (state > EquipmentState.NOT_FOUND)
            {
                icon.ChangeSprite($"BlacksmithUI_{EquipmentCategoryType.ToStringEN(row)}{((bp % 5) + 1)}Icon_Character");

                var equipmentData = Game.EquipmentSystem.GetEquipmentData(row, bp);
                var colorIndex = row == EquipmentCategoryType.SWORD ? 2 : 1;
                icon.GetChildAt(colorIndex).TextureColor = equipmentData.FirstColour;
                if (row != EquipmentCategoryType.CAPE)
                {
                    icon.GetChildAt(colorIndex + 1).TextureColor = equipmentData.SecondColour;
                }
            }

            _blueprints[row, bp] = icon;
            AddChild(icon);
        }

        // Runes
        for (var row = 0; row < Game.PlayerStats.GetRuneArray.Count; row++)
        for (var rune = 0; rune < Game.PlayerStats.GetRuneArray[row].Length; rune++)
        {
            var position = new Vector2(18f, 482f);
            const float scale = 0.5f;
            const int offset = 4;

            var icon = new SpriteObj("BlacksmithUI_QuestionMarkIcon_Sprite")
            {
                Position = new Vector2(rune * (scale * 60 + offset), row * (scale * 60 + offset)) + position,
                Scale = new Vector2(scale),
                ForceDraw = true,
            };

            var state = Game.PlayerStats.GetRuneArray[row][rune];
            if (state > EquipmentState.NOT_FOUND)
            {
                icon.ChangeSprite(EquipmentAbilityType.Icon(rune));
            }

            _runes[row, rune] = icon;
            AddChild(icon);
        }

        // Architect
        var architect = new SpriteObj("ArchitectBoduPull_Sprite")
        {
            Position = new Vector2(394f, 520f),
            OutlineWidth = 2,
            Anchor = Vector2.Zero,
        };
        _architectFeeText = new TextObj(Game.JunicodeFont)
        {
            FontSize = 12,
            OutlineWidth = 2,
            TextureColor = Color.Yellow,
            Align = Types.TextAlign.Centre,
            Text = "40%",
            Position = new Vector2(architect.X + 82, architect.Y + 8),
        };

        AddChild(architect);
        AddChild(_architectFeeText);

        var gatekeeper = new SpriteObj("NPCTollCollectorLaugh_Sprite")
        {
            Position = new Vector2(architect.Position.X, architect.Position.Y + 60),
            OutlineWidth = 2,
            Anchor = Vector2.Zero,
        };
        _gatekepperEnabledText = new TextObj(Game.JunicodeFont)
        {
            FontSize = 12,
            OutlineWidth = 2,
            TextureColor = Color.Red,
            Align = Types.TextAlign.Centre,
            Text = "100%",
            Position = new Vector2(gatekeeper.X + 82, gatekeeper.Y + 8),
        };

        AddChild(gatekeeper);
        AddChild(_gatekepperEnabledText);

        // Location Chekcs
        var chestBrown = new SpriteObj("Chest1_Sprite") { ForceDraw = true, Scale = new Vector2(0.5f) };
        var chestSilver = new SpriteObj("Chest2_Sprite") { ForceDraw = true, Scale = new Vector2(0.5f) };
        var chestGold = new SpriteObj("Chest3_Sprite") { ForceDraw = true, Scale = new Vector2(0.5f) };
        var chestFairy = new SpriteObj("Chest4_Sprite") { ForceDraw = true, Scale = new Vector2(0.5f) };
        var diaries = new SpriteObj("BonusRoomDiary_Sprite") { ForceDraw = true };

        chestBrown.Anchor = new Vector2(chestBrown.Width / 2, chestBrown.Height);
        chestSilver.Anchor = new Vector2(chestSilver.Width / 2, chestSilver.Height);
        chestGold.Anchor = new Vector2(chestGold.Width / 2, chestGold.Height);
        chestFairy.Anchor = new Vector2(chestFairy.Width / 2, chestFairy.Height);
        diaries.Anchor = new Vector2(diaries.Width / 2, diaries.Height);

        chestBrown.GoToFrame(2);
        chestSilver.GoToFrame(2);
        chestGold.GoToFrame(2);
        chestFairy.GoToFrame(2);

        var countsPosition = new Vector2(410, 80);
        diaries.Position = new Vector2(0, 0) + countsPosition;
        chestBrown.Position = new Vector2(60, 0) + countsPosition;
        chestSilver.Position = new Vector2(120, 0) + countsPosition;
        chestGold.Position = new Vector2(180, 0) + countsPosition;
        chestFairy.Position = new Vector2(240, 0) + countsPosition;

        AddChild(chestBrown);
        AddChild(chestSilver);
        AddChild(chestGold);
        AddChild(chestFairy);
        AddChild(diaries);

        _counts[0] = new TextObj(Game.JunicodeFont)
        {
            Text = "0/0",
            Align = Types.TextAlign.Centre,
            FontSize = 8,
            OutlineWidth = 2,
        };
        _counts[1] = _counts[0].Clone() as TextObj;
        _counts[2] = _counts[0].Clone() as TextObj;
        _counts[3] = _counts[0].Clone() as TextObj;
        _counts[4] = _counts[0].Clone() as TextObj;

        _counts[0].Position = diaries.Position + new Vector2(0, 8);
        _counts[1].Position = chestBrown.Position + new Vector2(0, 8);
        _counts[2].Position = chestSilver.Position + new Vector2(0, 8);
        _counts[3].Position = chestGold.Position + new Vector2(0, 8);
        _counts[4].Position = chestFairy.Position + new Vector2(0, 8);

        _counts[0].Text = "9/25";
        _counts[1].Text = "83/150";
        _counts[2].Text = "69/80";
        _counts[3].Text = "21/40";
        _counts[4].Text = "2/10";

        foreach (var chest in _counts)
        {
            AddChild(chest);
        }

        // Money
        var coin = new SpriteObj("CoinIcon_Sprite")
        {
            Anchor = Vector2.Zero,
            Position = _seedText.Position + new Vector2(0, 40),
        };
        _money = new TextObj(Game.JunicodeFont)
        {
            Position = coin.Position + new Vector2(40, 0),
            FontSize = 14,
            TextureColor = Color.Yellow,
            OutlineWidth = 2,
            Text = "49232",
        };

        AddChild(coin);
        AddChild(_money);

        // // Skills
        // _skills = new ObjContainer() { ForceDraw = true };
        // _skills.Position = new Vector2(593, 228);
        // _skills.Scale = new Vector2(0.75f, 0.75f);
        // for (var row = 0; row < _skillArray.GetLength(0); row++)
        // for (var col = 0; col < _skillArray.GetLength(1); col++)
        // {
        //     var skill = SkillSystem.GetSkill(_skillArray[row, col]);
        //     var obj = new SpriteObj(skill.SpriteName);
        //     obj.Anchor = Vector2.Zero;
        //     obj.Position = new Vector2((row * (60 + 16)), (col * (60 + 16)));
        //
        //     _skills.AddChild(obj);
        //     Console.WriteLine(obj.SpriteName);
        // }
        //
        // AddChild(_skills);
    }

    public void Blank()
    {
        for (var i = 0; i < NumChildren; i++)
        {
            GetChildAt(i).Visible = false;
        }
    }

    public void Update(ProfileSaveHeader header)
    {
        for (var i = 0; i < NumChildren; i++)
        {
            GetChildAt(i).Visible = true;
        }

        _seedText.Text = header.MultiWorld ? "AP " : "" + header.SeedName;
        _versionText.Text = $"(AP ${header.GeneratorVersion}) RLR {header.GameVersion}";
        _money.Text = $"{header.Gold}";
        _architectFeeText.Text = $"{header.ArchitectFee}%";
        _gatekepperEnabledText.Text = $"{header.CharonFee}%";
        _gatekepperEnabledText.TextureColor = header.CharonFee > 0
            ? Color.Red
            : Color.Green;

        // Blueprints
        for (var category = 0; category < header.Blueprints.Length; category++)
        for (var @base = 0; @base < header.Blueprints[category].Length; @base++)
        {
            var icon = _blueprints[category, @base];
            var state = header.Blueprints[category][@base];
            if (state > EquipmentState.NOT_FOUND)
            {
                icon.ChangeSprite($"BlacksmithUI_{EquipmentCategoryType.ToStringEN(category)}{(@base % 5) + 1}Icon_Character");

                var equipmentData = Game.EquipmentSystem.GetEquipmentData(category, @base);
                var colorIndex = category == EquipmentCategoryType.SWORD ? 2 : 1;
                icon.GetChildAt(colorIndex).TextureColor = equipmentData.FirstColour;
                if (category != EquipmentCategoryType.CAPE)
                {
                    icon.GetChildAt(colorIndex + 1).TextureColor = equipmentData.SecondColour;
                }
            }
            else
            {
                icon.ChangeSprite("BlacksmithUI_QuestionMarkIcon_Character");
            }
        }

        // Runes
        for (var category = 0; category < header.Runes.Length; category++)
        for (var rune = 0; rune < header.Runes[category].Length; rune++)
        {
            var icon = _runes[category, rune];
            var state = header.Runes[category][rune];
            icon.ChangeSprite(state > EquipmentState.NOT_FOUND
                ? EquipmentAbilityType.Icon(rune)
                : "BlacksmithUI_QuestionMarkIcon_Sprite");
        }

        for (var i = 0; i < header.LocationCounts.Length; i++)
        {
            var counts = header.LocationCounts[i];
            _counts[i].Text = $"{counts.Checked}/{counts.Total}";
        }
    }

    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        base.Dispose();
    }
}
