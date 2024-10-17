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
    public class EnemyObj_FireWizard : EnemyObj
    {

        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();
        private LogicBlock m_generalCooldownLB = new LogicBlock();

        private float SpellDelay = 0.7f;
        private float SpellInterval = 0.5f;

        private ProjectileObj m_fireballSummon;
        private Vector2 m_spellOffset = new Vector2(40, -80);
        private float TeleportDelay = 0.5f;
        private float TeleportDuration = 1.0f;
        private float MoveDuration = 1.0f;
        private float m_fireParticleEffectCounter = 0.5f;

        protected override void InitializeEV()
        {
            SpellInterval = 0.5f;

            #region Basic Variables - General
            Name = EnemyEV.FIRE_WIZARD_BASIC_NAME;
            LocStringID = EnemyEV.FIRE_WIZARD_BASIC_NAME_LOC_ID;

            MaxHealth = EnemyEV.FIRE_WIZARD_BASIC_MAX_HEALTH;
            Damage = EnemyEV.FIRE_WIZARD_BASIC_DAMAGE;
            XPValue = EnemyEV.FIRE_WIZARD_BASIC_XP_VALUE;

            MinMoneyDropAmount = EnemyEV.FIRE_WIZARD_BASIC_MIN_DROP_AMOUNT;
            MaxMoneyDropAmount = EnemyEV.FIRE_WIZARD_BASIC_MAX_DROP_AMOUNT;
            MoneyDropChance = EnemyEV.FIRE_WIZARD_BASIC_DROP_CHANCE;

            Speed = EnemyEV.FIRE_WIZARD_BASIC_SPEED;
            TurnSpeed = EnemyEV.FIRE_WIZARD_BASIC_TURN_SPEED;
            ProjectileSpeed = EnemyEV.FIRE_WIZARD_BASIC_PROJECTILE_SPEED;
            JumpHeight = EnemyEV.FIRE_WIZARD_BASIC_JUMP;
            CooldownTime = EnemyEV.FIRE_WIZARD_BASIC_COOLDOWN;
            AnimationDelay = 1 / EnemyEV.FIRE_WIZARD_BASIC_ANIMATION_DELAY;

            AlwaysFaceTarget = EnemyEV.FIRE_WIZARD_BASIC_ALWAYS_FACE_TARGET;
            CanFallOffLedges = EnemyEV.FIRE_WIZARD_BASIC_CAN_FALL_OFF_LEDGES;
            CanBeKnockedBack = EnemyEV.FIRE_WIZARD_BASIC_CAN_BE_KNOCKED_BACK;
            IsWeighted = EnemyEV.FIRE_WIZARD_BASIC_IS_WEIGHTED;

            Scale = EnemyEV.FireWizardBasicScale;
            ProjectileScale = EnemyEV.FireWizardBasicProjectileScale;
            TintablePart.TextureColor = EnemyEV.FireWizardBasicTint;

            MeleeRadius = EnemyEV.FIRE_WIZARD_BASIC_MELEE_RADIUS;
            ProjectileRadius = EnemyEV.FIRE_WIZARD_BASIC_PROJECTILE_RADIUS;
            EngageRadius = EnemyEV.FIRE_WIZARD_BASIC_ENGAGE_RADIUS;

            ProjectileDamage = Damage;
            KnockBack = EnemyEV.FireWizardBasicKnockBack;
            #endregion

            switch (Difficulty)
            {
                case (GameTypes.EnemyDifficulty.MINIBOSS):
                    #region Miniboss Variables - General
                    Name = EnemyEV.FIRE_WIZARD_MINIBOSS_NAME;
                    LocStringID = EnemyEV.FIRE_WIZARD_MINIBOSS_NAME_LOC_ID;

                    MaxHealth = EnemyEV.FIRE_WIZARD_MINIBOSS_MAX_HEALTH;
                    Damage = EnemyEV.FIRE_WIZARD_MINIBOSS_DAMAGE;
                    XPValue = EnemyEV.FIRE_WIZARD_MINIBOSS_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.FIRE_WIZARD_MINIBOSS_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.FIRE_WIZARD_MINIBOSS_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.FIRE_WIZARD_MINIBOSS_DROP_CHANCE;

                    Speed = EnemyEV.FIRE_WIZARD_MINIBOSS_SPEED;
                    TurnSpeed = EnemyEV.FIRE_WIZARD_MINIBOSS_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.FIRE_WIZARD_MINIBOSS_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.FIRE_WIZARD_MINIBOSS_JUMP;
                    CooldownTime = EnemyEV.FIRE_WIZARD_MINIBOSS_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.FIRE_WIZARD_MINIBOSS_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.FIRE_WIZARD_MINIBOSS_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.FIRE_WIZARD_MINIBOSS_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.FIRE_WIZARD_MINIBOSS_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.FIRE_WIZARD_MINIBOSS_IS_WEIGHTED;

                    Scale = EnemyEV.FireWizardMinibossScale;
                    ProjectileScale = EnemyEV.FireWizardMinibossProjectileScale;
                    TintablePart.TextureColor = EnemyEV.FireWizardMinibossTint;

                    MeleeRadius = EnemyEV.FIRE_WIZARD_MINIBOSS_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.FIRE_WIZARD_MINIBOSS_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.FIRE_WIZARD_MINIBOSS_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.FireWizardMinibossKnockBack;
                    #endregion
                    break;


                case (GameTypes.EnemyDifficulty.EXPERT):
                    m_spellOffset = new Vector2(40, -130);
                    SpellDelay = 1.0f;
                    SpellInterval = 1.0f;//0.5f;

					#region Expert Variables - General
					Name = EnemyEV.FIRE_WIZARD_EXPERT_NAME;
                    LocStringID = EnemyEV.FIRE_WIZARD_EXPERT_NAME_LOC_ID;
					
					MaxHealth = EnemyEV.FIRE_WIZARD_EXPERT_MAX_HEALTH;
					Damage = EnemyEV.FIRE_WIZARD_EXPERT_DAMAGE;
					XPValue = EnemyEV.FIRE_WIZARD_EXPERT_XP_VALUE;
					
					MinMoneyDropAmount = EnemyEV.FIRE_WIZARD_EXPERT_MIN_DROP_AMOUNT;
					MaxMoneyDropAmount = EnemyEV.FIRE_WIZARD_EXPERT_MAX_DROP_AMOUNT;
					MoneyDropChance = EnemyEV.FIRE_WIZARD_EXPERT_DROP_CHANCE;
					
					Speed = EnemyEV.FIRE_WIZARD_EXPERT_SPEED;
					TurnSpeed = EnemyEV.FIRE_WIZARD_EXPERT_TURN_SPEED;
					ProjectileSpeed = EnemyEV.FIRE_WIZARD_EXPERT_PROJECTILE_SPEED;
					JumpHeight = EnemyEV.FIRE_WIZARD_EXPERT_JUMP;
					CooldownTime = EnemyEV.FIRE_WIZARD_EXPERT_COOLDOWN;
					AnimationDelay = 1 / EnemyEV.FIRE_WIZARD_EXPERT_ANIMATION_DELAY;
					
					AlwaysFaceTarget = EnemyEV.FIRE_WIZARD_EXPERT_ALWAYS_FACE_TARGET;
					CanFallOffLedges = EnemyEV.FIRE_WIZARD_EXPERT_CAN_FALL_OFF_LEDGES;
					CanBeKnockedBack = EnemyEV.FIRE_WIZARD_EXPERT_CAN_BE_KNOCKED_BACK;
					IsWeighted = EnemyEV.FIRE_WIZARD_EXPERT_IS_WEIGHTED;
					
					Scale = EnemyEV.FireWizardExpertScale;
					ProjectileScale = EnemyEV.FireWizardExpertProjectileScale;
					TintablePart.TextureColor = EnemyEV.FireWizardExpertTint;
					
					MeleeRadius = EnemyEV.FIRE_WIZARD_EXPERT_MELEE_RADIUS;
					ProjectileRadius = EnemyEV.FIRE_WIZARD_EXPERT_PROJECTILE_RADIUS;
					EngageRadius = EnemyEV.FIRE_WIZARD_EXPERT_ENGAGE_RADIUS;
					
					ProjectileDamage = Damage;
					KnockBack = EnemyEV.FireWizardExpertKnockBack;
					#endregion
                    break;

                case (GameTypes.EnemyDifficulty.ADVANCED):
                    SpellInterval = 0.15f;

					#region Advanced Variables - General
					Name = EnemyEV.FIRE_WIZARD_ADVANCED_NAME;
                    LocStringID = EnemyEV.FIRE_WIZARD_ADVANCED_NAME_LOC_ID;
					
					MaxHealth = EnemyEV.FIRE_WIZARD_ADVANCED_MAX_HEALTH;
					Damage = EnemyEV.FIRE_WIZARD_ADVANCED_DAMAGE;
					XPValue = EnemyEV.FIRE_WIZARD_ADVANCED_XP_VALUE;
					
					MinMoneyDropAmount = EnemyEV.FIRE_WIZARD_ADVANCED_MIN_DROP_AMOUNT;
					MaxMoneyDropAmount = EnemyEV.FIRE_WIZARD_ADVANCED_MAX_DROP_AMOUNT;
					MoneyDropChance = EnemyEV.FIRE_WIZARD_ADVANCED_DROP_CHANCE;
					
					Speed = EnemyEV.FIRE_WIZARD_ADVANCED_SPEED;
					TurnSpeed = EnemyEV.FIRE_WIZARD_ADVANCED_TURN_SPEED;
					ProjectileSpeed = EnemyEV.FIRE_WIZARD_ADVANCED_PROJECTILE_SPEED;
					JumpHeight = EnemyEV.FIRE_WIZARD_ADVANCED_JUMP;
					CooldownTime = EnemyEV.FIRE_WIZARD_ADVANCED_COOLDOWN;
					AnimationDelay = 1 / EnemyEV.FIRE_WIZARD_ADVANCED_ANIMATION_DELAY;
					
					AlwaysFaceTarget = EnemyEV.FIRE_WIZARD_ADVANCED_ALWAYS_FACE_TARGET;
					CanFallOffLedges = EnemyEV.FIRE_WIZARD_ADVANCED_CAN_FALL_OFF_LEDGES;
					CanBeKnockedBack = EnemyEV.FIRE_WIZARD_ADVANCED_CAN_BE_KNOCKED_BACK;
					IsWeighted = EnemyEV.FIRE_WIZARD_ADVANCED_IS_WEIGHTED;
					
					Scale = EnemyEV.FireWizardAdvancedScale;
					ProjectileScale = EnemyEV.FireWizardAdvancedProjectileScale;
					TintablePart.TextureColor = EnemyEV.FireWizardAdvancedTint;
					
					MeleeRadius = EnemyEV.FIRE_WIZARD_ADVANCED_MELEE_RADIUS;
					EngageRadius = EnemyEV.FIRE_WIZARD_ADVANCED_ENGAGE_RADIUS;
					ProjectileRadius = EnemyEV.FIRE_WIZARD_ADVANCED_PROJECTILE_RADIUS;
					
					ProjectileDamage = Damage;
					KnockBack = EnemyEV.FireWizardAdvancedKnockBack;
					#endregion
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
            moveAwayLS.AddAction(new ChaseLogicAction(m_target, false, MoveDuration));

            LogicSet walkStopLS = new LogicSet(this);
            walkStopLS.AddAction(new ChangeSpriteLogicAction("EnemyWizardIdle_Character", true, true));
            walkStopLS.AddAction(new MoveLogicAction(m_target, true, 0));
            walkStopLS.AddAction(new DelayLogicAction(0.5f));

            LogicSet castSpellLS = new LogicSet(this);
            castSpellLS.AddAction(new MoveLogicAction(m_target, true, 0));
            castSpellLS.AddAction(new LockFaceDirectionLogicAction(true));
            castSpellLS.AddAction(new ChangeSpriteLogicAction("EnemyWizardSpell_Character"));
            castSpellLS.AddAction(new PlayAnimationLogicAction("Start", "BeforeSpell"));
            castSpellLS.AddAction(new RunFunctionLogicAction(this, "SummonFireball", null));
            castSpellLS.AddAction(new DelayLogicAction(SpellDelay));
            castSpellLS.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            castSpellLS.AddAction(new DelayLogicAction(SpellInterval));
            castSpellLS.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            castSpellLS.AddAction(new DelayLogicAction(SpellInterval));
            castSpellLS.AddAction(new RunFunctionLogicAction(this, "ResetFireball", null));
            castSpellLS.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            castSpellLS.AddAction(new DelayLogicAction(0.5f));
            castSpellLS.AddAction(new LockFaceDirectionLogicAction(false));
            castSpellLS.Tag = GameTypes.LogicSetType_ATTACK;

            LogicSet castAdvancedSpellLS = new LogicSet(this);
            castAdvancedSpellLS.AddAction(new MoveLogicAction(m_target, true, 0));
            castAdvancedSpellLS.AddAction(new LockFaceDirectionLogicAction(true));
            castAdvancedSpellLS.AddAction(new ChangeSpriteLogicAction("EnemyWizardSpell_Character"));
            castAdvancedSpellLS.AddAction(new PlayAnimationLogicAction("Start", "BeforeSpell"));
            castAdvancedSpellLS.AddAction(new RunFunctionLogicAction(this, "SummonFireball", null));
            castAdvancedSpellLS.AddAction(new DelayLogicAction(SpellDelay));
            castAdvancedSpellLS.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            castAdvancedSpellLS.AddAction(new DelayLogicAction(SpellInterval));
            castAdvancedSpellLS.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            castAdvancedSpellLS.AddAction(new DelayLogicAction(SpellInterval));
            castAdvancedSpellLS.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            castAdvancedSpellLS.AddAction(new DelayLogicAction(SpellInterval));
            castAdvancedSpellLS.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            castAdvancedSpellLS.AddAction(new DelayLogicAction(SpellInterval));
            castAdvancedSpellLS.AddAction(new RunFunctionLogicAction(this, "ResetFireball", null));
            castAdvancedSpellLS.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            castAdvancedSpellLS.AddAction(new DelayLogicAction(0.5f));
            castAdvancedSpellLS.AddAction(new LockFaceDirectionLogicAction(false));
            castAdvancedSpellLS.Tag = GameTypes.LogicSetType_ATTACK;

            LogicSet castExpertSpellLS = new LogicSet(this);
            castExpertSpellLS.AddAction(new MoveLogicAction(m_target, true, 0));
            castExpertSpellLS.AddAction(new LockFaceDirectionLogicAction(true));
            castExpertSpellLS.AddAction(new ChangeSpriteLogicAction("EnemyWizardSpell_Character"));
            castExpertSpellLS.AddAction(new PlayAnimationLogicAction("Start", "BeforeSpell"));
            castExpertSpellLS.AddAction(new RunFunctionLogicAction(this, "SummonFireball", null));
            castExpertSpellLS.AddAction(new DelayLogicAction(SpellDelay));
            castExpertSpellLS.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            castExpertSpellLS.AddAction(new DelayLogicAction(SpellInterval));
            castExpertSpellLS.AddAction(new RunFunctionLogicAction(this, "CastFireball"));
            castExpertSpellLS.AddAction(new DelayLogicAction(SpellInterval));
            castExpertSpellLS.AddAction(new RunFunctionLogicAction(this, "ResetFireball", null));
            castExpertSpellLS.AddAction(new RunFunctionLogicAction(this, "CastFireball"));            
            castExpertSpellLS.AddAction(new DelayLogicAction(0.5f));
            castExpertSpellLS.AddAction(new LockFaceDirectionLogicAction(false));
            castExpertSpellLS.Tag = GameTypes.LogicSetType_ATTACK;

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
            m_generalAdvancedLB.AddLogicSet(moveTowardsLS, moveAwayLS, walkStopLS, castAdvancedSpellLS, teleportLS);
            m_generalExpertLB.AddLogicSet(moveTowardsLS, moveAwayLS, walkStopLS, castExpertSpellLS, teleportLS);
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
                    RunLogicBlock(true, m_generalAdvancedLB, 40, 0, 0, 60, 0); // moveTowardsLS, moveAwayLS, walkStopLS castSpellLS, teleportLS
                    break;
                case (STATE_ENGAGE):
                    RunLogicBlock(true, m_generalAdvancedLB, 100, 0, 0, 0, 0); // moveTowardsLS, moveAwayLS, walkStopLS castSpellLS, teleportLS
                    break;
                case (STATE_WANDER):
                    RunLogicBlock(true, m_generalAdvancedLB, 0, 0, 100, 0, 0); // moveTowardsLS, moveAwayLS, walkStopLS castSpellLS, teleportLS
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

        public EnemyObj_FireWizard(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("EnemyWizardIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            this.Type = EnemyType.FIRE_WIZARD;
            this.PlayAnimation(true);
            this.TintablePart = _objectList[0];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (m_fireballSummon != null)
            {
                if (this.Flip == Microsoft.Xna.Framework.Graphics.SpriteEffects.None)
                    m_fireballSummon.Position = new Vector2(this.X + m_spellOffset.X, this.Y + m_spellOffset.Y);
                else
                    m_fireballSummon.Position = new Vector2(this.X - m_spellOffset.X, this.Y + m_spellOffset.Y);
            }

            if (m_fireParticleEffectCounter > 0)
            {
                m_fireParticleEffectCounter -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (m_fireParticleEffectCounter <= 0)
                {
                    m_levelScreen.ImpactEffectPool.DisplayFireParticleEffect(this);
                    m_fireParticleEffectCounter = 0.15f;
                }
            }
        }

        public void CastFireball()
        {
            ProjectileData projData = new ProjectileData(this)
            {
                SpriteName = "WizardFireballProjectile_Sprite",
                SourceAnchor = m_spellOffset,
                Target = m_target,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),
                IsWeighted = false,
                RotationSpeed = 0,
                Damage = Damage,
                AngleOffset = 0,
                CollidesWithTerrain = false,
                Scale = ProjectileScale,
            };

            if (this.Difficulty == GameTypes.EnemyDifficulty.ADVANCED)
                projData.AngleOffset = CDGMath.RandomInt(-25, 25);

            if (this.Difficulty == GameTypes.EnemyDifficulty.EXPERT)
            {
                projData.SpriteName = "GhostBossProjectile_Sprite";
                projData.CollidesWithTerrain = false;
            }

            SoundManager.Play3DSound(this, m_target, "FireWizard_Attack_01", "FireWizard_Attack_02", "FireWizard_Attack_03", "FireWizard_Attack_04");
            ProjectileObj fireball = m_levelScreen.ProjectileManager.FireProjectile(projData);
            fireball.Rotation = 0;
            if (this.Difficulty != GameTypes.EnemyDifficulty.EXPERT)
                Tweener.Tween.RunFunction(0.15f, this, "ChangeFireballState", fireball);
        }

        public void ChangeFireballState(ProjectileObj fireball)
        {
            fireball.CollidesWithTerrain = true;
        }

        public void SummonFireball()
        {
            ResetFireball();

            ProjectileData projData = new ProjectileData(this)
            {
                SpriteName = "WizardFireballProjectile_Sprite",
                SourceAnchor = m_spellOffset,
                Target = m_target,
                Speed = new Vector2(0, 0),
                IsWeighted = false,
                RotationSpeed = 0,
                Damage = Damage,
                AngleOffset = 0,
                CollidesWithTerrain = false,
                DestroysWithEnemy = false,
                Scale = ProjectileScale,
            };

            if (this.Difficulty == GameTypes.EnemyDifficulty.EXPERT)
            {
                projData.SpriteName = "GhostBossProjectile_Sprite";
                projData.CollidesWithTerrain = false;
            }

            //m_fireballSummon = m_levelScreen.ProjectileManager.FireProjectile("WizardFireballProjectile_Sprite", this, m_spellOffset, 0, 0, false, 0, ProjectileDamage);
            SoundManager.Play3DSound(this, m_target, "Fire_Wizard_Form");
            m_fireballSummon = m_levelScreen.ProjectileManager.FireProjectile(projData);
            m_fireballSummon.Opacity = 0;
            m_fireballSummon.Scale = Vector2.Zero;
            m_fireballSummon.AnimationDelay = 1 / 10f;
            m_fireballSummon.PlayAnimation(true);
            m_fireballSummon.Rotation = 0;

            Tweener.Tween.To(m_fireballSummon, 0.5f, Tweener.Ease.Back.EaseOut, "Opacity", "1", "ScaleX", ProjectileScale.X.ToString(), "ScaleY", ProjectileScale.Y.ToString());

            projData.Dispose();
        }

        public void ResetFireball()
        {
            if (m_fireballSummon != null)
            {
                m_levelScreen.ProjectileManager.DestroyProjectile(m_fireballSummon);
                m_fireballSummon = null;
            }
        }

        //public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
        //{
        //    base.HitEnemy(damage, position, isPlayer);
        //    if (m_currentActiveLB != null && m_currentActiveLB.IsActive == true)
        //    {
        //        m_currentActiveLB.StopLogicBlock();
        //        ResetFireball();
        //    }
        //}

        public override void Kill(bool giveXP = true)
        {
            if (m_currentActiveLB != null && m_currentActiveLB.IsActive == true)
            {
                m_currentActiveLB.StopLogicBlock();
                ResetFireball();
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
            ResetFireball();
            base.ResetState();
        }

        public override void Dispose()
        {
            if (IsDisposed == false)
            {
                // Done
                m_fireballSummon = null;
                base.Dispose();
            }
        }
    }
}
