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
    public class EnemyObj_BallAndChain : EnemyObj
    {
        private LogicBlock m_generalBasicLB = new LogicBlock();
        private LogicBlock m_generalCooldownLB = new LogicBlock();

        private ProjectileObj m_ballAndChain, m_ballAndChain2;
        private SpriteObj m_chain;
        public float ChainSpeed { get; set; }
        public float ChainSpeed2Modifier { get; set; }

        private int m_numChainLinks = 10;//15;
        private List<Vector2> m_chainLinksList;
        private List<Vector2> m_chainLinks2List;
        private float m_chainRadius;
        private float m_actualChainRadius; // The radius of the actual chain as it grows to reach m_chainRadius.
        private float m_ballAngle = 0;
        private float m_chainLinkDistance;
        private float m_BallSpeedDivider = 1.5f; //The amount the ball speeds get slowed down by (divided).
        private FrameSoundObj m_walkSound, m_walkSound2;

        protected override void InitializeEV()
        {
            ChainSpeed = 2.5f;
            ChainRadius = 260;
            ChainSpeed2Modifier = 1.5f;

            #region Basic Variables - General
            Name = EnemyEV.BALL_AND_CHAIN_BASIC_NAME;
            LocStringID = EnemyEV.BALL_AND_CHAIN_BASIC_NAME_LOC_ID;

            MaxHealth = EnemyEV.BALL_AND_CHAIN_BASIC_MAX_HEALTH;
            Damage = EnemyEV.BALL_AND_CHAIN_BASIC_DAMAGE;
            XPValue = EnemyEV.BALL_AND_CHAIN_BASIC_XP_VALUE;

            MinMoneyDropAmount = EnemyEV.BALL_AND_CHAIN_BASIC_MIN_DROP_AMOUNT;
            MaxMoneyDropAmount = EnemyEV.BALL_AND_CHAIN_BASIC_MAX_DROP_AMOUNT;
            MoneyDropChance = EnemyEV.BALL_AND_CHAIN_BASIC_DROP_CHANCE;

            Speed = EnemyEV.BALL_AND_CHAIN_BASIC_SPEED;
            TurnSpeed = EnemyEV.BALL_AND_CHAIN_BASIC_TURN_SPEED;
            ProjectileSpeed = EnemyEV.BALL_AND_CHAIN_BASIC_PROJECTILE_SPEED;
            JumpHeight = EnemyEV.BALL_AND_CHAIN_BASIC_JUMP;
            CooldownTime = EnemyEV.BALL_AND_CHAIN_BASIC_COOLDOWN;
            AnimationDelay = 1 / EnemyEV.BALL_AND_CHAIN_BASIC_ANIMATION_DELAY;

            AlwaysFaceTarget = EnemyEV.BALL_AND_CHAIN_BASIC_ALWAYS_FACE_TARGET;
            CanFallOffLedges = EnemyEV.BALL_AND_CHAIN_BASIC_CAN_FALL_OFF_LEDGES;
            CanBeKnockedBack = EnemyEV.BALL_AND_CHAIN_BASIC_CAN_BE_KNOCKED_BACK;
            IsWeighted = EnemyEV.BALL_AND_CHAIN_BASIC_IS_WEIGHTED;

            Scale = EnemyEV.BallAndChainBasicScale;
            ProjectileScale = EnemyEV.BallAndChainBasicProjectileScale;
            TintablePart.TextureColor = EnemyEV.BallAndChainBasicTint;

            MeleeRadius = EnemyEV.BALL_AND_CHAIN_BASIC_MELEE_RADIUS;
            ProjectileRadius = EnemyEV.BALL_AND_CHAIN_BASIC_PROJECTILE_RADIUS;
            EngageRadius = EnemyEV.BALL_AND_CHAIN_BASIC_ENGAGE_RADIUS;

            ProjectileDamage = Damage;
            KnockBack = EnemyEV.BallAndChainBasicKnockBack;
            #endregion

            switch (Difficulty)
            {
                case (GameTypes.EnemyDifficulty.Miniboss):
                    #region Miniboss Variables - General
                    Name = EnemyEV.BALL_AND_CHAIN_MINIBOSS_NAME;
                    LocStringID = EnemyEV.BALL_AND_CHAIN_MINIBOSS_NAME_LOC_ID;

                    MaxHealth = EnemyEV.BALL_AND_CHAIN_MINIBOSS_MAX_HEALTH;
                    Damage = EnemyEV.BALL_AND_CHAIN_MINIBOSS_DAMAGE;
                    XPValue = EnemyEV.BALL_AND_CHAIN_MINIBOSS_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.BALL_AND_CHAIN_MINIBOSS_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.BALL_AND_CHAIN_MINIBOSS_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.BALL_AND_CHAIN_MINIBOSS_DROP_CHANCE;

                    Speed = EnemyEV.BALL_AND_CHAIN_MINIBOSS_SPEED;
                    TurnSpeed = EnemyEV.BALL_AND_CHAIN_MINIBOSS_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.BALL_AND_CHAIN_MINIBOSS_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.BALL_AND_CHAIN_MINIBOSS_JUMP;
                    CooldownTime = EnemyEV.BALL_AND_CHAIN_MINIBOSS_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.BALL_AND_CHAIN_MINIBOSS_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.BALL_AND_CHAIN_MINIBOSS_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.BALL_AND_CHAIN_MINIBOSS_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.BALL_AND_CHAIN_MINIBOSS_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.BALL_AND_CHAIN_MINIBOSS_IS_WEIGHTED;

                    Scale = EnemyEV.BallAndChainMinibossScale;
                    ProjectileScale = EnemyEV.BallAndChainMinibossProjectileScale;
                    TintablePart.TextureColor = EnemyEV.BallAndChainMinibossTint;

                    MeleeRadius = EnemyEV.BALL_AND_CHAIN_MINIBOSS_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.BALL_AND_CHAIN_MINIBOSS_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.BALL_AND_CHAIN_MINIBOSS_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.BallAndChainMinibossKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Expert):
                    ChainRadius = 350;
                    ChainSpeed2Modifier = 1.5f;

                    #region Expert Variables - General
                    Name = EnemyEV.BALL_AND_CHAIN_EXPERT_NAME;
                    LocStringID = EnemyEV.BALL_AND_CHAIN_EXPERT_NAME_LOC_ID;

                    MaxHealth = EnemyEV.BALL_AND_CHAIN_EXPERT_MAX_HEALTH;
                    Damage = EnemyEV.BALL_AND_CHAIN_EXPERT_DAMAGE;
                    XPValue = EnemyEV.BALL_AND_CHAIN_EXPERT_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.BALL_AND_CHAIN_EXPERT_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.BALL_AND_CHAIN_EXPERT_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.BALL_AND_CHAIN_EXPERT_DROP_CHANCE;

                    Speed = EnemyEV.BALL_AND_CHAIN_EXPERT_SPEED;
                    TurnSpeed = EnemyEV.BALL_AND_CHAIN_EXPERT_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.BALL_AND_CHAIN_EXPERT_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.BALL_AND_CHAIN_EXPERT_JUMP;
                    CooldownTime = EnemyEV.BALL_AND_CHAIN_EXPERT_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.BALL_AND_CHAIN_EXPERT_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.BALL_AND_CHAIN_EXPERT_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.BALL_AND_CHAIN_EXPERT_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.BALL_AND_CHAIN_EXPERT_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.BALL_AND_CHAIN_EXPERT_IS_WEIGHTED;

                    Scale = EnemyEV.BallAndChainExpertScale;
                    ProjectileScale = EnemyEV.BallAndChainExpertProjectileScale;
                    TintablePart.TextureColor = EnemyEV.BallAndChainExpertTint;

                    MeleeRadius = EnemyEV.BALL_AND_CHAIN_EXPERT_MELEE_RADIUS;
                    ProjectileRadius = EnemyEV.BALL_AND_CHAIN_EXPERT_PROJECTILE_RADIUS;
                    EngageRadius = EnemyEV.BALL_AND_CHAIN_EXPERT_ENGAGE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.BallAndChainExpertKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Advanced):
                    ChainRadius = 275;
                    #region Advanced Variables - General
                    Name = EnemyEV.BALL_AND_CHAIN_ADVANCED_NAME;
                    LocStringID = EnemyEV.BALL_AND_CHAIN_ADVANCED_NAME_LOC_ID;

                    MaxHealth = EnemyEV.BALL_AND_CHAIN_ADVANCED_MAX_HEALTH;
                    Damage = EnemyEV.BALL_AND_CHAIN_ADVANCED_DAMAGE;
                    XPValue = EnemyEV.BALL_AND_CHAIN_ADVANCED_XP_VALUE;

                    MinMoneyDropAmount = EnemyEV.BALL_AND_CHAIN_ADVANCED_MIN_DROP_AMOUNT;
                    MaxMoneyDropAmount = EnemyEV.BALL_AND_CHAIN_ADVANCED_MAX_DROP_AMOUNT;
                    MoneyDropChance = EnemyEV.BALL_AND_CHAIN_ADVANCED_DROP_CHANCE;

                    Speed = EnemyEV.BALL_AND_CHAIN_ADVANCED_SPEED;
                    TurnSpeed = EnemyEV.BALL_AND_CHAIN_ADVANCED_TURN_SPEED;
                    ProjectileSpeed = EnemyEV.BALL_AND_CHAIN_ADVANCED_PROJECTILE_SPEED;
                    JumpHeight = EnemyEV.BALL_AND_CHAIN_ADVANCED_JUMP;
                    CooldownTime = EnemyEV.BALL_AND_CHAIN_ADVANCED_COOLDOWN;
                    AnimationDelay = 1 / EnemyEV.BALL_AND_CHAIN_ADVANCED_ANIMATION_DELAY;

                    AlwaysFaceTarget = EnemyEV.BALL_AND_CHAIN_ADVANCED_ALWAYS_FACE_TARGET;
                    CanFallOffLedges = EnemyEV.BALL_AND_CHAIN_ADVANCED_CAN_FALL_OFF_LEDGES;
                    CanBeKnockedBack = EnemyEV.BALL_AND_CHAIN_ADVANCED_CAN_BE_KNOCKED_BACK;
                    IsWeighted = EnemyEV.BALL_AND_CHAIN_ADVANCED_IS_WEIGHTED;

                    Scale = EnemyEV.BallAndChainAdvancedScale;
                    ProjectileScale = EnemyEV.BallAndChainAdvancedProjectileScale;
                    TintablePart.TextureColor = EnemyEV.BallAndChainAdvancedTint;

                    MeleeRadius = EnemyEV.BALL_AND_CHAIN_ADVANCED_MELEE_RADIUS;
                    EngageRadius = EnemyEV.BALL_AND_CHAIN_ADVANCED_ENGAGE_RADIUS;
                    ProjectileRadius = EnemyEV.BALL_AND_CHAIN_ADVANCED_PROJECTILE_RADIUS;

                    ProjectileDamage = Damage;
                    KnockBack = EnemyEV.BallAndChainAdvancedKnockBack;
                    #endregion
                    break;

                case (GameTypes.EnemyDifficulty.Basic):
                default:
                    break;
            }

            // Ball and chain has 2 tintable colour parts.
            _objectList[1].TextureColor = TintablePart.TextureColor;

            m_ballAndChain.Damage = Damage;
            m_ballAndChain.Scale = ProjectileScale;

            m_ballAndChain2.Damage = Damage;
            m_ballAndChain2.Scale = ProjectileScale;
        }

        protected override void InitializeLogic()
        {
            LogicSet walkTowardsLS = new LogicSet(this);
            //walkTowardsLS.AddAction(new PlayAnimationLogicAction(true));
            walkTowardsLS.AddAction(new MoveLogicAction(m_target, true));
            walkTowardsLS.AddAction(new DelayLogicAction(1.25f, 2.75f));

            LogicSet walkAwayLS = new LogicSet(this);
            //walkAwayLS.AddAction(new PlayAnimationLogicAction(true));
            walkAwayLS.AddAction(new MoveLogicAction(m_target, false));
            walkAwayLS.AddAction(new DelayLogicAction(1.25f, 2.75f));

            LogicSet walkStopLS = new LogicSet(this);
            walkStopLS.AddAction(new StopAnimationLogicAction());
            walkStopLS.AddAction(new MoveLogicAction(m_target, true, 0));
            walkStopLS.AddAction(new DelayLogicAction(1.0f, 1.5f));

            m_generalBasicLB.AddLogicSet(walkTowardsLS, walkAwayLS, walkStopLS);
            m_generalCooldownLB.AddLogicSet(walkTowardsLS, walkAwayLS, walkStopLS);

            logicBlocksToDispose.Add(m_generalBasicLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);

            SetCooldownLogicBlock(m_generalCooldownLB, 40, 40, 20); //walkTowardsLS, walkAwayLS, walkStopLS

            base.InitializeLogic();
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case (STATE_MELEE_ENGAGE):
                case (STATE_PROJECTILE_ENGAGE):
                case (STATE_ENGAGE):
                    RunLogicBlock(true, m_generalBasicLB, 60, 20, 20); //walkTowardsLS, walkAwayLS, walkStopLS
                    break;
                case (STATE_WANDER):
                    RunLogicBlock(true, m_generalBasicLB, 0, 0, 100); //walkTowardsLS, walkAwayLS, walkStopLS
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
                    RunLogicBlock(true, m_generalBasicLB, 60, 20, 20); //walkTowardsLS, walkAwayLS, walkStopLS
                    break;
                case (STATE_WANDER):
                    RunLogicBlock(true, m_generalBasicLB, 0, 0, 100); //walkTowardsLS, walkAwayLS, walkStopLS
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
                    RunLogicBlock(true, m_generalBasicLB, 60, 20, 20); //walkTowardsLS, walkAwayLS, walkStopLS
                    break;
                case (STATE_WANDER):
                    RunLogicBlock(true, m_generalBasicLB, 0, 0, 100); //walkTowardsLS, walkAwayLS, walkStopLS
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

        public EnemyObj_BallAndChain(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("EnemyFlailKnight_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            m_ballAndChain = new ProjectileObj("EnemyFlailKnightBall_Sprite"); // At this point, physicsManager is null.
            m_ballAndChain.IsWeighted = false;
            m_ballAndChain.CollidesWithTerrain = false;
            m_ballAndChain.IgnoreBoundsCheck = true;
            m_ballAndChain.OutlineWidth = 2;

            m_ballAndChain2 = m_ballAndChain.Clone() as ProjectileObj;

            m_chain = new SpriteObj("EnemyFlailKnightLink_Sprite");

            m_chainLinksList = new List<Vector2>();
            m_chainLinks2List = new List<Vector2>();
            for (int i = 0; i < m_numChainLinks; i++)
            {
                m_chainLinksList.Add(new Vector2());
            }

            for (int i = 0; i < (int)(m_numChainLinks / 2); i++)
            {
                m_chainLinks2List.Add(new Vector2());
            }
            this.Type = EnemyType.BALL_AND_CHAIN;

            this.TintablePart = _objectList[3];

            m_walkSound = new FrameSoundObj(this, m_target, 1, "KnightWalk1", "KnightWalk2");
            m_walkSound2 = new FrameSoundObj(this, m_target, 6, "KnightWalk1", "KnightWalk2");
        }

        public override void Update(GameTime gameTime)
        {
            if (IsPaused == false)
            {
                if (IsKilled == false && m_initialDelayCounter <= 0)
                {
                    float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

                    if (m_actualChainRadius < ChainRadius)
                    {
                        m_actualChainRadius += elapsedSeconds * 200;
                        m_chainLinkDistance = m_actualChainRadius / m_numChainLinks;
                    }

                    float distance = 0;
                    m_ballAndChain.Position = CDGMath.GetCirclePosition(m_ballAngle, m_actualChainRadius, new Vector2(this.X, this.Bounds.Top));
                    for (int i = 0; i < m_chainLinksList.Count; i++)
                    {
                        m_chainLinksList[i] = CDGMath.GetCirclePosition(m_ballAngle, distance, new Vector2(this.X, this.Bounds.Top));
                        distance += m_chainLinkDistance;
                    }

                    distance = 0;
                    if (Difficulty == GameTypes.EnemyDifficulty.Advanced)
                        m_ballAndChain2.Position = CDGMath.GetCirclePosition(m_ballAngle * ChainSpeed2Modifier, m_actualChainRadius / 2, new Vector2(this.X, this.Bounds.Top));
                    else if (Difficulty == GameTypes.EnemyDifficulty.Expert)
                        m_ballAndChain2.Position = CDGMath.GetCirclePosition(-m_ballAngle * ChainSpeed2Modifier, -m_actualChainRadius / 2, new Vector2(this.X, this.Bounds.Top));
                    for (int i = 0; i < m_chainLinks2List.Count; i++)
                    {
                        if (Difficulty == GameTypes.EnemyDifficulty.Advanced)
                            m_chainLinks2List[i] = CDGMath.GetCirclePosition(m_ballAngle * ChainSpeed2Modifier, distance, new Vector2(this.X, this.Bounds.Top));
                        else if (Difficulty == GameTypes.EnemyDifficulty.Expert)
                            m_chainLinks2List[i] = CDGMath.GetCirclePosition(-m_ballAngle * ChainSpeed2Modifier, -distance, new Vector2(this.X, this.Bounds.Top));
                        distance += m_chainLinkDistance;
                    }

                    m_ballAngle += (ChainSpeed * 60 * elapsedSeconds);

                    if (this.IsAnimating == false && this.CurrentSpeed != 0)
                        this.PlayAnimation(true);
                }

                if (this.SpriteName == "EnemyFlailKnight_Character")
                {
                    m_walkSound.Update();
                    m_walkSound2.Update();
                }
            }
            base.Update(gameTime);
        }

        public override void Draw(Camera2D camera)
        {
            if (IsKilled == false)
            {
                foreach (Vector2 chain in m_chainLinksList)
                {
                    m_chain.Position = chain;
                    m_chain.Draw(camera);
                }
                m_ballAndChain.Draw(camera);

                if (Difficulty > GameTypes.EnemyDifficulty.Basic)
                {
                    foreach (Vector2 chain in m_chainLinks2List)
                    {
                        m_chain.Position = chain;
                        m_chain.Draw(camera);
                    }
                    m_ballAndChain2.Draw(camera);
                }

            }
            base.Draw(camera);
        }

        public override void Kill(bool giveXP = true)
        {
            m_levelScreen.PhysicsManager.RemoveObject(m_ballAndChain);
            EnemyObj_BouncySpike spike = new EnemyObj_BouncySpike(m_target, null, m_levelScreen, this.Difficulty);
            spike.SavedStartingPos = this.Position;
            spike.Position = this.Position; // Set the spike's position to this, so that when the room respawns, the ball appears where the enemy was.
            m_levelScreen.AddEnemyToCurrentRoom(spike);
            spike.Position = m_ballAndChain.Position;
            spike.Speed = ChainSpeed * 200 / m_BallSpeedDivider;

            // Must be called afterward since AddEnemyToCurrentRoom overrides heading.
            spike.HeadingX = (float)Math.Cos(MathHelper.WrapAngle(MathHelper.ToRadians(m_ballAngle + 90)));
            spike.HeadingY = (float)Math.Sin(MathHelper.WrapAngle(MathHelper.ToRadians(m_ballAngle + 90)));

            if (Difficulty > GameTypes.EnemyDifficulty.Basic)
            {
                m_levelScreen.PhysicsManager.RemoveObject(m_ballAndChain2);

                EnemyObj_BouncySpike spike2 = new EnemyObj_BouncySpike(m_target, null, m_levelScreen, this.Difficulty);
                spike2.SavedStartingPos = this.Position;
                spike2.Position = this.Position;
                m_levelScreen.AddEnemyToCurrentRoom(spike2);
                spike2.Position = m_ballAndChain2.Position;
                spike2.Speed = ChainSpeed * 200 * ChainSpeed2Modifier / m_BallSpeedDivider;

                if (Difficulty == GameTypes.EnemyDifficulty.Advanced)
                {
                    spike2.HeadingX = (float)Math.Cos(MathHelper.WrapAngle(MathHelper.ToRadians(m_ballAngle * ChainSpeed2Modifier + 90)));
                    spike2.HeadingY = (float)Math.Sin(MathHelper.WrapAngle(MathHelper.ToRadians(m_ballAngle * ChainSpeed2Modifier + 90)));
                }
                else if (Difficulty == GameTypes.EnemyDifficulty.Expert)
                {
                    spike2.HeadingX = (float)Math.Cos(MathHelper.WrapAngle(MathHelper.ToRadians(-m_ballAngle * ChainSpeed2Modifier + 90)));
                    spike2.HeadingY = (float)Math.Sin(MathHelper.WrapAngle(MathHelper.ToRadians(-m_ballAngle * ChainSpeed2Modifier + 90)));
                }

                spike2.SpawnRoom = m_levelScreen.CurrentRoom;
                spike2.SaveToFile = false;

                if (this.IsPaused)
                    spike2.PauseEnemy();
            }

            spike.SpawnRoom = m_levelScreen.CurrentRoom;
            spike.SaveToFile = false;

            if (this.IsPaused)
                spike.PauseEnemy();

            base.Kill(giveXP);
        }

        public override void ResetState()
        {
            base.ResetState();

            m_actualChainRadius = 0;
            m_chainLinkDistance = m_actualChainRadius / m_numChainLinks;

            float distance = 0;
            m_ballAndChain.Position = CDGMath.GetCirclePosition(m_ballAngle, m_actualChainRadius, new Vector2(this.X, this.Bounds.Top));
            for (int i = 0; i < m_chainLinksList.Count; i++)
            {
                m_chainLinksList[i] = CDGMath.GetCirclePosition(m_ballAngle, distance, new Vector2(this.X, this.Bounds.Top));
                distance += m_chainLinkDistance;
            }

            distance = 0;
            if (Difficulty == GameTypes.EnemyDifficulty.Advanced)
                m_ballAndChain2.Position = CDGMath.GetCirclePosition(m_ballAngle * ChainSpeed2Modifier, m_actualChainRadius / 2, new Vector2(this.X, this.Bounds.Top));
            else if (Difficulty == GameTypes.EnemyDifficulty.Expert)
                m_ballAndChain2.Position = CDGMath.GetCirclePosition(-m_ballAngle * ChainSpeed2Modifier, -m_actualChainRadius / 2, new Vector2(this.X, this.Bounds.Top));
            for (int i = 0; i < m_chainLinks2List.Count; i++)
            {
                if (Difficulty == GameTypes.EnemyDifficulty.Advanced)
                    m_chainLinks2List[i] = CDGMath.GetCirclePosition(m_ballAngle * ChainSpeed2Modifier, distance, new Vector2(this.X, this.Bounds.Top));
                else if (Difficulty == GameTypes.EnemyDifficulty.Expert)
                    m_chainLinks2List[i] = CDGMath.GetCirclePosition(-m_ballAngle * ChainSpeed2Modifier, -distance, new Vector2(this.X, this.Bounds.Top));
                distance += m_chainLinkDistance;
            }
        }

        public override void HitEnemy(int damage, Vector2 position, bool isPlayer)
        {
            SoundManager.Play3DSound(this, m_target, "Knight_Hit01", "Knight_Hit02", "Knight_Hit03");
            base.HitEnemy(damage, position, isPlayer);
        }

        public override void Dispose()
        {
            if (IsDisposed == false)
            {
                // Done
                m_chain.Dispose();
                m_chain = null;
                m_ballAndChain.Dispose();
                m_ballAndChain = null;
                m_ballAndChain2.Dispose();
                m_ballAndChain2 = null;
                m_chainLinksList.Clear();
                m_chainLinksList = null;
                m_chainLinks2List.Clear();
                m_chainLinks2List = null;
                m_walkSound.Dispose();
                m_walkSound = null;
                m_walkSound2.Dispose();
                m_walkSound2 = null;
                base.Dispose();
            }
        }

        private float ChainRadius
        {
            get { return m_chainRadius; }
            set
            {
                m_chainRadius = value;
                //m_chainLinkDistance = m_chainRadius / m_numChainLinks;
            }
        }

        public ProjectileObj BallAndChain
        {
            get { return m_ballAndChain; }
        }

        public ProjectileObj BallAndChain2
        {
            get { return m_ballAndChain2; }
        }
    }
}
