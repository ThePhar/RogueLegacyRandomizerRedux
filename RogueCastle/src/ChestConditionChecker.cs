using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.GameStructs;
using RogueCastle.Objects;

namespace RogueCastle;

public enum ChestState {
    Locked,
    Free,
    Failed,
}

public static class ChestConditionChecker {
    private const int DISTANCE_THRESHOLD = 100;

    public static void SetConditionState(FairyChestObj chest, PlayerObj player) {
        switch (chest.ConditionType) {
            case ChestConditionType.INVISIBLE_CHEST:
            case ChestConditionType.NONE:
                if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < DISTANCE_THRESHOLD) {
                    chest.SetChestUnlocked();
                }

                break;

            case ChestConditionType.KILL_ALL_ENEMIES:
                if (player.AttachedLevel.CurrentRoom.ActiveEnemies <= 0) {
                    chest.SetChestUnlocked();
                }

                break;

            case ChestConditionType.DONT_LOOK:
                var flipCheck = SpriteEffects.None;
                if (chest.AbsPosition.X < player.AbsPosition.X) // Chest is to the left of the player.
                {
                    flipCheck = SpriteEffects.FlipHorizontally;
                }

                if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < 375 &&
                    Vector2.Distance(chest.AbsPosition, player.AbsPosition) > DISTANCE_THRESHOLD &&
                    player.Flip == flipCheck) {
                    chest.SetChestFailed();
                } else if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < DISTANCE_THRESHOLD) {
                    chest.SetChestUnlocked();
                }

                break;

            case ChestConditionType.NO_JUMPING:
                if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < 10000 &&
                    Vector2.Distance(chest.AbsPosition, player.AbsPosition) > DISTANCE_THRESHOLD &&
                    player.IsJumping &&
                    player.AccelerationY < 0) {
                    chest.SetChestFailed();
                } else if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < DISTANCE_THRESHOLD) {
                    chest.SetChestUnlocked();
                }

                break;

            case ChestConditionType.NO_FLOOR: // This one isn't being used.
                if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) > DISTANCE_THRESHOLD &&
                    Vector2.Distance(chest.AbsPosition, player.AbsPosition) < 1000 &&
                    player.IsTouchingGround) {
                    chest.SetChestFailed();
                } else if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < DISTANCE_THRESHOLD) {
                    chest.SetChestUnlocked();
                }

                break;

            case ChestConditionType.NO_ATTACKING_ENEMIES:
                if (player.AttachedLevel.CurrentRoom.EnemyList.Any(enemy => enemy.CurrentHealth < enemy.MaxHealth)) {
                    chest.SetChestFailed();
                }

                if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < DISTANCE_THRESHOLD &&
                    chest.State == ChestState.Locked) {
                    chest.SetChestUnlocked();
                }

                break;

            case ChestConditionType.REACH_IN5_SECONDS:
                if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) > DISTANCE_THRESHOLD && chest.Timer <= 0) {
                    chest.SetChestFailed();
                } else if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < DISTANCE_THRESHOLD && chest.Timer > 0) {
                    chest.SetChestUnlocked();
                }

                break;

            case ChestConditionType.TAKE_NO_DAMAGE:
                if (player.State == PlayerObj.STATE_HURT) {
                    chest.SetChestFailed();
                } else if (Vector2.Distance(chest.AbsPosition, player.AbsPosition) < DISTANCE_THRESHOLD) {
                    chest.SetChestUnlocked();
                }

                break;
        }
    }
}
