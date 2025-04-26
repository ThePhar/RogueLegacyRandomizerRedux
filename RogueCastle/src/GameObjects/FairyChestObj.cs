using System;
using System.Collections.Generic;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.GameStructs;
using RogueCastle.Screens;
using RogueCastle.Screens.BaseScreens;
using Tweener;
using Tweener.Ease;

namespace RogueCastle.Objects;

public class FairyChestObj(PhysicsManager physicsManager) : ChestObj(physicsManager)
{
    private const float SparkleDelay = 1;

    private int _conditionType;
    private SpriteObj _errorSprite = new("CancelIcon_Sprite") { Visible = false };
    private SpriteObj _lockSprite = new("Chest4Unlock_Sprite");
    private PlayerObj _player = Game.ScreenManager.Player;
    private float _sparkleCounter = SparkleDelay;

    private TextObj _timerText = new(Game.JunicodeFont)
    {
        FontSize = 18,
        DropShadow = new Vector2(2, 2),
        Align = Types.TextAlign.Centre,
    };

    public ChestState State { get; internal set; }

    public float Timer { get; set; }

    public int ConditionType => _conditionType;

    public void SetConditionType(int conditionType = 0)
    {
        if (conditionType != 0)
        {
            _conditionType = conditionType;
        }
        else
        {
            int.TryParse(Tag, out _conditionType);
        }

        if (_conditionType == ChestConditionType.REACH_IN5_SECONDS)
        {
            Timer = 5;
        }
    }

    public void SetChestUnlocked()
    {
        if (ConditionType != ChestConditionType.INVISIBLE_CHEST && ConditionType != ChestConditionType.NONE)
        {
            _player.AttachedLevel.ObjectiveComplete();
        }

        State = ChestState.Free;
        _lockSprite.PlayAnimation(false);
        Tween.By(_lockSprite, 0.2f, Linear.EaseNone, "Y", "40");
        Tween.To(_lockSprite, 0.2f, Linear.EaseNone, "delay", "0.1", "Opacity", "0");
    }

    public void SetChestFailed(bool skipTween = false)
    {
        if (skipTween == false)
        {
            _player.AttachedLevel.ObjectiveFailed();
        }

        State = ChestState.Failed;
        _errorSprite.Visible = true;
        _errorSprite.Opacity = 0f;
        _errorSprite.Scale = Vector2.One;
        _errorSprite.Position = new Vector2(X, Y - (Height / 2));

        if (skipTween == false)
        {
            SoundManager.Play3DSound(this, Game.ScreenManager.Player, "FairyChest_Fail");
            Tween.To(_errorSprite, 0.5f, Quad.EaseIn, "ScaleX", "0.5", "ScaleY", "0.5", "Opacity", "1");
        }
        else
        {
            _errorSprite.Scale = new Vector2(0.5f, 0.5f);
            _errorSprite.Opacity = 1;
        }
    }

    public override void OpenChest(ItemDropManager itemDropManager, PlayerObj player)
    {
        if (State != ChestState.Free)
        {
            return;
        }

        if (IsOpen || IsLocked)
        {
            return;
        }

        GoToFrame(2);
        SoundManager.Play3DSound(this, Game.ScreenManager.Player, "Chest_Open_Large");

        // Give gold if all runes have been found
        if (Game.PlayerStats.TotalRunesFound >= EquipmentCategoryType.TOTAL * EquipmentAbilityType.TOTAL)
        {
            GiveStatDrop(itemDropManager, _player, 1, 0);
        }
        else
        {
            List<byte[]> runeArray = Game.PlayerStats.GetRuneArray;
            var possibleRunes = new List<Vector2>();

            var categoryCounter = 0;
            foreach (var itemArray in runeArray)
            {
                var itemCounter = 0;
                foreach (var itemState in itemArray)
                {
                    if (itemState == EquipmentState.NOT_FOUND)
                    {
                        possibleRunes.Add(new Vector2(categoryCounter, itemCounter));
                    }

                    itemCounter++;
                }

                categoryCounter++;
            }

            if (possibleRunes.Count > 0)
            {
                var chosenRune = possibleRunes[CDGMath.RandomInt(0, possibleRunes.Count - 1)];
                Game.PlayerStats.GetRuneArray[(int)chosenRune.X][(int)chosenRune.Y] = EquipmentState.FOUND_BUT_NOT_SEEN;
                var objectList = new List<object>
                {
                    new Vector2(X, Y - (Height / 2f)),
                    GetItemType.RUNE,
                    new Vector2(chosenRune.X, chosenRune.Y),
                };

                (player.AttachedLevel.ScreenManager as RCScreenManager)!.DisplayScreen(ScreenType.GET_ITEM, true,
                    objectList);
                player.RunGetItemAnimation();

                Console.WriteLine($@"Unlocked item index {chosenRune.X} of type {chosenRune.Y}");
            }
            else
            {
                GiveGold(itemDropManager);
            }
        }

        player.AttachedLevel.RefreshMapChestIcons();
        //base.OpenChest(itemDropManager, player); // Regular chest opening logic.
    }

    public override void Draw(Camera2D camera)
    {
        if (State == ChestState.Locked)
        {
            ChestConditionChecker.SetConditionState(this, _player);
        }

        if (IsOpen == false)
        {
            // Adds the chest sparkle effect.
            // Only sparkle the chest if the game is playing. Otherwise, it's probably paused.
            if (Game.ScreenManager.CurrentScreen is ProceduralLevelScreen)
            {
                if (_sparkleCounter > 0)
                {
                    _sparkleCounter -= (float)camera.GameTime.ElapsedGameTime.TotalSeconds;
                    if (_sparkleCounter <= 0)
                    {
                        _sparkleCounter = SparkleDelay;
                        float delay = 0;
                        for (var i = 0; i < 2; i++)
                        {
                            Tween.To(this, delay, Linear.EaseNone);
                            Tween.AddEndHandlerToLastTween(
                                _player.AttachedLevel.ImpactEffectPool,
                                "DisplayChestSparkleEffect",
                                new Vector2(X, Y - (Height / 2)));
                            delay += 1 / 2f;
                        }
                    }
                }
            }

            if (ConditionType == ChestConditionType.REACH_IN5_SECONDS && State == ChestState.Locked)
            {
                if (_player.AttachedLevel.IsPaused == false)
                {
                    Timer -= (float)camera.GameTime.ElapsedGameTime.TotalSeconds;
                }

                _timerText.Position = new Vector2(Position.X, Y - 50);
                _timerText.Text = ((int)Timer + 1).ToString();
                _timerText.Draw(camera);

                // TODO: does this need to be refreshed on language change?
                _player.AttachedLevel.UpdateObjectiveProgress(
                    DialogueManager.GetText($"Chest_Locked {ConditionType}").Dialogue[0].GetResourceString() +
                    (int)(Timer + 1)
                );
            }
        }

        if (ConditionType == ChestConditionType.INVISIBLE_CHEST && !IsOpen)
        {
            return;
        }

        base.Draw(camera);

        _lockSprite.Flip = Flip;
        _lockSprite.Position = Flip == SpriteEffects.None
            ? new Vector2(X - 10, Y - (Height / 2))
            : new Vector2(X + 10, Y - (Height / 2));

        _lockSprite.Draw(camera);

        _errorSprite.Position = new Vector2(X, Y - (Height / 2));
        _errorSprite.Draw(camera);
    }

    public override void CollisionResponse(CollisionBox thisBox, CollisionBox otherBox, int collisionResponseType)
    {
        if (State == ChestState.Free)
        {
            base.CollisionResponse(thisBox, otherBox, collisionResponseType);
        }
    }

    public override void ForceOpen()
    {
        State = ChestState.Free;
        _errorSprite.Visible = false;
        _lockSprite.Visible = false;

        base.ForceOpen();
    }

    public override void ResetChest()
    {
        Opacity = 1;
        State = ChestState.Locked;
        TextureColor = Color.White;
        _errorSprite.Visible = false;
        _lockSprite.Visible = true;
        _lockSprite.Opacity = 1;
        _lockSprite.PlayAnimation(1, 1);

        if (ConditionType == ChestConditionType.REACH_IN5_SECONDS)
        {
            Timer = 5;
        }

        base.ResetChest();
    }

    protected override GameObj CreateCloneInstance()
    {
        return new FairyChestObj(PhysicsMngr);
    }

    protected override void FillCloneInstance(object obj)
    {
        base.FillCloneInstance(obj);

        var clone = obj as FairyChestObj;
        clone!.State = State;
        SetConditionType();
    }

    public override void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }

        _player = null;
        _lockSprite?.Dispose();
        _lockSprite = null;
        _errorSprite?.Dispose();
        _errorSprite = null;
        _timerText?.Dispose();
        _timerText = null;

        base.Dispose();
    }
}
