using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.EnvironmentVariables;
using RogueCastle.GameStructs;
using RogueCastle.Screens;

namespace RogueCastle
{
    public class EnemyObj_Energon : EnemyObj
    {
        private const byte TYPE_SWORD = 0;
        private const byte TYPE_SHIELD = 1;
        private const byte TYPE_DOWNSWORD = 2;

        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();
        private LogicBlock m_generalMiniBossLB = new LogicBlock();
        private LogicBlock m_generalCooldownLB = new LogicBlock();

        private SpriteObj m_shield;
        private DS2DPool<EnergonProjectileObj> m_projectilePool;
        private byte m_poolSize = 10;
        private byte m_currentAttackType = TYPE_SWORD;

        protected override void InitializeEV()
        {
            #region Basic Variables - General
            Name = EnemyEV.ENERGON_BASIC_NAME;
            LocStringID = EnemyEV.ENERGON_BASIC_NAME_LOC_ID;

            MaxHealth = EnemyEV.ENERGON_BASIC_MAX_HEALTH;
            Damage = EnemyEV.ENERGON_BASIC_DAMAGE;
            XPValue = EnemyEV.ENERGON_BASIC_XP_VALUE;

            MinMoneyDropAmount = EnemyEV.ENERGON_BASIC_MIN_DROP_AMOUNT;
            MaxMoneyDropAmount = EnemyEV.ENERGON_BASIC_MAX_DROP_AMOUNT;
            MoneyDropChance = EnemyEV.ENERGON_BASIC_DROP_CHANCE;

            Speed = EnemyEV.ENERGON_BASIC_SPEED;
            TurnSpeed = EnemyEV.ENERGON_BASIC_TURN_SPEED;
            ProjectileSpeed = EnemyEV.ENERGON_BASIC_PROJECTILE_SPEED;
            JumpHeight = EnemyEV.ENERGON_BASIC_JUMP;
            CooldownTime = EnemyEV.ENERGON_BASIC_COOLDOWN;
            AnimationDelay = 1 / EnemyEV.ENERGON_BASIC_ANIMATION_DELAY;

            AlwaysFaceTarget = EnemyEV.ENERGON_BASIC_ALWAYS_FACE_TARGET;
            CanFallOffLedges = EnemyEV.ENERGON_BASIC_CAN_FALL_OFF_LEDGES;
            CanBeKnockedBack = EnemyEV.ENERGON_BASIC_CAN_BE_KNOCKED_BACK;
            IsWeighted = EnemyEV.ENERGON_BASIC_IS_WEIGHTED;

            Scale = EnemyEV.EnergonBasicScale;
            ProjectileScale = EnemyEV.EnergonBasicProjectileScale;
            TintablePart.TextureColor = EnemyEV.EnergonBasicTint;

            MeleeRadius = EnemyEV.ENERGON_BASIC_MELEE_RADIUS;
            ProjectileRadius = EnemyEV.ENERGON_BASIC_PROJECTILE_RADIUS;
            EngageRadius = EnemyEV.ENERGON_BASIC_ENGAGE_RADIUS;

            ProjectileDamage = Damage;
            KnockBack = EnemyEV.EnergonBasicKnockBack;
            #endregion

            switch (Difficulty)
            {
                case (GameTypes.EnemyDifficulty.Miniboss):
                    #region Miniboss Variables - General
                    Name = EnemyEV.ENERGON_MINIBOSS_NAME;
                    LocStringID = EnemyEV.ENERGON_MINIBOSS_NAME_LOC_ID;

                    MaxHealth = EnemyEV.ENERGON_MINIBOSS_MAX_HEALTH;
                    Damage = EnemyEV.ENERGON_MINIBOSS_DAMAGE;
                    XPValue = EnemyEV.ENERGON_MINIBOSS_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.ENERGON_MINIBOSS_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.ENERGON_MINIBOSS_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.ENERGON_MINIBOSS_DROP_CHANCE;

                    Speed = EnemyEV.ENERGON_MINIBOSS_SPEED;
                    TurnSpeed = EnemyEV.ENERGON_MINIBOSS_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.ENERGON_MINIBOSS_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.ENERGON_MINIBOSS_JUMP;
                    CooldownTime = EnemyEV.ENERGON_MINIBOSS_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.ENERGON_MINIBOSS_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.ENERGON_MINIBOSS_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.ENERGON_MINIBOSS_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.ENERGON_MINIBOSS_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.ENERGON_MINIBOSS_IS_WEIGHTED;

                    Scale = EnemyEV.EnergonMinibossScale;
                    ProjectileScale = EnemyEV.EnergonMinibossProjectileScale;
                    TintablePart.TextureColor = EnemyEV.EnergonMinibossTint;

                    MeleeRadius = EnemyEV.ENERGON_MINIBOSS_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.ENERGON_MINIBOSS_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.ENERGON_MINIBOSS_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.EnergonMinibossKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Expert):
                    #region Expert Variables - General
                    Name = EnemyEV.ENERGON_EXPERT_NAME;
                    LocStringID = EnemyEV.ENERGON_EXPERT_NAME_LOC_ID;

                    MaxHealth = EnemyEV.ENERGON_EXPERT_MAX_HEALTH;
                    Damage = EnemyEV.ENERGON_EXPERT_DAMAGE;
                    XPValue = EnemyEV.ENERGON_EXPERT_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.ENERGON_EXPERT_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.ENERGON_EXPERT_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.ENERGON_EXPERT_DROP_CHANCE;

                    Speed = EnemyEV.ENERGON_EXPERT_SPEED;
                    TurnSpeed = EnemyEV.ENERGON_EXPERT_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.ENERGON_EXPERT_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.ENERGON_EXPERT_JUMP;
                    CooldownTime = EnemyEV.ENERGON_EXPERT_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.ENERGON_EXPERT_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.ENERGON_EXPERT_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.ENERGON_EXPERT_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.ENERGON_EXPERT_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.ENERGON_EXPERT_IS_WEIGHTED;

                    Scale = EnemyEV.EnergonExpertScale;
                    ProjectileScale = EnemyEV.EnergonExpertProjectileScale;
                    TintablePart.TextureColor = EnemyEV.EnergonExpertTint;

                    MeleeRadius = EnemyEV.ENERGON_EXPERT_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.ENERGON_EXPERT_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.ENERGON_EXPERT_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.EnergonExpertKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Advanced):
                    #region Advanced Variables - General
                    Name = EnemyEV.ENERGON_ADVANCED_NAME;
                    LocStringID = EnemyEV.ENERGON_ADVANCED_NAME_LOC_ID;

                    MaxHealth = EnemyEV.ENERGON_ADVANCED_MAX_HEALTH;
                    Damage = EnemyEV.ENERGON_ADVANCED_DAMAGE;
                    XPValue = EnemyEV.ENERGON_ADVANCED_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.ENERGON_ADVANCED_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.ENERGON_ADVANCED_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.ENERGON_ADVANCED_DROP_CHANCE;

                    Speed = EnemyEV.ENERGON_ADVANCED_SPEED;
                    TurnSpeed = EnemyEV.ENERGON_ADVANCED_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.ENERGON_ADVANCED_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.ENERGON_ADVANCED_JUMP;
                    CooldownTime = EnemyEV.ENERGON_ADVANCED_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.ENERGON_ADVANCED_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.ENERGON_ADVANCED_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.ENERGON_ADVANCED_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.ENERGON_ADVANCED_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.ENERGON_ADVANCED_IS_WEIGHTED;

                    Scale = EnemyEV.EnergonAdvancedScale;
                    ProjectileScale = EnemyEV.EnergonAdvancedProjectileScale;
                    TintablePart.TextureColor = EnemyEV.EnergonAdvancedTint;

                    MeleeRadius = EnemyEV.ENERGON_ADVANCED_MELEE_RADIUS;
                    EngageRadius = EnemyEV.ENERGON_ADVANCED_ENGAGE_RADIUS;
                    ProjectileRadius = EnemyEV.ENERGON_ADVANCED_PROJECTILE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.EnergonAdvancedKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Basic):
                default:
                    break;
            }		
	

        }

        protected override void InitializeLogic()
        {
            LogicSet walkTowardsLS = new LogicSet(this);
            walkTowardsLS.AddAction(new ChangeSpriteLogicAction("EnemyEnergonWalk_Character", true, true));
            walkTowardsLS.AddAction(new MoveLogicAction(m_target, true));
            walkTowardsLS.AddAction(new DelayLogicAction(0.2f, 0.75f));

            LogicSet walkAwayLS = new LogicSet(this);
            walkAwayLS.AddAction(new ChangeSpriteLogicAction("EnemyEnergonWalk_Character", true, true));
            walkAwayLS.AddAction(new MoveLogicAction(m_target, false));
            walkAwayLS.AddAction(new DelayLogicAction(0.2f, 0.75f));

            LogicSet walkStopLS = new LogicSet(this);
            walkStopLS.AddAction(new ChangeSpriteLogicAction("EnemyEnergonIdle_Character", true, true));
            walkStopLS.AddAction(new MoveLogicAction(m_target, true, 0));
            walkStopLS.AddAction(new DelayLogicAction(0.2f, 0.75f));

            LogicSet fireProjectile = new LogicSet(this);
            fireProjectile.AddAction(new ChangePropertyLogicAction(this, "CurrentSpeed", 0));
            fireProjectile.AddAction(new ChangeSpriteLogicAction("EnemyEnergonAttack_Character", false, false));
            fireProjectile.AddAction(new PlayAnimationLogicAction("Start", "BeforeAttack"));
            fireProjectile.AddAction(new RunFunctionLogicAction(this, "FireCurrentTypeProjectile"));
            fireProjectile.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            fireProjectile.AddAction(new ChangeSpriteLogicAction("EnemyEnergonIdle_Character", true, true));
            fireProjectile.Tag = GameTypes.LOGIC_SET_TYPE_ATTACK;

            LogicSet changeAttackType = new LogicSet(this);
            changeAttackType.AddAction(new ChangePropertyLogicAction(this, "CurrentSpeed", 0));
            changeAttackType.AddAction(new ChangeSpriteLogicAction("EnemyEnergonAttack_Character", false, false));
            changeAttackType.AddAction(new PlayAnimationLogicAction("Start", "BeforeAttack"));
            changeAttackType.AddAction(new RunFunctionLogicAction(this, "SwitchRandomType"));
            changeAttackType.AddAction(new PlayAnimationLogicAction("Attack", "End"));
            changeAttackType.AddAction(new ChangeSpriteLogicAction("EnemyEnergonIdle_Character", true, true));
            changeAttackType.AddAction(new DelayLogicAction(2.0f));

            m_generalBasicLB.AddLogicSet(walkTowardsLS, walkAwayLS, walkStopLS, fireProjectile, changeAttackType);
            m_generalCooldownLB.AddLogicSet(walkTowardsLS, walkAwayLS, walkStopLS);

            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalMiniBossLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);

            SetCooldownLogicBlock(m_generalCooldownLB, 0, 0, 100); //walkTowardsLS, walkAwayLS, walkStopLS

            base.InitializeLogic();
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case (STATE_MELEE_ENGAGE):
                case (STATE_PROJECTILE_ENGAGE):
                    RunLogicBlock(true, m_generalBasicLB, 0, 0, 30, 60, 10); //walkTowardsLS, walkAwayLS, walkStopLS, fireProjectile, changeAttackType
                    break;
                case (STATE_ENGAGE):
                case (STATE_WANDER):
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
                case (STATE_WANDER):
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
                case (STATE_WANDER):
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
                case (STATE_WANDER):
                default:
                    break;
            }
        }

        public void FireCurrentTypeProjectile()
        {
            FireProjectile(m_currentAttackType);
        }

        public void FireProjectile(byte type)
        {
            EnergonProjectileObj projectile = m_projectilePool.CheckOut();
            projectile.SetType(type);
            PhysicsMngr.AddObject(projectile);
            projectile.Target = m_target;
            projectile.Visible = true;
            projectile.Position = this.Position;
            projectile.CurrentSpeed = ProjectileSpeed;
            projectile.Flip = this.Flip;
            projectile.Scale = ProjectileScale;
            projectile.Opacity = 0.8f;
            projectile.Damage = this.Damage;
            projectile.PlayAnimation(true);
            //projectile.TurnSpeed = TurnSpeed;
        }

        public void DestroyProjectile(EnergonProjectileObj projectile)
        {
            if (m_projectilePool.ActiveObjsList.Contains(projectile)) // Only destroy projectiles if they haven't already been destroyed. It is possible for two objects to call Destroy on the same projectile.
            {
                projectile.Visible = false;
                projectile.Scale = new Vector2(1, 1);
                projectile.CollisionTypeTag = GameTypes.COLLISION_TYPE_ENEMY;
                PhysicsMngr.RemoveObject(projectile); // Might be better to keep them in the physics manager and just turn off their collision detection.
                m_projectilePool.CheckIn(projectile);
            }
        }

        public void DestroyAllProjectiles()
        {
            ProjectileObj[] activeProjectilesArray = m_projectilePool.ActiveObjsList.ToArray();
            foreach (EnergonProjectileObj projectile in activeProjectilesArray)
            {
                DestroyProjectile(projectile);
            }
        }

        public void SwitchRandomType()
        {
            byte storedAttackType = m_currentAttackType;
            while (storedAttackType == m_currentAttackType)
                storedAttackType = (byte)CDGMath.RandomInt(0, 2);
            SwitchType(storedAttackType);
        }

        public void SwitchType(byte type)
        {
            m_currentAttackType = type;
            switch (type)
            {
                case (TYPE_SWORD):
                    m_shield.ChangeSprite("EnergonSwordShield_Sprite");
                    break;
                case (TYPE_SHIELD):
                    m_shield.ChangeSprite("EnergonShieldShield_Sprite");
                    break;
                case (TYPE_DOWNSWORD):
                    m_shield.ChangeSprite("EnergonDownSwordShield_Sprite");
                    break;
            }
            m_shield.PlayAnimation(true);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (EnergonProjectileObj projectile in m_projectilePool.ActiveObjsList)
                projectile.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(Camera2D camera)
        {
            base.Draw(camera);
            m_shield.Position = this.Position;
            m_shield.Flip = this.Flip;
            m_shield.Draw(camera);
            foreach (ProjectileObj projectile in m_projectilePool.ActiveObjsList)
                projectile.Draw(camera);
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
        {
            if ((collisionResponseType == Consts.COLLISIONRESPONSE_FIRSTBOXHIT) && m_invincibleCounter <= 0 && otherBox.AbsParent is PlayerObj)
            {
                if (m_target.Bounds.Left + m_target.Bounds.Width / 2 < this.X)
                    m_target.AccelerationX = -m_target.EnemyKnockBack.X;
                else
                    m_target.AccelerationX = m_target.EnemyKnockBack.X;
                m_target.AccelerationY = -m_target.EnemyKnockBack.Y;

                //m_target.CancelAttack();
                Point intersectPt = Rectangle.Intersect(thisBox.AbsRect, otherBox.AbsRect).Center;
                Vector2 impactPosition = new Vector2(intersectPt.X, intersectPt.Y);

                m_levelScreen.ImpactEffectPool.DisplayBlockImpactEffect(impactPosition, Vector2.One);
                m_levelScreen.SetLastEnemyHit(this);
                m_invincibleCounter = InvincibilityTime;
                Blink(Color.LightBlue, 0.1f);

                ProjectileObj projectile = otherBox.AbsParent as ProjectileObj;
                if (projectile != null)
                    m_levelScreen.ProjectileManager.DestroyProjectile(projectile);
            }
            else if (otherBox.AbsParent is EnergonProjectileObj)
            {
                //base.CollisionResponse(thisBox, otherBox, collisionResponseType);

                EnergonProjectileObj projectile = otherBox.AbsParent as EnergonProjectileObj;
                if (projectile != null)
                {
                    Point intersectPt = Rectangle.Intersect(thisBox.AbsRect, otherBox.AbsRect).Center;
                    Vector2 impactPosition = new Vector2(intersectPt.X, intersectPt.Y);

                    DestroyProjectile(projectile);
                    if (projectile.AttackType == m_currentAttackType)
                        HitEnemy(projectile.Damage, impactPosition, true);
                    else
                        m_levelScreen.ImpactEffectPool.DisplayBlockImpactEffect(impactPosition, Vector2.One);
                }
            }
            else
                base.CollisionResponse(thisBox, otherBox, collisionResponseType);
        }

        public override void Kill(bool giveXP = true)
        {
            m_shield.Visible = false;
            DestroyAllProjectiles();
            base.Kill(giveXP);
        }

        public override void Reset()
        {
            m_shield.Visible = true;
            base.Reset();
        }

        public EnemyObj_Energon(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("EnemyEnergonIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            this.Type = EnemyType.ENERGON;
            m_shield = new SpriteObj("EnergonSwordShield_Sprite");
            m_shield.AnimationDelay = 1 / 10f;
            m_shield.PlayAnimation(true);
            m_shield.Opacity = 0.5f;
            m_shield.Scale = new Vector2(1.2f, 1.2f);

            m_projectilePool = new DS2DPool<EnergonProjectileObj>();

            for (int i = 0; i < m_poolSize; i++)
            {
                EnergonProjectileObj projectile = new EnergonProjectileObj("EnergonSwordProjectile_Sprite", this);
                projectile.Visible = false;
                projectile.CollidesWithTerrain = false;
                projectile.PlayAnimation(true);
                projectile.AnimationDelay = 1 / 20f;
                m_projectilePool.AddToPool(projectile);
            }
        }

        public override void Dispose()
        {
            if (IsDisposed == false)
            {
                m_projectilePool.Dispose();
                m_projectilePool = null;
                m_shield.Dispose();
                m_shield = null;
                base.Dispose();
            }
        }
    }
}
