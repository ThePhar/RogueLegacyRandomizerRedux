﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.GameStructs;
using RogueCastle.Screens;

namespace RogueCastle
{
    public class EnemyObj_Eagle: EnemyObj
    {
        private bool m_flyingLeft = false;
        private LogicBlock m_basicAttackLB = new LogicBlock();
        private LogicBlock m_generalCooldownLB = new LogicBlock();


        protected override void InitializeEV()
        {
            MaxHealth = 10;
            Damage = 10;
            XPValue = 5;

            HealthGainPerLevel = 2;
            DamageGainPerLevel = 2;
            XPBonusPerLevel = 1;

            this.IsWeighted = false;
            Speed = 6f;
            EngageRadius = 1900;
            ProjectileRadius = 1600;
            MeleeRadius = 650;
            CooldownTime = 2.0f;
            CanBeKnockedBack = false;
            JumpHeight = 20.5f;
            CanFallOffLedges = true;
            TurnSpeed = 0.0175f; //0.02f;

            switch (Difficulty)
            {
                case (GameTypes.EnemyDifficulty.Miniboss):
                case (GameTypes.EnemyDifficulty.Expert):
                case (GameTypes.EnemyDifficulty.Advanced):
                case (GameTypes.EnemyDifficulty.Basic):
                default:
                    break;
            }
        }
        protected override void InitializeLogic()
        {
            LogicSet basicFlyRight = new LogicSet(this);
            basicFlyRight.AddAction(new MoveDirectionLogicAction(new Vector2(1, 0)));
            basicFlyRight.AddAction(new DelayLogicAction(1));

            LogicSet basicFlyLeft = new LogicSet(this);
            basicFlyLeft.AddAction(new MoveDirectionLogicAction(new Vector2(-1,0)));
            basicFlyLeft.AddAction(new DelayLogicAction(1));

            m_basicAttackLB.AddLogicSet(basicFlyLeft, basicFlyRight);

            logicBlocksToDispose.Add(m_basicAttackLB);
            logicBlocksToDispose.Add(m_generalCooldownLB);

            base.InitializeLogic();
        }

        protected override void RunBasicLogic()
        {
            switch (State)
            {
                case (STATE_MELEE_ENGAGE):
                case (STATE_PROJECTILE_ENGAGE):
                case (STATE_ENGAGE):
                case (STATE_WANDER):
                    if (m_flyingLeft == true)
                        RunLogicBlock(false, m_basicAttackLB, 100, 0);
                    else
                        RunLogicBlock(false, m_basicAttackLB, 0, 100);

                    if (this.X > m_levelScreen.CurrentRoom.Bounds.Right)
                    {
                        this.Y = m_levelScreen.CurrentRoom.Y + CDGMath.RandomInt(100, m_levelScreen.CurrentRoom.Height - 100);
                        m_flyingLeft = true;
                    }
                    else if (this.X < m_levelScreen.CurrentRoom.Bounds.Left)
                    {
                        this.Y = m_levelScreen.CurrentRoom.Y + CDGMath.RandomInt(100, m_levelScreen.CurrentRoom.Height - 100);
                        m_flyingLeft = false;
                    }
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
            if (IsAnimating == false)
                PlayAnimation(true);
            base.Update(gameTime);
        }

        public EnemyObj_Eagle(PlayerObj target, PhysicsManager physicsManager, ProceduralLevelScreen levelToAttachTo, GameTypes.EnemyDifficulty difficulty)
            : base("Dummy_Character", target, physicsManager, levelToAttachTo, difficulty)
        {
            this.Type = EnemyType.EAGLE;
        }
    }
}
