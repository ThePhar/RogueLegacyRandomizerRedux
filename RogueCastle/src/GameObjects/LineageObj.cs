using System;
using System.Text.RegularExpressions;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.EVs;
using RogueCastle.Screens;
using Tweener;
using Tweener.Ease;

namespace RogueCastle;

public class LineageObj : ObjContainer // An object specific to the lineage screen, represents a single individual in a line of lineage.
{
    public byte Age = 30;

    public  bool    BeatenABoss = false;
    public  byte    ChildAge    = 4;
    public  byte    Class;
    public  bool    FlipPortrait;
    public  bool    IsFemale;
    private TextObj m_ageText;
    private TextObj m_classTextObj;

    private bool m_frameDropping;

    private SpriteObj m_frameSprite;

    private bool m_hasProsopagnosia;

    private bool         m_isDead;
    private Color        m_lichColour1 = new(255, 255, 255, 255);
    private Color        m_lichColour2 = new(198, 198, 198, 255);
    private SpriteObj    m_plaqueSprite;
    private TextObj      m_playerName;
    private string       m_playerNameString = "";
    private ObjContainer m_playerSprite;

    private Color m_skinColour1 = new(231, 175, 131, 255);
    private Color m_skinColour2 = new(199, 109, 112, 255);

    private SpriteObj m_spellIcon;
    private SpriteObj m_spellIconHolder;

    private readonly int     m_textYPos = 140;
    private          TextObj m_trait1Title;
    private          TextObj m_trait2Title;
    public           int     NumEnemiesKilled = 0;
    public           string  RomanNumeral     = "";
    public           byte    Spell;

    public LineageObj(LineageScreen screen, bool createEmpty = false)
    {
        Name = "";

        m_frameSprite = new SpriteObj("LineageScreenFrame_Sprite");
        //m_frameSprite.ForceDraw = true;
        m_frameSprite.Scale = new Vector2(2.8f, 2.8f);
        m_frameSprite.DropShadow = new Vector2(4, 6);

        m_plaqueSprite = new SpriteObj("LineageScreenPlaque1Long_Sprite");
        //m_plaqueSprite.ForceDraw = true;
        m_plaqueSprite.Scale = new Vector2(1.8f, 2);

        m_playerSprite = new ObjContainer("PlayerIdle_Character");
        //m_playerSprite.ForceDraw = true;
        //m_playerSprite.PlayAnimation(true);
        m_playerSprite.AnimationDelay = 1 / 10f;
        m_playerSprite.Scale = new Vector2(2, 2);
        m_playerSprite.OutlineWidth = 2;
        m_playerSprite.GetChildAt(PlayerPart.Sword1).Visible = false;
        m_playerSprite.GetChildAt(PlayerPart.Sword2).Visible = false;

        m_playerSprite.GetChildAt(PlayerPart.Cape).TextureColor = Color.Red;
        m_playerSprite.GetChildAt(PlayerPart.Hair).TextureColor = Color.Red;
        m_playerSprite.GetChildAt(PlayerPart.Glasses).Visible = false;
        m_playerSprite.GetChildAt(PlayerPart.Light).Visible = false;

        var darkPink = new Color(251, 156, 172);
        m_playerSprite.GetChildAt(PlayerPart.Bowtie).TextureColor = darkPink;

        m_playerName = new TextObj(Game.JunicodeFont);
        m_playerName.FontSize = 10;
        m_playerName.Text = "Sir Skunky IV";
        m_playerName.Align = Types.TextAlign.Centre;
        m_playerName.OutlineColour = new Color(181, 142, 39);
        m_playerName.OutlineWidth = 2;
        m_playerName.Y = m_textYPos;
        m_playerName.LimitCorners = true;
        m_playerName.X = 5;
        AddChild(m_playerName);

        m_classTextObj = new TextObj(Game.JunicodeFont);
        m_classTextObj.FontSize = 8;
        m_classTextObj.Align = Types.TextAlign.Centre;
        m_classTextObj.OutlineColour = new Color(181, 142, 39);
        m_classTextObj.OutlineWidth = 2;
        m_classTextObj.Text = LocaleBuilder.getString("LOC_ID_CLASS_NAME_1_MALE", m_classTextObj); // dummy locID to add TextObj to language refresh list
        m_classTextObj.Y = m_playerName.Y + m_playerName.Height - 8;
        m_classTextObj.LimitCorners = true;
        m_classTextObj.X = 5;
        AddChild(m_classTextObj);

        m_trait1Title = new TextObj(Game.JunicodeFont);
        m_trait1Title.FontSize = 8;
        m_trait1Title.Align = Types.TextAlign.Centre;
        m_trait1Title.OutlineColour = new Color(181, 142, 39);
        m_trait1Title.OutlineWidth = 2;
        m_trait1Title.X = 5;
        m_trait1Title.Y = m_classTextObj.Y + m_classTextObj.Height + 2;
        m_trait1Title.Text = LocaleBuilder.getString("LOC_ID_CLASS_NAME_1_MALE", m_trait1Title); // dummy locID to add TextObj to language refresh list
        m_trait1Title.LimitCorners = true;
        AddChild(m_trait1Title);

        m_trait2Title = m_trait1Title.Clone() as TextObj;
        m_trait2Title.Y += 20;
        m_trait2Title.Text = "";
        m_trait2Title.LimitCorners = true;
        AddChild(m_trait2Title);

        m_ageText = m_trait1Title.Clone() as TextObj;
        m_ageText.Text = "xxx - xxx";
        m_ageText.Visible = false;
        m_ageText.LimitCorners = true;
        AddChild(m_ageText);

        m_spellIcon = new SpriteObj("Blank_Sprite");
        m_spellIcon.OutlineWidth = 1;

        m_spellIconHolder = new SpriteObj("BlacksmithUI_IconBG_Sprite");

        if (createEmpty == false)
        {
            // Setting gender.
            IsFemale = false;
            if (CDGMath.RandomInt(0, 1) > 0)
            {
                IsFemale = true;
            }

            // Creating random name.
            if (IsFemale)
            {
                RomanNumeral = CreateFemaleName(screen);
            }
            else
            {
                RomanNumeral = CreateMaleName(screen);
            }

            // Selecting random traits.
            Traits = TraitType.CreateRandomTraits();

            // Selecting random class.
            Class = ClassType.GetRandomClass();

            var classText = "";
            if (LocaleBuilder.languageType == LanguageType.English)
            {
                classText += LocaleBuilder.getResourceString("LOC_ID_LINEAGE_OBJ_2_MALE", true);
                classText += " ";
                //m_classTextObj.Text = LocaleBuilder.getResourceString(this.IsFemale ? "LOC_ID_LINEAGE_OBJ_2_FEMALE" : "LOC_ID_LINEAGE_OBJ_2_MALE") +
                //     " " + LocaleBuilder.getResourceString(ClassType.ToStringID(this.Class, this.IsFemale));
            }

            m_classTextObj.Text = classText + LocaleBuilder.getResourceString(ClassType.ToStringID(Class, IsFemale));

            // Special check to make sure lich doesn't get dextrocardia.
            while ((Class == ClassType.Lich || Class == ClassType.Lich2) && (Traits.X == TraitType.Dextrocardia || Traits.Y == TraitType.Dextrocardia))
            {
                Traits = TraitType.CreateRandomTraits();
            }

            // Special check to make sure wizard don't get savantism.
            while ((Class == ClassType.Wizard || Class == ClassType.Wizard2 || Class == ClassType.Dragon) && (Traits.X == TraitType.Savant || Traits.Y == TraitType.Savant))
            {
                Traits = TraitType.CreateRandomTraits();
            }

            // Selecting random spell.  There's a check to make sure savants don't get particular spells.
            var spellList = ClassType.GetSpellList(Class);
            do
            {
                Spell = spellList[CDGMath.RandomInt(0, spellList.Length - 1)];
            } while ((Spell == SpellType.DamageShield || Spell == SpellType.TimeStop || Spell == SpellType.Translocator) && (Traits.X == TraitType.Savant || Traits.Y == TraitType.Savant));

            Array.Clear(spellList, 0, spellList.Length);

            // Setting age.
            Age = (byte) CDGMath.RandomInt(18, 30);
            ChildAge = (byte) CDGMath.RandomInt(2, 5);

            // This call updates the player's graphics.
            UpdateData();
        }
    }

    public string PlayerName
    {
        get => m_playerNameString;
        set
        {
            try
            {
                m_playerName.ChangeFontNoDefault(m_playerName.defaultFont);
                m_playerNameString = value;
                m_playerName.Text = Game.NameHelper(m_playerNameString, RomanNumeral, IsFemale);
                if (LocaleBuilder.languageType != LanguageType.Chinese_Simp && Regex.IsMatch(m_playerName.Text, @"\p{IsCyrillic}"))
                {
                    m_playerName.ChangeFontNoDefault(Game.RobotoSlabFont);
                }
            }
            catch
            {
                m_playerName.ChangeFontNoDefault(Game.NotoSansSCFont);
                m_playerNameString = value;
                m_playerName.Text = Game.NameHelper(m_playerNameString, RomanNumeral, IsFemale);
            }
        }
    }

    public byte ChestPiece    { get; set; }
    public byte HeadPiece     { get; set; }
    public byte ShoulderPiece { get; set; }

    public bool DisablePlaque { get; set; }

    public Vector2 Traits { get; internal set; }

    public bool IsDead
    {
        get => m_isDead;
        set
        {
            m_isDead = value;
            if (value)
            {
                m_trait1Title.Visible = false;
                m_trait2Title.Visible = false;
                m_ageText.Visible = true;
            }
            else
            {
                if (m_hasProsopagnosia)
                {
                    m_trait1Title.Visible = false;
                    m_trait2Title.Visible = false;
                }
                else
                {
                    m_trait1Title.Visible = true;
                    m_trait2Title.Visible = true;
                }

                m_ageText.Visible = false;
            }
        }
    }

    public bool HasProsopagnosia
    {
        set
        {
            m_hasProsopagnosia = value;
            if (m_isDead == false)
            {
                if (value)
                {
                    m_trait1Title.Visible = false;
                    m_trait2Title.Visible = false;
                }
                else
                {
                    m_trait1Title.Visible = true;
                    m_trait2Title.Visible = true;
                }
            }
        }
        get => m_hasProsopagnosia;
    }

    public override Rectangle Bounds => m_playerSprite.Bounds;

    public override Rectangle AbsBounds => m_playerSprite.Bounds;

    private string CreateMaleName(LineageScreen screen)
    {
        var name = Game.NameArray[CDGMath.RandomInt(0, Game.NameArray.Count - 1)];
        if (screen != null) // Make sure the current branch never has matching names.
        {
            var countBreaker = 0;
            while (screen.CurrentBranchNameCopyFound(name))
            {
                name = Game.NameArray[CDGMath.RandomInt(0, Game.NameArray.Count - 1)];
                countBreaker++;
                if (countBreaker > 20)
                {
                    break;
                }
            }
        }

        if (name != null)
        {
            if (name.Length > 10)
            {
                name = name.Substring(0, 9) + ".";
            }

            var nameNumber = 0;
            var romanNumerals = "";

            if (screen != null)
            {
                nameNumber = screen.NameCopies(name);
            }

            if (nameNumber > 0)
            {
                romanNumerals = CDGMath.ToRoman(nameNumber + 1);
            }

            RomanNumeral = romanNumerals;
            PlayerName = name;
            return romanNumerals;
        }

        PlayerName = "Hero";

        return "";
    }

    private string CreateFemaleName(LineageScreen screen)
    {
        var name = Game.FemaleNameArray[CDGMath.RandomInt(0, Game.FemaleNameArray.Count - 1)];
        if (screen != null) // Make sure the current branch never has matching names.
        {
            var countBreaker = 0;
            while (screen.CurrentBranchNameCopyFound(name))
            {
                name = Game.FemaleNameArray[CDGMath.RandomInt(0, Game.FemaleNameArray.Count - 1)];
                countBreaker++;
                if (countBreaker > 20)
                {
                    break;
                }
            }
        }

        if (name != null)
        {
            if (name.Length > 10)
            {
                name = name.Substring(0, 9) + ".";
            }

            var nameNumber = 0;
            var romanNumerals = "";

            if (screen != null)
            {
                nameNumber = screen.NameCopies(name);
            }

            if (nameNumber > 0)
            {
                romanNumerals = CDGMath.ToRoman(nameNumber + 1);
            }

            RomanNumeral = romanNumerals;
            PlayerName = name;
            return romanNumerals;
        }

        PlayerName = "Heroine";

        return "";
    }

    public void RandomizePortrait()
    {
        var randHeadPiece = CDGMath.RandomInt(1, PlayerPart.NumHeadPieces);
        var randShoulderPiece = CDGMath.RandomInt(1, PlayerPart.NumShoulderPieces);
        var randChestPiece = CDGMath.RandomInt(1, PlayerPart.NumChestPieces);

        if (Class == ClassType.Traitor)
        {
            randHeadPiece = PlayerPart.IntroHelm; // Force the head piece to be Johanne's headpiece if you are playing as the fountain.
        }
        else if (Class == ClassType.Dragon)
        {
            randHeadPiece = PlayerPart.DragonHelm;
        }

        SetPortrait((byte) randHeadPiece, (byte) randShoulderPiece, (byte) randChestPiece);
    }

    public void SetPortrait(byte headPiece, byte shoulderPiece, byte chestPiece)
    {
        HeadPiece = headPiece;
        ShoulderPiece = shoulderPiece;
        ChestPiece = chestPiece;

        var headPart = (m_playerSprite.GetChildAt(PlayerPart.Head) as IAnimateableObj).SpriteName;
        var numberIndex = headPart.IndexOf("_") - 1;
        headPart = headPart.Remove(numberIndex, 1);
        headPart = headPart.Replace("_", HeadPiece + "_");
        m_playerSprite.GetChildAt(PlayerPart.Head).ChangeSprite(headPart);

        var chestPart = (m_playerSprite.GetChildAt(PlayerPart.Chest) as IAnimateableObj).SpriteName;
        numberIndex = chestPart.IndexOf("_") - 1;
        chestPart = chestPart.Remove(numberIndex, 1);
        chestPart = chestPart.Replace("_", ChestPiece + "_");
        m_playerSprite.GetChildAt(PlayerPart.Chest).ChangeSprite(chestPart);

        var shoulderAPart = (m_playerSprite.GetChildAt(PlayerPart.ShoulderA) as IAnimateableObj).SpriteName;
        numberIndex = shoulderAPart.IndexOf("_") - 1;
        shoulderAPart = shoulderAPart.Remove(numberIndex, 1);
        shoulderAPart = shoulderAPart.Replace("_", ShoulderPiece + "_");
        m_playerSprite.GetChildAt(PlayerPart.ShoulderA).ChangeSprite(shoulderAPart);

        var shoulderBPart = (m_playerSprite.GetChildAt(PlayerPart.ShoulderB) as IAnimateableObj).SpriteName;
        numberIndex = shoulderBPart.IndexOf("_") - 1;
        shoulderBPart = shoulderBPart.Remove(numberIndex, 1);
        shoulderBPart = shoulderBPart.Replace("_", ShoulderPiece + "_");
        m_playerSprite.GetChildAt(PlayerPart.ShoulderB).ChangeSprite(shoulderBPart);
    }

    public void UpdateAge(int currentEra)
    {
        var startingEra = currentEra - ChildAge;
        var endingEra = currentEra + Age;
        m_ageText.Text = startingEra + " - " + endingEra;
    }

    public void UpdateData()
    {
        SetTraits(Traits);

        m_playerSprite.GetChildAt(PlayerPart.Hair).Visible = true;
        if (Traits.X == TraitType.Baldness || Traits.Y == TraitType.Baldness)
        {
            m_playerSprite.GetChildAt(PlayerPart.Hair).Visible = false;
        }

        // flibit added this.
        FlipPortrait = false;
        m_playerSprite.Rotation = 0;
        if (Traits.X == TraitType.Vertigo || Traits.Y == TraitType.Vertigo)
        {
            FlipPortrait = true;
        }

        var classText = "";
        if (LocaleBuilder.languageType == LanguageType.English)
        {
            classText += LocaleBuilder.getResourceString("LOC_ID_LINEAGE_OBJ_2_MALE", true);
            classText += " ";
            //m_classTextObj.Text = LocaleBuilder.getResourceString(this.IsFemale ? "LOC_ID_LINEAGE_OBJ_2_FEMALE" : "LOC_ID_LINEAGE_OBJ_2_MALE") +
            //     " " + LocaleBuilder.getResourceString(ClassType.ToStringID(this.Class, this.IsFemale));
        }

        m_classTextObj.Text = classText + LocaleBuilder.getResourceString(ClassType.ToStringID(Class, IsFemale));

        m_spellIcon.ChangeSprite(SpellType.Icon(Spell));

        if (Class == ClassType.Knight || Class == ClassType.Knight2)
        {
            m_playerSprite.GetChildAt(PlayerPart.Extra).Visible = true;
            m_playerSprite.GetChildAt(PlayerPart.Extra).ChangeSprite("PlayerIdleShield_Sprite");
        }
        else if (Class == ClassType.Banker || Class == ClassType.Banker2)
        {
            m_playerSprite.GetChildAt(PlayerPart.Extra).Visible = true;
            m_playerSprite.GetChildAt(PlayerPart.Extra).ChangeSprite("PlayerIdleLamp_Sprite");
        }
        else if (Class == ClassType.Wizard || Class == ClassType.Wizard2)
        {
            m_playerSprite.GetChildAt(PlayerPart.Extra).Visible = true;
            m_playerSprite.GetChildAt(PlayerPart.Extra).ChangeSprite("PlayerIdleBeard_Sprite");
        }
        else if (Class == ClassType.Ninja || Class == ClassType.Ninja2)
        {
            m_playerSprite.GetChildAt(PlayerPart.Extra).Visible = true;
            m_playerSprite.GetChildAt(PlayerPart.Extra).ChangeSprite("PlayerIdleHeadband_Sprite");
        }
        else if (Class == ClassType.Barbarian || Class == ClassType.Barbarian2)
        {
            m_playerSprite.GetChildAt(PlayerPart.Extra).Visible = true;
            m_playerSprite.GetChildAt(PlayerPart.Extra).ChangeSprite("PlayerIdleHorns_Sprite");
        }
        else
        {
            m_playerSprite.GetChildAt(PlayerPart.Extra).Visible = false;
        }

        // Special code for dragon.
        m_playerSprite.GetChildAt(PlayerPart.Wings).Visible = false;
        if (Class == ClassType.Dragon)
        {
            m_playerSprite.GetChildAt(PlayerPart.Wings).Visible = true;
            m_playerSprite.GetChildAt(PlayerPart.Head).ChangeSprite("PlayerIdleHead" + PlayerPart.DragonHelm + "_Sprite");
        }

        //Special code for traitor.
        if (Class == ClassType.Traitor)
        {
            m_playerSprite.GetChildAt(PlayerPart.Head).ChangeSprite("PlayerIdleHead" + PlayerPart.IntroHelm + "_Sprite");
        }

        // This is for male/female counterparts
        if (IsFemale == false)
        {
            m_playerSprite.GetChildAt(PlayerPart.Boobs).Visible = false;
            m_playerSprite.GetChildAt(PlayerPart.Bowtie).Visible = false;
        }
        else
        {
            m_playerSprite.GetChildAt(PlayerPart.Boobs).Visible = true;
            m_playerSprite.GetChildAt(PlayerPart.Bowtie).Visible = true;
        }

        m_playerSprite.Scale = new Vector2(2);
        if (Traits.X == TraitType.Gigantism || Traits.Y == TraitType.Gigantism)
        {
            m_playerSprite.Scale = new Vector2(GameEV.TRAIT_GIGANTISM, GameEV.TRAIT_GIGANTISM);
        }

        if (Traits.X == TraitType.Dwarfism || Traits.Y == TraitType.Dwarfism)
        {
            m_playerSprite.Scale = new Vector2(GameEV.TRAIT_DWARFISM, GameEV.TRAIT_DWARFISM);
        }

        if (Traits.X == TraitType.Ectomorph || Traits.Y == TraitType.Ectomorph)
        {
            m_playerSprite.ScaleX *= 0.825f;
            m_playerSprite.ScaleY *= 1.25f;
        }

        if (Traits.X == TraitType.Endomorph || Traits.Y == TraitType.Endomorph)
        {
            m_playerSprite.ScaleX *= 1.25f;
            m_playerSprite.ScaleY *= 1.175f;
        }

        if (Class == ClassType.SpellSword || Class == ClassType.SpellSword2)
        {
            m_playerSprite.OutlineColour = Color.White;
        }
        else
        {
            m_playerSprite.OutlineColour = Color.Black;
        }
    }

    public void DropFrame()
    {
        m_frameDropping = true;
        Tween.By(m_frameSprite, 0.7f, Back.EaseOut, "X", (-m_frameSprite.Width / 2f - 2).ToString(), "Y", "30", "Rotation", "45");
        Tween.By(m_playerSprite, 0.7f, Back.EaseOut, "X", (-m_frameSprite.Width / 2f - 2).ToString(), "Y", "30", "Rotation", "45");
        Tween.RunFunction(1.5f, this, "DropFrame2");
    }

    public void DropFrame2()
    {
        SoundManager.PlaySound("Cutsc_Picture_Fall");
        Tween.By(m_frameSprite, 0.5f, Quad.EaseIn, "Y", "1000");
        Tween.By(m_playerSprite, 0.5f, Quad.EaseIn, "Y", "1000");
    }

    public override void Draw(Camera2D camera)
    {
        //m_playerSprite.Rotation = 0;
        if (FlipPortrait)
        {
            m_playerSprite.Rotation = 180;
        }

        if (m_frameDropping == false)
        {
            m_frameSprite.Position = Position;
            m_frameSprite.Y -= 12;
            m_frameSprite.X += 5;
        }

        m_frameSprite.Opacity = Opacity;
        m_frameSprite.Draw(camera);

        if (IsDead == false && Spell != SpellType.None)
        {
            m_spellIconHolder.Position = new Vector2(m_frameSprite.X, m_frameSprite.Bounds.Bottom - 20);
            m_spellIcon.Position = m_spellIconHolder.Position;
            m_spellIconHolder.Draw(camera);
            m_spellIcon.Draw(camera);
        }

        m_playerSprite.OutlineColour = OutlineColour;
        m_playerSprite.OutlineWidth = OutlineWidth;
        if (m_frameDropping == false)
        {
            m_playerSprite.Position = Position;
            m_playerSprite.X += 10;
            if (FlipPortrait)
            {
                m_playerSprite.X -= 10;
                m_playerSprite.Y -= 30;
            }
        }

        m_playerSprite.Opacity = Opacity;
        m_playerSprite.Draw(camera);

        // only apply the lich effect if the lineageObj is being drawn.
        if (CollisionMath.Intersects(Bounds, camera.Bounds))
        {
            if (Class == ClassType.Lich || Class == ClassType.Lich2)
            {
                // This is the Tint Removal effect, that removes the tint from his face.
                Game.ColourSwapShader.Parameters["desiredTint"].SetValue(Color.White.ToVector4());
                Game.ColourSwapShader.Parameters["Opacity"].SetValue(m_playerSprite.Opacity);

                Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(m_skinColour1.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(m_lichColour1.ToVector4());

                Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(m_skinColour2.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(m_lichColour2.ToVector4());

                camera.End();
                camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, Game.ColourSwapShader, camera.GetTransformation());
                m_playerSprite.GetChildAt(PlayerPart.Head).Draw(camera);
                camera.End();
                camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.GetTransformation());
                if (IsFemale)
                {
                    m_playerSprite.GetChildAt(PlayerPart.Bowtie).Draw(camera);
                }

                m_playerSprite.GetChildAt(PlayerPart.Extra).Draw(camera);
            }
            else if (Class == ClassType.Assassin || Class == ClassType.Assassin2)
            {
                // This is the Tint Removal effect, that removes the tint from his face.
                Game.ColourSwapShader.Parameters["desiredTint"].SetValue(Color.White.ToVector4());
                Game.ColourSwapShader.Parameters["Opacity"].SetValue(m_playerSprite.Opacity);

                Game.ColourSwapShader.Parameters["ColourSwappedOut1"].SetValue(m_skinColour1.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedIn1"].SetValue(Color.Black.ToVector4());

                Game.ColourSwapShader.Parameters["ColourSwappedOut2"].SetValue(m_skinColour2.ToVector4());
                Game.ColourSwapShader.Parameters["ColourSwappedIn2"].SetValue(Color.Black.ToVector4());

                camera.End();
                camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, Game.ColourSwapShader, camera.GetTransformation());
                m_playerSprite.GetChildAt(PlayerPart.Head).Draw(camera);
                camera.End();
                camera.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.GetTransformation());
                if (IsFemale)
                {
                    m_playerSprite.GetChildAt(PlayerPart.Bowtie).Draw(camera);
                }

                m_playerSprite.GetChildAt(PlayerPart.Extra).Draw(camera);
            }
        }

        if (DisablePlaque == false)
        {
            if (m_frameDropping == false)
            {
                m_plaqueSprite.Position = Position;
                m_plaqueSprite.X += 5;
                m_plaqueSprite.Y = m_frameSprite.Y + m_frameSprite.Height - 30;
            }

            m_plaqueSprite.Draw(camera);
            camera.GraphicsDevice.SamplerStates[0] = SamplerState.LinearClamp;
            base.Draw(camera); // Base draws the text.
            camera.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
        }

        // Makes sure the frame is drawn in front of the plaque when it's doing its fall animation.
        if (m_frameDropping)
        {
            m_frameSprite.Draw(camera);
            m_playerSprite.Draw(camera);
        }
    }

    public void SetTraits(Vector2 traits)
    {
        Traits = traits;
        var traitString = "";
        if (Traits.X != 0)
        {
            traitString += LocaleBuilder.getResourceString(TraitType.ToStringID((byte) Traits.X));
        }
        //m_trait1Title.Text = TraitType.ToString((byte)Traits.X);
        else
        {
            m_trait1Title.Text = "";
        }

        if (Traits.Y != 0)
        {
            if (traits.X != 0)
            {
                traitString += ", " + LocaleBuilder.getResourceString(TraitType.ToStringID((byte) Traits.Y));
            }
            else
            {
                traitString += LocaleBuilder.getResourceString(TraitType.ToStringID((byte) Traits.Y));
            }
        }
        //m_trait2Title.Text = TraitType.ToString((byte)Traits.Y);

        m_trait1Title.Text = traitString;

        if (IsDead == false)
        {
            m_plaqueSprite.ScaleX = 1.8f;
            // Auto-scale the plaque if the trait text is too large.
            if (traits.X != TraitType.None)
            {
                float maxWidth = m_plaqueSprite.Width;
                float traitWidth = m_trait1Title.Width + 50;
                if (traitWidth > maxWidth)
                {
                    m_plaqueSprite.ScaleX *= traitWidth / maxWidth;
                }
            }
            //m_trait1Title.WordWrap(200);
        }
    }

    public void ClearTraits()
    {
        Traits = Vector2.Zero;
        m_trait1Title.Text = LocaleBuilder.getString("LOC_ID_LINEAGE_OBJ_1", m_trait1Title);
        m_trait2Title.Text = "";
    }

    public void OutlineLineageObj(Color color, int width)
    {
        m_plaqueSprite.OutlineColour = color;
        m_plaqueSprite.OutlineWidth = width;

        m_frameSprite.OutlineColour = color;
        m_frameSprite.OutlineWidth = width;
    }

    public void UpdateClassRank()
    {
        var className = "";
        var locIDToUse = "LOC_ID_LINEAGE_OBJ_4_NEW";

        if (LocaleBuilder.languageType == LanguageType.English)
        {
            //!this.IsFemale ? LocaleBuilder.getResourceString("LOC_ID_LINEAGE_OBJ_2_MALE") : LocaleBuilder.getResourceString("LOC_ID_LINEAGE_OBJ_2_FEMALE");

            className += LocaleBuilder.getResourceString("LOC_ID_LINEAGE_OBJ_2_MALE", true);
            className += " ";
        }

        if (BeatenABoss)
        {
            locIDToUse = "LOC_ID_LINEAGE_OBJ_3_NEW";
        }
        else
        {
            if (NumEnemiesKilled < 5)
            {
                locIDToUse = "LOC_ID_LINEAGE_OBJ_4_NEW";
            }
            else if (NumEnemiesKilled >= 5 && NumEnemiesKilled < 10)
            {
                locIDToUse = "LOC_ID_LINEAGE_OBJ_5_NEW";
            }
            else if (NumEnemiesKilled >= 10 && NumEnemiesKilled < 15)
            {
                locIDToUse = "LOC_ID_LINEAGE_OBJ_6_NEW";
            }
            else if (NumEnemiesKilled >= 15 && NumEnemiesKilled < 20)
            {
                locIDToUse = "LOC_ID_LINEAGE_OBJ_7_NEW";
            }
            else if (NumEnemiesKilled >= 20 && NumEnemiesKilled < 25)
            {
                locIDToUse = "LOC_ID_LINEAGE_OBJ_8_NEW";
            }
            else if (NumEnemiesKilled >= 25 && NumEnemiesKilled < 30)
            {
                locIDToUse = "LOC_ID_LINEAGE_OBJ_9_NEW";
            }
            else if (NumEnemiesKilled >= 30 && NumEnemiesKilled < 35)
            {
                locIDToUse = "LOC_ID_LINEAGE_OBJ_10_NEW";
            }
            else
            {
                locIDToUse = "LOC_ID_LINEAGE_OBJ_11_NEW";
            }
        }

        className += string.Format(LocaleBuilder.getResourceStringCustomFemale(locIDToUse, IsFemale), LocaleBuilder.getResourceString(ClassType.ToStringID(Class, IsFemale)));
        m_classTextObj.Text = className;

        m_plaqueSprite.ScaleX = 1.8f;
        // Auto-scale the plaque if the class rank text is too large.
        float maxWidth = m_plaqueSprite.Width;
        float classRankWidth = m_classTextObj.Width + 50;
        if (classRankWidth > maxWidth)
        {
            m_plaqueSprite.ScaleX *= classRankWidth / maxWidth;
        }
    }

    protected override GameObj CreateCloneInstance()
    {
        return new LineageObj(null); // Not used.
    }

    protected override void FillCloneInstance(object obj)
    {
        base.FillCloneInstance(obj);
    }

    public override void Dispose()
    {
        if (IsDisposed == false)
        {
            // Done
            m_playerSprite.Dispose();
            m_playerSprite = null;

            m_trait1Title = null;
            m_trait2Title = null;
            m_ageText = null;
            m_playerName = null;
            m_classTextObj = null;

            m_frameSprite.Dispose();
            m_frameSprite = null;
            m_plaqueSprite.Dispose();
            m_plaqueSprite = null;

            m_spellIcon.Dispose();
            m_spellIcon = null;

            m_spellIconHolder.Dispose();
            m_spellIconHolder = null;

            base.Dispose();
        }
    }

    public void RefreshTextObjs()
    {
        if (IsDead)
        {
            UpdateClassRank();
        }
        else
        {
            var classText = "";
            if (LocaleBuilder.languageType == LanguageType.English)
            {
                classText += LocaleBuilder.getResourceString("LOC_ID_LINEAGE_OBJ_2_MALE", true);
                classText += " ";
                //m_classTextObj.Text = LocaleBuilder.getResourceString(this.IsFemale ? "LOC_ID_LINEAGE_OBJ_2_FEMALE" : "LOC_ID_LINEAGE_OBJ_2_MALE") +
                //     " " + LocaleBuilder.getResourceString(ClassType.ToStringID(this.Class, this.IsFemale));
            }

            m_classTextObj.Text = classText + LocaleBuilder.getResourceString(ClassType.ToStringID(Class, IsFemale));
        }

        PlayerName = PlayerName; // This refreshes the name.
        SetTraits(Traits);
    }
}
