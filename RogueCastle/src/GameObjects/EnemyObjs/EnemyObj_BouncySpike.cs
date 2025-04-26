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
    public class EnemyObj_BouncySpike : EnemyObj
    {
        private float RotationSpeed = 250;//600;
        private float m_internalOrientation;

        public Vector2 SavedStartingPos { get; set; }
        public RoomObj SpawnRoom;

        private float m_selfDestructTimer = 0.7f;//1;
        private int m_selfDestructCounter = 0;
        private int m_selfDestructTotalBounces = 12;//14;//11;//7; //Total bounce needed in 1 second to destroy a spike.

        protected override void InitializeEV()
        {
            //this.Orientation = CDGMath.RandomInt(-180, 180);
            int randomizer = CDGMath.RandomInt(0,11);
            if (randomizer >= 9)
                this.Orientation = 0;
            else if (randomizer >= 6)
                this.Orientation = 180;
            else if (randomizer >= 4)
                this.Orientation = 90;
            else if (randomizer >= 1)
                this.Orientation = 270;
            else
                this.Orientation = 45;

            m_internalOrientation = this.Orientation;

            this.HeadingX = (float)Math.Cos(MathHelper.ToRadians(this.Orientation));
            this.HeadingY = (float)Math.Sin(MathHelper.ToRadians(this.Orientation));

            #region Basic Variables - General
            Name = EnemyEV.BOUNCY_SPIKE_BASIC_NAME;
            LocStringID = EnemyEV.BOUNCY_SPIKE_BASIC_NAME_LOC_ID;

            MaxHealth = EnemyEV.BOUNCY_SPIKE_BASIC_MAX_HEALTH;
            Damage = EnemyEV.BOUNCY_SPIKE_BASIC_DAMAGE;
            XPValue = EnemyEV.BOUNCY_SPIKE_BASIC_XP_VALUE;

            MinMoneyDropAmount = EnemyEV.BOUNCY_SPIKE_BASIC_MIN_DROP_AMOUNT;
            MaxMoneyDropAmount = EnemyEV.BOUNCY_SPIKE_BASIC_MAX_DROP_AMOUNT;
            MoneyDropChance = EnemyEV.BOUNCY_SPIKE_BASIC_DROP_CHANCE;

            Speed = EnemyEV.BOUNCY_SPIKE_BASIC_SPEED;
            TurnSpeed = EnemyEV.BOUNCY_SPIKE_BASIC_TURN_SPEED;
            ProjectileSpeed = EnemyEV.BOUNCY_SPIKE_BASIC_PROJECTILE_SPEED;
            JumpHeight = EnemyEV.BOUNCY_SPIKE_BASIC_JUMP;
            CooldownTime = EnemyEV.BOUNCY_SPIKE_BASIC_COOLDOWN;
            AnimationDelay = 1 / EnemyEV.BOUNCY_SPIKE_BASIC_ANIMATION_DELAY;

            AlwaysFaceTarget = EnemyEV.BOUNCY_SPIKE_BASIC_ALWAYS_FACE_TARGET;
            CanFallOffLedges = EnemyEV.BOUNCY_SPIKE_BASIC_CAN_FALL_OFF_LEDGES;
            CanBeKnockedBack = EnemyEV.BOUNCY_SPIKE_BASIC_CAN_BE_KNOCKED_BACK;
            IsWeighted = EnemyEV.BOUNCY_SPIKE_BASIC_IS_WEIGHTED;

            Scale = EnemyEV.BouncySpikeBasicScale;
            ProjectileScale = EnemyEV.BouncySpikeBasicProjectileScale;
            TintablePart.TextureColor = EnemyEV.BouncySpikeBasicTint;

            MeleeRadius = EnemyEV.BOUNCY_SPIKE_BASIC_MELEE_RADIUS;
            ProjectileRadius = EnemyEV.BOUNCY_SPIKE_BASIC_PROJECTILE_RADIUS;
            EngageRadius = EnemyEV.BOUNCY_SPIKE_BASIC_ENGAGE_RADIUS;

            ProjectileDamage = Damage;
            KnockBack = new Vector2(1, 2);
            LockFlip = true;
            #endregion

            switch (Difficulty)
            {
                case (GameTypes.EnemyDifficulty.Miniboss):
                    #region Miniboss Variables - General
                    Name = EnemyEV.BOUNCY_SPIKE_MINIBOSS_NAME;
                    LocStringID = EnemyEV.BOUNCY_SPIKE_MINIBOSS_NAME_LOC_ID;

                    MaxHealth = EnemyEV.BOUNCY_SPIKE_MINIBOSS_MAX_HEALTH;
                    Damage = EnemyEV.BOUNCY_SPIKE_MINIBOSS_DAMAGE;
                    XPValue = EnemyEV.BOUNCY_SPIKE_MINIBOSS_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.BOUNCY_SPIKE_MINIBOSS_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.BOUNCY_SPIKE_MINIBOSS_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.BOUNCY_SPIKE_MINIBOSS_DROP_CHANCE;

                    Speed = EnemyEV.BOUNCY_SPIKE_MINIBOSS_SPEED;
                    TurnSpeed = EnemyEV.BOUNCY_SPIKE_MINIBOSS_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.BOUNCY_SPIKE_MINIBOSS_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.BOUNCY_SPIKE_MINIBOSS_JUMP;
                    CooldownTime = EnemyEV.BOUNCY_SPIKE_MINIBOSS_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.BOUNCY_SPIKE_MINIBOSS_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.BOUNCY_SPIKE_MINIBOSS_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.BOUNCY_SPIKE_MINIBOSS_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.BOUNCY_SPIKE_MINIBOSS_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.BOUNCY_SPIKE_MINIBOSS_IS_WEIGHTED;

                    Scale = EnemyEV.BouncySpikeMinibossScale;
                    ProjectileScale = EnemyEV.BouncySpikeMinibossProjectileScale;
                    TintablePart.TextureColor = EnemyEV.BouncySpikeMinibossTint;

                    MeleeRadius = EnemyEV.BOUNCY_SPIKE_MINIBOSS_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.BOUNCY_SPIKE_MINIBOSS_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.BOUNCY_SPIKE_MINIBOSS_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = new Vector2(1, 2);
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Expert):
                    #region Expert Variables - General
                    Name = EnemyEV.BOUNCY_SPIKE_EXPERT_NAME;
                    LocStringID = EnemyEV.BOUNCY_SPIKE_EXPERT_NAME_LOC_ID;

                    MaxHealth = EnemyEV.BOUNCY_SPIKE_EXPERT_MAX_HEALTH;
                    Damage = EnemyEV.BOUNCY_SPIKE_EXPERT_DAMAGE;
                    XPValue = EnemyEV.BOUNCY_SPIKE_EXPERT_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.BOUNCY_SPIKE_EXPERT_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.BOUNCY_SPIKE_EXPERT_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.BOUNCY_SPIKE_EXPERT_DROP_CHANCE;

                    Speed = EnemyEV.BOUNCY_SPIKE_EXPERT_SPEED;
                    TurnSpeed = EnemyEV.BOUNCY_SPIKE_EXPERT_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.BOUNCY_SPIKE_EXPERT_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.BOUNCY_SPIKE_EXPERT_JUMP;
                    CooldownTime = EnemyEV.BOUNCY_SPIKE_EXPERT_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.BOUNCY_SPIKE_EXPERT_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.BOUNCY_SPIKE_EXPERT_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.BOUNCY_SPIKE_EXPERT_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.BOUNCY_SPIKE_EXPERT_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.BOUNCY_SPIKE_EXPERT_IS_WEIGHTED;

                    Scale = EnemyEV.BouncySpikeExpertScale;
                    ProjectileScale = EnemyEV.BouncySpikeExpertProjectileScale;
                    TintablePart.TextureColor = EnemyEV.BouncySpikeExpertTint;

                    MeleeRadius = EnemyEV.BOUNCY_SPIKE_EXPERT_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.BOUNCY_SPIKE_EXPERT_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.BOUNCY_SPIKE_EXPERT_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = new Vector2(1, 2);
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Advanced):
                    #region Advanced Variables - General
                    Name = EnemyEV.BOUNCY_SPIKE_ADVANCED_NAME;
                    LocStringID = EnemyEV.BOUNCY_SPIKE_ADVANCED_NAME_LOC_ID;

                    MaxHealth = EnemyEV.BOUNCY_SPIKE_ADVANCED_MAX_HEALTH;
                    Damage = EnemyEV.BOUNCY_SPIKE_ADVANCED_DAMAGE;
                    XPValue = EnemyEV.BOUNCY_SPIKE_ADVANCED_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.BOUNCY_SPIKE_ADVANCED_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.BOUNCY_SPIKE_ADVANCED_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.BOUNCY_SPIKE_ADVANCED_DROP_CHANCE;

                    Speed = EnemyEV.BOUNCY_SPIKE_ADVANCED_SPEED;
                    TurnSpeed = EnemyEV.BOUNCY_SPIKE_ADVANCED_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.BOUNCY_SPIKE_ADVANCED_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.BOUNCY_SPIKE_ADVANCED_JUMP;
                    CooldownTime = EnemyEV.BOUNCY_SPIKE_ADVANCED_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.BOUNCY_SPIKE_ADVANCED_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.BOUNCY_SPIKE_ADVANCED_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.BOUNCY_SPIKE_ADVANCED_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.BOUNCY_SPIKE_ADVANCED_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.BOUNCY_SPIKE_ADVANCED_IS_WEIGHTED;

                    Scale = EnemyEV.BouncySpikeAdvancedScale;
                    ProjectileScale = EnemyEV.BouncySpikeAdvancedProjectileScale;
                    TintablePart.TextureColor = EnemyEV.BouncySpikeAdvancedTint;

                    MeleeRadius = EnemyEV.BOUNCY_SPIKE_ADVANCED_MELEE_RADIUS;
                    EngageRadius = EnemyEV.BOUNCY_SPIKE_ADVANCED_ENGAGE_RADIUS;
                    ProjectileRadius = EnemyEV.BOUNCY_SPIKE_ADVANCED_PROJECTILE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = new Vector2(1, 2);
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Basic):
                default:
                    break;
            }		

        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case (STATE_MELEE_ENGAGE):
                case (STATE_PROJECTILE_ENGAGE):
                case (STATE_ENGAGE):
                case (STATE_WANDER):
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

        public override void Update(GameTime gameTime)
        {
            if (IsPaused == false)
            {
                float elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Checking for boundary bounces.
                Vector2 boundaryMTD = Vector2.Zero;
                Rectangle roomBounds = m_levelScreen.CurrentRoom.Bounds;

                if (this.Y < roomBounds.Top + 10)
                    boundaryMTD = CollisionMath.CalculateMTD(this.Bounds, new Rectangle(roomBounds.Left, roomBounds.Top, roomBounds.Width, 10));
                else if (this.Y > roomBounds.Bottom - 10)
                    boundaryMTD = CollisionMath.CalculateMTD(this.Bounds, new Rectangle(roomBounds.Left, roomBounds.Bottom - 10, roomBounds.Width, 10));

                if (this.X > roomBounds.Right - 10)
                    boundaryMTD = CollisionMath.CalculateMTD(this.Bounds, new Rectangle(roomBounds.Right - 10, roomBounds.Top, 10, roomBounds.Height));
                else if (this.X < roomBounds.Left + 10)
                    boundaryMTD = CollisionMath.CalculateMTD(this.Bounds, new Rectangle(roomBounds.Left, roomBounds.Top, 10, roomBounds.Height));

                if (boundaryMTD != Vector2.Zero)
                {
                    Vector2 v = Heading;
                    Vector2 l = new Vector2(boundaryMTD.Y, boundaryMTD.X * -1); // The angle of the side the vector hit is the normal to the MTD.
                    Vector2 newHeading = ((2 * (CDGMath.DotProduct(v, l) / CDGMath.DotProduct(l, l)) * l) - v);
                    this.Heading = newHeading;
                    SoundManager.Play3DSound(this, Game.ScreenManager.Player, "GiantSpike_Bounce_01", "GiantSpike_Bounce_02", "GiantSpike_Bounce_03");
                    m_selfDestructCounter++;
                    m_selfDestructTimer = 1;
                }

                if (m_selfDestructTimer > 0)
                {
                    m_selfDestructTimer -= elapsedTime;
                    if (m_selfDestructTimer <= 0)
                        m_selfDestructCounter = 0;
                }

                if (m_selfDestructCounter >= m_selfDestructTotalBounces)
                    this.Kill(false);

                if (CurrentSpeed == 0)
                    CurrentSpeed = Speed;

                if (this.HeadingX > 0)
                    this.Rotation += RotationSpeed * elapsedTime;
                else
                    this.Rotation -= RotationSpeed * elapsedTime;
            }
            base.Update(gameTime);
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
        {
            TerrainObj terrain = otherBox.Parent as TerrainObj;

            if (terrain != null && (terrain is DoorObj) == false)
            {
                if (terrain.CollidesBottom == true && terrain.CollidesLeft == true && terrain.CollidesRight == true && terrain.CollidesTop == true)
                {
                    Vector2 mtd = CollisionMath.RotatedRectIntersectsMTD(thisBox.AbsRect, (int)thisBox.AbsRotation, Vector2.Zero, otherBox.AbsRect, (int)otherBox.AbsRotation, Vector2.Zero);
                    if (mtd != Vector2.Zero)
                    {
                        Vector2 v = Heading;
                        Vector2 l = new Vector2(mtd.Y, mtd.X * -1); // The angle of the side the vector hit is the normal to the MTD.
                        Vector2 newHeading = ((2 * (CDGMath.DotProduct(v, l) / CDGMath.DotProduct(l, l)) * l) - v);
                        this.X += mtd.X;
                        this.Y += mtd.Y;
                        this.Heading = newHeading;
                        SoundManager.Play3DSound(this, Game.ScreenManager.Player,"GiantSpike_Bounce_01", "GiantSpike_Bounce_02", "GiantSpike_Bounce_03");
                        m_selfDestructCounter++;
                        m_selfDestructTimer = 1;
                    }
                }
            }
        }

        public override void Reset()
        {
            if (SpawnRoom != null)
            {
                m_levelScreen.RemoveEnemyFromRoom(this, SpawnRoom, this.SavedStartingPos);
                this.Dispose();
            }
            else
            {
                this.Orientation = m_internalOrientation;
                base.Reset();
            }
        }

        public EnemyObj_BouncySpike(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("EnemyBouncySpike_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            this.Type = EnemyType.BOUNCY_SPIKE;
            NonKillable = true;
        }

        public override void Dispose()
        {
            if (IsDisposed == false)
            {
                // Done
                SpawnRoom = null;
                base.Dispose();
            }
        }
    }
}
