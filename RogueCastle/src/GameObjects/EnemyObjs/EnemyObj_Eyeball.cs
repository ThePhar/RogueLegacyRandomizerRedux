using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Tweener;
using Microsoft.Xna.Framework.Audio;
using RogueCastle.EnvironmentVariables;
using RogueCastle.GameStructs;
using RogueCastle.LogicActions;

namespace RogueCastle
{
    public class EnemyObj_Eyeball: EnemyObj
    {
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();
        private LogicBlock m_generalMiniBossLB = new LogicBlock();
        private LogicBlock m_generalCooldownLB = new LogicBlock();
        private LogicBlock m_generalNeoLB = new LogicBlock();

        private SpriteObj m_pupil;
        private FrameSoundObj m_squishSound;
        public int PupilOffset { get; set; }

        private bool m_shake = false;
        private bool m_shookLeft = false;

        private float FireballDelay = 0.5f;
        private float m_shakeTimer = 0;
        private float m_shakeDuration = 0.03f;

        //2000
        private int m_bossCoins = 30;
        private int m_bossMoneyBags = 7;
        private int m_bossDiamonds = 1;

        private Cue m_deathLoop;
        private bool m_playDeathLoop;
        private bool m_isNeo = false;

        protected override void InitializeEV()
        {
            #region Basic Variables - General
            Name = EnemyEV.EYEBALL_BASIC_NAME;
            LocStringID = EnemyEV.EYEBALL_BASIC_NAME_LOC_ID;

            MaxHealth = EnemyEV.EYEBALL_BASIC_MAX_HEALTH;
            Damage = EnemyEV.EYEBALL_BASIC_DAMAGE;
            XPValue = EnemyEV.EYEBALL_BASIC_XP_VALUE;

            MinMoneyDropAmount = EnemyEV.EYEBALL_BASIC_MIN_DROP_AMOUNT;
            MaxMoneyDropAmount = EnemyEV.EYEBALL_BASIC_MAX_DROP_AMOUNT;
            MoneyDropChance = EnemyEV.EYEBALL_BASIC_DROP_CHANCE;

            Speed = EnemyEV.EYEBALL_BASIC_SPEED;
            TurnSpeed = EnemyEV.EYEBALL_BASIC_TURN_SPEED;
            ProjectileSpeed = EnemyEV.EYEBALL_BASIC_PROJECTILE_SPEED;
            JumpHeight = EnemyEV.EYEBALL_BASIC_JUMP;
            CooldownTime = EnemyEV.EYEBALL_BASIC_COOLDOWN;
            AnimationDelay = 1 / EnemyEV.EYEBALL_BASIC_ANIMATION_DELAY;

            AlwaysFaceTarget = EnemyEV.EYEBALL_BASIC_ALWAYS_FACE_TARGET;
            CanFallOffLedges = EnemyEV.EYEBALL_BASIC_CAN_FALL_OFF_LEDGES;
            CanBeKnockedBack = EnemyEV.EYEBALL_BASIC_CAN_BE_KNOCKED_BACK;
            IsWeighted = EnemyEV.EYEBALL_BASIC_IS_WEIGHTED;

            Scale = EnemyEV.EyeballBasicScale;
            ProjectileScale = EnemyEV.EyeballBasicProjectileScale;
            TintablePart.TextureColor = EnemyEV.EyeballBasicTint;

            MeleeRadius = EnemyEV.EYEBALL_BASIC_MELEE_RADIUS;
            ProjectileRadius = EnemyEV.EYEBALL_BASIC_PROJECTILE_RADIUS;
            EngageRadius = EnemyEV.EYEBALL_BASIC_ENGAGE_RADIUS;

            ProjectileDamage = Damage;
            KnockBack = EnemyEV.EyeballBasicKnockBack;
            #endregion
            PupilOffset = 4;
            switch (Difficulty)
            {
                case (GameTypes.EnemyDifficulty.Miniboss):
                    #region Miniboss Variables - General
                    Name = EnemyEV.EYEBALL_MINIBOSS_NAME;
                    LocStringID = EnemyEV.EYEBALL_MINIBOSS_NAME_LOC_ID;

                    MaxHealth = EnemyEV.EYEBALL_MINIBOSS_MAX_HEALTH;
                    Damage = EnemyEV.EYEBALL_MINIBOSS_DAMAGE;
                    XPValue = EnemyEV.EYEBALL_MINIBOSS_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.EYEBALL_MINIBOSS_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.EYEBALL_MINIBOSS_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.EYEBALL_MINIBOSS_DROP_CHANCE;

                    Speed = EnemyEV.EYEBALL_MINIBOSS_SPEED;
                    TurnSpeed = EnemyEV.EYEBALL_MINIBOSS_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.EYEBALL_MINIBOSS_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.EYEBALL_MINIBOSS_JUMP;
                    CooldownTime = EnemyEV.EYEBALL_MINIBOSS_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.EYEBALL_MINIBOSS_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.EYEBALL_MINIBOSS_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.EYEBALL_MINIBOSS_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.EYEBALL_MINIBOSS_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.EYEBALL_MINIBOSS_IS_WEIGHTED;

                    Scale = EnemyEV.EyeballMinibossScale;
                    ProjectileScale = EnemyEV.EyeballMinibossProjectileScale;
                    TintablePart.TextureColor = EnemyEV.EyeballMinibossTint;

                    MeleeRadius = EnemyEV.EYEBALL_MINIBOSS_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.EYEBALL_MINIBOSS_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.EYEBALL_MINIBOSS_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.EyeballMinibossKnockBack;
                    #endregion
                    PupilOffset = 0;
                    if (LevelEV.WeakenBosses == true)
                        this.MaxHealth = 1;
                    break;

                case (GameTypes.EnemyDifficulty.Expert):
                    #region Expert Variables - General
                    Name = EnemyEV.EYEBALL_EXPERT_NAME;
                    LocStringID = EnemyEV.EYEBALL_EXPERT_NAME_LOC_ID;

                    MaxHealth = EnemyEV.EYEBALL_EXPERT_MAX_HEALTH;
                    Damage = EnemyEV.EYEBALL_EXPERT_DAMAGE;
                    XPValue = EnemyEV.EYEBALL_EXPERT_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.EYEBALL_EXPERT_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.EYEBALL_EXPERT_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.EYEBALL_EXPERT_DROP_CHANCE;

                    Speed = EnemyEV.EYEBALL_EXPERT_SPEED;
                    TurnSpeed = EnemyEV.EYEBALL_EXPERT_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.EYEBALL_EXPERT_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.EYEBALL_EXPERT_JUMP;
                    CooldownTime = EnemyEV.EYEBALL_EXPERT_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.EYEBALL_EXPERT_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.EYEBALL_EXPERT_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.EYEBALL_EXPERT_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.EYEBALL_EXPERT_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.EYEBALL_EXPERT_IS_WEIGHTED;

                    Scale = EnemyEV.EyeballExpertScale;
                    ProjectileScale = EnemyEV.EyeballExpertProjectileScale;
                    TintablePart.TextureColor = EnemyEV.EyeballExpertTint;

                    MeleeRadius = EnemyEV.EYEBALL_EXPERT_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.EYEBALL_EXPERT_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.EYEBALL_EXPERT_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.EyeballExpertKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Advanced):
                    #region Advanced Variables - General
                    Name = EnemyEV.EYEBALL_ADVANCED_NAME;
                    LocStringID = EnemyEV.EYEBALL_ADVANCED_NAME_LOC_ID;

                    MaxHealth = EnemyEV.EYEBALL_ADVANCED_MAX_HEALTH;
                    Damage = EnemyEV.EYEBALL_ADVANCED_DAMAGE;
                    XPValue = EnemyEV.EYEBALL_ADVANCED_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.EYEBALL_ADVANCED_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.EYEBALL_ADVANCED_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.EYEBALL_ADVANCED_DROP_CHANCE;

                    Speed = EnemyEV.EYEBALL_ADVANCED_SPEED;
                    TurnSpeed = EnemyEV.EYEBALL_ADVANCED_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.EYEBALL_ADVANCED_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.EYEBALL_ADVANCED_JUMP;
                    CooldownTime = EnemyEV.EYEBALL_ADVANCED_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.EYEBALL_ADVANCED_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.EYEBALL_ADVANCED_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.EYEBALL_ADVANCED_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.EYEBALL_ADVANCED_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.EYEBALL_ADVANCED_IS_WEIGHTED;

                    Scale = EnemyEV.EyeballAdvancedScale;
                    ProjectileScale = EnemyEV.EyeballAdvancedProjectileScale;
                    TintablePart.TextureColor = EnemyEV.EyeballAdvancedTint;

                    MeleeRadius = EnemyEV.EYEBALL_ADVANCED_MELEE_RADIUS;
                    EngageRadius = EnemyEV.EYEBALL_ADVANCED_ENGAGE_RADIUS;
                    ProjectileRadius = EnemyEV.EYEBALL_ADVANCED_PROJECTILE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.EyeballAdvancedKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Basic):
                default:
                    break;
            }

            if (this.Difficulty == GameTypes.EnemyDifficulty.Miniboss)
                m_resetSpriteName = "EnemyEyeballBossEye_Character";
        }

        protected override void InitializeLogic()
        {
            ProjectileData projData = new ProjectileData(this)
            {
                SpriteName = "EyeballProjectile_Sprite",
                SourceAnchor = Vector2.Zero,
                Target = m_target,
                Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0,
                Damage = Damage,
                AngleOffset = 0,
                CollidesWithTerrain = false,
                Scale = ProjectileScale,
                
            };

            LogicSet fireProjectileBasicLS = new LogicSet(this);
            fireProjectileBasicLS.AddAction(new ChangeSpriteLogicAction("EnemyEyeballFire_Character", true, true));
            fireProjectileBasicLS.AddAction(new DelayLogicAction(FireballDelay));
            fireProjectileBasicLS.AddAction(new Play3DSoundLogicAction(this, m_target, "Eyeball_ProjectileAttack"));
            fireProjectileBasicLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            fireProjectileBasicLS.AddAction(new ChangeSpriteLogicAction("EnemyEyeballIdle_Character", false, false));            
            fireProjectileBasicLS.AddAction(new DelayLogicAction(1.0f, 3.0f));
            fireProjectileBasicLS.Tag = GameTypes.LOGIC_SET_TYPE_ATTACK;


            LogicSet fireProjectileAdvancedLS = new LogicSet(this);
            fireProjectileAdvancedLS.AddAction(new ChangeSpriteLogicAction("EnemyEyeballFire_Character", true, true));
            fireProjectileAdvancedLS.AddAction(new DelayLogicAction(FireballDelay));
            fireProjectileAdvancedLS.AddAction(new Play3DSoundLogicAction(this, m_target, "EyeballFire1"));
            fireProjectileAdvancedLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            fireProjectileAdvancedLS.AddAction(new DelayLogicAction(0.15f));
            fireProjectileAdvancedLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            fireProjectileAdvancedLS.AddAction(new DelayLogicAction(0.15f));
            fireProjectileAdvancedLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            fireProjectileAdvancedLS.AddAction(new DelayLogicAction(0.15f));
            fireProjectileAdvancedLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            //fireProjectileAdvancedLS.AddAction(new DelayLogicAction(0.15f));
            //fireProjectileAdvancedLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            fireProjectileAdvancedLS.AddAction(new ChangeSpriteLogicAction("EnemyEyeballIdle_Character", false, false));
            fireProjectileAdvancedLS.AddAction(new DelayLogicAction(0.75f, 2.0f));
            fireProjectileAdvancedLS.Tag = GameTypes.LOGIC_SET_TYPE_ATTACK;

            LogicSet fireProjectileExpertLS = new LogicSet(this);
            fireProjectileExpertLS.AddAction(new ChangeSpriteLogicAction("EnemyEyeballFire_Character", true, true));
            fireProjectileExpertLS.AddAction(new DelayLogicAction(FireballDelay));
            fireProjectileExpertLS.AddAction(new Play3DSoundLogicAction(this, m_target, "EyeballFire1"));
            fireProjectileExpertLS.AddAction(new DelayLogicAction(0.1f));
            ThrowThreeProjectiles(fireProjectileExpertLS);
            fireProjectileExpertLS.AddAction(new ChangeSpriteLogicAction("EnemyEyeballIdle_Character", false, false));
            fireProjectileExpertLS.AddAction(new DelayLogicAction(1.0f, 3.0f));
            fireProjectileExpertLS.Tag = GameTypes.LOGIC_SET_TYPE_ATTACK;


            ////////////////// MINI BOSS LOGIC //////////////////////////////
            LogicSet fireProjectileCardinalSpinLS = new LogicSet(this);
            fireProjectileCardinalSpinLS.AddAction(new ChangeSpriteLogicAction("EnemyEyeballBossFire_Character", true, true));
            fireProjectileCardinalSpinLS.AddAction(new RunFunctionLogicAction(this, "LockEyeball"));
            fireProjectileCardinalSpinLS.AddAction(new RunFunctionLogicAction(m_pupil, "ChangeSprite", "EnemyEyeballBossPupilFire_Sprite"));
            fireProjectileCardinalSpinLS.AddAction(new DelayLogicAction(FireballDelay));
            fireProjectileCardinalSpinLS.AddAction(new Play3DSoundLogicAction(this, m_target, "EyeballFire1"));
            fireProjectileCardinalSpinLS.AddAction(new DelayLogicAction(0.1f));
            //ThrowCardinalProjectiles(fireProjectileCardinalSpinLS);
            fireProjectileCardinalSpinLS.AddAction(new RunFunctionLogicAction(this, "ThrowCardinalProjectiles", 0, true, 0));
            fireProjectileCardinalSpinLS.AddAction(new DelayLogicAction(3.15f));
            fireProjectileCardinalSpinLS.AddAction(new ChangeSpriteLogicAction("EnemyEyeballBossEye_Character", false, false));
            fireProjectileCardinalSpinLS.AddAction(new RunFunctionLogicAction(m_pupil, "ChangeSprite", "EnemyEyeballBossPupil_Sprite"));
            fireProjectileCardinalSpinLS.AddAction(new RunFunctionLogicAction(this, "UnlockEyeball"));
            //fireProjectileCardinalSpinLS.AddAction(new DelayLogicAction(2.5f, 3.5f));
            fireProjectileCardinalSpinLS.Tag = GameTypes.LOGIC_SET_TYPE_ATTACK;

            #region Neo Version
            LogicSet fireProjectileCardinalSpinNeoLS = new LogicSet(this);
            fireProjectileCardinalSpinNeoLS.AddAction(new ChangeSpriteLogicAction("EnemyEyeballBossFire_Character", true, true));
            fireProjectileCardinalSpinNeoLS.AddAction(new RunFunctionLogicAction(this, "LockEyeball"));
            fireProjectileCardinalSpinNeoLS.AddAction(new RunFunctionLogicAction(m_pupil, "ChangeSprite", "EnemyEyeballBossPupilFire_Sprite"));
            fireProjectileCardinalSpinNeoLS.AddAction(new DelayLogicAction(FireballDelay));
            fireProjectileCardinalSpinNeoLS.AddAction(new Play3DSoundLogicAction(this, m_target, "EyeballFire1"));
            fireProjectileCardinalSpinNeoLS.AddAction(new DelayLogicAction(0.1f));
            //ThrowCardinalProjectiles(fireProjectileCardinalSpinLS);
            fireProjectileCardinalSpinNeoLS.AddAction(new RunFunctionLogicAction(this, "ThrowCardinalProjectilesNeo", 0, true, 0));
            fireProjectileCardinalSpinNeoLS.AddAction(new DelayLogicAction(3.0f));//(3.15f));
            fireProjectileCardinalSpinNeoLS.AddAction(new ChangeSpriteLogicAction("EnemyEyeballBossEye_Character", false, false));
            fireProjectileCardinalSpinNeoLS.AddAction(new RunFunctionLogicAction(m_pupil, "ChangeSprite", "EnemyEyeballBossPupil_Sprite"));
            fireProjectileCardinalSpinNeoLS.AddAction(new RunFunctionLogicAction(this, "UnlockEyeball"));
            //fireProjectileCardinalSpinLS.AddAction(new DelayLogicAction(2.5f, 3.5f));
            fireProjectileCardinalSpinNeoLS.Tag = GameTypes.LOGIC_SET_TYPE_ATTACK;

            #endregion


            LogicSet fireProjectileSprayLS = new LogicSet(this);
            fireProjectileSprayLS.AddAction(new ChangeSpriteLogicAction("EnemyEyeballBossFire_Character", true, true));
            fireProjectileSprayLS.AddAction(new RunFunctionLogicAction(this, "LockEyeball"));
            fireProjectileSprayLS.AddAction(new RunFunctionLogicAction(m_pupil, "ChangeSprite", "EnemyEyeballBossPupilFire_Sprite"));
            fireProjectileSprayLS.AddAction(new DelayLogicAction(FireballDelay));
            fireProjectileSprayLS.AddAction(new Play3DSoundLogicAction(this, m_target, "EyeballFire1"));
            fireProjectileSprayLS.AddAction(new DelayLogicAction(0.1f));
            //ThrowSprayProjectiles(fireProjectileSprayLS);
            fireProjectileSprayLS.AddAction(new RunFunctionLogicAction(this, "ThrowSprayProjectiles", true));
            fireProjectileSprayLS.AddAction(new DelayLogicAction(1.6f));
            fireProjectileSprayLS.AddAction(new RunFunctionLogicAction(this, "ThrowSprayProjectiles", true));
            fireProjectileSprayLS.AddAction(new DelayLogicAction(1.6f));
            fireProjectileSprayLS.AddAction(new ChangeSpriteLogicAction("EnemyEyeballBossEye_Character", false, false));
            fireProjectileSprayLS.AddAction(new RunFunctionLogicAction(m_pupil, "ChangeSprite", "EnemyEyeballBossPupil_Sprite"));
            fireProjectileSprayLS.AddAction(new RunFunctionLogicAction(this, "UnlockEyeball"));
            fireProjectileSprayLS.Tag = GameTypes.LOGIC_SET_TYPE_ATTACK;

            LogicSet fireProjectileRandomLS = new LogicSet(this);
            fireProjectileRandomLS.AddAction(new ChangeSpriteLogicAction("EnemyEyeballBossFire_Character", true, true));
            fireProjectileRandomLS.AddAction(new RunFunctionLogicAction(this, "LockEyeball"));
            fireProjectileRandomLS.AddAction(new RunFunctionLogicAction(m_pupil, "ChangeSprite", "EnemyEyeballBossPupilFire_Sprite"));
            fireProjectileRandomLS.AddAction(new DelayLogicAction(FireballDelay));
            fireProjectileRandomLS.AddAction(new Play3DSoundLogicAction(this, m_target, "EyeballFire1"));
            fireProjectileRandomLS.AddAction(new DelayLogicAction(0.1f));
            //ThrowRandomProjectiles(fireProjectileRandomLS);
            fireProjectileRandomLS.AddAction(new RunFunctionLogicAction(this, "ThrowRandomProjectiles"));
            fireProjectileRandomLS.AddAction(new DelayLogicAction(0.575f));
            fireProjectileRandomLS.AddAction(new RunFunctionLogicAction(this, "ThrowRandomProjectiles"));
            fireProjectileRandomLS.AddAction(new DelayLogicAction(0.575f));
            fireProjectileRandomLS.AddAction(new RunFunctionLogicAction(this, "ThrowRandomProjectiles"));
            fireProjectileRandomLS.AddAction(new DelayLogicAction(0.575f));
            fireProjectileRandomLS.AddAction(new RunFunctionLogicAction(this, "ThrowRandomProjectiles"));
            fireProjectileRandomLS.AddAction(new DelayLogicAction(0.575f));
            fireProjectileRandomLS.AddAction(new RunFunctionLogicAction(this, "ThrowRandomProjectiles"));
            fireProjectileRandomLS.AddAction(new DelayLogicAction(0.575f));
            fireProjectileRandomLS.AddAction(new ChangeSpriteLogicAction("EnemyEyeballBossEye_Character", false, false));
            fireProjectileRandomLS.AddAction(new RunFunctionLogicAction(m_pupil, "ChangeSprite", "EnemyEyeballBossPupil_Sprite"));
            fireProjectileRandomLS.AddAction(new RunFunctionLogicAction(this, "UnlockEyeball"));
            fireProjectileRandomLS.Tag = GameTypes.LOGIC_SET_TYPE_ATTACK;


            LogicSet doNothing = new LogicSet(this);
            doNothing.AddAction(new DelayLogicAction(0.2f, 0.5f));


            m_generalBasicLB.AddLogicSet(fireProjectileBasicLS, doNothing);
            m_generalAdvancedLB.AddLogicSet(fireProjectileAdvancedLS, doNothing);
            m_generalExpertLB.AddLogicSet(fireProjectileExpertLS, doNothing);
            m_generalMiniBossLB.AddLogicSet(fireProjectileCardinalSpinLS, fireProjectileSprayLS, fireProjectileRandomLS, doNothing);
            m_generalCooldownLB.AddLogicSet(doNothing);

            m_generalNeoLB.AddLogicSet(fireProjectileCardinalSpinNeoLS, fireProjectileSprayLS, fireProjectileRandomLS, doNothing);


            logicBlocksToDispose.Add(m_generalNeoLB);
            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalMiniBossLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);


            SetCooldownLogicBlock(m_generalCooldownLB, 100); //doNothing

            projData.Dispose(); // Don't forget to dispose the projectile data object.

            base.InitializeLogic();
        }

        private void ThrowThreeProjectiles(LogicSet ls)
        {
            ProjectileData expertData = new ProjectileData(this)
            {
                SpriteName = "EyeballProjectile_Sprite",
                SourceAnchor = Vector2.Zero,
                Target = m_target,
                Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0,
                Damage = Damage,
                AngleOffset = 0,
                CollidesWithTerrain = false,
                Scale = ProjectileScale,
                Angle = new Vector2(0, 0),
            };
            for (int i = 0; i <= 3; i++)
            {
                expertData.AngleOffset = 0;
                ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, expertData));
                expertData.AngleOffset = 45;
                ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, expertData));
                expertData.AngleOffset = -45;
                ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, expertData));
                ls.AddAction(new DelayLogicAction(0.1f));
            }
            expertData.Dispose();
        }

        private void ThrowCardinalProjectiles(LogicSet ls)
        {
            ProjectileData cardinalData = new ProjectileData(this)
            {
                SpriteName = "EyeballProjectile_Sprite",
                SourceAnchor = Vector2.Zero,
                Target = null,
                Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0,
                Damage = Damage,
                AngleOffset = 0,
                Scale = ProjectileScale,
                CollidesWithTerrain = false,
                Angle = new Vector2(0, 0),
            };

            int flipper = CDGMath.RandomPlusMinus();
            for (int i = 0; i <= 170; i = i + 10)
            {
                cardinalData.AngleOffset =  0 + i * flipper;
                ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, cardinalData));
                cardinalData.AngleOffset = 90 + i * flipper;
                ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, cardinalData));
                cardinalData.AngleOffset = 180 + i * flipper;
                ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, cardinalData));
                cardinalData.AngleOffset = 270 + i * flipper;
                ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, cardinalData));
                ls.AddAction(new DelayLogicAction(0.175f));
            }

            cardinalData.Dispose();
        }

        public void ThrowCardinalProjectiles(int startProjIndex, bool randomizeFlipper, int flipper)
        {
            if (startProjIndex < 17)
            {
                ProjectileData cardinalData = new ProjectileData(this)
                {
                    SpriteName = "EyeballProjectile_Sprite",
                    SourceAnchor = Vector2.Zero,
                    Target = null,
                    Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
                    IsWeighted = false,
                    RotationSpeed = 0,
                    Damage = Damage,
                    AngleOffset = 0,
                    Scale = ProjectileScale,
                    CollidesWithTerrain = false,
                    Angle = new Vector2(0, 0),
                };

                if (randomizeFlipper == true)
                    flipper = CDGMath.RandomPlusMinus();

                cardinalData.AngleOffset = -10 + (startProjIndex * 10) * flipper;
                m_levelScreen.ProjectileManager.FireProjectile(cardinalData);
                cardinalData.AngleOffset = 80 + (startProjIndex * 10) * flipper;
                m_levelScreen.ProjectileManager.FireProjectile(cardinalData);
                cardinalData.AngleOffset = 170 + (startProjIndex * 10) * flipper;
                m_levelScreen.ProjectileManager.FireProjectile(cardinalData);
                cardinalData.AngleOffset = 260 + (startProjIndex * 10) * flipper;
                m_levelScreen.ProjectileManager.FireProjectile(cardinalData);

                cardinalData.Dispose();

                startProjIndex++;
                Tween.RunFunction(0.12f, this, "ThrowCardinalProjectiles", startProjIndex, false, flipper);
            }
        }

        public void ThrowCardinalProjectilesNeo(int startProjIndex, bool randomizeFlipper, int flipper)
        {
            if (startProjIndex < 13)//12)//17)
            {
                ProjectileData cardinalData = new ProjectileData(this)
                {
                    SpriteName = "EyeballProjectile_Sprite",
                    SourceAnchor = Vector2.Zero,
                    Target = null,
                    Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
                    IsWeighted = false,
                    RotationSpeed = 0,
                    Damage = Damage,
                    AngleOffset = 0,
                    Scale = ProjectileScale,
                    CollidesWithTerrain = false,
                    Angle = new Vector2(0, 0),
                };

                if (randomizeFlipper == true)
                    flipper = CDGMath.RandomPlusMinus();

                cardinalData.AngleOffset = -10 + (startProjIndex * 17) * flipper;
                m_levelScreen.ProjectileManager.FireProjectile(cardinalData);
                cardinalData.AngleOffset = 80 + (startProjIndex * 17) * flipper;
                m_levelScreen.ProjectileManager.FireProjectile(cardinalData);
                cardinalData.AngleOffset = 170 + (startProjIndex * 17) * flipper;
                m_levelScreen.ProjectileManager.FireProjectile(cardinalData);
                cardinalData.AngleOffset = 260 + (startProjIndex * 17) * flipper;
                m_levelScreen.ProjectileManager.FireProjectile(cardinalData);

                cardinalData.Dispose();

                startProjIndex++;
                Tween.RunFunction(0.12f, this, "ThrowCardinalProjectilesNeo", startProjIndex, false, flipper);
            }
        }


        public void LockEyeball()
        {
            Tweener.Tween.To(this, 0.5f, Tweener.Ease.Quad.EaseInOut, "PupilOffset", "0");
        }

        public void UnlockEyeball()
        {
            Tweener.Tween.To(this, 0.5f, Tweener.Ease.Quad.EaseInOut, "PupilOffset", "30");
        }

        public void ChangeToBossPupil()
        {
            m_pupil.ChangeSprite("EnemyEyeballBossPupil_Sprite");
            m_pupil.Scale = new Vector2(0.9f, 0.9f);
        }

        public void ThrowSprayProjectiles(bool firstShot)
        {
            ProjectileData starData = new ProjectileData(this)
            {
                SpriteName = "EyeballProjectile_Sprite",
                SourceAnchor = Vector2.Zero,
                Target = null,
                Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0,
                Damage = Damage,
                AngleOffset = 0,
                Angle = new Vector2(0, 0),
                CollidesWithTerrain = false,
                Scale = ProjectileScale,
            };

            int y = 360 / 12;
            for (int i = 0; i <= 360; i = i + y)
            {
                if (firstShot== true)
                    starData.AngleOffset = 10 + i;
                else
                    starData.AngleOffset = 20 + i;

                m_levelScreen.ProjectileManager.FireProjectile(starData);
            }

            if (firstShot == true)
            {
                Tween.RunFunction(0.8f, this, "ThrowSprayProjectiles", false);
            }

            //int y = 360 / 12; //12;
            //for (int k = 0; k < 2; k++)
            //{
            //    starData.AngleOffset = 0;
            //    for (int i = 0; i <= 360; i = i + y)
            //    {
            //        starData.AngleOffset = 10 + i;
            //        m_levelScreen.ProjectileManager.FireProjectile(starData);
            //        //ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, starData));

            //    }

            //    ls.AddAction(new DelayLogicAction(0.8f));
            //    for (int i = 0; i <= 360; i = i + y)
            //    {
            //        starData.AngleOffset = 20 + i;
            //        ls.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, starData));
            //    }
            //    ls.AddAction(new DelayLogicAction(0.8f));
            //}
            starData.Dispose();
        }

        public void ThrowRandomProjectiles()
        {
            ProjectileData RandomBullet = new ProjectileData(this)
            {
                SpriteName = "EyeballProjectile_Sprite",
                SourceAnchor = Vector2.Zero,
                Target = null,
                Speed = new Vector2(this.ProjectileSpeed, this.ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0,
                Damage = Damage,
                AngleOffset = 0,
                Angle = new Vector2(0, 0),
                CollidesWithTerrain = false,
                Scale = ProjectileScale,
            };

            //for (int k = 0; k < 5; k++)
            {
                RandomBullet.Angle = new Vector2(0, 44);
                m_levelScreen.ProjectileManager.FireProjectile(RandomBullet);
                RandomBullet.Angle = new Vector2(45, 89);
                m_levelScreen.ProjectileManager.FireProjectile(RandomBullet);
                RandomBullet.Angle = new Vector2(90, 134);
                m_levelScreen.ProjectileManager.FireProjectile(RandomBullet);
                RandomBullet.Angle = new Vector2(135, 179);
                m_levelScreen.ProjectileManager.FireProjectile(RandomBullet);
                RandomBullet.Angle = new Vector2(180, 224);
                m_levelScreen.ProjectileManager.FireProjectile(RandomBullet);
                RandomBullet.Angle = new Vector2(225, 269);
                m_levelScreen.ProjectileManager.FireProjectile(RandomBullet);
                RandomBullet.Angle = new Vector2(270, 314);
                m_levelScreen.ProjectileManager.FireProjectile(RandomBullet);
                RandomBullet.Angle = new Vector2(315, 359);
                m_levelScreen.ProjectileManager.FireProjectile(RandomBullet);
//                ls.AddAction(new DelayLogicAction(0.575f));
            }
            RandomBullet.Dispose();
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
                    if (IsNeo == false)
                        RunLogicBlock(true, m_generalMiniBossLB, 40, 20, 40, 0); //fireProjectileCardinalSpinLS, fireProjectileSprayLS, fireProjectileRandomLS, doNothing
                    else
                        RunLogicBlock(false, m_generalNeoLB, 53, 12, 35, 0); //fireProjectileCardinalSpinLS, fireProjectileSprayLS, fireProjectileRandomLS, doNothing
                    break;
                default:
                    break;
            }
        }

        public EnemyObj_Eyeball(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("EnemyEyeballIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            m_pupil = new SpriteObj("EnemyEyeballPupil_Sprite");
            this.AddChild(m_pupil);
            //m_squishSound = new FrameSoundObj(this, 2, "EyeballSquish1", "EyeballSquish2", "EyeballSquish3");
            m_squishSound = new FrameSoundObj(this, m_target, 2, "Eyeball_Prefire");
            this.Type = EnemyType.EYEBALL;
            this.DisableCollisionBoxRotations = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (m_playDeathLoop == true)
            {
                if (m_deathLoop == null || m_deathLoop.IsPlaying == false)
                  m_deathLoop = SoundManager.PlaySound("Boss_Eyeball_Death_Loop");
            }

            float y = m_target.Y - this.Y;
            float x = m_target.X - this.X;
            float angle = (float)Math.Atan2(y, x);

            m_pupil.X = (float)Math.Cos(angle) * PupilOffset;
            m_pupil.Y = (float)Math.Sin(angle) * PupilOffset;

            if (m_shake == true)
            {
                if (m_shakeTimer > 0)
                {
                    m_shakeTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (m_shakeTimer <= 0)
                    {
                        m_shakeTimer = m_shakeDuration;
                        if (m_shookLeft == true)
                        {
                            m_shookLeft = false;
                            this.X += 5;
                        }
                        else
                        {
                            this.X -= 5;
                            m_shookLeft = true;
                        }
                    }
                }
            }

            m_squishSound.Update();

            base.Update(gameTime);
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
        {
            // Hits the player in Tanooki mode.
            if (this.Difficulty == GameTypes.EnemyDifficulty.Miniboss && m_bossVersionKilled == false)
            {
                PlayerObj player = otherBox.AbsParent as PlayerObj;
                if (player != null && otherBox.Type == Consts.WEAPON_HITBOX && player.IsInvincible == false && player.State == PlayerObj.STATE_TANOOKI)
                    player.HitPlayer(this);
            }

            base.CollisionResponse(thisBox, otherBox, collisionResponseType);
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
        {
            if (m_bossVersionKilled == false)
            {
                SoundManager.PlaySound("EyeballSquish1", "EyeballSquish2", "EyeballSquish3");
                base.HitEnemy(damage, position, isPlayer);
            }
        }

        public override void Kill(bool giveXP = true)
        {
            if (Difficulty != GameTypes.EnemyDifficulty.Miniboss)
            {
                //SoundManager.PlaySound("EyeballDeath1", "EyeballDeath2");
                base.Kill(giveXP);
            }
            else
            {
                if (m_target.CurrentHealth > 0)
                {
                    Game.PlayerStats.EyeballBossBeaten = true;

                    Tween.StopAllContaining(this, false);
                    if (m_currentActiveLB != null && m_currentActiveLB.IsActive == true)
                        m_currentActiveLB.StopLogicBlock();

                    SoundManager.StopMusic(0);
                    m_bossVersionKilled = true;
                    m_target.LockControls();
                    m_levelScreen.PauseScreen();
                    m_levelScreen.ProjectileManager.DestroyAllProjectiles(false);
                    m_levelScreen.RunWhiteSlashEffect();
                    SoundManager.PlaySound("Boss_Flash");
                    SoundManager.PlaySound("Boss_Eyeball_Freeze");
                    Tween.RunFunction(1f, this, "Part2");

                    GameUtil.UnlockAchievement("FEAR_OF_EYES");
                }
            }
        }

        public void Part2()
        {
            m_levelScreen.UnpauseScreen();
            m_target.UnlockControls();

            if (m_currentActiveLB != null)
                m_currentActiveLB.StopLogicBlock();

            LockEyeball();
            this.PauseEnemy(true);
            this.ChangeSprite("EnemyEyeballBossFire_Character");
            this.PlayAnimation(true);
            m_target.CurrentSpeed = 0;
            m_target.ForceInvincible = true;

            if (IsNeo == true)
                m_target.InvincibleToSpikes = true;

            Tween.To(m_levelScreen.Camera, 0.5f, Tweener.Ease.Quad.EaseInOut, "X", m_levelScreen.CurrentRoom.Bounds.Center.X.ToString(), "Y", m_levelScreen.CurrentRoom.Bounds.Center.Y.ToString());
            m_shake = true;
            m_shakeTimer = m_shakeDuration;
            m_playDeathLoop = true;

            for (int i = 0; i < 40; i++)
            {
                Vector2 explosionPos = new Vector2(CDGMath.RandomInt(this.Bounds.Left, this.Bounds.Right), CDGMath.RandomInt(this.Bounds.Top, this.Bounds.Bottom));
                Tween.RunFunction(i * 0.1f, typeof(SoundManager), "Play3DSound", this, m_target, new string[]{"Boss_Explo_01", "Boss_Explo_02", "Boss_Explo_03"});
                Tween.RunFunction(i * 0.1f, m_levelScreen.ImpactEffectPool, "DisplayExplosionEffect", explosionPos);
            }

            Tween.AddEndHandlerToLastTween(this, "Part3");

            if (IsNeo == false)
            {
                int totalGold = m_bossCoins + m_bossMoneyBags + m_bossDiamonds;
                List<int> goldArray = new List<int>();

                for (int i = 0; i < m_bossCoins; i++)
                    goldArray.Add(0);
                for (int i = 0; i < m_bossMoneyBags; i++)
                    goldArray.Add(1);
                for (int i = 0; i < m_bossDiamonds; i++)
                    goldArray.Add(2);

                CDGMath.Shuffle<int>(goldArray);
                float coinDelay = 2.5f / goldArray.Count; // The enemy dies for 2.5 seconds.

                for (int i = 0; i < goldArray.Count; i++)
                {
                    Vector2 goldPos = new Vector2(CDGMath.RandomInt(m_pupil.AbsBounds.Left, m_pupil.AbsBounds.Right), CDGMath.RandomInt(m_pupil.AbsBounds.Top, m_pupil.AbsBounds.Bottom));
                    if (goldArray[i] == 0)
                        Tween.RunFunction(i * coinDelay, m_levelScreen.ItemDropManager, "DropItem", goldPos, ItemDropType.COIN, ItemDropType.COIN_AMOUNT);
                    else if (goldArray[i] == 1)
                        Tween.RunFunction(i * coinDelay, m_levelScreen.ItemDropManager, "DropItem", goldPos, ItemDropType.MONEY_BAG, ItemDropType.MONEY_BAG_AMOUNT);
                    else
                        Tween.RunFunction(i * coinDelay, m_levelScreen.ItemDropManager, "DropItem", goldPos, ItemDropType.DIAMOND, ItemDropType.DIAMOND_AMOUNT);
                }
            }
        }

        public void Part3()
        {
            SoundManager.PlaySound("Boss_Eyeball_Death");
            m_playDeathLoop = false;
            if (m_deathLoop != null && m_deathLoop.IsPlaying == true)
                m_deathLoop.Stop(AudioStopOptions.Immediate);
            this.GoToFrame(1);
            this.StopAnimation();
            Tween.To(this, 2, Tweener.Tween.EaseNone, "Rotation", "-1080");
            Tween.To(this, 2, Tweener.Ease.Quad.EaseInOut, "ScaleX", "0.1", "ScaleY", "0.1");
            Tween.AddEndHandlerToLastTween(this, "DeathComplete");
        }

        public void DeathComplete()
        {
            SoundManager.PlaySound("Boss_Explo_01", "Boss_Explo_02", "Boss_Explo_03");
            base.Kill();
        }

        public bool BossVersionKilled
        {
            get { return m_bossVersionKilled; }
        }

        public override void Dispose()
        {
            if (IsDisposed == false)
            {
                // Done
                m_pupil.Dispose();
                m_pupil = null;
                m_squishSound.Dispose();
                m_squishSound = null;
                if (m_deathLoop != null && m_deathLoop.IsDisposed == false)
                    m_deathLoop.Dispose();
                m_deathLoop = null;
                base.Dispose();
            }
        }

        public bool IsNeo
        {
            get { return m_isNeo; }
            set
            {
                if (value == true)
                {
                    HealthGainPerLevel = 0;
                    DamageGainPerLevel = 0;
                    MoneyDropChance = 0;
                    ItemDropChance = 0;
                    m_saveToEnemiesKilledList = false;
                }
                m_isNeo = value;
            }
        }
    }
}
