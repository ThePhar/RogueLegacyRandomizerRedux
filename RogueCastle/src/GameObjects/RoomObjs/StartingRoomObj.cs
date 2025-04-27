using System;
using System.Collections.Generic;
using System.Linq;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using RogueCastle.EnvironmentVariables;
using RogueCastle.GameStructs;
using RogueCastle.Managers;
using RogueCastle.Objects;
using RogueCastle.Screens.BaseScreens;
using Tweener;
using Tweener.Ease;

namespace RogueCastle.GameObjects.RoomObjs;

public class StartingRoomObj : RoomObj {
    private const int ENCHANTRESS_HEAD_LAYER = 4;
    private const byte ARCHITECT_HEAD_LAYER = 1;

    private readonly Vector2 _architectIconPosition;
    private readonly Vector2 _blacksmithIconPosition;
    private readonly Vector2 _enchantressIconPosition;
    private ObjContainer _architect;
    private TerrainObj _architectBlock;
    private SpriteObj _architectIcon;
    private bool _architectRenovating;
    private BlacksmithObj _blacksmith;
    private FrameSoundObj _blacksmithAnvilSound;
    private TerrainObj _blacksmithBlock;
    private SpriteObj _blacksmithBoard;
    private SpriteObj _blacksmithIcon;
    private SpriteObj _blacksmithNewIcon;
    private bool _controlsLocked;
    private ObjContainer _enchantress;
    private TerrainObj _enchantressBlock;
    private SpriteObj _enchantressIcon;
    private SpriteObj _enchantressNewIcon;
    private GameObj _fern1, _fern2, _fern3;
    private bool _horizontalShake;
    private bool _isRaining;
    private bool _isSnowing;
    private float _lightningTimer;
    private SpriteObj _mountain1, _mountain2;
    private bool _playerWalkedOut;
    private List<RaindropObj> _rainFG;
    private Cue _rainSFX;
    private float _screenShakeCounter;
    private float _screenShakeMagnitude;
    private SpriteObj _screw;
    private bool _shakeScreen;
    private Vector2 _shakeStartingPos;
    private SpriteObj _tent;
    private PhysicsObjContainer _tollCollector;
    private SpriteObj _tollCollectorIcon;
    private GameObj _tree1, _tree2, _tree3;
    private bool _verticalShake;

    public StartingRoomObj() {
        //TraitSystem.LevelUpTrait(TraitSystem.GetTrait(TraitType.Smithy)); // Debug just so I can see the smithy.
        //TraitSystem.LevelUpTrait(TraitSystem.GetTrait(TraitType.Enchanter)); // Debug just so I can see the enchantress.
        //TraitSystem.LevelUpTrait(TraitSystem.GetTrait(TraitType.Architect)); // Debug just so I can see the architect.

        // Blacksmith
        _blacksmith = new BlacksmithObj {
            Flip = SpriteEffects.FlipHorizontally,
            Scale = new Vector2(2.5f, 2.5f),
            OutlineWidth = 2,
        };
        _blacksmith.Position = new Vector2(700, 720 - 60 - (_blacksmith.Bounds.Bottom - _blacksmith.Y) - 1); // -60 to subtract one tile.
        _blacksmithBoard = new SpriteObj("StartRoomBlacksmithBoard_Sprite") {
            Scale = new Vector2(2, 2),
            OutlineWidth = 2,
        };
        _blacksmithBoard.Position = new Vector2(_blacksmith.X - (_blacksmithBoard.Width / 2) - 35, _blacksmith.Bounds.Bottom - _blacksmithBoard.Height - 1);
        _blacksmithIcon = new SpriteObj("UpArrowBubble_Sprite") {
            Scale = new Vector2(2, 2),
            Visible = false,
            OutlineWidth = 2,
            Flip = _blacksmith.Flip,
        };
        _blacksmithIconPosition = new Vector2(_blacksmith.X - 60, _blacksmith.Y - 10);
        _blacksmithNewIcon = new SpriteObj("ExclamationSquare_Sprite") {
            Visible = false,
            OutlineWidth = 2,
        };

        // Enchantress
        _enchantressNewIcon = _blacksmithNewIcon.Clone() as SpriteObj;
        _enchantress = new ObjContainer("Enchantress_Character") {
            Scale = new Vector2(2f, 2f),
            Flip = SpriteEffects.FlipHorizontally,
            AnimationDelay = 1 / 10f,
            OutlineWidth = 2,
        };
        _enchantress.Position = new Vector2(1150, 720 - 60 - (_enchantress.Bounds.Bottom - _enchantress.AnchorY) - 2);
        _enchantress.PlayAnimation();
        (_enchantress.GetChildAt(ENCHANTRESS_HEAD_LAYER) as IAnimateableObj)!.StopAnimation();
        _enchantressIcon = new SpriteObj("UpArrowBubble_Sprite") {
            Scale = new Vector2(2f, 2f),
            Visible = false,
            Flip = _enchantress.Flip,
            OutlineWidth = 2,
        };
        _enchantressIconPosition = new Vector2(_enchantress.X - 60, _enchantress.Y - 100);
        _tent = new SpriteObj("StartRoomGypsyTent_Sprite") {
            Scale = new Vector2(1.5f, 1.5f),
            OutlineWidth = 2,
        };
        _tent.Position = new Vector2(_enchantress.X - (_tent.Width / 2) + 5, _enchantress.Bounds.Bottom - _tent.Height);

        // Architect
        _architect = new ObjContainer("ArchitectIdle_Character") {
            Flip = SpriteEffects.FlipHorizontally,
            Scale = new Vector2(2, 2),
            AnimationDelay = 1 / 10f,
            OutlineWidth = 2,
        };
        _architect.Position = new Vector2(1550, 720 - 60 - (_architect.Bounds.Bottom - _architect.AnchorY) - 2);
        _architect.PlayAnimation();
        (_architect.GetChildAt(ARCHITECT_HEAD_LAYER) as IAnimateableObj)!.StopAnimation();
        _architectIcon = new SpriteObj("UpArrowBubble_Sprite") {
            Scale = new Vector2(2, 2),
            Visible = false,
            Flip = _architect.Flip,
            OutlineWidth = 2,
        };
        _architectRenovating = false;
        _architectIconPosition = new Vector2(_architect.X - 60, _architect.Y - 100);
        _screw = new SpriteObj("ArchitectGear_Sprite") {
            Scale = new Vector2(2, 2),
            OutlineWidth = 2,
            Position = new Vector2(_architect.X + 30, _architect.Bounds.Bottom - 1),
            AnimationDelay = 1 / 10f,
        };

        // Toll Collector
        _tollCollector = new PhysicsObjContainer("NPCTollCollectorIdle_Character") {
            Flip = SpriteEffects.FlipHorizontally,
            Scale = new Vector2(2.5f, 2.5f),
            IsWeighted = false,
            IsCollidable = true,
            AnimationDelay = 1 / 10f,
            OutlineWidth = 2,
            CollisionTypeTag = GameTypes.COLLISION_TYPE_WALL,
        };
        _tollCollector.Position = new Vector2(2565, 720 - (60 * 5) - (_tollCollector.Bounds.Bottom - _tollCollector.AnchorY));
        _tollCollector.PlayAnimation();
        _tollCollectorIcon = new SpriteObj("UpArrowBubble_Sprite") {
            Scale = new Vector2(2, 2),
            Visible = false,
            Flip = _tollCollector.Flip,
            OutlineWidth = 2,
        };

        _rainFG = [];
        var numRainDrops = 400;
        if (LevelEV.SaveFrames) {
            numRainDrops /= 2;
        }

        for (var i = 0; i < numRainDrops; i++) {
            var rain = new RaindropObj(new Vector2(CDGMath.RandomInt(-100, 1270 * 2), CDGMath.RandomInt(-400, 720)));
            _rainFG.Add(rain);
        }
    }

    private bool BlacksmithNewIconVisible => Game.PlayerStats.GetBlueprintArray.SelectMany(category => category).Any(state => state == EquipmentState.FOUND_BUT_NOT_SEEN);

    private bool EnchantressNewIconVisible => Game.PlayerStats.GetRuneArray.SelectMany(category => category).Any(state => state == EquipmentState.FOUND_BUT_NOT_SEEN);

    private bool SmithyAvailable => SkillSystem.GetSkill(SkillType.Smithy).ModifierAmount > 0;

    private bool EnchantressAvailable => SkillSystem.GetSkill(SkillType.Enchanter).ModifierAmount > 0;

    private bool ArchitectAvailable => SkillSystem.GetSkill(SkillType.Architect).ModifierAmount > 0;

    private bool TollCollectorAvailable => Game.PlayerStats.TimesDead > 0 && _tollCollector.Visible && !Program.Game.ArchipelagoManager.SlotData.DisabledCharon;

    public override void Initialize() {
        foreach (var obj in TerrainObjList) {
            switch (obj.Name) {
                case "BlacksmithBlock":
                    _blacksmithBlock = obj;
                    break;

                case "EnchantressBlock":
                    _enchantressBlock = obj;
                    break;

                case "ArchitectBlock":
                    _architectBlock = obj;
                    break;

                case "bridge":
                    obj.ShowTerrain = false;
                    break;
            }
        }

        foreach (var obj in GameObjList) {
            switch (obj.Name) {
                case "Mountains 1":
                    _mountain1 = obj as SpriteObj;
                    break;

                case "Mountains 2":
                    _mountain2 = obj as SpriteObj;
                    break;
            }
        }

        base.Initialize();
    }

    public override void LoadContent(GraphicsDevice graphics) {
        if (_tree1 == null) {
            foreach (var obj in GameObjList) {
                switch (obj.Name) {
                    case "Tree1":
                        _tree1 = obj;
                        break;

                    case "Tree2":
                        _tree2 = obj;
                        break;

                    case "Tree3":
                        _tree3 = obj;
                        break;

                    case "Fern1":
                        _fern1 = obj;
                        break;

                    case "Fern2":
                        _fern2 = obj;
                        break;

                    case "Fern3":
                        _fern3 = obj;
                        break;
                }
            }
        }

        base.LoadContent(graphics);
    }

    public override void OnEnter() {
        switch (Game.PlayerStats.SpecialItem) {
            // Extra check to make sure you don't have a challenge token for a challenge already beaten.
            case SpecialItemType.EYEBALL_TOKEN when Game.PlayerStats.ChallengeEyeballBeaten:
            case SpecialItemType.SKULL_TOKEN when Game.PlayerStats.ChallengeSkullBeaten:
            case SpecialItemType.FIREBALL_TOKEN when Game.PlayerStats.ChallengeFireballBeaten:
            case SpecialItemType.BLOB_TOKEN when Game.PlayerStats.ChallengeBlobBeaten:
            case SpecialItemType.LAST_BOSS_TOKEN when Game.PlayerStats.ChallengeLastBossBeaten:
                Game.PlayerStats.SpecialItem = SpecialItemType.NONE;
                break;
        }

        Player.AttachedLevel.UpdatePlayerHUDSpecialItem();

        _isSnowing = DateTime.Now.Month == 12 || DateTime.Now.Month == 1; // Only snows in Dec. and Jan.
        if (_isSnowing) {
            foreach (var rainDrop in _rainFG) {
                rainDrop.ChangeToSnowflake();
            }
        }

        if (!Program.Game.SaveManager.FileExists(SaveType.Map) && Game.PlayerStats.HasArchitectFee) {
            Game.PlayerStats.HasArchitectFee = false;
        }

        Game.PlayerStats.TutorialComplete = true; // This needs to be removed later.
        Game.PlayerStats.IsDead = false;

        _lightningTimer = 5;
        Player.CurrentHealth = Player.MaxHealth;
        Player.CurrentMana = Player.MaxMana;
        Player.ForceInvincible = false;
        Program.Game.SaveManager.SaveFiles(SaveType.PlayerData, SaveType.Archipelago);

        if (TollCollectorAvailable && !Player.AttachedLevel.PhysicsManager.ObjectList.Contains(_tollCollector)) {
            Player.AttachedLevel.PhysicsManager.AddObject(_tollCollector);
        }

        _blacksmithAnvilSound ??= new FrameSoundObj(_blacksmith.GetChildAt(5) as IAnimateableObj, Player, 7, "Anvil1", "Anvil2", "Anvil3");

        // Special check for Glaucoma
        if (Game.PlayerStats.HasTrait(TraitType.GLAUCOMA)) {
            Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0.7f);
        } else {
            Game.ShadowEffect.Parameters["ShadowIntensity"].SetValue(0);
        }

        _playerWalkedOut = false;
        Player.UpdateCollisionBoxes(); // Necessary check since the OnEnter() is called before player can update its collision boxes.
        Player.Position = new Vector2(0, 720 - 60 - (Player.Bounds.Bottom - Player.Y));
        Player.State = 1; // Force the player into a walking state. This state will not update until the logic set is complete.
        Player.IsWeighted = false;
        Player.IsCollidable = false;
        var playerMoveLS = new LogicSet(Player);
        playerMoveLS.AddAction(new RunFunctionLogicAction(Player, "LockControls"));
        playerMoveLS.AddAction(new MoveDirectionLogicAction(new Vector2(1, 0)));
        playerMoveLS.AddAction(new ChangeSpriteLogicAction("PlayerWalking_Character"));
        playerMoveLS.AddAction(new PlayAnimationLogicAction());
        playerMoveLS.AddAction(new DelayLogicAction(0.5f));
        playerMoveLS.AddAction(new ChangePropertyLogicAction(Player, "CurrentSpeed", 0));
        playerMoveLS.AddAction(new ChangePropertyLogicAction(Player, "IsWeighted", true));
        playerMoveLS.AddAction(new ChangePropertyLogicAction(Player, "IsCollidable", true));
        Player.RunExternalLogicSet(playerMoveLS); // Do not dispose this logic set. The player object will do it on its own.
        Tween.By(this, 1.0f, Linear.EaseNone);
        Tween.AddEndHandlerToLastTween(Player, "UnlockControls");

        SoundManager.StopMusic(1);

        _isRaining = CDGMath.RandomPlusMinus() > 0;
        _isRaining = true;

        if (_isRaining) {
            _rainSFX?.Dispose();
            _rainSFX = SoundManager.PlaySound(_isSnowing == false ? "Rain1" : "snowloop_filtered");
        }

        _tent.TextureColor = new Color(200, 200, 200);
        _blacksmithBoard.TextureColor = new Color(200, 200, 200);
        _screw.TextureColor = new Color(200, 200, 200);

        if (Game.PlayerStats.LockCastle) {
            _screw.GoToFrame(_screw.TotalFrames);
            _architectBlock.Position = new Vector2(1492, 439 + 140);
        } else {
            _screw.GoToFrame(1);
            _architectBlock.Position = new Vector2(1492, 439);
        }

        Player.UpdateEquipmentColours();

        base.OnEnter();
    }

    public override void OnExit() {
        if (_rainSFX is { IsDisposed: false }) {
            _rainSFX.Stop(AudioStopOptions.Immediate);
        }
    }

    public override void Update(GameTime gameTime) {
        // Player should have max hp and mp while in the starting room.
        Player.CurrentMana = Player.MaxMana;
        Player.CurrentHealth = Player.MaxHealth;

        _enchantressBlock.Visible = EnchantressAvailable;
        _blacksmithBlock.Visible = SmithyAvailable;
        _architectBlock.Visible = ArchitectAvailable;

        var totalSeconds = Game.TotalGameTime;

        if (_playerWalkedOut == false) {
            switch (Player.ControlsLocked) {
                case false when Player.X < Bounds.Left:
                    _playerWalkedOut = true;
                    (Player.AttachedLevel.ScreenManager as RCScreenManager)!.StartWipeTransition();
                    Tween.RunFunction(0.2f, Player.AttachedLevel.ScreenManager, "DisplayScreen", ScreenType.SKILL, true, typeof(List<object>));
                    break;

                // Make sure the player can't pass the toll collector
                case false when Player.X > Bounds.Right && TollCollectorAvailable == false:
                    _playerWalkedOut = true;
                    LoadLevel();
                    break;
            }
        }

        if (_isRaining) {
            foreach (var obj in TerrainObjList) // Optimization
            {
                obj.UseCachedValues = true; // No need to set it back to false.  The physics manager will do that
            }

            foreach (var raindrop in _rainFG) {
                raindrop.Update(TerrainObjList, gameTime);
            }
        }

        _tree1.Rotation = -(float)Math.Sin(totalSeconds) * 2;
        _tree2.Rotation = (float)Math.Sin(totalSeconds * 2);
        _tree3.Rotation = (float)Math.Sin(totalSeconds * 2) * 2;
        _fern1.Rotation = (float)Math.Sin(totalSeconds * 3f) / 2;
        _fern2.Rotation = -(float)Math.Sin(totalSeconds * 4f);
        _fern3.Rotation = (float)Math.Sin(totalSeconds * 4f) / 2f;

        if (_architectRenovating == false) {
            HandleInput();
        }

        if (SmithyAvailable) {
            _blacksmithAnvilSound?.Update();
            _blacksmith.Update(gameTime);
        }

        _blacksmithIcon.Visible = Player != null && CollisionMath.Intersects(Player.TerrainBounds, _blacksmith.Bounds) && Player.IsTouchingGround && SmithyAvailable;
        _blacksmithIcon.Position = new Vector2(_blacksmithIconPosition.X, _blacksmithIconPosition.Y - 70 + ((float)Math.Sin(totalSeconds * 20) * 2));

        var enchantressBounds = new Rectangle((int)(_enchantress.X - 100), (int)_enchantress.Y, _enchantress.Bounds.Width + 100, _enchantress.Bounds.Height);
        _enchantressIcon.Visible = Player != null && CollisionMath.Intersects(Player.TerrainBounds, enchantressBounds) && Player.IsTouchingGround && EnchantressAvailable;
        _enchantressIcon.Position = new Vector2(_enchantressIconPosition.X + 20, _enchantressIconPosition.Y + ((float)Math.Sin(totalSeconds * 20) * 2));

        if (Player != null && CollisionMath.Intersects(Player.TerrainBounds, new Rectangle((int)_architect.X - 100, (int)_architect.Y, _architect.Width + 200, _architect.Height))
                           && Player.X < _architect.X && Player.Flip == SpriteEffects.None && ArchitectAvailable) {
            _architectIcon.Visible = true;
        } else {
            _architectIcon.Visible = false;
        }

        _architectIcon.Position = new Vector2(_architectIconPosition.X, _architectIconPosition.Y + ((float)Math.Sin(totalSeconds * 20) * 2));

        if (Player != null && CollisionMath.Intersects(Player.TerrainBounds, new Rectangle((int)_tollCollector.X - 100, (int)_tollCollector.Y, _tollCollector.Width + 200, _tollCollector.Height))
                           && Player.X < _tollCollector.X && Player.Flip == SpriteEffects.None && TollCollectorAvailable && _tollCollector.SpriteName == "NPCTollCollectorIdle_Character") {
            _tollCollectorIcon.Visible = true;
        } else {
            _tollCollectorIcon.Visible = false;
        }

        _tollCollectorIcon.Position = new Vector2(_tollCollector.X - (_tollCollector.Width / 2) - 10, _tollCollector.Y - _tollCollectorIcon.Height - (_tollCollector.Height / 2) + ((float)Math.Sin(totalSeconds * 20) * 2));

        // Setting blacksmith new icons settings.
        _blacksmithNewIcon.Visible = false;
        if (SmithyAvailable) {
            _blacksmithNewIcon.Visible = _blacksmithIcon.Visible switch {
                true when _blacksmithNewIcon.Visible => false,
                false when BlacksmithNewIconVisible  => true,
                _                                    => _blacksmithNewIcon.Visible,
            };

            _blacksmithNewIcon.Position = new Vector2(_blacksmithIcon.X + 50, _blacksmithIcon.Y - 30);
        }

        // Setting enchantress new icons settings.
        _enchantressNewIcon.Visible = false;
        if (EnchantressAvailable) {
            _enchantressNewIcon.Visible = _enchantressIcon.Visible switch {
                true when _enchantressNewIcon.Visible => false,
                false when EnchantressNewIconVisible  => true,
                _                                     => _enchantressNewIcon.Visible,
            };

            _enchantressNewIcon.Position = new Vector2(_enchantressIcon.X + 40, _enchantressIcon.Y - 0);
        }

        // lightning effect.
        if (_isRaining && _isSnowing == false) {
            if (_lightningTimer > 0) {
                _lightningTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_lightningTimer <= 0) {
                    if (CDGMath.RandomInt(0, 100) > 70) {
                        if (CDGMath.RandomInt(0, 1) > 0) {
                            Player.AttachedLevel.LightningEffectTwice();
                        } else {
                            Player.AttachedLevel.LightningEffectOnce();
                        }
                    }

                    _lightningTimer = 5;
                }
            }
        }

        if (_shakeScreen) {
            UpdateShake();
        }

        // Prevents the player from getting passed the Toll Collector.
        if (Player.Bounds.Right > _tollCollector.Bounds.Left && TollCollectorAvailable) // && !Game.PlayerStats.HasTrait(TraitType.Dwarfism))
        {
            Player.X = _tollCollector.Bounds.Left - (Player.Bounds.Right - Player.X);
            Player.AttachedLevel.UpdateCamera();
        }

        base.Update(gameTime);
    }

    private void LoadLevel() {
        Game.ScreenManager.DisplayScreen(ScreenType.LEVEL, true);
    }

    private void HandleInput() {
        if (_controlsLocked) {
            return;
        }

        var manager = Player.AttachedLevel.ScreenManager as RCScreenManager;

        if (Player.State != PlayerObj.STATE_DASHING) {
            if (_blacksmithIcon.Visible && (Game.GlobalInput.JustPressed(InputMapType.PLAYER_UP1) || Game.GlobalInput.JustPressed(InputMapType.PLAYER_UP2))) {
                MovePlayerTo(_blacksmith);
            }

            if (_enchantressIcon.Visible && (Game.GlobalInput.JustPressed(InputMapType.PLAYER_UP1) || Game.GlobalInput.JustPressed(InputMapType.PLAYER_UP2))) {
                MovePlayerTo(_enchantress);
            }

            if (_architectIcon.Visible && (Game.GlobalInput.JustPressed(InputMapType.PLAYER_UP1) || Game.GlobalInput.JustPressed(InputMapType.PLAYER_UP2))) {
                if (Program.Game.SaveManager.FileExists(SaveType.Map)) {
                    if (Game.PlayerStats.LockCastle == false) {
                        if (Game.PlayerStats.SpokeToArchitect == false) {
                            Game.PlayerStats.SpokeToArchitect = true;
                            manager!.DialogueScreen.SetDialogue("Meet Architect");
                        } else {
                            manager!.DialogueScreen.SetDialogue("Meet Architect 2");
                        }

                        manager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
                        manager.DialogueScreen.SetConfirmEndHandler(this, "ActivateArchitect");
                        manager.DialogueScreen.SetCancelEndHandler(typeof(Console), "WriteLine", "Canceling Selection");
                    } else {
                        manager!.DialogueScreen.SetDialogue("Castle Already Locked Architect");
                    }
                } else {
                    manager!.DialogueScreen.SetDialogue("No Castle Architect");
                }

                manager.DisplayScreen(ScreenType.DIALOGUE, true);
            }
        }

        if (!_tollCollectorIcon.Visible || (!Game.GlobalInput.JustPressed(InputMapType.PLAYER_UP1) && !Game.GlobalInput.JustPressed(InputMapType.PLAYER_UP2))) {
            return;
        }

        switch (Game.PlayerStats.SpecialItem) {
            case SpecialItemType.FREE_ENTRANCE:
                Tween.RunFunction(0.1f, this, "TollPaid", false);
                manager!.DialogueScreen.SetDialogue("Toll Collector Obol");
                manager.DisplayScreen(ScreenType.DIALOGUE, true);
                break;

            case SpecialItemType.EYEBALL_TOKEN:
                manager!.DialogueScreen.SetDialogue("Challenge Icon Eyeball");
                RunTollPaidSelection(manager);
                break;

            case SpecialItemType.SKULL_TOKEN:
                manager!.DialogueScreen.SetDialogue("Challenge Icon Skull");
                RunTollPaidSelection(manager);
                break;

            case SpecialItemType.FIREBALL_TOKEN:
                manager!.DialogueScreen.SetDialogue("Challenge Icon Fireball");
                RunTollPaidSelection(manager);
                break;

            case SpecialItemType.BLOB_TOKEN:
                manager!.DialogueScreen.SetDialogue("Challenge Icon Blob");
                RunTollPaidSelection(manager);
                break;

            case SpecialItemType.LAST_BOSS_TOKEN:
                manager!.DialogueScreen.SetDialogue("Challenge Icon Last Boss");
                RunTollPaidSelection(manager);
                break;

            default: {
                if (!Game.PlayerStats.SpokeToTollCollector) {
                    manager!.DialogueScreen.SetDialogue("Meet Toll Collector 1");
                } else {
                    var amount = SkillSystem.GetSkill(SkillType.PricesDown).ModifierAmount * 100;
                    manager!.DialogueScreen.SetDialogue("Meet Toll Collector Skip" + (int)Math.Round(amount, MidpointRounding.AwayFromZero));
                }

                RunTollPaidSelection(manager);
                break;
            }
        }
    }

    private void RunTollPaidSelection(RCScreenManager manager) {
        manager.DialogueScreen.SetDialogueChoice("ConfirmTest1");
        manager.DialogueScreen.SetConfirmEndHandler(this, "TollPaid", true);
        manager.DialogueScreen.SetCancelEndHandler(typeof(Console), "WriteLine", "Canceling Selection");
        manager.DisplayScreen(ScreenType.DIALOGUE, true);
    }

    public void MovePlayerTo(GameObj target) {
        _controlsLocked = true;
        if (Player.X != target.X - 150) {
            if (Player.X > target.Position.X - 150) {
                Player.Flip = SpriteEffects.FlipHorizontally;
            }

            var duration = CDGMath.DistanceBetweenPts(Player.Position, new Vector2(target.X - 150, target.Y)) / Player.Speed;

            Player.UpdateCollisionBoxes(); // Necessary check since the OnEnter() is called before player can update its collision boxes.
            Player.State = 1; // Force the player into a walking state. This state will not update until the logic set is complete.
            Player.IsWeighted = false;
            Player.AccelerationY = 0;
            Player.AccelerationX = 0;
            Player.IsCollidable = false;
            Player.CurrentSpeed = 0;
            Player.LockControls();
            Player.ChangeSprite("PlayerWalking_Character");
            Player.PlayAnimation();
            var playerMoveLS = new LogicSet(Player);
            playerMoveLS.AddAction(new DelayLogicAction(duration));
            Player.RunExternalLogicSet(playerMoveLS);
            Tween.To(Player, duration, Tween.EaseNone, "X", $"{target.Position.X - 150}");
            Tween.AddEndHandlerToLastTween(this, "MovePlayerComplete", target);
            return;
        }

        MovePlayerComplete(target);
    }

    public void MovePlayerComplete(GameObj target) {
        _controlsLocked = false;
        Player.IsWeighted = true;
        Player.IsCollidable = true;
        Player.UnlockControls();
        Player.Flip = SpriteEffects.None;

        var manager = Player.AttachedLevel.ScreenManager as RCScreenManager;

        if (target == _blacksmith) {
            if (Game.PlayerStats.SpokeToBlacksmith == false) {
                Game.PlayerStats.SpokeToBlacksmith = true;
                manager!.DialogueScreen.SetDialogue("Meet Blacksmith");
                manager.DialogueScreen.SetConfirmEndHandler(Player.AttachedLevel.ScreenManager, "DisplayScreen", ScreenType.BLACKSMITH, true, null);
                manager.DisplayScreen(ScreenType.DIALOGUE, true);
            } else {
                manager!.DisplayScreen(ScreenType.BLACKSMITH, true);
            }
        } else if (target == _enchantress) {
            if (Game.PlayerStats.SpokeToEnchantress == false) {
                Game.PlayerStats.SpokeToEnchantress = true;
                manager!.DialogueScreen.SetDialogue("Meet Enchantress");
                manager.DialogueScreen.SetConfirmEndHandler(Player.AttachedLevel.ScreenManager, "DisplayScreen", ScreenType.ENCHANTRESS, true, null);
                manager.DisplayScreen(ScreenType.DIALOGUE, true);
            } else {
                manager!.DisplayScreen(ScreenType.ENCHANTRESS, true);
            }
        }
    }

    public void TollPaid(bool chargeFee) {
        if (chargeFee) {
            var goldLost = Game.PlayerStats.Gold * (GameEV.GATEKEEPER_TOLL_COST - SkillSystem.GetSkill(SkillType.PricesDown).ModifierAmount);
            Game.PlayerStats.Gold -= (int)goldLost;
            if (goldLost > 0) {
                Player.AttachedLevel.TextManager.DisplayNumberStringText(-(int)goldLost, "LOC_ID_PLAYER_OBJ_1" /*"gold"*/, Color.Yellow, new Vector2(Player.X, Player.Bounds.Top));
            }
        }

        if (Game.PlayerStats.SpokeToTollCollector && Game.PlayerStats.SpecialItem != SpecialItemType.FREE_ENTRANCE
                                                  && Game.PlayerStats.SpecialItem != SpecialItemType.BLOB_TOKEN
                                                  && Game.PlayerStats.SpecialItem != SpecialItemType.LAST_BOSS_TOKEN
                                                  && Game.PlayerStats.SpecialItem != SpecialItemType.FIREBALL_TOKEN
                                                  && Game.PlayerStats.SpecialItem != SpecialItemType.EYEBALL_TOKEN
                                                  && Game.PlayerStats.SpecialItem != SpecialItemType.SKULL_TOKEN) {
            Player.AttachedLevel.ImpactEffectPool.DisplayDeathEffect(_tollCollector.Position);
            SoundManager.PlaySound("Charon_Laugh");
            HideTollCollector();
        } else {
            Game.PlayerStats.SpokeToTollCollector = true;
            SoundManager.PlaySound("Charon_Laugh");
            _tollCollector.ChangeSprite("NPCTollCollectorLaugh_Character");
            _tollCollector.AnimationDelay = 1 / 20f;
            _tollCollector.PlayAnimation();
            Tween.RunFunction(1, Player.AttachedLevel.ImpactEffectPool, "DisplayDeathEffect", _tollCollector.Position);
            Tween.RunFunction(1, this, "HideTollCollector");
        }

        if (Game.PlayerStats.SpecialItem == SpecialItemType.FREE_ENTRANCE ||
            Game.PlayerStats.SpecialItem == SpecialItemType.SKULL_TOKEN ||
            Game.PlayerStats.SpecialItem == SpecialItemType.EYEBALL_TOKEN ||
            Game.PlayerStats.SpecialItem == SpecialItemType.LAST_BOSS_TOKEN ||
            Game.PlayerStats.SpecialItem == SpecialItemType.FIREBALL_TOKEN ||
            Game.PlayerStats.SpecialItem == SpecialItemType.BLOB_TOKEN) {
            switch (Game.PlayerStats.SpecialItem) {
                case SpecialItemType.EYEBALL_TOKEN:
                    Game.PlayerStats.ChallengeEyeballUnlocked = true;
                    break;

                case SpecialItemType.SKULL_TOKEN:
                    Game.PlayerStats.ChallengeSkullUnlocked = true;
                    break;

                case SpecialItemType.FIREBALL_TOKEN:
                    Game.PlayerStats.ChallengeFireballUnlocked = true;
                    break;

                case SpecialItemType.BLOB_TOKEN:
                    Game.PlayerStats.ChallengeBlobUnlocked = true;
                    break;

                case SpecialItemType.LAST_BOSS_TOKEN:
                    Game.PlayerStats.ChallengeLastBossUnlocked = true;
                    break;
            }

            Game.PlayerStats.SpecialItem = SpecialItemType.NONE;
            Player.AttachedLevel.UpdatePlayerHUDSpecialItem();
        }
    }

    public void HideTollCollector() {
        SoundManager.Play3DSound(this, Player, "Charon_Poof");
        _tollCollector.Visible = false;
        Player.AttachedLevel.PhysicsManager.RemoveObject(_tollCollector);
    }

    public void ActivateArchitect() {
        Player.LockControls();
        Player.CurrentSpeed = 0;
        _architectIcon.Visible = false;
        _architectRenovating = true;
        _architect.ChangeSprite("ArchitectPull_Character");
        (_architect.GetChildAt(1) as SpriteObj)!.PlayAnimation(false);
        _screw.AnimationDelay = 1 / 30f;

        Game.PlayerStats.ArchitectUsed = true;

        Tween.RunFunction(0.5f, _architect.GetChildAt(0), "PlayAnimation", true);
        Tween.RunFunction(0.5f, typeof(SoundManager), "PlaySound", "Architect_Lever");
        Tween.RunFunction(1, typeof(SoundManager), "PlaySound", "Architect_Screw");
        Tween.RunFunction(1f, _screw, "PlayAnimation", false);
        Tween.By(_architectBlock, 0.8f, Tween.EaseNone, "delay", "1.1", "Y", "135");
        Tween.RunFunction(1f, this, "ShakeScreen", 2, true, false);
        Tween.RunFunction(1.5f, this, "StopScreenShake");
        Tween.RunFunction(1.5f, Player.AttachedLevel.ImpactEffectPool, "SkillTreeDustEffect", new Vector2(_screw.X - (_screw.Width / 2f), _screw.Y - 40), true, _screw.Width);
        Tween.RunFunction(3f, this, "StopArchitectActivation");
    }

    public void StopArchitectActivation() {
        _architectRenovating = false;
        _architectIcon.Visible = true;
        Player.UnlockControls();

        Game.PlayerStats.LockCastle = true;
        Game.PlayerStats.HasArchitectFee = true;

        foreach (var chest in Player.AttachedLevel.ChestList) // Resetting all fairy chests.
        {
            if (chest is FairyChestObj { State: ChestState.Failed } fairyChest) {
                fairyChest.ResetChest();
            }
        }

        foreach (var breakableObj in Player.AttachedLevel.RoomList.SelectMany(room => room.GameObjList.OfType<BreakableObj>())) {
            breakableObj.Reset();
        }

        var manager = Player.AttachedLevel.ScreenManager as RCScreenManager;
        manager!.DialogueScreen.SetDialogue("Castle Lock Complete Architect");
        manager.DisplayScreen(ScreenType.DIALOGUE, true);
    }

    public override void Draw(Camera2D camera) {
        // Hacked parallaxing.
        _mountain1.X = camera.TopLeftCorner.X * 0.5f;
        _mountain2.X = _mountain1.X + 2640; // 2640 not 1320 because it is mountain1 flipped.

        base.Draw(camera);

        if (_isRaining) {
            camera.Draw(Game.GenericTexture, new Rectangle(0, 0, 1320 * 2, 720), Color.Black * 0.3f);
        }

        if (_screenShakeCounter > 0) {
            camera.X += CDGMath.RandomPlusMinus();
            camera.Y += CDGMath.RandomPlusMinus();
            _screenShakeCounter -= (float)camera.GameTime.ElapsedGameTime.TotalSeconds;
        }

        if (SmithyAvailable) {
            _blacksmithBoard.Draw(camera);
            _blacksmith.Draw(camera);
            _blacksmithIcon.Draw(camera);
        }

        if (EnchantressAvailable) {
            _tent.Draw(camera);
            _enchantress.Draw(camera);
            _enchantressIcon.Draw(camera);
        }

        if (ArchitectAvailable) {
            _screw.Draw(camera);
            _architect.Draw(camera);
            _architectIcon.Draw(camera);
        }

        if (TollCollectorAvailable) {
            _tollCollector.Draw(camera);
            _tollCollectorIcon.Draw(camera);
        }

        _blacksmithNewIcon.Draw(camera);
        _enchantressNewIcon.Draw(camera);

        if (_isRaining) {
            foreach (var raindrop in _rainFG) {
                raindrop.Draw(camera);
            }
        }
    }

    public override void PauseRoom() {
        foreach (var rainDrop in _rainFG) {
            rainDrop.PauseAnimation();
        }

        _rainSFX?.Pause();

        _enchantress.PauseAnimation();
        _blacksmith.PauseAnimation();
        _architect.PauseAnimation();
        _tollCollector.PauseAnimation();

        base.PauseRoom();
    }

    public override void UnpauseRoom() {
        foreach (var rainDrop in _rainFG) {
            rainDrop.ResumeAnimation();
        }

        if (_rainSFX is { IsPaused: true }) {
            _rainSFX.Resume();
        }

        _enchantress.ResumeAnimation();
        _blacksmith.ResumeAnimation();
        _architect.ResumeAnimation();
        _tollCollector.ResumeAnimation();

        base.UnpauseRoom();
    }

    public void ShakeScreen(float magnitude, bool horizontalShake = true, bool verticalShake = true) {
        _shakeStartingPos = Player.AttachedLevel.Camera.Position;
        Player.AttachedLevel.CameraLockedToPlayer = false;
        _screenShakeMagnitude = magnitude;
        _horizontalShake = horizontalShake;
        _verticalShake = verticalShake;
        _shakeScreen = true;
    }

    public void UpdateShake() {
        if (_horizontalShake) {
            Player.AttachedLevel.Camera.X = _shakeStartingPos.X + (CDGMath.RandomPlusMinus() * (CDGMath.RandomFloat(0, 1) * _screenShakeMagnitude));
        }

        if (_verticalShake) {
            Player.AttachedLevel.Camera.Y = _shakeStartingPos.Y + (CDGMath.RandomPlusMinus() * (CDGMath.RandomFloat(0, 1) * _screenShakeMagnitude));
        }
    }

    public void StopScreenShake() {
        Player.AttachedLevel.CameraLockedToPlayer = true;
        _shakeScreen = false;
    }

    protected override GameObj CreateCloneInstance() {
        return new StartingRoomObj();
    }

    public override void Dispose() {
        if (IsDisposed) {
            return;
        }

        _blacksmith.Dispose();
        _blacksmith = null;
        _blacksmithIcon.Dispose();
        _blacksmithIcon = null;
        _blacksmithNewIcon.Dispose();
        _blacksmithNewIcon = null;
        _blacksmithBoard.Dispose();
        _blacksmithBoard = null;
        _enchantress.Dispose();
        _enchantress = null;
        _enchantressIcon.Dispose();
        _enchantressIcon = null;
        _enchantressNewIcon.Dispose();
        _enchantressNewIcon = null;
        _tent.Dispose();
        _tent = null;
        _architect.Dispose();
        _architect = null;
        _architectIcon.Dispose();
        _architectIcon = null;
        _screw.Dispose();
        _screw = null;
        _blacksmithAnvilSound?.Dispose();
        _blacksmithAnvilSound = null;
        _tree1 = null;
        _tree2 = null;
        _tree3 = null;
        _fern1 = null;
        _fern2 = null;
        _fern3 = null;

        foreach (var raindrop in _rainFG) {
            raindrop.Dispose();
        }

        _rainFG.Clear();
        _rainFG = null;
        _mountain1 = null;
        _mountain2 = null;
        _tollCollector.Dispose();
        _tollCollector = null;
        _tollCollectorIcon.Dispose();
        _tollCollectorIcon = null;
        _blacksmithBlock = null;
        _enchantressBlock = null;
        _architectBlock = null;
        _rainSFX?.Dispose();
        _rainSFX = null;

        base.Dispose();
    }
}
