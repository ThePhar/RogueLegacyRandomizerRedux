using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.EnvironmentVariables;
using RogueCastle.GameStructs;
using RogueCastle.LogicActions;
using RogueCastle.Managers;
using RogueCastle.Screens;
using RogueCastle.Screens.BaseScreens;
using Tweener;
using Tweener.Ease;

namespace RogueCastle.GameObjects.EnemyObjs;

public class EnemyObj_LastBoss : EnemyObj {
    private const float AXE_PROJECTILE_SPEED = 1100;
    private readonly Vector2 _axeSpellScale = new(3.0f, 3.0f);
    private const float DAGGER_PROJECTILE_SPEED = 900;
    private readonly Vector2 _daggerSpellScale = new(3.5f, 3.5f);
    private const float CAST_DELAY = 0.25f;
    private readonly LogicBlock _cooldownLB = new();
    private readonly LogicBlock _damageShieldLB = new();
    private readonly LogicBlock _firstFormDashAwayLB = new();
    private readonly LogicBlock _generalAdvancedLB = new();
    private readonly LogicBlock _generalBasicLB = new();
    private readonly LogicBlock _generalBasicNeoLB = new();
    private const float LAST_BOSS_ATTACK_DELAY = 0.35f;
    private const int MEGA_SHIELD_DISTANCE = 525;
    private const float MEGA_SHIELD_SCALE = 4.0f;
    private const float MEGA_SHIELD_SPEED = 1.0f;
    private const int NUM_SPEARS = 26;
    private const int ORBS_EASY = 1;
    private const int ORBS_HARD = 3;
    private const int ORBS_NORMAL = 2;
    private readonly LogicBlock _secondFormCooldownLB = new();
    private const float SPEAR_DURATION = 1.75f;
    private const float SPELL_CLOSE_LIFESPAN = 6;
    private const float SPELL_CLOSE_SCALE = 3.5f;
    private readonly FrameSoundObj _walkDownSoundFinalBoss;
    private readonly FrameSoundObj _walkUpSoundFinalBoss;
    private const int MEGA_FLYING_DAGGER_PROJECTILE_SPEED = 2350;
    private const int MEGA_FLYING_SWORD_AMOUNT = 29;
    private const int MEGA_UPWARD_SWORD_PROJECTILE_AMOUNT = 8;
    private const int MEGA_UPWARD_SWORD_PROJECTILE_SPEED = 2450;
    private ProjectileData _axeProjData;
    private ProjectileData _daggerProjData;
    private List<ProjectileObj> _damageShieldProjectiles;
    private BlankObj _delayObj;
    private bool _firstFormDying;
    private bool _isDashing;
    private bool _isHurt;
    private bool _isNeo;
    private bool _neoDying;
    private float _smokeCounter = 0.05f;
    private float _teleportDuration;

    public EnemyObj_LastBoss(
        PlayerObj target,
        PhysicsManager physicsManager,
        ProceduralLevelScreen levelToAttachTo,
        GameTypes.EnemyDifficulty difficulty
    ) : base("PlayerIdle_Character", target, physicsManager, levelToAttachTo, difficulty) {
        foreach (var obj in _objectList) {
            obj.TextureColor = new Color(100, 100, 100);
        }

        Type = EnemyType.LAST_BOSS;

        _damageShieldProjectiles = [];

        _objectList[PlayerPart.BOOBS].Visible = false;
        _objectList[PlayerPart.EXTRA].Visible = false;
        _objectList[PlayerPart.LIGHT].Visible = false;
        _objectList[PlayerPart.GLASSES].Visible = false;
        _objectList[PlayerPart.BOWTIE].Visible = false;
        _objectList[PlayerPart.WINGS].Visible = false;

        var headPart = (_objectList[PlayerPart.HEAD] as IAnimateableObj)!.SpriteName;
        var numberIndex = headPart.IndexOf("_", StringComparison.Ordinal) - 1;
        headPart = headPart.Remove(numberIndex, 1);
        headPart = headPart.Replace("_", PlayerPart.INTRO_HELM + "_");
        _objectList[PlayerPart.HEAD].ChangeSprite(headPart);
        PlayAnimation();

        _delayObj = new BlankObj(0, 0);
        _walkDownSoundFinalBoss = new FrameSoundObj(this, 3, "FinalBoss_St2_Foot_01", "FinalBoss_St2_Foot_02", "FinalBoss_St2_Foot_03");
        _walkUpSoundFinalBoss = new FrameSoundObj(this, 6, "FinalBoss_St2_Foot_04", "FinalBoss_St2_Foot_05");
    }

    public bool IsSecondForm { get; private set; }

    public bool IsNeo {
        get => _isNeo;
        set {
            _isNeo = value;
            if (!value) {
                return;
            }

            HealthGainPerLevel = 0;
            DamageGainPerLevel = 0;
            ItemDropChance = 0;
            MoneyDropChance = 0;
            m_saveToEnemiesKilledList = false;
            CanFallOffLedges = true;
        }
    }

    protected override void InitializeEV() {
        #region Basic Variables - General

        Name = EnemyEV.LAST_BOSS_BASIC_NAME;
        LocStringID = EnemyEV.LAST_BOSS_BASIC_NAME_LOC_ID;
        MaxHealth = EnemyEV.LAST_BOSS_BASIC_MAX_HEALTH;
        Damage = EnemyEV.LAST_BOSS_BASIC_DAMAGE;
        XPValue = EnemyEV.LAST_BOSS_BASIC_XP_VALUE;
        MinMoneyDropAmount = EnemyEV.LAST_BOSS_BASIC_MIN_DROP_AMOUNT;
        MaxMoneyDropAmount = EnemyEV.LAST_BOSS_BASIC_MAX_DROP_AMOUNT;
        MoneyDropChance = EnemyEV.LAST_BOSS_BASIC_DROP_CHANCE;
        Speed = EnemyEV.LAST_BOSS_BASIC_SPEED;
        TurnSpeed = EnemyEV.LAST_BOSS_BASIC_TURN_SPEED;
        ProjectileSpeed = EnemyEV.LAST_BOSS_BASIC_PROJECTILE_SPEED;
        JumpHeight = EnemyEV.LAST_BOSS_BASIC_JUMP;
        CooldownTime = EnemyEV.LAST_BOSS_BASIC_COOLDOWN;
        AnimationDelay = 1 / EnemyEV.LAST_BOSS_BASIC_ANIMATION_DELAY;
        AlwaysFaceTarget = EnemyEV.LAST_BOSS_BASIC_ALWAYS_FACE_TARGET;
        CanFallOffLedges = EnemyEV.LAST_BOSS_BASIC_CAN_FALL_OFF_LEDGES;
        CanBeKnockedBack = EnemyEV.LAST_BOSS_BASIC_CAN_BE_KNOCKED_BACK;
        IsWeighted = EnemyEV.LAST_BOSS_BASIC_IS_WEIGHTED;
        Scale = EnemyEV.LastBossBasicScale;
        ProjectileScale = EnemyEV.LastBossBasicProjectileScale;
        TintablePart.TextureColor = EnemyEV.LastBossBasicTint;
        MeleeRadius = EnemyEV.LAST_BOSS_BASIC_MELEE_RADIUS;
        ProjectileRadius = EnemyEV.LAST_BOSS_BASIC_PROJECTILE_RADIUS;
        EngageRadius = EnemyEV.LAST_BOSS_BASIC_ENGAGE_RADIUS;
        ProjectileDamage = Damage;
        KnockBack = EnemyEV.LastBossBasicKnockBack;

        #endregion

        switch (Difficulty) {
            case GameTypes.EnemyDifficulty.Miniboss:

                #region Miniboss Variables - General

                Name = EnemyEV.LAST_BOSS_MINIBOSS_NAME;
                LocStringID = EnemyEV.LAST_BOSS_MINIBOSS_NAME_LOC_ID;
                MaxHealth = EnemyEV.LAST_BOSS_MINIBOSS_MAX_HEALTH;
                Damage = EnemyEV.LAST_BOSS_MINIBOSS_DAMAGE;
                XPValue = EnemyEV.LAST_BOSS_MINIBOSS_XP_VALUE;
                MinMoneyDropAmount = EnemyEV.LAST_BOSS_MINIBOSS_MIN_DROP_AMOUNT;
                MaxMoneyDropAmount = EnemyEV.LAST_BOSS_MINIBOSS_MAX_DROP_AMOUNT;
                MoneyDropChance = EnemyEV.LAST_BOSS_MINIBOSS_DROP_CHANCE;
                Speed = EnemyEV.LAST_BOSS_MINIBOSS_SPEED;
                TurnSpeed = EnemyEV.LAST_BOSS_MINIBOSS_TURN_SPEED;
                ProjectileSpeed = EnemyEV.LAST_BOSS_MINIBOSS_PROJECTILE_SPEED;
                JumpHeight = EnemyEV.LAST_BOSS_MINIBOSS_JUMP;
                CooldownTime = EnemyEV.LAST_BOSS_MINIBOSS_COOLDOWN;
                AnimationDelay = 1 / EnemyEV.LAST_BOSS_MINIBOSS_ANIMATION_DELAY;
                AlwaysFaceTarget = EnemyEV.LAST_BOSS_MINIBOSS_ALWAYS_FACE_TARGET;
                CanFallOffLedges = EnemyEV.LAST_BOSS_MINIBOSS_CAN_FALL_OFF_LEDGES;
                CanBeKnockedBack = EnemyEV.LAST_BOSS_MINIBOSS_CAN_BE_KNOCKED_BACK;
                IsWeighted = EnemyEV.LAST_BOSS_MINIBOSS_IS_WEIGHTED;
                Scale = EnemyEV.LastBossMinibossScale;
                ProjectileScale = EnemyEV.LastBossMinibossProjectileScale;
                TintablePart.TextureColor = EnemyEV.LastBossMinibossTint;
                MeleeRadius = EnemyEV.LAST_BOSS_MINIBOSS_MELEE_RADIUS;
                ProjectileRadius = EnemyEV.LAST_BOSS_MINIBOSS_PROJECTILE_RADIUS;
                EngageRadius = EnemyEV.LAST_BOSS_MINIBOSS_ENGAGE_RADIUS;
                ProjectileDamage = Damage;
                KnockBack = EnemyEV.LastBossMinibossKnockBack;

                #endregion

                break;

            case GameTypes.EnemyDifficulty.Expert:

                #region Expert Variables - General

                Name = EnemyEV.LAST_BOSS_EXPERT_NAME;
                LocStringID = EnemyEV.LAST_BOSS_EXPERT_NAME_LOC_ID;
                MaxHealth = EnemyEV.LAST_BOSS_EXPERT_MAX_HEALTH;
                Damage = EnemyEV.LAST_BOSS_EXPERT_DAMAGE;
                XPValue = EnemyEV.LAST_BOSS_EXPERT_XP_VALUE;
                MinMoneyDropAmount = EnemyEV.LAST_BOSS_EXPERT_MIN_DROP_AMOUNT;
                MaxMoneyDropAmount = EnemyEV.LAST_BOSS_EXPERT_MAX_DROP_AMOUNT;
                MoneyDropChance = EnemyEV.LAST_BOSS_EXPERT_DROP_CHANCE;
                Speed = EnemyEV.LAST_BOSS_EXPERT_SPEED;
                TurnSpeed = EnemyEV.LAST_BOSS_EXPERT_TURN_SPEED;
                ProjectileSpeed = EnemyEV.LAST_BOSS_EXPERT_PROJECTILE_SPEED;
                JumpHeight = EnemyEV.LAST_BOSS_EXPERT_JUMP;
                CooldownTime = EnemyEV.LAST_BOSS_EXPERT_COOLDOWN;
                AnimationDelay = 1 / EnemyEV.LAST_BOSS_EXPERT_ANIMATION_DELAY;
                AlwaysFaceTarget = EnemyEV.LAST_BOSS_EXPERT_ALWAYS_FACE_TARGET;
                CanFallOffLedges = EnemyEV.LAST_BOSS_EXPERT_CAN_FALL_OFF_LEDGES;
                CanBeKnockedBack = EnemyEV.LAST_BOSS_EXPERT_CAN_BE_KNOCKED_BACK;
                IsWeighted = EnemyEV.LAST_BOSS_EXPERT_IS_WEIGHTED;
                Scale = EnemyEV.LastBossExpertScale;
                ProjectileScale = EnemyEV.LastBossExpertProjectileScale;
                TintablePart.TextureColor = EnemyEV.LastBossExpertTint;
                MeleeRadius = EnemyEV.LAST_BOSS_EXPERT_MELEE_RADIUS;
                ProjectileRadius = EnemyEV.LAST_BOSS_EXPERT_PROJECTILE_RADIUS;
                EngageRadius = EnemyEV.LAST_BOSS_EXPERT_ENGAGE_RADIUS;
                ProjectileDamage = Damage;
                KnockBack = EnemyEV.LastBossExpertKnockBack;

                #endregion

                break;

            case GameTypes.EnemyDifficulty.Advanced:

                #region Advanced Variables - General

                Name = EnemyEV.LAST_BOSS_ADVANCED_NAME;
                LocStringID = EnemyEV.LAST_BOSS_ADVANCED_NAME_LOC_ID;
                MaxHealth = EnemyEV.LAST_BOSS_ADVANCED_MAX_HEALTH;
                Damage = EnemyEV.LAST_BOSS_ADVANCED_DAMAGE;
                XPValue = EnemyEV.LAST_BOSS_ADVANCED_XP_VALUE;
                MinMoneyDropAmount = EnemyEV.LAST_BOSS_ADVANCED_MIN_DROP_AMOUNT;
                MaxMoneyDropAmount = EnemyEV.LAST_BOSS_ADVANCED_MAX_DROP_AMOUNT;
                MoneyDropChance = EnemyEV.LAST_BOSS_ADVANCED_DROP_CHANCE;
                Speed = EnemyEV.LAST_BOSS_ADVANCED_SPEED;
                TurnSpeed = EnemyEV.LAST_BOSS_ADVANCED_TURN_SPEED;
                ProjectileSpeed = EnemyEV.LAST_BOSS_ADVANCED_PROJECTILE_SPEED;
                JumpHeight = EnemyEV.LAST_BOSS_ADVANCED_JUMP;
                CooldownTime = EnemyEV.LAST_BOSS_ADVANCED_COOLDOWN;
                AnimationDelay = 1 / EnemyEV.LAST_BOSS_ADVANCED_ANIMATION_DELAY;
                AlwaysFaceTarget = EnemyEV.LAST_BOSS_ADVANCED_ALWAYS_FACE_TARGET;
                CanFallOffLedges = EnemyEV.LAST_BOSS_ADVANCED_CAN_FALL_OFF_LEDGES;
                CanBeKnockedBack = EnemyEV.LAST_BOSS_ADVANCED_CAN_BE_KNOCKED_BACK;
                IsWeighted = EnemyEV.LAST_BOSS_ADVANCED_IS_WEIGHTED;
                Scale = EnemyEV.LastBossAdvancedScale;
                ProjectileScale = EnemyEV.LastBossAdvancedProjectileScale;
                TintablePart.TextureColor = EnemyEV.LastBossAdvancedTint;
                MeleeRadius = EnemyEV.LAST_BOSS_ADVANCED_MELEE_RADIUS;
                EngageRadius = EnemyEV.LAST_BOSS_ADVANCED_ENGAGE_RADIUS;
                ProjectileRadius = EnemyEV.LAST_BOSS_ADVANCED_PROJECTILE_RADIUS;
                ProjectileDamage = Damage;
                KnockBack = EnemyEV.LastBossAdvancedKnockBack;

                #endregion

                break;

            case GameTypes.EnemyDifficulty.Basic:
            default:
                AnimationDelay = 1 / 10f;
                if (LevelEV.WeakenBosses) {
                    MaxHealth = 1;
                }

                break;
        }
    }

    protected override void InitializeLogic() {
        #region First Form Logic

        //////////////////////////////// FIRST FORM LOGIC
        var walkTowardsLS = new LogicSet(this);
        walkTowardsLS.AddAction(new DebugTraceLogicAction("WalkTowardSLS"));
        walkTowardsLS.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true));
        walkTowardsLS.AddAction(new GroundCheckLogicAction());
        walkTowardsLS.AddAction(new ChangeSpriteLogicAction("PlayerWalking_Character"));
        walkTowardsLS.AddAction(new MoveLogicAction(m_target, true));
        walkTowardsLS.AddAction(new LockFaceDirectionLogicAction(true));
        walkTowardsLS.AddAction(new DelayLogicAction(0.3f, 0.75f));
        walkTowardsLS.AddAction(new LockFaceDirectionLogicAction(false));

        var walkAwayLS = new LogicSet(this);
        walkAwayLS.AddAction(new DebugTraceLogicAction("WalkAway"));
        walkAwayLS.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true));
        walkAwayLS.AddAction(new GroundCheckLogicAction());
        walkAwayLS.AddAction(new ChangeSpriteLogicAction("PlayerWalking_Character"));
        walkAwayLS.AddAction(new MoveLogicAction(m_target, false));
        walkAwayLS.AddAction(new DelayLogicAction(0.2f, 0.75f));
        walkAwayLS.AddAction(new LockFaceDirectionLogicAction(false));

        var walkStopLS = new LogicSet(this);
        walkStopLS.AddAction(new DebugTraceLogicAction("walkStop"));
        walkStopLS.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true));
        walkStopLS.AddAction(new GroundCheckLogicAction());
        walkStopLS.AddAction(new ChangeSpriteLogicAction("PlayerIdle_Character"));
        walkStopLS.AddAction(new MoveLogicAction(m_target, true, 0));
        walkStopLS.AddAction(new DelayLogicAction(0.25f, 0.5f));

        var attackLS = new LogicSet(this);
        attackLS.AddAction(new DebugTraceLogicAction("attack"));
        attackLS.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true));
        attackLS.AddAction(new MoveLogicAction(m_target, true, 0));
        attackLS.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 1 / 20f));
        attackLS.AddAction(new LockFaceDirectionLogicAction(true));
        attackLS.AddAction(new ChangeSpriteLogicAction("PlayerAttacking3_Character", false, false));
        attackLS.AddAction(new PlayAnimationLogicAction(2, 4));
        attackLS.AddAction(new Play3DSoundLogicAction(this, m_target, "Player_Attack01", "Player_Attack02"));
        attackLS.AddAction(new PlayAnimationLogicAction("AttackStart", "End"));
        attackLS.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 1 / 10f));
        attackLS.AddAction(new ChangeSpriteLogicAction("PlayerIdle_Character"));
        attackLS.AddAction(new LockFaceDirectionLogicAction(false));
        attackLS.Tag = GameTypes.LOGIC_SET_TYPE_ATTACK;

        var moveAttackLS = new LogicSet(this);
        moveAttackLS.AddAction(new DebugTraceLogicAction("moveattack"));
        moveAttackLS.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true));
        moveAttackLS.AddAction(new MoveLogicAction(m_target, true));
        moveAttackLS.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 1 / 20f));
        moveAttackLS.AddAction(new LockFaceDirectionLogicAction(true));
        moveAttackLS.AddAction(new ChangeSpriteLogicAction("PlayerAttacking3_Character", false, false));
        moveAttackLS.AddAction(new PlayAnimationLogicAction(2, 4));
        moveAttackLS.AddAction(new Play3DSoundLogicAction(this, m_target, "Player_Attack01", "Player_Attack02"));
        moveAttackLS.AddAction(new PlayAnimationLogicAction("AttackStart", "End"));
        moveAttackLS.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 1 / 10f));
        moveAttackLS.AddAction(new ChangeSpriteLogicAction("PlayerIdle_Character"));
        moveAttackLS.AddAction(new LockFaceDirectionLogicAction(false));
        moveAttackLS.Tag = GameTypes.LOGIC_SET_TYPE_ATTACK;

        var throwShieldLS = new LogicSet(this);
        throwShieldLS.AddAction(new DebugTraceLogicAction("Throwing Daggers"));
        throwShieldLS.AddAction(new MoveLogicAction(m_target, true, 0));
        throwShieldLS.AddAction(new LockFaceDirectionLogicAction(true));
        throwShieldLS.AddAction(new ChangeSpriteLogicAction("PlayerLevelUp_Character"));
        throwShieldLS.AddAction(new PlayAnimationLogicAction(false));
        throwShieldLS.AddAction(new RunFunctionLogicAction(this, "CastCloseShield"));
        throwShieldLS.AddAction(new LockFaceDirectionLogicAction(false));
        throwShieldLS.Tag = GameTypes.LOGIC_SET_TYPE_ATTACK;

        var throwDaggerLS = new LogicSet(this);
        throwDaggerLS.AddAction(new DebugTraceLogicAction("Throwing Daggers"));
        throwDaggerLS.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true));
        throwDaggerLS.AddAction(new MoveLogicAction(m_target, true, 0));
        throwDaggerLS.AddAction(new LockFaceDirectionLogicAction(true));
        throwDaggerLS.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 1 / 30f));
        throwDaggerLS.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", false));
        throwDaggerLS.AddAction(new ChangeSpriteLogicAction("PlayerLevelUp_Character"));
        throwDaggerLS.AddAction(new PlayAnimationLogicAction(false));
        throwDaggerLS.AddAction(new RunFunctionLogicAction(this, "ThrowDaggerProjectiles"));
        throwDaggerLS.AddAction(new DelayLogicAction(0.25f));
        throwDaggerLS.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true));
        throwDaggerLS.AddAction(new LockFaceDirectionLogicAction(false));
        throwDaggerLS.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 1 / 10f));
        throwDaggerLS.Tag = GameTypes.LOGIC_SET_TYPE_ATTACK;

        #region NEODaggers

        var throwDaggerNeoLS = new LogicSet(this);
        throwDaggerNeoLS.AddAction(new DebugTraceLogicAction("Throwing Daggers"));
        throwDaggerNeoLS.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true));
        throwDaggerNeoLS.AddAction(new MoveLogicAction(m_target, true, 0));
        throwDaggerNeoLS.AddAction(new LockFaceDirectionLogicAction(true));
        throwDaggerNeoLS.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 1 / 30f));
        throwDaggerNeoLS.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", false));
        throwDaggerNeoLS.AddAction(new ChangeSpriteLogicAction("PlayerLevelUp_Character"));
        throwDaggerNeoLS.AddAction(new PlayAnimationLogicAction(false));
        throwDaggerNeoLS.AddAction(new RunFunctionLogicAction(this, "ThrowDaggerProjectilesNeo"));
        throwDaggerNeoLS.AddAction(new DelayLogicAction(0.25f));
        throwDaggerNeoLS.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true));
        throwDaggerNeoLS.AddAction(new LockFaceDirectionLogicAction(false));
        throwDaggerNeoLS.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 1 / 10f));
        throwDaggerNeoLS.Tag = GameTypes.LOGIC_SET_TYPE_ATTACK;

        #endregion

        var jumpLS = new LogicSet(this);
        jumpLS.AddAction(new DebugTraceLogicAction("jumpLS"));
        jumpLS.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true));
        jumpLS.AddAction(new GroundCheckLogicAction());
        jumpLS.AddAction(new MoveLogicAction(m_target, true));
        jumpLS.AddAction(new LockFaceDirectionLogicAction(true));
        jumpLS.AddAction(new Play3DSoundLogicAction(this, m_target, "Player_Jump"));
        jumpLS.AddAction(new JumpLogicAction());
        jumpLS.AddAction(new DelayLogicAction(0.2f));
        jumpLS.AddAction(new RunFunctionLogicAction(this, "ThrowAxeProjectiles"));
        jumpLS.AddAction(new DelayLogicAction(0.75f));
        jumpLS.AddAction(new LockFaceDirectionLogicAction(false));
        jumpLS.AddAction(new GroundCheckLogicAction());

        #region NEOJump

        var jumpNeoLS = new LogicSet(this);
        jumpNeoLS.AddAction(new DebugTraceLogicAction("jumpLS"));
        jumpNeoLS.AddAction(new ChangePropertyLogicAction(this, "CanBeKnockedBack", true));
        jumpNeoLS.AddAction(new GroundCheckLogicAction());
        jumpNeoLS.AddAction(new MoveLogicAction(m_target, true));
        jumpNeoLS.AddAction(new LockFaceDirectionLogicAction(true));
        jumpNeoLS.AddAction(new Play3DSoundLogicAction(this, m_target, "Player_Jump"));
        jumpNeoLS.AddAction(new JumpLogicAction());
        jumpNeoLS.AddAction(new DelayLogicAction(0.2f));
        jumpNeoLS.AddAction(new RunFunctionLogicAction(this, "ThrowAxeProjectilesNeo"));
        jumpNeoLS.AddAction(new DelayLogicAction(0.75f));
        jumpNeoLS.AddAction(new LockFaceDirectionLogicAction(false));
        jumpNeoLS.AddAction(new GroundCheckLogicAction());

        #endregion

        var dashLS = new LogicSet(this);
        dashLS.AddAction(new DebugTraceLogicAction("dashLS"));
        dashLS.AddAction(new RunFunctionLogicAction(this, "CastCloseShield"));
        dashLS.AddAction(new RunFunctionLogicAction(this, "Dash", 0));
        dashLS.AddAction(new DelayLogicAction(0.25f));
        dashLS.AddAction(new RunFunctionLogicAction(this, "DashComplete"));

        var dashRightLS = new LogicSet(this);
        dashRightLS.AddAction(new DebugTraceLogicAction("dashAwayRightLS"));
        dashRightLS.AddAction(new RunFunctionLogicAction(this, "Dash", 1));
        dashRightLS.AddAction(new DelayLogicAction(0.25f));
        dashRightLS.AddAction(new RunFunctionLogicAction(this, "DashComplete"));

        var dashLeftLS = new LogicSet(this);
        dashLeftLS.AddAction(new DebugTraceLogicAction("dashAwayLeftLS"));
        dashLeftLS.AddAction(new RunFunctionLogicAction(this, "Dash", -1));
        dashLeftLS.AddAction(new DelayLogicAction(0.25f));
        dashLeftLS.AddAction(new RunFunctionLogicAction(this, "DashComplete"));

        #endregion

        #region SecondBoss

        ////////////////////////////// SECOND FORM LOGIC
        var walkTowardsSF = new LogicSet(this);
        walkTowardsSF.AddAction(new GroundCheckLogicAction());
        walkTowardsSF.AddAction(new ChangeSpriteLogicAction("EnemyLastBossRun_Character"));
        walkTowardsSF.AddAction(new MoveLogicAction(m_target, true));
        walkTowardsSF.AddAction(new LockFaceDirectionLogicAction(true));
        walkTowardsSF.AddAction(new DelayLogicAction(0.35f, 1.15f));
        walkTowardsSF.AddAction(new LockFaceDirectionLogicAction(false));

        var walkAwaySF = new LogicSet(this);
        walkAwaySF.AddAction(new GroundCheckLogicAction());
        walkAwaySF.AddAction(new ChangeSpriteLogicAction("EnemyLastBossRun_Character"));
        walkAwaySF.AddAction(new MoveLogicAction(m_target, false));
        //walkAwayLS.AddAction(new LockFaceDirectionLogicAction(true));
        walkAwaySF.AddAction(new DelayLogicAction(0.2f, 1.0f));
        walkAwaySF.AddAction(new LockFaceDirectionLogicAction(false));

        var walkStopSF = new LogicSet(this);
        walkStopSF.AddAction(new GroundCheckLogicAction());
        walkStopSF.AddAction(new ChangeSpriteLogicAction("EnemyLastBossIdle_Character"));
        walkStopSF.AddAction(new MoveLogicAction(m_target, true, 0));
        walkStopSF.AddAction(new DelayLogicAction(0.2f, 0.5f));

        var attackSF = new LogicSet(this);
        attackSF.AddAction(new MoveLogicAction(m_target, true, 0));
        attackSF.AddAction(new LockFaceDirectionLogicAction(true));
        attackSF.AddAction(new ChangeSpriteLogicAction("EnemyLastBossAttack_Character", false, false));
        attackSF.AddAction(new PlayAnimationLogicAction("Start", "BeforeAttack"));
        attackSF.AddAction(new DelayLogicAction(LAST_BOSS_ATTACK_DELAY));
        attackSF.AddAction(new Play3DSoundLogicAction(this, m_target, "FinalBoss_St2_SwordSwing"));
        attackSF.AddAction(new Play3DSoundLogicAction(this, m_target, "FinalBoss_St2_Effort_01", "FinalBoss_St2_Effort_02", "FinalBoss_St2_Effort_03", "FinalBoss_St2_Effort_04", "FinalBoss_St2_Effort_05"));
        attackSF.AddAction(new PlayAnimationLogicAction("Attack", "End"));
        attackSF.AddAction(new ChangeSpriteLogicAction("EnemyLastBossIdle_Character"));
        attackSF.AddAction(new LockFaceDirectionLogicAction(false));

        var castSpearsSF = new LogicSet(this);
        RunTeleportLS(castSpearsSF, "Centre");
        castSpearsSF.AddAction(new ChangeSpriteLogicAction("EnemyLastBossSpell_Character", false, false));
        castSpearsSF.AddAction(new Play3DSoundLogicAction(this, m_target, "FinalBoss_St2_SwordSlam_Prime"));
        castSpearsSF.AddAction(new PlayAnimationLogicAction("Start", "BeforeCast"));
        castSpearsSF.AddAction(new DelayLogicAction(CAST_DELAY));
        castSpearsSF.AddAction(new RunFunctionLogicAction(this, "CastSpears", NUM_SPEARS, SPEAR_DURATION));
        castSpearsSF.AddAction(new Play3DSoundLogicAction(this, m_target, "FinalBoss_St2_SwordSlam"));
        castSpearsSF.AddAction(new PlayAnimationLogicAction("BeforeCast", "End"));
        castSpearsSF.AddAction(new DelayLogicAction(SPEAR_DURATION + 1));

        var castRandomSwordsSF = new LogicSet(this);
        castRandomSwordsSF.AddAction(new ChangePropertyLogicAction(this, "CurrentSpeed", 0));
        castRandomSwordsSF.AddAction(new ChangeSpriteLogicAction("EnemyLastBossSpell2_Character", false, false));
        castRandomSwordsSF.AddAction(new Play3DSoundLogicAction(this, m_target, "FinalBoss_St2_SwordSummon_a"));
        castRandomSwordsSF.AddAction(new PlayAnimationLogicAction("Start", "Cast"));
        castRandomSwordsSF.AddAction(new DelayLogicAction(CAST_DELAY));
        castRandomSwordsSF.AddAction(new RunFunctionLogicAction(this, "CastSwordsRandom"));
        castRandomSwordsSF.AddAction(new PlayAnimationLogicAction("Cast", "End"));
        castRandomSwordsSF.AddAction(new DelayLogicAction(1));

        var castSwordsLeftSF = new LogicSet(this);
        castSwordsLeftSF.AddAction(new LockFaceDirectionLogicAction(true, 1));
        RunTeleportLS(castSwordsLeftSF, "Left");
        castSwordsLeftSF.AddAction(new ChangeSpriteLogicAction("EnemyLastBossSpell_Character", false, false));
        castSwordsLeftSF.AddAction(new Play3DSoundLogicAction(this, m_target, "FinalBoss_St2_SwordSlam_Prime"));
        castSwordsLeftSF.AddAction(new PlayAnimationLogicAction("Start", "BeforeCast"));
        castSwordsLeftSF.AddAction(new DelayLogicAction(CAST_DELAY));
        castSwordsLeftSF.AddAction(new RunFunctionLogicAction(this, "CastSwords", true));
        castSwordsLeftSF.AddAction(new Play3DSoundLogicAction(this, m_target, "FinalBoss_St2_SwordSlam"));
        castSwordsLeftSF.AddAction(new PlayAnimationLogicAction("BeforeCast", "End"));
        castSwordsLeftSF.AddAction(new DelayLogicAction(1));
        castSwordsLeftSF.AddAction(new LockFaceDirectionLogicAction(false));

        var castSwordRightSF = new LogicSet(this);
        castSwordRightSF.AddAction(new LockFaceDirectionLogicAction(true, -1));
        RunTeleportLS(castSwordRightSF, "Right");
        castSwordRightSF.AddAction(new ChangeSpriteLogicAction("EnemyLastBossSpell_Character", false, false));
        castSwordRightSF.AddAction(new Play3DSoundLogicAction(this, m_target, "FinalBoss_St2_SwordSlam_Prime"));
        castSwordRightSF.AddAction(new PlayAnimationLogicAction("Start", "BeforeCast"));
        castSwordRightSF.AddAction(new Play3DSoundLogicAction(this, m_target, "FinalBoss_St2_BlockLaugh"));
        castSwordRightSF.AddAction(new DelayLogicAction(CAST_DELAY));
        castSwordRightSF.AddAction(new RunFunctionLogicAction(this, "CastSwords", false));
        castSwordRightSF.AddAction(new Play3DSoundLogicAction(this, m_target, "FinalBoss_St2_SwordSlam"));
        castSwordRightSF.AddAction(new PlayAnimationLogicAction("BeforeCast", "End"));
        castSwordRightSF.AddAction(new DelayLogicAction(1));
        castSwordRightSF.AddAction(new LockFaceDirectionLogicAction(false));

        var castShield1SF = new LogicSet(this);
        castShield1SF.AddAction(new RunFunctionLogicAction(this, "CastDamageShield", ORBS_EASY));
        castShield1SF.AddAction(new LockFaceDirectionLogicAction(false));

        var castShield2SF = new LogicSet(this);
        castShield2SF.AddAction(new RunFunctionLogicAction(this, "CastDamageShield", ORBS_NORMAL));
        castShield2SF.AddAction(new LockFaceDirectionLogicAction(false));

        var castShield3SF = new LogicSet(this);
        castShield3SF.AddAction(new RunFunctionLogicAction(this, "CastDamageShield", ORBS_HARD));
        castShield3SF.AddAction(new DelayLogicAction(0));
        castShield3SF.AddAction(new LockFaceDirectionLogicAction(false));

        #endregion

        _generalBasicLB.AddLogicSet(walkTowardsLS, walkAwayLS, walkStopLS, jumpLS, moveAttackLS, throwShieldLS, throwDaggerLS, dashLS);
        _generalAdvancedLB.AddLogicSet(walkTowardsSF, walkAwaySF, walkStopSF, attackSF, castSpearsSF, castRandomSwordsSF, castSwordsLeftSF, castSwordRightSF);
        _damageShieldLB.AddLogicSet(castShield1SF, castShield2SF, castShield3SF);
        _cooldownLB.AddLogicSet(walkTowardsLS, walkAwayLS, walkStopLS, jumpLS, moveAttackLS, throwShieldLS, throwDaggerLS, dashLS);
        _secondFormCooldownLB.AddLogicSet(walkTowardsSF, walkAwaySF, walkStopSF);
        _generalBasicNeoLB.AddLogicSet(walkTowardsLS, walkAwayLS, walkStopLS, jumpNeoLS, moveAttackLS, throwShieldLS, throwDaggerNeoLS, dashLS);

        logicBlocksToDispose.Add(_generalBasicLB);
        logicBlocksToDispose.Add(_generalAdvancedLB);
        logicBlocksToDispose.Add(_damageShieldLB);
        logicBlocksToDispose.Add(_cooldownLB);
        logicBlocksToDispose.Add(_secondFormCooldownLB);
        logicBlocksToDispose.Add(_generalBasicNeoLB);

        // Special logic block to get out of corners.
        _firstFormDashAwayLB.AddLogicSet(dashLeftLS, dashRightLS);
        logicBlocksToDispose.Add(_firstFormDashAwayLB);

        // walkTowardsLS, walkAwayLS, walkStopLS, jumpLS, moveAttackLS, throwShieldLS, throwDaggerLS, dashLS
        SetCooldownLogicBlock(_cooldownLB, 70, 0, 30, 0, 0, 0, 0, 0);
        base.InitializeLogic();
    }

    private void RunTeleportLS(LogicSet logicSet, string roomPosition) {
        logicSet.AddAction(new ChangePropertyLogicAction(this, "IsCollidable", false));
        logicSet.AddAction(new ChangePropertyLogicAction(this, "IsWeighted", false));
        logicSet.AddAction(new ChangePropertyLogicAction(this, "Opacity", 0.5f));
        logicSet.AddAction(new ChangePropertyLogicAction(this, "CurrentSpeed", 0));
        logicSet.AddAction(new ChangeSpriteLogicAction("EnemyLastBossTeleport_Character", false, false));
        logicSet.AddAction(new Play3DSoundLogicAction(this, m_target, "FinalBoss_St2_BlockAction"));
        logicSet.AddAction(new DelayLogicAction(0.25f));
        logicSet.AddAction(new RunFunctionLogicAction(this, "TeleportTo", roomPosition));
        logicSet.AddAction(new DelayObjLogicAction(_delayObj));
        logicSet.AddAction(new ChangePropertyLogicAction(this, "IsCollidable", true));
        logicSet.AddAction(new ChangePropertyLogicAction(this, "IsWeighted", true));
        logicSet.AddAction(new ChangePropertyLogicAction(this, "Opacity", 1));
    }

    public void ThrowAxeProjectiles() {
        if (_axeProjData != null) {
            _axeProjData.Dispose();
            _axeProjData = null;
        }

        _axeProjData = new ProjectileData(this) {
            SpriteName = "SpellAxe_Sprite",
            SourceAnchor = new Vector2(20, -20),
            Target = null,
            Speed = new Vector2(AXE_PROJECTILE_SPEED, AXE_PROJECTILE_SPEED),
            IsWeighted = true,
            RotationSpeed = 10,
            Damage = Damage,
            AngleOffset = 0,
            Angle = new Vector2(-90, -90),
            CollidesWithTerrain = false,
            Scale = _axeSpellScale,
        };

        Tween.RunFunction(0, this, "CastAxe", false);
        Tween.RunFunction(0.15f, this, "CastAxe", true);
        Tween.RunFunction(0.3f, this, "CastAxe", true);
        Tween.RunFunction(0.45f, this, "CastAxe", true);
        Tween.RunFunction(0.6f, this, "CastAxe", true);
    }

    public void ThrowAxeProjectilesNeo() {
        if (_axeProjData != null) {
            _axeProjData.Dispose();
            _axeProjData = null;
        }

        _axeProjData = new ProjectileData(this) {
            SpriteName = "SpellAxe_Sprite",
            SourceAnchor = new Vector2(20, -20),
            Target = null,
            Speed = new Vector2(AXE_PROJECTILE_SPEED, AXE_PROJECTILE_SPEED),
            IsWeighted = true,
            RotationSpeed = 10,
            Damage = Damage,
            AngleOffset = 0,
            Angle = new Vector2(-90, -90),
            CollidesWithTerrain = false,
            Scale = _axeSpellScale,
        };

        Tween.RunFunction(0.3f, this, "CastAxe", true);
        Tween.RunFunction(0.3f, this, "CastAxe", true);
        Tween.RunFunction(0.3f, this, "CastAxe", true);
    }

    public void CastAxe(bool randomize) {
        if (randomize) {
            _axeProjData.AngleOffset = CDGMath.RandomInt(-70, 70);
        }

        m_levelScreen.ProjectileManager.FireProjectile(_axeProjData);
        SoundManager.Play3DSound(this, m_target, "Cast_Axe");
        m_levelScreen.ImpactEffectPool.LastBossSpellCastEffect(this, 45, true);
    }

    public void ThrowDaggerProjectiles() {
        if (_daggerProjData != null) {
            _daggerProjData.Dispose();
            _daggerProjData = null;
        }

        _daggerProjData = new ProjectileData(this) {
            SpriteName = "SpellDagger_Sprite",
            SourceAnchor = Vector2.Zero,
            Target = m_target,
            Speed = new Vector2(DAGGER_PROJECTILE_SPEED, DAGGER_PROJECTILE_SPEED),
            IsWeighted = false,
            RotationSpeed = 0,
            Damage = Damage,
            AngleOffset = 0,
            CollidesWithTerrain = false,
            Scale = _daggerSpellScale,
        };

        Tween.RunFunction(0, this, "CastDaggers", false);
        Tween.RunFunction(0.05f, this, "CastDaggers", true);
        Tween.RunFunction(0.1f, this, "CastDaggers", true);
        Tween.RunFunction(0.15f, this, "CastDaggers", true);
        Tween.RunFunction(0.2f, this, "CastDaggers", true);
    }

    public void ThrowDaggerProjectilesNeo() {
        if (_daggerProjData != null) {
            _daggerProjData.Dispose();
            _daggerProjData = null;
        }

        _daggerProjData = new ProjectileData(this) {
            SpriteName = "SpellDagger_Sprite",
            SourceAnchor = Vector2.Zero,
            Target = m_target,
            Speed = new Vector2(DAGGER_PROJECTILE_SPEED - 160, DAGGER_PROJECTILE_SPEED - 160),
            IsWeighted = false,
            RotationSpeed = 0,
            Damage = Damage,
            AngleOffset = 0,
            CollidesWithTerrain = false,
            Scale = _daggerSpellScale,
        };

        Tween.RunFunction(0, this, "CastDaggers", false);
        Tween.RunFunction(0.05f, this, "CastDaggers", true);
        Tween.RunFunction(0.1f, this, "CastDaggers", true);
    }

    public void CastDaggers(bool randomize) {
        if (randomize) {
            _daggerProjData.AngleOffset = CDGMath.RandomInt(-8, 8);
        }

        m_levelScreen.ProjectileManager.FireProjectile(_daggerProjData);
        SoundManager.Play3DSound(this, m_target, "Cast_Dagger");
        m_levelScreen.ImpactEffectPool.LastBossSpellCastEffect(this, 0, true);
    }

    public void CastCloseShield() {
        var projData = new ProjectileData(this) {
            SpriteName = "SpellClose_Sprite",
            Speed = new Vector2(0, 0),
            IsWeighted = false,
            RotationSpeed = 0f,
            DestroysWithEnemy = false,
            DestroysWithTerrain = false,
            CollidesWithTerrain = false,
            Scale = new Vector2(SPELL_CLOSE_SCALE, SPELL_CLOSE_SCALE),
            Damage = Damage,
            Lifespan = SPELL_CLOSE_LIFESPAN,
            LockPosition = true,
        };

        m_levelScreen.ProjectileManager.FireProjectile(projData);
        SoundManager.Play3DSound(this, m_target, "Cast_GiantSword");
        m_levelScreen.ImpactEffectPool.LastBossSpellCastEffect(this, 90, true);
        projData.Dispose();
    }

    public void TeleportTo(string roomPosition) {
        float xDistance = roomPosition switch {
            "Left"   => m_levelScreen.CurrentRoom.Bounds.Left + 200,
            "Right"  => m_levelScreen.CurrentRoom.Bounds.Right - 200,
            "Centre" => m_levelScreen.CurrentRoom.Bounds.Center.X,
            _        => 0,
        };

        var position = new Vector2(xDistance, Y);

        var totalMovement = Math.Abs(CDGMath.DistanceBetweenPts(Position, position));
        _teleportDuration = totalMovement * 0.001f;
        _delayObj.X = _teleportDuration; // Delay hack.
        Tween.To(this, _teleportDuration, Quad.EaseInOut, "X", $"{position.X}");
        SoundManager.Play3DSound(this, m_target, "FinalBoss_St2_BlockMove");
    }

    public void CastSwords(bool castLeft) {
        var swordData = new ProjectileData(this) {
            SpriteName = "LastBossSwordProjectile_Sprite",
            Target = null,
            Speed = new Vector2(0, 0),
            IsWeighted = false,
            RotationSpeed = 0,
            Damage = Damage,
            StartingRotation = 0,
            AngleOffset = 0,
            CollidesWithTerrain = false,
            DestroysWithEnemy = false,
        };

        float delay = 1;
        var projSpeed = MEGA_FLYING_DAGGER_PROJECTILE_SPEED;
        if (castLeft == false) {
            projSpeed = MEGA_FLYING_DAGGER_PROJECTILE_SPEED * -1;
        }

        SoundManager.Play3DSound(this, m_target, "FinalBoss_St2_SwordSummon_b");
        for (var i = 0; i < MEGA_FLYING_SWORD_AMOUNT; i++) {
            var spellPos = new Vector2(X, Y + CDGMath.RandomInt(-1320, 100));
            var proj = m_levelScreen.ProjectileManager.FireProjectile(swordData);
            proj.Position = spellPos;
            Tween.By(proj, 2.5f, Tween.EaseNone, "delay", $"{delay}", "X", projSpeed.ToString());
            Tween.AddEndHandlerToLastTween(proj, "KillProjectile");
            Tween.RunFunction(delay, typeof(SoundManager), "Play3DSound", this, m_target, new[] { "FinalBoss_St2_SwordSummon_c_01", "FinalBoss_St2_SwordSummon_c_02", "FinalBoss_St2_SwordSummon_c_03", "FinalBoss_St2_SwordSummon_c_04", "FinalBoss_St2_SwordSummon_c_05", "FinalBoss_St2_SwordSummon_c_06", "FinalBoss_St2_SwordSummon_c_07", "FinalBoss_St2_SwordSummon_c_08" });
            m_levelScreen.ImpactEffectPool.SpellCastEffect(spellPos, 0, false);
            delay += 0.075f;
        }
    }

    public void CastSpears(int numSpears, float duration) {
        var spearData = new ProjectileData(this) {
            SpriteName = "LastBossSpearProjectile_Sprite",
            Target = null,
            Speed = new Vector2(0, 0),
            IsWeighted = false,
            RotationSpeed = 0,
            Damage = Damage,
            StartingRotation = 0,
            AngleOffset = 0,
            CollidesWithTerrain = false,
            DestroysWithEnemy = false,
            ShowIcon = false,
            LockPosition = true,
            CanBeFusRohDahed = false,
        };

        var xOffsetRight = 0;
        var xOffsetLeft = 0;
        var delay = 0.5f;
        UpdateCollisionBoxes();
        var roomCentre = new Vector2(m_levelScreen.CurrentRoom.Bounds.Center.X, Y);

        for (var i = 0; i < numSpears; i++) {
            // Spears spreading right.
            var proj = m_levelScreen.ProjectileManager.FireProjectile(spearData);
            proj.Scale = new Vector2(2, 2);
            proj.X = roomCentre.X + 50 + xOffsetRight;
            proj.Y = Y + (Bounds.Bottom - Y);
            proj.StopAnimation();
            
            xOffsetRight += proj.Width;
            Tween.RunFunction(delay, typeof(SoundManager), "Play3DSound", this, m_target, new[] { "FinalBoss_St2_Lance_01", "FinalBoss_St2_Lance_02", "FinalBoss_St2_Lance_03", "FinalBoss_St2_Lance_04", "FinalBoss_St2_Lance_05", "FinalBoss_St2_Lance_06", "FinalBoss_St2_Lance_07", "FinalBoss_St2_Lance_08" });
            Tween.RunFunction(delay, proj, "PlayAnimation", "Before", "End", false);
            Tween.RunFunction(delay + duration, proj, "PlayAnimation", "Retract", "RetractComplete", false);
            Tween.RunFunction(delay + duration, typeof(SoundManager), "Play3DSound", this, m_target, new[] { "FinalBoss_St2_Lance_Retract_01", "FinalBoss_St2_Lance_Retract_02", "FinalBoss_St2_Lance_Retract_03", "FinalBoss_St2_Lance_Retract_04", "FinalBoss_St2_Lance_Retract_05", "FinalBoss_St2_Lance_Retract_06" });
            Tween.RunFunction(delay + duration + 1, proj, "KillProjectile");

            // Spears spreading left.
            var projLeft = m_levelScreen.ProjectileManager.FireProjectile(spearData);
            projLeft.Scale = new Vector2(2, 2);
            projLeft.X = roomCentre.X - 50 + xOffsetLeft;
            projLeft.Y = Y + (Bounds.Bottom - Y);
            projLeft.StopAnimation();

            xOffsetLeft -= projLeft.Width;
            Tween.RunFunction(delay, projLeft, "PlayAnimation", "Before", "End", false);
            Tween.RunFunction(delay + duration, projLeft, "PlayAnimation", "Retract", "RetractComplete", false);
            Tween.RunFunction(delay + duration + 1, projLeft, "KillProjectile");

            delay += 0.05f;
        }

        spearData.Dispose();
    }

    public void CastSwordsRandom() {
        var roomCentre = new Vector2(m_levelScreen.CurrentRoom.Bounds.Center.X, Y);
        UpdateCollisionBoxes();
        var swordData = new ProjectileData(this) {
            SpriteName = "LastBossSwordVerticalProjectile_Sprite",
            Target = null,
            Speed = new Vector2(0, 0),
            IsWeighted = false,
            RotationSpeed = 0,
            Damage = Damage,
            StartingRotation = 0,
            AngleOffset = 0,
            CollidesWithTerrain = false,
            DestroysWithEnemy = false,
            LockPosition = true,
        };

        var xOffsetRight = 0;
        var xOffsetLeft = 0;
        float delay = 1;
        for (var i = 0; i < MEGA_UPWARD_SWORD_PROJECTILE_AMOUNT; i++) {
            var proj = m_levelScreen.ProjectileManager.FireProjectile(swordData);
            proj.Scale = new Vector2(1.5f, 1.5f);
            proj.X = roomCentre.X + 50 + xOffsetRight;
            proj.Y = roomCentre.Y + (Bounds.Bottom - Y) + 120;
            proj.Opacity = 0;
            Tween.To(proj, 0.25f, Tween.EaseNone, "Opacity", "1");
            Tween.By(proj, 2.5f, Quad.EaseIn, "delay", $"{delay}", "Y", $"{-MEGA_UPWARD_SWORD_PROJECTILE_SPEED}");
            Tween.AddEndHandlerToLastTween(proj, "KillProjectile");

            xOffsetRight = CDGMath.RandomInt(50, 1000);

            var projLeft = m_levelScreen.ProjectileManager.FireProjectile(swordData);
            projLeft.Scale = new Vector2(2, 2);
            projLeft.X = roomCentre.X - 50 + xOffsetLeft;
            projLeft.Y = roomCentre.Y + (Bounds.Bottom - Y) + 120;
            projLeft.Opacity = 0;
            Tween.To(projLeft, 0.25f, Tween.EaseNone, "Opacity", "1");
            Tween.By(projLeft, 2.5f, Quad.EaseIn, "delay", $"{delay}", "Y", $"{-MEGA_UPWARD_SWORD_PROJECTILE_SPEED}");
            Tween.AddEndHandlerToLastTween(proj, "KillProjectile");

            xOffsetLeft = -CDGMath.RandomInt(50, 1000);

            delay += 0.25f;
        }

        swordData.Dispose();
    }

    public void ChangeProjectileSpeed(ProjectileObj proj, float speed, Vector2 heading) {
        proj.AccelerationX = heading.X * speed;
        proj.AccelerationY = -heading.Y * speed;
    }

    public void CastDamageShield(int numOrbs) {
        foreach (var projectile in _damageShieldProjectiles) {
            projectile.KillProjectile();
        }

        _damageShieldProjectiles.Clear();

        var orbData = new ProjectileData(this) {
            SpriteName = "LastBossOrbProjectile_Sprite",
            Angle = new Vector2(-65, -65),
            Speed = new Vector2(MEGA_SHIELD_SPEED, MEGA_SHIELD_SPEED),
            Target = this,
            IsWeighted = false,
            RotationSpeed = 0,
            CollidesWithTerrain = false,
            DestroysWithTerrain = false,
            DestroysWithEnemy = false,
            CanBeFusRohDahed = false,
            ShowIcon = false,
            Lifespan = 9999,
            Damage = Damage / 2,
        };

        SoundManager.Play3DSound(this, m_target, "FinalBoss_St2_SwordSummon_b");
        for (var i = 0; i < numOrbs; i++) {
            var angle = 360f / numOrbs * i;

            var proj = m_levelScreen.ProjectileManager.FireProjectile(orbData);

            // AltX and AltY are used as holders to hold the projectiles angle and distance from player respectively.
            proj.AltX = angle;
            proj.AltY = MEGA_SHIELD_DISTANCE;
            proj.Spell = SpellType.DAMAGE_SHIELD;
            proj.AccelerationXEnabled = false;
            proj.AccelerationYEnabled = false;
            proj.IgnoreBoundsCheck = true;
            proj.Scale = new Vector2(MEGA_SHIELD_SCALE, MEGA_SHIELD_SCALE);
            proj.Position = CDGMath.GetCirclePosition(angle, MEGA_SHIELD_DISTANCE, Position);
            m_levelScreen.ImpactEffectPool.SpellCastEffect(proj.Position, proj.Rotation, false);

            _damageShieldProjectiles.Add(proj);
        }
    }

    public void Dash(int heading) {
        HeadingY = 0;
        if (m_target.Position.X < X) /* RIGHT of Player */
        {
            if (heading == 0) {
                HeadingX = 1;
            }

            ChangeSprite(Flip == SpriteEffects.None ? "PlayerFrontDash_Character" : "PlayerDash_Character");

            m_levelScreen.ImpactEffectPool.DisplayDashEffect(new Vector2(X, TerrainBounds.Bottom), false);
        } else /* LEFT of Player */ {
            if (heading == 0) {
                HeadingX = -1;
            }

            ChangeSprite(Flip == SpriteEffects.None ? "PlayerDash_Character" : "PlayerFrontDash_Character");

            m_levelScreen.ImpactEffectPool.DisplayDashEffect(new Vector2(X, TerrainBounds.Bottom), true);
        }

        if (heading != 0) {
            HeadingX = heading;
        }

        SoundManager.Play3DSound(this, m_target, "Player_Dash");

        LockFlip = true;
        AccelerationX = 0;
        AccelerationY = 0;
        PlayAnimation(false);
        CurrentSpeed = 900;
        AccelerationYEnabled = false;
        _isDashing = true;
    }

    public void DashComplete() {
        LockFlip = false;
        CurrentSpeed = 500;
        AccelerationYEnabled = true;
        _isDashing = false;
        AnimationDelay = 1 / 10f;
    }

    public override void Update(GameTime gameTime) {
        if (_smokeCounter > 0 && IsSecondForm == false) {
            _smokeCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_smokeCounter <= 0) {
                _smokeCounter = 0.25f;
                if (CurrentSpeed > 0) {
                    _smokeCounter = 0.05f;
                }

                m_levelScreen.ImpactEffectPool.BlackSmokeEffect(this);
            }
        }

        if (!IsSecondForm) {
            if (!m_isTouchingGround && m_currentActiveLB != null && SpriteName != "PlayerAttacking3_Character" && !_isDashing && SpriteName != "PlayerLevelUp_Character") {
                switch (AccelerationY) {
                    case < 0 when SpriteName != "PlayerJumping_Character":
                        ChangeSprite("PlayerJumping_Character");
                        PlayAnimation();
                        break;

                    case > 0 when SpriteName != "PlayerFalling_Character":
                        ChangeSprite("PlayerFalling_Character");
                        PlayAnimation();
                        break;
                }
            } else if (m_isTouchingGround && m_currentActiveLB != null && SpriteName == "PlayerAttacking3_Character" && CurrentSpeed != 0) {
                var bossLegs = GetChildAt(PlayerPart.LEGS) as SpriteObj;
                if (bossLegs!.SpriteName != "PlayerWalkingLegs_Sprite") {
                    bossLegs.ChangeSprite("PlayerWalkingLegs_Sprite");
                    bossLegs.PlayAnimation(CurrentFrame, TotalFrames);
                    bossLegs.Y += 4;
                    bossLegs.OverrideParentAnimationDelay = true;
                    bossLegs.AnimationDelay = 1 / 10f;
                }
            }
        } else {
            if (SpriteName == "EnemyLastBossRun_Character") {
                _walkUpSoundFinalBoss.Update();
                _walkDownSoundFinalBoss.Update();
            }
        }

        if (!IsSecondForm && CurrentHealth <= 0 && m_target.CurrentHealth > 0 && !IsNeo) {
            // This is the first form death animation.
            if (IsTouchingGround && _firstFormDying == false) {
                _firstFormDying = true;
                m_levelScreen.ItemDropManager.DropItemWide(Position, ItemDropType.HEALTH, GameEV.ITEM_HEALTHDROP_AMOUNT);
                m_levelScreen.ItemDropManager.DropItemWide(Position, ItemDropType.HEALTH, GameEV.ITEM_HEALTHDROP_AMOUNT);
                m_levelScreen.ItemDropManager.DropItemWide(Position, ItemDropType.HEALTH, GameEV.ITEM_HEALTHDROP_AMOUNT);
                m_levelScreen.ItemDropManager.DropItemWide(Position, ItemDropType.HEALTH, GameEV.ITEM_HEALTHDROP_AMOUNT);
                m_levelScreen.ItemDropManager.DropItemWide(Position, ItemDropType.MANA, GameEV.ITEM_MANADROP_AMOUNT);
                m_levelScreen.ItemDropManager.DropItemWide(Position, ItemDropType.MANA, GameEV.ITEM_MANADROP_AMOUNT);
                m_levelScreen.ItemDropManager.DropItemWide(Position, ItemDropType.MANA, GameEV.ITEM_MANADROP_AMOUNT);
                IsWeighted = false;
                IsCollidable = false;
                AnimationDelay = 1 / 10f;
                CurrentSpeed = 0;
                AccelerationX = 0;
                AccelerationY = 0;
                TextureColor = Color.White;
                ChangeSprite("PlayerDeath_Character");
                SoundManager.PlaySound("Boss_Flash");
                SoundManager.StopMusic(1);
                m_target.StopAllSpells();
                m_target.ForceInvincible = true;

                Flip = m_target.X < X ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

                if (m_currentActiveLB is { IsActive: true }) {
                    m_currentActiveLB.StopLogicBlock();
                }
            }

            // This is the logic to move the player next to the boss.
            if (m_target.IsTouchingGround && IsSecondForm == false && SpriteName == "PlayerDeath_Character") {
                MovePlayerTo();
            }
        }

        if ((!_firstFormDying && !IsSecondForm) || (_firstFormDying && IsSecondForm) || (IsNeo && CurrentHealth > 0)) {
            base.Update(gameTime);
        }

        // Code for the neo version
        if (!IsSecondForm && CurrentHealth <= 0 && m_target.CurrentHealth > 0 && IsNeo && IsTouchingGround && !_firstFormDying) {
            KillPlayerNeo();
            _firstFormDying = true;
        }
    }

    public void MovePlayerTo() {
        m_target.StopAllSpells();
        m_levelScreen.ProjectileManager.DestroyAllProjectiles(true);
        IsSecondForm = true;
        m_isKilled = true;
        m_levelScreen.RunCinematicBorders(16);
        m_currentActiveLB.StopLogicBlock();

        const int xOffset = 250;
        Vector2 targetPos;

        if ((m_target.X < X && X > m_levelScreen.CurrentRoom.X + 500) || X > m_levelScreen.CurrentRoom.Bounds.Right - 500)
        {
            targetPos = new Vector2(X - xOffset, Y); // Move to the left of the boss.
            if (targetPos.X > m_levelScreen.CurrentRoom.Bounds.Right - 500) {
                targetPos.X = m_levelScreen.CurrentRoom.Bounds.Right - 500;
            }
        } else {
            targetPos = new Vector2(X + xOffset, Y); // Move to the right of the boss.
        }

        m_target.Flip = SpriteEffects.None;
        if (targetPos.X < m_target.X) {
            m_target.Flip = SpriteEffects.FlipHorizontally;
        }

        var duration = CDGMath.DistanceBetweenPts(m_target.Position, targetPos) / m_target.Speed;

        m_target.UpdateCollisionBoxes();
        m_target.State = 1;
        m_target.IsWeighted = false;
        m_target.AccelerationY = 0;
        m_target.AccelerationX = 0;
        m_target.IsCollidable = false;
        m_target.Y = m_levelScreen.CurrentRoom.Bounds.Bottom - (60 * 3) - (m_target.Bounds.Bottom - m_target.Y);
        m_target.CurrentSpeed = 0;
        m_target.LockControls();
        m_target.ChangeSprite("PlayerWalking_Character");
        var playerMoveLS = new LogicSet(m_target);
        playerMoveLS.AddAction(new DelayLogicAction(duration));
        m_target.RunExternalLogicSet(playerMoveLS);
        m_target.PlayAnimation();
        Tween.To(m_target, duration, Tween.EaseNone, "X", $"{targetPos.X}");
        Tween.AddEndHandlerToLastTween(this, "SecondFormDeath");
    }

    // This is where the death animation code needs to go.
    public void SecondFormDeath() {
        m_target.Flip = m_target.X < X ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

        PlayAnimation(false);
        SoundManager.PlaySound("FinalBoss_St1_DeathGrunt");
        Tween.RunFunction(0.1f, typeof(SoundManager), "PlaySound", "Player_Death_SwordTwirl");
        Tween.RunFunction(0.7f, typeof(SoundManager), "PlaySound", "Player_Death_SwordLand");
        Tween.RunFunction(1.2f, typeof(SoundManager), "PlaySound", "Player_Death_BodyFall");

        float delay = 2;
        Tween.RunFunction(2, this, "PlayBlackSmokeSounds");
        for (var i = 0; i < 30; i++) {
            Tween.RunFunction(delay, m_levelScreen.ImpactEffectPool, "BlackSmokeEffect", Position, new Vector2(1 + (delay * 1), 1 + (delay * 1)));
            delay += 0.05f;
        }

        Tween.RunFunction(3, this, "HideEnemy");
        Tween.RunFunction(6, this, "SecondFormDialogue");
    }

    public void PlayBlackSmokeSounds() {
        SoundManager.PlaySound("Cutsc_Smoke");
    }

    public void HideEnemy() {
        Visible = false;
    }

    public void SecondFormDialogue() {
        var manager = m_levelScreen.ScreenManager as RCScreenManager;
        manager!.DialogueScreen.SetDialogue("FinalBossTalk02");
        manager.DialogueScreen.SetConfirmEndHandler(m_levelScreen.CurrentRoom, "RunFountainCutscene");
        manager.DisplayScreen(ScreenType.DIALOGUE, true);
    }

    public void SecondFormComplete() {
        m_target.ForceInvincible = false;
        Level += LevelEV.LAST_BOSS_MODE2_LEVEL_MOD;
        Flip = SpriteEffects.FlipHorizontally;
        Visible = true;
        MaxHealth = EnemyEV.LAST_BOSS_ADVANCED_MAX_HEALTH;
        Damage = EnemyEV.LAST_BOSS_ADVANCED_DAMAGE;
        CurrentHealth = MaxHealth;
        Name = EnemyEV.LAST_BOSS_ADVANCED_NAME;
        if (LevelEV.WeakenBosses) {
            CurrentHealth = 1;
        }

        MinMoneyDropAmount = EnemyEV.LAST_BOSS_ADVANCED_MIN_DROP_AMOUNT;
        MaxMoneyDropAmount = EnemyEV.LAST_BOSS_ADVANCED_MAX_DROP_AMOUNT;
        MoneyDropChance = EnemyEV.LAST_BOSS_ADVANCED_DROP_CHANCE;
        Speed = EnemyEV.LAST_BOSS_ADVANCED_SPEED;
        TurnSpeed = EnemyEV.LAST_BOSS_ADVANCED_TURN_SPEED;
        ProjectileSpeed = EnemyEV.LAST_BOSS_ADVANCED_PROJECTILE_SPEED;
        JumpHeight = EnemyEV.LAST_BOSS_ADVANCED_JUMP;
        CooldownTime = EnemyEV.LAST_BOSS_ADVANCED_COOLDOWN;
        AnimationDelay = 1 / EnemyEV.LAST_BOSS_ADVANCED_ANIMATION_DELAY;
        AlwaysFaceTarget = EnemyEV.LAST_BOSS_ADVANCED_ALWAYS_FACE_TARGET;
        CanFallOffLedges = EnemyEV.LAST_BOSS_ADVANCED_CAN_FALL_OFF_LEDGES;
        CanBeKnockedBack = EnemyEV.LAST_BOSS_ADVANCED_CAN_BE_KNOCKED_BACK;
        ProjectileScale = EnemyEV.LastBossAdvancedProjectileScale;
        TintablePart.TextureColor = EnemyEV.LastBossAdvancedTint;
        MeleeRadius = EnemyEV.LAST_BOSS_ADVANCED_MELEE_RADIUS;
        EngageRadius = EnemyEV.LAST_BOSS_ADVANCED_ENGAGE_RADIUS;
        ProjectileRadius = EnemyEV.LAST_BOSS_ADVANCED_PROJECTILE_RADIUS;
        ProjectileDamage = Damage;
        KnockBack = EnemyEV.LastBossAdvancedKnockBack;
        ChangeSprite("EnemyLastBossIdle_Character");
        
        //TEDDY MEGA BOSS COOLDOWN
        SetCooldownLogicBlock(_secondFormCooldownLB, 40, 20, 40); //walkTowardsSF, walkAwaySF, walkStopSF
        PlayAnimation();
        Name = "The Fountain";

        IsWeighted = true;
        IsCollidable = true;
    }

    public void SecondFormActive() {
        if (IsPaused) {
            UnpauseEnemy(true);
        }

        m_levelScreen.CameraLockedToPlayer = true;
        m_target.UnlockControls();
        m_target.IsWeighted = true;
        m_target.IsCollidable = true;
        m_isKilled = false;
    }

    public override void HitEnemy(int damage, Vector2 collisionPt, bool isPlayer) {
        if (!IsSecondForm) {
            if (_isHurt || _isDashing) {
                return;
            }

            SoundManager.Play3DSound(this, m_target, "FinalBoss_St1_Dmg_01", "FinalBoss_St1_Dmg_02", "FinalBoss_St1_Dmg_03", "FinalBoss_St1_Dmg_04");
        } else {
            SoundManager.Play3DSound(this, m_target, "FinalBoss_St2_Hit_01", "FinalBoss_St2_Hit_03", "FinalBoss_St2_Hit_04");
            SoundManager.Play3DSound(this, m_target, "FinalBoss_St2_DmgVox_01", "FinalBoss_St2_DmgVox_02", "FinalBoss_St2_DmgVox_03", "FinalBoss_St2_DmgVox_04",
                "FinalBoss_St2_DmgVox_05", "FinalBoss_St2_DmgVox_06", "FinalBoss_St2_DmgVox_07", "FinalBoss_St2_DmgVox_08", "FinalBoss_St2_DmgVox_09");
        }

        base.HitEnemy(damage, collisionPt, isPlayer);
    }

    public override void Kill(bool giveXP = true) {
        if (m_target.CurrentHealth <= 0) {
            return;
        }

        if (IsSecondForm && m_bossVersionKilled == false) {
            // TODO: This is where the victory conditions should go.
            m_bossVersionKilled = true;
            SetPlayerData();

            m_levelScreen.PauseScreen();
            m_levelScreen.ProjectileManager.DestroyAllProjectiles(false);
            m_target.StopAllSpells();
            m_levelScreen.RunWhiteSlashEffect();
            ChangeSprite("EnemyLastBossDeath_Character");
            Flip = m_target.X < X ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Tween.RunFunction(1f, this, "Part2");
            SoundManager.PlaySound("Boss_Flash");
            SoundManager.PlaySound("Boss_Eyeball_Freeze");
            SoundManager.StopMusic();
            m_target.LockControls();

            GameUtil.UnlockAchievement("FEAR_OF_FATHERS");

            if (Game.PlayerStats.TimesCastleBeaten > 1) {
                GameUtil.UnlockAchievement("FEAR_OF_TWINS");
            }
        }

        if (IsNeo && _neoDying == false) {
            _neoDying = true;
            m_levelScreen.PauseScreen();
            SoundManager.PauseMusic();

            m_levelScreen.RunWhiteSlashEffect();

            SoundManager.PlaySound("Boss_Flash");
            SoundManager.PlaySound("Boss_Eyeball_Freeze");
            Tween.RunFunction(1, m_levelScreen, "UnpauseScreen");
            Tween.RunFunction(1, typeof(SoundManager), "ResumeMusic");
        }
    }

    public void KillPlayerNeo() {
        m_isKilled = true;

        if (m_currentActiveLB is { IsActive: true }) {
            m_currentActiveLB.StopLogicBlock();
        }

        IsWeighted = false;
        IsCollidable = false;
        AnimationDelay = 1 / 10f;
        CurrentSpeed = 0;
        AccelerationX = 0;
        AccelerationY = 0;

        ChangeSprite("PlayerDeath_Character");
        PlayAnimation(false);
        SoundManager.PlaySound("FinalBoss_St1_DeathGrunt");
        Tween.RunFunction(0.1f, typeof(SoundManager), "PlaySound", "Player_Death_SwordTwirl");
        Tween.RunFunction(0.7f, typeof(SoundManager), "PlaySound", "Player_Death_SwordLand");
        Tween.RunFunction(1.2f, typeof(SoundManager), "PlaySound", "Player_Death_BodyFall");
    }

    // This sets all the player data once the last boss has been beaten.
    public static void SetPlayerData() {
        // Creating a new family tree node and saving.
        var newNode = new FamilyTreeNode {
            Name = Game.PlayerStats.PlayerName,
            Age = Game.PlayerStats.Age,
            ChildAge = Game.PlayerStats.ChildAge,
            Class = Game.PlayerStats.Class,
            HeadPiece = Game.PlayerStats.HeadPiece,
            ChestPiece = Game.PlayerStats.ChestPiece,
            ShoulderPiece = Game.PlayerStats.ShoulderPiece,
            NumEnemiesBeaten = Game.PlayerStats.NumEnemiesBeaten,
            BeatenABoss = true,
            Traits = Game.PlayerStats.Traits,
            IsFemale = Game.PlayerStats.IsFemale,
            RomanNumeral = Game.PlayerStats.RomanNumeral,
        };
        Game.PlayerStats.FamilyTreeArray.Add(newNode);

        // Setting necessary after-death flags and saving.
        Game.PlayerStats.NewBossBeaten = false;
        Game.PlayerStats.RerolledChildren = false;
        Game.PlayerStats.NumEnemiesBeaten = 0;
        Game.PlayerStats.LichHealth = 0;
        Game.PlayerStats.LichMana = 0;
        Game.PlayerStats.LichHealthMod = 1;
        Game.PlayerStats.LoadStartingRoom = true;

        // The important one. These two flags will trigger new game plus at title screen.
        Game.PlayerStats.LastbossBeaten = true;
        Game.PlayerStats.CharacterFound = false; // This should be the ONLY place this is ever set to false.
        Game.PlayerStats.TimesCastleBeaten++;

        // Thanatophobia trait.
        if (Game.PlayerStats.ArchitectUsed == false && Game.PlayerStats.TimesDead <= 15) {
            GameUtil.UnlockAchievement("FEAR_OF_DYING");
        }

        Program.Game.SaveManager.SaveFiles(SaveType.PlayerData, SaveType.Archipelago);
    }

    public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType) {
        var mtd = CollisionMath.CalculateMTD(thisBox.AbsRect, otherBox.AbsRect);

        // Hits the player in Tanooki mode.
        if (otherBox.AbsParent is PlayerObj player && otherBox.Type == Consts.TERRAIN_HITBOX && player.IsInvincible == false && player.State != PlayerObj.STATE_HURT) {
            player.HitPlayer(this);
        }

        if (m_isTouchingGround && _isHurt) {
            _isHurt = false;
            if (IsSecondForm == false) {
                ChangeSprite("PlayerIdle_Character");
            }
        }

        // This has to go before the wall check.
        if (otherBox.AbsParent is EnemyObj_Platform == false) {
            base.CollisionResponse(thisBox, otherBox, collisionResponseType);
        }

        // This code gets him out of corners.
        if (otherBox.AbsParent is not TerrainObj terrain || m_isTouchingGround || terrain is DoorObj || IsSecondForm) {
            return;
        }

        if (m_currentActiveLB is { IsActive: true }) {
            m_currentActiveLB.StopLogicBlock();
        }

        switch (mtd.X)
        {
            // Dash right
            case > 0:
                RunLogicBlock(true, _firstFormDashAwayLB, 0, 100); // dashLeftLS, dashRightLS
                break;

            //Dash left
            case < 0:
                RunLogicBlock(true, _firstFormDashAwayLB, 100, 0); // dashLeftLS, dashRightLS
                break;
        }
    }

    public void Part2() {
        SoundManager.PlaySound("FinalBoss_St2_WeatherChange_a");
        m_levelScreen.UnpauseScreen();
        m_currentActiveLB?.StopLogicBlock();

        PauseEnemy(true);
        m_target.CurrentSpeed = 0;
        m_target.ForceInvincible = true;

        Tween.RunFunction(1, m_levelScreen, "RevealMorning");
        Tween.RunFunction(1, m_levelScreen.CurrentRoom, "ChangeWindowOpacity");
        Tween.RunFunction(5, this, "Part3");
    }

    public void Part3() {
        var manager = m_levelScreen.ScreenManager as RCScreenManager;
        manager!.DialogueScreen.SetDialogue("FinalBossTalk03");
        manager.DialogueScreen.SetConfirmEndHandler(this, "Part4");
        manager.DisplayScreen(ScreenType.DIALOGUE, true);
    }

    public void Part4() {
        var dataList = new List<object> { this };
        (m_levelScreen.ScreenManager as RCScreenManager)!.DisplayScreen(ScreenType.GAME_OVER_BOSS, true, dataList);
    }

    public override void ChangeSprite(string spriteName) {
        base.ChangeSprite(spriteName);
        if (IsSecondForm) {
            return;
        }

        var headPart = (_objectList[PlayerPart.HEAD] as IAnimateableObj)!.SpriteName;
        var numberIndex = headPart.IndexOf("_", StringComparison.Ordinal) - 1;
        headPart = headPart.Remove(numberIndex, 1);
        headPart = headPart.Replace("_", PlayerPart.INTRO_HELM + "_");
        _objectList[PlayerPart.HEAD].ChangeSprite(headPart);

        _objectList[PlayerPart.BOOBS].Visible = false;
        _objectList[PlayerPart.EXTRA].Visible = false;
        _objectList[PlayerPart.LIGHT].Visible = false;
        _objectList[PlayerPart.GLASSES].Visible = false;
        _objectList[PlayerPart.BOWTIE].Visible = false;
        _objectList[PlayerPart.WINGS].Visible = false;
    }

    public override void Draw(Camera2D camera) {
        // This here is just awful. But I can't put it in the update because when he's killed he stops updating.
        if (IsKilled && TextureColor != Color.White) {
            m_blinkTimer = 0;
            TextureColor = Color.White;
        }

        base.Draw(camera);
    }

    public override void Reset() {
        _neoDying = false;
        IsSecondForm = false;
        _firstFormDying = false;
        CanBeKnockedBack = true;
        base.Reset();
    }

    public override void Dispose() {
        if (IsDisposed) {
            return;
        }

        // Done
        _damageShieldProjectiles.Clear();
        _damageShieldProjectiles = null;
        _delayObj.Dispose();
        _delayObj = null;

        if (_daggerProjData != null) {
            _daggerProjData.Dispose();
            _daggerProjData = null;
        }

        if (_axeProjData != null) {
            _axeProjData.Dispose();
            _axeProjData = null;
        }

        base.Dispose();
    }

    public void ForceSecondForm(bool value) {
        IsSecondForm = value;
    }

    #region Basic and Advanced Logic

    protected override void RunBasicLogic() {
        if (CurrentHealth <= 0) {
            return;
        }

        if (IsSecondForm) {
            RunAdvancedLogic();
            return;
        }

        if (_isHurt) {
            return;
        }

        switch (State) {
            case STATE_MELEE_ENGAGE:
                if (IsNeo == false) {
                    RunLogicBlock(true, _generalBasicLB, 0, 0, 0, 35, 35, 00, 0, 30); //walkTowardsLS, walkAwayLS, walkStopLS, jumpLS, moveAttackLS, throwShieldLS, throwDaggerLS, dashLS
                } else {
                    RunLogicBlock(true, _generalBasicNeoLB, 0, 0, 0, 50, 20, 00, 0, 30); //walkTowardsLS, walkAwayLS, walkStopLS, jumpLS, moveAttackLS, throwShieldLS, throwDaggerLS, dashLS
                }

                break;

            case STATE_PROJECTILE_ENGAGE:
                if (IsNeo == false) {
                    RunLogicBlock(true, _generalBasicLB, 35, 0, 0, 25, 0, 00, 20, 20); //walkTowardsLS, walkAwayLS, walkStopLS, jumpLS, moveAttackLS, throwShieldLS, throwDaggerLS, dashLS
                } else {
                    RunLogicBlock(true, _generalBasicNeoLB, 25, 0, 20, 15, 0, 00, 15, 25); //walkTowardsLS, walkAwayLS, walkStopLS, jumpLS, moveAttackLS, throwShieldLS, throwDaggerLS, dashLS
                }

                break;

            case STATE_ENGAGE:
                if (IsNeo == false) {
                    RunLogicBlock(true, _generalBasicLB, 40, 0, 0, 20, 0, 00, 40, 0); //walkTowardsLS, walkAwayLS, walkStopLS, jumpLS, moveAttackLS, throwShieldLS, throwDaggerLS, dashLS
                } else {
                    RunLogicBlock(true, _generalBasicNeoLB, 40, 0, 20, 20, 0, 00, 20, 0); //walkTowardsLS, walkAwayLS, walkStopLS, jumpLS, moveAttackLS, throwShieldLS, throwDaggerLS, dashLS
                }

                break;

            case STATE_WANDER:
                if (IsNeo == false) {
                    RunLogicBlock(true, _generalBasicLB, 50, 0, 0, 0, 0, 00, 50, 0); //walkTowardsLS, walkAwayLS, walkStopLS, jumpLS, moveAttackLS, throwShieldLS, throwDaggerLS, dashLS
                } else {
                    RunLogicBlock(true, _generalBasicNeoLB, 50, 0, 10, 10, 0, 00, 30, 0); //walkTowardsLS, walkAwayLS, walkStopLS, jumpLS, moveAttackLS, throwShieldLS, throwDaggerLS, dashLS
                }

                break;
        }
    }

    protected override void RunAdvancedLogic() {
        switch (State) {
            case STATE_MELEE_ENGAGE:
                RunLogicBlock(true, _generalAdvancedLB, 31, 15, 0, 26, 3, 13, 6, 6); //walkTowardsSF, walkAwaySF, walkStopSF, attackSF, castSpearsSF, castRandomSwordsSF, castSwordsLeftSF, castSwordRightSF
                break;

            case STATE_PROJECTILE_ENGAGE:
                RunLogicBlock(true, _generalAdvancedLB, 52, 12, 0, 0, 11, 15, 5, 5); //walkTowardsSF, walkAwaySF, walkStopSF, attackSF, castSpearsSF, castRandomSwordsSF, castSwordsLeftSF, castSwordRightSF
                break;

            case STATE_ENGAGE:
                RunLogicBlock(true, _generalAdvancedLB, 68, 0, 0, 0, 10, 12, 5, 5); //walkTowardsSF, walkAwaySF, walkStopSF, attackSF, castSpearsSF, castRandomSwordsSF, castSwordsLeftSF, castSwordRightSF
                break;

            case STATE_WANDER:
                RunLogicBlock(true, _generalAdvancedLB, 63, 0, 0, 0, 15, 12, 5, 5); //walkTowardsSF, walkAwaySF, walkStopSF, attackSF, castSpearsSF, castRandomSwordsSF, castSwordsLeftSF, castSwordRightSF
                break;
        }
    }

    #endregion

    #region Expert and Miniboss (Not Used)

    protected override void RunExpertLogic() {
        if (CurrentHealth <= 0) {
            return;
        }

        if (IsSecondForm) {
            RunAdvancedLogic();
            return;
        }

        if (_isHurt) {
            return;
        }

        switch (State) {
            case STATE_MELEE_ENGAGE:
                if (m_isTouchingGround) {
                    RunLogicBlock(true, _generalBasicLB, 0, 10, 0, 20, 35, 10, 0, 25); //walkTowardsLS, walkAwayLS, walkStopLS, jumpLS, moveAttackLS, throwAxeLS, throwDaggerLS, dashLS
                } else {
                    RunLogicBlock(true, _generalBasicLB, 0, 10, 0, 0, 55, 10, 0, 25); //walkTowardsLS, walkAwayLS, walkStopLS, jumpLS, moveAttackLS, throwAxeLS, throwDaggerLS, dashLS
                }

                break;

            case STATE_PROJECTILE_ENGAGE:
                if (m_target.IsJumping == false) {
                    RunLogicBlock(true, _generalBasicLB, 20, 0, 10, 10, 0, 15, 20, 10); //walkTowardsLS, walkAwayLS, walkStopLS, jumpLS, moveAttackLS, throwAxeLS, throwDaggerLS, dashLS
                } else {
                    RunLogicBlock(true, _generalBasicLB, 40, 0, 15, 0, 0, 15, 20, 10); //walkTowardsLS, walkAwayLS, walkStopLS, jumpLS, moveAttackLS, throwAxeLS, throwDaggerLS, dashLS
                }

                break;

            case STATE_ENGAGE:
                if (m_target.IsJumping == false) {
                    RunLogicBlock(true, _generalBasicLB, 30, 0, 15, 20, 0, 25, 0, 10); //walkTowardsLS, walkAwayLS, walkStopLS, jumpLS, moveAttackLS, throwAxeLS, throwDaggerLS, dashLS
                } else {
                    RunLogicBlock(true, _generalBasicLB, 50, 0, 15, 0, 0, 25, 0, 10); //walkTowardsLS, walkAwayLS, walkStopLS, jumpLS, moveAttackLS, throwAxeLS, throwDaggerLS, dashLS
                }

                break;

            case STATE_WANDER:
                if (m_target.IsJumping == false) {
                    RunLogicBlock(true, _generalBasicLB, 50, 0, 10, 20, 0, 0, 20, 0); //walkTowardsLS, walkAwayLS, walkStopLS, jumpLS, moveAttackLS, throwAxeLS, throwDaggerLS, dashLS
                } else {
                    RunLogicBlock(true, _generalBasicLB, 50, 0, 10, 20, 0, 0, 20, 0); //walkTowardsLS, walkAwayLS, walkStopLS, jumpLS, moveAttackLS, throwAxeLS, throwDaggerLS, dashLS
                }

                break;
        }
    }

    protected override void RunMinibossLogic() {
        RunLogicBlock(true, _generalAdvancedLB, 0, 0, 0, 0, 100, 0, 0, 0); //walkTowardsSF, walkAwaySF, walkStopSF, attackSF, castSpearsSF, castRandomSwordsSF, castSwordsLeftSF, castSwordRightSF
    }

    #endregion
}
