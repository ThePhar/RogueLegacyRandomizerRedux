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
    public class EnemyObj_Horse : EnemyObj
    {
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();
        private LogicBlock m_generalMiniBossLB = new LogicBlock();
        private LogicBlock m_generalCooldownLB = new LogicBlock();

        private LogicBlock m_turnLB = new LogicBlock();

        private int m_wallDistanceCheck = 430;

        private float m_collisionCheckTimer = 0.5f;
        private float m_fireDropTimer = 0.5f;
        private float m_fireDropInterval = 0.075f;
        private float m_fireDropLifespan = 0.75f;//2;

        private int m_numFireShieldObjs = 2;//4;//2;
        private float m_fireDistance = 110;
        private float m_fireRotationSpeed = 1.5f;//0;//1.5f;
        private float m_fireShieldScale = 2.5f;
        private List<ProjectileObj> m_fireShieldList;

        private FrameSoundObj m_gallopSound;

        private bool m_turning = false; // Ensures the horse doesn't turn multiple times in a single update.

        protected override void InitializeEV()
        {
            LockFlip = true;
            #region Basic Variables - General
            Name = EnemyEV.HORSE_BASIC_NAME;
            LocStringID = EnemyEV.HORSE_BASIC_NAME_LOC_ID;

            MaxHealth = EnemyEV.HORSE_BASIC_MAX_HEALTH;
            Damage = EnemyEV.HORSE_BASIC_DAMAGE;
            XPValue = EnemyEV.HORSE_BASIC_XP_VALUE;

            MinMoneyDropAmount = EnemyEV.HORSE_BASIC_MIN_DROP_AMOUNT;
            MaxMoneyDropAmount = EnemyEV.HORSE_BASIC_MAX_DROP_AMOUNT;
            MoneyDropChance = EnemyEV.HORSE_BASIC_DROP_CHANCE;

            Speed = EnemyEV.HORSE_BASIC_SPEED;
            TurnSpeed = EnemyEV.HORSE_BASIC_TURN_SPEED;
            ProjectileSpeed = EnemyEV.HORSE_BASIC_PROJECTILE_SPEED;
            JumpHeight = EnemyEV.HORSE_BASIC_JUMP;
            CooldownTime = EnemyEV.HORSE_BASIC_COOLDOWN;
            AnimationDelay = 1 / EnemyEV.HORSE_BASIC_ANIMATION_DELAY;

            AlwaysFaceTarget = EnemyEV.HORSE_BASIC_ALWAYS_FACE_TARGET;
            CanFallOffLedges = EnemyEV.HORSE_BASIC_CAN_FALL_OFF_LEDGES;
            CanBeKnockedBack = EnemyEV.HORSE_BASIC_CAN_BE_KNOCKED_BACK;
            IsWeighted = EnemyEV.HORSE_BASIC_IS_WEIGHTED;

            Scale = EnemyEV.HorseBasicScale;
            ProjectileScale = EnemyEV.HorseBasicProjectileScale;
            TintablePart.TextureColor = EnemyEV.HorseBasicTint;

            MeleeRadius = EnemyEV.HORSE_BASIC_MELEE_RADIUS;
            ProjectileRadius = EnemyEV.HORSE_BASIC_PROJECTILE_RADIUS;
            EngageRadius = EnemyEV.HORSE_BASIC_ENGAGE_RADIUS;

            ProjectileDamage = Damage;
            KnockBack = EnemyEV.HorseBasicKnockBack;
            #endregion

            switch (Difficulty)
            {
                case (GameTypes.EnemyDifficulty.Miniboss):
                    #region Miniboss Variables - General
                    Name = EnemyEV.HORSE_MINIBOSS_NAME;
                    LocStringID = EnemyEV.HORSE_MINIBOSS_NAME_LOC_ID;

                    MaxHealth = EnemyEV.HORSE_MINIBOSS_MAX_HEALTH;
                    Damage = EnemyEV.HORSE_MINIBOSS_DAMAGE;
                    XPValue = EnemyEV.HORSE_MINIBOSS_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.HORSE_MINIBOSS_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.HORSE_MINIBOSS_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.HORSE_MINIBOSS_DROP_CHANCE;

                    Speed = EnemyEV.HORSE_MINIBOSS_SPEED;
                    TurnSpeed = EnemyEV.HORSE_MINIBOSS_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.HORSE_MINIBOSS_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.HORSE_MINIBOSS_JUMP;
                    CooldownTime = EnemyEV.HORSE_MINIBOSS_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.HORSE_MINIBOSS_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.HORSE_MINIBOSS_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.HORSE_MINIBOSS_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.HORSE_MINIBOSS_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.HORSE_MINIBOSS_IS_WEIGHTED;

                    Scale = EnemyEV.HorseMinibossScale;
                    ProjectileScale = EnemyEV.HorseMinibossProjectileScale;
                    TintablePart.TextureColor = EnemyEV.HorseMinibossTint;

                    MeleeRadius = EnemyEV.HORSE_MINIBOSS_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.HORSE_MINIBOSS_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.HORSE_MINIBOSS_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.HorseMinibossKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Expert):
                    #region Expert Variables - General
                    Name = EnemyEV.HORSE_EXPERT_NAME;
                    LocStringID = EnemyEV.HORSE_EXPERT_NAME_LOC_ID;

                    MaxHealth = EnemyEV.HORSE_EXPERT_MAX_HEALTH;
                    Damage = EnemyEV.HORSE_EXPERT_DAMAGE;
                    XPValue = EnemyEV.HORSE_EXPERT_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.HORSE_EXPERT_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.HORSE_EXPERT_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.HORSE_EXPERT_DROP_CHANCE;

                    Speed = EnemyEV.HORSE_EXPERT_SPEED;
                    TurnSpeed = EnemyEV.HORSE_EXPERT_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.HORSE_EXPERT_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.HORSE_EXPERT_JUMP;
                    CooldownTime = EnemyEV.HORSE_EXPERT_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.HORSE_EXPERT_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.HORSE_EXPERT_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.HORSE_EXPERT_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.HORSE_EXPERT_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.HORSE_EXPERT_IS_WEIGHTED;

                    Scale = EnemyEV.HorseExpertScale;
                    ProjectileScale = EnemyEV.HorseExpertProjectileScale;
                    TintablePart.TextureColor = EnemyEV.HorseExpertTint;

                    MeleeRadius = EnemyEV.HORSE_EXPERT_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.HORSE_EXPERT_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.HORSE_EXPERT_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.HorseExpertKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Advanced):
                    #region Advanced Variables - General
                    Name = EnemyEV.HORSE_ADVANCED_NAME;
                    LocStringID = EnemyEV.HORSE_ADVANCED_NAME_LOC_ID;

                    MaxHealth = EnemyEV.HORSE_ADVANCED_MAX_HEALTH;
                    Damage = EnemyEV.HORSE_ADVANCED_DAMAGE;
                    XPValue = EnemyEV.HORSE_ADVANCED_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.HORSE_ADVANCED_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.HORSE_ADVANCED_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.HORSE_ADVANCED_DROP_CHANCE;

                    Speed = EnemyEV.HORSE_ADVANCED_SPEED;
                    TurnSpeed = EnemyEV.HORSE_ADVANCED_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.HORSE_ADVANCED_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.HORSE_ADVANCED_JUMP;
                    CooldownTime = EnemyEV.HORSE_ADVANCED_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.HORSE_ADVANCED_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.HORSE_ADVANCED_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.HORSE_ADVANCED_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.HORSE_ADVANCED_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.HORSE_ADVANCED_IS_WEIGHTED;

                    Scale = EnemyEV.HorseAdvancedScale;
                    ProjectileScale = EnemyEV.HorseAdvancedProjectileScale;
                    TintablePart.TextureColor = EnemyEV.HorseAdvancedTint;

                    MeleeRadius = EnemyEV.HORSE_ADVANCED_MELEE_RADIUS;
                    EngageRadius = EnemyEV.HORSE_ADVANCED_ENGAGE_RADIUS;
                    ProjectileRadius = EnemyEV.HORSE_ADVANCED_PROJECTILE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.HorseAdvancedKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Basic):
                default:
                    break;
            }				
        }

        protected override void InitializeLogic()
        {

            LogicSet runLeftLS = new LogicSet(this);
            runLeftLS.AddAction(new ChangeSpriteLogicAction("EnemyHorseRun_Character", true, true));
            runLeftLS.AddAction(new MoveDirectionLogicAction(new Vector2(-1,0)));
            runLeftLS.AddAction(new DelayLogicAction(0.5f));

            LogicSet runRightLS = new LogicSet(this);
            runRightLS.AddAction(new ChangeSpriteLogicAction("EnemyHorseRun_Character", true, true));
            runRightLS.AddAction(new MoveDirectionLogicAction(new Vector2(1, 0)));
            runRightLS.AddAction(new DelayLogicAction(0.5f));

            LogicSet turnRightLS = new LogicSet(this);
            turnRightLS.AddAction(new ChangeSpriteLogicAction("EnemyHorseTurn_Character", true, true));
            turnRightLS.AddAction(new MoveDirectionLogicAction(new Vector2(-1, 0)));
            turnRightLS.AddAction(new DelayLogicAction(0.25f));
            turnRightLS.AddAction(new ChangePropertyLogicAction(this, "Flip", SpriteEffects.None));
            turnRightLS.AddAction(new RunFunctionLogicAction(this, "ResetTurn"));

            LogicSet turnLeftLS = new LogicSet(this);
            turnLeftLS.AddAction(new ChangeSpriteLogicAction("EnemyHorseTurn_Character", true, true));
            turnLeftLS.AddAction(new MoveDirectionLogicAction(new Vector2(1, 0)));
            turnLeftLS.AddAction(new DelayLogicAction(0.25f));
            turnLeftLS.AddAction(new ChangePropertyLogicAction(this, "Flip", SpriteEffects.FlipHorizontally));
            turnLeftLS.AddAction(new RunFunctionLogicAction(this, "ResetTurn"));

            LogicSet runLeftExpertLS = new LogicSet(this);
            runLeftExpertLS.AddAction(new ChangeSpriteLogicAction("EnemyHorseRun_Character", true, true));
            runLeftExpertLS.AddAction(new MoveDirectionLogicAction(new Vector2(-1, 0)));
            ThrowStandingProjectiles(runLeftExpertLS, true);
            //runLeftExpertLS.AddAction(new DelayLogicAction(0.5f));

            LogicSet runRightExpertLS = new LogicSet(this);
            runRightExpertLS.AddAction(new ChangeSpriteLogicAction("EnemyHorseRun_Character", true, true));
            runRightExpertLS.AddAction(new MoveDirectionLogicAction(new Vector2(1, 0)));
            ThrowStandingProjectiles(runRightExpertLS, true);
            //runRightExpertLS.AddAction(new DelayLogicAction(0.0f));


            m_generalBasicLB.AddLogicSet(runLeftLS, runRightLS);
            //m_generalExpertLB.AddLogicSet(runLeftExpertLS, runRightExpertLS);

            m_turnLB.AddLogicSet(turnLeftLS, turnRightLS);

            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_turnLB);

            m_gallopSound = new FrameSoundObj(this, m_target, 2, "Enemy_Horse_Gallop_01", "Enemy_Horse_Gallop_02", "Enemy_Horse_Gallop_03");

            base.InitializeLogic();
        }

        private void ThrowStandingProjectiles(LogicSet ls, bool useBossProjectile = false)
        {
            ProjectileData projData = new ProjectileData(this)
            {
                SpriteName = "SpellDamageShield_Sprite",
                SourceAnchor = new Vector2(0, 60),//Vector2.Zero,
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0,
                Damage = Damage,
                AngleOffset = 0,
                Angle = new Vector2(0, 0),
                CollidesWithTerrain = false,
                Scale = ProjectileScale,
                Lifespan = 0.75f,//0.65f,//0.75f
            };

            ls.AddAction(new Play3DSoundLogicAction(this,  Game.ScreenManager.Player,"FairyAttack1"));
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            ls.AddAction(new DelayLogicAction(0.075f));
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            ls.AddAction(new DelayLogicAction(0.075f));
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Dispose();
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case (STATE_MELEE_ENGAGE):
                case (STATE_PROJECTILE_ENGAGE):
                case (STATE_ENGAGE):
                case (STATE_WANDER):
                    if (this.Flip == SpriteEffects.FlipHorizontally)
                        RunLogicBlock(true, m_generalBasicLB, 100, 0); // Run to the left.
                    else
                        RunLogicBlock(true, m_generalBasicLB, 0, 100); // Run to the right
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
                case (STATE_WANDER):
                    if (this.Flip == SpriteEffects.FlipHorizontally)
                        RunLogicBlock(true, m_generalBasicLB, 100, 0); // Run to the left.
                    else
                        RunLogicBlock(true, m_generalBasicLB, 0, 100); // Run to the right
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
                case (STATE_WANDER):
                    if (this.Flip == SpriteEffects.FlipHorizontally)
                        RunLogicBlock(true, m_generalBasicLB, 100, 0); // Run to the left.
                    else
                        RunLogicBlock(true, m_generalBasicLB, 0, 100); // Run to the right
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
                case (STATE_WANDER):
                    if (this.Flip == SpriteEffects.FlipHorizontally)
                        RunLogicBlock(true, m_generalBasicLB, 100, 0); // Run to the left.
                    else
                        RunLogicBlock(true, m_generalBasicLB, 0, 100); // Run to the right
                    break;
                default:
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            //m_collidingWithGround = false;
            //m_collidingWithWall = false;

            //foreach (TerrainObj obj in m_levelScreen.CurrentRoom.TerrainObjList)
            //{
            //    if (obj.Rotation == 0)
            //    {
            //        if (CollisionMath.Intersects(obj.Bounds, WallCollisionPoint) == true)
            //            m_collidingWithWall = true;
            //        if (CollisionMath.Intersects(obj.Bounds, GroundCollisionPoint) == true)
            //            m_collidingWithGround = true;
            //    }
            //}

            //if (m_currentActiveLB != m_turnLB)
            //{
            //    if (m_collidingWithWall == true || m_collidingWithGround == false)
            //    {
            //        if (this.HeadingX < 0)
            //        {
            //            m_currentActiveLB.StopLogicBlock();
            //            RunLogicBlock(false, m_turnLB, 0, 100);
            //        }
            //        else
            //        {
            //            m_currentActiveLB.StopLogicBlock();
            //            RunLogicBlock(false, m_turnLB, 100, 0);
            //        }
            //    }
            //}

            if (m_target.AttachedLevel.CurrentRoom.Name != "Ending")
                m_gallopSound.Update();

            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.Difficulty >= GameTypes.EnemyDifficulty.Advanced)
            {
                if (m_fireDropTimer > 0)
                {
                    m_fireDropTimer -= elapsedTime;
                    if (m_fireDropTimer <= 0)
                    {
                        DropFireProjectile();
                        m_fireDropTimer = m_fireDropInterval;
                    }
                }
            }
            
            if (this.Difficulty == GameTypes.EnemyDifficulty.Expert && this.IsPaused == false)
            {
                if (m_fireShieldList.Count < 1)
                    CastFireShield(m_numFireShieldObjs);
            }

            if (((this.Bounds.Left < m_levelScreen.CurrentRoom.Bounds.Left) || (this.Bounds.Right > m_levelScreen.CurrentRoom.Bounds.Right)) && m_collisionCheckTimer <= 0)
               TurnHorse();

            Rectangle collPt = new Rectangle();
            Rectangle collPt2 = new Rectangle(); // Pt2 is to check for sloped collisions.
            if (this.Flip == SpriteEffects.FlipHorizontally)
            {
                collPt = new Rectangle(this.Bounds.Left - 10, this.Bounds.Bottom + 20, 5, 5);
                collPt2 = new Rectangle(this.Bounds.Right + 50, this.Bounds.Bottom - 20, 5, 5);
            }
            else
            {
                collPt = new Rectangle(this.Bounds.Right + 10, this.Bounds.Bottom + 20, 5, 5);
                collPt2 = new Rectangle(this.Bounds.Left - 50, this.Bounds.Bottom - 20, 5, 5);
            }


            bool turn = true;
            foreach (TerrainObj terrain in m_levelScreen.CurrentRoom.TerrainObjList)
            {
                if (CollisionMath.Intersects(terrain.Bounds, collPt) || CollisionMath.Intersects(terrain.Bounds, collPt2))
                {
                    turn = false;
                    break;
                }
            }

            if (turn == true)
                TurnHorse();

            if (m_collisionCheckTimer > 0)
                m_collisionCheckTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        public void ResetTurn()
        {
            m_turning = false;
        }

        private void DropFireProjectile()
        {
            this.UpdateCollisionBoxes();
            ProjectileData projData = new ProjectileData(this)
            {
                SpriteName = "SpellDamageShield_Sprite",
                SourceAnchor = new Vector2(0, (this.Bounds.Bottom - this.Y) - 10),
                //Target = m_target,
                Speed = new Vector2(0, 0),
                IsWeighted = false,
                RotationSpeed = 0,
                Damage = Damage,
                Angle = new Vector2(0, 0),
                AngleOffset = 0,
                CollidesWithTerrain = false,
                Scale = ProjectileScale,
                Lifespan = m_fireDropLifespan,
                LockPosition = true,
            };

            m_levelScreen.ProjectileManager.FireProjectile(projData);
            projData.Dispose();
        }

        private void CastFireShield(int numFires)
        {
            ProjectileData projData = new ProjectileData(this)
            {
                SpriteName = "SpellDamageShield_Sprite",
                SourceAnchor = new Vector2(0, (this.Bounds.Bottom - this.Y) - 10),
                //Target = m_target,
                Speed = new Vector2(m_fireRotationSpeed, m_fireRotationSpeed),
                IsWeighted = false,
                RotationSpeed = 0,
                Target = this,
                Damage = Damage,
                Angle = new Vector2(0, 0),
                AngleOffset = 0,
                CollidesWithTerrain = false,
                Scale = new Vector2(m_fireShieldScale, m_fireShieldScale),
                Lifespan = 999999,
                DestroysWithEnemy = false,
                LockPosition = true,
            };

            SoundManager.PlaySound("Cast_FireShield");
            float projectileDistance = m_fireDistance;
            for (int i = 0; i < (int)numFires; i++)
            {
                float angle = (360f / numFires) * i;

                ProjectileObj proj = m_levelScreen.ProjectileManager.FireProjectile(projData);
                proj.AltX = angle; // AltX and AltY are used as holders to hold the projectiles angle and distance from player respectively.
                proj.AltY = projectileDistance;
                proj.Spell = SpellType.DAMAGE_SHIELD;
                proj.CanBeFusRohDahed = false;
                proj.AccelerationXEnabled = false;
                proj.AccelerationYEnabled = false;
                proj.IgnoreBoundsCheck = true;

                m_fireShieldList.Add(proj);
            }
        }

        private void TurnHorse()
        {
            if (m_turning == false)
            {
                m_turning = true;
                if (this.HeadingX < 0)
                {
                    m_currentActiveLB.StopLogicBlock();
                    RunLogicBlock(false, m_turnLB, 0, 100);
                }
                else
                {
                    m_currentActiveLB.StopLogicBlock();
                    RunLogicBlock(false, m_turnLB, 100, 0);
                }
                m_collisionCheckTimer = 0.5f;
            }
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
        {
            TerrainObj terrain = otherBox.AbsParent as TerrainObj;
            if (otherBox.AbsParent.Bounds.Top < this.TerrainBounds.Bottom - 20 && terrain != null && terrain.CollidesLeft == true && terrain.CollidesRight == true && terrain.CollidesBottom == true)
            {
                if (collisionResponseType == Consts.COLLISIONRESPONSE_TERRAIN && otherBox.AbsRotation == 0 && m_collisionCheckTimer <= 0)
                {
                    Vector2 mtd = CollisionMath.CalculateMTD(thisBox.AbsRect, otherBox.AbsRect);
                    if (mtd.X != 0)
                        TurnHorse();
                }
            }
            base.CollisionResponse(thisBox, otherBox, collisionResponseType);
        }

        public override void HitEnemy(int damage, Vector2 collisionPt, bool isPlayer)
        {
            SoundManager.Play3DSound(this, m_target, "Enemy_Horse_Hit_01", "Enemy_Horse_Hit_02", "Enemy_Horse_Hit_03");
            base.HitEnemy(damage, collisionPt, isPlayer);
        }

        public override void Kill(bool giveXP = true)
        {
            foreach (ProjectileObj projectile in m_fireShieldList)
                projectile.RunDestroyAnimation(false);

            m_fireShieldList.Clear();

            SoundManager.Play3DSound(this, m_target, "Enemy_Horse_Dead");
            base.Kill(giveXP);
        }

        public override void ResetState()
        {
            m_fireShieldList.Clear();
            base.ResetState();
        }

        public EnemyObj_Horse(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("EnemyHorseRun_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            this.Type = EnemyType.HORSE;
            m_fireShieldList = new List<ProjectileObj>();
        }

        public override void Dispose()
        {
            if (IsDisposed == false)
            {
                if (m_gallopSound != null)
                    m_gallopSound.Dispose();
                m_gallopSound = null;
                base.Dispose();
            }
        }

        private Rectangle WallCollisionPoint
        {
            get
            {
                if (this.HeadingX < 0)
                    return new Rectangle((int)this.X - m_wallDistanceCheck, (int)this.Y, 2, 2);
                else
                    return new Rectangle((int)this.X + m_wallDistanceCheck, (int)this.Y, 2, 2);
            }
        }

        private Rectangle GroundCollisionPoint
        {
            get
            {
                if (this.HeadingX < 0)
                    return new Rectangle((int)(this.X - (m_wallDistanceCheck * this.ScaleX)), (int)(this.Y + (60 * this.ScaleY)), 2, 2);
                else
                    return new Rectangle((int)(this.X + (m_wallDistanceCheck * this.ScaleX)), (int)(this.Y + (60 * this.ScaleY)), 2, 2);
            }
        }
    }
}
