using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using RogueCastle.GameStructs;

namespace RogueCastle.EnvironmentVariables;

public static class SpellEV
{
    public struct SpellDefinition(string name, int cost, float damage, float x, float y, int rarity)
    {
        public readonly string Name = name; // Not really used.
        public readonly int Cost = cost;
        public readonly float Damage = damage;
        public readonly float XValue = x;
        public readonly float YValue = y;
        public readonly int Rarity = rarity;
    }

    public static readonly SpellDefinition Axe           = new("Axe",                  15, 1f,    0f,    0f, 1);
    public static readonly SpellDefinition Dagger        = new("Dagger",               10, 1f,    0f,    0f, 1);
    public static readonly SpellDefinition TimeBomb      = new("Runic Trigger",        15, 1.5f,  1f,    0f, 1);
    public static readonly SpellDefinition TimeStop      = new("Stop Watch",           15, 0f,    3f,    0f, 2);
    public static readonly SpellDefinition Nuke          = new("Nuke",                 40, 0.75f, 0f,    0f, 3);
    public static readonly SpellDefinition Translocator  = new("Quantum Translocater", 5,  0f,    0f,    0f, 3);
    public static readonly SpellDefinition Displacer     = new("Displacer",            10, 0f,    0f,    0f, 0);
    public static readonly SpellDefinition Boomerang     = new("Cross",                15, 1f,    18f,   0f, 2);
    public static readonly SpellDefinition DualBlades    = new("Spark",                15, 1f,    0f,    0f, 1);
    public static readonly SpellDefinition Close         = new("Katana",               15, 0.5f,  2.1f,  0f, 2);
    public static readonly SpellDefinition DamageShield  = new("Leaf",                 15, 1f,    9999f, 5f, 2);
    public static readonly SpellDefinition Bounce        = new("Chaos",                30, 0.4f,  3.5f,  0f, 3);
    public static readonly SpellDefinition Laser         = new("Laser",                15, 1f,    5f,    0f, 3);
    public static readonly SpellDefinition RapidDagger   = new("Rapid Dagger",         30, 0.75f, 0f,    0f, 1);
    public static readonly SpellDefinition DragonFire    = new("Dragon Fire",          15, 1f,    0.35f, 0f, 3);
    public static readonly SpellDefinition DragonFireNeo = new("Dragon Fire Neo",      0,  1f,    0.75f, 0f, 3);

    public static ProjectileData GetProjData(this SpellType type, PlayerObj player)
    {
        var projData = new ProjectileData(player)
        {
            SpriteName = "BoneProjectile_Sprite",
            SourceAnchor = Vector2.Zero,
            Target = null,
            Speed = new Vector2(0, 0),
            IsWeighted = false,
            RotationSpeed = 0,
            Damage = 0,
            AngleOffset = 0,
            CollidesWithTerrain = false,
            Scale = Vector2.One,
            ShowIcon = false,
        };

        switch (type)
        {
            case SpellType.Axe:
                projData.SpriteName = "SpellAxe_Sprite";
                projData.Angle = new Vector2(-74, -74);
                projData.Speed = new Vector2(1050, 1050); //(1000, 1000);
                projData.SourceAnchor = new Vector2(50, -50);
                projData.IsWeighted = true;
                projData.RotationSpeed = 10; //15;
                projData.CollidesWithTerrain = false;
                projData.DestroysWithTerrain = false;
                projData.DestroysWithEnemy = false;
                projData.Scale = new Vector2(3,3); //(2, 2);
                break;
            case SpellType.Dagger:
                projData.SpriteName = "SpellDagger_Sprite";
                projData.Angle = Vector2.Zero;
                projData.SourceAnchor = new Vector2(50, 0);
                projData.Speed = new Vector2(1750, 1750);
                projData.IsWeighted = false;
                projData.RotationSpeed = 0; //35;
                projData.CollidesWithTerrain = true;
                projData.DestroysWithTerrain = true;
                projData.Scale = new Vector2(2.5f,2.5f); //(2, 2);
                break;
            case SpellType.DragonFire:
                projData.SpriteName = "TurretProjectile_Sprite";
                projData.Angle = Vector2.Zero;
                projData.SourceAnchor = new Vector2(50, 0);
                projData.Speed = new Vector2(1100,1100); //(1450, 1450);
                projData.Lifespan = DragonFire.XValue;
                projData.IsWeighted = false;
                projData.RotationSpeed = 0; //35;
                projData.CollidesWithTerrain = true;
                projData.DestroysWithTerrain = true;
                projData.Scale = new Vector2(2.5f,2.5f); //(2, 2);
                break;
            case SpellType.DragonFireNeo:
                projData.SpriteName = "TurretProjectile_Sprite";
                projData.Angle = Vector2.Zero;
                projData.SourceAnchor = new Vector2(50, 0);
                projData.Speed = new Vector2(1750,1750); //(1450, 1450);
                projData.Lifespan = DragonFireNeo.XValue;
                projData.IsWeighted = false;
                projData.RotationSpeed = 0; //35;
                projData.CollidesWithTerrain = true;
                projData.DestroysWithTerrain = true;
                projData.Scale = new Vector2(2.75f,2.75f); //(2, 2);
                break;
            case SpellType.TimeBomb:
                projData.SpriteName = "SpellTimeBomb_Sprite";
                projData.Angle = new Vector2(-35,-35); //(-65, -65);
                projData.Speed = new Vector2(500,500); //(1000, 1000);
                projData.SourceAnchor = new Vector2 (50, -50); //(0, -100); //(50, -50);
                projData.IsWeighted = true;
                projData.RotationSpeed = 0;
                projData.StartingRotation = 0;
                projData.CollidesWithTerrain = true;
                projData.DestroysWithTerrain = false;
                projData.CollidesWith1Ways = true;
                projData.Scale = new Vector2(3,3); //(2, 2);
                break;
            case SpellType.TimeStop:
                break;
            case SpellType.Nuke:
                projData.SpriteName = "SpellNuke_Sprite";
                projData.Angle = new Vector2(-65, -65);
                projData.Speed = new Vector2(500, 500);
                projData.IsWeighted = false;
                projData.RotationSpeed = 0;
                projData.CollidesWithTerrain = false;
                projData.DestroysWithTerrain = false;
                projData.ChaseTarget = false; // This needs to be set to false because I'm doing something special for the nuke spell.
                projData.DestroysWithEnemy = true;
                projData.Scale = new Vector2(2, 2);
                break;
            case SpellType.Translocator:
                break;
            case SpellType.Displacer:
                projData.SourceAnchor = new Vector2(0,0); //(300, 0);
                projData.SpriteName = "SpellDisplacer_Sprite";
                projData.Angle = new Vector2(0,0); //(90,90); //(0,0); //(-65, -65);
                projData.Speed = Vector2.Zero; //new Vector2(8000,8000); //(1000, 1000);
                projData.IsWeighted = false;
                projData.RotationSpeed = 0; //45; //0;
                projData.CollidesWithTerrain = true;
                projData.DestroysWithTerrain = false;
                projData.CollidesWith1Ways = true; // SETTING TO TRUE TO FIX BUGS WITH LARGE GUYS GETTING INTO TINY HOLES
                projData.Scale = new Vector2(2,2); // (2, 2);
                break;
            case SpellType.Boomerang:
                projData.SpriteName = "SpellBoomerang_Sprite";
                projData.Angle = new Vector2(0, 0);
                projData.SourceAnchor = new Vector2(50, -10);
                projData.Speed = new Vector2(790, 790); //Vector2(730, 730); //(1000, 1000);
                projData.IsWeighted = false;
                projData.RotationSpeed = 25;
                projData.CollidesWithTerrain = false;
                projData.DestroysWithTerrain = false;
                projData.DestroysWithEnemy = false;
                projData.Scale = new Vector2(3,3);
                break;
            case SpellType.DualBlades:
                projData.SpriteName = "SpellDualBlades_Sprite";
                projData.Angle = new Vector2(-55, -55);
                projData.SourceAnchor = new Vector2(50, 30);
                projData.Speed = new Vector2(1000, 1000);
                projData.IsWeighted = false;
                //projData.StartingRotation = 45;
                projData.RotationSpeed = 30; //20;
                projData.CollidesWithTerrain = false;
                projData.DestroysWithTerrain = false;
                projData.DestroysWithEnemy = false;
                projData.Scale = new Vector2(2, 2);
                break;
            case SpellType.Close:
                projData.SpriteName = "SpellClose_Sprite";
                //projData.Angle = new Vector2(90, 90);
                projData.SourceAnchor = new Vector2(120, -60); //(75,-200); //(50, 0);
                projData.Speed = new Vector2(0,0); //(450,450); //(1000, 1000);
                projData.IsWeighted = false;
                projData.RotationSpeed = 0f;
                projData.DestroysWithEnemy = false;
                projData.DestroysWithTerrain = false;
                projData.CollidesWithTerrain = false;
                projData.Scale = new Vector2(2.5f, 2.5f);
                projData.LockPosition = true;
                break;
            case SpellType.DamageShield:
                projData.SpriteName = "SpellDamageShield_Sprite";
                projData.Angle = new Vector2(-65, -65);
                projData.Speed = new Vector2(3.25f, 3.25f); //(2.45f, 2.45f); //(2.0f, 2.0f);
                projData.Target = player;
                projData.IsWeighted = false;
                projData.RotationSpeed = 0;
                projData.CollidesWithTerrain = false;
                projData.DestroysWithTerrain = false;
                projData.DestroysWithEnemy = false;
                projData.Scale = new Vector2(3.0f, 3.0f);
                projData.DestroyOnRoomTransition = false;
                break;
            case SpellType.Bounce:
                projData.SpriteName = "SpellBounce_Sprite";
                projData.Angle = new Vector2(-135, -135);
                projData.Speed = new Vector2(785,785); //(825, 825);
                projData.IsWeighted = false;
                projData.StartingRotation = -135;
                projData.FollowArc = false;
                projData.RotationSpeed = 20;
                projData.SourceAnchor = new Vector2(-10, -10);
                projData.DestroysWithTerrain = false;
                projData.DestroysWithEnemy = false; //true;
                projData.CollidesWithTerrain = true;
                projData.Scale = new Vector2(3.25f, 3.25f); //(2.5f, 2.5f);
                break;
            case SpellType.Laser:
                projData.SpriteName = "LaserSpell_Sprite";
                projData.Angle = new Vector2(0, 0);
                projData.Speed = new Vector2(0, 0);
                projData.IsWeighted = false;
                projData.IsCollidable = false;
                projData.StartingRotation = 0;
                projData.FollowArc = false;
                projData.RotationSpeed = 0;
                projData.DestroysWithTerrain = false;
                projData.DestroysWithEnemy = false;
                projData.CollidesWithTerrain = false;
                projData.LockPosition = true;
                break;
            case SpellType.RapidDagger:
                projData.SpriteName = "LaserSpell_Sprite";
                projData.Angle = new Vector2(0, 0);
                projData.Speed = new Vector2(0, 0);
                projData.IsWeighted = false;
                projData.IsCollidable = false;
                projData.StartingRotation = 0;
                projData.FollowArc = false;
                projData.RotationSpeed = 0;
                projData.DestroysWithTerrain = false;
                projData.DestroysWithEnemy = false;
                projData.CollidesWithTerrain = false;
                projData.LockPosition = true;
                break;
        }

        return projData;
    }

    public static int GetManaCost(this SpellType type)
    {
        return type switch
        {
            SpellType.Dagger        => Dagger.Cost,
            SpellType.Axe           => Axe.Cost,
            SpellType.TimeBomb      => TimeBomb.Cost,
            SpellType.TimeStop      => TimeStop.Cost,
            SpellType.Nuke          => Nuke.Cost,
            SpellType.Translocator  => Translocator.Cost,
            SpellType.Displacer     => Displacer.Cost,
            SpellType.Boomerang     => Boomerang.Cost,
            SpellType.DualBlades    => DualBlades.Cost,
            SpellType.Close         => Close.Cost,
            SpellType.DamageShield  => DamageShield.Cost,
            SpellType.Bounce        => Bounce.Cost,
            SpellType.Laser         => Laser.Cost,
            SpellType.DragonFire    => DragonFire.Cost,
            SpellType.DragonFireNeo => DragonFireNeo.Cost,
            SpellType.RapidDagger   => RapidDagger.Cost,
            _                       => 0,
        };
    }

    public static int GetRarity(this SpellType type)
    {
        return type switch
        {
            SpellType.Dagger        => Dagger.Rarity,
            SpellType.Axe           => Axe.Rarity,
            SpellType.TimeBomb      => TimeBomb.Rarity,
            SpellType.TimeStop      => TimeStop.Rarity,
            SpellType.Nuke          => Nuke.Rarity,
            SpellType.Translocator  => Translocator.Rarity,
            SpellType.Displacer     => Displacer.Rarity,
            SpellType.Boomerang     => Boomerang.Rarity,
            SpellType.DualBlades    => DualBlades.Rarity,
            SpellType.Close         => Close.Rarity,
            SpellType.DamageShield  => DamageShield.Rarity,
            SpellType.Bounce        => Bounce.Rarity,
            SpellType.Laser         => Laser.Rarity,
            SpellType.DragonFire    => DragonFire.Rarity,
            SpellType.DragonFireNeo => DragonFireNeo.Rarity,
            SpellType.RapidDagger   => RapidDagger.Rarity,
            _                       => 0,
        };
    }

    public static float GetDamageMultiplier(this SpellType type)
    {
        return type switch
        {
            SpellType.Dagger        => Dagger.Damage,
            SpellType.Axe           => Axe.Damage,
            SpellType.TimeBomb      => TimeBomb.Damage,
            SpellType.TimeStop      => TimeStop.Damage,
            SpellType.Nuke          => Nuke.Damage,
            SpellType.Translocator  => Translocator.Damage,
            SpellType.Displacer     => Displacer.Damage,
            SpellType.Boomerang     => Boomerang.Damage,
            SpellType.DualBlades    => DualBlades.Damage,
            SpellType.Close         => Close.Damage,
            SpellType.DamageShield  => DamageShield.Damage,
            SpellType.Bounce        => Bounce.Damage,
            SpellType.Laser         => Laser.Damage,
            SpellType.DragonFire    => DragonFire.Damage,
            SpellType.DragonFireNeo => DragonFireNeo.Damage,
            SpellType.RapidDagger   => RapidDagger.Damage,
            _                       => 0,
        };
    }

    public static float GetXValue(this SpellType type)
    {
        return type switch
        {
            SpellType.Dagger        => Dagger.XValue,
            SpellType.Axe           => Axe.XValue,
            SpellType.TimeBomb      => TimeBomb.XValue,
            SpellType.TimeStop      => TimeStop.XValue,
            SpellType.Nuke          => Nuke.XValue,
            SpellType.Translocator  => Translocator.XValue,
            SpellType.Displacer     => Displacer.XValue,
            SpellType.Boomerang     => Boomerang.XValue,
            SpellType.DualBlades    => DualBlades.XValue,
            SpellType.Close         => Close.XValue,
            SpellType.DamageShield  => DamageShield.XValue,
            SpellType.Bounce        => Bounce.XValue,
            SpellType.Laser         => Laser.XValue,
            SpellType.DragonFire    => DragonFire.XValue,
            SpellType.DragonFireNeo => DragonFireNeo.XValue,
            _                       => 0,
        };
    }

    public static float GetYValue(this SpellType type)
    {
        return type switch
        {
            SpellType.Dagger        => Dagger.YValue,
            SpellType.Axe           => Axe.YValue,
            SpellType.TimeBomb      => TimeBomb.YValue,
            SpellType.TimeStop      => TimeStop.YValue,
            SpellType.Nuke          => Nuke.YValue,
            SpellType.Translocator  => Translocator.YValue,
            SpellType.Displacer     => Displacer.YValue,
            SpellType.Boomerang     => Boomerang.YValue,
            SpellType.DualBlades    => DualBlades.YValue,
            SpellType.Close         => Close.YValue,
            SpellType.DamageShield  => DamageShield.YValue,
            SpellType.Bounce        => Bounce.YValue,
            SpellType.Laser         => Laser.YValue,
            SpellType.DragonFire    => DragonFire.YValue,
            SpellType.DragonFireNeo => DragonFireNeo.YValue,
            _                       => 0,
        };
    }

    public static string ToStringID(this SpellType type)
    {
        return type switch
        {
            SpellType.Dagger        => "LOC_ID_SPELL_TYPE_1",
            SpellType.Axe           => "LOC_ID_SPELL_TYPE_2",
            SpellType.TimeBomb      => "LOC_ID_SPELL_TYPE_3",
            SpellType.TimeStop      => "LOC_ID_SPELL_TYPE_4",
            SpellType.Nuke          => "LOC_ID_SPELL_TYPE_5",
            SpellType.Translocator  => "LOC_ID_SPELL_TYPE_6",
            SpellType.Displacer     => "LOC_ID_SPELL_TYPE_7",
            SpellType.Boomerang     => "LOC_ID_SPELL_TYPE_8",
            SpellType.DualBlades    => "LOC_ID_SPELL_TYPE_9",
            SpellType.Close         => "LOC_ID_SPELL_TYPE_10",
            SpellType.DamageShield  => "LOC_ID_SPELL_TYPE_11",
            SpellType.Bounce        => "LOC_ID_SPELL_TYPE_12",
            SpellType.Laser         => "LOC_ID_SPELL_TYPE_13",
            SpellType.DragonFire    => "LOC_ID_SPELL_TYPE_14",
            SpellType.DragonFireNeo => "LOC_ID_SPELL_TYPE_14",
            SpellType.RapidDagger   => "LOC_ID_SPELL_TYPE_15",
            _                       => "",
        };
    }

    public static string DescriptionID(this SpellType type)
    {
        return type switch
        {
            SpellType.Dagger        => "LOC_ID_SPELL_DESC_1",
            SpellType.Axe           => "LOC_ID_SPELL_DESC_2",
            SpellType.TimeBomb      => "LOC_ID_SPELL_DESC_3",
            SpellType.TimeStop      => "LOC_ID_SPELL_DESC_4",
            SpellType.Nuke          => "LOC_ID_SPELL_DESC_5",
            SpellType.Translocator  => "LOC_ID_SPELL_DESC_6",
            SpellType.Displacer     => "LOC_ID_SPELL_DESC_7",
            SpellType.Boomerang     => "LOC_ID_SPELL_DESC_8",
            SpellType.DualBlades    => "LOC_ID_SPELL_DESC_9",
            SpellType.Close         => "LOC_ID_SPELL_DESC_10",
            SpellType.DamageShield  => "LOC_ID_SPELL_DESC_11",
            SpellType.Bounce        => "LOC_ID_SPELL_DESC_12",
            SpellType.Laser         => "LOC_ID_SPELL_DESC_13",
            SpellType.DragonFire    => "LOC_ID_SPELL_DESC_14",
            SpellType.DragonFireNeo => "LOC_ID_SPELL_DESC_14",
            SpellType.RapidDagger   => "LOC_ID_SPELL_DESC_15",
            _                       => "",
        };
    }

    public static string Icon(this SpellType type)
    {
        return type switch
        {
            SpellType.Dagger        => "DaggerIcon_Sprite",
            SpellType.Axe           => "AxeIcon_Sprite",
            SpellType.TimeBomb      => "TimeBombIcon_Sprite",
            SpellType.TimeStop      => "TimeStopIcon_Sprite",
            SpellType.Nuke          => "NukeIcon_Sprite",
            SpellType.Translocator  => "TranslocatorIcon_Sprite",
            SpellType.Displacer     => "DisplacerIcon_Sprite",
            SpellType.Boomerang     => "BoomerangIcon_Sprite",
            SpellType.DualBlades    => "DualBladesIcon_Sprite",
            SpellType.Close         => "CloseIcon_Sprite",
            SpellType.DamageShield  => "DamageShieldIcon_Sprite",
            SpellType.Bounce        => "BounceIcon_Sprite",
            SpellType.Laser         => "DaggerIcon_Sprite",
            SpellType.DragonFire    => "DragonFireIcon_Sprite",
            SpellType.DragonFireNeo => "DragonFireIcon_Sprite",
            SpellType.RapidDagger   => "RapidDaggerIcon_Sprite",
            _                       => "DaggerIcon_Sprite",
        };
    }

    // TODO: Should be moved to a more relevant section?
    public static Vector3 GetNext3Spells()
    {
        SpellType[] spellArray = ClassType.GetSpellList(ClassType.Wizard2);
        List<SpellType> spellList = spellArray.ToList();

        var spellIndex = spellList.IndexOf(Game.PlayerStats.Spell);
        spellList.Clear();

        var wizardSpells = new SpellType[3];
        for (var i = 0; i < 3; i++)
        {
            wizardSpells[i] = spellArray[spellIndex];
            spellIndex++;
            if (spellIndex >= spellArray.Length)
                spellIndex = 0;
        }

        return new Vector3((byte)wizardSpells[0], (byte)wizardSpells[1], (byte)wizardSpells[2]);
    }
}
