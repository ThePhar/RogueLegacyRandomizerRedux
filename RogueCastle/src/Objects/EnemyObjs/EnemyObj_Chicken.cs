using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.EnvironmentVariables;
using RogueCastle.GameStructs;

namespace RogueCastle
{
    public class EnemyObj_Chicken : EnemyObj
    {
        private LogicBlock m_generalBasicLB = new LogicBlock();

        protected override void InitializeEV()
        {
            this.LockFlip = true;

            #region Basic Variables - General
            Name = EnemyEV.CHICKEN_BASIC_NAME;
            LocStringID = EnemyEV.CHICKEN_BASIC_NAME_LOC_ID;

            MaxHealth = EnemyEV.CHICKEN_BASIC_MAX_HEALTH;
            Damage = EnemyEV.CHICKEN_BASIC_DAMAGE;
            XPValue = EnemyEV.CHICKEN_BASIC_XP_VALUE;

            MinMoneyDropAmount = EnemyEV.CHICKEN_BASIC_MIN_DROP_AMOUNT;
            MaxMoneyDropAmount = EnemyEV.CHICKEN_BASIC_MAX_DROP_AMOUNT;
            MoneyDropChance = EnemyEV.CHICKEN_BASIC_DROP_CHANCE;

            Speed = EnemyEV.CHICKEN_BASIC_SPEED;
            TurnSpeed = EnemyEV.CHICKEN_BASIC_TURN_SPEED;
            ProjectileSpeed = EnemyEV.CHICKEN_BASIC_PROJECTILE_SPEED;
            JumpHeight = EnemyEV.CHICKEN_BASIC_JUMP;
            CooldownTime = EnemyEV.CHICKEN_BASIC_COOLDOWN;
            AnimationDelay = 1 / EnemyEV.CHICKEN_BASIC_ANIMATION_DELAY;

            AlwaysFaceTarget = EnemyEV.CHICKEN_BASIC_ALWAYS_FACE_TARGET;
            CanFallOffLedges = EnemyEV.CHICKEN_BASIC_CAN_FALL_OFF_LEDGES;
            CanBeKnockedBack = EnemyEV.CHICKEN_BASIC_CAN_BE_KNOCKED_BACK;
            IsWeighted = EnemyEV.CHICKEN_BASIC_IS_WEIGHTED;

            Scale = EnemyEV.ChickenBasicScale;
            ProjectileScale = EnemyEV.ChickenBasicProjectileScale;
            TintablePart.TextureColor = EnemyEV.ChickenBasicTint;

            MeleeRadius = EnemyEV.CHICKEN_BASIC_MELEE_RADIUS;
            ProjectileRadius = EnemyEV.CHICKEN_BASIC_PROJECTILE_RADIUS;
            EngageRadius = EnemyEV.CHICKEN_BASIC_ENGAGE_RADIUS;

            ProjectileDamage = Damage;
            KnockBack = EnemyEV.ChickenBasicKnockBack;
            #endregion

            switch (Difficulty)
            {
                case (GameTypes.EnemyDifficulty.MINIBOSS):
                    #region Miniboss Variables - General
                    Name = EnemyEV.CHICKEN_MINIBOSS_NAME;
                    LocStringID = EnemyEV.CHICKEN_MINIBOSS_NAME_LOC_ID;

                    MaxHealth = EnemyEV.CHICKEN_MINIBOSS_MAX_HEALTH;
                    Damage = EnemyEV.CHICKEN_MINIBOSS_DAMAGE;
                    XPValue = EnemyEV.CHICKEN_MINIBOSS_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.CHICKEN_MINIBOSS_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.CHICKEN_MINIBOSS_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.CHICKEN_MINIBOSS_DROP_CHANCE;

                    Speed = EnemyEV.CHICKEN_MINIBOSS_SPEED;
                    TurnSpeed = EnemyEV.CHICKEN_MINIBOSS_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.CHICKEN_MINIBOSS_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.CHICKEN_MINIBOSS_JUMP;
                    CooldownTime = EnemyEV.CHICKEN_MINIBOSS_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.CHICKEN_MINIBOSS_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.CHICKEN_MINIBOSS_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.CHICKEN_MINIBOSS_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.CHICKEN_MINIBOSS_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.CHICKEN_MINIBOSS_IS_WEIGHTED;

                    Scale = EnemyEV.ChickenMinibossScale;
                    ProjectileScale = EnemyEV.ChickenMinibossProjectileScale;
                    TintablePart.TextureColor = EnemyEV.ChickenMinibossTint;

                    MeleeRadius = EnemyEV.CHICKEN_MINIBOSS_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.CHICKEN_MINIBOSS_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.CHICKEN_MINIBOSS_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.ChickenMinibossKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.EXPERT):
                    #region Expert Variables - General
                    Name = EnemyEV.CHICKEN_EXPERT_NAME;
                    LocStringID = EnemyEV.CHICKEN_EXPERT_NAME_LOC_ID;

                    MaxHealth = EnemyEV.CHICKEN_EXPERT_MAX_HEALTH;
                    Damage = EnemyEV.CHICKEN_EXPERT_DAMAGE;
                    XPValue = EnemyEV.CHICKEN_EXPERT_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.CHICKEN_EXPERT_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.CHICKEN_EXPERT_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.CHICKEN_EXPERT_DROP_CHANCE;

                    Speed = EnemyEV.CHICKEN_EXPERT_SPEED;
                    TurnSpeed = EnemyEV.CHICKEN_EXPERT_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.CHICKEN_EXPERT_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.CHICKEN_EXPERT_JUMP;
                    CooldownTime = EnemyEV.CHICKEN_EXPERT_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.CHICKEN_EXPERT_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.CHICKEN_EXPERT_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.CHICKEN_EXPERT_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.CHICKEN_EXPERT_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.CHICKEN_EXPERT_IS_WEIGHTED;

                    Scale = EnemyEV.ChickenExpertScale;
                    ProjectileScale = EnemyEV.ChickenExpertProjectileScale;
                    TintablePart.TextureColor = EnemyEV.ChickenExpertTint;

                    MeleeRadius = EnemyEV.CHICKEN_EXPERT_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.CHICKEN_EXPERT_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.CHICKEN_EXPERT_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.ChickenExpertKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.ADVANCED):
                    #region Advanced Variables - General
                    Name = EnemyEV.CHICKEN_ADVANCED_NAME;
                    LocStringID = EnemyEV.CHICKEN_ADVANCED_NAME_LOC_ID;

                    MaxHealth = EnemyEV.CHICKEN_ADVANCED_MAX_HEALTH;
                    Damage = EnemyEV.CHICKEN_ADVANCED_DAMAGE;
                    XPValue = EnemyEV.CHICKEN_ADVANCED_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.CHICKEN_ADVANCED_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.CHICKEN_ADVANCED_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.CHICKEN_ADVANCED_DROP_CHANCE;

                    Speed = EnemyEV.CHICKEN_ADVANCED_SPEED;
                    TurnSpeed = EnemyEV.CHICKEN_ADVANCED_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.CHICKEN_ADVANCED_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.CHICKEN_ADVANCED_JUMP;
                    CooldownTime = EnemyEV.CHICKEN_ADVANCED_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.CHICKEN_ADVANCED_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.CHICKEN_ADVANCED_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.CHICKEN_ADVANCED_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.CHICKEN_ADVANCED_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.CHICKEN_ADVANCED_IS_WEIGHTED;

                    Scale = EnemyEV.ChickenAdvancedScale;
                    ProjectileScale = EnemyEV.ChickenAdvancedProjectileScale;
                    TintablePart.TextureColor = EnemyEV.ChickenAdvancedTint;

                    MeleeRadius = EnemyEV.CHICKEN_ADVANCED_MELEE_RADIUS;
                    EngageRadius = EnemyEV.CHICKEN_ADVANCED_ENGAGE_RADIUS;
                    ProjectileRadius = EnemyEV.CHICKEN_ADVANCED_PROJECTILE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.ChickenAdvancedKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.BASIC):
                default:
                    this.Scale = new Vector2(2, 2);
                    break;
            }
         
            IsWeighted = true;
        }

        protected override void InitializeLogic()
        {
            LogicSet walkLeftLS = new LogicSet(this);
            walkLeftLS.AddAction(new ChangeSpriteLogicAction("EnemyChickenRun_Character", true, true));
            walkLeftLS.AddAction(new ChangePropertyLogicAction(this, "Flip", Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally));
            walkLeftLS.AddAction(new MoveDirectionLogicAction());
            walkLeftLS.AddAction(new DelayLogicAction(0.5f, 1.0f));

            LogicSet walkRightLS = new LogicSet(this);
            walkRightLS.AddAction(new ChangeSpriteLogicAction("EnemyChickenRun_Character", true, true));
            walkRightLS.AddAction(new ChangePropertyLogicAction(this, "Flip", Microsoft.Xna.Framework.Graphics.SpriteEffects.None));
            walkRightLS.AddAction(new MoveDirectionLogicAction());
            walkRightLS.AddAction(new DelayLogicAction(0.5f, 1.0f));

            m_generalBasicLB.AddLogicSet(walkLeftLS, walkRightLS);

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
                        RunLogicBlock(true, m_generalBasicLB, 50, 50);
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
                        RunLogicBlock(true, m_generalBasicLB, 50, 50);
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
                        RunLogicBlock(true, m_generalBasicLB, 50, 50);
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
                        RunLogicBlock(true, m_generalBasicLB, 50, 50);
                    break;
            }
        }

        public void MakeCollideable()
        {
            this.IsCollidable = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (m_levelScreen != null && m_levelScreen.CurrentRoom != null)
            {
                if (this.IsKilled == false && CollisionMath.Intersects(this.TerrainBounds, this.m_levelScreen.CurrentRoom.Bounds) == false)
                    this.Kill(true);
            }

            base.Update(gameTime);
        }

        public EnemyObj_Chicken(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("EnemyChickenRun_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            this.Type = EnemyType.CHICKEN;
        }

        public override void HitEnemy(int damage, Vector2 collisionPt, bool isPlayer)
        {
            SoundManager.Play3DSound(this, m_target, "Chicken_Cluck_01", "Chicken_Cluck_02", "Chicken_Cluck_03");
            base.HitEnemy(damage, collisionPt, isPlayer);
        }
    }
}
