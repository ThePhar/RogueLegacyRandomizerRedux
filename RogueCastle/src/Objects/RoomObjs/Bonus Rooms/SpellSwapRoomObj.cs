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
    public class SpellSwapRoomObj : BonusRoomObj
    {
        public byte Spell { get; set; }
        private SpriteObj m_pedestal;
        private SpriteObj m_icon;
        private float m_iconYPos;

        private SpriteObj m_speechBubble;

        public SpellSwapRoomObj()
        {
        }

        public override void Initialize()
        {
            m_speechBubble = new SpriteObj("UpArrowSquare_Sprite");
            m_speechBubble.Flip = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;

            m_icon = new SpriteObj("Blank_Sprite");

            foreach (GameObj obj in GameObjList)
            {
                if (obj.Name == "pedestal")
                {
                    m_pedestal = obj as SpriteObj;
                    break;
                }
            }

            m_pedestal.OutlineWidth = 2;

            m_icon.X = m_pedestal.X;
            m_icon.Y = m_pedestal.Y - (m_pedestal.Y - m_pedestal.Bounds.Top) - m_icon.Height / 2f - 20;
            m_icon.OutlineWidth = 2;

            m_iconYPos = m_icon.Y;
            GameObjList.Add(m_icon);

            m_speechBubble.Y = m_icon.Y - 30;
            m_speechBubble.X = m_icon.X;
            m_speechBubble.Visible = false;
            GameObjList.Add(m_speechBubble);

            base.Initialize();
        }

        private void RandomizeItem()
        {
            // Selecting random spell.
            if (Game.PlayerStats.Class != ClassType.Dragon && Game.PlayerStats.Class != ClassType.Traitor)
            {
                byte[] spellList = ClassType.GetSpellList(Game.PlayerStats.Class);

                // Code to make sure spell swap doesn't interfere with savantism.
                do
                {
                    Spell = spellList[CDGMath.RandomInt(0, spellList.Length - 1)];
                } while ((Game.PlayerStats.HasTrait(TraitType.SAVANT)) &&
                (Spell == SpellType.TRANSLOCATOR || Spell == SpellType.TIME_STOP || Spell == SpellType.DAMAGE_SHIELD));

                Array.Clear(spellList, 0, spellList.Length);
                ID = (int) Spell;
            }
            else
            {
                if (Game.PlayerStats.Class == ClassType.Dragon)
                {
                    ID = (int) SpellType.DRAGON_FIRE;
                    Spell = SpellType.DRAGON_FIRE;
                }
                else if (Game.PlayerStats.Class == ClassType.Traitor)
                {
                    ID = (int) SpellType.RAPID_DAGGER;
                    Spell = SpellType.RAPID_DAGGER;
                }
            }
                
            //Spell = ClassType.GetSpellList(Game.PlayerStats.Class)[0];

            m_icon.ChangeSprite(SpellEV.Icon(Spell));
            //RoomCompleted = true;
        }

        public override void OnEnter()
        {
            if (ID == -1 && RoomCompleted == false) // Make sure not to randomize the item if it's already been randomized once.
            {
                do
                {
                    RandomizeItem();
                } while ((Spell == Game.PlayerStats.Spell && Game.PlayerStats.Class != ClassType.Dragon && Game.PlayerStats.Class != ClassType.Traitor) // Make sure the room doesn't randomize to the item the player already has.
                    || (Spell == SpellType.DRAGON_FIRE && Game.PlayerStats.Class != ClassType.Dragon) // Make sure Dragon fire is not set as the spell for a locked castle and changed class.
                    || (Spell == SpellType.RAPID_DAGGER && Game.PlayerStats.Class != ClassType.Traitor)); // Make sure rapid dagger is not set as the spell for a locked castle and changed class.
            }
            else if (ID != -1)
            {
                Spell = (byte)ID;
                m_icon.ChangeSprite(SpellEV.Icon(Spell));

                if (RoomCompleted == true)
                {
                    m_icon.Visible = false;
                    m_speechBubble.Visible = false;
                }
            }

            base.OnEnter();
        }

        public override void Update(GameTime gameTime)
        {
            m_icon.Y = m_iconYPos - 10 + (float)Math.Sin(Game.TotalGameTime * 2) * 5;
            m_speechBubble.Y = m_iconYPos - 90 + (float)Math.Sin(Game.TotalGameTime * 20) * 2;

            if (RoomCompleted == false)
            {
                m_icon.Visible = true;

                if (CollisionMath.Intersects(Player.Bounds, m_pedestal.Bounds))
                {
                    m_speechBubble.Visible = true;
                    if (Game.GlobalInput.JustPressed(InputMapType.PLAYER_UP1) || Game.GlobalInput.JustPressed(InputMapType.PLAYER_UP2))
                    {
                        TakeItem();
                    }
                }
                else
                    m_speechBubble.Visible = false;
            }
            else
                m_icon.Visible = false;

            base.Update(gameTime);
        }

        public void TakeItem()
        {
            RoomCompleted = true;
            Game.PlayerStats.Spell = Spell;

            if (Game.PlayerStats.Class == ClassType.Wizard2)
                Game.PlayerStats.WizardSpellList = SpellEV.GetNext3Spells();

            Spell = 0;
            m_speechBubble.Visible = false;
            m_icon.Visible = false;
            (Game.ScreenManager.CurrentScreen as ProceduralLevelScreen).UpdatePlayerSpellIcon();
            List<object> objectList = new List<object>();
            objectList.Add(new Vector2(m_icon.X, m_icon.Y - m_icon.Height / 2f));
            objectList.Add(GetItemType.SPELL);
            objectList.Add(new Vector2((byte)Game.PlayerStats.Spell, 0));

            (Player.AttachedLevel.ScreenManager as RCScreenManager).DisplayScreen(ScreenType.GET_ITEM, true, objectList);
            Tweener.Tween.RunFunction(0, Player, "RunGetItemAnimation"); // Necessary to delay this call by one update.
        }

        public override void Dispose()
        {
            if (IsDisposed == false)
            {
                // Done
                m_pedestal = null;
                m_icon = null;
                m_speechBubble = null;
                base.Dispose();
            }
        }

    }
}
