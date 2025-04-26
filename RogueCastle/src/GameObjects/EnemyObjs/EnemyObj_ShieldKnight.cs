using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.EnvironmentVariables;
using RogueCastle.GameStructs;
using RogueCastle.LogicActions;
using RogueCastle.Screens;

namespace RogueCastle
{
    public class EnemyObj_ShieldKnight : EnemyObj
    {
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();

        private Vector2 ShieldKnockback = new Vector2(900, 1050);
        private float m_blockDmgReduction = 0.6f;//0.8f;//0.9f;
        private FrameSoundObj m_walkSound, m_walkSound2;

        protected override void InitializeEV()
        {
            LockFlip = true;

            #region Basic Variables - General
            Name = EnemyEV.SHIELD_KNIGHT_BASIC_NAME;
            LocStringID = EnemyEV.SHIELD_KNIGHT_BASIC_NAME_LOC_ID;

            MaxHealth = EnemyEV.SHIELD_KNIGHT_BASIC_MAX_HEALTH;
            Damage = EnemyEV.SHIELD_KNIGHT_BASIC_DAMAGE;
            XPValue = EnemyEV.SHIELD_KNIGHT_BASIC_XP_VALUE;

            MinMoneyDropAmount = EnemyEV.SHIELD_KNIGHT_BASIC_MIN_DROP_AMOUNT;
            MaxMoneyDropAmount = EnemyEV.SHIELD_KNIGHT_BASIC_MAX_DROP_AMOUNT;
            MoneyDropChance = EnemyEV.SHIELD_KNIGHT_BASIC_DROP_CHANCE;

            Speed = EnemyEV.SHIELD_KNIGHT_BASIC_SPEED;
            TurnSpeed = EnemyEV.SHIELD_KNIGHT_BASIC_TURN_SPEED;
            ProjectileSpeed = EnemyEV.SHIELD_KNIGHT_BASIC_PROJECTILE_SPEED;
            JumpHeight = EnemyEV.SHIELD_KNIGHT_BASIC_JUMP;
            CooldownTime = EnemyEV.SHIELD_KNIGHT_BASIC_COOLDOWN;
            AnimationDelay = 1 / EnemyEV.SHIELD_KNIGHT_BASIC_ANIMATION_DELAY;

            AlwaysFaceTarget = EnemyEV.SHIELD_KNIGHT_BASIC_ALWAYS_FACE_TARGET;
            CanFallOffLedges = EnemyEV.SHIELD_KNIGHT_BASIC_CAN_FALL_OFF_LEDGES;
            CanBeKnockedBack = EnemyEV.SHIELD_KNIGHT_BASIC_CAN_BE_KNOCKED_BACK;
            IsWeighted = EnemyEV.SHIELD_KNIGHT_BASIC_IS_WEIGHTED;

            Scale = EnemyEV.ShieldKnightBasicScale;
            ProjectileScale = EnemyEV.ShieldKnightBasicProjectileScale;
            TintablePart.TextureColor = EnemyEV.ShieldKnightBasicTint;

            MeleeRadius = EnemyEV.SHIELD_KNIGHT_BASIC_MELEE_RADIUS;
            ProjectileRadius = EnemyEV.SHIELD_KNIGHT_BASIC_PROJECTILE_RADIUS;
            EngageRadius = EnemyEV.SHIELD_KNIGHT_BASIC_ENGAGE_RADIUS;

            ProjectileDamage = Damage;
            KnockBack = EnemyEV.ShieldKnightBasicKnockBack;
            #endregion

            switch (Difficulty)
            {
                case (GameTypes.EnemyDifficulty.Miniboss):
                    ShieldKnockback = new Vector2(1200, 1350);

                    #region Miniboss Variables - General
                    Name = EnemyEV.SHIELD_KNIGHT_MINIBOSS_NAME;
                    LocStringID = EnemyEV.SHIELD_KNIGHT_MINIBOSS_NAME_LOC_ID;

                    MaxHealth = EnemyEV.SHIELD_KNIGHT_MINIBOSS_MAX_HEALTH;
                    Damage = EnemyEV.SHIELD_KNIGHT_MINIBOSS_DAMAGE;
                    XPValue = EnemyEV.SHIELD_KNIGHT_MINIBOSS_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.SHIELD_KNIGHT_MINIBOSS_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.SHIELD_KNIGHT_MINIBOSS_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.SHIELD_KNIGHT_MINIBOSS_DROP_CHANCE;

                    Speed = EnemyEV.SHIELD_KNIGHT_MINIBOSS_SPEED;
                    TurnSpeed = EnemyEV.SHIELD_KNIGHT_MINIBOSS_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.SHIELD_KNIGHT_MINIBOSS_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.SHIELD_KNIGHT_MINIBOSS_JUMP;
                    CooldownTime = EnemyEV.SHIELD_KNIGHT_MINIBOSS_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.SHIELD_KNIGHT_MINIBOSS_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.SHIELD_KNIGHT_MINIBOSS_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.SHIELD_KNIGHT_MINIBOSS_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.SHIELD_KNIGHT_MINIBOSS_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.SHIELD_KNIGHT_MINIBOSS_IS_WEIGHTED;

                    Scale = EnemyEV.ShieldKnightMinibossScale;
                    ProjectileScale = EnemyEV.ShieldKnightMinibossProjectileScale;
                    TintablePart.TextureColor = EnemyEV.ShieldKnightMinibossTint;

                    MeleeRadius = EnemyEV.SHIELD_KNIGHT_MINIBOSS_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.SHIELD_KNIGHT_MINIBOSS_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.SHIELD_KNIGHT_MINIBOSS_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.ShieldKnightMinibossKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Expert):
                    ShieldKnockback = new Vector2(1550, 1650);

                    #region Expert Variables - General
                    Name = EnemyEV.SHIELD_KNIGHT_EXPERT_NAME;
                    LocStringID = EnemyEV.SHIELD_KNIGHT_EXPERT_NAME_LOC_ID;

                    MaxHealth = EnemyEV.SHIELD_KNIGHT_EXPERT_MAX_HEALTH;
                    Damage = EnemyEV.SHIELD_KNIGHT_EXPERT_DAMAGE;
                    XPValue = EnemyEV.SHIELD_KNIGHT_EXPERT_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.SHIELD_KNIGHT_EXPERT_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.SHIELD_KNIGHT_EXPERT_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.SHIELD_KNIGHT_EXPERT_DROP_CHANCE;

                    Speed = EnemyEV.SHIELD_KNIGHT_EXPERT_SPEED;
                    TurnSpeed = EnemyEV.SHIELD_KNIGHT_EXPERT_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.SHIELD_KNIGHT_EXPERT_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.SHIELD_KNIGHT_EXPERT_JUMP;
                    CooldownTime = EnemyEV.SHIELD_KNIGHT_EXPERT_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.SHIELD_KNIGHT_EXPERT_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.SHIELD_KNIGHT_EXPERT_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.SHIELD_KNIGHT_EXPERT_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.SHIELD_KNIGHT_EXPERT_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.SHIELD_KNIGHT_EXPERT_IS_WEIGHTED;

                    Scale = EnemyEV.ShieldKnightExpertScale;
                    ProjectileScale = EnemyEV.ShieldKnightExpertProjectileScale;
                    TintablePart.TextureColor = EnemyEV.ShieldKnightExpertTint;

                    MeleeRadius = EnemyEV.SHIELD_KNIGHT_EXPERT_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.SHIELD_KNIGHT_EXPERT_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.SHIELD_KNIGHT_EXPERT_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.ShieldKnightExpertKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Advanced):
                    ShieldKnockback = new Vector2(1050, 1150);

                    #region Advanced Variables - General
                    Name = EnemyEV.SHIELD_KNIGHT_ADVANCED_NAME;
                    LocStringID = EnemyEV.SHIELD_KNIGHT_ADVANCED_NAME_LOC_ID;

                    MaxHealth = EnemyEV.SHIELD_KNIGHT_ADVANCED_MAX_HEALTH;
                    Damage = EnemyEV.SHIELD_KNIGHT_ADVANCED_DAMAGE;
                    XPValue = EnemyEV.SHIELD_KNIGHT_ADVANCED_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.SHIELD_KNIGHT_ADVANCED_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.SHIELD_KNIGHT_ADVANCED_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.SHIELD_KNIGHT_ADVANCED_DROP_CHANCE;

                    Speed = EnemyEV.SHIELD_KNIGHT_ADVANCED_SPEED;
                    TurnSpeed = EnemyEV.SHIELD_KNIGHT_ADVANCED_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.SHIELD_KNIGHT_ADVANCED_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.SHIELD_KNIGHT_ADVANCED_JUMP;
                    CooldownTime = EnemyEV.SHIELD_KNIGHT_ADVANCED_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.SHIELD_KNIGHT_ADVANCED_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.SHIELD_KNIGHT_ADVANCED_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.SHIELD_KNIGHT_ADVANCED_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.SHIELD_KNIGHT_ADVANCED_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.SHIELD_KNIGHT_ADVANCED_IS_WEIGHTED;

                    Scale = EnemyEV.ShieldKnightAdvancedScale;
                    ProjectileScale = EnemyEV.ShieldKnightAdvancedProjectileScale;
                    TintablePart.TextureColor = EnemyEV.ShieldKnightAdvancedTint;

                    MeleeRadius = EnemyEV.SHIELD_KNIGHT_ADVANCED_MELEE_RADIUS;
                    EngageRadius = EnemyEV.SHIELD_KNIGHT_ADVANCED_ENGAGE_RADIUS;
                    ProjectileRadius = EnemyEV.SHIELD_KNIGHT_ADVANCED_PROJECTILE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.ShieldKnightAdvancedKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Basic):
                default:
                    break;
            }		


        }

        protected override void InitializeLogic()
        {
            LogicSet walkStopLS = new LogicSet(this);  //Face direction locked, so this only makes him stop moving.
            walkStopLS.AddAction(new LockFaceDirectionLogicAction(true));
            walkStopLS.AddAction(new ChangeSpriteLogicAction("EnemyShieldKnightIdle_Character", true, true));
            //walkStopLS.AddAction(new MoveLogicAction(m_target, true, 0));
            walkStopLS.AddAction(new MoveDirectionLogicAction(0));
            walkStopLS.AddAction(new DelayLogicAction(0.5f, 2.0f));

            LogicSet walkTowardsLS = new LogicSet(this);
            walkTowardsLS.AddAction(new LockFaceDirectionLogicAction(true));
            walkTowardsLS.AddAction(new ChangeSpriteLogicAction("EnemyShieldKnightWalk_Character", true, true));
            //walkTowardsLS.AddAction(new MoveLogicAction(m_target, true));
            walkTowardsLS.AddAction(new MoveDirectionLogicAction());
            walkTowardsLS.AddAction(new DelayLogicAction(0.5f, 2.0f));

            LogicSet turnLS = new LogicSet(this);
            turnLS.AddAction(new LockFaceDirectionLogicAction(true));
            turnLS.AddAction(new MoveDirectionLogicAction(0));
            turnLS.AddAction(new ChangeSpriteLogicAction("EnemyShieldKnightTurnIn_Character", false, false));
            turnLS.AddAction(new PlayAnimationLogicAction(1, 2));
            turnLS.AddAction(new Play3DSoundLogicAction(this,  Game.ScreenManager.Player,"ShieldKnight_Turn"));
            turnLS.AddAction(new PlayAnimationLogicAction(3, this.TotalFrames));
            turnLS.AddAction(new LockFaceDirectionLogicAction(false));
            turnLS.AddAction(new MoveLogicAction(m_target, true, 0));
            turnLS.AddAction(new LockFaceDirectionLogicAction(true));
            turnLS.AddAction(new ChangeSpriteLogicAction("EnemyShieldKnightTurnOut_Character", true, false));
            turnLS.AddAction(new MoveDirectionLogicAction());


            LogicSet turnExpertLS = new LogicSet(this);
            turnExpertLS.AddAction(new LockFaceDirectionLogicAction(true));
            turnExpertLS.AddAction(new MoveDirectionLogicAction(0));
            turnExpertLS.AddAction(new ChangeSpriteLogicAction("EnemyShieldKnightTurnIn_Character", false, false));
            turnExpertLS.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 1 / 20f));
            turnExpertLS.AddAction(new PlayAnimationLogicAction(1, 2));
            turnExpertLS.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "ShieldKnight_Turn"));
            turnExpertLS.AddAction(new PlayAnimationLogicAction(3, this.TotalFrames));
            turnExpertLS.AddAction(new LockFaceDirectionLogicAction(false));
            turnExpertLS.AddAction(new MoveLogicAction(m_target, true, 0));
            turnExpertLS.AddAction(new LockFaceDirectionLogicAction(true));
            turnExpertLS.AddAction(new ChangeSpriteLogicAction("EnemyShieldKnightTurnOut_Character", true, false));
            turnExpertLS.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 1 / EnemyEV.SHIELD_KNIGHT_EXPERT_ANIMATION_DELAY));
            turnExpertLS.AddAction(new MoveDirectionLogicAction());

            m_generalBasicLB.AddLogicSet(walkStopLS, walkTowardsLS, turnLS);
            m_generalExpertLB.AddLogicSet(walkStopLS, walkTowardsLS, turnExpertLS);

            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalExpertLB);

            SetCooldownLogicBlock(m_generalBasicLB, 100); 

            base.InitializeLogic();

        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case (STATE_MELEE_ENGAGE):
                case (STATE_PROJECTILE_ENGAGE):
                case (STATE_ENGAGE):
                    if ((m_target.X > this.X && this.HeadingX < 0) || (m_target.X < this.X && this.HeadingX >= 0))
                        RunLogicBlock(true, m_generalBasicLB, 0, 0, 100); // walkStopLS, walkTowardsLS, turnLS
                    else
                        RunLogicBlock(true, m_generalBasicLB, 0, 100, 0); // walkStopLS, walkTowardsLS, turnLS
                    break;
                case (STATE_WANDER):
                    if ((m_target.X > this.X && this.HeadingX < 0) || (m_target.X < this.X && this.HeadingX >= 0))
                        RunLogicBlock(true, m_generalBasicLB, 0, 0, 100); // walkStopLS, walkTowardsLS, turnLS
                    else
                        RunLogicBlock(true, m_generalBasicLB, 100, 0, 0); // walkStopLS, walkTowardsLS, turnLS
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
                    if ((m_target.X > this.X && this.HeadingX < 0) || (m_target.X < this.X && this.HeadingX >= 0))
                        RunLogicBlock(true, m_generalBasicLB, 0, 0, 100); // walkStopLS, walkTowardsLS, turnLS
                    else
                        RunLogicBlock(true, m_generalBasicLB, 0, 100, 0); // walkStopLS, walkTowardsLS, turnLS
                    break;
                case (STATE_WANDER):
                    if ((m_target.X > this.X && this.HeadingX < 0) || (m_target.X < this.X && this.HeadingX >= 0))
                        RunLogicBlock(true, m_generalBasicLB, 0, 0, 100); // walkStopLS, walkTowardsLS, turnLS
                    else
                        RunLogicBlock(true, m_generalBasicLB, 100, 0, 0); // walkStopLS, walkTowardsLS, turnLS
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
                    if ((m_target.X > this.X && this.HeadingX < 0) || (m_target.X < this.X && this.HeadingX >= 0))
                        RunLogicBlock(true, m_generalExpertLB, 0, 0, 100); // walkStopLS, walkTowardsLS, turnLS
                    else
                        RunLogicBlock(true, m_generalExpertLB, 0, 100, 0); // walkStopLS, walkTowardsLS, turnLS
                    break;
                case (STATE_WANDER):
                    if ((m_target.X > this.X && this.HeadingX < 0) || (m_target.X < this.X && this.HeadingX >= 0))
                        RunLogicBlock(true, m_generalExpertLB, 0, 0, 100); // walkStopLS, walkTowardsLS, turnLS
                    else
                        RunLogicBlock(true, m_generalExpertLB, 100, 0, 0); // walkStopLS, walkTowardsLS, turnLS
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
                    if ((m_target.X > this.X && this.HeadingX < 0) || (m_target.X < this.X && this.HeadingX >= 0))
                        RunLogicBlock(true, m_generalBasicLB, 100, 0, 0); // walkStopLS, walkTowardsLS, turnLS
                    else
                        RunLogicBlock(true, m_generalBasicLB, 100, 0, 0); // walkStopLS, walkTowardsLS, turnLS
                    break;
                case (STATE_WANDER):
                    if ((m_target.X > this.X && this.HeadingX < 0) || (m_target.X < this.X && this.HeadingX >= 0))
                        RunLogicBlock(true, m_generalBasicLB, 100, 0, 0); // walkStopLS, walkTowardsLS, turnLS
                    else
                        RunLogicBlock(true, m_generalBasicLB, 100, 0, 0); // walkStopLS, walkTowardsLS, turnLS
                    break;
                default:
                    break;
            }
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
        {
            PlayerObj player = otherBox.AbsParent as PlayerObj;
            ProjectileObj proj = otherBox.AbsParent as ProjectileObj;

            //if ((collisionResponseType == Consts.COLLISIONRESPONSE_FIRSTBOXHIT) && ((otherBox.AbsParent is ProjectileObj == false && m_invincibleCounter <= 0) || (otherBox.AbsParent is ProjectileObj && m_invincibleCounterProjectile <= 0) &&
            //    ((this.Flip == SpriteEffects.None && otherBox.AbsParent.AbsPosition.X > this.X) || (this.Flip == SpriteEffects.FlipHorizontally && otherBox.AbsParent.AbsPosition.X < this.X)) && (player != null && player.IsAirAttacking == false)))

            // Enemy blocked.
            if (collisionResponseType == Consts.COLLISIONRESPONSE_FIRSTBOXHIT &&
                ((player != null && m_invincibleCounter <= 0) || (proj != null && m_invincibleCounterProjectile <= 0)) &&
                ((this.Flip == SpriteEffects.None && otherBox.AbsParent.AbsPosition.X > this.X) || (this.Flip == SpriteEffects.FlipHorizontally && otherBox.AbsParent.AbsPosition.X < this.X)) &&
                (player != null && player.SpriteName != "PlayerAirAttack_Character") //player.IsAirAttacking == false)
                )
            {
                if (CanBeKnockedBack == true)
                {
                    CurrentSpeed = 0;
                    m_currentActiveLB.StopLogicBlock();
                    RunLogicBlock(true, m_generalBasicLB, 100, 0, 0);
                    //if (otherBox.AbsParent.Bounds.Left + otherBox.AbsParent.Bounds.Width / 2 > this.X)
                    //    AccelerationX = -KnockBack.X;
                    //else
                    //    AccelerationX = KnockBack.X;
                    //AccelerationY = -KnockBack.Y;
                }


                if (m_target.IsAirAttacking == true)
                {
                    m_target.IsAirAttacking = false; // Only allow one object to perform upwards air knockback on the player.
                    m_target.AccelerationY = -m_target.AirAttackKnockBack;
                    m_target.NumAirBounces++;
                }
                else
                {
                    if (m_target.Bounds.Left + m_target.Bounds.Width / 2 < this.X)
                        m_target.AccelerationX = -ShieldKnockback.X;
                    else
                        m_target.AccelerationX = ShieldKnockback.X;
                    m_target.AccelerationY = -ShieldKnockback.Y;
                    //if (m_target.Bounds.Left + m_target.Bounds.Width / 2 < this.X)
                    //    m_target.AccelerationX = -m_target.EnemyKnockBack.X;
                    //else
                    //    m_target.AccelerationX = m_target.EnemyKnockBack.X;
                    //m_target.AccelerationY = -m_target.EnemyKnockBack.Y;
                }

                // This must be called before the invincible counter is set.
                base.CollisionResponse(thisBox, otherBox, collisionResponseType);

                Point intersectPt = Rectangle.Intersect(thisBox.AbsRect, otherBox.AbsRect).Center;
                Vector2 impactPosition = new Vector2(intersectPt.X, intersectPt.Y);
                m_levelScreen.ImpactEffectPool.DisplayBlockImpactEffect(impactPosition, new Vector2(2,2));
                SoundManager.Play3DSound(this, Game.ScreenManager.Player,"ShieldKnight_Block01", "ShieldKnight_Block02", "ShieldKnight_Block03");
                m_invincibleCounter = InvincibilityTime;
                m_levelScreen.SetLastEnemyHit(this);
                Blink(Color.LightBlue, 0.1f);

                ProjectileObj projectile = otherBox.AbsParent as ProjectileObj;
                if (projectile != null)
                {
                    m_invincibleCounterProjectile = InvincibilityTime;
                    m_levelScreen.ProjectileManager.DestroyProjectile(projectile);
                }
            }
            else
                base.CollisionResponse(thisBox, otherBox, collisionResponseType);
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
        {
            if (m_target != null && m_target.CurrentHealth > 0)
            {
                SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Knight_Hit01", "Knight_Hit02", "Knight_Hit03");
                if ((this.Flip == SpriteEffects.None && m_target.X > this.X) || (this.Flip == SpriteEffects.FlipHorizontally && m_target.X < this.X))
                {
                    // Air attacks and all other damage other than sword swipes should deal their full damage.
                    if (m_target.SpriteName != "PlayerAirAttack_Character")
                        damage = (int)(damage * (1 - m_blockDmgReduction));
                }
            }
            base.HitEnemy(damage, position, isPlayer);
        }

        public override void Update(GameTime gameTime)
        {
            if (this.SpriteName == "EnemyShieldKnightWalk_Character")
            {
                m_walkSound.Update();
                m_walkSound2.Update();
            }
            base.Update(gameTime);
        }

        public EnemyObj_ShieldKnight(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("EnemyShieldKnightIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            this.Type = EnemyType.SHIELD_KNIGHT;
            m_walkSound = new FrameSoundObj(this, m_target, 1, "KnightWalk1", "KnightWalk2");
            m_walkSound2 = new FrameSoundObj(this, m_target, 6, "KnightWalk1", "KnightWalk2");
        }

        public override void Dispose()
        {
            if (IsDisposed == false)
            {
                m_walkSound.Dispose();
                m_walkSound = null;
                m_walkSound2.Dispose();
                m_walkSound2 = null;
                base.Dispose();
            }
        }
    }
}
