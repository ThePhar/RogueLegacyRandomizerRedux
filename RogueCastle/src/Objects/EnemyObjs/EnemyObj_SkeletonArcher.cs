using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.Enumerations;
using RogueCastle.EnvironmentVariables;

namespace RogueCastle
{
    public class EnemyObj_SkeletonArcher : EnemyObj
    {
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();

        private float m_fireDelay = 0.5f;

        protected override void InitializeEV()
        {
            LockFlip = false; // true;
            this.IsWeighted = true;
            IsCollidable = true;

            #region Basic Variables - General
            Name = EnemyEV.SKELETON_ARCHER_BASIC_NAME;
            LocStringID = EnemyEV.SKELETON_ARCHER_BASIC_NAME_LOC_ID;

            MaxHealth = EnemyEV.SKELETON_ARCHER_BASIC_MAX_HEALTH;
            Damage = EnemyEV.SKELETON_ARCHER_BASIC_DAMAGE;
            XPValue = EnemyEV.SKELETON_ARCHER_BASIC_XP_VALUE;

            MinMoneyDropAmount = EnemyEV.SKELETON_ARCHER_BASIC_MIN_DROP_AMOUNT;
            MaxMoneyDropAmount = EnemyEV.SKELETON_ARCHER_BASIC_MAX_DROP_AMOUNT;
            MoneyDropChance = EnemyEV.SKELETON_ARCHER_BASIC_DROP_CHANCE;

            Speed = EnemyEV.SKELETON_ARCHER_BASIC_SPEED;
            TurnSpeed = EnemyEV.SKELETON_ARCHER_BASIC_TURN_SPEED;
            ProjectileSpeed = EnemyEV.SKELETON_ARCHER_BASIC_PROJECTILE_SPEED;
            JumpHeight = EnemyEV.SKELETON_ARCHER_BASIC_JUMP;
            CooldownTime = EnemyEV.SKELETON_ARCHER_BASIC_COOLDOWN;
            AnimationDelay = 1 / EnemyEV.SKELETON_ARCHER_BASIC_ANIMATION_DELAY;

            AlwaysFaceTarget = EnemyEV.SKELETON_ARCHER_BASIC_ALWAYS_FACE_TARGET;
            CanFallOffLedges = EnemyEV.SKELETON_ARCHER_BASIC_CAN_FALL_OFF_LEDGES;
            CanBeKnockedBack = EnemyEV.SKELETON_ARCHER_BASIC_CAN_BE_KNOCKED_BACK;
            IsWeighted = EnemyEV.SKELETON_ARCHER_BASIC_IS_WEIGHTED;

            Scale = EnemyEV.SkeletonArcherBasicScale;
            ProjectileScale = EnemyEV.SkeletonArcherBasicProjectileScale;
            TintablePart.TextureColor = EnemyEV.SkeletonArcherBasicTint;

            MeleeRadius = EnemyEV.SKELETON_ARCHER_BASIC_MELEE_RADIUS;
            ProjectileRadius = EnemyEV.SKELETON_ARCHER_BASIC_PROJECTILE_RADIUS;
            EngageRadius = EnemyEV.SKELETON_ARCHER_BASIC_ENGAGE_RADIUS;

            ProjectileDamage = Damage;
            KnockBack = EnemyEV.SkeletonArcherBasicKnockBack;
            #endregion

            switch (Difficulty)
            {
                case (GameTypes.EnemyDifficulty.MINIBOSS):
                    #region Miniboss Variables - General
                    Name = EnemyEV.SKELETON_ARCHER_MINIBOSS_NAME;
                    LocStringID = EnemyEV.SKELETON_ARCHER_MINIBOSS_NAME_LOC_ID;

                    MaxHealth = EnemyEV.SKELETON_ARCHER_MINIBOSS_MAX_HEALTH;
                    Damage = EnemyEV.SKELETON_ARCHER_MINIBOSS_DAMAGE;
                    XPValue = EnemyEV.SKELETON_ARCHER_MINIBOSS_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.SKELETON_ARCHER_MINIBOSS_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.SKELETON_ARCHER_MINIBOSS_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.SKELETON_ARCHER_MINIBOSS_DROP_CHANCE;

                    Speed = EnemyEV.SKELETON_ARCHER_MINIBOSS_SPEED;
                    TurnSpeed = EnemyEV.SKELETON_ARCHER_MINIBOSS_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.SKELETON_ARCHER_MINIBOSS_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.SKELETON_ARCHER_MINIBOSS_JUMP;
                    CooldownTime = EnemyEV.SKELETON_ARCHER_MINIBOSS_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.SKELETON_ARCHER_MINIBOSS_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.SKELETON_ARCHER_MINIBOSS_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.SKELETON_ARCHER_MINIBOSS_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.SKELETON_ARCHER_MINIBOSS_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.SKELETON_ARCHER_MINIBOSS_IS_WEIGHTED;

                    Scale = EnemyEV.SkeletonArcherMinibossScale;
                    ProjectileScale = EnemyEV.SkeletonArcherMinibossProjectileScale;
                    TintablePart.TextureColor = EnemyEV.SkeletonArcherMinibossTint;

                    MeleeRadius = EnemyEV.SKELETON_ARCHER_MINIBOSS_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.SKELETON_ARCHER_MINIBOSS_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.SKELETON_ARCHER_MINIBOSS_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.SkeletonArcherMinibossKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.EXPERT):
                    #region Expert Variables - General
                    Name = EnemyEV.SKELETON_ARCHER_EXPERT_NAME;
                    LocStringID = EnemyEV.SKELETON_ARCHER_EXPERT_NAME_LOC_ID;

                    MaxHealth = EnemyEV.SKELETON_ARCHER_EXPERT_MAX_HEALTH;
                    Damage = EnemyEV.SKELETON_ARCHER_EXPERT_DAMAGE;
                    XPValue = EnemyEV.SKELETON_ARCHER_EXPERT_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.SKELETON_ARCHER_EXPERT_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.SKELETON_ARCHER_EXPERT_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.SKELETON_ARCHER_EXPERT_DROP_CHANCE;

                    Speed = EnemyEV.SKELETON_ARCHER_EXPERT_SPEED;
                    TurnSpeed = EnemyEV.SKELETON_ARCHER_EXPERT_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.SKELETON_ARCHER_EXPERT_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.SKELETON_ARCHER_EXPERT_JUMP;
                    CooldownTime = EnemyEV.SKELETON_ARCHER_EXPERT_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.SKELETON_ARCHER_EXPERT_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.SKELETON_ARCHER_EXPERT_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.SKELETON_ARCHER_EXPERT_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.SKELETON_ARCHER_EXPERT_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.SKELETON_ARCHER_EXPERT_IS_WEIGHTED;

                    Scale = EnemyEV.SkeletonArcherExpertScale;
                    ProjectileScale = EnemyEV.SkeletonArcherExpertProjectileScale;
                    TintablePart.TextureColor = EnemyEV.SkeletonArcherExpertTint;

                    MeleeRadius = EnemyEV.SKELETON_ARCHER_EXPERT_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.SKELETON_ARCHER_EXPERT_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.SKELETON_ARCHER_EXPERT_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.SkeletonArcherExpertKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.ADVANCED):
                    #region Advanced Variables - General
                    Name = EnemyEV.SKELETON_ARCHER_ADVANCED_NAME;
                    LocStringID = EnemyEV.SKELETON_ARCHER_ADVANCED_NAME_LOC_ID;

                    MaxHealth = EnemyEV.SKELETON_ARCHER_ADVANCED_MAX_HEALTH;
                    Damage = EnemyEV.SKELETON_ARCHER_ADVANCED_DAMAGE;
                    XPValue = EnemyEV.SKELETON_ARCHER_ADVANCED_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.SKELETON_ARCHER_ADVANCED_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.SKELETON_ARCHER_ADVANCED_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.SKELETON_ARCHER_ADVANCED_DROP_CHANCE;

                    Speed = EnemyEV.SKELETON_ARCHER_ADVANCED_SPEED;
                    TurnSpeed = EnemyEV.SKELETON_ARCHER_ADVANCED_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.SKELETON_ARCHER_ADVANCED_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.SKELETON_ARCHER_ADVANCED_JUMP;
                    CooldownTime = EnemyEV.SKELETON_ARCHER_ADVANCED_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.SKELETON_ARCHER_ADVANCED_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.SKELETON_ARCHER_ADVANCED_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.SKELETON_ARCHER_ADVANCED_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.SKELETON_ARCHER_ADVANCED_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.SKELETON_ARCHER_ADVANCED_IS_WEIGHTED;

                    Scale = EnemyEV.SkeletonArcherAdvancedScale;
                    ProjectileScale = EnemyEV.SkeletonArcherAdvancedProjectileScale;
                    TintablePart.TextureColor = EnemyEV.SkeletonArcherAdvancedTint;

                    MeleeRadius = EnemyEV.SKELETON_ARCHER_ADVANCED_MELEE_RADIUS;
                    EngageRadius = EnemyEV.SKELETON_ARCHER_ADVANCED_ENGAGE_RADIUS;
                    ProjectileRadius = EnemyEV.SKELETON_ARCHER_ADVANCED_PROJECTILE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.SkeletonArcherAdvancedKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.BASIC):
                default:
                    break;
            }		

        }

        protected override void InitializeLogic()
        {

            LogicSet walkStopLS = new LogicSet(this);
            walkStopLS.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherIdle_Character", true, true));
            walkStopLS.AddAction(new MoveLogicAction(m_target, true, 0));
            walkStopLS.AddAction(new DelayLogicAction(0.2f, 0.75f));

            ProjectileData projData = new ProjectileData(this)
            {
                SpriteName = "ArrowProjectile_Sprite",
                SourceAnchor = new Vector2(10,-20),
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = true,
                RotationSpeed = 0,
                Damage = Damage,
                AngleOffset = 0,
                CollidesWithTerrain = true,
                Scale = ProjectileScale,
                FollowArc = true,
            };

            #region FireStraightProjectile

            LogicSet fireStraightProjectile = new LogicSet(this);
            fireStraightProjectile.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false));
            projData.Angle = new Vector2(-20, -20);
            fireStraightProjectile.AddAction(new RunFunctionLogicAction(this, "AngleArcher", projData.Angle.X));
            fireStraightProjectile.AddAction(new LockFaceDirectionLogicAction(true));
            fireStraightProjectile.AddAction(new Play3DSoundLogicAction(this, m_target,"SkeletonArcher_Load"));
            fireStraightProjectile.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack"));
            fireStraightProjectile.AddAction(new DelayLogicAction(m_fireDelay));
            fireStraightProjectile.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            fireStraightProjectile.AddAction(new Play3DSoundLogicAction(this, m_target,"SkeletonArcher_Attack_01", "SkeletonArcher_Attack_02", "SkeletonArcher_Attack_03"));
            fireStraightProjectile.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack"));
            fireStraightProjectile.AddAction(new DelayLogicAction(0.5f));
            fireStraightProjectile.AddAction(new ChangePropertyLogicAction(this.GetChildAt(1), "Rotation", 0));
            fireStraightProjectile.AddAction(new DelayLogicAction(0.5f));
            fireStraightProjectile.AddAction(new LockFaceDirectionLogicAction(false));

            LogicSet fireStraightAdvancedProjectile = new LogicSet(this);
            fireStraightAdvancedProjectile.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false));
            projData.Angle = new Vector2(-20, -20);
            fireStraightAdvancedProjectile.AddAction(new RunFunctionLogicAction(this, "AngleArcher", projData.Angle.X));
            fireStraightAdvancedProjectile.AddAction(new LockFaceDirectionLogicAction(true));
            fireStraightAdvancedProjectile.AddAction(new Play3DSoundLogicAction(this, m_target,"SkeletonArcher_Load"));
            fireStraightAdvancedProjectile.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack"));
            fireStraightAdvancedProjectile.AddAction(new DelayLogicAction(m_fireDelay));
            projData.Angle = new Vector2(-20, -20);
            fireStraightAdvancedProjectile.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-50, -50);
            fireStraightAdvancedProjectile.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            fireStraightAdvancedProjectile.AddAction(new Play3DSoundLogicAction(this, m_target,"SkeletonArcher_Attack_01", "SkeletonArcher_Attack_02", "SkeletonArcher_Attack_03"));
            fireStraightAdvancedProjectile.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack"));
            fireStraightAdvancedProjectile.AddAction(new DelayLogicAction(0.5f));
            fireStraightAdvancedProjectile.AddAction(new ChangePropertyLogicAction(this.GetChildAt(1), "Rotation", 0));
            fireStraightAdvancedProjectile.AddAction(new DelayLogicAction(0.5f));
            fireStraightAdvancedProjectile.AddAction(new LockFaceDirectionLogicAction(false));

            LogicSet fireStraightExpertProjectile = new LogicSet(this);
            fireStraightExpertProjectile.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false));
            projData.Angle = new Vector2(-35, -35);
            fireStraightExpertProjectile.AddAction(new RunFunctionLogicAction(this, "AngleArcher", projData.Angle.X));
            fireStraightExpertProjectile.AddAction(new LockFaceDirectionLogicAction(true));
            fireStraightExpertProjectile.AddAction(new Play3DSoundLogicAction(this, m_target,"SkeletonArcher_Load"));
            fireStraightExpertProjectile.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack"));
            fireStraightExpertProjectile.AddAction(new DelayLogicAction(m_fireDelay));
            projData.Angle = new Vector2(-35, -35);
            fireStraightExpertProjectile.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-15, -15);
            fireStraightExpertProjectile.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-55, -55);
            fireStraightExpertProjectile.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            fireStraightExpertProjectile.AddAction(new Play3DSoundLogicAction(this, m_target,"SkeletonArcher_Attack_01", "SkeletonArcher_Attack_02", "SkeletonArcher_Attack_03"));
            fireStraightExpertProjectile.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack"));
            fireStraightExpertProjectile.AddAction(new DelayLogicAction(0.5f));
            fireStraightExpertProjectile.AddAction(new ChangePropertyLogicAction(this.GetChildAt(1), "Rotation", 0));
            fireStraightExpertProjectile.AddAction(new DelayLogicAction(0.5f));
            fireStraightExpertProjectile.AddAction(new LockFaceDirectionLogicAction(false));
            #endregion

            #region FireArchProjectile

            LogicSet fireArchProjectile = new LogicSet(this);
            fireArchProjectile.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false));
            projData.Angle = new Vector2(-50, -50);
            fireArchProjectile.AddAction(new RunFunctionLogicAction(this, "AngleArcher", projData.Angle.X));
            fireArchProjectile.AddAction(new LockFaceDirectionLogicAction(true));
            fireArchProjectile.AddAction(new Play3DSoundLogicAction(this, m_target,"SkeletonArcher_Load"));
            fireArchProjectile.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack"));
            fireArchProjectile.AddAction(new DelayLogicAction(m_fireDelay));
            projData.Angle = new Vector2(-50, -50);
            fireArchProjectile.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            //projData.Angle = new Vector2(-75, -75);
            //fireArchProjectile.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            //projData.Angle = new Vector2(-35, -35);
            //fireArchProjectile.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            fireArchProjectile.AddAction(new Play3DSoundLogicAction(this, m_target,"SkeletonArcher_Attack_01", "SkeletonArcher_Attack_02", "SkeletonArcher_Attack_03"));
            fireArchProjectile.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack"));
            fireArchProjectile.AddAction(new DelayLogicAction(0.5f));
            fireArchProjectile.AddAction(new ChangePropertyLogicAction(this.GetChildAt(1), "Rotation", 0));
            fireArchProjectile.AddAction(new DelayLogicAction(0.5f));
            fireArchProjectile.AddAction(new LockFaceDirectionLogicAction(false));

            LogicSet fireArchAdvancedProjectile = new LogicSet(this);
            fireArchAdvancedProjectile.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false));
            projData.Angle = new Vector2(-50, -50);
            fireArchAdvancedProjectile.AddAction(new RunFunctionLogicAction(this, "AngleArcher", projData.Angle.X));
            fireArchAdvancedProjectile.AddAction(new LockFaceDirectionLogicAction(true));
            fireArchAdvancedProjectile.AddAction(new Play3DSoundLogicAction(this, m_target,"SkeletonArcher_Load"));
            fireArchAdvancedProjectile.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack"));
            fireArchAdvancedProjectile.AddAction(new DelayLogicAction(m_fireDelay));
            projData.Angle = new Vector2(-50, -50);
            fireArchAdvancedProjectile.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-75, -75);
            fireArchAdvancedProjectile.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            fireArchAdvancedProjectile.AddAction(new Play3DSoundLogicAction(this, m_target,"SkeletonArcher_Attack_01", "SkeletonArcher_Attack_02", "SkeletonArcher_Attack_03"));
            fireArchAdvancedProjectile.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack"));
            fireArchAdvancedProjectile.AddAction(new DelayLogicAction(0.5f));
            fireArchAdvancedProjectile.AddAction(new ChangePropertyLogicAction(this.GetChildAt(1), "Rotation", 0));
            fireArchAdvancedProjectile.AddAction(new DelayLogicAction(0.5f));
            fireArchAdvancedProjectile.AddAction(new LockFaceDirectionLogicAction(false));

            LogicSet fireArchExpertProjectile = new LogicSet(this);
            fireArchExpertProjectile.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false));
            projData.Angle = new Vector2(-60, -60);
            fireArchExpertProjectile.AddAction(new RunFunctionLogicAction(this, "AngleArcher", projData.Angle.X));
            fireArchExpertProjectile.AddAction(new LockFaceDirectionLogicAction(true));
            fireArchExpertProjectile.AddAction(new Play3DSoundLogicAction(this, m_target,"SkeletonArcher_Load"));
            fireArchExpertProjectile.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack"));
            fireArchExpertProjectile.AddAction(new DelayLogicAction(m_fireDelay));
            projData.Angle = new Vector2(-60, -60);
            fireArchExpertProjectile.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-75, -75);
            fireArchExpertProjectile.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-45, -45);
            fireArchExpertProjectile.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            fireArchExpertProjectile.AddAction(new Play3DSoundLogicAction(this, m_target,"SkeletonArcher_Attack_01", "SkeletonArcher_Attack_02", "SkeletonArcher_Attack_03"));
            fireArchExpertProjectile.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack"));
            fireArchExpertProjectile.AddAction(new DelayLogicAction(0.5f));
            fireArchExpertProjectile.AddAction(new ChangePropertyLogicAction(this.GetChildAt(1), "Rotation", 0));
            fireArchExpertProjectile.AddAction(new DelayLogicAction(0.5f));
            fireArchExpertProjectile.AddAction(new LockFaceDirectionLogicAction(false));
            #endregion

            #region fireHighArchProjectile
            LogicSet fireHighArchProjectile = new LogicSet(this);
            fireHighArchProjectile.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false));
            projData.Angle = new Vector2(-70, -70);
            fireHighArchProjectile.AddAction(new RunFunctionLogicAction(this, "AngleArcher", projData.Angle.X));
            fireHighArchProjectile.AddAction(new LockFaceDirectionLogicAction(true));
            fireHighArchProjectile.AddAction(new Play3DSoundLogicAction(this, m_target,"SkeletonArcher_Load"));
            fireHighArchProjectile.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack"));
            fireHighArchProjectile.AddAction(new DelayLogicAction(m_fireDelay));
            projData.Angle = new Vector2(-70, -70);
            fireHighArchProjectile.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            fireHighArchProjectile.AddAction(new Play3DSoundLogicAction(this, m_target,"SkeletonArcher_Attack_01", "SkeletonArcher_Attack_02", "SkeletonArcher_Attack_03"));
            fireHighArchProjectile.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack"));
            fireHighArchProjectile.AddAction(new DelayLogicAction(0.5f));
            fireHighArchProjectile.AddAction(new ChangePropertyLogicAction(this.GetChildAt(1), "Rotation", 0));
            fireHighArchProjectile.AddAction(new DelayLogicAction(0.5f));
            fireHighArchProjectile.AddAction(new LockFaceDirectionLogicAction(false));

            LogicSet fireHighArchAdvancedProjectile = new LogicSet(this);
            fireHighArchAdvancedProjectile.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false));
            projData.Angle = new Vector2(-55, -55);
            fireHighArchAdvancedProjectile.AddAction(new RunFunctionLogicAction(this, "AngleArcher", projData.Angle.X));
            fireHighArchAdvancedProjectile.AddAction(new LockFaceDirectionLogicAction(true));
            fireHighArchAdvancedProjectile.AddAction(new Play3DSoundLogicAction(this, m_target,"SkeletonArcher_Load"));
            fireHighArchAdvancedProjectile.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack"));
            fireHighArchAdvancedProjectile.AddAction(new DelayLogicAction(m_fireDelay));
            projData.Angle = new Vector2(-55, -55);
            fireHighArchAdvancedProjectile.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-85, -85);
            fireHighArchAdvancedProjectile.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            fireHighArchAdvancedProjectile.AddAction(new Play3DSoundLogicAction(this, m_target,"SkeletonArcher_Attack_01", "SkeletonArcher_Attack_02", "SkeletonArcher_Attack_03"));
            fireHighArchAdvancedProjectile.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack"));
            fireHighArchAdvancedProjectile.AddAction(new DelayLogicAction(0.5f));
            fireHighArchAdvancedProjectile.AddAction(new ChangePropertyLogicAction(this.GetChildAt(1), "Rotation", 0));
            fireHighArchAdvancedProjectile.AddAction(new DelayLogicAction(0.5f));
            fireHighArchAdvancedProjectile.AddAction(new LockFaceDirectionLogicAction(false));

            LogicSet fireHighArchExpertProjectile = new LogicSet(this);
            fireHighArchExpertProjectile.AddAction(new ChangeSpriteLogicAction("EnemySkeletonArcherAttack_Character", false, false));
            projData.Angle = new Vector2(-90, -90);
            fireHighArchExpertProjectile.AddAction(new RunFunctionLogicAction(this, "AngleArcher", projData.Angle.X));
            fireHighArchExpertProjectile.AddAction(new LockFaceDirectionLogicAction(true));
            fireHighArchExpertProjectile.AddAction(new Play3DSoundLogicAction(this, m_target,"SkeletonArcher_Load"));
            fireHighArchExpertProjectile.AddAction(new PlayAnimationLogicAction("Start", "BeginAttack"));
            fireHighArchExpertProjectile.AddAction(new DelayLogicAction(m_fireDelay));

            projData.Angle = new Vector2(-90, -90);
            fireHighArchExpertProjectile.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-85, -85);
            fireHighArchExpertProjectile.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-95, -95);
            fireHighArchExpertProjectile.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            fireHighArchExpertProjectile.AddAction(new Play3DSoundLogicAction(this, m_target,"SkeletonArcher_Attack_01", "SkeletonArcher_Attack_02", "SkeletonArcher_Attack_03"));
            fireHighArchExpertProjectile.AddAction(new PlayAnimationLogicAction("Attack", "EndAttack"));
            fireHighArchExpertProjectile.AddAction(new DelayLogicAction(0.5f));
            fireHighArchExpertProjectile.AddAction(new ChangePropertyLogicAction(this.GetChildAt(1), "Rotation", 0));
            fireHighArchExpertProjectile.AddAction(new DelayLogicAction(0.5f));
            fireHighArchExpertProjectile.AddAction(new LockFaceDirectionLogicAction(false));
            #endregion

            m_generalBasicLB.AddLogicSet(walkStopLS, fireStraightProjectile, fireHighArchProjectile, fireArchProjectile);
            m_generalAdvancedLB.AddLogicSet(walkStopLS, fireStraightAdvancedProjectile, fireHighArchAdvancedProjectile, fireArchAdvancedProjectile);
            m_generalExpertLB.AddLogicSet(walkStopLS, fireStraightExpertProjectile, fireHighArchExpertProjectile, fireArchExpertProjectile);
            
            //m_generalAdvancedLB.AddLogicSet(fireProjectileLS);
            //m_generalExpertLB.AddLogicSet(fireProjectileLS);
            //m_generalMiniBossLB.AddLogicSet(fireProjectileLS);

            projData.Dispose();

            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);

            _objectList[1].TextureColor = TintablePart.TextureColor;

            base.InitializeLogic();
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case (STATE_MELEE_ENGAGE):
                    RunLogicBlock(false, m_generalBasicLB, 20, 50, 15,15);
                    break;
                case (STATE_PROJECTILE_ENGAGE):
                    RunLogicBlock(false, m_generalBasicLB, 20, 15, 50, 15);
                    break;
                case (STATE_ENGAGE):
                    RunLogicBlock(false, m_generalBasicLB, 20, 15, 15, 50);
                    break;
                case (STATE_WANDER):
                    RunLogicBlock(false, m_generalBasicLB, 100, 0, 0, 0);
                    break;
                default:
                    break;
            }
        }

        protected override void RunAdvancedLogic()
        {
            switch (State)
            {
                case (STATE_MELEE_ENGAGE):
                    RunLogicBlock(false, m_generalAdvancedLB, 20, 50, 15, 15);
                    break;
                case (STATE_PROJECTILE_ENGAGE):
                    RunLogicBlock(false, m_generalAdvancedLB, 20, 15, 50, 15);
                    break;
                case (STATE_ENGAGE):
                    RunLogicBlock(false, m_generalAdvancedLB, 20, 15, 15, 50);
                    break;
                case (STATE_WANDER):
                    RunLogicBlock(false, m_generalAdvancedLB, 100, 0, 0, 0);
                    break;
                default:
                    break;
            }
        }

        protected override void RunExpertLogic()
        {
            switch (State)
            {
                case (STATE_MELEE_ENGAGE):
                    RunLogicBlock(false, m_generalExpertLB, 20, 15, 50, 15);
                    break;
                case (STATE_PROJECTILE_ENGAGE):
                    RunLogicBlock(false, m_generalExpertLB, 20, 50, 15, 15);
                    break;
                case (STATE_ENGAGE):
                    RunLogicBlock(false, m_generalExpertLB, 20, 15, 15, 50);
                    break;
                case (STATE_WANDER):
                    RunLogicBlock(false, m_generalExpertLB, 100, 0, 0, 0);
                    break;
                default:
                    break;
            }
        }

        protected override void RunMinibossLogic()
        {
            switch (State)
            {
                case (STATE_MELEE_ENGAGE):
                    RunLogicBlock(false, m_generalBasicLB, 20, 50, 15, 15);
                    break;
                case (STATE_PROJECTILE_ENGAGE):
                    RunLogicBlock(false, m_generalBasicLB, 20, 15, 50, 15);
                    break;
                case (STATE_ENGAGE):
                    RunLogicBlock(false, m_generalBasicLB, 20, 15, 15, 50);
                    break;
                case (STATE_WANDER):
                    RunLogicBlock(false, m_generalBasicLB, 100, 0, 0, 0);
                    break;
                default:
                    break;
            }
        }

        public void AngleArcher(float angle)
        {
            if (this.Flip == Microsoft.Xna.Framework.Graphics.SpriteEffects.None)
                this.GetChildAt(1).Rotation = angle;
            else
                this.GetChildAt(1).Rotation = -angle;
        }

        public override void ResetState()
        {
            this.GetChildAt(1).Rotation = 0;
            base.ResetState();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
        {
            SoundManager.Play3DSound(this, Game.ScreenManager.Player, "SkeletonAttack1");
            base.HitEnemy(damage, position, isPlayer);
        }

        public EnemyObj_SkeletonArcher(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("EnemySkeletonArcherIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            this.Type = EnemyType.SkeletonArcher;
            TintablePart = _objectList[0];
        }
    }
}
