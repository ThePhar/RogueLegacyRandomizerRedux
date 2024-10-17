using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using RogueCastle.GameStructs;

namespace RogueCastle.EnvironmentVariables;

public static class SpellEV
{
    public static readonly SpellDefinition Axe = new("Axe", 15, 1f, 0f, 0f, 1);
    public static readonly SpellDefinition Dagger = new("Dagger", 10, 1f, 0f, 0f, 1);
    public static readonly SpellDefinition TimeBomb = new("Runic Trigger", 15, 1.5f, 1f, 0f, 1);
    public static readonly SpellDefinition TimeStop = new("Stop Watch", 15, 0f, 3f, 0f, 2);
    public static readonly SpellDefinition Nuke = new("Nuke", 40, 0.75f, 0f, 0f, 3);
    public static readonly SpellDefinition Translocator = new("Quantum Translocater", 5, 0f, 0f, 0f, 3);
    public static readonly SpellDefinition Displacer = new("Displacer", 10, 0f, 0f, 0f, 0);
    public static readonly SpellDefinition Boomerang = new("Cross", 15, 1f, 18f, 0f, 2);
    public static readonly SpellDefinition DualBlades = new("Spark", 15, 1f, 0f, 0f, 1);
    public static readonly SpellDefinition Close = new("Katana", 15, 0.5f, 2.1f, 0f, 2);
    public static readonly SpellDefinition DamageShield = new("Leaf", 15, 1f, 9999f, 5f, 2);
    public static readonly SpellDefinition Bounce = new("Chaos", 30, 0.4f, 3.5f, 0f, 3);
    public static readonly SpellDefinition Laser = new("Laser", 15, 1f, 5f, 0f, 3);
    public static readonly SpellDefinition RapidDagger = new("Rapid Dagger", 30, 0.75f, 0f, 0f, 1);
    public static readonly SpellDefinition DragonFire = new("Dragon Fire", 15, 1f, 0.35f, 0f, 3);
    public static readonly SpellDefinition DragonFireNeo = new("Dragon Fire Neo", 0, 1f, 0.75f, 0f, 3);

    public static ProjectileData GetProjData(byte type, PlayerObj player)
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
            case SpellType.AXE:
                projData.SpriteName = "SpellAxe_Sprite";
                projData.Angle = new Vector2(-74, -74);
                projData.Speed = new Vector2(1050, 1050); //(1000, 1000);
                projData.SourceAnchor = new Vector2(50, -50);
                projData.IsWeighted = true;
                projData.RotationSpeed = 10; //15;
                projData.CollidesWithTerrain = false;
                projData.DestroysWithTerrain = false;
                projData.DestroysWithEnemy = false;
                projData.Scale = new Vector2(3, 3); //(2, 2);
                break;
            case SpellType.DAGGER:
                projData.SpriteName = "SpellDagger_Sprite";
                projData.Angle = Vector2.Zero;
                projData.SourceAnchor = new Vector2(50, 0);
                projData.Speed = new Vector2(1750, 1750);
                projData.IsWeighted = false;
                projData.RotationSpeed = 0; //35;
                projData.CollidesWithTerrain = true;
                projData.DestroysWithTerrain = true;
                projData.Scale = new Vector2(2.5f, 2.5f); //(2, 2);
                break;
            case SpellType.DRAGON_FIRE:
                projData.SpriteName = "TurretProjectile_Sprite";
                projData.Angle = Vector2.Zero;
                projData.SourceAnchor = new Vector2(50, 0);
                projData.Speed = new Vector2(1100, 1100); //(1450, 1450);
                projData.Lifespan = DragonFire.XValue;
                projData.IsWeighted = false;
                projData.RotationSpeed = 0; //35;
                projData.CollidesWithTerrain = true;
                projData.DestroysWithTerrain = true;
                projData.Scale = new Vector2(2.5f, 2.5f); //(2, 2);
                break;
            case SpellType.DRAGON_FIRE_NEO:
                projData.SpriteName = "TurretProjectile_Sprite";
                projData.Angle = Vector2.Zero;
                projData.SourceAnchor = new Vector2(50, 0);
                projData.Speed = new Vector2(1750, 1750); //(1450, 1450);
                projData.Lifespan = DragonFireNeo.XValue;
                projData.IsWeighted = false;
                projData.RotationSpeed = 0; //35;
                projData.CollidesWithTerrain = true;
                projData.DestroysWithTerrain = true;
                projData.Scale = new Vector2(2.75f, 2.75f); //(2, 2);
                break;
            case SpellType.TIME_BOMB:
                projData.SpriteName = "SpellTimeBomb_Sprite";
                projData.Angle = new Vector2(-35, -35); //(-65, -65);
                projData.Speed = new Vector2(500, 500); //(1000, 1000);
                projData.SourceAnchor = new Vector2(50, -50); //(0, -100); //(50, -50);
                projData.IsWeighted = true;
                projData.RotationSpeed = 0;
                projData.StartingRotation = 0;
                projData.CollidesWithTerrain = true;
                projData.DestroysWithTerrain = false;
                projData.CollidesWith1Ways = true;
                projData.Scale = new Vector2(3, 3); //(2, 2);
                break;
            case SpellType.TIME_STOP:
                break;
            case SpellType.NUKE:
                projData.SpriteName = "SpellNuke_Sprite";
                projData.Angle = new Vector2(-65, -65);
                projData.Speed = new Vector2(500, 500);
                projData.IsWeighted = false;
                projData.RotationSpeed = 0;
                projData.CollidesWithTerrain = false;
                projData.DestroysWithTerrain = false;
                projData.ChaseTarget =
                    false; // This needs to be set to false because I'm doing something special for the nuke spell.
                projData.DestroysWithEnemy = true;
                projData.Scale = new Vector2(2, 2);
                break;
            case SpellType.TRANSLOCATOR:
                break;
            case SpellType.DISPLACER:
                projData.SourceAnchor = new Vector2(0, 0); //(300, 0);
                projData.SpriteName = "SpellDisplacer_Sprite";
                projData.Angle = new Vector2(0, 0); //(90,90); //(0,0); //(-65, -65);
                projData.Speed = Vector2.Zero; //new Vector2(8000,8000); //(1000, 1000);
                projData.IsWeighted = false;
                projData.RotationSpeed = 0; //45; //0;
                projData.CollidesWithTerrain = true;
                projData.DestroysWithTerrain = false;
                projData.CollidesWith1Ways =
                    true; // SETTING TO TRUE TO FIX BUGS WITH LARGE GUYS GETTING INTO TINY HOLES
                projData.Scale = new Vector2(2, 2); // (2, 2);
                break;
            case SpellType.BOOMERANG:
                projData.SpriteName = "SpellBoomerang_Sprite";
                projData.Angle = new Vector2(0, 0);
                projData.SourceAnchor = new Vector2(50, -10);
                projData.Speed = new Vector2(790, 790); //Vector2(730, 730); //(1000, 1000);
                projData.IsWeighted = false;
                projData.RotationSpeed = 25;
                projData.CollidesWithTerrain = false;
                projData.DestroysWithTerrain = false;
                projData.DestroysWithEnemy = false;
                projData.Scale = new Vector2(3, 3);
                break;
            case SpellType.DUAL_BLADES:
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
            case SpellType.CLOSE:
                projData.SpriteName = "SpellClose_Sprite";
                //projData.Angle = new Vector2(90, 90);
                projData.SourceAnchor = new Vector2(120, -60); //(75,-200); //(50, 0);
                projData.Speed = new Vector2(0, 0); //(450,450); //(1000, 1000);
                projData.IsWeighted = false;
                projData.RotationSpeed = 0f;
                projData.DestroysWithEnemy = false;
                projData.DestroysWithTerrain = false;
                projData.CollidesWithTerrain = false;
                projData.Scale = new Vector2(2.5f, 2.5f);
                projData.LockPosition = true;
                break;
            case SpellType.DAMAGE_SHIELD:
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
            case SpellType.BOUNCE:
                projData.SpriteName = "SpellBounce_Sprite";
                projData.Angle = new Vector2(-135, -135);
                projData.Speed = new Vector2(785, 785); //(825, 825);
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
            case SpellType.LASER:
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
            case SpellType.RAPID_DAGGER:
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

    public static int GetManaCost(byte type)
    {
        return type switch
        {
            SpellType.DAGGER          => Dagger.Cost,
            SpellType.AXE             => Axe.Cost,
            SpellType.TIME_BOMB       => TimeBomb.Cost,
            SpellType.TIME_STOP       => TimeStop.Cost,
            SpellType.NUKE            => Nuke.Cost,
            SpellType.TRANSLOCATOR    => Translocator.Cost,
            SpellType.DISPLACER       => Displacer.Cost,
            SpellType.BOOMERANG       => Boomerang.Cost,
            SpellType.DUAL_BLADES     => DualBlades.Cost,
            SpellType.CLOSE           => Close.Cost,
            SpellType.DAMAGE_SHIELD   => DamageShield.Cost,
            SpellType.BOUNCE          => Bounce.Cost,
            SpellType.LASER           => Laser.Cost,
            SpellType.DRAGON_FIRE     => DragonFire.Cost,
            SpellType.DRAGON_FIRE_NEO => DragonFireNeo.Cost,
            SpellType.RAPID_DAGGER    => RapidDagger.Cost,
            _                         => 0,
        };
    }

    public static int GetRarity(byte type)
    {
        return type switch
        {
            SpellType.DAGGER          => Dagger.Rarity,
            SpellType.AXE             => Axe.Rarity,
            SpellType.TIME_BOMB       => TimeBomb.Rarity,
            SpellType.TIME_STOP       => TimeStop.Rarity,
            SpellType.NUKE            => Nuke.Rarity,
            SpellType.TRANSLOCATOR    => Translocator.Rarity,
            SpellType.DISPLACER       => Displacer.Rarity,
            SpellType.BOOMERANG       => Boomerang.Rarity,
            SpellType.DUAL_BLADES     => DualBlades.Rarity,
            SpellType.CLOSE           => Close.Rarity,
            SpellType.DAMAGE_SHIELD   => DamageShield.Rarity,
            SpellType.BOUNCE          => Bounce.Rarity,
            SpellType.LASER           => Laser.Rarity,
            SpellType.DRAGON_FIRE     => DragonFire.Rarity,
            SpellType.DRAGON_FIRE_NEO => DragonFireNeo.Rarity,
            SpellType.RAPID_DAGGER    => RapidDagger.Rarity,
            _                         => 0,
        };
    }

    public static float GetDamageMultiplier(byte type)
    {
        return type switch
        {
            SpellType.DAGGER          => Dagger.Damage,
            SpellType.AXE             => Axe.Damage,
            SpellType.TIME_BOMB       => TimeBomb.Damage,
            SpellType.TIME_STOP       => TimeStop.Damage,
            SpellType.NUKE            => Nuke.Damage,
            SpellType.TRANSLOCATOR    => Translocator.Damage,
            SpellType.DISPLACER       => Displacer.Damage,
            SpellType.BOOMERANG       => Boomerang.Damage,
            SpellType.DUAL_BLADES     => DualBlades.Damage,
            SpellType.CLOSE           => Close.Damage,
            SpellType.DAMAGE_SHIELD   => DamageShield.Damage,
            SpellType.BOUNCE          => Bounce.Damage,
            SpellType.LASER           => Laser.Damage,
            SpellType.DRAGON_FIRE     => DragonFire.Damage,
            SpellType.DRAGON_FIRE_NEO => DragonFireNeo.Damage,
            SpellType.RAPID_DAGGER    => RapidDagger.Damage,
            _                         => 0,
        };
    }

    public static float GetXValue(byte type)
    {
        return type switch
        {
            SpellType.DAGGER          => Dagger.XValue,
            SpellType.AXE             => Axe.XValue,
            SpellType.TIME_BOMB       => TimeBomb.XValue,
            SpellType.TIME_STOP       => TimeStop.XValue,
            SpellType.NUKE            => Nuke.XValue,
            SpellType.TRANSLOCATOR    => Translocator.XValue,
            SpellType.DISPLACER       => Displacer.XValue,
            SpellType.BOOMERANG       => Boomerang.XValue,
            SpellType.DUAL_BLADES     => DualBlades.XValue,
            SpellType.CLOSE           => Close.XValue,
            SpellType.DAMAGE_SHIELD   => DamageShield.XValue,
            SpellType.BOUNCE          => Bounce.XValue,
            SpellType.LASER           => Laser.XValue,
            SpellType.DRAGON_FIRE     => DragonFire.XValue,
            SpellType.DRAGON_FIRE_NEO => DragonFireNeo.XValue,
            _                         => 0,
        };
    }

    public static float GetYValue(byte type)
    {
        return type switch
        {
            SpellType.DAGGER          => Dagger.YValue,
            SpellType.AXE             => Axe.YValue,
            SpellType.TIME_BOMB       => TimeBomb.YValue,
            SpellType.TIME_STOP       => TimeStop.YValue,
            SpellType.NUKE            => Nuke.YValue,
            SpellType.TRANSLOCATOR    => Translocator.YValue,
            SpellType.DISPLACER       => Displacer.YValue,
            SpellType.BOOMERANG       => Boomerang.YValue,
            SpellType.DUAL_BLADES     => DualBlades.YValue,
            SpellType.CLOSE           => Close.YValue,
            SpellType.DAMAGE_SHIELD   => DamageShield.YValue,
            SpellType.BOUNCE          => Bounce.YValue,
            SpellType.LASER           => Laser.YValue,
            SpellType.DRAGON_FIRE     => DragonFire.YValue,
            SpellType.DRAGON_FIRE_NEO => DragonFireNeo.YValue,
            _                         => 0,
        };
    }

    public static string ToStringID(byte type)
    {
        return type switch
        {
            SpellType.DAGGER          => "LOC_ID_SPELL_TYPE_1",
            SpellType.AXE             => "LOC_ID_SPELL_TYPE_2",
            SpellType.TIME_BOMB       => "LOC_ID_SPELL_TYPE_3",
            SpellType.TIME_STOP       => "LOC_ID_SPELL_TYPE_4",
            SpellType.NUKE            => "LOC_ID_SPELL_TYPE_5",
            SpellType.TRANSLOCATOR    => "LOC_ID_SPELL_TYPE_6",
            SpellType.DISPLACER       => "LOC_ID_SPELL_TYPE_7",
            SpellType.BOOMERANG       => "LOC_ID_SPELL_TYPE_8",
            SpellType.DUAL_BLADES     => "LOC_ID_SPELL_TYPE_9",
            SpellType.CLOSE           => "LOC_ID_SPELL_TYPE_10",
            SpellType.DAMAGE_SHIELD   => "LOC_ID_SPELL_TYPE_11",
            SpellType.BOUNCE          => "LOC_ID_SPELL_TYPE_12",
            SpellType.LASER           => "LOC_ID_SPELL_TYPE_13",
            SpellType.DRAGON_FIRE     => "LOC_ID_SPELL_TYPE_14",
            SpellType.DRAGON_FIRE_NEO => "LOC_ID_SPELL_TYPE_14",
            SpellType.RAPID_DAGGER    => "LOC_ID_SPELL_TYPE_15",
            _                         => "",
        };
    }

    public static string DescriptionID(byte type)
    {
        return type switch
        {
            SpellType.DAGGER          => "LOC_ID_SPELL_DESC_1",
            SpellType.AXE             => "LOC_ID_SPELL_DESC_2",
            SpellType.TIME_BOMB       => "LOC_ID_SPELL_DESC_3",
            SpellType.TIME_STOP       => "LOC_ID_SPELL_DESC_4",
            SpellType.NUKE            => "LOC_ID_SPELL_DESC_5",
            SpellType.TRANSLOCATOR    => "LOC_ID_SPELL_DESC_6",
            SpellType.DISPLACER       => "LOC_ID_SPELL_DESC_7",
            SpellType.BOOMERANG       => "LOC_ID_SPELL_DESC_8",
            SpellType.DUAL_BLADES     => "LOC_ID_SPELL_DESC_9",
            SpellType.CLOSE           => "LOC_ID_SPELL_DESC_10",
            SpellType.DAMAGE_SHIELD   => "LOC_ID_SPELL_DESC_11",
            SpellType.BOUNCE          => "LOC_ID_SPELL_DESC_12",
            SpellType.LASER           => "LOC_ID_SPELL_DESC_13",
            SpellType.DRAGON_FIRE     => "LOC_ID_SPELL_DESC_14",
            SpellType.DRAGON_FIRE_NEO => "LOC_ID_SPELL_DESC_14",
            SpellType.RAPID_DAGGER    => "LOC_ID_SPELL_DESC_15",
            _                         => "",
        };
    }

    public static string Icon(byte type)
    {
        return type switch
        {
            SpellType.DAGGER          => "DaggerIcon_Sprite",
            SpellType.AXE             => "AxeIcon_Sprite",
            SpellType.TIME_BOMB       => "TimeBombIcon_Sprite",
            SpellType.TIME_STOP       => "TimeStopIcon_Sprite",
            SpellType.NUKE            => "NukeIcon_Sprite",
            SpellType.TRANSLOCATOR    => "TranslocatorIcon_Sprite",
            SpellType.DISPLACER       => "DisplacerIcon_Sprite",
            SpellType.BOOMERANG       => "BoomerangIcon_Sprite",
            SpellType.DUAL_BLADES     => "DualBladesIcon_Sprite",
            SpellType.CLOSE           => "CloseIcon_Sprite",
            SpellType.DAMAGE_SHIELD   => "DamageShieldIcon_Sprite",
            SpellType.BOUNCE          => "BounceIcon_Sprite",
            SpellType.LASER           => "DaggerIcon_Sprite",
            SpellType.DRAGON_FIRE     => "DragonFireIcon_Sprite",
            SpellType.DRAGON_FIRE_NEO => "DragonFireIcon_Sprite",
            SpellType.RAPID_DAGGER    => "RapidDaggerIcon_Sprite",
            _                         => "DaggerIcon_Sprite",
        };
    }

    // TODO: Should be moved to a more relevant section?
    public static byte[] GetNext3Spells()
    {
        var spellArray = ClassType.GetSpellList(ClassType.WIZARD2);
        List<byte> spellList = spellArray.ToList();

        var spellIndex = spellList.IndexOf(Game.PlayerStats.Spell);
        spellList.Clear();

        var wizardSpells = new byte[3];
        for (var i = 0; i < 3; i++)
        {
            wizardSpells[i] = spellArray[spellIndex];
            spellIndex++;
            if (spellIndex >= spellArray.Length)
            {
                spellIndex = 0;
            }
        }

        return wizardSpells;
    }

    public struct SpellDefinition(string name, int cost, float damage, float x, float y, int rarity)
    {
        public readonly string Name = name; // Not really used.
        public readonly int Cost = cost;
        public readonly float Damage = damage;
        public readonly float XValue = x;
        public readonly float YValue = y;
        public readonly int Rarity = rarity;
    }
}
