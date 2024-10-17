using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DS2DEngine;
using Microsoft.Xna.Framework;
using InputSystem;
using RogueCastle.EnvironmentVariables;
using RogueCastle.GameStructs;

namespace RogueCastle
{
    public class EnemyObj_Portrait : EnemyObj
    {
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();
        private LogicBlock m_generalMiniBossLB = new LogicBlock();
        private LogicBlock m_generalCooldownLB = new LogicBlock();

        public bool Shake { get; set; }
        public bool Chasing { get; set; }

        protected override void InitializeEV()
        {
            this.LockFlip = true;

            #region Basic Variables - General
            Name = EnemyEV.PORTRAIT_BASIC_NAME;
            LocStringID = EnemyEV.PORTRAIT_BASIC_NAME_LOC_ID;

            MaxHealth = EnemyEV.PORTRAIT_BASIC_MAX_HEALTH;
            Damage = EnemyEV.PORTRAIT_BASIC_DAMAGE;
            XPValue = EnemyEV.PORTRAIT_BASIC_XP_VALUE;

            MinMoneyDropAmount = EnemyEV.PORTRAIT_BASIC_MIN_DROP_AMOUNT;
            MaxMoneyDropAmount = EnemyEV.PORTRAIT_BASIC_MAX_DROP_AMOUNT;
            MoneyDropChance = EnemyEV.PORTRAIT_BASIC_DROP_CHANCE;

            Speed = EnemyEV.PORTRAIT_BASIC_SPEED;
            TurnSpeed = EnemyEV.PORTRAIT_BASIC_TURN_SPEED;
            ProjectileSpeed = EnemyEV.PORTRAIT_BASIC_PROJECTILE_SPEED;
            JumpHeight = EnemyEV.PORTRAIT_BASIC_JUMP;
            CooldownTime = EnemyEV.PORTRAIT_BASIC_COOLDOWN;
            AnimationDelay = 1 / EnemyEV.PORTRAIT_BASIC_ANIMATION_DELAY;

            AlwaysFaceTarget = EnemyEV.PORTRAIT_BASIC_ALWAYS_FACE_TARGET;
            CanFallOffLedges = EnemyEV.PORTRAIT_BASIC_CAN_FALL_OFF_LEDGES;
            CanBeKnockedBack = EnemyEV.PORTRAIT_BASIC_CAN_BE_KNOCKED_BACK;
            IsWeighted = EnemyEV.PORTRAIT_BASIC_IS_WEIGHTED;

            Scale = EnemyEV.PortraitBasicScale;
            ProjectileScale = EnemyEV.PortraitBasicProjectileScale;
            TintablePart.TextureColor = EnemyEV.PortraitBasicTint;

            MeleeRadius = EnemyEV.PORTRAIT_BASIC_MELEE_RADIUS;
            ProjectileRadius = EnemyEV.PORTRAIT_BASIC_PROJECTILE_RADIUS;
            EngageRadius = EnemyEV.PORTRAIT_BASIC_ENGAGE_RADIUS;

            ProjectileDamage = Damage;
            KnockBack = EnemyEV.PortraitBasicKnockBack;
            #endregion

            switch (Difficulty)
            {
                case (GameTypes.EnemyDifficulty.Miniboss):
                    #region Miniboss Variables - General
                    Name = EnemyEV.PORTRAIT_MINIBOSS_NAME;
                    LocStringID = EnemyEV.PORTRAIT_MINIBOSS_NAME_LOC_ID;

                    MaxHealth = EnemyEV.PORTRAIT_MINIBOSS_MAX_HEALTH;
                    Damage = EnemyEV.PORTRAIT_MINIBOSS_DAMAGE;
                    XPValue = EnemyEV.PORTRAIT_MINIBOSS_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.PORTRAIT_MINIBOSS_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.PORTRAIT_MINIBOSS_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.PORTRAIT_MINIBOSS_DROP_CHANCE;

                    Speed = EnemyEV.PORTRAIT_MINIBOSS_SPEED;
                    TurnSpeed = EnemyEV.PORTRAIT_MINIBOSS_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.PORTRAIT_MINIBOSS_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.PORTRAIT_MINIBOSS_JUMP;
                    CooldownTime = EnemyEV.PORTRAIT_MINIBOSS_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.PORTRAIT_MINIBOSS_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.PORTRAIT_MINIBOSS_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.PORTRAIT_MINIBOSS_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.PORTRAIT_MINIBOSS_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.PORTRAIT_MINIBOSS_IS_WEIGHTED;

                    Scale = EnemyEV.PortraitMinibossScale;
                    ProjectileScale = EnemyEV.PortraitMinibossProjectileScale;
                    TintablePart.TextureColor = EnemyEV.PortraitMinibossTint;

                    MeleeRadius = EnemyEV.PORTRAIT_MINIBOSS_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.PORTRAIT_MINIBOSS_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.PORTRAIT_MINIBOSS_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.PortraitMinibossKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Expert):
                    #region Expert Variables - General
                    Name = EnemyEV.PORTRAIT_EXPERT_NAME;
                    LocStringID = EnemyEV.PORTRAIT_EXPERT_NAME_LOC_ID;

                    MaxHealth = EnemyEV.PORTRAIT_EXPERT_MAX_HEALTH;
                    Damage = EnemyEV.PORTRAIT_EXPERT_DAMAGE;
                    XPValue = EnemyEV.PORTRAIT_EXPERT_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.PORTRAIT_EXPERT_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.PORTRAIT_EXPERT_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.PORTRAIT_EXPERT_DROP_CHANCE;

                    Speed = EnemyEV.PORTRAIT_EXPERT_SPEED;
                    TurnSpeed = EnemyEV.PORTRAIT_EXPERT_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.PORTRAIT_EXPERT_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.PORTRAIT_EXPERT_JUMP;
                    CooldownTime = EnemyEV.PORTRAIT_EXPERT_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.PORTRAIT_EXPERT_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.PORTRAIT_EXPERT_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.PORTRAIT_EXPERT_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.PORTRAIT_EXPERT_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.PORTRAIT_EXPERT_IS_WEIGHTED;

                    Scale = EnemyEV.PortraitExpertScale;
                    ProjectileScale = EnemyEV.PortraitExpertProjectileScale;
                    TintablePart.TextureColor = EnemyEV.PortraitExpertTint;

                    MeleeRadius = EnemyEV.PORTRAIT_EXPERT_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.PORTRAIT_EXPERT_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.PORTRAIT_EXPERT_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.PortraitExpertKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Advanced):
                    #region Advanced Variables - General
                    Name = EnemyEV.PORTRAIT_ADVANCED_NAME;
                    LocStringID = EnemyEV.PORTRAIT_ADVANCED_NAME_LOC_ID;

                    MaxHealth = EnemyEV.PORTRAIT_ADVANCED_MAX_HEALTH;
                    Damage = EnemyEV.PORTRAIT_ADVANCED_DAMAGE;
                    XPValue = EnemyEV.PORTRAIT_ADVANCED_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.PORTRAIT_ADVANCED_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.PORTRAIT_ADVANCED_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.PORTRAIT_ADVANCED_DROP_CHANCE;

                    Speed = EnemyEV.PORTRAIT_ADVANCED_SPEED;
                    TurnSpeed = EnemyEV.PORTRAIT_ADVANCED_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.PORTRAIT_ADVANCED_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.PORTRAIT_ADVANCED_JUMP;
                    CooldownTime = EnemyEV.PORTRAIT_ADVANCED_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.PORTRAIT_ADVANCED_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.PORTRAIT_ADVANCED_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.PORTRAIT_ADVANCED_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.PORTRAIT_ADVANCED_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.PORTRAIT_ADVANCED_IS_WEIGHTED;

                    Scale = EnemyEV.PortraitAdvancedScale;
                    ProjectileScale = EnemyEV.PortraitAdvancedProjectileScale;
                    TintablePart.TextureColor = EnemyEV.PortraitAdvancedTint;

                    MeleeRadius = EnemyEV.PORTRAIT_ADVANCED_MELEE_RADIUS;
                    EngageRadius = EnemyEV.PORTRAIT_ADVANCED_ENGAGE_RADIUS;
                    ProjectileRadius = EnemyEV.PORTRAIT_ADVANCED_PROJECTILE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.PortraitAdvancedKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Basic):
                default:
                    break;
            }
        }

        protected override void InitializeLogic()
        {
            LogicSet basicWarningLS = new LogicSet(this);
            basicWarningLS.AddAction(new ChangePropertyLogicAction(this, "Shake", true));
            basicWarningLS.AddAction(new DelayLogicAction(1));
            basicWarningLS.AddAction(new ChangePropertyLogicAction(this, "Shake", false));
            basicWarningLS.AddAction(new DelayLogicAction(1));

            LogicSet moveTowardsLS = new LogicSet(this);
            moveTowardsLS.AddAction(new ChaseLogicAction(m_target, Vector2.Zero, Vector2.Zero, true, 1));

            LogicSet moveTowardsAdvancedLS = new LogicSet(this);
            moveTowardsAdvancedLS.AddAction(new ChaseLogicAction(m_target, Vector2.Zero, Vector2.Zero, true, 1.75f));
            ThrowAdvancedProjectiles(moveTowardsAdvancedLS, true);

            LogicSet moveTowardsExpertLS = new LogicSet(this);
            moveTowardsExpertLS.AddAction(new ChaseLogicAction(m_target, Vector2.Zero, Vector2.Zero, true, 1.75f));
            ThrowExpertProjectiles(moveTowardsExpertLS, true);

            LogicSet moveTowardsMiniBosstLS = new LogicSet(this);
            moveTowardsMiniBosstLS.AddAction(new ChaseLogicAction(m_target, Vector2.Zero, Vector2.Zero, true, 1.25f));
            ThrowProjectiles(moveTowardsMiniBosstLS, true);

            m_generalBasicLB.AddLogicSet(basicWarningLS, moveTowardsLS);
            m_generalAdvancedLB.AddLogicSet(basicWarningLS, moveTowardsAdvancedLS);
            m_generalExpertLB.AddLogicSet(basicWarningLS, moveTowardsExpertLS);
            m_generalMiniBossLB.AddLogicSet(basicWarningLS, moveTowardsMiniBosstLS);
            m_generalCooldownLB.AddLogicSet(basicWarningLS, moveTowardsLS);

            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalMiniBossLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);

            base.InitializeLogic();

            // HACK ALERT
            // This is NOT the right way to add collision boxes but it doesn't seem to be working the normal way.  This may cause problems though.
            this.CollisionBoxes.Clear();
            this.CollisionBoxes.Add(new CollisionBox((int)(-18 * this.ScaleX), (int)(-24 * this.ScaleY), (int)(36 * this.ScaleX), (int)(48 * this.ScaleY), 2, this));
            this.CollisionBoxes.Add(new CollisionBox((int)(-15 * this.ScaleX), (int)(-21 * this.ScaleY), (int)(31 * this.ScaleX), (int)(44 * this.ScaleY), 1, this));

            if (this.Difficulty == GameTypes.EnemyDifficulty.Miniboss)
            {
                (GetChildAt(0) as SpriteObj).ChangeSprite("GiantPortrait_Sprite");
                this.Scale = new Vector2(2, 2);
                SpriteObj pic = new SpriteObj("Portrait" + CDGMath.RandomInt(0, 7) + "_Sprite");
                pic.OverrideParentScale = true;
                this.AddChild(pic);
                this.CollisionBoxes.Clear();
                this.CollisionBoxes.Add(new CollisionBox(-62 * 2, -88 * 2, 125 * 2, 177 * 2, 2, this));
                this.CollisionBoxes.Add(new CollisionBox(-62 * 2, -88 * 2, 125 * 2, 177 * 2, 1, this));
            }

        }

        private void ThrowProjectiles(LogicSet ls, bool useBossProjectile = false)
        {
            ProjectileData projData = new ProjectileData(this)
            {
                SpriteName = "GhostProjectile_Sprite",
                SourceAnchor = Vector2.Zero,
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0,
                Damage = Damage,
                AngleOffset = 0,
                Angle = new Vector2(0, 0),
                CollidesWithTerrain = false,
                Scale = ProjectileScale,
            };

            //if (this.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
                //projData.SpriteName = "GhostProjectileBoss_Sprite";

            ls.AddAction(new Play3DSoundLogicAction(this,  Game.ScreenManager.Player,"FairyAttack1"));
            projData.Angle = new Vector2(135, 135);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(45, 45);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-45, -45);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-135, -135);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Dispose();
        }

        private void ThrowExpertProjectiles(LogicSet ls, bool useBossProjectile = false)
        {
            ProjectileData projData = new ProjectileData(this)
            {
                SpriteName = "GhostProjectile_Sprite",
                SourceAnchor = Vector2.Zero,
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0,
                Damage = Damage,
                AngleOffset = 0,
                Angle = new Vector2(0, 0),
                CollidesWithTerrain = false,
                Scale = ProjectileScale,
            };

            //if (this.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
            //projData.SpriteName = "GhostProjectileBoss_Sprite";

            ls.AddAction(new Play3DSoundLogicAction(this,  Game.ScreenManager.Player,"FairyAttack1"));
            projData.Angle = new Vector2(0, 0);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(90, 90);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-90, -90);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(180, 180);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Dispose();
        }

        private void ThrowAdvancedProjectiles(LogicSet ls, bool useBossProjectile = false)
        {
            ProjectileData projData = new ProjectileData(this)
            {
                SpriteName = "GhostProjectile_Sprite",
                SourceAnchor = Vector2.Zero,
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0,
                Damage = Damage,
                AngleOffset = 0,
                Angle = new Vector2(0, 0),
                CollidesWithTerrain = false,
                Scale = ProjectileScale,
            };

            //if (this.Difficulty == GameTypes.EnemyDifficulty.MINIBOSS)
            //projData.SpriteName = "GhostProjectileBoss_Sprite";

            ls.AddAction(new Play3DSoundLogicAction(this,  Game.ScreenManager.Player,"FairyAttack1"));
            projData.Angle = new Vector2(90, 90);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-90, -90);
            ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Dispose();
        }

        protected override void RunBasicLogic()
        {
            if (Chasing == false)
            {
                switch (State)
                {
                    case (STATE_MELEE_ENGAGE):
                    case (STATE_PROJECTILE_ENGAGE):
                        ChasePlayer();
                        break;
                    case (STATE_ENGAGE):
                        RunLogicBlock(true, m_generalBasicLB, 100, 0);
                        break;
                    case (STATE_WANDER):
                        break;
                    default:
                        break;
                }
            }
            else
                RunLogicBlock(true, m_generalBasicLB, 0, 100);
        }

        protected override void RunAdvancedLogic()
        {
            if (Chasing == false)
            {
                switch (State)
                {
                    case (STATE_MELEE_ENGAGE):
                    case (STATE_PROJECTILE_ENGAGE):
                        ChasePlayer();
                        break;
                    case (STATE_ENGAGE):
                        RunLogicBlock(true, m_generalAdvancedLB, 100, 0);
                        break;
                    case (STATE_WANDER):
                        break;
                    default:
                        break;
                }
            }
            else
                RunLogicBlock(true, m_generalAdvancedLB, 0, 100);
        }

        protected override void RunExpertLogic()
        {
            if (Chasing == false)
            {
                switch (State)
                {
                    case (STATE_MELEE_ENGAGE):
                    case (STATE_PROJECTILE_ENGAGE):
                        ChasePlayer();
                        break;
                    case (STATE_ENGAGE):
                        RunLogicBlock(true, m_generalExpertLB, 100, 0);
                        break;
                    case (STATE_WANDER):
                        break;
                    default:
                        break;
                }
            }
            else
                RunLogicBlock(true, m_generalExpertLB, 0, 100);
        }

        protected override void RunMinibossLogic()
        {
            if (Chasing == false)
            {
                switch (State)
                {
                    case (STATE_MELEE_ENGAGE):
                    case (STATE_PROJECTILE_ENGAGE):
                        Chasing = true;
                        break;
                    case (STATE_ENGAGE):
                        RunLogicBlock(true, m_generalMiniBossLB, 100, 0);
                        break;
                    case (STATE_WANDER):
                        break;
                    default:
                        break;
                }
            }
            else
                RunLogicBlock(true, m_generalMiniBossLB, 0, 100);
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Chasing == false)
            {
                if (this.Difficulty != GameTypes.EnemyDifficulty.Miniboss)
                {
                    if (Shake == true)
                        this.Rotation = (float)Math.Sin(Game.TotalGameTime * 15) * 2;
                    else
                        this.Rotation = 0;
                }
            }
            else
            {
                if (this.Difficulty == GameTypes.EnemyDifficulty.Miniboss)
                    this.Rotation += (420 * elapsedSeconds);
                //this.Rotation += 7;
                else
                    this.Rotation += (600 * elapsedSeconds);
                    //this.Rotation += 10;

                SpriteObj portrait = this.GetChildAt(0) as SpriteObj;
                if (portrait.SpriteName != "EnemyPortrait" + (int)this.Difficulty + "_Sprite")
                    ChangePortrait();
            }

            base.Update(gameTime);
        }

        public override void HitEnemy(int damage, Vector2 collisionPt, bool isPlayer)
        {
            ChasePlayer();
            base.HitEnemy(damage, collisionPt, isPlayer);
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
        {
            if (otherBox.AbsParent is PlayerObj)
                ChasePlayer();
            base.CollisionResponse(thisBox, otherBox, collisionResponseType);
        }

        public void ChasePlayer()
        {
            if (Chasing == false)
            {
                if (m_currentActiveLB != null)
                    m_currentActiveLB.StopLogicBlock();

                Chasing = true;
                if (m_target.X < this.X)
                    this.Orientation = 0;
                else
                    this.Orientation = MathHelper.ToRadians(180);
            }
        }

        public override void Reset()
        {
            Chasing = false;
            base.Reset();
        }

        public EnemyObj_Portrait(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("EnemyPortrait_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            this.Type = EnemyType.PORTRAIT;
            // Creating the picture frame for the enemy.
            string framePicture = "FramePicture" + CDGMath.RandomInt(1,16) + "_Sprite";
            this.GetChildAt(0).ChangeSprite(framePicture);
            PhysicsObj frame = this.GetChildAt(0) as PhysicsObj;
            this.DisableCollisionBoxRotations = false;
        }

        public void ChangePortrait()
        {
            SoundManager.PlaySound("FinalBoss_St2_BlockLaugh");
            SpriteObj portrait = this.GetChildAt(0) as SpriteObj;
            portrait.ChangeSprite("EnemyPortrait" + (int)this.Difficulty + "_Sprite");
            if (this.Difficulty == GameTypes.EnemyDifficulty.Miniboss)
                this.GetChildAt(1).Visible = false;
        }
    }
}
