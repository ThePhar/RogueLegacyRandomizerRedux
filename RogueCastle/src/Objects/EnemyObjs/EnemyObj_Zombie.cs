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
    public class EnemyObj_Zombie : EnemyObj
    {
        private LogicBlock m_basicWalkLS = new LogicBlock();
        private LogicBlock m_basicRiseLowerLS = new LogicBlock();

        public bool Risen { get; set; }
        public bool Lowered { get; set; }

        protected override void InitializeEV()
        {
            #region Basic Variables - General
            Name = EnemyEV.ZOMBIE_BASIC_NAME;
            LocStringID = EnemyEV.ZOMBIE_BASIC_NAME_LOC_ID;

            MaxHealth = EnemyEV.ZOMBIE_BASIC_MAX_HEALTH;
            Damage = EnemyEV.ZOMBIE_BASIC_DAMAGE;
            XPValue = EnemyEV.ZOMBIE_BASIC_XP_VALUE;

            MinMoneyDropAmount = EnemyEV.ZOMBIE_BASIC_MIN_DROP_AMOUNT;
            MaxMoneyDropAmount = EnemyEV.ZOMBIE_BASIC_MAX_DROP_AMOUNT;
            MoneyDropChance = EnemyEV.ZOMBIE_BASIC_DROP_CHANCE;

            Speed = EnemyEV.ZOMBIE_BASIC_SPEED;
            TurnSpeed = EnemyEV.ZOMBIE_BASIC_TURN_SPEED;
            ProjectileSpeed = EnemyEV.ZOMBIE_BASIC_PROJECTILE_SPEED;
            JumpHeight = EnemyEV.ZOMBIE_BASIC_JUMP;
            CooldownTime = EnemyEV.ZOMBIE_BASIC_COOLDOWN;
            AnimationDelay = 1 / EnemyEV.ZOMBIE_BASIC_ANIMATION_DELAY;

            AlwaysFaceTarget = EnemyEV.ZOMBIE_BASIC_ALWAYS_FACE_TARGET;
            CanFallOffLedges = EnemyEV.ZOMBIE_BASIC_CAN_FALL_OFF_LEDGES;
            CanBeKnockedBack = EnemyEV.ZOMBIE_BASIC_CAN_BE_KNOCKED_BACK;
            IsWeighted = EnemyEV.ZOMBIE_BASIC_IS_WEIGHTED;

            Scale = EnemyEV.ZombieBasicScale;
            ProjectileScale = EnemyEV.ZombieBasicProjectileScale;
            TintablePart.TextureColor = EnemyEV.ZombieBasicTint;

            MeleeRadius = EnemyEV.ZOMBIE_BASIC_MELEE_RADIUS;
            ProjectileRadius = EnemyEV.ZOMBIE_BASIC_PROJECTILE_RADIUS;
            EngageRadius = EnemyEV.ZOMBIE_BASIC_ENGAGE_RADIUS;

            ProjectileDamage = Damage;
            KnockBack = EnemyEV.ZombieBasicKnockBack;
            #endregion

            switch (Difficulty)
            {
                case (GameTypes.EnemyDifficulty.MINIBOSS):

                    #region Miniboss Variables - General
                    Name = EnemyEV.ZOMBIE_MINIBOSS_NAME;
                    LocStringID = EnemyEV.ZOMBIE_MINIBOSS_NAME_LOC_ID;

                    MaxHealth = EnemyEV.ZOMBIE_MINIBOSS_MAX_HEALTH;
                    Damage = EnemyEV.ZOMBIE_MINIBOSS_DAMAGE;
                    XPValue = EnemyEV.ZOMBIE_MINIBOSS_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.ZOMBIE_MINIBOSS_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.ZOMBIE_MINIBOSS_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.ZOMBIE_MINIBOSS_DROP_CHANCE;

                    Speed = EnemyEV.ZOMBIE_MINIBOSS_SPEED;
                    TurnSpeed = EnemyEV.ZOMBIE_MINIBOSS_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.ZOMBIE_MINIBOSS_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.ZOMBIE_MINIBOSS_JUMP;
                    CooldownTime = EnemyEV.ZOMBIE_MINIBOSS_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.ZOMBIE_MINIBOSS_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.ZOMBIE_MINIBOSS_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.ZOMBIE_MINIBOSS_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.ZOMBIE_MINIBOSS_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.ZOMBIE_MINIBOSS_IS_WEIGHTED;

                    Scale = EnemyEV.ZombieMinibossScale;
                    ProjectileScale = EnemyEV.ZombieMinibossProjectileScale;
                    TintablePart.TextureColor = EnemyEV.ZombieMinibossTint;

                    MeleeRadius = EnemyEV.ZOMBIE_MINIBOSS_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.ZOMBIE_MINIBOSS_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.ZOMBIE_MINIBOSS_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.ZombieMinibossKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.EXPERT):

                    #region Expert Variables - General
                    Name = EnemyEV.ZOMBIE_EXPERT_NAME;
                    LocStringID = EnemyEV.ZOMBIE_EXPERT_NAME_LOC_ID;

                    MaxHealth = EnemyEV.ZOMBIE_EXPERT_MAX_HEALTH;
                    Damage = EnemyEV.ZOMBIE_EXPERT_DAMAGE;
                    XPValue = EnemyEV.ZOMBIE_EXPERT_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.ZOMBIE_EXPERT_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.ZOMBIE_EXPERT_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.ZOMBIE_EXPERT_DROP_CHANCE;

                    Speed = EnemyEV.ZOMBIE_EXPERT_SPEED;
                    TurnSpeed = EnemyEV.ZOMBIE_EXPERT_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.ZOMBIE_EXPERT_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.ZOMBIE_EXPERT_JUMP;
                    CooldownTime = EnemyEV.ZOMBIE_EXPERT_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.ZOMBIE_EXPERT_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.ZOMBIE_EXPERT_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.ZOMBIE_EXPERT_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.ZOMBIE_EXPERT_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.ZOMBIE_EXPERT_IS_WEIGHTED;

                    Scale = EnemyEV.ZombieExpertScale;
                    ProjectileScale = EnemyEV.ZombieExpertProjectileScale;
                    TintablePart.TextureColor = EnemyEV.ZombieExpertTint;

                    MeleeRadius = EnemyEV.ZOMBIE_EXPERT_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.ZOMBIE_EXPERT_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.ZOMBIE_EXPERT_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.ZombieExpertKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.ADVANCED):

                    #region Advanced Variables - General
                    Name = EnemyEV.ZOMBIE_ADVANCED_NAME;
                    LocStringID = EnemyEV.ZOMBIE_ADVANCED_NAME_LOC_ID;

                    MaxHealth = EnemyEV.ZOMBIE_ADVANCED_MAX_HEALTH;
                    Damage = EnemyEV.ZOMBIE_ADVANCED_DAMAGE;
                    XPValue = EnemyEV.ZOMBIE_ADVANCED_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.ZOMBIE_ADVANCED_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.ZOMBIE_ADVANCED_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.ZOMBIE_ADVANCED_DROP_CHANCE;

                    Speed = EnemyEV.ZOMBIE_ADVANCED_SPEED;
                    TurnSpeed = EnemyEV.ZOMBIE_ADVANCED_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.ZOMBIE_ADVANCED_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.ZOMBIE_ADVANCED_JUMP;
                    CooldownTime = EnemyEV.ZOMBIE_ADVANCED_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.ZOMBIE_ADVANCED_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.ZOMBIE_ADVANCED_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.ZOMBIE_ADVANCED_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.ZOMBIE_ADVANCED_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.ZOMBIE_ADVANCED_IS_WEIGHTED;

                    Scale = EnemyEV.ZombieAdvancedScale;
                    ProjectileScale = EnemyEV.ZombieAdvancedProjectileScale;
                    TintablePart.TextureColor = EnemyEV.ZombieAdvancedTint;

                    MeleeRadius = EnemyEV.ZOMBIE_ADVANCED_MELEE_RADIUS;
                    EngageRadius = EnemyEV.ZOMBIE_ADVANCED_ENGAGE_RADIUS;
                    ProjectileRadius = EnemyEV.ZOMBIE_ADVANCED_PROJECTILE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.ZombieAdvancedKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.BASIC):
                default:
                    break;
            }		
        }

        protected override void InitializeLogic()
        {
            LogicSet walkTowardsLS = new LogicSet(this);
            walkTowardsLS.AddAction(new LockFaceDirectionLogicAction(false));
            walkTowardsLS.AddAction(new ChangeSpriteLogicAction("EnemyZombieWalk_Character", true, true));
            walkTowardsLS.AddAction(new MoveLogicAction(m_target, true));
            walkTowardsLS.AddAction(new LockFaceDirectionLogicAction(true));
            walkTowardsLS.AddAction(new Play3DSoundLogicAction(this,  Game.ScreenManager.Player,"Zombie_Groan_01", "Zombie_Groan_02", "Zombie_Groan_03", "Blank", "Blank", "Blank", "Blank", "Blank"));
            walkTowardsLS.AddAction(new DelayLogicAction(0.5f));

            LogicSet riseLS = new LogicSet(this);
            riseLS.AddAction(new LockFaceDirectionLogicAction(false));
            riseLS.AddAction(new MoveLogicAction(m_target, false, 0));
            //riseLS.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 1 /  m_Rise_Animation_Speed));
            riseLS.AddAction(new ChangeSpriteLogicAction("EnemyZombieRise_Character", false, false));
            riseLS.AddAction(new Play3DSoundLogicAction(this,  Game.ScreenManager.Player,"Zombie_Rise"));
            riseLS.AddAction(new PlayAnimationLogicAction(false));
            //riseLS.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 1 / 14));
            riseLS.AddAction(new ChangePropertyLogicAction(this, "Risen", true));
            riseLS.AddAction(new ChangePropertyLogicAction(this, "Lowered", false));

            LogicSet lowerLS = new LogicSet(this);
            lowerLS.AddAction(new LockFaceDirectionLogicAction(false));
            lowerLS.AddAction(new MoveLogicAction(m_target, false, 0));
            lowerLS.AddAction(new ChangeSpriteLogicAction("EnemyZombieLower_Character", false, false));
            lowerLS.AddAction(new Play3DSoundLogicAction(this,  Game.ScreenManager.Player,"Zombie_Lower"));
            lowerLS.AddAction(new PlayAnimationLogicAction(false));            
            lowerLS.AddAction(new ChangePropertyLogicAction(this, "Risen", false));
            lowerLS.AddAction(new ChangePropertyLogicAction(this, "Lowered", true));

            m_basicWalkLS.AddLogicSet(walkTowardsLS);
            m_basicRiseLowerLS.AddLogicSet(riseLS, lowerLS);

            logicBlocksToDispose.Add(m_basicWalkLS);
            logicBlocksToDispose.Add(m_basicRiseLowerLS);

            base.InitializeLogic();
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case (STATE_MELEE_ENGAGE):
                case (STATE_PROJECTILE_ENGAGE):
                case (STATE_ENGAGE):
                        if (this.Risen == false)
                            RunLogicBlock(false, m_basicRiseLowerLS, 100, 0);
                        else
                            RunLogicBlock(false, m_basicWalkLS, 100); // walkTowardsLS
                    //if (Math.Abs(this.Y - m_target.Y) < 100) // Ensures the zombie doesn't appear if the player is above or below him.
                    //{
                    //    if (this.Risen == false)
                    //        RunLogicBlock(false, m_basicRiseLowerLS, 100, 0);
                    //    else
                    //        RunLogicBlock(false, m_basicWalkLS, 100); // walkTowardsLS
                    //}
                    //else if (m_target.IsTouchingGround == true)
                    //{
                    //    if (this.Lowered == false)
                    //        RunLogicBlock(false, m_basicRiseLowerLS, 0, 100); // riseLS, lowerLS
                    //}
                    break;
                case (STATE_WANDER):
                    if (this.Lowered == false)
                        RunLogicBlock(false, m_basicRiseLowerLS, 0, 100); // riseLS, lowerLS
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
                case (STATE_ENGAGE):
                    if (this.Risen == false)
                        RunLogicBlock(false, m_basicRiseLowerLS, 100, 0);
                    else
                        RunLogicBlock(false, m_basicWalkLS, 100); // walkTowardsLS
                    //if (Math.Abs(this.Y - m_target.Y) < 100) // Ensures the zombie doesn't appear if the player is above or below him.
                    //{
                    //    if (this.Risen == false)
                    //        RunLogicBlock(false, m_basicRiseLowerLS, 100, 0);
                    //    else
                    //        RunLogicBlock(false, m_basicWalkLS, 100); // walkTowardsLS
                    //}
                    //else if (m_target.IsTouchingGround == true)
                    //{
                    //    if (this.Lowered == false)
                    //        RunLogicBlock(false, m_basicRiseLowerLS, 0, 100); // riseLS, lowerLS
                    //}
                    break;
                case (STATE_WANDER):
                    if (this.Lowered == false)
                        RunLogicBlock(false, m_basicRiseLowerLS, 0, 100); // riseLS, lowerLS
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
                case (STATE_ENGAGE):
                    if (this.Risen == false)
                        RunLogicBlock(false, m_basicRiseLowerLS, 100, 0);
                    else
                        RunLogicBlock(false, m_basicWalkLS, 100); // walkTowardsLS
                    //if (Math.Abs(this.Y - m_target.Y) < 100) // Ensures the zombie doesn't appear if the player is above or below him.
                    //{
                    //    if (this.Risen == false)
                    //        RunLogicBlock(false, m_basicRiseLowerLS, 100, 0);
                    //    else
                    //        RunLogicBlock(false, m_basicWalkLS, 100); // walkTowardsLS
                    //}
                    //else if (m_target.IsTouchingGround == true)
                    //{
                    //    if (this.Lowered == false)
                    //        RunLogicBlock(false, m_basicRiseLowerLS, 0, 100); // riseLS, lowerLS
                    //}
                    break;
                case (STATE_WANDER):
                    if (this.Lowered == false)
                        RunLogicBlock(false, m_basicRiseLowerLS, 0, 100); // riseLS, lowerLS
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
                case (STATE_ENGAGE):
                    if (this.Risen == false)
                        RunLogicBlock(false, m_basicRiseLowerLS, 100, 0);
                    else
                        RunLogicBlock(false, m_basicWalkLS, 100); // walkTowardsLS
                    //if (Math.Abs(this.Y - m_target.Y) < 100) // Ensures the zombie doesn't appear if the player is above or below him.
                    //{
                    //    if (this.Risen == false)
                    //        RunLogicBlock(false, m_basicRiseLowerLS, 100, 0);
                    //    else
                    //        RunLogicBlock(false, m_basicWalkLS, 100); // walkTowardsLS
                    //}
                    //else if (m_target.IsTouchingGround == true)
                    //{
                    //    if (this.Lowered == false)
                    //        RunLogicBlock(false, m_basicRiseLowerLS, 0, 100); // riseLS, lowerLS
                    //}
                    break;
                case (STATE_WANDER):
                    if (this.Lowered == false)
                        RunLogicBlock(false, m_basicRiseLowerLS, 0, 100); // riseLS, lowerLS
                    break;
                default:
                    break;
            }
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
        {
            SoundManager.Play3DSound(this, Game.ScreenManager.Player,"Zombie_Hit");
            base.HitEnemy(damage, position, isPlayer);
        }

        public override void Update(GameTime gameTime)
        {
            // Hack to stop zombies in the ground from animating. I don't know why zombies keep animating.
            if ((m_currentActiveLB == null || m_currentActiveLB.IsActive == false) && this.Risen == false && this.IsAnimating == true)
            {
                this.ChangeSprite("EnemyZombieRise_Character");
                this.StopAnimation();
            }

            base.Update(gameTime);
        }

        public override void ResetState()
        {
            Lowered = true;
            Risen = false;
            base.ResetState();

            this.ChangeSprite("EnemyZombieLower_Character");
            this.GoToFrame(this.TotalFrames);
            this.StopAnimation();
        }

        public override void Reset()
        {
            this.ChangeSprite("EnemyZombieRise_Character");
            this.StopAnimation();
            Lowered = true;
            Risen = false;
            base.Reset();
        }

        public EnemyObj_Zombie(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("EnemyZombieLower_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            this.GoToFrame(this.TotalFrames);
            Lowered = true;
            this.ForceDraw = true;
            this.StopAnimation();
            this.Type = EnemyType.Zombie;
            this.PlayAnimationOnRestart = false;
        }
    }
}
