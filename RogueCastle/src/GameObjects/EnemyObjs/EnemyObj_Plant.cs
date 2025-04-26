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
    public class EnemyObj_Plant : EnemyObj
    {
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();
        private LogicBlock m_generalMiniBossLB = new LogicBlock();
        private LogicBlock m_generalCooldownLB = new LogicBlock();
        private LogicBlock m_generalCooldownExpertLB = new LogicBlock();

        protected override void InitializeEV()
        {
            #region Basic Variables - General
            Name = EnemyEV.PLANT_BASIC_NAME;
            LocStringID = EnemyEV.PLANT_BASIC_NAME_LOC_ID;

            MaxHealth = EnemyEV.PLANT_BASIC_MAX_HEALTH;
            Damage = EnemyEV.PLANT_BASIC_DAMAGE;
            XPValue = EnemyEV.PLANT_BASIC_XP_VALUE;

            MinMoneyDropAmount = EnemyEV.PLANT_BASIC_MIN_DROP_AMOUNT;
            MaxMoneyDropAmount = EnemyEV.PLANT_BASIC_MAX_DROP_AMOUNT;
            MoneyDropChance = EnemyEV.PLANT_BASIC_DROP_CHANCE;

            Speed = EnemyEV.PLANT_BASIC_SPEED;
            TurnSpeed = EnemyEV.PLANT_BASIC_TURN_SPEED;
            ProjectileSpeed = EnemyEV.PLANT_BASIC_PROJECTILE_SPEED;
            JumpHeight = EnemyEV.PLANT_BASIC_JUMP;
            CooldownTime = EnemyEV.PLANT_BASIC_COOLDOWN;
            AnimationDelay = 1 / EnemyEV.PLANT_BASIC_ANIMATION_DELAY;

            AlwaysFaceTarget = EnemyEV.PLANT_BASIC_ALWAYS_FACE_TARGET;
            CanFallOffLedges = EnemyEV.PLANT_BASIC_CAN_FALL_OFF_LEDGES;
            CanBeKnockedBack = EnemyEV.PLANT_BASIC_CAN_BE_KNOCKED_BACK;
            IsWeighted = EnemyEV.PLANT_BASIC_IS_WEIGHTED;

            Scale = EnemyEV.PlantBasicScale;
            ProjectileScale = EnemyEV.PlantBasicProjectileScale;
            TintablePart.TextureColor = EnemyEV.PlantBasicTint;

            MeleeRadius = EnemyEV.PLANT_BASIC_MELEE_RADIUS;
            ProjectileRadius = EnemyEV.PLANT_BASIC_PROJECTILE_RADIUS;
            EngageRadius = EnemyEV.PLANT_BASIC_ENGAGE_RADIUS;

            ProjectileDamage = Damage;
            KnockBack = EnemyEV.PlantBasicKnockBack;
            #endregion

            switch (Difficulty)
            {
                case (GameTypes.EnemyDifficulty.Miniboss):
                    #region Miniboss Variables - General
                    Name = EnemyEV.PLANT_MINIBOSS_NAME;
                    LocStringID = EnemyEV.PLANT_MINIBOSS_NAME_LOC_ID;

                    MaxHealth = EnemyEV.PLANT_MINIBOSS_MAX_HEALTH;
                    Damage = EnemyEV.PLANT_MINIBOSS_DAMAGE;
                    XPValue = EnemyEV.PLANT_MINIBOSS_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.PLANT_MINIBOSS_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.PLANT_MINIBOSS_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.PLANT_MINIBOSS_DROP_CHANCE;

                    Speed = EnemyEV.PLANT_MINIBOSS_SPEED;
                    TurnSpeed = EnemyEV.PLANT_MINIBOSS_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.PLANT_MINIBOSS_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.PLANT_MINIBOSS_JUMP;
                    CooldownTime = EnemyEV.PLANT_MINIBOSS_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.PLANT_MINIBOSS_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.PLANT_MINIBOSS_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.PLANT_MINIBOSS_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.PLANT_MINIBOSS_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.PLANT_MINIBOSS_IS_WEIGHTED;

                    Scale = EnemyEV.PlantMinibossScale;
                    ProjectileScale = EnemyEV.PlantMinibossProjectileScale;
                    TintablePart.TextureColor = EnemyEV.PlantMinibossTint;

                    MeleeRadius = EnemyEV.PLANT_MINIBOSS_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.PLANT_MINIBOSS_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.PLANT_MINIBOSS_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.PlantMinibossKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Expert):
                    #region Expert Variables - General
                    Name = EnemyEV.PLANT_EXPERT_NAME;
                    LocStringID = EnemyEV.PLANT_EXPERT_NAME_LOC_ID;

                    MaxHealth = EnemyEV.PLANT_EXPERT_MAX_HEALTH;
                    Damage = EnemyEV.PLANT_EXPERT_DAMAGE;
                    XPValue = EnemyEV.PLANT_EXPERT_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.PLANT_EXPERT_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.PLANT_EXPERT_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.PLANT_EXPERT_DROP_CHANCE;

                    Speed = EnemyEV.PLANT_EXPERT_SPEED;
                    TurnSpeed = EnemyEV.PLANT_EXPERT_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.PLANT_EXPERT_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.PLANT_EXPERT_JUMP;
                    CooldownTime = EnemyEV.PLANT_EXPERT_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.PLANT_EXPERT_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.PLANT_EXPERT_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.PLANT_EXPERT_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.PLANT_EXPERT_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.PLANT_EXPERT_IS_WEIGHTED;

                    Scale = EnemyEV.PlantExpertScale;
                    ProjectileScale = EnemyEV.PlantExpertProjectileScale;
                    TintablePart.TextureColor = EnemyEV.PlantExpertTint;

                    MeleeRadius = EnemyEV.PLANT_EXPERT_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.PLANT_EXPERT_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.PLANT_EXPERT_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.PlantExpertKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Advanced):
                    #region Advanced Variables - General
                    Name = EnemyEV.PLANT_ADVANCED_NAME;
                    LocStringID = EnemyEV.PLANT_ADVANCED_NAME_LOC_ID;

                    MaxHealth = EnemyEV.PLANT_ADVANCED_MAX_HEALTH;
                    Damage = EnemyEV.PLANT_ADVANCED_DAMAGE;
                    XPValue = EnemyEV.PLANT_ADVANCED_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.PLANT_ADVANCED_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.PLANT_ADVANCED_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.PLANT_ADVANCED_DROP_CHANCE;

                    Speed = EnemyEV.PLANT_ADVANCED_SPEED;
                    TurnSpeed = EnemyEV.PLANT_ADVANCED_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.PLANT_ADVANCED_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.PLANT_ADVANCED_JUMP;
                    CooldownTime = EnemyEV.PLANT_ADVANCED_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.PLANT_ADVANCED_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.PLANT_ADVANCED_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.PLANT_ADVANCED_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.PLANT_ADVANCED_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.PLANT_ADVANCED_IS_WEIGHTED;

                    Scale = EnemyEV.PlantAdvancedScale;
                    ProjectileScale = EnemyEV.PlantAdvancedProjectileScale;
                    TintablePart.TextureColor = EnemyEV.PlantAdvancedTint;

                    MeleeRadius = EnemyEV.PLANT_ADVANCED_MELEE_RADIUS;
                    EngageRadius = EnemyEV.PLANT_ADVANCED_ENGAGE_RADIUS;
                    ProjectileRadius = EnemyEV.PLANT_ADVANCED_PROJECTILE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.PlantAdvancedKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Basic):
                default:
                    break;
            }

            _objectList[1].TextureColor = new Color(201, 59, 136);
        }

        protected override void InitializeLogic()
        {
            ProjectileData projData = new ProjectileData(this)
            {
                SpriteName = "PlantProjectile_Sprite",
                SourceAnchor = Vector2.Zero,
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = true,
                RotationSpeed = 0,
                Damage = Damage,
                AngleOffset = 0,
                CollidesWithTerrain = true,
                Scale = ProjectileScale,
                //Angle = new Vector2(-100, -100),
            };

            LogicSet spitProjectileLS = new LogicSet(this);
            spitProjectileLS.AddAction(new Play3DSoundLogicAction(this,  Game.ScreenManager.Player,"Enemy_Venus_Squirm_01", "Enemy_Venus_Squirm_02", "Enemy_Venus_Squirm_03"));
            spitProjectileLS.AddAction(new ChangeSpriteLogicAction("EnemyPlantAttack_Character", false, false));
            spitProjectileLS.AddAction(new PlayAnimationLogicAction(1, this.TotalFrames - 1));
            spitProjectileLS.AddAction(new Play3DSoundLogicAction(this,  Game.ScreenManager.Player,"Enemy_Venus_Attack_01"));
            projData.Angle = new Vector2(-90, -90);
            spitProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-75, -75);
            spitProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-105, -105);
            spitProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            spitProjectileLS.AddAction(new PlayAnimationLogicAction(this.TotalFrames - 1, TotalFrames));
            spitProjectileLS.AddAction(new ChangeSpriteLogicAction("EnemyPlantIdle_Character", true, true));
            //spitProjectileLS.AddAction(new DelayLogicAction(2.0f));
            spitProjectileLS.Tag = GameTypes.LOGIC_SET_TYPE_ATTACK;



            LogicSet spitAdvancedProjectileLS = new LogicSet(this);
            spitAdvancedProjectileLS.AddAction(new ChangeSpriteLogicAction("EnemyPlantAttack_Character", false, false));
            spitAdvancedProjectileLS.AddAction(new PlayAnimationLogicAction(1, this.TotalFrames - 1));
            spitAdvancedProjectileLS.AddAction(new Play3DSoundLogicAction(this,  Game.ScreenManager.Player,"Enemy_Venus_Attack_01"));
            projData.Angle = new Vector2(-60, -60);
            spitAdvancedProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-90, -90);
            spitAdvancedProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-75, -75);
            spitAdvancedProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-105, -105);
            spitAdvancedProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-120, -120);
            spitAdvancedProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            spitAdvancedProjectileLS.AddAction(new PlayAnimationLogicAction(this.TotalFrames - 1, TotalFrames));
            spitAdvancedProjectileLS.AddAction(new ChangeSpriteLogicAction("EnemyPlantIdle_Character", true, true));
            //spitAdvancedProjectileLS.AddAction(new DelayLogicAction(2.0f));
            spitAdvancedProjectileLS.Tag = GameTypes.LOGIC_SET_TYPE_ATTACK;


            LogicSet spitExpertProjectileLS = new LogicSet(this);
            spitExpertProjectileLS.AddAction(new ChangeSpriteLogicAction("EnemyPlantAttack_Character", false, false));
            spitExpertProjectileLS.AddAction(new PlayAnimationLogicAction(1, this.TotalFrames - 1));
            spitExpertProjectileLS.AddAction(new Play3DSoundLogicAction(this,  Game.ScreenManager.Player,"Enemy_Venus_Attack_01"));

            projData.Angle = new Vector2(-45, -45);
            spitExpertProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-60, -60);
            spitExpertProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-85, -85);
            spitExpertProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-90, -90);
            spitExpertProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-95, -95);
            spitExpertProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-75, -75);
            spitExpertProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-105, -105);
            spitExpertProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-120, -120);
            spitExpertProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-135, -135);
            spitExpertProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            spitExpertProjectileLS.AddAction(new PlayAnimationLogicAction(this.TotalFrames - 1, TotalFrames));
            spitExpertProjectileLS.AddAction(new ChangeSpriteLogicAction("EnemyPlantIdle_Character", true, true));
            //spitExpertProjectileLS.AddAction(new DelayLogicAction(2.0f));
            spitExpertProjectileLS.Tag = GameTypes.LOGIC_SET_TYPE_ATTACK;

            LogicSet spitMinibossProjectileLS = new LogicSet(this);
            spitMinibossProjectileLS.AddAction(new ChangeSpriteLogicAction("EnemyPlantAttack_Character", false, false));
            spitMinibossProjectileLS.AddAction(new PlayAnimationLogicAction(1, this.TotalFrames - 1));
            spitMinibossProjectileLS.AddAction(new Play3DSoundLogicAction(this,  Game.ScreenManager.Player,"Enemy_Venus_Attack_01"));

            projData.Angle = new Vector2(-60, -60);
            spitMinibossProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-87, -87);
            spitMinibossProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-90, -90);
            spitMinibossProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-93, -93);
            spitMinibossProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-75, -75);
            spitMinibossProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-105, -105);
            spitMinibossProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            projData.Angle = new Vector2(-120, -120);
            spitMinibossProjectileLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            spitMinibossProjectileLS.AddAction(new PlayAnimationLogicAction(this.TotalFrames - 1, TotalFrames));
            spitMinibossProjectileLS.AddAction(new ChangeSpriteLogicAction("EnemyPlantIdle_Character", true, true));
            //spitExpertProjectileLS.AddAction(new DelayLogicAction(2.0f));
            spitMinibossProjectileLS.Tag = GameTypes.LOGIC_SET_TYPE_ATTACK;

            LogicSet walkStopLS = new LogicSet(this);
            walkStopLS.AddAction(new ChangeSpriteLogicAction("EnemyPlantIdle_Character", true, true));
            //walkStopLS.AddAction(new StopAnimationLogicAction());
            walkStopLS.AddAction(new MoveLogicAction(m_target, true, 0));
            walkStopLS.AddAction(new DelayLogicAction(0.25f));


            LogicSet walkTowardsLS = new LogicSet(this);
            walkTowardsLS.AddAction(new Play3DSoundLogicAction(this,  Game.ScreenManager.Player,"Enemy_Venus_Squirm_01", "Enemy_Venus_Squirm_02", "Enemy_Venus_Squirm_03", "Blank", "Blank", "Blank"));
            walkTowardsLS.AddAction(new MoveLogicAction(m_target, true));
            walkTowardsLS.AddAction(new DelayLogicAction(0.25f, 0.45f));

            LogicSet walkAwayLS = new LogicSet(this);
            walkAwayLS.AddAction(new Play3DSoundLogicAction(this,  Game.ScreenManager.Player,"Enemy_Venus_Squirm_01", "Enemy_Venus_Squirm_02", "Enemy_Venus_Squirm_03", "Blank", "Blank", "Blank"));
            walkAwayLS.AddAction(new MoveLogicAction(m_target, false));
            walkAwayLS.AddAction(new DelayLogicAction(0.25f, 0.45f));

            LogicSet walkStopMiniBossLS = new LogicSet(this);
            walkStopMiniBossLS.AddAction(new Play3DSoundLogicAction(this,  Game.ScreenManager.Player,"Enemy_Venus_Squirm_01", "Enemy_Venus_Squirm_02", "Enemy_Venus_Squirm_03", "Blank", "Blank", "Blank"));
            walkStopMiniBossLS.AddAction(new ChangeSpriteLogicAction("EnemyPlantIdle_Character", true, true));
            //walkStopLS.AddAction(new StopAnimationLogicAction());
            walkStopMiniBossLS.AddAction(new MoveLogicAction(m_target, true, 0));
            walkStopMiniBossLS.AddAction(new DelayLogicAction(0.25f, 0.45f));

            m_generalBasicLB.AddLogicSet(spitProjectileLS);
            m_generalAdvancedLB.AddLogicSet(spitAdvancedProjectileLS);
            m_generalExpertLB.AddLogicSet(spitExpertProjectileLS);
            m_generalMiniBossLB.AddLogicSet(spitMinibossProjectileLS);
            m_generalCooldownLB.AddLogicSet(walkStopLS);
            m_generalCooldownExpertLB.AddLogicSet(walkTowardsLS, walkAwayLS, walkStopMiniBossLS);

            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalMiniBossLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);
            logicBlocksToDispose.Add(m_generalCooldownExpertLB);

            SetCooldownLogicBlock(m_generalCooldownLB, 100); //walkStopLS
            if (Difficulty == GameTypes.EnemyDifficulty.Miniboss)
                SetCooldownLogicBlock(m_generalCooldownExpertLB, 50, 50, 0); //walkTowardsLS, walkAwayLS, walkStopLS

            projData.Dispose();
            base.InitializeLogic();
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case (STATE_MELEE_ENGAGE):
                case (STATE_PROJECTILE_ENGAGE):
                case (STATE_ENGAGE):
                    RunLogicBlock(true, m_generalBasicLB, 100);
                    break;
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
                    RunLogicBlock(true, m_generalAdvancedLB, 100);
                    break;
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
                    RunLogicBlock(true, m_generalExpertLB, 100);
                    break;
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
                    RunLogicBlock(true, m_generalMiniBossLB, 100);
                    break;
                default:
                    break;
            }
        }

        public EnemyObj_Plant(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("EnemyPlantIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            this.Type = EnemyType.PLANT;
        }
    }
}
