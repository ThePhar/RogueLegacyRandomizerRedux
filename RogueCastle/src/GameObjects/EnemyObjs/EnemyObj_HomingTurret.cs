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
    public class EnemyObj_HomingTurret : EnemyObj
    {
        private float FireDelay = 5;

        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();
        private LogicBlock m_generalMiniBossLB = new LogicBlock();

        protected override void InitializeEV()
        {
            LockFlip = false;
            FireDelay = 2.0f;//5;

            #region Basic Variables - General
            Name = EnemyEV.HOMING_TURRET_BASIC_NAME;
            LocStringID = EnemyEV.HOMING_TURRET_BASIC_NAME_LOC_ID;

            MaxHealth = EnemyEV.HOMING_TURRET_BASIC_MAX_HEALTH;
            Damage = EnemyEV.HOMING_TURRET_BASIC_DAMAGE;
            XPValue = EnemyEV.HOMING_TURRET_BASIC_XP_VALUE;

            MinMoneyDropAmount = EnemyEV.HOMING_TURRET_BASIC_MIN_DROP_AMOUNT;
            MaxMoneyDropAmount = EnemyEV.HOMING_TURRET_BASIC_MAX_DROP_AMOUNT;
            MoneyDropChance = EnemyEV.HOMING_TURRET_BASIC_DROP_CHANCE;

            Speed = EnemyEV.HOMING_TURRET_BASIC_SPEED;
            TurnSpeed = EnemyEV.HOMING_TURRET_BASIC_TURN_SPEED;
            ProjectileSpeed = EnemyEV.HOMING_TURRET_BASIC_PROJECTILE_SPEED;
            JumpHeight = EnemyEV.HOMING_TURRET_BASIC_JUMP;
            CooldownTime = EnemyEV.HOMING_TURRET_BASIC_COOLDOWN;
            AnimationDelay = 1 / EnemyEV.HOMING_TURRET_BASIC_ANIMATION_DELAY;

            AlwaysFaceTarget = EnemyEV.HOMING_TURRET_BASIC_ALWAYS_FACE_TARGET;
            CanFallOffLedges = EnemyEV.HOMING_TURRET_BASIC_CAN_FALL_OFF_LEDGES;
            CanBeKnockedBack = EnemyEV.HOMING_TURRET_BASIC_CAN_BE_KNOCKED_BACK;
            IsWeighted = EnemyEV.HOMING_TURRET_BASIC_IS_WEIGHTED;

            Scale = EnemyEV.HomingTurretBasicScale;
            ProjectileScale = EnemyEV.HomingTurretBasicProjectileScale;
            TintablePart.TextureColor = EnemyEV.HomingTurretBasicTint;

            MeleeRadius = EnemyEV.HOMING_TURRET_BASIC_MELEE_RADIUS;
            ProjectileRadius = EnemyEV.HOMING_TURRET_BASIC_PROJECTILE_RADIUS;
            EngageRadius = EnemyEV.HOMING_TURRET_BASIC_ENGAGE_RADIUS;

            ProjectileDamage = Damage;
            KnockBack = EnemyEV.HomingTurretBasicKnockBack;
            #endregion

            InitialLogicDelay = 1;

            switch (Difficulty)
            {
                case (GameTypes.EnemyDifficulty.Miniboss):
                    #region Miniboss Variables - General
                    Name = EnemyEV.HOMING_TURRET_MINIBOSS_NAME;
                    LocStringID = EnemyEV.HOMING_TURRET_MINIBOSS_NAME_LOC_ID;

                    MaxHealth = EnemyEV.HOMING_TURRET_MINIBOSS_MAX_HEALTH;
                    Damage = EnemyEV.HOMING_TURRET_MINIBOSS_DAMAGE;
                    XPValue = EnemyEV.HOMING_TURRET_MINIBOSS_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.HOMING_TURRET_MINIBOSS_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.HOMING_TURRET_MINIBOSS_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.HOMING_TURRET_MINIBOSS_DROP_CHANCE;

                    Speed = EnemyEV.HOMING_TURRET_MINIBOSS_SPEED;
                    TurnSpeed = EnemyEV.HOMING_TURRET_MINIBOSS_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.HOMING_TURRET_MINIBOSS_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.HOMING_TURRET_MINIBOSS_JUMP;
                    CooldownTime = EnemyEV.HOMING_TURRET_MINIBOSS_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.HOMING_TURRET_MINIBOSS_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.HOMING_TURRET_MINIBOSS_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.HOMING_TURRET_MINIBOSS_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.HOMING_TURRET_MINIBOSS_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.HOMING_TURRET_MINIBOSS_IS_WEIGHTED;

                    Scale = EnemyEV.HomingTurretMinibossScale;
                    ProjectileScale = EnemyEV.HomingTurretMinibossProjectileScale;
                    TintablePart.TextureColor = EnemyEV.HomingTurretMinibossTint;

                    MeleeRadius = EnemyEV.HOMING_TURRET_MINIBOSS_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.HOMING_TURRET_MINIBOSS_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.HOMING_TURRET_MINIBOSS_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.HomingTurretMinibossKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Expert):
                    FireDelay = 2.25f;//5;
                    #region Expert Variables - General
                    Name = EnemyEV.HOMING_TURRET_EXPERT_NAME;
                    LocStringID = EnemyEV.HOMING_TURRET_EXPERT_NAME_LOC_ID;

                    MaxHealth = EnemyEV.HOMING_TURRET_EXPERT_MAX_HEALTH;
                    Damage = EnemyEV.HOMING_TURRET_EXPERT_DAMAGE;
                    XPValue = EnemyEV.HOMING_TURRET_EXPERT_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.HOMING_TURRET_EXPERT_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.HOMING_TURRET_EXPERT_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.HOMING_TURRET_EXPERT_DROP_CHANCE;

                    Speed = EnemyEV.HOMING_TURRET_EXPERT_SPEED;
                    TurnSpeed = EnemyEV.HOMING_TURRET_EXPERT_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.HOMING_TURRET_EXPERT_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.HOMING_TURRET_EXPERT_JUMP;
                    CooldownTime = EnemyEV.HOMING_TURRET_EXPERT_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.HOMING_TURRET_EXPERT_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.HOMING_TURRET_EXPERT_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.HOMING_TURRET_EXPERT_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.HOMING_TURRET_EXPERT_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.HOMING_TURRET_EXPERT_IS_WEIGHTED;

                    Scale = EnemyEV.HomingTurretExpertScale;
                    ProjectileScale = EnemyEV.HomingTurretExpertProjectileScale;
                    TintablePart.TextureColor = EnemyEV.HomingTurretExpertTint;

                    MeleeRadius = EnemyEV.HOMING_TURRET_EXPERT_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.HOMING_TURRET_EXPERT_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.HOMING_TURRET_EXPERT_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.HomingTurretExpertKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Advanced):
                    FireDelay = 1.5f;//5;
                    #region Advanced Variables - General
                    Name = EnemyEV.HOMING_TURRET_ADVANCED_NAME;
                    LocStringID = EnemyEV.HOMING_TURRET_ADVANCED_NAME_LOC_ID;

                    MaxHealth = EnemyEV.HOMING_TURRET_ADVANCED_MAX_HEALTH;
                    Damage = EnemyEV.HOMING_TURRET_ADVANCED_DAMAGE;
                    XPValue = EnemyEV.HOMING_TURRET_ADVANCED_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.HOMING_TURRET_ADVANCED_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.HOMING_TURRET_ADVANCED_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.HOMING_TURRET_ADVANCED_DROP_CHANCE;

                    Speed = EnemyEV.HOMING_TURRET_ADVANCED_SPEED;
                    TurnSpeed = EnemyEV.HOMING_TURRET_ADVANCED_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.HOMING_TURRET_ADVANCED_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.HOMING_TURRET_ADVANCED_JUMP;
                    CooldownTime = EnemyEV.HOMING_TURRET_ADVANCED_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.HOMING_TURRET_ADVANCED_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.HOMING_TURRET_ADVANCED_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.HOMING_TURRET_ADVANCED_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.HOMING_TURRET_ADVANCED_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.HOMING_TURRET_ADVANCED_IS_WEIGHTED;

                    Scale = EnemyEV.HomingTurretAdvancedScale;
                    ProjectileScale = EnemyEV.HomingTurretAdvancedProjectileScale;
                    TintablePart.TextureColor = EnemyEV.HomingTurretAdvancedTint;

                    MeleeRadius = EnemyEV.HOMING_TURRET_ADVANCED_MELEE_RADIUS;
                    EngageRadius = EnemyEV.HOMING_TURRET_ADVANCED_ENGAGE_RADIUS;
                    ProjectileRadius = EnemyEV.HOMING_TURRET_ADVANCED_PROJECTILE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.HomingTurretAdvancedKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Basic):
                default:
                    break;
            }							

        }

        protected override void InitializeLogic()
        {
            float angle = this.Rotation;
            float delay = this.ParseTagToFloat("delay");
            float speed = this.ParseTagToFloat("speed");
            if (delay == 0)
            {
                Console.WriteLine("ERROR: Turret set with delay of 0. Shoots too fast.");
                delay = FireDelay;
            }

            if (speed == 0)
                speed = ProjectileSpeed;

            ProjectileData projData = new ProjectileData(this)
            {
                SpriteName = "HomingProjectile_Sprite",
                SourceAnchor = new Vector2(35,0),
                //Target = m_target,
                Speed = new Vector2(speed, speed),
                IsWeighted = false,
                RotationSpeed = 0,
                Damage = Damage,
                AngleOffset = 0,
                CollidesWithTerrain = true,
                Scale = ProjectileScale,
                FollowArc = false,
                ChaseTarget = false,
                TurnSpeed = 0f,//0.02f,
                StartingRotation = 0,
                Lifespan = 10f,            
            };

            LogicSet walkStopLS = new LogicSet(this);
            walkStopLS.AddAction(new DelayLogicAction(0.5f));

            LogicSet fireProjectileLS = new LogicSet(this);
            fireProjectileLS.AddAction(new PlayAnimationLogicAction(false), Types.Sequence.Parallel);
            fireProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            fireProjectileLS.AddAction(new RunFunctionLogicAction(this, "FireProjectileEffect"));
            fireProjectileLS.AddAction(new Play3DSoundLogicAction(this, m_target, "Turret_Attack01", "Turret_Attack02", "Turret_Attack03"));
            fireProjectileLS.AddAction(new DelayLogicAction(delay));

            LogicSet fireProjectileAdvancedLS = new LogicSet(this);
            fireProjectileAdvancedLS.AddAction(new PlayAnimationLogicAction(false), Types.Sequence.Parallel);
            fireProjectileAdvancedLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            fireProjectileAdvancedLS.AddAction(new Play3DSoundLogicAction(this, m_target, "Turret_Attack01", "Turret_Attack02", "Turret_Attack03"));
            fireProjectileAdvancedLS.AddAction(new DelayLogicAction(0.1f));
            fireProjectileAdvancedLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            fireProjectileAdvancedLS.AddAction(new Play3DSoundLogicAction(this, m_target, "Turret_Attack01", "Turret_Attack02", "Turret_Attack03"));
            fireProjectileAdvancedLS.AddAction(new DelayLogicAction(0.1f));
            fireProjectileAdvancedLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            fireProjectileAdvancedLS.AddAction(new Play3DSoundLogicAction(this, m_target, "Turret_Attack01", "Turret_Attack02", "Turret_Attack03"));
            fireProjectileAdvancedLS.AddAction(new RunFunctionLogicAction(this, "FireProjectileEffect"));
            fireProjectileAdvancedLS.AddAction(new DelayLogicAction(delay));

            LogicSet fireProjectileExpertLS = new LogicSet(this);
            fireProjectileExpertLS.AddAction(new PlayAnimationLogicAction(false), Types.Sequence.Parallel);
            projData.ChaseTarget = true;
            projData.Target = m_target;
            projData.TurnSpeed = 0.02f;//0.065f;
            fireProjectileExpertLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            fireProjectileExpertLS.AddAction(new Play3DSoundLogicAction(this, m_target, "Turret_Attack01", "Turret_Attack02", "Turret_Attack03"));
            fireProjectileExpertLS.AddAction(new RunFunctionLogicAction(this, "FireProjectileEffect"));
            fireProjectileExpertLS.AddAction(new DelayLogicAction(delay));

            m_generalBasicLB.AddLogicSet(fireProjectileLS, walkStopLS);
            m_generalAdvancedLB.AddLogicSet(fireProjectileAdvancedLS, walkStopLS);
            m_generalExpertLB.AddLogicSet(fireProjectileExpertLS, walkStopLS);
            m_generalMiniBossLB.AddLogicSet(fireProjectileLS, walkStopLS);

            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalMiniBossLB);

            projData.Dispose();

            base.InitializeLogic();
        }

        public void FireProjectileEffect()
        {
            Vector2 pos = this.Position;
            if (this.Flip == Microsoft.Xna.Framework.Graphics.SpriteEffects.None)
                pos.X += 30;
            else
                pos.X -= 30;
            m_levelScreen.ImpactEffectPool.TurretFireEffect(pos, new Vector2(0.5f, 0.5f));
            m_levelScreen.ImpactEffectPool.TurretFireEffect(pos, new Vector2(0.5f, 0.5f));
            m_levelScreen.ImpactEffectPool.TurretFireEffect(pos, new Vector2(0.5f, 0.5f));
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case (STATE_MELEE_ENGAGE):
                case (STATE_PROJECTILE_ENGAGE):
                case (STATE_ENGAGE):
                    RunLogicBlock(false, m_generalBasicLB, 100, 0);
                    break;
                case (STATE_WANDER):
                default:
                    RunLogicBlock(false, m_generalBasicLB, 100, 0);
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
                    RunLogicBlock(false, m_generalAdvancedLB, 100, 0);
                    break;
                case (STATE_WANDER):
                default:
                    RunLogicBlock(false, m_generalAdvancedLB, 100, 0);
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
                    RunLogicBlock(false, m_generalExpertLB, 100, 0);
                    break;
                case (STATE_WANDER):
                default:
                    RunLogicBlock(false, m_generalExpertLB, 0, 100);
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
                    RunLogicBlock(false, m_generalBasicLB, 100, 0);
                    break;
                case (STATE_WANDER):
                default:
                    RunLogicBlock(false, m_generalBasicLB, 100, 0);
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public EnemyObj_HomingTurret(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("EnemyHomingTurret_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            this.StopAnimation();
            ForceDraw = true;
            this.Type = EnemyType.HOMING_TURRET;
            this.PlayAnimationOnRestart = false;
        }
    }
}
