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
    public class EnemyObj_Wolf: EnemyObj
    {
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalCooldownLB = new LogicBlock();
        private LogicBlock m_wolfHitLB = new LogicBlock();

        public bool Chasing { get; set; }
        private float PounceDelay = 0.3f;
        private float PounceLandDelay = 0.5f; // How long after a wolf lands does he start chasing again.
        private Color FurColour = Color.White;

        private float m_startDelay = 1f;//  A special delay for wolves since they move too quick.
        private float m_startDelayCounter = 0;

        private FrameSoundObj m_runFrameSound;

        protected override void InitializeEV()
        {
            #region Basic Variables - General
            Name = EnemyEV.WOLF_BASIC_NAME;
            LocStringID = EnemyEV.WOLF_BASIC_NAME_LOC_ID;

            MaxHealth = EnemyEV.WOLF_BASIC_MAX_HEALTH;
            Damage = EnemyEV.WOLF_BASIC_DAMAGE;
            XPValue = EnemyEV.WOLF_BASIC_XP_VALUE;

            MinMoneyDropAmount = EnemyEV.WOLF_BASIC_MIN_DROP_AMOUNT;
            MaxMoneyDropAmount = EnemyEV.WOLF_BASIC_MAX_DROP_AMOUNT;
            MoneyDropChance = EnemyEV.WOLF_BASIC_DROP_CHANCE;

            Speed = EnemyEV.WOLF_BASIC_SPEED;
            TurnSpeed = EnemyEV.WOLF_BASIC_TURN_SPEED;
            ProjectileSpeed = EnemyEV.WOLF_BASIC_PROJECTILE_SPEED;
            JumpHeight = EnemyEV.WOLF_BASIC_JUMP;
            CooldownTime = EnemyEV.WOLF_BASIC_COOLDOWN;
            AnimationDelay = 1 / EnemyEV.WOLF_BASIC_ANIMATION_DELAY;

            AlwaysFaceTarget = EnemyEV.WOLF_BASIC_ALWAYS_FACE_TARGET;
            CanFallOffLedges = EnemyEV.WOLF_BASIC_CAN_FALL_OFF_LEDGES;
            CanBeKnockedBack = EnemyEV.WOLF_BASIC_CAN_BE_KNOCKED_BACK;
            IsWeighted = EnemyEV.WOLF_BASIC_IS_WEIGHTED;

            Scale = EnemyEV.WolfBasicScale;
            ProjectileScale = EnemyEV.WolfBasicProjectileScale;
            TintablePart.TextureColor = EnemyEV.WolfBasicTint;

            MeleeRadius = EnemyEV.WOLF_BASIC_MELEE_RADIUS;
            ProjectileRadius = EnemyEV.WOLF_BASIC_PROJECTILE_RADIUS;
            EngageRadius = EnemyEV.WOLF_BASIC_ENGAGE_RADIUS;

            ProjectileDamage = Damage;
            KnockBack = EnemyEV.WolfBasicKnockBack;
            #endregion

            InitialLogicDelay = 1;

            switch (Difficulty)
            {
                case (GameTypes.EnemyDifficulty.MINIBOSS):
                    #region Miniboss Variables - General
                    Name = EnemyEV.WOLF_MINIBOSS_NAME;
                    LocStringID = EnemyEV.WOLF_MINIBOSS_NAME_LOC_ID;

                    MaxHealth = EnemyEV.WOLF_MINIBOSS_MAX_HEALTH;
                    Damage = EnemyEV.WOLF_MINIBOSS_DAMAGE;
                    XPValue = EnemyEV.WOLF_MINIBOSS_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.WOLF_MINIBOSS_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.WOLF_MINIBOSS_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.WOLF_MINIBOSS_DROP_CHANCE;

                    Speed = EnemyEV.WOLF_MINIBOSS_SPEED;
                    TurnSpeed = EnemyEV.WOLF_MINIBOSS_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.WOLF_MINIBOSS_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.WOLF_MINIBOSS_JUMP;
                    CooldownTime = EnemyEV.WOLF_MINIBOSS_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.WOLF_MINIBOSS_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.WOLF_MINIBOSS_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.WOLF_MINIBOSS_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.WOLF_MINIBOSS_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.WOLF_MINIBOSS_IS_WEIGHTED;

                    Scale = EnemyEV.WolfMinibossScale;
                    ProjectileScale = EnemyEV.WolfMinibossProjectileScale;
                    TintablePart.TextureColor = EnemyEV.WolfMinibossTint;

                    MeleeRadius = EnemyEV.WOLF_MINIBOSS_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.WOLF_MINIBOSS_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.WOLF_MINIBOSS_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.WolfMinibossKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.EXPERT):
                    #region Expert Variables - General
                    Name = EnemyEV.WOLF_EXPERT_NAME;
                    LocStringID = EnemyEV.WOLF_EXPERT_NAME_LOC_ID;

                    MaxHealth = EnemyEV.WOLF_EXPERT_MAX_HEALTH;
                    Damage = EnemyEV.WOLF_EXPERT_DAMAGE;
                    XPValue = EnemyEV.WOLF_EXPERT_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.WOLF_EXPERT_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.WOLF_EXPERT_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.WOLF_EXPERT_DROP_CHANCE;

                    Speed = EnemyEV.WOLF_EXPERT_SPEED;
                    TurnSpeed = EnemyEV.WOLF_EXPERT_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.WOLF_EXPERT_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.WOLF_EXPERT_JUMP;
                    CooldownTime = EnemyEV.WOLF_EXPERT_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.WOLF_EXPERT_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.WOLF_EXPERT_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.WOLF_EXPERT_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.WOLF_EXPERT_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.WOLF_EXPERT_IS_WEIGHTED;

                    Scale = EnemyEV.WolfExpertScale;
                    ProjectileScale = EnemyEV.WolfExpertProjectileScale;
                    TintablePart.TextureColor = EnemyEV.WolfExpertTint;

                    MeleeRadius = EnemyEV.WOLF_EXPERT_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.WOLF_EXPERT_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.WOLF_EXPERT_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.WolfExpertKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.ADVANCED):
                    #region Advanced Variables - General
                    Name = EnemyEV.WOLF_ADVANCED_NAME;
                    LocStringID = EnemyEV.WOLF_ADVANCED_NAME_LOC_ID;

                    MaxHealth = EnemyEV.WOLF_ADVANCED_MAX_HEALTH;
                    Damage = EnemyEV.WOLF_ADVANCED_DAMAGE;
                    XPValue = EnemyEV.WOLF_ADVANCED_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.WOLF_ADVANCED_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.WOLF_ADVANCED_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.WOLF_ADVANCED_DROP_CHANCE;

                    Speed = EnemyEV.WOLF_ADVANCED_SPEED;
                    TurnSpeed = EnemyEV.WOLF_ADVANCED_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.WOLF_ADVANCED_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.WOLF_ADVANCED_JUMP;
                    CooldownTime = EnemyEV.WOLF_ADVANCED_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.WOLF_ADVANCED_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.WOLF_ADVANCED_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.WOLF_ADVANCED_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.WOLF_ADVANCED_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.WOLF_ADVANCED_IS_WEIGHTED;

                    Scale = EnemyEV.WolfAdvancedScale;
                    ProjectileScale = EnemyEV.WolfAdvancedProjectileScale;
                    TintablePart.TextureColor = EnemyEV.WolfAdvancedTint;

                    MeleeRadius = EnemyEV.WOLF_ADVANCED_MELEE_RADIUS;
                    EngageRadius = EnemyEV.WOLF_ADVANCED_ENGAGE_RADIUS;
                    ProjectileRadius = EnemyEV.WOLF_ADVANCED_PROJECTILE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.WolfAdvancedKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.BASIC):
                default:
                    //this.MaxHealth = 999;
                    break;
            }		
        }

        protected override void InitializeLogic()
        {
            LogicSet walkTowardsLS = new LogicSet(this);
            walkTowardsLS.AddAction(new LockFaceDirectionLogicAction(false));
            walkTowardsLS.AddAction(new MoveLogicAction(m_target, true));
            walkTowardsLS.AddAction(new LockFaceDirectionLogicAction(true));
            walkTowardsLS.AddAction(new ChangeSpriteLogicAction("EnemyWargRun_Character", true, true));
            walkTowardsLS.AddAction(new ChangePropertyLogicAction(this, "Chasing", true));
            walkTowardsLS.AddAction(new DelayLogicAction(1.0f));

            LogicSet stopWalkLS = new LogicSet(this);
            stopWalkLS.AddAction(new LockFaceDirectionLogicAction(false));
            stopWalkLS.AddAction(new MoveLogicAction(m_target, true, 0));
            stopWalkLS.AddAction(new ChangeSpriteLogicAction("EnemyWargIdle_Character", true, true));
            stopWalkLS.AddAction(new ChangePropertyLogicAction(this, "Chasing", false));
            stopWalkLS.AddAction(new DelayLogicAction(1.0f));

            LogicSet jumpLS = new LogicSet(this);
            jumpLS.AddAction(new GroundCheckLogicAction()); // Make sure it can only jump while touching ground.
            jumpLS.AddAction(new LockFaceDirectionLogicAction(false));
            jumpLS.AddAction(new MoveLogicAction(m_target, true, 0));
            jumpLS.AddAction(new LockFaceDirectionLogicAction(true));
            jumpLS.AddAction(new Play3DSoundLogicAction(this,  Game.ScreenManager.Player,"Wolf_Attack"));
            jumpLS.AddAction(new ChangeSpriteLogicAction("EnemyWargPounce_Character", true, true));
            jumpLS.AddAction(new DelayLogicAction(PounceDelay));
            jumpLS.AddAction(new ChangeSpriteLogicAction("EnemyWargJump_Character", false, false));
            jumpLS.AddAction(new PlayAnimationLogicAction("Start", "Windup"));
            jumpLS.AddAction(new MoveDirectionLogicAction());
            jumpLS.AddAction(new JumpLogicAction());
            jumpLS.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            jumpLS.AddAction(new GroundCheckLogicAction());
            jumpLS.AddAction(new ChangeSpriteLogicAction("EnemyWargIdle_Character", true, true));
            jumpLS.AddAction(new MoveLogicAction(m_target, true, 0));
            jumpLS.AddAction(new DelayLogicAction(PounceLandDelay));



            LogicSet wolfHit = new LogicSet(this);
            wolfHit.AddAction(new ChangeSpriteLogicAction("EnemyWargHit_Character", false, false));
            wolfHit.AddAction(new DelayLogicAction(0.2f));
            wolfHit.AddAction(new GroundCheckLogicAction());

            m_generalBasicLB.AddLogicSet(walkTowardsLS, stopWalkLS, jumpLS);
            m_wolfHitLB.AddLogicSet(wolfHit);

            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);
            logicBlocksToDispose.Add(m_wolfHitLB);

            SetCooldownLogicBlock(m_generalCooldownLB, 40, 40, 20); //walkTowardsLS, walkAwayLS, walkStopLS

            base.InitializeLogic();
        }

        protected override void RunBasicLogic()
        {
            if (m_startDelayCounter <= 0)
            {
                switch (State)
                {
                    case (STATE_MELEE_ENGAGE):
                    case (STATE_PROJECTILE_ENGAGE):
                        if (m_target.Y < this.Y - m_target.Height)
                            RunLogicBlock(false, m_generalBasicLB, 0, 0, 100); // walkTowardsLS, stopLS, jumpLS
                        else
                            RunLogicBlock(false, m_generalBasicLB, 100, 0, 0);
                        break;
                    case (STATE_ENGAGE):
                        if (Chasing == false)
                            RunLogicBlock(false, m_generalBasicLB, 100, 0, 0);
                        break;
                    case (STATE_WANDER):
                        if (Chasing == true)
                            RunLogicBlock(false, m_generalBasicLB, 0, 100, 0);
                        break;
                    default:
                        break;
                }
            }
        }

        protected override void RunAdvancedLogic()
        {
            switch (State)
            {
                case (STATE_MELEE_ENGAGE):
                case (STATE_PROJECTILE_ENGAGE):
                    if (m_target.Y < this.Y - m_target.Height)
                        RunLogicBlock(false, m_generalBasicLB, 0, 0, 100); // walkTowardsLS, stopLS, jumpLS
                    else
                        RunLogicBlock(false, m_generalBasicLB, 100, 0, 0);
                    break;
                case (STATE_ENGAGE):
                    if (Chasing == false)
                        RunLogicBlock(false, m_generalBasicLB, 100, 0, 0);
                    break;
                case (STATE_WANDER):
                    if (Chasing == true)
                        RunLogicBlock(false, m_generalBasicLB, 0, 100, 0);
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
                case (STATE_PROJECTILE_ENGAGE):
                    if (m_target.Y < this.Y - m_target.Height)
                        RunLogicBlock(false, m_generalBasicLB, 0, 0, 100); // walkTowardsLS, stopLS, jumpLS
                    else
                        RunLogicBlock(false, m_generalBasicLB, 100, 0, 0);
                    break;
                case (STATE_ENGAGE):
                    if (Chasing == false)
                        RunLogicBlock(false, m_generalBasicLB, 100, 0, 0);
                    break;
                case (STATE_WANDER):
                    if (Chasing == true)
                        RunLogicBlock(false, m_generalBasicLB, 0, 100, 0);
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
                case (STATE_PROJECTILE_ENGAGE):
                    if (m_target.Y < this.Y - m_target.Height)
                        RunLogicBlock(false, m_generalBasicLB, 0, 0, 100); // walkTowardsLS, stopLS, jumpLS
                    else
                        RunLogicBlock(false, m_generalBasicLB, 100, 0, 0);
                    break;
                case (STATE_ENGAGE):
                    if (Chasing == false)
                        RunLogicBlock(false, m_generalBasicLB, 100, 0, 0);
                    break;
                case (STATE_WANDER):
                    if (Chasing == true)
                        RunLogicBlock(false, m_generalBasicLB, 0, 100, 0);
                    break;
                default:
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (m_startDelayCounter > 0)
                m_startDelayCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Maintains the enemy's speed in the air so that he can jump onto platforms.
            if (m_isTouchingGround == false && IsWeighted == true && CurrentSpeed == 0 && this.SpriteName == "EnemyWargJump_Character")
                this.CurrentSpeed = this.Speed;

            base.Update(gameTime);

            if (m_isTouchingGround == true && CurrentSpeed == 0 && IsAnimating == false)
            {
                this.ChangeSprite("EnemyWargIdle_Character");
                this.PlayAnimation(true);
            }

            if (this.SpriteName == "EnemyWargRun_Character")
                m_runFrameSound.Update();
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
        {
            SoundManager.Play3DSound(this, Game.ScreenManager.Player,"Wolf_Hit_01", "Wolf_Hit_02", "Wolf_Hit_03");
            if (m_currentActiveLB != null && m_currentActiveLB.IsActive == true)
                m_currentActiveLB.StopLogicBlock();

            m_currentActiveLB = m_wolfHitLB;
            m_currentActiveLB.RunLogicBlock(100);
            base.HitEnemy(damage, position, isPlayer);
        }

        public override void ResetState()
        {
            m_startDelayCounter = m_startDelay;
            base.ResetState();
        }

        public override void Reset()
        {
            m_startDelayCounter = m_startDelay;
            base.Reset();
        }

        public EnemyObj_Wolf(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("EnemyWargIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            this.Type = EnemyType.Wolf;
            m_startDelayCounter = m_startDelay;
            m_runFrameSound = new FrameSoundObj(this, 1, "Wolf_Move01", "Wolf_Move02", "Wolf_Move03");
        }
    }
}
