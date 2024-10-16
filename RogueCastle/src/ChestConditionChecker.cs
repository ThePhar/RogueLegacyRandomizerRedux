using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.Objects;

namespace RogueCastle;

public enum ChestState
{
    Locked,
    Free,
    Failed,
}

public static class ChestConditionChecker
{
    private const int DistanceThreshold = 100;

    public static void SetConditionState(FairyChestObj chest, PlayerObj player)
    {
        switch (chest.ConditionType)
        {
            case ChestConditionType.InvisibleChest:
            case ChestConditionType.None:
                if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < DistanceThreshold)
                {
                    chest.SetChestUnlocked();
                }

                break;

            case ChestConditionType.KillAllEnemies:
                if (player.AttachedLevel.CurrentRoom.ActiveEnemies <= 0)
                {
                    chest.SetChestUnlocked();
                }

                break;

            case ChestConditionType.DontLook:
                var flipCheck = SpriteEffects.None;
                if (chest.AbsPosition.X < player.AbsPosition.X) // Chest is to the left of the player.
                {
                    flipCheck = SpriteEffects.FlipHorizontally;
                }

                if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < 375 &&
                    Vector2.Distance(chest.AbsPosition, player.AbsPosition) > DistanceThreshold &&
                    player.Flip == flipCheck)
                {
                    chest.SetChestFailed();
                }
                else if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < DistanceThreshold)
                {
                    chest.SetChestUnlocked();
                }

                break;

            case ChestConditionType.NoJumping:
                if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < 10000 &&
                    Vector2.Distance(chest.AbsPosition, player.AbsPosition) > DistanceThreshold &&
                    player.IsJumping &&
                    player.AccelerationY < 0)
                {
                    chest.SetChestFailed();
                }
                else if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < DistanceThreshold)
                {
                    chest.SetChestUnlocked();
                }

                break;

            case ChestConditionType.NoFloor: // This one isn't being used.
                if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) > DistanceThreshold &&
                    Vector2.Distance(chest.AbsPosition, player.AbsPosition) < 1000 &&
                    player.IsTouchingGround)
                {
                    chest.SetChestFailed();
                }
                else if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < DistanceThreshold)
                {
                    chest.SetChestUnlocked();
                }

                break;

            case ChestConditionType.NoAttackingEnemies:
                if (player.AttachedLevel.CurrentRoom.EnemyList.Any(enemy => enemy.CurrentHealth < enemy.MaxHealth))
                {
                    chest.SetChestFailed();
                }

                if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < DistanceThreshold &&
                    chest.State == ChestState.Locked)
                {
                    chest.SetChestUnlocked();
                }

                break;

            case ChestConditionType.ReachIn5Seconds:
                if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) > DistanceThreshold && chest.Timer <= 0)
                {
                    chest.SetChestFailed();
                }
                else if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < DistanceThreshold && chest.Timer > 0)
                {
                    chest.SetChestUnlocked();
                }

                break;

            case ChestConditionType.TakeNoDamage:
                if (player.State == PlayerObj.STATE_HURT)
                {
                    chest.SetChestFailed();
                }
                else if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < DistanceThreshold)
                {
                    chest.SetChestUnlocked();
                }

                break;
        }
    }
}
