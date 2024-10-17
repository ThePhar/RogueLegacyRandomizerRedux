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
    public class EnemyObj_IceWizard : EnemyObj
    {
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();
        private LogicBlock m_generalCooldownLB = new LogicBlock();

        private float SpellDelay = 0.8f;
        //private float SpellInterval = 0.5f;

        private int SpellProjectileCount = 7;//10;
        private ProjectileObj m_iceballSummon;
        private Vector2 m_spellOffset = new Vector2(40, -80);

        private float TeleportDelay = 0.5f;
        private float TeleportDuration = 1.0f;

        private float MoveDuration = 1.0f;
        private Vector2 IceScale = Vector2.One;

        private float m_iceParticleEffectCounter = 0.5f;

        protected override void InitializeEV()
        {
            SpellProjectileCount = 7;
            #region Basic Variables - General
            Name = EnemyEV.ICE_WIZARD_BASIC_NAME;
            LocStringID = EnemyEV.ICE_WIZARD_BASIC_NAME_LOC_ID;

            MaxHealth = EnemyEV.ICE_WIZARD_BASIC_MAX_HEALTH;
            Damage = EnemyEV.ICE_WIZARD_BASIC_DAMAGE;
            XPValue = EnemyEV.ICE_WIZARD_BASIC_XP_VALUE;

            MinMoneyDropAmount = EnemyEV.ICE_WIZARD_BASIC_MIN_DROP_AMOUNT;
            MaxMoneyDropAmount = EnemyEV.ICE_WIZARD_BASIC_MAX_DROP_AMOUNT;
            MoneyDropChance = EnemyEV.ICE_WIZARD_BASIC_DROP_CHANCE;

            Speed = EnemyEV.ICE_WIZARD_BASIC_SPEED;
            TurnSpeed = EnemyEV.ICE_WIZARD_BASIC_TURN_SPEED;
            ProjectileSpeed = EnemyEV.ICE_WIZARD_BASIC_PROJECTILE_SPEED;
            JumpHeight = EnemyEV.ICE_WIZARD_BASIC_JUMP;
            CooldownTime = EnemyEV.ICE_WIZARD_BASIC_COOLDOWN;
            AnimationDelay = 1 / EnemyEV.ICE_WIZARD_BASIC_ANIMATION_DELAY;

            AlwaysFaceTarget = EnemyEV.ICE_WIZARD_BASIC_ALWAYS_FACE_TARGET;
            CanFallOffLedges = EnemyEV.ICE_WIZARD_BASIC_CAN_FALL_OFF_LEDGES;
            CanBeKnockedBack = EnemyEV.ICE_WIZARD_BASIC_CAN_BE_KNOCKED_BACK;
            IsWeighted = EnemyEV.ICE_WIZARD_BASIC_IS_WEIGHTED;

            Scale = EnemyEV.IceWizardBasicScale;
            ProjectileScale = EnemyEV.IceWizardBasicProjectileScale;
            TintablePart.TextureColor = EnemyEV.IceWizardBasicTint;

            MeleeRadius = EnemyEV.ICE_WIZARD_BASIC_MELEE_RADIUS;
            ProjectileRadius = EnemyEV.ICE_WIZARD_BASIC_PROJECTILE_RADIUS;
            EngageRadius = EnemyEV.ICE_WIZARD_BASIC_ENGAGE_RADIUS;

            ProjectileDamage = Damage;
            KnockBack = EnemyEV.IceWizardBasicKnockBack;
            #endregion

            switch (Difficulty)
            {
                case (GameTypes.EnemyDifficulty.MINIBOSS):
                    #region Miniboss Variables - General
                    Name = EnemyEV.ICE_WIZARD_MINIBOSS_NAME;
                    LocStringID = EnemyEV.ICE_WIZARD_MINIBOSS_NAME_LOC_ID;

                    MaxHealth = EnemyEV.ICE_WIZARD_MINIBOSS_MAX_HEALTH;
                    Damage = EnemyEV.ICE_WIZARD_MINIBOSS_DAMAGE;
                    XPValue = EnemyEV.ICE_WIZARD_MINIBOSS_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.ICE_WIZARD_MINIBOSS_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.ICE_WIZARD_MINIBOSS_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.ICE_WIZARD_MINIBOSS_DROP_CHANCE;

                    Speed = EnemyEV.ICE_WIZARD_MINIBOSS_SPEED;
                    TurnSpeed = EnemyEV.ICE_WIZARD_MINIBOSS_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.ICE_WIZARD_MINIBOSS_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.ICE_WIZARD_MINIBOSS_JUMP;
                    CooldownTime = EnemyEV.ICE_WIZARD_MINIBOSS_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.ICE_WIZARD_MINIBOSS_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.ICE_WIZARD_MINIBOSS_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.ICE_WIZARD_MINIBOSS_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.ICE_WIZARD_MINIBOSS_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.ICE_WIZARD_MINIBOSS_IS_WEIGHTED;

                    Scale = EnemyEV.IceWizardMinibossScale;
                    ProjectileScale = EnemyEV.IceWizardMinibossProjectileScale;
                    TintablePart.TextureColor = EnemyEV.IceWizardMinibossTint;

                    MeleeRadius = EnemyEV.ICE_WIZARD_MINIBOSS_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.ICE_WIZARD_MINIBOSS_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.ICE_WIZARD_MINIBOSS_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.IceWizardMinibossKnockBack;
                    #endregion

                    break;

                case (GameTypes.EnemyDifficulty.EXPERT):
                    SpellProjectileCount = 8;//14;
                    SpellDelay = 1.0f;
                    m_spellOffset = new Vector2(40, -130);
                    #region Expert Variables - General
                    Name = EnemyEV.ICE_WIZARD_EXPERT_NAME;
                    LocStringID = EnemyEV.ICE_WIZARD_EXPERT_NAME_LOC_ID;

                    MaxHealth = EnemyEV.ICE_WIZARD_EXPERT_MAX_HEALTH;
                    Damage = EnemyEV.ICE_WIZARD_EXPERT_DAMAGE;
                    XPValue = EnemyEV.ICE_WIZARD_EXPERT_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.ICE_WIZARD_EXPERT_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.ICE_WIZARD_EXPERT_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.ICE_WIZARD_EXPERT_DROP_CHANCE;

                    Speed = EnemyEV.ICE_WIZARD_EXPERT_SPEED;
                    TurnSpeed = EnemyEV.ICE_WIZARD_EXPERT_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.ICE_WIZARD_EXPERT_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.ICE_WIZARD_EXPERT_JUMP;
                    CooldownTime = EnemyEV.ICE_WIZARD_EXPERT_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.ICE_WIZARD_EXPERT_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.ICE_WIZARD_EXPERT_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.ICE_WIZARD_EXPERT_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.ICE_WIZARD_EXPERT_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.ICE_WIZARD_EXPERT_IS_WEIGHTED;

                    Scale = EnemyEV.IceWizardExpertScale;
                    ProjectileScale = EnemyEV.IceWizardExpertProjectileScale;
                    TintablePart.TextureColor = EnemyEV.IceWizardExpertTint;

                    MeleeRadius = EnemyEV.ICE_WIZARD_EXPERT_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.ICE_WIZARD_EXPERT_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.ICE_WIZARD_EXPERT_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.IceWizardExpertKnockBack;
                    #endregion
                    IceScale = new Vector2(2, 2);
                    break;

                case (GameTypes.EnemyDifficulty.ADVANCED):

                    SpellProjectileCount = 14; //14;
					#region Advanced Variables - General
					Name = EnemyEV.ICE_WIZARD_ADVANCED_NAME;
                    LocStringID = EnemyEV.ICE_WIZARD_ADVANCED_NAME_LOC_ID;
					
					MaxHealth = EnemyEV.ICE_WIZARD_ADVANCED_MAX_HEALTH;
					Damage = EnemyEV.ICE_WIZARD_ADVANCED_DAMAGE;
					XPValue = EnemyEV.ICE_WIZARD_ADVANCED_XP_VALUE;
					
					MinMoneyDropAmount = EnemyEV.ICE_WIZARD_ADVANCED_MIN_DROP_AMOUNT;
					MaxMoneyDropAmount = EnemyEV.ICE_WIZARD_ADVANCED_MAX_DROP_AMOUNT;
					MoneyDropChance = EnemyEV.ICE_WIZARD_ADVANCED_DROP_CHANCE;
					
					Speed = EnemyEV.ICE_WIZARD_ADVANCED_SPEED;
					TurnSpeed = EnemyEV.ICE_WIZARD_ADVANCED_TURN_SPEED;
					ProjectileSpeed = EnemyEV.ICE_WIZARD_ADVANCED_PROJECTILE_SPEED;
					JumpHeight = EnemyEV.ICE_WIZARD_ADVANCED_JUMP;
					CooldownTime = EnemyEV.ICE_WIZARD_ADVANCED_COOLDOWN;
					AnimationDelay = 1 / EnemyEV.ICE_WIZARD_ADVANCED_ANIMATION_DELAY;
					
					AlwaysFaceTarget = EnemyEV.ICE_WIZARD_ADVANCED_ALWAYS_FACE_TARGET;
					CanFallOffLedges = EnemyEV.ICE_WIZARD_ADVANCED_CAN_FALL_OFF_LEDGES;
					CanBeKnockedBack = EnemyEV.ICE_WIZARD_ADVANCED_CAN_BE_KNOCKED_BACK;
					IsWeighted = EnemyEV.ICE_WIZARD_ADVANCED_IS_WEIGHTED;
					
					Scale = EnemyEV.IceWizardAdvancedScale;
					ProjectileScale = EnemyEV.IceWizardAdvancedProjectileScale;
					TintablePart.TextureColor = EnemyEV.IceWizardAdvancedTint;
					
					MeleeRadius = EnemyEV.ICE_WIZARD_ADVANCED_MELEE_RADIUS;
					EngageRadius = EnemyEV.ICE_WIZARD_ADVANCED_ENGAGE_RADIUS;
					ProjectileRadius = EnemyEV.ICE_WIZARD_ADVANCED_PROJECTILE_RADIUS;
					
					ProjectileDamage = Damage;
					KnockBack = EnemyEV.IceWizardAdvancedKnockBack;
					#endregion
                    m_spellOffset = new Vector2(40, -100);
                    IceScale = new Vector2(1.5f, 1.5f);
                    break;

                case (GameTypes.EnemyDifficulty.BASIC):
                default:
                    break;
            }		

        }

        protected override void InitializeLogic()
        {
            LogicSet moveTowardsLS = new LogicSet(this);
            moveTowardsLS.AddAction(new ChangeSpriteLogicAction("EnemyWizardIdle_Character", true, true));
            moveTowardsLS.AddAction(new ChaseLogicAction(m_target, new Vector2(-255, -175), new Vector2(255, -75), true, MoveDuration));

            LogicSet moveAwayLS = new LogicSet(this);
            moveAwayLS.AddAction(new ChangeSpriteLogicAction("EnemyWizardIdle_Character", true, true));
            moveAwayLS.AddAction(new ChaseLogicAction(m_target, false, 1f));

            LogicSet walkStopLS = new LogicSet(this);
            walkStopLS.AddAction(new ChangeSpriteLogicAction("EnemyWizardIdle_Character", true, true));
            walkStopLS.AddAction(new MoveLogicAction(m_target, true, 0));
            walkStopLS.AddAction(new DelayLogicAction(0.5f));

            LogicSet castSpellLS = new LogicSet(this);
            castSpellLS.AddAction(new MoveLogicAction(m_target, true, 0));
            castSpellLS.AddAction(new LockFaceDirectionLogicAction(true));
            castSpellLS.AddAction(new ChangeSpriteLogicAction("EnemyWizardSpell_Character"));
            castSpellLS.AddAction(new PlayAnimationLogicAction("Start", "BeforeSpell"));
            castSpellLS.AddAction(new RunFunctionLogicAction(this, "SummonIceball", null));
            castSpellLS.AddAction(new DelayLogicAction(SpellDelay));
            castSpellLS.AddAction(new RunFunctionLogicAction(this, "ShatterIceball", SpellProjectileCount));
            castSpellLS.AddAction(new PlayAnimationLogicAction("CastSpell", "End"), Types.Sequence.Parallel);
            castSpellLS.AddAction(new DelayLogicAction(0.5f));
            castSpellLS.AddAction(new RunFunctionLogicAction(this, "ResetIceball", null));
            castSpellLS.AddAction(new DelayLogicAction(0.5f));
            castSpellLS.AddAction(new LockFaceDirectionLogicAction(false));
            castSpellLS.Tag = GameTypes.LogicSetType_ATTACK;

            LogicSet castSpellExpertLS = new LogicSet(this);
            castSpellExpertLS.AddAction(new MoveLogicAction(m_target, true, 0));
            castSpellExpertLS.AddAction(new LockFaceDirectionLogicAction(true));
            castSpellExpertLS.AddAction(new ChangeSpriteLogicAction("EnemyWizardSpell_Character"));
            castSpellExpertLS.AddAction(new PlayAnimationLogicAction("Start", "BeforeSpell"));
            castSpellExpertLS.AddAction(new RunFunctionLogicAction(this, "SummonIceball", null));
            castSpellExpertLS.AddAction(new DelayLogicAction(SpellDelay));
            castSpellExpertLS.AddAction(new RunFunctionLogicAction(this, "ShatterExpertIceball", SpellProjectileCount));
            castSpellExpertLS.AddAction(new DelayLogicAction(0.135f));
            castSpellExpertLS.AddAction(new RunFunctionLogicAction(this, "ShatterExpertIceball", SpellProjectileCount));
            castSpellExpertLS.AddAction(new DelayLogicAction(0.135f));
            castSpellExpertLS.AddAction(new RunFunctionLogicAction(this, "ShatterExpertIceball", SpellProjectileCount));
            castSpellExpertLS.AddAction(new DelayLogicAction(0.135f));
            castSpellExpertLS.AddAction(new RunFunctionLogicAction(this, "ShatterExpertIceball", SpellProjectileCount));
            castSpellExpertLS.AddAction(new DelayLogicAction(0.135f));
            castSpellExpertLS.AddAction(new RunFunctionLogicAction(this, "ShatterExpertIceball", SpellProjectileCount));
            castSpellExpertLS.AddAction(new DelayLogicAction(0.135f));
            castSpellExpertLS.AddAction(new RunFunctionLogicAction(this, "ShatterExpertIceball", SpellProjectileCount));
            castSpellExpertLS.AddAction(new DelayLogicAction(0.135f));
            castSpellExpertLS.AddAction(new RunFunctionLogicAction(this, "ShatterExpertIceball", SpellProjectileCount));
            castSpellExpertLS.AddAction(new PlayAnimationLogicAction("CastSpell", "End"), Types.Sequence.Parallel);
            castSpellExpertLS.AddAction(new DelayLogicAction(0.5f));
            castSpellExpertLS.AddAction(new RunFunctionLogicAction(this, "ResetIceball", null));
            castSpellExpertLS.AddAction(new DelayLogicAction(0.5f));
            castSpellExpertLS.AddAction(new LockFaceDirectionLogicAction(false));
            castSpellExpertLS.Tag = GameTypes.LogicSetType_ATTACK;

            LogicSet teleportLS = new LogicSet(this);
            teleportLS.AddAction(new MoveLogicAction(m_target, true, 0));
            teleportLS.AddAction(new LockFaceDirectionLogicAction(true));
            teleportLS.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 1 / 30f));
            teleportLS.AddAction(new ChangeSpriteLogicAction("EnemyWizardTeleportOut_Character", false, false));
            teleportLS.AddAction(new PlayAnimationLogicAction("Start", "BeforeTeleport"));
            teleportLS.AddAction(new DelayLogicAction(TeleportDelay));
            teleportLS.AddAction(new PlayAnimationLogicAction("TeleportStart", "End"));
            teleportLS.AddAction(new ChangePropertyLogicAction(this, "IsCollidable", false));
            teleportLS.AddAction(new DelayLogicAction(TeleportDuration));
            teleportLS.AddAction(new TeleportLogicAction(m_target, new Vector2(-400, -400), new Vector2(400, 400)));
            teleportLS.AddAction(new ChangePropertyLogicAction(this, "IsCollidable", true));
            teleportLS.AddAction(new ChangeSpriteLogicAction("EnemyWizardTeleportIn_Character", true, false));
            teleportLS.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 1 / 10f));
            teleportLS.AddAction(new LockFaceDirectionLogicAction(false));
            teleportLS.AddAction(new DelayLogicAction(0.5f));


            m_generalBasicLB.AddLogicSet(moveTowardsLS, moveAwayLS, walkStopLS, castSpellLS, teleportLS);
            m_generalAdvancedLB.AddLogicSet(moveTowardsLS, moveAwayLS, walkStopLS, castSpellLS, teleportLS);
            m_generalExpertLB.AddLogicSet(moveTowardsLS, moveAwayLS, walkStopLS, castSpellExpertLS, teleportLS);
            m_generalCooldownLB.AddLogicSet(moveTowardsLS, moveAwayLS, walkStopLS);

            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);

            SetCooldownLogicBlock(m_generalCooldownLB, 100, 0, 0); //moveTowardsLS, moveAwayLS, walkStopLS

            base.InitializeLogic();
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case (STATE_MELEE_ENGAGE):
                case (STATE_PROJECTILE_ENGAGE):
                    RunLogicBlock(true, m_generalBasicLB, 40, 0, 0, 60, 0); // moveTowardsLS, moveAwayLS, walkStopLS castSpellLS, teleportLS
                    break;
                case (STATE_ENGAGE):
                    RunLogicBlock(true, m_generalBasicLB, 100, 0, 0, 0, 0); // moveTowardsLS, moveAwayLS, walkStopLS castSpellLS, teleportLS
                    break;
                case (STATE_WANDER):
                    RunLogicBlock(true, m_generalBasicLB, 0, 0, 100, 0, 0); // moveTowardsLS, moveAwayLS, walkStopLS castSpellLS, teleportLS
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
                    RunLogicBlock(true, m_generalBasicLB, 40, 0, 0, 60, 0); // moveTowardsLS, moveAwayLS, walkStopLS castSpellLS, teleportLS
                    break;
                case (STATE_ENGAGE):
                    RunLogicBlock(true, m_generalBasicLB, 100, 0, 0, 0, 0); // moveTowardsLS, moveAwayLS, walkStopLS castSpellLS, teleportLS
                    break;
                case (STATE_WANDER):
                    RunLogicBlock(true, m_generalBasicLB, 0, 0, 100, 0, 0); // moveTowardsLS, moveAwayLS, walkStopLS castSpellLS, teleportLS
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
                    RunLogicBlock(true, m_generalExpertLB, 40, 0, 0, 60, 0); // moveTowardsLS, moveAwayLS, walkStopLS castSpellLS, teleportLS
                    break;
                case (STATE_ENGAGE):
                    RunLogicBlock(true, m_generalExpertLB, 100, 0, 0, 0, 0); // moveTowardsLS, moveAwayLS, walkStopLS castSpellLS, teleportLS
                    break;
                case (STATE_WANDER):
                    RunLogicBlock(true, m_generalExpertLB, 0, 0, 100, 0, 0); // moveTowardsLS, moveAwayLS, walkStopLS castSpellLS, teleportLS
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
                default:
                    break;
            }
        }

        public EnemyObj_IceWizard(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("EnemyWizardIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            this.PlayAnimation(true);
            this.TintablePart = _objectList[0];
            this.Type = EnemyType.IceWizard;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (m_iceballSummon != null)
            {
                if (this.Flip == Microsoft.Xna.Framework.Graphics.SpriteEffects.None)
                    m_iceballSummon.Position = new Vector2(this.X + m_spellOffset.X, this.Y + m_spellOffset.Y);
                else
                    m_iceballSummon.Position = new Vector2(this.X - m_spellOffset.X, this.Y + m_spellOffset.Y);
            }

            if (m_iceParticleEffectCounter > 0)
            {
                m_iceParticleEffectCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (m_iceParticleEffectCounter <= 0)
                {
                    m_levelScreen.ImpactEffectPool.DisplayIceParticleEffect(this);
                    m_iceParticleEffectCounter = 0.15f;
                }
            }
        }

        public void SummonIceball()
        {
            ResetIceball();

            ProjectileData projData = new ProjectileData(this)
            {
                SpriteName = "WizardIceSpell_Sprite",
                SourceAnchor = m_spellOffset,
                Target = null,
                Speed = new Vector2(0, 0),
                IsWeighted = false,
                RotationSpeed = 0,
                Damage = Damage,
                AngleOffset = 0,
                CollidesWithTerrain = false,
                DestroysWithEnemy = false,
                Scale = IceScale,
                LockPosition = true,
            };

            SoundManager.Play3DSound(this, m_target,"Ice_Wizard_Form");
            //SoundManager.PlaySound(this, m_target, "Ice_Wizard_Form");

            //m_iceballSummon = m_levelScreen.ProjectileManager.FireProjectile("WizardIceSpell_Sprite", this, m_spellOffset, 0, 0, false, 0, ProjectileDamage);
            m_iceballSummon = m_levelScreen.ProjectileManager.FireProjectile(projData);
            m_iceballSummon.PlayAnimation("Start", "Grown");

            projData.Dispose();
        }

        public void ShatterIceball(int numIceballs)
        {
            SoundManager.Play3DSound(this, m_target, "Ice_Wizard_Attack_Glass");

            if (m_iceballSummon.SpriteName == "WizardIceSpell_Sprite") // Temporary hack fix for crashing iceballs.
                m_iceballSummon.PlayAnimation("Grown", "End");

            ProjectileData projData = new ProjectileData(this)
            {
                SpriteName = "WizardIceProjectile_Sprite",
                SourceAnchor = m_spellOffset,
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0,
                Damage = Damage,
                AngleOffset = 0,
                CollidesWithTerrain = false,
                Scale = ProjectileScale,
            };

            float angle = 0;
            float angleDiff = 360 / numIceballs;

            for (int i = 0; i < numIceballs; i++)
            {
                projData.Angle = new Vector2(angle, angle);
                ProjectileObj iceball = m_levelScreen.ProjectileManager.FireProjectile(projData);
                Tweener.Tween.RunFunction(0.15f, this, "ChangeIceballState", iceball);
                angle += angleDiff;

            }

            projData.Dispose();
        }

        public void ShatterExpertIceball(int numIceballs)
        {
            SoundManager.Play3DSound(this, m_target, "Ice_Wizard_Attack");

            if (m_iceballSummon.SpriteName == "WizardIceSpell_Sprite") // Temporary hack fix for crashing iceballs.
                m_iceballSummon.PlayAnimation("Grown", "End");

            ProjectileData projData = new ProjectileData(this)
            {
                SpriteName = "WizardIceProjectile_Sprite",
                SourceAnchor = m_spellOffset,
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0,
                Damage = Damage,
                AngleOffset = 0,
                CollidesWithTerrain = false,
                Scale = ProjectileScale,
            };

            float angle = 0;
            float angleDiff = 60;


            for (int i = 0; i < numIceballs; i++)
            {
                angle = CDGMath.RandomInt(0, 360);
                projData.Angle = new Vector2(angle, angle);

                ProjectileObj iceball = m_levelScreen.ProjectileManager.FireProjectile(projData);
                Tweener.Tween.RunFunction(0.15f, this, "ChangeIceballState", iceball);
                angle += angleDiff;

            }

            projData.Dispose();
        }

        public void ChangeIceballState(ProjectileObj iceball)
        {
            iceball.CollidesWithTerrain = true;
        }

        public void ResetIceball()
        {
            if (m_iceballSummon != null)
            {
                m_levelScreen.ProjectileManager.DestroyProjectile(m_iceballSummon);
                m_iceballSummon = null;
            }
        }

        //public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
        //{
        //    base.HitEnemy(damage, position, isPlayer);
        //    if (m_currentActiveLB != null && m_currentActiveLB.IsActive == true)
        //    {
        //        m_currentActiveLB.StopLogicBlock();
        //        ResetIceball();
        //    }
        //}

        public override void Kill(bool giveXP = true)
        {
            if (m_currentActiveLB != null && m_currentActiveLB.IsActive == true)
            {
                m_currentActiveLB.StopLogicBlock();
                ResetIceball();
            }
            base.Kill(giveXP);
        }

        public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
        {
            if (otherBox.AbsParent is PlayerObj)
                this.CurrentSpeed = 0;
            if (collisionResponseType != Consts.COLLISIONRESPONSE_TERRAIN)
                base.CollisionResponse(thisBox, otherBox, collisionResponseType);
            else if ((otherBox.AbsParent is PlayerObj) == false)// Add this else to turn on terrain collision.
            {
                IPhysicsObj otherPhysicsObj = otherBox.AbsParent as IPhysicsObj;
                if (otherPhysicsObj.CollidesBottom == true && otherPhysicsObj.CollidesTop == true && otherPhysicsObj.CollidesLeft == true && otherPhysicsObj.CollidesRight == true)
                    this.Position += CollisionMath.RotatedRectIntersectsMTD(thisBox.AbsRect, thisBox.AbsRotation, Vector2.Zero, otherBox.AbsRect, otherBox.AbsRotation, Vector2.Zero);
            }
        }

        public override void ResetState()
        {
            ResetIceball();
            base.ResetState();
        }

        public override void Dispose()
        {
            if (IsDisposed == false)
            {
                // Done
                m_iceballSummon = null;
                base.Dispose();
            }
        }
    }
}
