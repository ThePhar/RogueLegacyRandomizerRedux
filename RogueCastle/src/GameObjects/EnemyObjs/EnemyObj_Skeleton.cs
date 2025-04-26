using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.EnvironmentVariables;
using RogueCastle.GameStructs;
using RogueCastle.LogicActions;
using RogueCastle.Screens;

namespace RogueCastle
{
    public class EnemyObj_Skeleton : EnemyObj
    {
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();
        private LogicBlock m_generalMiniBossLB = new LogicBlock();
        private LogicBlock m_generalCooldownLB = new LogicBlock();

        private float AttackDelay = 0.1f;
        private float JumpDelay = 0.25f;

        protected override void InitializeEV()
        {
            #region Basic Variables - General
            Name = EnemyEV.SKELETON_BASIC_NAME;
            LocStringID = EnemyEV.SKELETON_BASIC_NAME_LOC_ID;

            MaxHealth = EnemyEV.SKELETON_BASIC_MAX_HEALTH;
            Damage = EnemyEV.SKELETON_BASIC_DAMAGE;
            XPValue = EnemyEV.SKELETON_BASIC_XP_VALUE;

            MinMoneyDropAmount = EnemyEV.SKELETON_BASIC_MIN_DROP_AMOUNT;
            MaxMoneyDropAmount = EnemyEV.SKELETON_BASIC_MAX_DROP_AMOUNT;
            MoneyDropChance = EnemyEV.SKELETON_BASIC_DROP_CHANCE;

            Speed = EnemyEV.SKELETON_BASIC_SPEED;
            TurnSpeed = EnemyEV.SKELETON_BASIC_TURN_SPEED;
            ProjectileSpeed = EnemyEV.SKELETON_BASIC_PROJECTILE_SPEED;
            JumpHeight = EnemyEV.SKELETON_BASIC_JUMP;
            CooldownTime = EnemyEV.SKELETON_BASIC_COOLDOWN;
            AnimationDelay = 1 / EnemyEV.SKELETON_BASIC_ANIMATION_DELAY;

            AlwaysFaceTarget = EnemyEV.SKELETON_BASIC_ALWAYS_FACE_TARGET;
            CanFallOffLedges = EnemyEV.SKELETON_BASIC_CAN_FALL_OFF_LEDGES;
            CanBeKnockedBack = EnemyEV.SKELETON_BASIC_CAN_BE_KNOCKED_BACK;
            IsWeighted = EnemyEV.SKELETON_BASIC_IS_WEIGHTED;

            Scale = EnemyEV.SkeletonBasicScale;
            ProjectileScale = EnemyEV.SkeletonBasicProjectileScale;
            TintablePart.TextureColor = EnemyEV.SkeletonBasicTint;

            MeleeRadius = EnemyEV.SKELETON_BASIC_MELEE_RADIUS;
            ProjectileRadius = EnemyEV.SKELETON_BASIC_PROJECTILE_RADIUS;
            EngageRadius = EnemyEV.SKELETON_BASIC_ENGAGE_RADIUS;

            ProjectileDamage = Damage;
            KnockBack = EnemyEV.SkeletonBasicKnockBack;
            #endregion

            switch (Difficulty)
            {
                case (GameTypes.EnemyDifficulty.Miniboss):
                    #region Miniboss Variables - General
                    Name = EnemyEV.SKELETON_MINIBOSS_NAME;
                    LocStringID = EnemyEV.SKELETON_MINIBOSS_NAME_LOC_ID;

                    MaxHealth = EnemyEV.SKELETON_MINIBOSS_MAX_HEALTH;
                    Damage = EnemyEV.SKELETON_MINIBOSS_DAMAGE;
                    XPValue = EnemyEV.SKELETON_MINIBOSS_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.SKELETON_MINIBOSS_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.SKELETON_MINIBOSS_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.SKELETON_MINIBOSS_DROP_CHANCE;

                    Speed = EnemyEV.SKELETON_MINIBOSS_SPEED;
                    TurnSpeed = EnemyEV.SKELETON_MINIBOSS_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.SKELETON_MINIBOSS_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.SKELETON_MINIBOSS_JUMP;
                    CooldownTime = EnemyEV.SKELETON_MINIBOSS_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.SKELETON_MINIBOSS_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.SKELETON_MINIBOSS_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.SKELETON_MINIBOSS_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.SKELETON_MINIBOSS_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.SKELETON_MINIBOSS_IS_WEIGHTED;

                    Scale = EnemyEV.SkeletonMinibossScale;
                    ProjectileScale = EnemyEV.SkeletonMinibossProjectileScale;
                    TintablePart.TextureColor = EnemyEV.SkeletonMinibossTint;

                    MeleeRadius = EnemyEV.SKELETON_MINIBOSS_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.SKELETON_MINIBOSS_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.SKELETON_MINIBOSS_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.SkeletonMinibossKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Expert):
                    #region Expert Variables - General
                    Name = EnemyEV.SKELETON_EXPERT_NAME;
                    LocStringID = EnemyEV.SKELETON_EXPERT_NAME_LOC_ID;

                    MaxHealth = EnemyEV.SKELETON_EXPERT_MAX_HEALTH;
                    Damage = EnemyEV.SKELETON_EXPERT_DAMAGE;
                    XPValue = EnemyEV.SKELETON_EXPERT_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.SKELETON_EXPERT_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.SKELETON_EXPERT_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.SKELETON_EXPERT_DROP_CHANCE;

                    Speed = EnemyEV.SKELETON_EXPERT_SPEED;
                    TurnSpeed = EnemyEV.SKELETON_EXPERT_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.SKELETON_EXPERT_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.SKELETON_EXPERT_JUMP;
                    CooldownTime = EnemyEV.SKELETON_EXPERT_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.SKELETON_EXPERT_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.SKELETON_EXPERT_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.SKELETON_EXPERT_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.SKELETON_EXPERT_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.SKELETON_EXPERT_IS_WEIGHTED;

                    Scale = EnemyEV.SkeletonExpertScale;
                    ProjectileScale = EnemyEV.SkeletonExpertProjectileScale;
                    TintablePart.TextureColor = EnemyEV.SkeletonExpertTint;

                    MeleeRadius = EnemyEV.SKELETON_EXPERT_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.SKELETON_EXPERT_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.SKELETON_EXPERT_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.SkeletonExpertKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Advanced):
                    #region Advanced Variables - General
                    Name = EnemyEV.SKELETON_ADVANCED_NAME;
                    LocStringID = EnemyEV.SKELETON_ADVANCED_NAME_LOC_ID;

                    MaxHealth = EnemyEV.SKELETON_ADVANCED_MAX_HEALTH;
                    Damage = EnemyEV.SKELETON_ADVANCED_DAMAGE;
                    XPValue = EnemyEV.SKELETON_ADVANCED_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.SKELETON_ADVANCED_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.SKELETON_ADVANCED_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.SKELETON_ADVANCED_DROP_CHANCE;

                    Speed = EnemyEV.SKELETON_ADVANCED_SPEED;
                    TurnSpeed = EnemyEV.SKELETON_ADVANCED_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.SKELETON_ADVANCED_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.SKELETON_ADVANCED_JUMP;
                    CooldownTime = EnemyEV.SKELETON_ADVANCED_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.SKELETON_ADVANCED_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.SKELETON_ADVANCED_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.SKELETON_ADVANCED_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.SKELETON_ADVANCED_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.SKELETON_ADVANCED_IS_WEIGHTED;

                    Scale = EnemyEV.SkeletonAdvancedScale;
                    ProjectileScale = EnemyEV.SkeletonAdvancedProjectileScale;
                    TintablePart.TextureColor = EnemyEV.SkeletonAdvancedTint;

                    MeleeRadius = EnemyEV.SKELETON_ADVANCED_MELEE_RADIUS;
                    EngageRadius = EnemyEV.SKELETON_ADVANCED_ENGAGE_RADIUS;
                    ProjectileRadius = EnemyEV.SKELETON_ADVANCED_PROJECTILE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.SkeletonAdvancedKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Basic):
                default:
                    break;
            }		
	

        }

        protected override void InitializeLogic()
        {
            ProjectileData projData = new ProjectileData(this)
            {
                SpriteName = "BoneProjectile_Sprite",
                SourceAnchor = new Vector2(20, -20),
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = true,
                RotationSpeed = 10,
                Damage = Damage,
                AngleOffset = 0,
                Angle = new Vector2(-72, -72),
                CollidesWithTerrain = false,
                Scale = ProjectileScale,
            };

            LogicSet walkTowardsLS = new LogicSet(this);
            walkTowardsLS.AddAction(new ChangeSpriteLogicAction("EnemySkeletonWalk_Character", true, true));
            walkTowardsLS.AddAction(new MoveLogicAction(m_target, true));
            walkTowardsLS.AddAction(new DelayLogicAction(0.2f, 0.75f));

            LogicSet walkAwayLS = new LogicSet(this);
            walkAwayLS.AddAction(new ChangeSpriteLogicAction("EnemySkeletonWalk_Character", true, true));
            walkAwayLS.AddAction(new MoveLogicAction(m_target, false));
            walkAwayLS.AddAction(new DelayLogicAction(0.2f, 0.75f));

            LogicSet walkStopLS = new LogicSet(this);
            walkStopLS.AddAction(new ChangeSpriteLogicAction("EnemySkeletonIdle_Character", true, true));
            walkStopLS.AddAction(new MoveLogicAction(m_target, true, 0));
            walkStopLS.AddAction(new DelayLogicAction(0.2f, 0.75f));


            LogicSet walkStopMiniBossLS = new LogicSet(this);
            walkStopMiniBossLS.AddAction(new StopAnimationLogicAction());
            walkStopMiniBossLS.AddAction(new MoveLogicAction(m_target, true, 0));
            walkStopMiniBossLS.AddAction(new DelayLogicAction(0.5f, 1.0f));   

            LogicSet throwBoneFarLS = new LogicSet(this);
            throwBoneFarLS.AddAction(new MoveLogicAction(m_target, true, 0));
            throwBoneFarLS.AddAction(new LockFaceDirectionLogicAction(true));
            throwBoneFarLS.AddAction(new ChangeSpriteLogicAction("EnemySkeletonAttack_Character", false, false));
            throwBoneFarLS.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            throwBoneFarLS.AddAction(new DelayLogicAction(AttackDelay));
            throwBoneFarLS.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SkeletonHit1"));
            throwBoneFarLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            throwBoneFarLS.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            throwBoneFarLS.AddAction(new DelayLogicAction(0.2f, 0.4f));
            throwBoneFarLS.AddAction(new LockFaceDirectionLogicAction(false));
            throwBoneFarLS.Tag = GameTypes.LOGIC_SET_TYPE_ATTACK;

            LogicSet throwBoneHighLS = new LogicSet(this);
            throwBoneHighLS.AddAction(new MoveLogicAction(m_target, true, 0));
            throwBoneHighLS.AddAction(new LockFaceDirectionLogicAction(true));
            throwBoneHighLS.AddAction(new ChangeSpriteLogicAction("EnemySkeletonAttack_Character", false, false));
            throwBoneHighLS.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            throwBoneHighLS.AddAction(new DelayLogicAction(AttackDelay));
            throwBoneHighLS.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SkeletonHit1"));
            projData.Angle = new Vector2(-85, -85);
            throwBoneHighLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            throwBoneHighLS.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            throwBoneHighLS.AddAction(new DelayLogicAction(0.2f, 0.4f));
            throwBoneHighLS.AddAction(new LockFaceDirectionLogicAction(false));
            throwBoneHighLS.Tag = GameTypes.LOGIC_SET_TYPE_ATTACK;

            LogicSet jumpBoneFarLS = new LogicSet(this);
            jumpBoneFarLS.AddAction(new MoveLogicAction(m_target, true, 0));
            jumpBoneFarLS.AddAction(new ChangeSpriteLogicAction("EnemySkeletonJump_Character", false, false));
            jumpBoneFarLS.AddAction(new PlayAnimationLogicAction(1, 3, false));
            jumpBoneFarLS.AddAction(new DelayLogicAction(JumpDelay));
            jumpBoneFarLS.AddAction(new JumpLogicAction());
            jumpBoneFarLS.AddAction(new LockFaceDirectionLogicAction(true));
            jumpBoneFarLS.AddAction(new ChangeSpriteLogicAction("EnemySkeletonAttack_Character", false, false));
            jumpBoneFarLS.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            jumpBoneFarLS.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SkeletonHit1"));
            projData.Angle = new Vector2(-72, -72);
            jumpBoneFarLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            jumpBoneFarLS.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            jumpBoneFarLS.AddAction(new DelayLogicAction(0.2f, 0.4f));
            jumpBoneFarLS.AddAction(new LockFaceDirectionLogicAction(false));
            jumpBoneFarLS.Tag = GameTypes.LOGIC_SET_TYPE_ATTACK;

            LogicSet jumpBoneHighLS = new LogicSet(this);
            jumpBoneHighLS.AddAction(new MoveLogicAction(m_target, true, 0));
            jumpBoneHighLS.AddAction(new ChangeSpriteLogicAction("EnemySkeletonJump_Character", false, false));
            jumpBoneHighLS.AddAction(new PlayAnimationLogicAction(1, 3, false));
            jumpBoneHighLS.AddAction(new DelayLogicAction(JumpDelay));
            jumpBoneHighLS.AddAction(new JumpLogicAction());
            jumpBoneHighLS.AddAction(new LockFaceDirectionLogicAction(true));
            jumpBoneHighLS.AddAction(new ChangeSpriteLogicAction("EnemySkeletonAttack_Character", false, false));
            jumpBoneHighLS.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            jumpBoneHighLS.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SkeletonHit1"));
            projData.Angle = new Vector2(-85, -85);
            jumpBoneHighLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            jumpBoneHighLS.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            jumpBoneHighLS.AddAction(new DelayLogicAction(0.2f, 0.4f));
            jumpBoneHighLS.AddAction(new LockFaceDirectionLogicAction(false));
            jumpBoneHighLS.Tag = GameTypes.LOGIC_SET_TYPE_ATTACK;

            LogicSet throwBoneExpertLS = new LogicSet(this);
            throwBoneExpertLS.AddAction(new MoveLogicAction(m_target, true, 0));
            throwBoneExpertLS.AddAction(new LockFaceDirectionLogicAction(true));
            throwBoneExpertLS.AddAction(new ChangeSpriteLogicAction("EnemySkeletonAttack_Character", false, false));
            throwBoneExpertLS.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            throwBoneExpertLS.AddAction(new DelayLogicAction(AttackDelay));
            throwBoneExpertLS.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SkeletonHit1"));
            ThrowThreeProjectiles(throwBoneExpertLS);
            throwBoneExpertLS.AddAction(new PlayAnimationLogicAction("Attack", "End"), Types.Sequence.Parallel);
            throwBoneExpertLS.AddAction(new DelayLogicAction(0.15f));
            ThrowThreeProjectiles(throwBoneExpertLS);
            throwBoneExpertLS.AddAction(new DelayLogicAction(0.15f));
            ThrowThreeProjectiles(throwBoneExpertLS);
            throwBoneExpertLS.AddAction(new DelayLogicAction(0.2f, 0.4f));
            throwBoneExpertLS.AddAction(new LockFaceDirectionLogicAction(false));
            throwBoneExpertLS.Tag = GameTypes.LOGIC_SET_TYPE_ATTACK;

            LogicSet jumpBoneExpertLS = new LogicSet(this);
            jumpBoneExpertLS.AddAction(new MoveLogicAction(m_target, true, 0));
            jumpBoneExpertLS.AddAction(new ChangeSpriteLogicAction("EnemySkeletonJump_Character", false, false));
            jumpBoneExpertLS.AddAction(new PlayAnimationLogicAction(1, 3, false));
            jumpBoneExpertLS.AddAction(new DelayLogicAction(JumpDelay));
            jumpBoneExpertLS.AddAction(new JumpLogicAction());
            jumpBoneExpertLS.AddAction(new LockFaceDirectionLogicAction(true));
            jumpBoneExpertLS.AddAction(new ChangeSpriteLogicAction("EnemySkeletonAttack_Character", false, false));
            jumpBoneExpertLS.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            jumpBoneExpertLS.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SkeletonHit1"));
            ThrowThreeProjectiles(jumpBoneExpertLS);
            jumpBoneExpertLS.AddAction(new PlayAnimationLogicAction("Attack", "End"), Types.Sequence.Parallel);
            jumpBoneExpertLS.AddAction(new DelayLogicAction(0.15f));
            ThrowThreeProjectiles(jumpBoneExpertLS);
            jumpBoneExpertLS.AddAction(new DelayLogicAction(0.15f));
            ThrowThreeProjectiles(jumpBoneExpertLS);
            jumpBoneExpertLS.AddAction(new DelayLogicAction(0.2f, 0.4f));
            jumpBoneExpertLS.AddAction(new LockFaceDirectionLogicAction(false));
            jumpBoneExpertLS.Tag = GameTypes.LOGIC_SET_TYPE_ATTACK;

            LogicSet throwBoneMiniBossLS = new LogicSet(this);
            throwBoneMiniBossLS.AddAction(new MoveLogicAction(m_target, true, 0));
            throwBoneMiniBossLS.AddAction(new LockFaceDirectionLogicAction(true));
            throwBoneMiniBossLS.AddAction(new ChangeSpriteLogicAction("EnemySkeletonAttack_Character", false, false));
            throwBoneMiniBossLS.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            throwBoneMiniBossLS.AddAction(new DelayLogicAction(AttackDelay));
            projData.Angle = new Vector2(-89, -35);
            projData.RotationSpeed = 8;
            projData.SourceAnchor = new Vector2(5, -20);
            throwBoneMiniBossLS.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SkeletonHit1"));
            throwBoneMiniBossLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            throwBoneMiniBossLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            throwBoneMiniBossLS.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            throwBoneMiniBossLS.AddAction(new LockFaceDirectionLogicAction(false));
            throwBoneMiniBossLS.AddAction(new ChangeSpriteLogicAction("EnemySkeletonIdle_Character", true, true));
            throwBoneMiniBossLS.AddAction(new DelayLogicAction(0.40f, 0.90f));
            

            LogicSet throwBoneMiniBossRageLS = new LogicSet(this);
            throwBoneMiniBossRageLS.AddAction(new MoveLogicAction(m_target, true, 0));
            throwBoneMiniBossRageLS.AddAction(new LockFaceDirectionLogicAction(true));
            throwBoneMiniBossRageLS.AddAction(new ChangeSpriteLogicAction("EnemySkeletonAttack_Character", false, false));
            throwBoneMiniBossRageLS.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            throwBoneMiniBossRageLS.AddAction(new DelayLogicAction(AttackDelay));
            throwBoneMiniBossRageLS.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SkeletonHit1"));
            throwBoneMiniBossRageLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            throwBoneMiniBossRageLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            throwBoneMiniBossRageLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            throwBoneMiniBossRageLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            throwBoneMiniBossRageLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            throwBoneMiniBossRageLS.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            throwBoneMiniBossRageLS.AddAction(new LockFaceDirectionLogicAction(false));
            throwBoneMiniBossRageLS.AddAction(new DelayLogicAction(0.15f, 0.35f));
            throwBoneMiniBossRageLS.AddAction(new ChangeSpriteLogicAction("EnemySkeletonIdle_Character", true, true));

            m_generalBasicLB.AddLogicSet(walkTowardsLS, walkAwayLS, walkStopLS, throwBoneFarLS, throwBoneHighLS);
            m_generalAdvancedLB.AddLogicSet(walkTowardsLS, walkAwayLS, walkStopLS, throwBoneFarLS, throwBoneHighLS, jumpBoneFarLS, jumpBoneHighLS);
            m_generalExpertLB.AddLogicSet(walkTowardsLS, walkAwayLS, walkStopLS, throwBoneExpertLS, jumpBoneExpertLS);
            m_generalMiniBossLB.AddLogicSet(walkTowardsLS, walkAwayLS, walkStopMiniBossLS, throwBoneMiniBossLS, throwBoneMiniBossRageLS);
            m_generalCooldownLB.AddLogicSet(walkTowardsLS, walkAwayLS, walkStopLS);

            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalMiniBossLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);

            SetCooldownLogicBlock(m_generalCooldownLB, 30, 30, 40); //walkTowardsLS, walkAwayLS, walkStopLS

            projData.Dispose();

            base.InitializeLogic();
        }

        private void ThrowThreeProjectiles(LogicSet ls)
        {
            ProjectileData projData = new ProjectileData(this)
            {
                SpriteName = "BoneProjectile_Sprite",
                SourceAnchor = new Vector2(20, -20),
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = true,
                RotationSpeed = 10,
                Damage = Damage,
                AngleOffset = 0,
                Angle = new Vector2(-72, -72),
                CollidesWithTerrain = false,
                Scale = ProjectileScale,
            };

            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Speed = new Vector2(ProjectileSpeed - 350, ProjectileSpeed - 350);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Speed = new Vector2(ProjectileSpeed + 350, ProjectileSpeed + 350);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            //ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, "BoneProjectile_Sprite", new Vector2(20, -20), -72, ProjectileSpeed, true, 10, Damage));
            //ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, "BoneProjectile_Sprite", new Vector2(20, -20), -72, ProjectileSpeed - 350, true, 10, Damage));
            //ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, "BoneProjectile_Sprite", new Vector2(20, -20), -72, ProjectileSpeed + 350, true, 10, Damage));
            //ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, "BoneProjectile_Sprite", new Vector2(20, -18), -60, 10, true, 10, Damage));
            //ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, "BoneProjectile_Sprite", new Vector2(20, -18), -60, 16, true, 10, Damage));
            //ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, "BoneProjectile_Sprite", new Vector2(20, -18), -60, 22, true, 10, Damage));

            projData.Dispose();
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case (STATE_MELEE_ENGAGE):
                    RunLogicBlock(true, m_generalBasicLB, 10, 10, 0, 30, 50); //walkTowardsLS, walkAwayLS, walkStopLS, throwBoneFarLS, throwBoneHighLS
                    break;
                case (STATE_PROJECTILE_ENGAGE):
                    RunLogicBlock(true, m_generalBasicLB, 10, 10, 0, 40, 40); //walkTowardsLS, walkAwayLS, walkStopLS, throwBoneFarLS, throwBoneHighLS
                    break;
                case (STATE_ENGAGE):
                case (STATE_WANDER):
                    RunLogicBlock(true, m_generalBasicLB, 40, 40, 20, 0, 0); //walkTowardsLS, walkAwayLS, walkStopLS, throwBoneFarLS, throwBoneHighLS
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
                case (STATE_PROJECTILE_ENGAGE):
                    RunLogicBlock(true, m_generalAdvancedLB, 10, 10, 0, 15, 15, 25, 25); //walkTowardsLS, walkAwayLS, walkStopLS, throwBoneFarLS, throwBoneHighLS, jumpBoneFarLS, jumpBoneHighLS
                    break;
                case (STATE_ENGAGE):
                case (STATE_WANDER):
                    RunLogicBlock(true, m_generalAdvancedLB, 40, 40, 20, 0, 0, 0, 0); //walkTowardsLS, walkAwayLS, walkStopLS, throwBoneFarLS, throwBoneHighLS, jumpBoneFarLS, jumpBoneHighLS
                    break;
                default:
                    RunBasicLogic();
                    break;
            }
        }

        protected override void RunExpertLogic()
        {
            switch (State)
            {
                case (STATE_MELEE_ENGAGE):
                case (STATE_PROJECTILE_ENGAGE):
                    RunLogicBlock(true, m_generalExpertLB, 15, 15, 0, 35, 35); //walkTowardsLS, walkAwayLS, walkStopLS, throwBoneExpertLS, jumpBoneExpertLS
                    break;
                case (STATE_ENGAGE):
                case (STATE_WANDER):
                    RunLogicBlock(true, m_generalExpertLB, 35, 35, 0, 0, 15); //walkTowardsLS, walkAwayLS, walkStopLS, throwBoneExpertLS, jumpBoneExpertLS
                    break;
                default:
                    RunBasicLogic();
                    break;
            }
        }

        protected override void RunMinibossLogic()
        {
            switch (State)
            {
                case (STATE_MELEE_ENGAGE):
                case (STATE_PROJECTILE_ENGAGE):
                case (STATE_ENGAGE):
                case (STATE_WANDER):
                default:
                    if (m_levelScreen.CurrentRoom.ActiveEnemies > 1)
                        RunLogicBlock(true, m_generalMiniBossLB, 0, 0, 10, 90, 0); //walkTowardsLS, walkAwayLS, walkStopMiniBossLS, throwBoneMiniBossLS, throwBoneMiniBossRageLS
                    else
                    {
                        Console.WriteLine("RAGING");
                        RunLogicBlock(true, m_generalMiniBossLB, 0, 0, 10, 0, 90); //walkTowardsLS, walkAwayLS, walkStopMiniBossLS, throwBoneMiniBossLS, throwBoneMiniBossRageLS
                    }
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (this.Difficulty == GameTypes.EnemyDifficulty.Miniboss && m_levelScreen.CurrentRoom.ActiveEnemies == 1)
                this.TintablePart.TextureColor = new Color(185, 0, 15);

            base.Update(gameTime);
        }

        public EnemyObj_Skeleton(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("EnemySkeletonIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            this.Type = EnemyType.SKELETON;
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
        {
            SoundManager.Play3DSound(this, Game.ScreenManager.Player,"SkeletonAttack1");
            base.HitEnemy(damage, position, isPlayer);
        }
    }
}
