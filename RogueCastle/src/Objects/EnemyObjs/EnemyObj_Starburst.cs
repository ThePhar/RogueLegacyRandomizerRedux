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
    public class EnemyObj_Starburst : EnemyObj
    {
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();
        private LogicBlock m_generalMiniBossLB = new LogicBlock();

        private float FireballDelay = 0.5f;

        protected override void InitializeEV()
        {
            #region Basic Variables - General
            Name = EnemyEV.STARBURST_BASIC_NAME;
            LocStringID = EnemyEV.STARBURST_BASIC_NAME_LOC_ID;

            MaxHealth = EnemyEV.STARBURST_BASIC_MAX_HEALTH;
            Damage = EnemyEV.STARBURST_BASIC_DAMAGE;
            XPValue = EnemyEV.STARBURST_BASIC_XP_VALUE;

            MinMoneyDropAmount = EnemyEV.STARBURST_BASIC_MIN_DROP_AMOUNT;
            MaxMoneyDropAmount = EnemyEV.STARBURST_BASIC_MAX_DROP_AMOUNT;
            MoneyDropChance = EnemyEV.STARBURST_BASIC_DROP_CHANCE;

            Speed = EnemyEV.STARBURST_BASIC_SPEED;
            TurnSpeed = EnemyEV.STARBURST_BASIC_TURN_SPEED;
            ProjectileSpeed = EnemyEV.STARBURST_BASIC_PROJECTILE_SPEED;
            JumpHeight = EnemyEV.STARBURST_BASIC_JUMP;
            CooldownTime = EnemyEV.STARBURST_BASIC_COOLDOWN;
            AnimationDelay = 1 / EnemyEV.STARBURST_BASIC_ANIMATION_DELAY;

            AlwaysFaceTarget = EnemyEV.STARBURST_BASIC_ALWAYS_FACE_TARGET;
            CanFallOffLedges = EnemyEV.STARBURST_BASIC_CAN_FALL_OFF_LEDGES;
            CanBeKnockedBack = EnemyEV.STARBURST_BASIC_CAN_BE_KNOCKED_BACK;
            IsWeighted = EnemyEV.STARBURST_BASIC_IS_WEIGHTED;

            Scale = EnemyEV.StarburstBasicScale;
            ProjectileScale = EnemyEV.StarburstBasicProjectileScale;
            TintablePart.TextureColor = EnemyEV.StarburstBasicTint;

            MeleeRadius = EnemyEV.STARBURST_BASIC_MELEE_RADIUS;
            ProjectileRadius = EnemyEV.STARBURST_BASIC_PROJECTILE_RADIUS;
            EngageRadius = EnemyEV.STARBURST_BASIC_ENGAGE_RADIUS;

            ProjectileDamage = Damage;
            KnockBack = EnemyEV.StarburstBasicKnockBack;
            #endregion

            switch (Difficulty)
            {
                case (GameTypes.EnemyDifficulty.MINIBOSS):
                    #region Miniboss Variables - General
                    Name = EnemyEV.STARBURST_MINIBOSS_NAME;
                    LocStringID = EnemyEV.STARBURST_MINIBOSS_NAME_LOC_ID;

                    MaxHealth = EnemyEV.STARBURST_MINIBOSS_MAX_HEALTH;
                    Damage = EnemyEV.STARBURST_MINIBOSS_DAMAGE;
                    XPValue = EnemyEV.STARBURST_MINIBOSS_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.STARBURST_MINIBOSS_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.STARBURST_MINIBOSS_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.STARBURST_MINIBOSS_DROP_CHANCE;

                    Speed = EnemyEV.STARBURST_MINIBOSS_SPEED;
                    TurnSpeed = EnemyEV.STARBURST_MINIBOSS_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.STARBURST_MINIBOSS_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.STARBURST_MINIBOSS_JUMP;
                    CooldownTime = EnemyEV.STARBURST_MINIBOSS_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.STARBURST_MINIBOSS_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.STARBURST_MINIBOSS_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.STARBURST_MINIBOSS_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.STARBURST_MINIBOSS_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.STARBURST_MINIBOSS_IS_WEIGHTED;

                    Scale = EnemyEV.StarburstMinibossScale;
                    ProjectileScale = EnemyEV.StarburstMinibossProjectileScale;
                    TintablePart.TextureColor = EnemyEV.StarburstMinibossTint;

                    MeleeRadius = EnemyEV.STARBURST_MINIBOSS_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.STARBURST_MINIBOSS_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.STARBURST_MINIBOSS_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.StarburstMinibossKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.EXPERT):
                    #region Expert Variables - General
                    Name = EnemyEV.STARBURST_EXPERT_NAME;
                    LocStringID = EnemyEV.STARBURST_EXPERT_NAME_LOC_ID;

                    MaxHealth = EnemyEV.STARBURST_EXPERT_MAX_HEALTH;
                    Damage = EnemyEV.STARBURST_EXPERT_DAMAGE;
                    XPValue = EnemyEV.STARBURST_EXPERT_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.STARBURST_EXPERT_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.STARBURST_EXPERT_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.STARBURST_EXPERT_DROP_CHANCE;

                    Speed = EnemyEV.STARBURST_EXPERT_SPEED;
                    TurnSpeed = EnemyEV.STARBURST_EXPERT_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.STARBURST_EXPERT_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.STARBURST_EXPERT_JUMP;
                    CooldownTime = EnemyEV.STARBURST_EXPERT_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.STARBURST_EXPERT_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.STARBURST_EXPERT_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.STARBURST_EXPERT_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.STARBURST_EXPERT_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.STARBURST_EXPERT_IS_WEIGHTED;

                    Scale = EnemyEV.StarburstExpertScale;
                    ProjectileScale = EnemyEV.StarburstExpertProjectileScale;
                    TintablePart.TextureColor = EnemyEV.StarburstExpertTint;

                    MeleeRadius = EnemyEV.STARBURST_EXPERT_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.STARBURST_EXPERT_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.STARBURST_EXPERT_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.StarburstExpertKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.ADVANCED):
                    #region Advanced Variables - General
                    Name = EnemyEV.STARBURST_ADVANCED_NAME;
                    LocStringID = EnemyEV.STARBURST_ADVANCED_NAME_LOC_ID;

                    MaxHealth = EnemyEV.STARBURST_ADVANCED_MAX_HEALTH;
                    Damage = EnemyEV.STARBURST_ADVANCED_DAMAGE;
                    XPValue = EnemyEV.STARBURST_ADVANCED_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.STARBURST_ADVANCED_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.STARBURST_ADVANCED_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.STARBURST_ADVANCED_DROP_CHANCE;

                    Speed = EnemyEV.STARBURST_ADVANCED_SPEED;
                    TurnSpeed = EnemyEV.STARBURST_ADVANCED_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.STARBURST_ADVANCED_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.STARBURST_ADVANCED_JUMP;
                    CooldownTime = EnemyEV.STARBURST_ADVANCED_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.STARBURST_ADVANCED_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.STARBURST_ADVANCED_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.STARBURST_ADVANCED_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.STARBURST_ADVANCED_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.STARBURST_ADVANCED_IS_WEIGHTED;

                    Scale = EnemyEV.StarburstAdvancedScale;
                    ProjectileScale = EnemyEV.StarburstAdvancedProjectileScale;
                    TintablePart.TextureColor = EnemyEV.StarburstAdvancedTint;

                    MeleeRadius = EnemyEV.STARBURST_ADVANCED_MELEE_RADIUS;
                    EngageRadius = EnemyEV.STARBURST_ADVANCED_ENGAGE_RADIUS;
                    ProjectileRadius = EnemyEV.STARBURST_ADVANCED_PROJECTILE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.StarburstAdvancedKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.BASIC):
                default:
                    break;
            }			

        }

        protected override void InitializeLogic()
        {
            ProjectileData projData = new ProjectileData(this)
            {
                SpriteName = "TurretProjectile_Sprite",
                SourceAnchor = Vector2.Zero,
                Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0,
                Damage = Damage,
                AngleOffset = 0,
                CollidesWithTerrain = true, //false
                Scale = ProjectileScale,
            };

            LogicSet fireProjectileBasicLS = new LogicSet(this);
            projData.Angle = new Vector2(0, 0);
            fireProjectileBasicLS.AddAction(new RunFunctionLogicAction(this, "FireAnimation"));
            fireProjectileBasicLS.AddAction(new DelayLogicAction(FireballDelay));
            fireProjectileBasicLS.AddAction(new Play3DSoundLogicAction(this,  Game.ScreenManager.Player,"Eyeball_ProjectileAttack"));
            fireProjectileBasicLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-90, -90);
            fireProjectileBasicLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(90, 90);
            fireProjectileBasicLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(180, 180);
            fireProjectileBasicLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            fireProjectileBasicLS.AddAction(new ChangeSpriteLogicAction("EnemyStarburstIdle_Character", true, true));
            fireProjectileBasicLS.AddAction(new DelayLogicAction(1.0f, 1.0f));
            fireProjectileBasicLS.Tag = GameTypes.LogicSetType_ATTACK;

            LogicSet fireProjectileAdvancedLS = new LogicSet(this);
            projData.Angle = new Vector2(45, 45);
            fireProjectileAdvancedLS.AddAction(new ChangePropertyLogicAction(_objectList[1], "Rotation", 45));
            fireProjectileAdvancedLS.AddAction(new RunFunctionLogicAction(this, "FireAnimation"));
            fireProjectileAdvancedLS.AddAction(new ChangePropertyLogicAction(_objectList[1], "Rotation", 45));
            fireProjectileAdvancedLS.AddAction(new DelayLogicAction(FireballDelay));
            fireProjectileAdvancedLS.AddAction(new Play3DSoundLogicAction(this,  Game.ScreenManager.Player,"Eyeball_ProjectileAttack"));
            fireProjectileAdvancedLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-45, -45);
            fireProjectileAdvancedLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(135, 135);
            fireProjectileAdvancedLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-135, -135);
            fireProjectileAdvancedLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));       
            projData.Angle = new Vector2(-90, -90);
            fireProjectileAdvancedLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(90, 90);
            fireProjectileAdvancedLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(180, 180);
            fireProjectileAdvancedLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(0, 0);
            fireProjectileAdvancedLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            fireProjectileAdvancedLS.AddAction(new ChangeSpriteLogicAction("EnemyStarburstIdle_Character", true, true));
            fireProjectileAdvancedLS.AddAction(new ChangePropertyLogicAction(_objectList[1], "Rotation", 45));
            fireProjectileAdvancedLS.AddAction(new DelayLogicAction(1.0f, 1.0f));
            fireProjectileAdvancedLS.Tag = GameTypes.LogicSetType_ATTACK;

            //TEDDY - Just like Advanced projetiles but made so the bullets go through terrain.
            #region EXPERT
            LogicSet fireProjectileExpertLS = new LogicSet(this);
            projData.Angle = new Vector2(45, 45);
            projData.CollidesWithTerrain = false;
            projData.SpriteName = "GhostProjectile_Sprite";
            fireProjectileExpertLS.AddAction(new ChangePropertyLogicAction(_objectList[1], "Rotation", 45));
            fireProjectileExpertLS.AddAction(new RunFunctionLogicAction(this, "FireAnimation"));
            fireProjectileExpertLS.AddAction(new ChangePropertyLogicAction(_objectList[1], "Rotation", 45));
            fireProjectileExpertLS.AddAction(new DelayLogicAction(FireballDelay));
            fireProjectileExpertLS.AddAction(new Play3DSoundLogicAction(this,  Game.ScreenManager.Player,"Eyeball_ProjectileAttack"));
            fireProjectileExpertLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-45, -45);
            fireProjectileExpertLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(135, 135);
            fireProjectileExpertLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-135, -135);
            fireProjectileExpertLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-90, -90);
            fireProjectileExpertLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(90, 90);
            fireProjectileExpertLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(180, 180);
            fireProjectileExpertLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(0, 0);
            fireProjectileExpertLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            fireProjectileExpertLS.AddAction(new ChangeSpriteLogicAction("EnemyStarburstIdle_Character", true, true));
            fireProjectileExpertLS.AddAction(new DelayLogicAction(1.0f, 1.0f));

            fireProjectileExpertLS.AddAction(new RunFunctionLogicAction(this, "FireAnimation"));
            fireProjectileExpertLS.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "Eyeball_ProjectileAttack"));
            projData.Angle = new Vector2(25, 25);
            fireProjectileExpertLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-25, -25);
            fireProjectileExpertLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(115, 115);
            fireProjectileExpertLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-115, -115);
            fireProjectileExpertLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-70, -70);
            fireProjectileExpertLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(70, 70);
            fireProjectileExpertLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(160, 160);
            fireProjectileExpertLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-160, -160);
            fireProjectileExpertLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            fireProjectileExpertLS.AddAction(new ChangeSpriteLogicAction("EnemyStarburstIdle_Character", true, true));
            fireProjectileExpertLS.AddAction(new ChangePropertyLogicAction(_objectList[1], "Rotation", 45));
            fireProjectileExpertLS.AddAction(new DelayLogicAction(1.25f, 1.25f));
            fireProjectileExpertLS.Tag = GameTypes.LogicSetType_ATTACK;

            #endregion


            LogicSet doNothing = new LogicSet(this);
            doNothing.AddAction(new DelayLogicAction(0.5f, 0.5f));

            m_generalBasicLB.AddLogicSet(fireProjectileBasicLS, doNothing);
            m_generalAdvancedLB.AddLogicSet(fireProjectileAdvancedLS, doNothing);
            m_generalExpertLB.AddLogicSet(fireProjectileExpertLS, doNothing);
            m_generalMiniBossLB.AddLogicSet(fireProjectileAdvancedLS, doNothing);

            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalMiniBossLB);

            projData.Dispose();

            base.InitializeLogic();
        }

        public void FireAnimation()
        {
            this.ChangeSprite("EnemyStarburstAttack_Character");
            (_objectList[0] as IAnimateableObj).PlayAnimation(true);
            (_objectList[1] as IAnimateableObj).PlayAnimation(false);
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case (STATE_MELEE_ENGAGE):
                case (STATE_PROJECTILE_ENGAGE):
                case (STATE_ENGAGE):
                    RunLogicBlock(true, m_generalBasicLB, 100, 0);
                    break;
                case (STATE_WANDER):
                    RunLogicBlock(true, m_generalBasicLB, 0, 100);
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
                    RunLogicBlock(true, m_generalAdvancedLB, 100, 0);
                    break;
                case (STATE_WANDER):
                    RunLogicBlock(true, m_generalAdvancedLB, 0, 100);
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
                    RunLogicBlock(true, m_generalExpertLB, 100, 0);
                    break;
                case (STATE_WANDER):
                    RunLogicBlock(true, m_generalExpertLB, 0, 100);
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
                    RunLogicBlock(true, m_generalMiniBossLB, 60, 40, 0);
                    break;
                default:
                    break;
            }
        }

        public EnemyObj_Starburst(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("EnemyStarburstIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            this.Type = EnemyType.STARBURST;
        }

    }
}
