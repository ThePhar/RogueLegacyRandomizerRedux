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
    public class EnemyObj_Mimic : EnemyObj
    {
        private bool m_isAttacking = false;
        private LogicBlock m_generalBasicLB = new LogicBlock();

        private FrameSoundObj m_closeSound;

        protected override void InitializeEV()
        {
            //this.AnimationDelay = 1 / 20f;
            //this.Scale = new Vector2(2, 2);
            //this.MaxHealth = 150;
            //this.IsWeighted = true;
            //this.JumpHeight = 400;
            //this.Speed = 400;

            #region Basic Variables - General
            Name = EnemyEV.MIMIC_BASIC_NAME;
            LocStringID = EnemyEV.MIMIC_BASIC_NAME_LOC_ID;

            MaxHealth = EnemyEV.MIMIC_BASIC_MAX_HEALTH;
            Damage = EnemyEV.MIMIC_BASIC_DAMAGE;
            XPValue = EnemyEV.MIMIC_BASIC_XP_VALUE;

            MinMoneyDropAmount = EnemyEV.MIMIC_BASIC_MIN_DROP_AMOUNT;
            MaxMoneyDropAmount = EnemyEV.MIMIC_BASIC_MAX_DROP_AMOUNT;
            MoneyDropChance = EnemyEV.MIMIC_BASIC_DROP_CHANCE;

            Speed = EnemyEV.MIMIC_BASIC_SPEED;
            TurnSpeed = EnemyEV.MIMIC_BASIC_TURN_SPEED;
            ProjectileSpeed = EnemyEV.MIMIC_BASIC_PROJECTILE_SPEED;
            JumpHeight = EnemyEV.MIMIC_BASIC_JUMP;
            CooldownTime = EnemyEV.MIMIC_BASIC_COOLDOWN;
            AnimationDelay = 1 / EnemyEV.MIMIC_BASIC_ANIMATION_DELAY;

            AlwaysFaceTarget = EnemyEV.MIMIC_BASIC_ALWAYS_FACE_TARGET;
            CanFallOffLedges = EnemyEV.MIMIC_BASIC_CAN_FALL_OFF_LEDGES;
            CanBeKnockedBack = EnemyEV.MIMIC_BASIC_CAN_BE_KNOCKED_BACK;
            IsWeighted = EnemyEV.MIMIC_BASIC_IS_WEIGHTED;

            Scale = EnemyEV.MimicBasicScale;
            ProjectileScale = EnemyEV.MimicBasicProjectileScale;
            TintablePart.TextureColor = EnemyEV.MimicBasicTint;

            MeleeRadius = EnemyEV.MIMIC_BASIC_MELEE_RADIUS;
            ProjectileRadius = EnemyEV.MIMIC_BASIC_PROJECTILE_RADIUS;
            EngageRadius = EnemyEV.MIMIC_BASIC_ENGAGE_RADIUS;

            ProjectileDamage = Damage;
            KnockBack = EnemyEV.MimicBasicKnockBack;
            #endregion

            switch (Difficulty)
            {
                case (GameTypes.EnemyDifficulty.Miniboss):
                    #region Miniboss Variables - General
                    Name = EnemyEV.MIMIC_MINIBOSS_NAME;
                    LocStringID = EnemyEV.MIMIC_MINIBOSS_NAME_LOC_ID;

                    MaxHealth = EnemyEV.MIMIC_MINIBOSS_MAX_HEALTH;
                    Damage = EnemyEV.MIMIC_MINIBOSS_DAMAGE;
                    XPValue = EnemyEV.MIMIC_MINIBOSS_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.MIMIC_MINIBOSS_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.MIMIC_MINIBOSS_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.MIMIC_MINIBOSS_DROP_CHANCE;

                    Speed = EnemyEV.MIMIC_MINIBOSS_SPEED;
                    TurnSpeed = EnemyEV.MIMIC_MINIBOSS_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.MIMIC_MINIBOSS_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.MIMIC_MINIBOSS_JUMP;
                    CooldownTime = EnemyEV.MIMIC_MINIBOSS_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.MIMIC_MINIBOSS_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.MIMIC_MINIBOSS_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.MIMIC_MINIBOSS_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.MIMIC_MINIBOSS_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.MIMIC_MINIBOSS_IS_WEIGHTED;

                    Scale = EnemyEV.MimicMinibossScale;
                    ProjectileScale = EnemyEV.MimicMinibossProjectileScale;
                    TintablePart.TextureColor = EnemyEV.MimicMinibossTint;

                    MeleeRadius = EnemyEV.MIMIC_MINIBOSS_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.MIMIC_MINIBOSS_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.MIMIC_MINIBOSS_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.MimicMinibossKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Expert):
                    #region Expert Variables - General
                    Name = EnemyEV.MIMIC_EXPERT_NAME;
                    LocStringID = EnemyEV.MIMIC_EXPERT_NAME_LOC_ID;

                    MaxHealth = EnemyEV.MIMIC_EXPERT_MAX_HEALTH;
                    Damage = EnemyEV.MIMIC_EXPERT_DAMAGE;
                    XPValue = EnemyEV.MIMIC_EXPERT_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.MIMIC_EXPERT_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.MIMIC_EXPERT_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.MIMIC_EXPERT_DROP_CHANCE;

                    Speed = EnemyEV.MIMIC_EXPERT_SPEED;
                    TurnSpeed = EnemyEV.MIMIC_EXPERT_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.MIMIC_EXPERT_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.MIMIC_EXPERT_JUMP;
                    CooldownTime = EnemyEV.MIMIC_EXPERT_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.MIMIC_EXPERT_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.MIMIC_EXPERT_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.MIMIC_EXPERT_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.MIMIC_EXPERT_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.MIMIC_EXPERT_IS_WEIGHTED;

                    Scale = EnemyEV.MimicExpertScale;
                    ProjectileScale = EnemyEV.MimicExpertProjectileScale;
                    TintablePart.TextureColor = EnemyEV.MimicExpertTint;

                    MeleeRadius = EnemyEV.MIMIC_EXPERT_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.MIMIC_EXPERT_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.MIMIC_EXPERT_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.MimicExpertKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Advanced):
                    #region Advanced Variables - General
                    Name = EnemyEV.MIMIC_ADVANCED_NAME;
                    LocStringID = EnemyEV.MIMIC_ADVANCED_NAME_LOC_ID;

                    MaxHealth = EnemyEV.MIMIC_ADVANCED_MAX_HEALTH;
                    Damage = EnemyEV.MIMIC_ADVANCED_DAMAGE;
                    XPValue = EnemyEV.MIMIC_ADVANCED_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.MIMIC_ADVANCED_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.MIMIC_ADVANCED_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.MIMIC_ADVANCED_DROP_CHANCE;

                    Speed = EnemyEV.MIMIC_ADVANCED_SPEED;
                    TurnSpeed = EnemyEV.MIMIC_ADVANCED_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.MIMIC_ADVANCED_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.MIMIC_ADVANCED_JUMP;
                    CooldownTime = EnemyEV.MIMIC_ADVANCED_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.MIMIC_ADVANCED_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.MIMIC_ADVANCED_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.MIMIC_ADVANCED_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.MIMIC_ADVANCED_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.MIMIC_ADVANCED_IS_WEIGHTED;

                    Scale = EnemyEV.MimicAdvancedScale;
                    ProjectileScale = EnemyEV.MimicAdvancedProjectileScale;
                    TintablePart.TextureColor = EnemyEV.MimicAdvancedTint;

                    MeleeRadius = EnemyEV.MIMIC_ADVANCED_MELEE_RADIUS;
                    EngageRadius = EnemyEV.MIMIC_ADVANCED_ENGAGE_RADIUS;
                    ProjectileRadius = EnemyEV.MIMIC_ADVANCED_PROJECTILE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.MimicAdvancedKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Basic):
                default:
                    break;
            }

            this.LockFlip = true;
        }

        protected override void InitializeLogic()
        {
            LogicSet basicWarningLS = new LogicSet(this);
            basicWarningLS.AddAction(new ChangeSpriteLogicAction("EnemyMimicShake_Character",false, false));
            basicWarningLS.AddAction(new PlayAnimationLogicAction(false));
            basicWarningLS.AddAction(new ChangeSpriteLogicAction("EnemyMimicIdle_Character", true, false));
            basicWarningLS.AddAction(new DelayLogicAction(3));

            LogicSet jumpTowardsLS = new LogicSet(this);
            jumpTowardsLS.AddAction(new GroundCheckLogicAction()); // Make sure it can only jump while touching ground.
            jumpTowardsLS.AddAction(new LockFaceDirectionLogicAction(false));
            jumpTowardsLS.AddAction(new MoveLogicAction(m_target, true, 0));
            jumpTowardsLS.AddAction(new LockFaceDirectionLogicAction(true));
            jumpTowardsLS.AddAction(new ChangeSpriteLogicAction("EnemyMimicAttack_Character", true, true));
            jumpTowardsLS.AddAction(new MoveDirectionLogicAction());
            jumpTowardsLS.AddAction(new Play3DSoundLogicAction(this, m_target, "Chest_Open_Large"));
            jumpTowardsLS.AddAction(new JumpLogicAction());
            jumpTowardsLS.AddAction(new DelayLogicAction(0.3f));
            jumpTowardsLS.AddAction(new GroundCheckLogicAction());

            LogicSet jumpUpLS = new LogicSet(this);

            m_generalBasicLB.AddLogicSet(basicWarningLS, jumpTowardsLS);

            logicBlocksToDispose.Add(m_generalBasicLB);

            base.InitializeLogic();
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case (STATE_MELEE_ENGAGE):
                case (STATE_PROJECTILE_ENGAGE):
                case (STATE_ENGAGE):
                case (STATE_WANDER):
                default:
                    if (m_isAttacking == false)
                        RunLogicBlock(false, m_generalBasicLB, 100, 0); // basicWarningLS, jumpTowardsLS
                    else
                        RunLogicBlock(false, m_generalBasicLB, 0, 100); // basicWarningLS, jumpTowardsLS                    
                    break;
            }
        }

        protected override void RunAdvancedLogic()
        {
            switch (State)
            {
                case (STATE_MELEE_ENGAGE):
                case (STATE_PROJECTILE_ENGAGE):
                case (STATE_ENGAGE):
                case (STATE_WANDER):
                default:
                    if (m_isAttacking == false)
                        RunLogicBlock(false, m_generalBasicLB, 100, 0); // basicWarningLS, jumpTowardsLS
                    else
                        RunLogicBlock(false, m_generalBasicLB, 0, 100); // basicWarningLS, jumpTowardsLS                    
                    break;
            }
        }

        protected override void RunExpertLogic()
        {
            switch (State)
            {
                case (STATE_MELEE_ENGAGE):
                case (STATE_PROJECTILE_ENGAGE):
                case (STATE_ENGAGE):
                case (STATE_WANDER):
                default:
                    if (m_isAttacking == false)
                        RunLogicBlock(false, m_generalBasicLB, 100, 0); // basicWarningLS, jumpTowardsLS
                    else
                        RunLogicBlock(false, m_generalBasicLB, 0, 100); // basicWarningLS, jumpTowardsLS                    
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
                    if (m_isAttacking == false)
                        RunLogicBlock(false, m_generalBasicLB, 100, 0); // basicWarningLS, jumpTowardsLS
                    else
                        RunLogicBlock(false, m_generalBasicLB, 0, 100); // basicWarningLS, jumpTowardsLS                    
                    break;
            }
        }

        public override void HitEnemy(int damage, Vector2 collisionPt, bool isPlayer)
        {
            if (m_isAttacking == false)
            {
                m_currentActiveLB.StopLogicBlock();
                m_isAttacking = true;
                LockFlip = false;
            }
            base.HitEnemy(damage, collisionPt, isPlayer);
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
        {
            if (otherBox.AbsParent is PlayerObj)
            {
                if (m_isAttacking == false)
                {
                    m_currentActiveLB.StopLogicBlock();
                    m_isAttacking = true;
                    LockFlip = false;
                }
            }

            base.CollisionResponse(thisBox, otherBox, collisionResponseType);
        }


        public EnemyObj_Mimic(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("EnemyMimicIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            this.Type = EnemyType.MIMIC;
            this.OutlineWidth = 0;

            m_closeSound = new FrameSoundObj(this, m_target, 1, "Chest_Snap");
        }

        public override void Update(GameTime gameTime)
        {
            if (this.SpriteName == "EnemyMimicAttack_Character")
                m_closeSound.Update();
            base.Update(gameTime);
        }

        public override void Dispose()
        {
            if (IsDisposed == false)
            {
                m_closeSound.Dispose();
                m_closeSound = null;
                base.Dispose();
            }
        }
    }
}
