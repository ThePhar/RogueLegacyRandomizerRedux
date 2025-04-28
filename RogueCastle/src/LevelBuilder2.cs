using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using DS2DEngine;
using Microsoft.Xna.Framework;
using RogueCastle.EnvironmentVariables;
using RogueCastle.GameObjects.RoomObjs;
using RogueCastle.GameStructs;
using RogueCastle.Screens;

namespace RogueCastle;

internal static class LevelBuilder2 {
    private const int MAX_ROOM_SIZE = 4;

    private static readonly List<RoomObj>[,] CastleRoomArray = new List<RoomObj>[MAX_ROOM_SIZE, MAX_ROOM_SIZE];
    private static readonly List<RoomObj>[,] GardenRoomArray = new List<RoomObj>[MAX_ROOM_SIZE, MAX_ROOM_SIZE];
    private static readonly List<RoomObj>[,] TowerRoomArray = new List<RoomObj>[MAX_ROOM_SIZE, MAX_ROOM_SIZE];
    private static readonly List<RoomObj>[,] DungeonRoomArray = new List<RoomObj>[MAX_ROOM_SIZE, MAX_ROOM_SIZE];

    private static List<RoomObj> _bossRoomArray;
    private static RoomObj _testRoom;
    private static RoomObj _castleEntranceRoom;
    private static RoomObj _linkerCastleRoom;
    private static RoomObj _linkerDungeonRoom;
    private static RoomObj _linkerGardenRoom;
    private static RoomObj _linkerTowerRoom;
    private static RoomObj _bossCastleEntranceRoom;
    private static RoomObj _bossDungeonEntranceRoom;
    private static RoomObj _bossGardenEntranceRoom;
    private static RoomObj _bossTowerEntranceRoom;
    private static List<RoomObj> _secretCastleRoomArray;
    private static List<RoomObj> _secretGardenRoomArray;
    private static List<RoomObj> _secretTowerRoomArray;
    private static List<RoomObj> _secretDungeonRoomArray;
    private static List<RoomObj> _bonusCastleRoomArray;
    private static List<RoomObj> _bonusGardenRoomArray;
    private static List<RoomObj> _bonusTowerRoomArray;
    private static List<RoomObj> _bonusDungeonRoomArray;
    private static List<RoomObj> _dlcCastleRoomArray;
    private static List<RoomObj> _dlcGardenRoomArray;
    private static List<RoomObj> _dlcTowerRoomArray;
    private static List<RoomObj> _dlcDungeonRoomArray;
    private static RoomObj _tutorialRoom;
    private static RoomObj _throneRoom;
    private static RoomObj _endingRoom;
    private static CompassRoomObj _compassRoom;

    // Challenge Room Content.
    private static List<RoomObj> _challengeRoomArray;

    private static bool _hasTopDoor;
    private static bool _hasBottomDoor;
    private static bool _hasLeftDoor;
    private static bool _hasRightDoor;

    private static bool _hasTopLeftDoor;
    private static bool _hasTopRightDoor;
    private static bool _hasBottomLeftDoor;
    private static bool _hasBottomRightDoor;
    private static bool _hasRightTopDoor;
    private static bool _hasRightBottomDoor;
    private static bool _hasLeftTopDoor;
    private static bool _hasLeftBottomDoor;

    public static RoomObj StartingRoom { get; private set; }

    public static List<RoomObj> SequencedRoomList {
        get {
            // Add the special rooms first.
            var sequencedRoomList = new List<RoomObj> {
                StartingRoom,
                _linkerCastleRoom,
                _linkerTowerRoom,
                _linkerDungeonRoom,
                _linkerGardenRoom,
                _bossCastleEntranceRoom,
                _bossTowerEntranceRoom,
                _bossDungeonEntranceRoom,
                _bossGardenEntranceRoom,
                _castleEntranceRoom,
            };

            // Add the normal rooms.
            foreach (List<RoomObj> roomList in CastleRoomArray) {
                sequencedRoomList.AddRange(roomList);
            }

            foreach (List<RoomObj> roomList in DungeonRoomArray) {
                sequencedRoomList.AddRange(roomList);
            }

            foreach (List<RoomObj> roomList in TowerRoomArray) {
                sequencedRoomList.AddRange(roomList);
            }

            foreach (List<RoomObj> roomList in GardenRoomArray) {
                sequencedRoomList.AddRange(roomList);
            }

            // Add the secret rooms.
            sequencedRoomList.AddRange(_secretCastleRoomArray);
            sequencedRoomList.AddRange(_secretTowerRoomArray);
            sequencedRoomList.AddRange(_secretDungeonRoomArray);
            sequencedRoomList.AddRange(_secretGardenRoomArray);

            // Add the bonus rooms.
            sequencedRoomList.AddRange(_bonusCastleRoomArray);
            sequencedRoomList.AddRange(_bonusTowerRoomArray);
            sequencedRoomList.AddRange(_bonusDungeonRoomArray);
            sequencedRoomList.AddRange(_bonusGardenRoomArray);

            // Add the boss room array.
            sequencedRoomList.AddRange(_bossRoomArray);

            // Add the challenge room array.
            sequencedRoomList.AddRange(_challengeRoomArray);

            //Add the compass room.
            sequencedRoomList.Add(_compassRoom);

            for (var i = 0; i < sequencedRoomList.Count; i++) {
                if (sequencedRoomList[i] != null) {
                    continue;
                }

                Console.WriteLine($@"WARNING: Null room found at index {i} of sequencedRoomList.  Removing room...");
                sequencedRoomList.RemoveAt(i);
                i--;
            }

            return sequencedRoomList;
        }
    }

    public static void Initialize() {
        for (var i = 0; i < MAX_ROOM_SIZE; i++)
        for (var k = 0; k < MAX_ROOM_SIZE; k++) {
            CastleRoomArray[i, k] = [];
            DungeonRoomArray[i, k] = [];
            TowerRoomArray[i, k] = [];
            GardenRoomArray[i, k] = [];
        }

        _secretCastleRoomArray = [];
        _secretGardenRoomArray = [];
        _secretTowerRoomArray = [];
        _secretDungeonRoomArray = [];
        _bonusCastleRoomArray = [];
        _bonusGardenRoomArray = [];
        _bonusTowerRoomArray = [];
        _bonusDungeonRoomArray = [];
        _bossRoomArray = [];
        _challengeRoomArray = [];
        _dlcCastleRoomArray = [];
        _dlcDungeonRoomArray = [];
        _dlcGardenRoomArray = [];
        _dlcTowerRoomArray = [];
    }

    public static void StoreRoom(RoomObj room, GameTypes.LevelType levelType) {
        // Ignore non-special rooms.
        if (room.Name is "Start" or "Linker" or "Boss" or "EntranceBoss" or "Secret" or "Bonus" or "CastleEntrance" or
            "Throne" or "Tutorial" or "Ending" or "Compass" or "DEBUG_ROOM" or "ChallengeBoss") {
            return;
        }

        if (room.Width % GlobalEV.SCREEN_WIDTH != 0) {
            throw new Exception($"Room Name: {room.Name} is not a width divisible by {GlobalEV.SCREEN_WIDTH}. Cannot parse the file.");
        }

        if (room.Height % GlobalEV.SCREEN_HEIGHT != 0) {
            throw new Exception($"Room Name: {room.Name} is not a height divisible by {GlobalEV.SCREEN_HEIGHT}. Cannot parse the file.");
        }

        var i = room.Width / GlobalEV.SCREEN_WIDTH;
        var k = room.Height / GlobalEV.SCREEN_HEIGHT;

        if (room.IsDLCMap == false) {
            List<RoomObj>[,] roomArray = levelType switch {
                GameTypes.LevelType.Castle  => CastleRoomArray,
                GameTypes.LevelType.Dungeon => DungeonRoomArray,
                GameTypes.LevelType.Tower   => TowerRoomArray,
                GameTypes.LevelType.Garden  => GardenRoomArray,
                _                           => null,
            };

            roomArray![i - 1, k - 1].Add(room.Clone() as RoomObj);

            // Do not reverse the actual room.
            var roomClone = room.Clone() as RoomObj;
            roomClone!.Reverse();
            roomArray[i - 1, k - 1].Add(roomClone);
        } else {
            // Storing DLC maps in a separate array.
            List<RoomObj> dlcRoomArray = GetSequencedDLCRoomList(levelType);
            dlcRoomArray.Add(room.Clone() as RoomObj);

            // Do not reverse the actual room.
            var roomClone = room.Clone() as RoomObj;
            roomClone!.Reverse();
            dlcRoomArray.Add(roomClone);
        }
    }

    public static void StoreSpecialRoom(RoomObj room, GameTypes.LevelType levelType, bool storeDebug = false) {
        if (storeDebug) {
            _testRoom = room.Clone() as RoomObj;
            _testRoom!.LevelType = LevelEV.TestRoomLevelType;
        }

        switch (room.Name) {
            case "Start":
                if (StartingRoom == null) // Only store the first found copy of the starting room.
                {
                    StartingRoom = new StartingRoomObj();
                    StartingRoom.CopyRoomProperties(room);
                    StartingRoom.CopyRoomObjects(room);
                }

                break;

            case "Linker":
                var linkerRoom = room.Clone() as RoomObj;

                switch (levelType) {
                    case GameTypes.LevelType.Castle:
                        _linkerCastleRoom = linkerRoom;
                        break;

                    case GameTypes.LevelType.Dungeon:
                        _linkerDungeonRoom = linkerRoom;
                        break;

                    case GameTypes.LevelType.Tower:
                        _linkerTowerRoom = linkerRoom;
                        break;

                    case GameTypes.LevelType.Garden:
                        _linkerGardenRoom = linkerRoom;
                        break;
                }

                var teleporter = new TeleporterObj();
                teleporter.Position = new Vector2(linkerRoom!.X + (linkerRoom.Width / 2f) - (teleporter.Bounds.Right - teleporter.AnchorX), linkerRoom.Y + linkerRoom.Height - 60);
                linkerRoom.GameObjList.Add(teleporter);
                break;

            case "Boss":
                // Locking the doors to boss rooms so you can't exit a boss fight.
                foreach (var door in room.DoorList.Where(door => door.IsBossDoor))
                {
                    door.Locked = true;
                }

                _bossRoomArray.Add(room.Clone() as RoomObj);
                break;

            case "EntranceBoss":
                var bossEntranceRoom = room.Clone() as RoomObj;
                var bossTeleporter = new TeleporterObj();
                bossTeleporter.Position = new Vector2(bossEntranceRoom!.X + (bossEntranceRoom.Width / 2f) - (bossTeleporter.Bounds.Right - bossTeleporter.AnchorX), bossEntranceRoom.Y + bossEntranceRoom.Height - 60);
                bossEntranceRoom.GameObjList.Add(bossTeleporter);

                NpcObj donationBox = null;
                foreach (var obj in bossEntranceRoom.GameObjList.Where(obj => obj.Name == "donationbox"))
                {
                    donationBox = new NpcObj((obj as ObjContainer)!.SpriteName) { 
                        useArrowIcon = true,
                        Position = obj.Position,
                        Name = obj.Name,
                        Scale = obj.Scale,
                    };
                    donationBox.Y -= 2;
                    obj.Visible = false;
                }

                if (donationBox != null) {
                    bossEntranceRoom.GameObjList.Add(donationBox);
                }

                switch (levelType) {
                    case GameTypes.LevelType.Castle:
                        _bossCastleEntranceRoom = bossEntranceRoom;
                        break;

                    case GameTypes.LevelType.Dungeon:
                        _bossDungeonEntranceRoom = bossEntranceRoom;
                        break;

                    case GameTypes.LevelType.Tower:
                        _bossTowerEntranceRoom = bossEntranceRoom;
                        break;

                    case GameTypes.LevelType.Garden:
                        _bossGardenEntranceRoom = bossEntranceRoom;
                        break;
                }

                break;

            case "CastleEntrance":
                // Only store the first copy of the castle entrance room.
                if (_castleEntranceRoom == null) {
                    _castleEntranceRoom = new CastleEntranceRoomObj();
                    _castleEntranceRoom.CopyRoomProperties(room);
                    _castleEntranceRoom.CopyRoomObjects(room);
                    _castleEntranceRoom.LevelType = GameTypes.LevelType.Castle;
                }

                break;

            case "Compass":
                if (_compassRoom == null) {
                    _compassRoom = new CompassRoomObj();
                    _compassRoom.CopyRoomProperties(room);
                    _compassRoom.CopyRoomObjects(room);
                }

                break;

            case "Secret":
                List<RoomObj> secretRoomArray = null;

                switch (levelType) {
                    case GameTypes.LevelType.Castle:
                        secretRoomArray = _secretCastleRoomArray;
                        break;

                    case GameTypes.LevelType.Dungeon:
                        secretRoomArray = _secretDungeonRoomArray;
                        break;

                    case GameTypes.LevelType.Tower:
                        secretRoomArray = _secretTowerRoomArray;
                        break;

                    case GameTypes.LevelType.Garden:
                        secretRoomArray = _secretGardenRoomArray;
                        break;
                }

                secretRoomArray!.Add(room.Clone() as RoomObj);
                
                // Do not reverse the actual room.
                var roomClone = room.Clone() as RoomObj;
                roomClone!.Reverse();
                secretRoomArray.Add(roomClone);
                break;

            case "Bonus":
                List<RoomObj> bonusRoomArray = null;

                switch (levelType) {
                    case GameTypes.LevelType.Castle:
                        bonusRoomArray = _bonusCastleRoomArray;
                        break;

                    case GameTypes.LevelType.Dungeon:
                        bonusRoomArray = _bonusDungeonRoomArray;
                        break;

                    case GameTypes.LevelType.Tower:
                        bonusRoomArray = _bonusTowerRoomArray;
                        break;

                    case GameTypes.LevelType.Garden:
                        bonusRoomArray = _bonusGardenRoomArray;
                        break;
                }

                bonusRoomArray!.Add(room.Clone() as RoomObj);
                var bonusRoomClone = room.Clone() as RoomObj;

                // Do not reverse the actual room.
                bonusRoomClone!.Reverse(); 
                bonusRoomArray.Add(bonusRoomClone);
                break;

            case "Tutorial":
                if (_tutorialRoom == null) {
                    _tutorialRoom = new TutorialRoomObj();
                    _tutorialRoom.CopyRoomProperties(room);
                    _tutorialRoom.CopyRoomObjects(room);
                }

                break;

            case "Throne":
                if (_throneRoom == null) {
                    _throneRoom = new ThroneRoomObj();
                    _throneRoom.CopyRoomProperties(room);
                    _throneRoom.CopyRoomObjects(room);
                }

                break;

            case "Ending":
                if (_endingRoom == null) {
                    _endingRoom = new EndingRoomObj();
                    _endingRoom.CopyRoomProperties(room);
                    _endingRoom.CopyRoomObjects(room);
                }

                break;

            case "ChallengeBoss":
                // Locking the doors to challenge rooms so you can't exit the challenge.
                foreach (var door in room.DoorList.Where(door => door.IsBossDoor))
                {
                    door.Locked = true;
                }

                _challengeRoomArray.Add(room.Clone() as RoomObj);
                break;
        }
    }

    // todo
    public static List<RoomObj> CreateArea(int roomCount, AreaStruct area, List<RoomObj> collisionChecklist, RoomObj seedRoom, bool isStartingRoom) {
        // Get the appropriate room arrays (creates a copy).
        var (secretRooms, bonusRooms) = area.LevelType switch {
            GameTypes.LevelType.Castle  => (_secretCastleRoomArray.ToList(), _bonusCastleRoomArray.ToList()),
            GameTypes.LevelType.Garden  => (_secretGardenRoomArray.ToList(), _bonusGardenRoomArray.ToList()),
            GameTypes.LevelType.Tower   => (_secretTowerRoomArray.ToList(), _bonusTowerRoomArray.ToList()),
            GameTypes.LevelType.Dungeon => (_secretDungeonRoomArray.ToList(), _bonusDungeonRoomArray.ToList()),
            _                           => (null, null),
        };

        // Validate we don't require more rooms than we actually have.
        if (area.SecretRooms.Max > secretRooms.Count) {
            throw new Exception($"Cannot add {area.SecretRooms.Max} secret rooms from pool of {secretRooms.Count} secret rooms.");
        }

        if (area.BonusRooms.Max > bonusRooms.Count) {
            throw new Exception($"Cannot add {area.BonusRooms.Max} bonus rooms from pool of {bonusRooms.Count} bonus rooms.");
        }

        // The way boss rooms work, is as more and more rooms get built, the chance of a boss room appearing increases,
        // until it hits 100% for the final room.
        var bossAdded = !area.BossInArea;
        var bossRoomChance = area.BossInArea ? 0 : -100f;
        var bossRoomChanceIncrease = 100f / roomCount;
        
        // Calculate needs for secrets and bonus rooms.
        var secretRoomsNeeded = CDGMath.RandomInt(area.SecretRooms.Min, area.SecretRooms.Max);
        var secretRoomTotal = secretRoomsNeeded;
        var secretRoomIncrease = roomCount / (secretRoomsNeeded + 1); // The number of rooms you need to increase by before adding a secret room.
        var nextSecretRoomToAdd = secretRoomIncrease; // This is where the first secret room should be added.

        var bonusRoomsNeeded = CDGMath.RandomInt(area.BonusRooms.Min, area.BonusRooms.Max);
        var bonusRoomTotal = bonusRoomsNeeded;
        var bonusRoomIncrease = roomCount / (bonusRoomsNeeded + 1);
        var nextBonusRoomToAdd = bonusRoomIncrease;

        // Creating a copy of all the rooms in the level so that I can manipulate this list at will without affecting the actual list.
        collisionChecklist = collisionChecklist.ToList();

        var doorsToLink = new List<DoorObj>(); // This might run better as a linked list or maybe a FIFO queue.
        var roomList = new List<RoomObj>();
        var numRoomsLeftToCreate = roomCount;
        var (doorLPct, doorRPct, doorTPct, doorBPct, startingDoorPosition) = area.LevelType switch {
            GameTypes.LevelType.Castle => (
                LevelEV.LEVEL_CASTLE_LEFTDOOR,
                LevelEV.LEVEL_CASTLE_RIGHTDOOR,
                LevelEV.LEVEL_CASTLE_TOPDOOR,
                LevelEV.LEVEL_CASTLE_BOTTOMDOOR,
                "Right"
            ),
            GameTypes.LevelType.Garden => (
                LevelEV.LEVEL_GARDEN_LEFTDOOR,
                LevelEV.LEVEL_GARDEN_RIGHTDOOR,
                LevelEV.LEVEL_GARDEN_TOPDOOR,
                LevelEV.LEVEL_GARDEN_BOTTOMDOOR,
                "Right"
            ),
            GameTypes.LevelType.Tower => (
                LevelEV.LEVEL_TOWER_LEFTDOOR,
                LevelEV.LEVEL_TOWER_RIGHTDOOR,
                LevelEV.LEVEL_TOWER_TOPDOOR,
                LevelEV.LEVEL_TOWER_BOTTOMDOOR,
                "Top"
            ),
            GameTypes.LevelType.Dungeon => (
                LevelEV.LEVEL_DUNGEON_LEFTDOOR,
                LevelEV.LEVEL_DUNGEON_RIGHTDOOR,
                LevelEV.LEVEL_DUNGEON_TOPDOOR,
                LevelEV.LEVEL_DUNGEON_BOTTOMDOOR,
                "Bottom"
            ),
            _ => throw new ArgumentOutOfRangeException($"Unexpected area: {area.LevelType}"),
        };

        DoorObj startingDoor = null;

        // Place the first room.
        if (isStartingRoom) {
            roomList.Add(seedRoom);
            collisionChecklist.Add(seedRoom);

            seedRoom.LevelType = GameTypes.LevelType.None;
            numRoomsLeftToCreate--; // Because the starting room is added to the list so reduce the number of rooms that need to be made by 1.
            MoveRoom(seedRoom, Vector2.Zero); // Sets the starting room to position (0,0) for simplicity.

            // Adding the castle entrance room to the game after the starting room.
            var castleEntrance = _castleEntranceRoom.Clone() as RoomObj;
            roomList.Add(castleEntrance);
            collisionChecklist.Add(castleEntrance);
            numRoomsLeftToCreate--;
            MoveRoom(castleEntrance, new Vector2(seedRoom.X + seedRoom.Width, seedRoom.Bounds.Bottom - castleEntrance!.Height));

            seedRoom = castleEntrance; // This last line is extremely important. It sets this room as the new starting room to attach new rooms to.
        }

        //Finding the first door to the right in the starting room
        foreach (var door in seedRoom.DoorList.Where(door => door.DoorPosition == startingDoorPosition)) {
            doorsToLink.Add(door);
            startingDoor = door;
            break;
        }

        // Making sure the starting room has a door positioned to the right.
        if (doorsToLink.Count == 0)
        {
            throw new Exception($"The starting room does not have a {startingDoorPosition} positioned door. Cannot create level.");
        }

        while (numRoomsLeftToCreate > 0) {
            if (doorsToLink.Count <= 0) {
                Console.WriteLine(@"    ERROR: Ran out of available rooms to make.");
                break;
            }

            var linkDoor = false; // Each door has a percentage chance of linking to another room. This bool keeps track of that.
            var needsLinking = doorsToLink[0]; // Always link the first door in the list. Once that's done, remove the door from the list and go through the list again.

            // If there are still rooms to build but this is the last available door in the list, force it open.
            if ((doorsToLink.Count <= 5 && needsLinking != startingDoor) || needsLinking == startingDoor)
            {
                linkDoor = true;
            } else {
                // Each door has a percentage chance of linking to another room. This code determines that chance.
                var percentChance = needsLinking.DoorPosition switch {
                    "Left"   => doorLPct,
                    "Right"  => doorRPct,
                    "Top"    => doorTPct,
                    "Bottom" => doorBPct,
                    _        => 100,
                };

                if (percentChance - CDGMath.RandomInt(1, 100) >= 0) {
                    linkDoor = true;
                }
            }

            if (linkDoor == false) {
                doorsToLink.Remove(needsLinking);
                continue;
            }

            List<DoorObj> suitableDoorList = null;

            var addingBossRoom = false;
            if (bossRoomChance >= CDGMath.RandomInt(50, 100) && bossAdded == false) {
                var bossEntranceRoom = area.LevelType switch {
                    GameTypes.LevelType.Castle  => _bossCastleEntranceRoom,
                    GameTypes.LevelType.Dungeon => _bossDungeonEntranceRoom,
                    GameTypes.LevelType.Garden  => _bossGardenEntranceRoom,
                    GameTypes.LevelType.Tower   => _bossTowerEntranceRoom,
                    _                           => null,
                };

                addingBossRoom = true;
                var doorNeeded = GetOppositeDoorPosition(needsLinking.DoorPosition);
                suitableDoorList = [];
                foreach (var door in bossEntranceRoom.DoorList.Where(door => door.DoorPosition == doorNeeded && !CheckForRoomCollision(needsLinking, collisionChecklist, door) && !suitableDoorList.Contains(door)))
                {
                    bossAdded = true;
                    suitableDoorList.Add(door);
                    break;
                }
            } else {
                bossRoomChance += bossRoomChanceIncrease;
            }

            var addingSpecialRoom = false;
            var addingSecretRoom = false;

            // Only continue adding rooms if the boss room isn't being added at the moment or if no suitable boss room could be found.
            if ((addingBossRoom && bossAdded == false) || addingBossRoom == false)
            {
                if (roomList.Count >= nextSecretRoomToAdd && secretRoomsNeeded > 0) // Add a secret room instead of a normal room.
                {
                    addingSpecialRoom = true;
                    addingSecretRoom = true;
                    suitableDoorList = FindSuitableDoors(needsLinking, secretRooms, collisionChecklist);
                } else if (roomList.Count >= nextBonusRoomToAdd && bonusRoomsNeeded > 0) // Add a bonus room instead of a normal room.
                {
                    addingSpecialRoom = true;
                    suitableDoorList = FindSuitableDoors(needsLinking, bonusRooms, collisionChecklist);
                }

                // If you are not adding a special room, or if you are adding a special room but you cannot find a suitable room, add a normal room.
                if (addingSpecialRoom == false || (addingSpecialRoom && suitableDoorList.Count == 0)) {
                    if (roomList.Count < 5) // When building a level early, make sure you don't accidentally choose rooms with no doors (or only 1 door) in them.
                    {
                        suitableDoorList = FindSuitableDoors(needsLinking, MAX_ROOM_SIZE, MAX_ROOM_SIZE, collisionChecklist, area.LevelType, true); // Searches all rooms up to the specified size and finds suitable doors to connect to the current door.
                    } else // The list of rooms already added needs to be passed in to check for room collisions.
                    {
                        suitableDoorList = FindSuitableDoors(needsLinking, MAX_ROOM_SIZE, MAX_ROOM_SIZE, collisionChecklist, area.LevelType, false); // Searches all rooms up to the specified size and finds suitable doors to connect to the current door.
                    }
                } else // You are adding a special room and you have found a suitable list of rooms it can connect to. Yay!
                {
                    if (addingSecretRoom) {
                        nextSecretRoomToAdd = roomList.Count + secretRoomIncrease;
                        secretRoomsNeeded--;
                    } else if (addingSecretRoom == false) {
                        nextBonusRoomToAdd = roomList.Count + bonusRoomIncrease;
                        bonusRoomsNeeded--;
                    }
                }
            }

            // If for some reason not a single suitable room could be found, remove this door from the list of doors that need rooms connected to them.
            if (suitableDoorList.Count == 0) {
                doorsToLink.Remove(needsLinking);
            } else {
                var randomDoorIndex = CDGMath.RandomInt(0, suitableDoorList.Count - 1); // Choose a random door index from the suitableDoorList to attach to the door.
                CDGMath.Shuffle(suitableDoorList);
                var doorToLinkTo = suitableDoorList[randomDoorIndex];

                // This code prevents the same special rooms from being added to an area twice.
                if (addingSpecialRoom) {
                    switch (addingSecretRoom)
                    {
                        case true:
                            secretRooms.Remove(doorToLinkTo.Room);
                            break;

                        case false:
                            bonusRooms.Remove(doorToLinkTo.Room);
                            break;
                    }
                }

                var roomToAdd = doorToLinkTo.Room.Clone() as RoomObj; // Make sure to get a clone of the room since suitableDoorList returns a list of suitable rooms from the actual LevelBuilder array.
                foreach (var door in roomToAdd.DoorList.Where(door => door.Position == doorToLinkTo.Position))
                {
                    doorToLinkTo = door; // Because roomToAdd is cloned from doorToLinkTo, doorToLinkTo needs to relink itself to the cloned room.
                    break;
                }

                roomToAdd.LevelType = area.LevelType; // Setting the room LevelType.
                roomToAdd.TextureColor = area.Color; // Setting the room Color.
                roomList.Add(roomToAdd);
                collisionChecklist.Add(roomToAdd); // Add the newly selected room to the list that checks for room collisions.

                // Positioning the newly linked room next to the starting room based on the door's position.
                var newRoomPosition = needsLinking.DoorPosition switch {
                    "Left"   => new Vector2(needsLinking.X - doorToLinkTo.Room.Width, needsLinking.Y - (doorToLinkTo.Y - doorToLinkTo.Room.Y)),
                    "Right"  => new Vector2(needsLinking.X + needsLinking.Width, needsLinking.Y - (doorToLinkTo.Y - doorToLinkTo.Room.Y)),
                    "Top"    => new Vector2(needsLinking.X - (doorToLinkTo.X - doorToLinkTo.Room.X), needsLinking.Y - doorToLinkTo.Room.Height),
                    "Bottom" => new Vector2(needsLinking.X - (doorToLinkTo.X - doorToLinkTo.Room.X), needsLinking.Y + needsLinking.Height),
                    _        => Vector2.Zero,
                };

                MoveRoom(roomToAdd, newRoomPosition);

                numRoomsLeftToCreate--; // Reducing the number of rooms that need to be made.
                doorsToLink.Remove(needsLinking); // Remove the door that was just linked from the list of doors that need linking.

                // Add all doors (except the door that is linked to) in the newly created room to the list of doors that need linking.
                doorsToLink.AddRange(roomToAdd.DoorList.Where(door => door != doorToLinkTo && door.Room != seedRoom && door.X >= StartingRoom.Width));

                // And finally what is perhaps the most important part, set the Attached flag to each door to true so that there is a way to know if this door is connected to another door.
                doorToLinkTo.Attached = true;
                needsLinking.Attached = true;
            }
        }

        // Check if we created all the secret and bonus rooms we needed.
        if (secretRoomsNeeded != 0) {
            Console.WriteLine($@"WARNING: Only {(secretRoomTotal - secretRoomsNeeded)} secret rooms of {secretRoomTotal} creation attempts were successful");
        }

        if (bonusRoomsNeeded != 0) {
            Console.WriteLine($@"WARNING: Only {(bonusRoomTotal - bonusRoomsNeeded)} secret rooms of {bonusRoomTotal} creation attempts were successful");
        }

        return roomList;
    }

    private static List<DoorObj> FindSuitableDoors(DoorObj doorToCheck, int roomWidth, int roomHeight, List<RoomObj> roomList, GameTypes.LevelType levelType, bool findRoomsWithMoreDoors) {
        var suitableDoorList = new List<DoorObj>();
        var doorPositionToCheck = GetOppositeDoorPosition(doorToCheck.DoorPosition);

        for (var i = 1; i <= roomWidth; i++)
        for (var k = 1; k <= roomHeight; k++) {
            List<RoomObj> gameRoomList = GetRoomList(i, k, levelType);
            foreach (var room in gameRoomList) {
                // If findRoomsWithMoreDoors == true, it will only add rooms with more than 1 door (no dead ends).
                var allowRoom = findRoomsWithMoreDoors == false || room.DoorList.Count > 1;
                if (!allowRoom) {
                    continue;
                }

                foreach (var door in room.DoorList) {
                    if (door.DoorPosition != doorPositionToCheck) {
                        continue;
                    }

                    var collisionFound = CheckForRoomCollision(doorToCheck, roomList, door);

                    if (collisionFound == false && suitableDoorList.Contains(door) == false) {
                        suitableDoorList.Add(door);
                    }
                }
            }
        }

        // Appending the DLC rooms.
        List<RoomObj> dlcRoomList = GetSequencedDLCRoomList(levelType);
        foreach (var room in dlcRoomList) {
            // If findRoomsWithMoreDoors == true, it will only add rooms with more than 1 door (no dead ends).
            var allowRoom = !findRoomsWithMoreDoors || room.DoorList.Count > 1;
            if (!allowRoom) {
                continue;
            }

            foreach (var door in room.DoorList) {
                if (door.DoorPosition != doorPositionToCheck) {
                    continue;
                }

                var collisionFound = CheckForRoomCollision(doorToCheck, roomList, door);

                if (collisionFound == false && suitableDoorList.Contains(door) == false) {
                    suitableDoorList.Add(door);
                }
            }
        }

        return suitableDoorList;
    }

    // Same as method above, except simpler. Finds a suitable list of doors from the provided room list.
    // Used for adding secret and bonus rooms.
    private static List<DoorObj> FindSuitableDoors(DoorObj doorToCheck, List<RoomObj> roomList, List<RoomObj> roomCollisionList) {
        var suitableDoorList = new List<DoorObj>();
        var doorPositionToCheck = GetOppositeDoorPosition(doorToCheck.DoorPosition);

        foreach (var room in roomList) {
            // Do not add diary rooms to the procedural generation if you have unlocked all the diaries.
            if (Game.PlayerStats.DiaryEntry >= LevelEV.TOTAL_JOURNAL_ENTRIES - 1 && room.Name == "Bonus" && room.Tag == BonusRoomType.DIARY.ToString()) {
                continue;
            }

            foreach (var door in room.DoorList) {
                if (door.DoorPosition != doorPositionToCheck) {
                    continue;
                }

                var collisionFound = CheckForRoomCollision(doorToCheck, roomCollisionList, door);
                if (collisionFound == false && suitableDoorList.Contains(door) == false) {
                    suitableDoorList.Add(door);
                }
            }
        }

        return suitableDoorList;
    }

    private static void RemoveDoorFromRoom(DoorObj doorToRemove) {
        //Add a wall to the room where the door is and remove the door, effectively closing it.
        var roomToCloseDoor = doorToRemove.Room;

        var doorTerrain = new TerrainObj(doorToRemove.Width, doorToRemove.Height);
        doorTerrain.AddCollisionBox(0, 0, doorTerrain.Width, doorTerrain.Height, Consts.TERRAIN_HITBOX);
        doorTerrain.AddCollisionBox(0, 0, doorTerrain.Width, doorTerrain.Height, Consts.BODY_HITBOX);
        doorTerrain.Position = doorToRemove.Position;
        roomToCloseDoor.TerrainObjList.Add(doorTerrain);

        // Add the newly created wall's borders.
        var borderObj = new BorderObj { Position = doorTerrain.Position };
        borderObj.SetWidth(doorTerrain.Width);
        borderObj.SetHeight(doorTerrain.Height);

        switch (doorToRemove.DoorPosition) {
            case "Left":
                borderObj.BorderRight = true;
                break;

            case "Right":
                borderObj.BorderLeft = true;
                break;

            case "Top":
                borderObj.BorderBottom = true;
                break;

            case "Bottom":
                borderObj.BorderTop = true;
                break;
        }

        roomToCloseDoor.BorderList.Add(borderObj);
        roomToCloseDoor.DoorList.Remove(doorToRemove);
        doorToRemove.Dispose();
    }

    public static void CloseRemainingDoors(List<RoomObj> roomList) {
        var doorList = new List<DoorObj>();
        var allDoorList = new List<DoorObj>();

        foreach (var door in from room in roomList from door in room.DoorList where door.DoorPosition != "None" select door) {
            allDoorList.Add(door);
            if (door.Attached == false) {
                doorList.Add(door);
            }
        }

        foreach (var firstDoor in doorList) {
            var removeDoor = true;

            foreach (var secondDoor in allDoorList.Where(secondDoor => firstDoor != secondDoor)) {
                switch (firstDoor.DoorPosition) {
                    case "Left":
                        if (secondDoor.X < firstDoor.X && CollisionMath.Intersects(new Rectangle((int)(firstDoor.X - 5), (int)firstDoor.Y, firstDoor.Width, firstDoor.Height), secondDoor.Bounds)) {
                            removeDoor = false;
                        }

                        break;

                    case "Right":
                        if (secondDoor.X > firstDoor.X && CollisionMath.Intersects(new Rectangle((int)(firstDoor.X + 5), (int)firstDoor.Y, firstDoor.Width, firstDoor.Height), secondDoor.Bounds)) {
                            removeDoor = false;
                        }

                        break;

                    case "Top":
                        if (secondDoor.Y < firstDoor.Y && CollisionMath.Intersects(new Rectangle((int)firstDoor.X, (int)(firstDoor.Y - 5), firstDoor.Width, firstDoor.Height), secondDoor.Bounds)) {
                            removeDoor = false;
                        }

                        break;

                    case "Bottom":
                        if (secondDoor.Y > firstDoor.Y && CollisionMath.Intersects(new Rectangle((int)(firstDoor.X - 5), (int)(firstDoor.Y + 5), firstDoor.Width, firstDoor.Height), secondDoor.Bounds)) {
                            removeDoor = false;
                        }

                        break;

                    case "None":
                        removeDoor = false;
                        break;
                }
            }

            if (removeDoor) {
                RemoveDoorFromRoom(firstDoor);
            } else {
                firstDoor.Attached = true;
            }
        }
    }

    public static void AddDoorBorders(List<RoomObj> roomList) {
        foreach (var door in roomList.SelectMany(room => room.DoorList)) {
            // Code for attaching borders to the doors.
            switch (door.DoorPosition) {
                case "Left":
                case "Right":
                    // -60 because a grid tile is 60 large.
                    var bottomBorder = new BorderObj { Position = new Vector2(door.Room.X + (door.X - door.Room.X), door.Room.Y + (door.Y - door.Room.Y) - 60) };
                    bottomBorder.SetWidth(door.Width);
                    bottomBorder.SetHeight(60);
                    bottomBorder.BorderBottom = true;
                    door.Room.BorderList.Add(bottomBorder);

                    var topBorder = new BorderObj { Position = new Vector2(door.Room.X + (door.X - door.Room.X), door.Room.Y + (door.Y - door.Room.Y) + door.Height) };
                    topBorder.SetWidth(door.Width);
                    topBorder.SetHeight(60);
                    topBorder.BorderTop = true;
                    door.Room.BorderList.Add(topBorder);
                    break;

                case "Top":
                case "Bottom":
                    var rightBorder = new BorderObj { Position = new Vector2(door.Room.X + (door.X - door.Room.X) + door.Width, door.Room.Y + (door.Y - door.Room.Y)) };
                    rightBorder.SetWidth(60);
                    rightBorder.SetHeight(door.Height);
                    rightBorder.BorderLeft = true;
                    door.Room.BorderList.Add(rightBorder);

                    var leftBorder = new BorderObj { Position = new Vector2(door.Room.X + (door.X - door.Room.X) - 60, door.Room.Y + (door.Y - door.Room.Y)) };
                    leftBorder.SetWidth(60);
                    leftBorder.SetHeight(door.Height);
                    leftBorder.BorderRight = true;
                    door.Room.BorderList.Add(leftBorder);
                    break;
            }
        }
    }

    //1. Search all the rooms, and find the lowest room with an available door. Available doors are all doors that are not the opposite of doorPositionWanted.
    //2. Add a linker room to that door.
    //3. Keep only the door that is connecting the room and the linker room and the doorPositionWanted door open.
    //4. Return the doorPositionWanted door of the linker room.
    //NOTE: furthestRoomDirection is the furthest room you want to find in a specific direction. 
    //      doorPositionWanted is the door position that should be returned when the room is found and linker room is added.
    public static DoorObj FindFurthestDoor(List<RoomObj> roomList, string furthestRoomDirection, string doorPositionWanted, bool addLinkerRoom, bool castleOnly) {
        var oppositeOfDoorWanted = GetOppositeDoorPosition(doorPositionWanted);

        var startingRoom = roomList[0];
        float furthestDistance = -10;
        DoorObj doorToLinkTo = null; // Only used if doorToReturn is null at the end.

        foreach (var room in roomList) {
            if (room == startingRoom || ((room.LevelType != GameTypes.LevelType.Castle || !castleOnly) && castleOnly)) {
                continue;
            }

            var distance = furthestRoomDirection switch {
                // Furthest room to the right.
                "Right" => room.X - startingRoom.X,
                // Leftmost room.
                "Left" => startingRoom.X - room.X,
                // Highest room.
                "Top" => startingRoom.Y - room.Y,
                // Lowest room.
                "Bottom" => room.Y - startingRoom.Y,
                _        => 0,
            };

            // Will find rooms the same distance away but will not override doorToReturn if one is found.
            if (distance < furthestDistance) {
                continue;
            }

            foreach (var door in room.DoorList.Where(door => door.DoorPosition != "None")) {
                if (door.DoorPosition != doorPositionWanted && door.DoorPosition == oppositeOfDoorWanted) {
                    continue;
                }

                var addRoom = roomList.All(collidingRoom => collidingRoom == door.Room || !CollisionMath.Intersects(new Rectangle((int)collidingRoom.X - 10, (int)collidingRoom.Y - 10, collidingRoom.Width + 20, collidingRoom.Height + 20), door.Bounds));
                if (!addRoom) {
                    continue;
                }

                doorToLinkTo = door;
                furthestDistance = distance;
                break;
            }
        }

        if (doorToLinkTo is not null) {
            return addLinkerRoom ? AddLinkerToRoom(roomList, doorToLinkTo, doorPositionWanted) : doorToLinkTo;
        }

        Console.WriteLine(@"Could not find suitable furthest door. That's a problem.");
        return null;
    }

    public static DoorObj AddLinkerToRoom(List<RoomObj> roomList, DoorObj needsLinking, string doorPositionWanted) {
        DoorObj doorToReturn = null;
        var linkerRoom = needsLinking.Room.LevelType switch {
            GameTypes.LevelType.Castle  => _linkerCastleRoom.Clone() as RoomObj,
            GameTypes.LevelType.Dungeon => _linkerDungeonRoom.Clone() as RoomObj,
            GameTypes.LevelType.Tower   => _linkerTowerRoom.Clone() as RoomObj,
            GameTypes.LevelType.Garden  => _linkerGardenRoom.Clone() as RoomObj,
            _                           => null,
        };

        linkerRoom!.TextureColor = needsLinking.Room.TextureColor;

        var doorToLinkToPosition = GetOppositeDoorPosition(needsLinking.DoorPosition);
        var doorToLinkTo = linkerRoom.DoorList.FirstOrDefault(door => door.DoorPosition == doorToLinkToPosition);

        // Positioning the newly linked room next to the starting room based on the door's position.
        var newRoomPosition = needsLinking.DoorPosition switch {
            "Left"   => new Vector2(needsLinking.X - doorToLinkTo!.Room.Width, needsLinking.Y - (doorToLinkTo.Y - doorToLinkTo.Room.Y)),
            "Right"  => new Vector2(needsLinking.X + needsLinking.Width, needsLinking.Y - (doorToLinkTo.Y - doorToLinkTo.Room.Y)),
            "Top"    => new Vector2(needsLinking.X - (doorToLinkTo!.X - doorToLinkTo.Room.X), needsLinking.Y - doorToLinkTo.Room.Height),
            "Bottom" => new Vector2(needsLinking.X - (doorToLinkTo!.X - doorToLinkTo.Room.X), needsLinking.Y + needsLinking.Height),
            _        => Vector2.Zero,
        };

        MoveRoom(linkerRoom, newRoomPosition);

        needsLinking.Attached = true;
        doorToLinkTo!.Attached = true;

        foreach (var door in linkerRoom.DoorList.Where(door => door.DoorPosition == doorPositionWanted)) {
            doorToReturn = door;
        }

        linkerRoom.LevelType = needsLinking.Room.LevelType;
        roomList.Add(linkerRoom);
        return doorToReturn;
    }

    public static void AddRemoveExtraObjects(List<RoomObj> roomList) {
        foreach (var room in roomList) {
            _hasTopDoor = false;
            _hasBottomDoor = false;
            _hasLeftDoor = false;
            _hasRightDoor = false;

            _hasTopLeftDoor = false;
            _hasTopRightDoor = false;
            _hasBottomLeftDoor = false;
            _hasBottomRightDoor = false;
            _hasRightTopDoor = false;
            _hasRightBottomDoor = false;
            _hasLeftTopDoor = false;
            _hasLeftBottomDoor = false;

            foreach (var door in room.DoorList) {
                switch (door.DoorPosition) {
                    case "Top":
                        _hasTopDoor = true;
                        if (door.X - room.X == 540) {
                            _hasTopLeftDoor = true;
                        }

                        if (room.Bounds.Right - door.X == 780) {
                            _hasTopRightDoor = true;
                        }

                        break;

                    case "Bottom":
                        _hasBottomDoor = true;
                        if (door.X - room.X == 540) {
                            _hasBottomLeftDoor = true;
                        }

                        if (room.Bounds.Right - door.X == 780) {
                            _hasBottomRightDoor = true;
                        }

                        break;

                    case "Left":
                        _hasLeftDoor = true;
                        if (door.Y - room.Y == 240) {
                            _hasLeftTopDoor = true;
                        }

                        if (room.Bounds.Bottom - door.Y == 480) {
                            _hasLeftBottomDoor = true;
                        }

                        break;

                    case "Right":
                        _hasRightDoor = true;
                        if (door.Y - room.Y == 240) {
                            _hasRightTopDoor = true;
                        }

                        if (room.Bounds.Bottom - door.Y == 480) {
                            _hasRightBottomDoor = true;
                        }

                        break;
                }
            }

            RemoveFromListHelper(room.TerrainObjList);
            RemoveFromListHelper(room.GameObjList);
            RemoveFromListHelper(room.EnemyList);
            RemoveFromListHelper(room.BorderList);
        }
    }

    [SuppressMessage("ReSharper", "StringIndexOfIsCultureSpecific.1")]
    private static void RemoveFromListHelper<T>(List<T> list) {
        for (var i = 0; i < list.Count; i++) {
            var name = (list[i] as GameObj)!.Name;
            if (name == null) {
                continue;
            }

            if ((_hasTopLeftDoor == false && name.IndexOf("TopLeft") != -1 && name.IndexOf("!TopLeft") == -1) || (_hasTopLeftDoor && name.IndexOf("!TopLeft") != -1) ||
                (_hasTopRightDoor == false && name.IndexOf("TopRight") != -1 && name.IndexOf("!TopRight") == -1) || (_hasTopRightDoor && name.IndexOf("!TopRight") != -1) ||
                (_hasTopDoor == false && name.IndexOf("Top") != -1 && name.IndexOf("!Top") == -1 && name.Length == 3) || (_hasTopDoor && name.IndexOf("!Top") != -1 && name.Length == 4)) {
                list.Remove(list[i]);
                i--;
            }

            if ((_hasBottomLeftDoor == false && name.IndexOf("BottomLeft") != -1 && name.IndexOf("!BottomLeft") == -1) || (_hasBottomLeftDoor && name.IndexOf("!BottomLeft") != -1) ||
                (_hasBottomRightDoor == false && name.IndexOf("BottomRight") != -1 && name.IndexOf("!BottomRight") == -1) || (_hasBottomRightDoor && name.IndexOf("!BottomRight") != -1) ||
                (_hasBottomDoor == false && name.IndexOf("Bottom") != -1 && name.IndexOf("!Bottom") == -1 && name.Length == 6) || (_hasBottomDoor && name.IndexOf("!Bottom") != -1 && name.Length == 7)) {
                list.Remove(list[i]);
                i--;
            }

            if ((_hasLeftTopDoor == false && name.IndexOf("LeftTop") != -1 && name.IndexOf("!LeftTop") == -1) || (_hasLeftTopDoor && name.IndexOf("!LeftTop") != -1) ||
                (_hasLeftBottomDoor == false && name.IndexOf("LeftBottom") != -1 && name.IndexOf("!LeftBottom") == -1) || (_hasLeftBottomDoor && name.IndexOf("!LeftBottom") != -1) ||
                (_hasLeftDoor == false && name.IndexOf("Left") != -1 && name.IndexOf("!Left") == -1 && name.Length == 4) || (_hasLeftDoor && name.IndexOf("!Left") != -1 && name.Length == 5)) {
                list.Remove(list[i]);
                i--;
            }

            if ((_hasRightTopDoor == false && name.IndexOf("RightTop") != -1 && name.IndexOf("!RightTop") == -1) || (_hasRightTopDoor && name.IndexOf("!RightTop") != -1) ||
                (_hasRightBottomDoor == false && name.IndexOf("RightBottom") != -1 && name.IndexOf("!RightBottom") == -1) || (_hasRightBottomDoor && name.IndexOf("!RightBottom") != -1) ||
                (_hasRightDoor == false && name.IndexOf("Right") != -1 && name.IndexOf("!Right") == -1 && name.Length == 5) || (_hasRightDoor && name.IndexOf("!Right") != -1 && name.Length == 6)) {
                list.Remove(list[i]);
                i--;
            }
        }
    }

    // todo
    public static void AddProceduralEnemies(List<RoomObj> roomList) {
        var startingRoomPos = roomList[0].Position;

        foreach (var room in roomList) {
            // Setting the pool of the types of the enemies the room can select from.
            byte[] enemyPool = null;
            byte[] enemyDifficultyPool = null;
            switch (room.LevelType) {
                default:
                case GameTypes.LevelType.Castle:
                    enemyPool = LevelEV.CastleEnemyList;
                    enemyDifficultyPool = LevelEV.CastleEnemyDifficultyList;
                    break;

                case GameTypes.LevelType.Garden:
                    enemyPool = LevelEV.GardenEnemyList;
                    enemyDifficultyPool = LevelEV.GardenEnemyDifficultyList;
                    break;

                case GameTypes.LevelType.Tower:
                    enemyPool = LevelEV.TowerEnemyList;
                    enemyDifficultyPool = LevelEV.TowerEnemyDifficultyList;
                    break;

                case GameTypes.LevelType.Dungeon:
                    enemyPool = LevelEV.DungeonEnemyList;
                    enemyDifficultyPool = LevelEV.DungeonEnemyDifficultyList;
                    break;
            }

            if (enemyPool.Length != enemyDifficultyPool.Length) {
                throw new Exception("Cannot create enemy. Enemy pool != enemy difficulty pool - LevelBuilder2.cs - AddProceduralEnemies()");
            }

            // Selecting the random enemy types for each specific orb colour.
            var randomEnemyOrbIndex = CDGMath.RandomInt(0, enemyPool.Length - 1);
            var storedEnemyIndex = randomEnemyOrbIndex;

            // Storing red enemy type.
            var redEnemyType = enemyPool[randomEnemyOrbIndex];
            var redEnemyDifficulty = enemyDifficultyPool[randomEnemyOrbIndex];

            //Shuffling to get a new enemy type.
            while (randomEnemyOrbIndex == storedEnemyIndex) // Code to prevent two different coloured orbs from having the same enemy type. Disabled for now since the castle only has 1 enemy type.
            {
                randomEnemyOrbIndex = CDGMath.RandomInt(0, enemyPool.Length - 1);
            }

            storedEnemyIndex = randomEnemyOrbIndex;

            // Storing blue enemy type.
            var blueEnemyType = enemyPool[randomEnemyOrbIndex];
            var blueEnemyDifficulty = enemyDifficultyPool[randomEnemyOrbIndex];

            // Shuffling to get a new enemy type.
            while (randomEnemyOrbIndex == storedEnemyIndex) {
                randomEnemyOrbIndex = CDGMath.RandomInt(0, enemyPool.Length - 1);
            }

            storedEnemyIndex = randomEnemyOrbIndex;

            // Storing green enemy type.
            var greenEnemyType = enemyPool[randomEnemyOrbIndex];
            var greenEnemyDifficulty = enemyDifficultyPool[randomEnemyOrbIndex];

            // Shuffling to get a new enemy type.
            while (randomEnemyOrbIndex == storedEnemyIndex) {
                randomEnemyOrbIndex = CDGMath.RandomInt(0, enemyPool.Length - 1);
            }

            storedEnemyIndex = randomEnemyOrbIndex;

            // Storing white enemy type.
            var whiteEnemyType = enemyPool[randomEnemyOrbIndex];
            var whiteEnemyDifficulty = enemyDifficultyPool[randomEnemyOrbIndex];

            // Shuffling to get a new enemy type.
            while (randomEnemyOrbIndex == storedEnemyIndex) {
                randomEnemyOrbIndex = CDGMath.RandomInt(0, enemyPool.Length - 1);
            }

            storedEnemyIndex = randomEnemyOrbIndex;

            // Storing black enemy type.
            var blackEnemyType = enemyPool[randomEnemyOrbIndex];
            var blackEnemyDifficulty = enemyDifficultyPool[randomEnemyOrbIndex];

            // Shuffling to get new boss type. No need to prevent two of the same enemy types of appearing.
            randomEnemyOrbIndex = CDGMath.RandomInt(0, enemyPool.Length - 1);
            // Storing yellow enemy type.
            var yellowEnemyType = enemyPool[randomEnemyOrbIndex];
            while (yellowEnemyType == EnemyType.BOUNCY_SPIKE) // Prevent expert enemy from being bouncy spike
            {
                randomEnemyOrbIndex = CDGMath.RandomInt(0, enemyPool.Length - 1);
                yellowEnemyType = enemyPool[randomEnemyOrbIndex];
            }

            for (var i = 0; i < room.GameObjList.Count; i++) {
                // Creating a random enemy from an orb in the room.
                var enemyOrb = room.GameObjList[i] as EnemyOrbObj;
                if (enemyOrb != null) {
                    //if (CDGMath.RandomInt(1, 100) <= 100) // Currently 100% chance of spawning the enemy.
                    {
                        EnemyObj newEnemy = null;
                        if (enemyOrb.OrbType == 0) // Red orb.
                        {
                            newEnemy = EnemyBuilder.BuildEnemy(redEnemyType, null, null, null, (GameTypes.EnemyDifficulty)redEnemyDifficulty);
                        } else if (enemyOrb.OrbType == 1) // Blue orb.
                        {
                            newEnemy = EnemyBuilder.BuildEnemy(blueEnemyType, null, null, null, (GameTypes.EnemyDifficulty)blueEnemyDifficulty);
                        } else if (enemyOrb.OrbType == 2) // Green orb.
                        {
                            newEnemy = EnemyBuilder.BuildEnemy(greenEnemyType, null, null, null, (GameTypes.EnemyDifficulty)greenEnemyDifficulty);
                        } else if (enemyOrb.OrbType == 3) // White orb.
                        {
                            newEnemy = EnemyBuilder.BuildEnemy(whiteEnemyType, null, null, null, (GameTypes.EnemyDifficulty)whiteEnemyDifficulty);
                        } else if (enemyOrb.OrbType == 4) // Black orb.
                        {
                            newEnemy = EnemyBuilder.BuildEnemy(blackEnemyType, null, null, null, (GameTypes.EnemyDifficulty)blackEnemyDifficulty);
                        } else {
                            newEnemy = EnemyBuilder.BuildEnemy(yellowEnemyType, null, null, null, GameTypes.EnemyDifficulty.Expert); // In procedurallevelscreen, expert enemies will be given +10 levels.
                        }

                        // A check to ensure a forceflying orb selects a flying enemy.
                        while (enemyOrb.ForceFlying && newEnemy.IsWeighted) {
                            if (newEnemy != null) {
                                newEnemy.Dispose();
                            }

                            if (enemyOrb.OrbType == 0) // Red
                            {
                                randomEnemyOrbIndex = CDGMath.RandomInt(0, enemyPool.Length - 1);
                                redEnemyType = enemyPool[randomEnemyOrbIndex];
                                redEnemyDifficulty = enemyDifficultyPool[randomEnemyOrbIndex];
                                newEnemy = EnemyBuilder.BuildEnemy(redEnemyType, null, null, null, (GameTypes.EnemyDifficulty)redEnemyDifficulty);
                            } else if (enemyOrb.OrbType == 1) // Blue
                            {
                                randomEnemyOrbIndex = CDGMath.RandomInt(0, enemyPool.Length - 1);
                                blueEnemyType = enemyPool[randomEnemyOrbIndex];
                                blueEnemyDifficulty = enemyDifficultyPool[randomEnemyOrbIndex];
                                newEnemy = EnemyBuilder.BuildEnemy(blueEnemyType, null, null, null, (GameTypes.EnemyDifficulty)blueEnemyDifficulty);
                            } else if (enemyOrb.OrbType == 2) // Green
                            {
                                randomEnemyOrbIndex = CDGMath.RandomInt(0, enemyPool.Length - 1);
                                greenEnemyType = enemyPool[randomEnemyOrbIndex];
                                greenEnemyDifficulty = enemyDifficultyPool[randomEnemyOrbIndex];
                                newEnemy = EnemyBuilder.BuildEnemy(greenEnemyType, null, null, null, (GameTypes.EnemyDifficulty)greenEnemyDifficulty);
                            } else if (enemyOrb.OrbType == 3) // White
                            {
                                randomEnemyOrbIndex = CDGMath.RandomInt(0, enemyPool.Length - 1);
                                whiteEnemyType = enemyPool[randomEnemyOrbIndex];
                                whiteEnemyDifficulty = enemyDifficultyPool[randomEnemyOrbIndex];
                                newEnemy = EnemyBuilder.BuildEnemy(whiteEnemyType, null, null, null, (GameTypes.EnemyDifficulty)whiteEnemyDifficulty);
                            } else if (enemyOrb.OrbType == 4) // Black
                            {
                                randomEnemyOrbIndex = CDGMath.RandomInt(0, enemyPool.Length - 1);
                                blackEnemyType = enemyPool[randomEnemyOrbIndex];
                                blackEnemyDifficulty = enemyDifficultyPool[randomEnemyOrbIndex];
                                newEnemy = EnemyBuilder.BuildEnemy(blackEnemyType, null, null, null, (GameTypes.EnemyDifficulty)blackEnemyDifficulty);
                            } else // Yellow
                            {
                                randomEnemyOrbIndex = CDGMath.RandomInt(0, enemyPool.Length - 1);
                                yellowEnemyType = enemyPool[randomEnemyOrbIndex];
                                newEnemy = EnemyBuilder.BuildEnemy(yellowEnemyType, null, null, null, GameTypes.EnemyDifficulty.Expert);
                            }
                        }

                        newEnemy.Position = enemyOrb.Position;
                        newEnemy.IsProcedural = true;
                        room.EnemyList.Add(newEnemy);
                    }

                    // Remove the orb from the list.
                    room.GameObjList.Remove(enemyOrb);
                    enemyOrb.Dispose();
                    i--;
                    continue;
                }

                // Creating a random enemy for the room.
                var tag = room.GameObjList[i] as EnemyTagObj;
                if (tag != null) {
                    //if (CDGMath.RandomInt(1, 100) <= 100) // Currently 100% chance of spawning the enemy.
                    {
                        var randomEnemyIndex = CDGMath.RandomInt(0, enemyPool.Length - 1);
                        var newEnemy = EnemyBuilder.BuildEnemy(enemyPool[randomEnemyIndex], null, null, null, GameTypes.EnemyDifficulty.Basic);
                        newEnemy.Position = tag.Position;
                        newEnemy.IsProcedural = true;
                        room.EnemyList.Add(newEnemy);

                        // Changing the enemy's level based on its distance from the start of the level.
                        //float enemyDistanceFromStart = Math.Abs(CDGMath.DistanceBetweenPts(startingRoomPos, newEnemy.Position));
                        //newEnemy.Level = (int)Math.Ceiling(enemyDistanceFromStart / 1500);
                        //Console.WriteLine(newEnemy.Level);
                    }

                    // Remove the extra tag from the list.
                    room.GameObjList.Remove(tag);
                    tag.Dispose();
                    i--;
                }
            }
        }
    }

    // todo
    public static void OverrideProceduralEnemies(ProceduralLevelScreen level, byte[] enemyTypeData, byte[] enemyDifficultyData) {
        Console.WriteLine("////////////////// OVERRIDING CREATED ENEMIES. LOADING PRE-CONSTRUCTED ENEMY LIST ////////");

        //foreach (RoomObj room in level.RoomList)
        //{
        //    int count = 0;
        //    foreach (EnemyObj enemy in room.EnemyList)
        //    {
        //        if (enemy.IsProcedural == true)
        //            count++;
        //    }
        //    Console.WriteLine(count);
        //}

        var indexCounter = 0;
        // Store the enemies in a separate array.
        foreach (var room in level.RoomList) {
            //if (room.Name != "Bonus" && room.Name != "Boss") // This shouldn't be necessary since bonus room enemies and boss enemies are not (or should not be) saved.
            //if (room.Name != "Boss" && room.Name != "Bonus" && room.Name != "Ending" && room.Name != "Tutorial" && room.Name != "Compass")
            {
                for (var i = 0; i < room.EnemyList.Count; i++) {
                    var enemyToOverride = room.EnemyList[i];
                    if (enemyToOverride.IsProcedural) // Only replace procedural enemies.
                    {
                        var newEnemy = EnemyBuilder.BuildEnemy(enemyTypeData[indexCounter], level.Player, null, level, GameTypes.EnemyDifficulty.Basic, true);
                        newEnemy.IsProcedural = true;
                        newEnemy.Position = enemyToOverride.Position;

                        newEnemy.Level = enemyToOverride.Level;
                        newEnemy.SetDifficulty((GameTypes.EnemyDifficulty)enemyDifficultyData[indexCounter], false);
                        //newEnemy.SetDifficulty(enemyToOverride.Difficulty, false);

                        room.EnemyList[i].Dispose();
                        room.EnemyList[i] = newEnemy;
                        indexCounter++;
                    }
                }
            }
        }

        Console.WriteLine("//////////////// PRE-CONSTRUCTED ENEMY LIST LOADED ////////////////");
    }

    public static void AddBottomPlatforms(List<RoomObj> roomList) {
        foreach (var room in roomList) {
            foreach (var door in room.DoorList) {
                if (door.DoorPosition != "Bottom") {
                    continue;
                }

                var bottomPlatform = new TerrainObj(door.Width, door.Height);
                bottomPlatform.AddCollisionBox(0, 0, bottomPlatform.Width, bottomPlatform.Height, Consts.TERRAIN_HITBOX);
                bottomPlatform.AddCollisionBox(0, 0, bottomPlatform.Width, bottomPlatform.Height, Consts.BODY_HITBOX);
                bottomPlatform.Position = door.Position;
                bottomPlatform.CollidesBottom = false;
                bottomPlatform.CollidesLeft = false;
                bottomPlatform.CollidesRight = false;
                bottomPlatform.SetHeight(30); // Each grid in the game is 60 pixels. 60/2 = 30.
                room.TerrainObjList.Add(bottomPlatform);

                var border = new BorderObj { Position = bottomPlatform.Position };
                border.SetWidth(bottomPlatform.Width);
                border.SetHeight(bottomPlatform.Height);
                border.BorderTop = true;
                room.BorderList.Add(border);
            }
        }
    }

    public static void AddCompassRoom(List<RoomObj> roomList) {
        var compassRoom = _compassRoom.Clone() as CompassRoomObj;
        MoveRoom(compassRoom, new Vector2(-999999, -999999));
        roomList.Add(compassRoom);
    }

    public static ProceduralLevelScreen CreateEndingRoom() {
        var endingScreen = new ProceduralLevelScreen();
        var endingRoom = _endingRoom.Clone() as RoomObj;
        MoveRoom(endingRoom, Vector2.Zero);
        endingScreen.AddRoom(endingRoom);
        AddDoorBorders(endingScreen.RoomList);
        AddBottomPlatforms(endingScreen.RoomList);
        AddRemoveExtraObjects(endingScreen.RoomList);
        AddProceduralEnemies(endingScreen.RoomList);
        LinkAllBossEntrances(endingScreen.RoomList);
        ConvertBonusRooms(endingScreen.RoomList);
        ConvertBossRooms(endingScreen.RoomList);
        ConvertChallengeBossRooms(endingScreen.RoomList);
        InitializeRooms(endingScreen.RoomList);

        return endingScreen;
    }

    public static ProceduralLevelScreen CreateStartingRoom() {
        var startingRoomScreen = new ProceduralLevelScreen();
        var startingRoom = StartingRoom.Clone() as RoomObj;
        MoveRoom(startingRoom, Vector2.Zero);
        startingRoomScreen.AddRoom(startingRoom);
        AddDoorBorders(startingRoomScreen.RoomList);
        AddBottomPlatforms(startingRoomScreen.RoomList);
        AddRemoveExtraObjects(startingRoomScreen.RoomList);
        AddProceduralEnemies(startingRoomScreen.RoomList);
        LinkAllBossEntrances(startingRoomScreen.RoomList);
        ConvertBonusRooms(startingRoomScreen.RoomList);
        ConvertBossRooms(startingRoomScreen.RoomList);
        ConvertChallengeBossRooms(startingRoomScreen.RoomList);
        InitializeRooms(startingRoomScreen.RoomList);

        return startingRoomScreen;
    }

    public static ProceduralLevelScreen CreateTutorialRoom() {
        var tutorialRoomScreen = new ProceduralLevelScreen();

        // Adding intro cutscene room.
        var introRoom = new IntroRoomObj();
        introRoom.CopyRoomProperties(StartingRoom);
        introRoom.CopyRoomObjects(StartingRoom);
        MoveRoom(introRoom, Vector2.Zero);
        tutorialRoomScreen.AddRoom(introRoom);
        Game.ScreenManager.Player.Position = new Vector2(150, 150);

        // Adding tutorial room.
        var tutorialRoom = _tutorialRoom.Clone() as TutorialRoomObj;
        MoveRoom(tutorialRoom, new Vector2(introRoom.Width, -tutorialRoom!.Height + introRoom.Height));
        tutorialRoomScreen.AddRoom(tutorialRoom);

        // Adding throne room.
        var throneRoom = _throneRoom.Clone() as ThroneRoomObj;
        MoveRoom(throneRoom, new Vector2(-10000, -10000));
        tutorialRoomScreen.AddRoom(throneRoom);

        // Linking tutorial room to throne room.
        tutorialRoom.LinkedRoom = throneRoom;

        // Initializing rooms.
        AddDoorBorders(tutorialRoomScreen.RoomList);
        AddBottomPlatforms(tutorialRoomScreen.RoomList);
        AddRemoveExtraObjects(tutorialRoomScreen.RoomList);
        AddProceduralEnemies(tutorialRoomScreen.RoomList);
        LinkAllBossEntrances(tutorialRoomScreen.RoomList);
        ConvertBonusRooms(tutorialRoomScreen.RoomList);
        ConvertBossRooms(tutorialRoomScreen.RoomList);
        ConvertChallengeBossRooms(tutorialRoomScreen.RoomList);
        InitializeRooms(tutorialRoomScreen.RoomList);

        return tutorialRoomScreen;
    }

    // todo
    // Creates a level based purely on knowing the room indexes used, and the type of the room. Used for loading maps.
    public static ProceduralLevelScreen CreateLevel(Vector4[] roomInfoList, Vector3[] roomColorList) {
        Console.WriteLine(@"///////////// LOADING PRE-CONSTRUCTED LEVEL //////");
        List<RoomObj> sequencedRoomList = SequencedRoomList;
        List<RoomObj> dlcCastleRoomList = GetSequencedDLCRoomList(GameTypes.LevelType.Castle);
        List<RoomObj> dlcGardenRoomList = GetSequencedDLCRoomList(GameTypes.LevelType.Garden);
        List<RoomObj> dlcTowerRoomList = GetSequencedDLCRoomList(GameTypes.LevelType.Tower);
        List<RoomObj> dlcDungeonRoomList = GetSequencedDLCRoomList(GameTypes.LevelType.Dungeon);

        var createdLevel = new ProceduralLevelScreen();
        var roomList = new List<RoomObj>();

        var counter = 0;
        foreach (var roomData in roomInfoList) {
            //RoomObj room = sequencedRoomList[(int)roomData.W].Clone() as RoomObj;

            // New logic to load levels from the DLC lists.
            RoomObj room = null;
            var roomIndex = (int)roomData.W;
            if (roomIndex < 10000) // Take a room from the original room list.
            {
                room = sequencedRoomList[roomIndex].Clone() as RoomObj;
            } else if (roomIndex >= 10000 && roomIndex < 20000) // Take a room from the Castle DLC list.
            {
                room = dlcCastleRoomList[roomIndex - 10000].Clone() as RoomObj;
            } else if (roomIndex >= 20000 && roomIndex < 30000) // Take a room from the Garden DLC list.
            {
                room = dlcGardenRoomList[roomIndex - 20000].Clone() as RoomObj;
            } else if (roomIndex >= 30000 && roomIndex < 40000) // Take a room from the Tower DLC list.
            {
                room = dlcTowerRoomList[roomIndex - 30000].Clone() as RoomObj;
            } else // Take a room from the Dungeon DLC list.
            {
                room = dlcDungeonRoomList[roomIndex - 40000].Clone() as RoomObj;
            }

            room.LevelType = (GameTypes.LevelType)roomData.X;
            MoveRoom(room, new Vector2(roomData.Y, roomData.Z));
            roomList.Add(room);

            room.TextureColor = new Color((byte)roomColorList[counter].X, (byte)roomColorList[counter].Y, (byte)roomColorList[counter].Z);
            counter++;
        }

        createdLevel.AddRooms(roomList);

        // Linking all boss rooms.
        CloseRemainingDoors(createdLevel.RoomList);
        AddDoorBorders(createdLevel.RoomList); // Must be called after all doors are closed. Adds the borders that are created by doors existing/not existing.
        AddBottomPlatforms(createdLevel.RoomList); // Must be called after all doors are closed.
        AddRemoveExtraObjects(createdLevel.RoomList); // Adds all the Top/!Top objects. Must be called after all doors are closed, and before enemies are added.
        AddProceduralEnemies(createdLevel.RoomList);
        LinkAllBossEntrances(createdLevel.RoomList);
        ConvertBonusRooms(createdLevel.RoomList);
        ConvertBossRooms(createdLevel.RoomList);
        ConvertChallengeBossRooms(createdLevel.RoomList);
        AddCompassRoom(createdLevel.RoomList); // The last room always has to be the compass room.
        InitializeRooms(createdLevel.RoomList); // Initializes any special things for the created rooms, like special door entrances, bosses, etc.

        Console.WriteLine("///////////// PRE-CONSTRUCTED LEVEL LOADED //////");

        return createdLevel;
    }

    // todo
    public static ProceduralLevelScreen CreateLevel(RoomObj startingRoom = null, params AreaStruct[] areaStructs) {
        // DEBUG: For testing rooms only.
        if (_testRoom != null && LevelEV.RunTestRoom) {
            Console.WriteLine(@"OVERRIDING ROOM CREATION. RUNNING TEST ROOM");
            var debugLevel = new ProceduralLevelScreen();
            var debugRoom = _testRoom.Clone() as RoomObj;
            if (LevelEV.TestRoomReverse) {
                debugRoom!.Reverse();
            }

            MoveRoom(debugRoom, Vector2.Zero);
            debugLevel.AddRoom(debugRoom);
            if (LevelEV.CloseTestRoomDoors) {
                CloseRemainingDoors(debugLevel.RoomList);
            }

            AddDoorBorders(debugLevel.RoomList);
            AddBottomPlatforms(debugLevel.RoomList);
            AddRemoveExtraObjects(debugLevel.RoomList);
            AddProceduralEnemies(debugLevel.RoomList);
            LinkAllBossEntrances(debugLevel.RoomList);

            ConvertBonusRooms(debugLevel.RoomList);
            ConvertBossRooms(debugLevel.RoomList);
            ConvertChallengeBossRooms(debugLevel.RoomList);
            InitializeRooms(debugLevel.RoomList);

            // A hack for special rooms, since they're set to none level type.
            debugLevel.RoomList[0].LevelType = LevelEV.TestRoomLevelType;

            return debugLevel;
        }

        var createdLevel = new ProceduralLevelScreen(); // Create a blank level.
        var masterRoomList = new List<RoomObj>();
        var sequentialStructs = new List<AreaStruct>();
        var nonSequentialStructs = new List<AreaStruct>();

        // Separating the level structs into sequentially attached and non-sequentially attached ones.
        foreach (var areaStruct in areaStructs) {
            if (areaStruct.LevelType is GameTypes.LevelType.Castle or GameTypes.LevelType.Garden)
            {
                sequentialStructs.Add(areaStruct);
            } else {
                nonSequentialStructs.Add(areaStruct);
            }
        }

        var sequentialAreas = new List<RoomObj>[sequentialStructs.Count];
        var nonSequentialAreas = new List<RoomObj>[nonSequentialStructs.Count];

        ///////////// ROOM CREATION STARTS///////////////
    restartAll:
        Console.WriteLine(@"/// Beginning level creation...");
        masterRoomList.Clear();
        
        // Build all the sequentially attached rooms first.
        for (var i = 0; i < sequentialStructs.Count; i++) {
            var creationCounter = 0;

        restartSequential:
            sequentialAreas[i] = null;
            var areaInfo = sequentialStructs[i];
            var numRooms = CDGMath.RandomInt(areaInfo.TotalRooms.Min, areaInfo.TotalRooms.Max);

            // Keep recreating the area until the requested number of rooms are built.
            DoorObj linkerDoorAdded = null;
            var addedBossRoom = true;
            while (sequentialAreas[i] == null || sequentialAreas[i].Count < numRooms || addedBossRoom == false) {
                addedBossRoom = true;
                if (areaInfo.BossInArea) {
                    addedBossRoom = false;
                }

                // The first room is always the Starting Room.
                if (i == 0) {
                    if (startingRoom == null) {
                        sequentialAreas[i] = CreateArea(numRooms, areaInfo, masterRoomList, StartingRoom.Clone() as StartingRoomObj, true);
                    } else {
                        sequentialAreas[i] = CreateArea(numRooms, areaInfo, masterRoomList, startingRoom.Clone() as StartingRoomObj, true);
                    }
                } else {
                    var masterListCopy = new List<RoomObj>(); // Only use a copy in case rooms couldn't be found and this keeps getting added.
                    masterListCopy.AddRange(masterRoomList);

                    linkerDoorAdded = FindFurthestDoor(masterListCopy, "Right", "Right", true, LevelEV.LINK_TO_CASTLE_ONLY);
                    if (linkerDoorAdded == null) {
                        goto restartAll; // This isn't supposed to be possible.
                    }

                    sequentialAreas[i] = CreateArea(numRooms, areaInfo, _bonusDungeonRoomArray, linkerDoorAdded.Room, false);
                }

                // Checking to make sure the area has a boss entrance in it.
                if (sequentialAreas[i].Any(room => room.Name == "EntranceBoss")) {
                    addedBossRoom = true;
                }

                if (addedBossRoom == false) {
                    Console.WriteLine(@"	Could not find suitable boss room for area. Recreating sequential area.");
                } else {
                    Console.WriteLine($@"    Created sequential area of size: {sequentialAreas[i].Count}");
                }

                // A safety check. If the sequential rooms cannot be added after 15 attempts, recreate the entire map.
                creationCounter++;
                if (creationCounter > 15) {
                    Console.WriteLine(@"    Could not create non-sequential area after 15 attempts. Recreating entire level.");
                    goto restartAll;
                }
            }

            if (i != 0) {
                masterRoomList.Add(linkerDoorAdded.Room);
            }

            // Making sure each area has a top, bottom, and rightmost exit door.  If not, recreate sequential area.
            if (FindFurthestDoor(sequentialAreas[i], "Right", "Right", false, false) == null
                || FindFurthestDoor(sequentialAreas[i], "Top", "Top", false, false) == null
                || FindFurthestDoor(sequentialAreas[i], "Bottom", "Bottom", false, false) == null) {
                var removedLinkedRoom = false;
                if (i != 0) {
                    removedLinkedRoom = masterRoomList.Remove(linkerDoorAdded.Room);
                } else {
                    removedLinkedRoom = true;
                }

                Console.WriteLine("Attempting re-creation of sequential area. Linker Room removed: " + removedLinkedRoom);
                goto restartSequential; // Didn't want to use gotos, but only way I could think of restarting loop.  Goes back to the start of this forloop.
            }

            // A sequential area with all exits has been created. Add it to the masterRoomList;
            masterRoomList.AddRange(sequentialAreas[i]);
        }

        Console.WriteLine("////////// ALL SEQUENTIAL AREAS SUCCESSFULLY ADDED");

        // Now create all non-sequential areas.
        for (var i = 0; i < nonSequentialStructs.Count; i++) {
            var creationCounter = 0;
            restartNonSequential:
            nonSequentialAreas[i] = null;
            var areaInfo = nonSequentialStructs[i];
            var numRooms = CDGMath.RandomInt(areaInfo.TotalRooms.Min, areaInfo.TotalRooms.Max);
            var furthestDoorDirection = "";
            switch (areaInfo.LevelType) {
                case GameTypes.LevelType.Tower:
                    furthestDoorDirection = "Top";
                    break;

                case GameTypes.LevelType.Dungeon:
                    furthestDoorDirection = "Bottom"; //"Bottom";
                    break;

                case GameTypes.LevelType.Garden:
                    furthestDoorDirection = "Right"; //TEDDY - COMMENTED OUT DUNGEON CONNECT TOP, AND ADDED GARDEN CONNECT TOP
                    break;

                default:
                    throw new Exception("Could not create non-sequential area of type " + areaInfo.LevelType);
            }

            //int creationCounter = 0;
            DoorObj linkerDoorAdded = null;
            var addedBossRoom = true;
            // Keep recreating the area until the requested number of rooms are built.
            while (nonSequentialAreas[i] == null || nonSequentialAreas[i].Count < numRooms || addedBossRoom == false) {
                addedBossRoom = true;
                if (areaInfo.BossInArea) {
                    addedBossRoom = false;
                }

                var masterListCopy = new List<RoomObj>(); // Only use a copy in case rooms couldn't be found and this keeps getting added.
                masterListCopy.AddRange(masterRoomList);

                // Finds the furthest door of the previous area, and adds a linker room to it (on a separate list so as not to modify the original room list).
                // Creates a new area from the linker room.
                linkerDoorAdded = FindFurthestDoor(masterListCopy, furthestDoorDirection, furthestDoorDirection, true, LevelEV.LINK_TO_CASTLE_ONLY);
                if (linkerDoorAdded == null) {
                    goto restartAll; // This isn't supposed to be possible.
                }

                nonSequentialAreas[i] = CreateArea(numRooms, areaInfo, masterListCopy, linkerDoorAdded.Room, false);

                // A safety check. If the non-sequential rooms cannot be added after 15 attempts, recreate the entire map.
                creationCounter++;
                if (creationCounter > 15) {
                    Console.WriteLine("Could not create non-sequential area after 15 attempts. Recreating entire level.");
                    goto restartAll;
                }

                // Checking to make sure the area has a boss entrance in it.
                foreach (var room in nonSequentialAreas[i]) {
                    if (room.Name == "EntranceBoss") {
                        addedBossRoom = true;
                        break;
                    }
                }

                if (addedBossRoom == false) {
                    Console.WriteLine("Could not find suitable boss room for area. Recreating non-sequential area.");
                } else {
                    Console.WriteLine("Created non-sequential area of size: " + nonSequentialAreas[i].Count);
                }
            }

            masterRoomList.Add(linkerDoorAdded.Room);

            // Making sure each area has a top, bottom, and rightmost exit door.  If not, recreate all sequential areas.
            if ((areaInfo.LevelType == GameTypes.LevelType.Tower && (FindFurthestDoor(nonSequentialAreas[i], "Right", "Right", false, false) == null ||
                                                                     FindFurthestDoor(nonSequentialAreas[i], "Top", "Top", false, false) == null)) ||
                (areaInfo.LevelType == GameTypes.LevelType.Dungeon && (FindFurthestDoor(nonSequentialAreas[i], "Right", "Right", false, false) == null ||
                                                                       FindFurthestDoor(nonSequentialAreas[i], "Bottom", "Bottom", false, false) == null))) {
                var removedLinkedRoom = false;
                removedLinkedRoom = masterRoomList.Remove(linkerDoorAdded.Room);
                Console.WriteLine("Attempting re-creation of a non-sequential area. Linker Room removed: " + removedLinkedRoom);
                goto restartNonSequential;
            }

            // A non-sequential area with all exits has been created. Add it to the masterRoomList;
            masterRoomList.AddRange(nonSequentialAreas[i]);
        }

        Console.WriteLine("////////// ALL NON-SEQUENTIAL AREAS SUCCESSFULLY ADDED");

        createdLevel.AddRooms(masterRoomList); // Add the rooms to the level.
        CloseRemainingDoors(createdLevel.RoomList);
        AddDoorBorders(createdLevel.RoomList); // Must be called after all doors are closed. Adds the borders that are created by doors existing/not existing.
        AddBottomPlatforms(createdLevel.RoomList); // Must be called after all doors are closed.
        AddRemoveExtraObjects(createdLevel.RoomList); // Adds all the Top/!Top objects. Must be called after all doors are closed, and before enemies are added.
        AddProceduralEnemies(createdLevel.RoomList);
        LinkAllBossEntrances(createdLevel.RoomList); // Links all boss entrances to actual boss rooms.
        ConvertBonusRooms(createdLevel.RoomList);
        ConvertBossRooms(createdLevel.RoomList);
        ConvertChallengeBossRooms(createdLevel.RoomList);
        AddCompassRoom(createdLevel.RoomList); // The last room always has to be the compass room.
        InitializeRooms(createdLevel.RoomList); // Initializes any special things for the created rooms, like special door entrances, bosses, etc.

        Console.WriteLine("////////// LEVEL CREATION SUCCESSFUL");

        return createdLevel;
    }

    private static void ConvertBonusRooms(List<RoomObj> roomList) {
        // flibit didn't like this
        // CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        // ci.NumberFormat.CurrencyDecimalSeparator = ".";
        var ci = CultureInfo.InvariantCulture;

        for (var i = 0; i < roomList.Count; i++) {
            var room = roomList[i];
            if (room.Name != "Bonus") {
                continue;
            }

            if (room.Tag == "") {
                room.Tag = "0";
            }

            RoomObj roomToAdd = byte.Parse(room.Tag, NumberStyles.Any, ci) switch {
                BonusRoomType.PICK_CHEST      => new ChestBonusRoomObj(),
                BonusRoomType.SPECIAL_ITEM    => new SpecialItemRoomObj(),
                BonusRoomType.RANDOM_TELEPORT => new RandomTeleportRoomObj(),
                BonusRoomType.SPELL_SWAP      => new SpellSwapRoomObj(),
                BonusRoomType.VITA_CHAMBER    => new VitaChamberRoomObj(),
                BonusRoomType.DIARY           => new DiaryRoomObj(),
                BonusRoomType.PORTRAIT        => new PortraitRoomObj(),
                BonusRoomType.CARNIVAL_SHOOT1 => new CarnivalShoot1BonusRoom(),
                BonusRoomType.CARNIVAL_SHOOT2 => new CarnivalShoot2BonusRoom(),
                BonusRoomType.ARENA           => new ArenaBonusRoom(),
                BonusRoomType.JUKEBOX         => new JukeboxBonusRoom(),
                _                             => null,
            };

            if (roomToAdd == null) {
                continue;
            }

            roomToAdd.CopyRoomProperties(room);
            roomToAdd.CopyRoomObjects(room);
            roomList.Insert(roomList.IndexOf(room), roomToAdd);
            roomList.Remove(room);
        }
    }

    private static void ConvertBossRooms(List<RoomObj> roomList) {
        // flibit didn't like this
        // CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        // ci.NumberFormat.CurrencyDecimalSeparator = ".";
        var ci = CultureInfo.InvariantCulture;

        for (var i = 0; i < roomList.Count; i++) {
            var room = roomList[i];
            if (room.Name != "Boss") {
                continue;
            }

            if (room.Tag == "") {
                room.Tag = "0";
            }

            var bossRoomType = int.Parse(room.Tag, NumberStyles.Any, ci);
            RoomObj bossRoom = bossRoomType switch {
                BossRoomType.EYEBALL_BOSS_ROOM  => new EyeballBossRoom(),
                BossRoomType.LAST_BOSS_ROOM     => new LastBossRoom(),
                BossRoomType.BLOB_BOSS_ROOM     => new BlobBossRoom(),
                BossRoomType.FAIRY_BOSS_ROOM    => new FairyBossRoom(),
                BossRoomType.FIREBALL_BOSS_ROOM => new FireballBossRoom(),
                _                               => null,
            };

            if (bossRoom == null) {
                continue;
            }

            bossRoom.CopyRoomProperties(room);
            bossRoom.CopyRoomObjects(room);
            if (bossRoom.LinkedRoom != null) // Adding this so you can test the room without linking it.
            {
                bossRoom.LinkedRoom = room.LinkedRoom;

                // A roundabout way of relinking the boss entrance room to the newly made eyeball boss.
                bossRoom.LinkedRoom.LinkedRoom = bossRoom;
            }

            roomList.Insert(roomList.IndexOf(room), bossRoom);
            roomList.Remove(room);
        }
    }

    private static void ConvertChallengeBossRooms(List<RoomObj> roomList) {
        // flibit didn't like this
        // CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
        // ci.NumberFormat.CurrencyDecimalSeparator = ".";
        var ci = CultureInfo.InvariantCulture;

        for (var i = 0; i < roomList.Count; i++) {
            var room = roomList[i];
            if (room.Name != "ChallengeBoss") {
                continue;
            }

            if (room.Tag == "") {
                room.Tag = "0";
            }

            RoomObj challengeRoom;
            var challengeRoomType = int.Parse(room.Tag, NumberStyles.Any, ci);

            switch (challengeRoomType) {
                case BossRoomType.EYEBALL_BOSS_ROOM:
                    challengeRoom = new EyeballChallengeRoom();
                    MoveRoom(challengeRoom, new Vector2(0, 5000000));
                    break;

                case BossRoomType.FAIRY_BOSS_ROOM:
                    challengeRoom = new FairyChallengeRoom();
                    MoveRoom(challengeRoom, new Vector2(0, -6000000));
                    break;

                case BossRoomType.FIREBALL_BOSS_ROOM:
                    challengeRoom = new FireballChallengeRoom();
                    MoveRoom(challengeRoom, new Vector2(0, -7000000));
                    break;

                case BossRoomType.BLOB_BOSS_ROOM:
                    challengeRoom = new BlobChallengeRoom();
                    MoveRoom(challengeRoom, new Vector2(0, -8000000));
                    break;

                case BossRoomType.LAST_BOSS_ROOM:
                    challengeRoom = new LastBossChallengeRoom();
                    MoveRoom(challengeRoom, new Vector2(0, -9000000));
                    break;

                default:
                    challengeRoom = new EyeballChallengeRoom();
                    MoveRoom(challengeRoom, new Vector2(0, -5000000));
                    break;
            }

            var storedPos = challengeRoom.Position;
            challengeRoom.CopyRoomProperties(room);
            challengeRoom.CopyRoomObjects(room);
            MoveRoom(challengeRoom, storedPos);
            if (challengeRoom.LinkedRoom != null) // Adding this so you can test the room without linking it.
            {
                challengeRoom.LinkedRoom = room.LinkedRoom;
            }

            roomList.Insert(roomList.IndexOf(room), challengeRoom);
            roomList.Remove(room);
        }
    }

    private static void InitializeRooms(List<RoomObj> roomList) {
        foreach (var room in roomList) {
            room.Initialize();
        }
    }

    private static string GetOppositeDoorPosition(string doorPosition) {
        return doorPosition switch {
            "Left"   => "Right",
            "Right"  => "Left",
            "Top"    => "Bottom",
            "Bottom" => "Top",
            _        => "",
        };
    }

    private static bool CheckForRoomCollision(DoorObj doorToCheck, List<RoomObj> roomList, DoorObj otherDoorToCheck) {
        //Code to make sure the room does not collide with other rooms in the list.
        var newRoomPosition = doorToCheck.DoorPosition switch {
            "Left"   => new Vector2(doorToCheck.Room.X - otherDoorToCheck.Room.Width, doorToCheck.Y - (otherDoorToCheck.Y - otherDoorToCheck.Room.Y)),
            "Right"  => new Vector2(doorToCheck.X + doorToCheck.Width, doorToCheck.Y - (otherDoorToCheck.Y - otherDoorToCheck.Room.Y)),
            "Top"    => new Vector2(doorToCheck.X - (otherDoorToCheck.X - otherDoorToCheck.Room.X), doorToCheck.Y - otherDoorToCheck.Room.Height),
            "Bottom" => new Vector2(doorToCheck.X - (otherDoorToCheck.X - otherDoorToCheck.Room.X), doorToCheck.Y + doorToCheck.Height),
            _        => Vector2.Zero,
        };

        // Do not allow rooms be made past xPos = 0.
        return newRoomPosition.X < 0 || roomList.Any(roomObj => CollisionMath.Intersects(
            new Rectangle((int)roomObj.X, (int)roomObj.Y, roomObj.Width, roomObj.Height),
            new Rectangle((int)newRoomPosition.X, (int)newRoomPosition.Y, otherDoorToCheck.Room.Width, otherDoorToCheck.Room.Height)
        ));
    }

    public static void MoveRoom(RoomObj room, Vector2 newPosition) {
        // The amount everything in the room needs to shift by in order to put them in their new position next to the room they are linking to.
        var positionShift = room.Position - newPosition; 
        
        //Shifting the room to link to and its contents next to the room that needs linking.
        room.Position = newPosition;
        foreach (var obj in room.TerrainObjList) {
            obj.Position -= positionShift;
        }

        foreach (var obj in room.GameObjList) {
            obj.Position -= positionShift;
        }

        foreach (var door in room.DoorList) {
            door.Position -= positionShift;
        }

        foreach (var enemy in room.EnemyList) {
            enemy.Position -= positionShift;
        }

        foreach (var border in room.BorderList) {
            border.Position -= positionShift;
        }
    }

    public static void LinkAllBossEntrances(List<RoomObj> roomList) {
        // This is where all the boss rooms will float in. It must be left of the level so that it doesn't accidentally run into any of the level's rooms.
        var newRoomPosition = new Vector2(-100000, 0);
        var maxRoomIndex = _bossRoomArray.Count - 1;

        var bossRoomsToAdd = new List<RoomObj>();

        var challengeRoomsToAdd = new List<RoomObj>();
        RoomObj challengeRoom;

        foreach (var room in roomList) {
            byte bossRoomType = 0;
            switch (room.LevelType) {
                case GameTypes.LevelType.Castle:
                    bossRoomType = LevelEV.CASTLE_BOSS_ROOM;
                    break;

                case GameTypes.LevelType.Tower:
                    bossRoomType = LevelEV.TOWER_BOSS_ROOM;
                    break;

                case GameTypes.LevelType.Dungeon:
                    bossRoomType = LevelEV.DUNGEON_BOSS_ROOM;
                    break;

                case GameTypes.LevelType.Garden:
                    bossRoomType = LevelEV.GARDEN_BOSS_ROOM;
                    break;
            }

            RoomObj bossRoom;
            switch (room.Name) {
                case "EntranceBoss": {
                    bossRoom = GetSpecificBossRoom(bossRoomType);
                    bossRoom = bossRoom?.Clone() as RoomObj ?? GetBossRoom(CDGMath.RandomInt(0, maxRoomIndex)).Clone() as RoomObj;
                    bossRoom!.LevelType = room.LevelType;
                    MoveRoom(bossRoom, newRoomPosition);
                    newRoomPosition.X += bossRoom.Width;
                    room.LinkedRoom = bossRoom;
                    bossRoom.LinkedRoom = room;
                    bossRoomsToAdd.Add(bossRoom);

                    // Now linking challenge boss rooms
                    challengeRoom = GetChallengeRoom(bossRoomType);
                    if (challengeRoom != null) {
                        challengeRoom = challengeRoom.Clone() as RoomObj;
                        challengeRoom!.LevelType = room.LevelType;
                        challengeRoom.LinkedRoom = room;
                        challengeRoomsToAdd.Add(challengeRoom);
                    }

                    break;
                }

                // Creating the special Last boss room that links to tutorial room.
                case "CastleEntrance": {
                    // Creating tutorial room and boss room.
                    var tutorialRoom = _tutorialRoom.Clone() as TutorialRoomObj;
                    bossRoom = GetSpecificBossRoom(BossRoomType.LAST_BOSS_ROOM).Clone() as RoomObj;

                    // Moving tutorial room and boss room to proper positions.
                    // Special positioning for the last boss room. Necessary since CastleEntranceRoomObj blocks you from going past 0.
                    MoveRoom(tutorialRoom, new Vector2(100000, -100000));
                    MoveRoom(bossRoom, new Vector2(150000, -100000));

                    // Linking castle entrance to tutorial room.
                    room.LinkedRoom = tutorialRoom;

                    // Linking tutorial room to boss room.
                    tutorialRoom!.LinkedRoom = bossRoom;
                    bossRoom!.LinkedRoom = tutorialRoom;
                    bossRoomsToAdd.Add(bossRoom);
                    bossRoomsToAdd.Add(tutorialRoom);

                    break;
                }
            }
        }

        // Adding the Last boss challenge room.
        challengeRoom = GetChallengeRoom(BossRoomType.LAST_BOSS_ROOM);
        if (challengeRoom != null) {
            challengeRoom = challengeRoom.Clone() as RoomObj;
            challengeRoom!.LevelType = GameTypes.LevelType.Castle;
            challengeRoom.LinkedRoom = null;

            challengeRoomsToAdd.Add(challengeRoom);
        }

        roomList.AddRange(bossRoomsToAdd);
        roomList.AddRange(challengeRoomsToAdd);
    }

    public static List<RoomObj> GetRoomList(int roomWidth, int roomHeight, GameTypes.LevelType levelType) {
        return GetLevelTypeRoomArray(levelType)[roomWidth - 1, roomHeight - 1];
    }

    public static RoomObj GetBossRoom(int index) {
        return _bossRoomArray[index];
    }

    public static RoomObj GetSpecificBossRoom(byte bossRoomType) {
        return _bossRoomArray.FirstOrDefault(room => room.Tag != "" && byte.Parse(room.Tag) == bossRoomType);
    }

    public static RoomObj GetChallengeRoom(byte bossRoomType) {
        return _challengeRoomArray.FirstOrDefault(room => room.Tag != "" && byte.Parse(room.Tag) == bossRoomType);
    }

    public static RoomObj GetChallengeBossRoomFromRoomList(GameTypes.LevelType levelType, List<RoomObj> roomList) {
        return roomList.Where(room => room.Name == "ChallengeBoss").FirstOrDefault(room => room.LevelType == levelType);
    }

    public static List<RoomObj>[,] GetLevelTypeRoomArray(GameTypes.LevelType levelType) {
        switch (levelType) {
            default:
            case GameTypes.LevelType.None:
                throw new Exception("Cannot create level of type NONE");

            case GameTypes.LevelType.Castle:
                return CastleRoomArray;

            case GameTypes.LevelType.Garden:
                return GardenRoomArray;

            case GameTypes.LevelType.Tower:
                return TowerRoomArray;

            case GameTypes.LevelType.Dungeon:
                return DungeonRoomArray;
        }
    }

    public static void IndexRoomList() {
        var index = 0;
        foreach (var room in SequencedRoomList) {
            room.PoolIndex = index;
            index++;
        }

        // Storing DLC maps. For easy differentiating (for extensibility in the future):
        // 10000 is for Castle areas.
        // 20000 is for Garden areas.
        // 30000 is for Tower areas.
        // 40000 is for Dungeon areas.

        index = 10000;
        List<RoomObj> roomList = GetSequencedDLCRoomList(GameTypes.LevelType.Castle);
        foreach (var room in roomList) {
            room.PoolIndex = index;
            index++;
        }

        index = 20000;
        roomList = GetSequencedDLCRoomList(GameTypes.LevelType.Garden);
        foreach (var room in roomList) {
            room.PoolIndex = index;
            index++;
        }

        index = 30000;
        roomList = GetSequencedDLCRoomList(GameTypes.LevelType.Tower);
        foreach (var room in roomList) {
            room.PoolIndex = index;
            index++;
        }

        index = 40000;
        roomList = GetSequencedDLCRoomList(GameTypes.LevelType.Dungeon);
        foreach (var room in roomList) {
            room.PoolIndex = index;
            index++;
        }
    }

    // Only for DLC rooms.
    // The logic is different from Non-DLC rooms to allow for easily adding more rooms in the future.
    public static List<RoomObj> GetSequencedDLCRoomList(GameTypes.LevelType levelType) {
        return levelType switch {
            GameTypes.LevelType.Castle  => _dlcCastleRoomArray,
            GameTypes.LevelType.Dungeon => _dlcDungeonRoomArray,
            GameTypes.LevelType.Garden  => _dlcGardenRoomArray,
            GameTypes.LevelType.Tower   => _dlcTowerRoomArray,
            _                           => null,
        };
    }
}

public struct AreaStruct {
    public string Name;
    public GameTypes.LevelType LevelType;
    public Vector2 EnemyLevel;
    public (int Min, int Max) TotalRooms;
    public (int Min, int Max) BonusRooms;
    public (int Min, int Max) SecretRooms;
    public int BossLevel;
    public int EnemyLevelScale;
    public bool BossInArea;
    public bool IsFinalArea;
    public Color Color;
    public Color MapColor;
    public bool LinkToCastleOnly;
    public byte BossType;
}
