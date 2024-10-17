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
    public class EnemyObj_Spark : EnemyObj
    {
        private bool m_hookedToGround = false;
        private byte m_collisionBoxSize = 10; // How large the side collision boxes should be. The faster the spark moves, the faster this should be.

        protected override void InitializeEV()
        {
            LockFlip = true;
            this.IsWeighted = false;

            #region Basic Variables - General
            Name = EnemyEV.SPARK_BASIC_NAME;
            LocStringID = EnemyEV.SPARK_BASIC_NAME_LOC_ID;

            MaxHealth = EnemyEV.SPARK_BASIC_MAX_HEALTH;
            Damage = EnemyEV.SPARK_BASIC_DAMAGE;
            XPValue = EnemyEV.SPARK_BASIC_XP_VALUE;

            MinMoneyDropAmount = EnemyEV.SPARK_BASIC_MIN_DROP_AMOUNT;
            MaxMoneyDropAmount = EnemyEV.SPARK_BASIC_MAX_DROP_AMOUNT;
            MoneyDropChance = EnemyEV.SPARK_BASIC_DROP_CHANCE;

            Speed = EnemyEV.SPARK_BASIC_SPEED;
            TurnSpeed = EnemyEV.SPARK_BASIC_TURN_SPEED;
            ProjectileSpeed = EnemyEV.SPARK_BASIC_PROJECTILE_SPEED;
            JumpHeight = EnemyEV.SPARK_BASIC_JUMP;
            CooldownTime = EnemyEV.SPARK_BASIC_COOLDOWN;
            AnimationDelay = 1 / EnemyEV.SPARK_BASIC_ANIMATION_DELAY;

            AlwaysFaceTarget = EnemyEV.SPARK_BASIC_ALWAYS_FACE_TARGET;
            CanFallOffLedges = EnemyEV.SPARK_BASIC_CAN_FALL_OFF_LEDGES;
            CanBeKnockedBack = EnemyEV.SPARK_BASIC_CAN_BE_KNOCKED_BACK;
            IsWeighted = EnemyEV.SPARK_BASIC_IS_WEIGHTED;

            Scale = EnemyEV.SparkBasicScale;
            ProjectileScale = EnemyEV.SparkBasicProjectileScale;
            TintablePart.TextureColor = EnemyEV.SparkBasicTint;

            MeleeRadius = EnemyEV.SPARK_BASIC_MELEE_RADIUS;
            ProjectileRadius = EnemyEV.SPARK_BASIC_PROJECTILE_RADIUS;
            EngageRadius = EnemyEV.SPARK_BASIC_ENGAGE_RADIUS;

            ProjectileDamage = Damage;
            KnockBack = EnemyEV.SparkBasicKnockBack;
            #endregion

            switch (Difficulty)
            {
                case (GameTypes.EnemyDifficulty.MINIBOSS):
                    #region Miniboss Variables - General
                    Name = EnemyEV.SPARK_MINIBOSS_NAME;
                    LocStringID = EnemyEV.SPARK_MINIBOSS_NAME_LOC_ID;

                    MaxHealth = EnemyEV.SPARK_MINIBOSS_MAX_HEALTH;
                    Damage = EnemyEV.SPARK_MINIBOSS_DAMAGE;
                    XPValue = EnemyEV.SPARK_MINIBOSS_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.SPARK_MINIBOSS_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.SPARK_MINIBOSS_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.SPARK_MINIBOSS_DROP_CHANCE;

                    Speed = EnemyEV.SPARK_MINIBOSS_SPEED;
                    TurnSpeed = EnemyEV.SPARK_MINIBOSS_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.SPARK_MINIBOSS_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.SPARK_MINIBOSS_JUMP;
                    CooldownTime = EnemyEV.SPARK_MINIBOSS_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.SPARK_MINIBOSS_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.SPARK_MINIBOSS_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.SPARK_MINIBOSS_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.SPARK_MINIBOSS_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.SPARK_MINIBOSS_IS_WEIGHTED;

                    Scale = EnemyEV.SparkMinibossScale;
                    ProjectileScale = EnemyEV.SparkMinibossProjectileScale;
                    TintablePart.TextureColor = EnemyEV.SparkMinibossTint;

                    MeleeRadius = EnemyEV.SPARK_MINIBOSS_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.SPARK_MINIBOSS_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.SPARK_MINIBOSS_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.SparkMinibossKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.EXPERT):
                    #region Expert Variables - General
                    Name = EnemyEV.SPARK_EXPERT_NAME;
                    LocStringID = EnemyEV.SPARK_EXPERT_NAME_LOC_ID;

                    MaxHealth = EnemyEV.SPARK_EXPERT_MAX_HEALTH;
                    Damage = EnemyEV.SPARK_EXPERT_DAMAGE;
                    XPValue = EnemyEV.SPARK_EXPERT_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.SPARK_EXPERT_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.SPARK_EXPERT_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.SPARK_EXPERT_DROP_CHANCE;

                    Speed = EnemyEV.SPARK_EXPERT_SPEED;
                    TurnSpeed = EnemyEV.SPARK_EXPERT_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.SPARK_EXPERT_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.SPARK_EXPERT_JUMP;
                    CooldownTime = EnemyEV.SPARK_EXPERT_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.SPARK_EXPERT_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.SPARK_EXPERT_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.SPARK_EXPERT_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.SPARK_EXPERT_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.SPARK_EXPERT_IS_WEIGHTED;

                    Scale = EnemyEV.SparkExpertScale;
                    ProjectileScale = EnemyEV.SparkExpertProjectileScale;
                    TintablePart.TextureColor = EnemyEV.SparkExpertTint;

                    MeleeRadius = EnemyEV.SPARK_EXPERT_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.SPARK_EXPERT_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.SPARK_EXPERT_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.SparkExpertKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.ADVANCED):
                    #region Advanced Variables - General
                    Name = EnemyEV.SPARK_ADVANCED_NAME;
                    LocStringID = EnemyEV.SPARK_ADVANCED_NAME_LOC_ID;

                    MaxHealth = EnemyEV.SPARK_ADVANCED_MAX_HEALTH;
                    Damage = EnemyEV.SPARK_ADVANCED_DAMAGE;
                    XPValue = EnemyEV.SPARK_ADVANCED_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.SPARK_ADVANCED_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.SPARK_ADVANCED_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.SPARK_ADVANCED_DROP_CHANCE;

                    Speed = EnemyEV.SPARK_ADVANCED_SPEED;
                    TurnSpeed = EnemyEV.SPARK_ADVANCED_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.SPARK_ADVANCED_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.SPARK_ADVANCED_JUMP;
                    CooldownTime = EnemyEV.SPARK_ADVANCED_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.SPARK_ADVANCED_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.SPARK_ADVANCED_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.SPARK_ADVANCED_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.SPARK_ADVANCED_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.SPARK_ADVANCED_IS_WEIGHTED;

                    Scale = EnemyEV.SparkAdvancedScale;
                    ProjectileScale = EnemyEV.SparkAdvancedProjectileScale;
                    TintablePart.TextureColor = EnemyEV.SparkAdvancedTint;

                    MeleeRadius = EnemyEV.SPARK_ADVANCED_MELEE_RADIUS;
                    EngageRadius = EnemyEV.SPARK_ADVANCED_ENGAGE_RADIUS;
                    ProjectileRadius = EnemyEV.SPARK_ADVANCED_PROJECTILE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.SparkAdvancedKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.BASIC):
                default:
                    break;
            }			

        }

        protected override void InitializeLogic()
        {

            this.CurrentSpeed = Speed;
            base.InitializeLogic();
        }

        public void HookToGround()
        {
            m_hookedToGround = true;
            float closestGround = 1000;
            TerrainObj closestTerrain = null;
            foreach (TerrainObj terrainObj in m_levelScreen.CurrentRoom.TerrainObjList)
            {
                if (terrainObj.Y >= this.Y)
                {
                    if (terrainObj.Y - this.Y < closestGround && CollisionMath.Intersects(terrainObj.Bounds, new Rectangle((int)this.X, (int)(this.Y + (terrainObj.Y - this.Y) + 5), this.Width, (int)(this.Height / 2))))
                    {
                        closestGround = terrainObj.Y - this.Y;
                        closestTerrain = terrainObj;
                    }
                }
            }

            if (closestTerrain != null)
                //this.Y = closestTerrain.Y - (this.TerrainBounds.Bottom - this.Y - 40);
                this.Y = closestTerrain.Y -(this.Height / 2) + 5;
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
            if (m_hookedToGround == false)
                HookToGround();

            CollisionCheckRight();
            if (IsPaused == false)
                this.Position += this.Heading * (this.CurrentSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);

            base.Update(gameTime);
        }

        private void CollisionCheckRight()
        {
            bool collidesTop = false;
            bool collidesBottom = false;
            bool collidesLeft = false;
            bool collidesRight = false;

            bool collidesTL = false;
            bool collidesTR = false;
            bool collidesBL = false;
            bool collidesBR = false;
            float rotation = 0;

            if (this.Bounds.Right >= m_levelScreen.CurrentRoom.Bounds.Right)
            {
                collidesTR = true;
                collidesRight = true;
                collidesBR = true;
            }
            else if (this.Bounds.Left <= m_levelScreen.CurrentRoom.Bounds.Left)
            {
                collidesTL = true;
                collidesLeft = true;
                collidesBL = true;
            }

            if (this.Bounds.Top <= m_levelScreen.CurrentRoom.Bounds.Top)
            {
                collidesTR = true;
                collidesTop = true;
                collidesTL = true;
            }
            else if (this.Bounds.Bottom >= m_levelScreen.CurrentRoom.Bounds.Bottom)
            {
                collidesBL = true;
                collidesBottom = true;
                collidesBR = true;
            }
               

            foreach (TerrainObj obj in m_levelScreen.CurrentRoom.TerrainObjList)
            {
                Rectangle objAbsRect = new Rectangle((int)obj.X, (int)obj.Y, obj.Width, obj.Height);

                if (CollisionMath.RotatedRectIntersects(TopLeftPoint, 0, Vector2.Zero, objAbsRect, obj.Rotation, Vector2.Zero))
                    collidesTL = true;

                if (CollisionMath.RotatedRectIntersects(TopRightPoint, 0, Vector2.Zero, objAbsRect, obj.Rotation, Vector2.Zero))
                    collidesTR = true;

                if (CollisionMath.RotatedRectIntersects(BottomRightPoint, 0, Vector2.Zero, objAbsRect, obj.Rotation, Vector2.Zero))
                {
                    collidesBR = true;
                    if (obj.Rotation != 0)
                    {
                        Vector2 mtd = CollisionMath.RotatedRectIntersectsMTD(this.BottomRightPoint, 0, Vector2.Zero, objAbsRect, obj.Rotation, Vector2.Zero);
                        if (mtd.X < 0 && mtd.Y < 0)
                            rotation = -45;
                    }
                }

                if (CollisionMath.RotatedRectIntersects(BottomLeftPoint, 0, Vector2.Zero, objAbsRect, obj.Rotation, Vector2.Zero))
                {
                    collidesBL = true;
                    if (obj.Rotation != 0)
                    {
                        Vector2 mtd = CollisionMath.RotatedRectIntersectsMTD(BottomLeftPoint, 0, Vector2.Zero, objAbsRect, obj.Rotation, Vector2.Zero);
                        if (mtd.X > 0 && mtd.Y < 0)
                            rotation = 45;
                    }
                }

                if (CollisionMath.RotatedRectIntersects(TopRect, 0, Vector2.Zero, objAbsRect, obj.Rotation, Vector2.Zero))
                    collidesTop = true;

                if (CollisionMath.RotatedRectIntersects(BottomRect, 0, Vector2.Zero, objAbsRect, obj.Rotation, Vector2.Zero))
                    collidesBottom = true;

                if (CollisionMath.RotatedRectIntersects(LeftRect, 0, Vector2.Zero, objAbsRect, obj.Rotation, Vector2.Zero))
                    collidesLeft = true;

                if (CollisionMath.RotatedRectIntersects(RightRect, 0, Vector2.Zero, objAbsRect, obj.Rotation, Vector2.Zero))
                    collidesRight = true;
            }

            if (collidesBR == true && collidesTR == false && collidesRight == false)
                this.Orientation = 0;

            if ((collidesTR == true && collidesBR == true) && collidesTL == false)
                this.Orientation = MathHelper.ToRadians(-90);

            if ((collidesTR == true && collidesTL == true) && collidesBL == false)
                this.Orientation = MathHelper.ToRadians(-180);

            if (collidesTL == true &&  collidesLeft == true && collidesBottom == false)//collidesBL == true && collidesLeft == true && collidesBottom == false)
                this.Orientation = MathHelper.ToRadians(90);

            // Special cliff cases
            if (collidesTR == true && collidesTop == false && collidesRight == false)
                this.Orientation = MathHelper.ToRadians(-90);

            if (collidesTL == true && collidesTop == false && collidesLeft == false)
                this.Orientation = MathHelper.ToRadians(-180);

            if (collidesBL == true && collidesLeft == false && collidesRight == false && collidesBottom == false)
                this.Orientation = MathHelper.ToRadians(90);

            if (collidesBR == true && collidesBottom == false && collidesRight == false)
                this.Orientation = 0;

            if (rotation != 0)
            {
                if ((rotation < 0 && collidesBR == true && collidesRight == true) || (rotation > 0 && collidesBR == false))
                    this.Orientation = MathHelper.ToRadians(rotation);
            }

            this.HeadingX = (float)Math.Cos(this.Orientation);
            this.HeadingY = (float)Math.Sin(this.Orientation);
        }

        public EnemyObj_Spark(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("EnemySpark_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            IsWeighted = false;
            ForceDraw = true;
            this.Type = EnemyType.Spark;
            this.NonKillable = true;
        }

        //public override void Draw(Camera2D camera)
        //{
        //    base.Draw(camera);
        //    camera.Draw(Game.GenericTexture, TopLeftPoint, Color.Green * 0.5f);
        //    camera.Draw(Game.GenericTexture, TopRightPoint, Color.Green * 0.5f);
        //    camera.Draw(Game.GenericTexture, BottomLeftPoint, Color.Green * 0.5f);
        //    camera.Draw(Game.GenericTexture, BottomRightPoint, Color.Green * 0.5f);

        //    camera.Draw(Game.GenericTexture, LeftRect, Color.Red * 0.5f);
        //    camera.Draw(Game.GenericTexture, RightRect, Color.Red * 0.5f);
        //    camera.Draw(Game.GenericTexture, BottomRect, Color.Red * 0.5f);
        //    camera.Draw(Game.GenericTexture, TopRect, Color.Red * 0.5f);
        //}

        private Rectangle TopRect
        {
            get { return new Rectangle(this.Bounds.Left + m_collisionBoxSize, this.Bounds.Top, this.Width - (m_collisionBoxSize * 2) , m_collisionBoxSize); }
        }

        private Rectangle BottomRect
        {
            get { return new Rectangle(this.Bounds.Left + m_collisionBoxSize, this.Bounds.Bottom - m_collisionBoxSize, this.Width - (m_collisionBoxSize * 2), m_collisionBoxSize); }
        }

        private Rectangle LeftRect
        {
            get { return new Rectangle(this.Bounds.Left, this.Bounds.Top + m_collisionBoxSize, m_collisionBoxSize, this.Height - (m_collisionBoxSize * 2)); }
        }

        private Rectangle RightRect
        {
            get { return new Rectangle(this.Bounds.Right - m_collisionBoxSize, this.Bounds.Top + m_collisionBoxSize, m_collisionBoxSize, this.Height - (m_collisionBoxSize * 2)); }
        }


        private Rectangle TopLeftPoint
        {
            get { return new Rectangle(this.Bounds.Left, this.Bounds.Top, m_collisionBoxSize, m_collisionBoxSize); }
        }

        private Rectangle TopRightPoint
        {
            get { return new Rectangle(this.Bounds.Right - m_collisionBoxSize, this.Bounds.Top, m_collisionBoxSize, m_collisionBoxSize); }
        }

        private Rectangle BottomLeftPoint
        {
            get { return new Rectangle(this.Bounds.Left, this.Bounds.Bottom - m_collisionBoxSize, m_collisionBoxSize, m_collisionBoxSize); }
        }

        private Rectangle BottomRightPoint
        {
            get { return new Rectangle(this.Bounds.Right - m_collisionBoxSize, this.Bounds.Bottom - m_collisionBoxSize, m_collisionBoxSize, m_collisionBoxSize); }
        }
    }
}
