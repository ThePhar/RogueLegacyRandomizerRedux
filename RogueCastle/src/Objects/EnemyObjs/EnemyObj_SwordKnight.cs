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
    public class EnemyObj_SwordKnight: EnemyObj
    {
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalAdvancedLB = new LogicBlock();
        private LogicBlock m_generalExpertLB = new LogicBlock();
        private LogicBlock m_generalCooldownLB = new LogicBlock();

        private float SlashDelay = 0.0f;
        private float SlashTripleDelay = 1.25f;
        private float TripleAttackSpeed = 500f;
        private FrameSoundObj m_walkSound, m_walkSound2;

        protected override void InitializeEV()
        {
            SlashDelay = 0.25f;
            #region Basic Variables - General
            Name = EnemyEV.SWORD_KNIGHT_BASIC_NAME;
            LocStringID = EnemyEV.SWORD_KNIGHT_BASIC_NAME_LOC_ID;

            MaxHealth = EnemyEV.SWORD_KNIGHT_BASIC_MAX_HEALTH;
            Damage = EnemyEV.SWORD_KNIGHT_BASIC_DAMAGE;
            XPValue = EnemyEV.SWORD_KNIGHT_BASIC_XP_VALUE;

            MinMoneyDropAmount = EnemyEV.SWORD_KNIGHT_BASIC_MIN_DROP_AMOUNT;
            MaxMoneyDropAmount = EnemyEV.SWORD_KNIGHT_BASIC_MAX_DROP_AMOUNT;
            MoneyDropChance = EnemyEV.SWORD_KNIGHT_BASIC_DROP_CHANCE;

            Speed = EnemyEV.SWORD_KNIGHT_BASIC_SPEED;
            TurnSpeed = EnemyEV.SWORD_KNIGHT_BASIC_TURN_SPEED;
            ProjectileSpeed = EnemyEV.SWORD_KNIGHT_BASIC_PROJECTILE_SPEED;
            JumpHeight = EnemyEV.SWORD_KNIGHT_BASIC_JUMP;
            CooldownTime = EnemyEV.SWORD_KNIGHT_BASIC_COOLDOWN;
            AnimationDelay = 1 / EnemyEV.SWORD_KNIGHT_BASIC_ANIMATION_DELAY;

            AlwaysFaceTarget = EnemyEV.SWORD_KNIGHT_BASIC_ALWAYS_FACE_TARGET;
            CanFallOffLedges = EnemyEV.SWORD_KNIGHT_BASIC_CAN_FALL_OFF_LEDGES;
            CanBeKnockedBack = EnemyEV.SWORD_KNIGHT_BASIC_CAN_BE_KNOCKED_BACK;
            IsWeighted = EnemyEV.SWORD_KNIGHT_BASIC_IS_WEIGHTED;

            Scale = EnemyEV.SwordKnightBasicScale;
            ProjectileScale = EnemyEV.SwordKnightBasicProjectileScale;
            TintablePart.TextureColor = EnemyEV.SwordKnightBasicTint;

            MeleeRadius = EnemyEV.SWORD_KNIGHT_BASIC_MELEE_RADIUS;
            ProjectileRadius = EnemyEV.SWORD_KNIGHT_BASIC_PROJECTILE_RADIUS;
            EngageRadius = EnemyEV.SWORD_KNIGHT_BASIC_ENGAGE_RADIUS;

            ProjectileDamage = Damage;
            KnockBack = EnemyEV.SwordKnightBasicKnockBack;
            #endregion


            switch (Difficulty)
            {
                case (GameTypes.EnemyDifficulty.MINIBOSS):
                    ForceDraw = true;
                    SlashDelay = 1.05f;
					#region Miniboss Variables - General
					Name = EnemyEV.SWORD_KNIGHT_MINIBOSS_NAME;
                    LocStringID = EnemyEV.SWORD_KNIGHT_MINIBOSS_NAME_LOC_ID;
					
					MaxHealth = EnemyEV.SWORD_KNIGHT_MINIBOSS_MAX_HEALTH;
					Damage = EnemyEV.SWORD_KNIGHT_MINIBOSS_DAMAGE;
					XPValue = EnemyEV.SWORD_KNIGHT_MINIBOSS_XP_VALUE;
					
					MinMoneyDropAmount = EnemyEV.SWORD_KNIGHT_MINIBOSS_MIN_DROP_AMOUNT;
					MaxMoneyDropAmount = EnemyEV.SWORD_KNIGHT_MINIBOSS_MAX_DROP_AMOUNT;
					MoneyDropChance = EnemyEV.SWORD_KNIGHT_MINIBOSS_DROP_CHANCE;
					
					Speed = EnemyEV.SWORD_KNIGHT_MINIBOSS_SPEED;
					TurnSpeed = EnemyEV.SWORD_KNIGHT_MINIBOSS_TURN_SPEED;
					ProjectileSpeed = EnemyEV.SWORD_KNIGHT_MINIBOSS_PROJECTILE_SPEED;
					JumpHeight = EnemyEV.SWORD_KNIGHT_MINIBOSS_JUMP;
					CooldownTime = EnemyEV.SWORD_KNIGHT_MINIBOSS_COOLDOWN;
					AnimationDelay = 1 / EnemyEV.SWORD_KNIGHT_MINIBOSS_ANIMATION_DELAY;
					
					AlwaysFaceTarget = EnemyEV.SWORD_KNIGHT_MINIBOSS_ALWAYS_FACE_TARGET;
					CanFallOffLedges = EnemyEV.SWORD_KNIGHT_MINIBOSS_CAN_FALL_OFF_LEDGES;
					CanBeKnockedBack = EnemyEV.SWORD_KNIGHT_MINIBOSS_CAN_BE_KNOCKED_BACK;
					IsWeighted = EnemyEV.SWORD_KNIGHT_MINIBOSS_IS_WEIGHTED;
					
					Scale = EnemyEV.SwordKnightMinibossScale;
					ProjectileScale = EnemyEV.SwordKnightMinibossProjectileScale;
					TintablePart.TextureColor = EnemyEV.SwordKnightMinibossTint;
					
					MeleeRadius = EnemyEV.SWORD_KNIGHT_MINIBOSS_MELEE_RADIUS;
					ProjectileRadius = EnemyEV.SWORD_KNIGHT_MINIBOSS_PROJECTILE_RADIUS;
					EngageRadius = EnemyEV.SWORD_KNIGHT_MINIBOSS_ENGAGE_RADIUS;
					
					ProjectileDamage = Damage;
					KnockBack = EnemyEV.SwordKnightMinibossKnockBack;
					#endregion
                    break;

                case (GameTypes.EnemyDifficulty.EXPERT):
                    SlashDelay = 0.25f;
                    TripleAttackSpeed = 500f;
					#region Expert Variables - General
					Name = EnemyEV.SWORD_KNIGHT_EXPERT_NAME;
                    LocStringID = EnemyEV.SWORD_KNIGHT_EXPERT_NAME_LOC_ID;
					
					MaxHealth = EnemyEV.SWORD_KNIGHT_EXPERT_MAX_HEALTH;
					Damage = EnemyEV.SWORD_KNIGHT_EXPERT_DAMAGE;
					XPValue = EnemyEV.SWORD_KNIGHT_EXPERT_XP_VALUE;
					
					MinMoneyDropAmount = EnemyEV.SWORD_KNIGHT_EXPERT_MIN_DROP_AMOUNT;
					MaxMoneyDropAmount = EnemyEV.SWORD_KNIGHT_EXPERT_MAX_DROP_AMOUNT;
					MoneyDropChance = EnemyEV.SWORD_KNIGHT_EXPERT_DROP_CHANCE;
					
					Speed = EnemyEV.SWORD_KNIGHT_EXPERT_SPEED;
					TurnSpeed = EnemyEV.SWORD_KNIGHT_EXPERT_TURN_SPEED;
					ProjectileSpeed = EnemyEV.SWORD_KNIGHT_EXPERT_PROJECTILE_SPEED;
					JumpHeight = EnemyEV.SWORD_KNIGHT_EXPERT_JUMP;
					CooldownTime = EnemyEV.SWORD_KNIGHT_EXPERT_COOLDOWN;
					AnimationDelay = 1 / EnemyEV.SWORD_KNIGHT_EXPERT_ANIMATION_DELAY;
					
					AlwaysFaceTarget = EnemyEV.SWORD_KNIGHT_EXPERT_ALWAYS_FACE_TARGET;
					CanFallOffLedges = EnemyEV.SWORD_KNIGHT_EXPERT_CAN_FALL_OFF_LEDGES;
					CanBeKnockedBack = EnemyEV.SWORD_KNIGHT_EXPERT_CAN_BE_KNOCKED_BACK;
					IsWeighted = EnemyEV.SWORD_KNIGHT_EXPERT_IS_WEIGHTED;
					
					Scale = EnemyEV.SwordKnightExpertScale;
					ProjectileScale = EnemyEV.SwordKnightExpertProjectileScale;
					TintablePart.TextureColor = EnemyEV.SwordKnightExpertTint;
					
					MeleeRadius = EnemyEV.SWORD_KNIGHT_EXPERT_MELEE_RADIUS;
					ProjectileRadius = EnemyEV.SWORD_KNIGHT_EXPERT_PROJECTILE_RADIUS;
					EngageRadius = EnemyEV.SWORD_KNIGHT_EXPERT_ENGAGE_RADIUS;
					
					ProjectileDamage = Damage;
					KnockBack = EnemyEV.SwordKnightExpertKnockBack;
					#endregion
                    break;

                case (GameTypes.EnemyDifficulty.ADVANCED):
                    SlashDelay = 0.25f;
					#region Advanced Variables - General
					Name = EnemyEV.SWORD_KNIGHT_ADVANCED_NAME;
                    LocStringID = EnemyEV.SWORD_KNIGHT_ADVANCED_NAME_LOC_ID;
					
					MaxHealth = EnemyEV.SWORD_KNIGHT_ADVANCED_MAX_HEALTH;
					Damage = EnemyEV.SWORD_KNIGHT_ADVANCED_DAMAGE;
					XPValue = EnemyEV.SWORD_KNIGHT_ADVANCED_XP_VALUE;
					
					MinMoneyDropAmount = EnemyEV.SWORD_KNIGHT_ADVANCED_MIN_DROP_AMOUNT;
					MaxMoneyDropAmount = EnemyEV.SWORD_KNIGHT_ADVANCED_MAX_DROP_AMOUNT;
					MoneyDropChance = EnemyEV.SWORD_KNIGHT_ADVANCED_DROP_CHANCE;
					
					Speed = EnemyEV.SWORD_KNIGHT_ADVANCED_SPEED;
					TurnSpeed = EnemyEV.SWORD_KNIGHT_ADVANCED_TURN_SPEED;
					ProjectileSpeed = EnemyEV.SWORD_KNIGHT_ADVANCED_PROJECTILE_SPEED;
					JumpHeight = EnemyEV.SWORD_KNIGHT_ADVANCED_JUMP;
					CooldownTime = EnemyEV.SWORD_KNIGHT_ADVANCED_COOLDOWN;
					AnimationDelay = 1 / EnemyEV.SWORD_KNIGHT_ADVANCED_ANIMATION_DELAY;
					
					AlwaysFaceTarget = EnemyEV.SWORD_KNIGHT_ADVANCED_ALWAYS_FACE_TARGET;
					CanFallOffLedges = EnemyEV.SWORD_KNIGHT_ADVANCED_CAN_FALL_OFF_LEDGES;
					CanBeKnockedBack = EnemyEV.SWORD_KNIGHT_ADVANCED_CAN_BE_KNOCKED_BACK;
					IsWeighted = EnemyEV.SWORD_KNIGHT_ADVANCED_IS_WEIGHTED;
					
					Scale = EnemyEV.SwordKnightAdvancedScale;
					ProjectileScale = EnemyEV.SwordKnightAdvancedProjectileScale;
					TintablePart.TextureColor = EnemyEV.SwordKnightAdvancedTint;
					
					MeleeRadius = EnemyEV.SWORD_KNIGHT_ADVANCED_MELEE_RADIUS;
					EngageRadius = EnemyEV.SWORD_KNIGHT_ADVANCED_ENGAGE_RADIUS;
					ProjectileRadius = EnemyEV.SWORD_KNIGHT_ADVANCED_PROJECTILE_RADIUS;
					
					ProjectileDamage = Damage;
					KnockBack = EnemyEV.SwordKnightAdvancedKnockBack;
					#endregion
                    break;

                case (GameTypes.EnemyDifficulty.BASIC):
                default:
                    break;
            }		

        }

        protected override void InitializeLogic()
        {
            LogicSet walkTowardsLS = new LogicSet(this);
            walkTowardsLS.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightWalk_Character", true, true));
            walkTowardsLS.AddAction(new MoveLogicAction(m_target, true));
            walkTowardsLS.AddAction(new DelayLogicAction(1));

            LogicSet walkAwayLS = new LogicSet(this);
            walkAwayLS.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightWalk_Character", true, true));
            walkAwayLS.AddAction(new MoveLogicAction(m_target, false));
            walkAwayLS.AddAction(new DelayLogicAction(1));

            LogicSet walkStopLS = new LogicSet(this);
            walkStopLS.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightIdle_Character", true, true));
            walkStopLS.AddAction(new MoveLogicAction(m_target, true, 0));
            walkStopLS.AddAction(new DelayLogicAction(0.5f));

            LogicSet attackLS = new LogicSet(this);
            attackLS.AddAction(new MoveLogicAction(m_target, true, 0));
            attackLS.AddAction(new LockFaceDirectionLogicAction(true));
            attackLS.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightAttack_Character", false, false));
            attackLS.AddAction(new PlayAnimationLogicAction("Start", "Windup", false));
            attackLS.AddAction(new DelayLogicAction(SlashDelay));
            attackLS.AddAction(new Play3DSoundLogicAction(this,  Game.ScreenManager.Player,"SwordKnight_Attack_v02"));
            attackLS.AddAction(new PlayAnimationLogicAction("Attack", "End", false));
            attackLS.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightIdle_Character", false, false));
            attackLS.AddAction(new LockFaceDirectionLogicAction(false));
            attackLS.Tag = GameTypes.LogicSetType_ATTACK;

            LogicSet attackTripleLS = new LogicSet(this);
            attackTripleLS.AddAction(new MoveLogicAction(m_target, true, 0));
            attackTripleLS.AddAction(new LockFaceDirectionLogicAction(true));
            attackTripleLS.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightAttack_Character", false, false));
            attackTripleLS.AddAction(new PlayAnimationLogicAction("Start", "Windup", false));
            attackTripleLS.AddAction(new DelayLogicAction(SlashTripleDelay));
            attackTripleLS.AddAction(new MoveDirectionLogicAction(TripleAttackSpeed));
            attackTripleLS.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 1 / 60f));
            attackTripleLS.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SwordKnight_Attack_v02"));
            attackTripleLS.AddAction(new PlayAnimationLogicAction("Attack", "End", false));


            //attackTripleLS.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightAttack_Character", false, false));
            attackTripleLS.AddAction(new PlayAnimationLogicAction("Start", "Windup", false));
            attackTripleLS.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SwordKnight_Attack_v02"));
            attackTripleLS.AddAction(new PlayAnimationLogicAction("Attack", "End", false));

            //attackTripleLS.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightAttack_Character", false, false));
            //attackTripleLS.AddAction(new PlayAnimationLogicAction("Start", "Windup", false));
            //attackTripleLS.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SwordKnight_Attack_v02"));
            //attackTripleLS.AddAction(new PlayAnimationLogicAction("Attack", "End", false));

            attackTripleLS.AddAction(new MoveLogicAction(null, true, 0));
            attackTripleLS.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 1 / EnemyEV.SWORD_KNIGHT_ADVANCED_ANIMATION_DELAY));
            attackTripleLS.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightIdle_Character", false, false));
            attackTripleLS.AddAction(new LockFaceDirectionLogicAction(false));
            attackTripleLS.Tag = GameTypes.LogicSetType_ATTACK;

            ProjectileData projData = new ProjectileData(this)
            {
                SpriteName = "EnemySpearKnightWave_Sprite",
                SourceAnchor = new Vector2(60, 0),
                Target = null,
                Speed = new Vector2(ProjectileSpeed, ProjectileSpeed),                
                IsWeighted = false,
                RotationSpeed = 0,
                Damage = Damage,
                AngleOffset = 0,
                Angle = new Vector2(0, 0),
                CollidesWithTerrain = true,
                Scale = ProjectileScale,
            };

            LogicSet attackAdvancedLS = new LogicSet(this);
            attackAdvancedLS.AddAction(new MoveLogicAction(m_target, true, 0));
            attackAdvancedLS.AddAction(new LockFaceDirectionLogicAction(true));
            attackAdvancedLS.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightAttack_Character", false, false));
            attackAdvancedLS.AddAction(new PlayAnimationLogicAction("Start", "Windup", false));
            attackAdvancedLS.AddAction(new DelayLogicAction(SlashDelay));
            attackAdvancedLS.AddAction(new Play3DSoundLogicAction(this,  Game.ScreenManager.Player,"SpearKnightAttack1"));
            attackAdvancedLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            attackAdvancedLS.AddAction(new PlayAnimationLogicAction("Attack", "End", false));
            attackAdvancedLS.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightIdle_Character", false, false));
            attackAdvancedLS.AddAction(new LockFaceDirectionLogicAction(false));
            attackAdvancedLS.Tag = GameTypes.LogicSetType_ATTACK;

            LogicSet attackTripleExpertLS = new LogicSet(this);
            attackTripleExpertLS.AddAction(new MoveLogicAction(m_target, true, 0));
            attackTripleExpertLS.AddAction(new LockFaceDirectionLogicAction(true));
            attackTripleExpertLS.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightAttack_Character", false, false));
            attackTripleExpertLS.AddAction(new PlayAnimationLogicAction("Start", "Windup", false));
            attackTripleExpertLS.AddAction(new DelayLogicAction(SlashTripleDelay));
            attackTripleExpertLS.AddAction(new MoveDirectionLogicAction(TripleAttackSpeed));
            attackTripleExpertLS.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 1 / 120f));
            attackTripleExpertLS.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SwordKnight_Attack_v02"));
            attackTripleExpertLS.AddAction(new PlayAnimationLogicAction("Attack", "End", false));


            attackTripleExpertLS.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightAttack_Character", false, false));
            attackTripleExpertLS.AddAction(new PlayAnimationLogicAction("Start", "Windup", false));
            attackTripleExpertLS.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SwordKnight_Attack_v02"));
            attackTripleExpertLS.AddAction(new PlayAnimationLogicAction("Attack", "End", false));

            attackTripleExpertLS.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightAttack_Character", false, false));
            attackTripleExpertLS.AddAction(new PlayAnimationLogicAction("Start", "Windup", false));
            attackTripleExpertLS.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SwordKnight_Attack_v02"));
            attackTripleExpertLS.AddAction(new PlayAnimationLogicAction("Attack", "End", false));

            attackTripleExpertLS.AddAction(new MoveLogicAction(null, true, 0));
            attackTripleExpertLS.AddAction(new ChangePropertyLogicAction(this, "AnimationDelay", 1 / EnemyEV.SWORD_KNIGHT_ADVANCED_ANIMATION_DELAY));
            attackTripleExpertLS.AddAction(new Play3DSoundLogicAction(this, Game.ScreenManager.Player, "SpearKnightAttack1"));
            attackTripleExpertLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            attackTripleExpertLS.AddAction(new PlayAnimationLogicAction("Attack", "End", false));
            attackTripleExpertLS.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightIdle_Character", false, false));
            attackTripleExpertLS.AddAction(new LockFaceDirectionLogicAction(false));
            attackTripleExpertLS.Tag = GameTypes.LogicSetType_ATTACK;

            LogicSet attackExpertLS = new LogicSet(this);
            attackExpertLS.AddAction(new MoveLogicAction(m_target, true, 0));
            attackExpertLS.AddAction(new LockFaceDirectionLogicAction(true));
            attackExpertLS.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightAttack_Character", false, false));
            attackExpertLS.AddAction(new PlayAnimationLogicAction("Start", "Windup", false));
            attackExpertLS.AddAction(new DelayLogicAction(SlashDelay));
            attackExpertLS.AddAction(new Play3DSoundLogicAction(this,  Game.ScreenManager.Player,"SpearKnightAttack1"));
            attackExpertLS.AddAction(new FireProjectileLogicAction(m_levelScreen.ProjectileManager, projData));
            attackExpertLS.AddAction(new PlayAnimationLogicAction("Attack", "End", false));
            //
            attackExpertLS.AddAction(new ChangeSpriteLogicAction("EnemySwordKnightIdle_Character", false, false));
            attackExpertLS.AddAction(new LockFaceDirectionLogicAction(false));
            attackExpertLS.Tag = GameTypes.LogicSetType_ATTACK;

            m_generalBasicLB.AddLogicSet(walkTowardsLS, walkAwayLS, walkStopLS, attackLS);
            m_generalAdvancedLB.AddLogicSet(walkTowardsLS, walkAwayLS, walkStopLS, attackLS, attackTripleLS);
            m_generalExpertLB.AddLogicSet(walkTowardsLS, walkAwayLS, walkStopLS, attackLS, attackTripleExpertLS);
            m_generalCooldownLB.AddLogicSet(walkTowardsLS, walkAwayLS, walkStopLS);

            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalAdvancedLB);
            logicBlocksToDispose.Add(m_generalExpertLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);

            //SetCooldownLogicBlock(m_generalCooldownLB, 0, 0, 100); //walkTowardsLS, walkAwayLS, walkStopLS
            switch (Difficulty)
            {
                case (GameTypes.EnemyDifficulty.BASIC):
                     SetCooldownLogicBlock(m_generalCooldownLB, 14, 11, 75); //walkTowardsLS, walkAwayLS, walkStopLS
                    break;
                case (GameTypes.EnemyDifficulty.ADVANCED):
                case (GameTypes.EnemyDifficulty.EXPERT):
                    SetCooldownLogicBlock(m_generalCooldownLB, 40, 30, 30); //walkTowardsLS, walkAwayLS, walkStopLS
                    break;
                case (GameTypes.EnemyDifficulty.MINIBOSS):
                    SetCooldownLogicBlock(m_generalCooldownLB, 100, 0, 0); //walkTowardsLS, walkAwayLS, walkStopLS
                    break;
                default:
                    break;
            }
           

            base.InitializeLogic();
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case (STATE_MELEE_ENGAGE):
                    RunLogicBlock(true, m_generalBasicLB, 0, 0, 0, 100); //walkTowardsLS, walkAwayLS, walkStopLS, attackLS
                    break;
                case (STATE_PROJECTILE_ENGAGE):
                case (STATE_ENGAGE):
                    RunLogicBlock(true, m_generalBasicLB, 15, 15, 70, 0); //walkTowardsLS, walkAwayLS, walkStopLS, attackLS
                    break;
                case (STATE_WANDER):
                    RunLogicBlock(true, m_generalBasicLB, 0, 0, 100, 0); //walkTowardsLS, walkAwayLS, walkStopLS, attackLS
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
                    RunLogicBlock(true, m_generalAdvancedLB, 0, 0, 0, 65, 35); //walkTowardsLS, walkAwayLS, walkStopLS, attackLS
                    break;
                case (STATE_PROJECTILE_ENGAGE):
                case (STATE_ENGAGE):
                    RunLogicBlock(true, m_generalAdvancedLB, 60, 20, 20, 0, 0); //walkTowardsLS, walkAwayLS, walkStopLS, attackLS
                    break;
                case (STATE_WANDER):
                    RunLogicBlock(true, m_generalAdvancedLB, 0, 0, 100, 0, 0); //walkTowardsLS, walkAwayLS, walkStopLS, attackLS
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
                    RunLogicBlock(true, m_generalExpertLB, 0, 0, 0, 62, 38); //walkTowardsLS, walkAwayLS, walkStopLS, attackLS
                    break;
                case (STATE_PROJECTILE_ENGAGE):
                case (STATE_ENGAGE):
                    RunLogicBlock(true, m_generalBasicLB, 60, 20, 20, 0); //walkTowardsLS, walkAwayLS, walkStopLS, attackLS
                    break;
                case (STATE_WANDER):
                    RunLogicBlock(true, m_generalBasicLB, 0, 0, 100, 0); //walkTowardsLS, walkAwayLS, walkStopLS, attackLS
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
                    //RunLogicBlock(true, m_generalBasicLB, 0, 0, 0, 100); //walkTowardsLS, walkAwayLS, walkStopLS, attackLS
                    RunLogicBlock(true, m_generalBasicLB, 100, 0, 0, 0); //walkTowardsLS, walkAwayLS, walkStopLS, attackLS
                    break;
                default:
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (this.SpriteName == "EnemySwordKnightWalk_Character")
            {
                m_walkSound.Update();
                m_walkSound2.Update();
            }
            base.Update(gameTime);
        }

        public EnemyObj_SwordKnight(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("EnemySwordKnightIdle_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            this.Type = EnemyType.SWORD_KNIGHT;
            m_walkSound = new FrameSoundObj(this, m_target, 1, "KnightWalk1", "KnightWalk2");
            m_walkSound2 = new FrameSoundObj(this, m_target, 6, "KnightWalk1", "KnightWalk2");
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
        {
            SoundManager.Play3DSound(this, Game.ScreenManager.Player,"Knight_Hit01", "Knight_Hit02", "Knight_Hit03");
            base.HitEnemy(damage, position, isPlayer);
        }

        public override void Dispose()
        {
            if (IsDisposed == false)
            {
                m_walkSound.Dispose();
                m_walkSound = null;
                m_walkSound2.Dispose();
                m_walkSound2 = null;
                base.Dispose();
            }
        }
    }
}
