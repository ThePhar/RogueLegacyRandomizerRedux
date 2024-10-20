using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using RogueCastle.EnvironmentVariables;
using RogueCastle.GameStructs;
using RogueCastle.Objects;
using RogueCastle.Randomizer;
using Tweener;

namespace RogueCastle.Managers;

public class SaveGameManager(Game game)
{
    private const string FileNameLineage = "RLRandomizerLineage.rcdat";
    private const string FileNameMap = "RLRandomizerMap.rcdat";
    private const string FileNameMapData = "RLRandomizerMapData.rcdat";
    private const string FileNamePlayer = "RLRandomizerPlayer.rcdat";
    private const string FileNameUpgrades = "RLRandomizerUpgrades.rcdat";
    private const string FileNameArchipelago = "RLRandomizerMultiworld.rcdat";
    private const string StorageContainerName = "RandomizerStorageContainer";

    private bool _autosaveLoaded;
    private int _saveFailCounter;
    private StorageContainer _storageContainer;

    public void Initialize()
    {
        if (_storageContainer != null)
        {
            _storageContainer.Dispose();
            _storageContainer = null;
        }

        PerformDirectoryCheck();
    }

    public void SaveFiles(params SaveType[] saveList)
    {
        // no saving if disabled (or slot 0).
        if (LevelEV.DisableSaving || Game.GameConfig.ProfileSlot == 0)
        {
            return;
        }

        GetStorageContainer();
        try
        {
            foreach (var saveType in saveList)
            {
                SaveData(saveType, false);
            }

            _saveFailCounter = 0;
        }
        catch
        {
            if (_saveFailCounter > 2)
            {
                var manager = Game.ScreenManager;
                manager.DialogueScreen.SetDialogue("Save File Error Antivirus");
                Tween.RunFunction(0.25f, manager, "DisplayScreen", ScreenType.DIALOGUE, true, typeof(List<object>));
                _saveFailCounter = 0;
            }
            else
            {
                _saveFailCounter++;
            }
        }
        finally
        {
            if (_storageContainer is { IsDisposed: false })
            {
                _storageContainer.Dispose();
                _storageContainer = null;
            }
        }
    }

    public void SaveBackupFiles(params SaveType[] saveList)
    {
        if (LevelEV.DisableSaving)
        {
            return;
        }

        GetStorageContainer();
        foreach (var saveType in saveList)
        {
            SaveData(saveType, true);
        }

        _storageContainer.Dispose();
        _storageContainer = null;
    }

    public void SaveAllFileTypes(bool saveBackup)
    {
        if (saveBackup == false)
        {
            SaveFiles(SaveType.PlayerData, SaveType.UpgradeData, SaveType.Map, SaveType.MapData, SaveType.Lineage);
        }
        else
        {
            SaveBackupFiles(SaveType.PlayerData, SaveType.UpgradeData, SaveType.Map, SaveType.MapData, SaveType.Lineage, SaveType.Archipelago);
        }
    }

    public void LoadFiles(ProceduralLevelScreen level, params SaveType[] loadList)
    {
        if (LevelEV.DisableSaving)
        {
            return;
        }

        if (!LevelEV.EnableBackupSaving)
        {
            GetStorageContainer();
            foreach (var loadType in loadList)
            {
                LoadData(loadType, level);
            }

            _storageContainer.Dispose();
            _storageContainer = null;
            return;
        }

        GetStorageContainer();

        var currentType = SaveType.None;
        try
        {
            foreach (var loadType in loadList)
            {
                currentType = loadType;
                LoadData(loadType, level);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($@"Save File Error: {e.Message}");
            // Only perform autosave loading if you're not loading the map.  This is because the map is loaded on a separate thread, so it needs to
            // manually call ForceBackup() once the thread has exited.
            if (currentType != SaveType.Map && currentType != SaveType.MapData && currentType != SaveType.None)
            {
                if (_autosaveLoaded == false)
                {
                    Game.ScreenManager.DialogueScreen.SetDialogue("Save File Error");
                    Game.GameIsCorrupt = true;
                    Game.ScreenManager.DialogueScreen.SetConfirmEndHandler(this, "LoadAutosave");
                }
                else
                {
                    _autosaveLoaded = false;
                    Game.ScreenManager.DialogueScreen.SetDialogue("Save File Error 2");
                    Game.GameIsCorrupt = true;
                    Game.ScreenManager.DialogueScreen.SetConfirmEndHandler(game, "Exit");
                }

                Game.ScreenManager.DisplayScreen(ScreenType.DIALOGUE, false);

                // Just a small trigger to make sure the game knows the file is corrupt, and to stop doing whatever it's doing.
                Game.PlayerStats.HeadPiece = 0;
            }
            else
            {
                // This triggers the try/catch block in the loading screen.
                throw new Exception();
            }
        }
        finally
        {
            if (_storageContainer is { IsDisposed: false })
            {
                _storageContainer.Dispose();
            }
        }
    }

    public void ForceBackup()
    {
        if (_storageContainer is { IsDisposed: false })
        {
            _storageContainer.Dispose();
        }

        Game.ScreenManager.DialogueScreen.SetDialogue("Save File Error");
        Game.ScreenManager.DialogueScreen.SetConfirmEndHandler(this, "LoadAutosave");
        Game.ScreenManager.DisplayScreen(ScreenType.DIALOGUE, false);
    }

    public void LoadAutosave()
    {
        Console.WriteLine(@"Save file corrupted");
        SkillSystem.ResetAllTraits();
        Game.PlayerStats.Dispose();
        Game.PlayerStats = new PlayerStats();
        Game.ScreenManager.Player.Reset();
        LoadBackups();
        Game.ScreenManager.DisplayScreen(ScreenType.TITLE, true);
    }

    public void StartNewGame()
    {
        ClearAllFileTypes(false);
        ClearAllFileTypes(true);
        SkillSystem.ResetAllTraits();
        Game.PlayerStats.Dispose();
        Game.PlayerStats = new PlayerStats();
        Game.ScreenManager.Player.Reset();
        Game.ScreenManager.DisplayScreen(ScreenType.TUTORIAL_ROOM, true);
    }

    public void ResetAutosave()
    {
        _autosaveLoaded = false;
    }

    public void LoadAllFileTypes(ProceduralLevelScreen level)
    {
        LoadFiles(level, SaveType.PlayerData, SaveType.UpgradeData, SaveType.Map, SaveType.MapData, SaveType.Lineage, SaveType.Archipelago);
    }

    public void ClearFiles(params SaveType[] deleteList)
    {
        GetStorageContainer();
        foreach (var deleteType in deleteList)
        {
            DeleteData(deleteType);
        }

        _storageContainer.Dispose();
        _storageContainer = null;
    }

    public void ClearBackupFiles(params SaveType[] deleteList)
    {
        GetStorageContainer();
        foreach (var deleteType in deleteList)
        {
            DeleteBackupData(deleteType);
        }

        _storageContainer.Dispose();
        _storageContainer = null;
    }

    public void ClearAllFileTypes(bool deleteBackups)
    {
        if (deleteBackups == false)
        {
            ClearFiles(SaveType.PlayerData, SaveType.UpgradeData, SaveType.Map, SaveType.MapData, SaveType.Lineage, SaveType.Archipelago);
        }
        else
        {
            ClearBackupFiles(SaveType.PlayerData, SaveType.UpgradeData, SaveType.Map, SaveType.MapData, SaveType.Lineage, SaveType.Archipelago);
        }
    }

    private void DeleteData(SaveType deleteType)
    {
        switch (deleteType)
        {
            case SaveType.PlayerData:
                if (_storageContainer.FileExists($"Profile{Game.GameConfig.ProfileSlot}/{FileNamePlayer}"))
                {
                    _storageContainer.DeleteFile($"Profile{Game.GameConfig.ProfileSlot}/{FileNamePlayer}");
                }

                break;
            case SaveType.UpgradeData:
                if (_storageContainer.FileExists($"Profile{Game.GameConfig.ProfileSlot}/{FileNameUpgrades}"))
                {
                    _storageContainer.DeleteFile($"Profile{Game.GameConfig.ProfileSlot}/{FileNameUpgrades}");
                }

                break;
            case SaveType.Map:
                if (_storageContainer.FileExists($"Profile{Game.GameConfig.ProfileSlot}/{FileNameMap}"))
                {
                    _storageContainer.DeleteFile($"Profile{Game.GameConfig.ProfileSlot}/{FileNameMap}");
                }

                break;
            case SaveType.MapData:
                if (_storageContainer.FileExists($"Profile{Game.GameConfig.ProfileSlot}/{FileNameMapData}"))
                {
                    _storageContainer.DeleteFile($"Profile{Game.GameConfig.ProfileSlot}/{FileNameMapData}");
                }

                break;
            case SaveType.Lineage:
                if (_storageContainer.FileExists($"Profile{Game.GameConfig.ProfileSlot}/{FileNameLineage}"))
                {
                    _storageContainer.DeleteFile($"Profile{Game.GameConfig.ProfileSlot}/{FileNameLineage}");
                }

                break;
            case SaveType.Archipelago:
                if (_storageContainer.FileExists($"Profile{Game.GameConfig.ProfileSlot}/{FileNameArchipelago}"))
                {
                    _storageContainer.DeleteFile($"Profile{Game.GameConfig.ProfileSlot}/{FileNameLineage}");
                }

                break;
        }

        Console.WriteLine($@"Save file type {deleteType} deleted.");
    }

    private void DeleteBackupData(SaveType deleteType)
    {
        switch (deleteType)
        {
            case SaveType.PlayerData:
                if (_storageContainer.FileExists($"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNamePlayer}"))
                {
                    _storageContainer.DeleteFile($"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNamePlayer}");
                }

                break;
            case SaveType.UpgradeData:
                if (_storageContainer.FileExists($"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNameUpgrades}"))
                {
                    _storageContainer.DeleteFile($"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNameUpgrades}");
                }

                break;
            case SaveType.Map:
                if (_storageContainer.FileExists($"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNameMap}"))
                {
                    _storageContainer.DeleteFile($"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNameMap}");
                }

                break;
            case SaveType.MapData:
                if (_storageContainer.FileExists($"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNameMapData}"))
                {
                    _storageContainer.DeleteFile($"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNameMapData}");
                }

                break;
            case SaveType.Lineage:
                if (_storageContainer.FileExists($"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNameLineage}"))
                {
                    _storageContainer.DeleteFile($"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNameLineage}");
                }

                break;
            case SaveType.Archipelago:
                if (_storageContainer.FileExists($"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNameArchipelago}"))
                {
                    _storageContainer.DeleteFile($"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNameArchipelago}");
                }

                break;
        }

        Console.WriteLine($@"Backup save file type {deleteType} deleted.");
    }

    private void LoadBackups()
    {
        Console.WriteLine(@"Replacing save file with back up saves");
        GetStorageContainer();

        // Player Data
        if (
            _storageContainer.FileExists($"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNamePlayer}") &&
            _storageContainer.FileExists($"Profile{Game.GameConfig.ProfileSlot}/{FileNamePlayer}")
        )
        {
            var fileToCopy = _storageContainer.OpenFile(
                $"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNamePlayer}",
                FileMode.Open, FileAccess.Read, FileShare.Read); // Open the backup

            // Copy the backup prior to corruption revert. This is in case the loading didn't work, and all data is
            // lost (since backups get overwritten once people start playing again).
            var backupCopy = _storageContainer.CreateFile(
                $"Profile{Game.GameConfig.ProfileSlot}/AutoSaveBACKUP_{FileNamePlayer}");
            fileToCopy.CopyTo(backupCopy);
            backupCopy.Close();
            fileToCopy.Close();

            fileToCopy = _storageContainer.OpenFile(
                $"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNamePlayer}",
                FileMode.Open, FileAccess.Read, FileShare.Read); // Open the backup
            var fileToOverride = _storageContainer.CreateFile($"Profile{Game.GameConfig.ProfileSlot}/{FileNamePlayer}");
            fileToCopy.CopyTo(fileToOverride);
            fileToCopy.Close();
            fileToOverride.Close();
        }

        // Upgrade Data
        if (
            _storageContainer.FileExists($"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNameUpgrades}") &&
            _storageContainer.FileExists($"Profile{Game.GameConfig.ProfileSlot}/{FileNameUpgrades}")
        )
        {
            var fileToCopy = _storageContainer.OpenFile(
                $"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNameUpgrades}",
                FileMode.Open, FileAccess.Read, FileShare.Read); // Open the backup

            // Copy the backup prior to corruption revert. This is in case the loading didn't work, and all data is
            // lost (since backups get overwritten once people start playing again).
            var backupCopy = _storageContainer.CreateFile(
                $"Profile{Game.GameConfig.ProfileSlot}/AutoSaveBACKUP_{FileNameUpgrades}");
            fileToCopy.CopyTo(backupCopy);
            backupCopy.Close();
            fileToCopy.Close();

            fileToCopy = _storageContainer.OpenFile(
                $"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNameUpgrades}",
                FileMode.Open, FileAccess.Read, FileShare.Read); // Open the backup
            var fileToOverride = _storageContainer.CreateFile($"Profile{Game.GameConfig.ProfileSlot}/{FileNameUpgrades}");
            fileToCopy.CopyTo(fileToOverride);
            fileToCopy.Close();
            fileToOverride.Close();
        }

        // Map Data
        if (
            _storageContainer.FileExists($"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNameMap}") &&
            _storageContainer.FileExists($"Profile{Game.GameConfig.ProfileSlot}/{FileNameMap}")
        )
        {
            var fileToCopy = _storageContainer.OpenFile(
                $"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNameMap}",
                FileMode.Open, FileAccess.Read, FileShare.Read); // Open the backup

            // Copy the backup prior to corruption revert. This is in case the loading didn't work, and all data is
            // lost (since backups get overwritten once people start playing again).
            var backupCopy = _storageContainer.CreateFile(
                $"Profile{Game.GameConfig.ProfileSlot}/AutoSaveBACKUP_{FileNameMap}");
            fileToCopy.CopyTo(backupCopy);
            backupCopy.Close();
            fileToCopy.Close();

            fileToCopy = _storageContainer.OpenFile(
                $"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNameMap}",
                FileMode.Open, FileAccess.Read, FileShare.Read); // Open the backup
            var fileToOverride = _storageContainer.CreateFile($"Profile{Game.GameConfig.ProfileSlot}/{FileNameMap}");
            fileToCopy.CopyTo(fileToOverride);
            fileToCopy.Close();
            fileToOverride.Close();
        }

        // Map Data... Data
        if (
            _storageContainer.FileExists($"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNameMapData}") &&
            _storageContainer.FileExists($"Profile{Game.GameConfig.ProfileSlot}/{FileNameMapData}")
        )
        {
            var fileToCopy = _storageContainer.OpenFile(
                $"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNameMapData}",
                FileMode.Open, FileAccess.Read, FileShare.Read); // Open the backup

            // Copy the backup prior to corruption revert. This is in case the loading didn't work, and all data is
            // lost (since backups get overwritten once people start playing again).
            var backupCopy = _storageContainer.CreateFile(
                $"Profile{Game.GameConfig.ProfileSlot}/AutoSaveBACKUP_{FileNameMapData}");
            fileToCopy.CopyTo(backupCopy);
            backupCopy.Close();
            fileToCopy.Close();

            fileToCopy = _storageContainer.OpenFile(
                $"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNameMapData}",
                FileMode.Open, FileAccess.Read, FileShare.Read); // Open the backup
            var fileToOverride = _storageContainer.CreateFile($"Profile{Game.GameConfig.ProfileSlot}/{FileNameMapData}");
            fileToCopy.CopyTo(fileToOverride);
            fileToCopy.Close();
            fileToOverride.Close();
        }

        // Lineage Data
        if (
            _storageContainer.FileExists($"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNameLineage}") &&
            _storageContainer.FileExists($"Profile{Game.GameConfig.ProfileSlot}/{FileNameLineage}")
        )
        {
            var fileToCopy = _storageContainer.OpenFile(
                $"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNameLineage}",
                FileMode.Open, FileAccess.Read, FileShare.Read); // Open the backup

            // Copy the backup prior to corruption revert.  This is in case the loading didn't work, and all data is
            // lost (since backups get overwritten once people start playing again).
            var backupCopy = _storageContainer.CreateFile(
                $"Profile{Game.GameConfig.ProfileSlot}/AutoSaveBACKUP_{FileNameLineage}");
            fileToCopy.CopyTo(backupCopy);
            backupCopy.Close();
            fileToCopy.Close();

            fileToCopy = _storageContainer.OpenFile(
                $"Profile{Game.GameConfig.ProfileSlot}/AutoSave_{FileNameLineage}",
                FileMode.Open, FileAccess.Read, FileShare.Read); // Open the backup
            var fileToOverride = _storageContainer.CreateFile($"Profile{Game.GameConfig.ProfileSlot}/{FileNameLineage}");
            fileToCopy.CopyTo(fileToOverride);
            fileToCopy.Close();
            fileToOverride.Close();
        }

        _autosaveLoaded = true;
        _storageContainer.Dispose();
        _storageContainer = null;
    }

    private void SaveData(SaveType saveType, bool saveBackup)
    {
        switch (saveType)
        {
            case SaveType.PlayerData:
                SavePlayerData(saveBackup);
                break;
            case SaveType.UpgradeData:
                SaveUpgradeData(saveBackup);
                break;
            case SaveType.Map:
                SaveMap(saveBackup);
                break;
            case SaveType.MapData:
                SaveMapData(saveBackup);
                break;
            case SaveType.Lineage:
                SaveLineageData(saveBackup);
                break;
            case SaveType.Archipelago:
                SaveArchipelagoData(saveBackup);
                break;
        }

        Console.WriteLine('\n' + $@"Data type {saveType} saved!");
    }

    private void SaveArchipelagoData(bool saveBackup)
    {
        var fileName = FileNameArchipelago;
        if (saveBackup)
        {
            fileName = fileName.Insert(0, "AutoSave_");
        }

        fileName = fileName.Insert(0, "Profile" + Game.GameConfig.ProfileSlot + "/");
        var manager = game.ArchipelagoManager;
        using var stream = _storageContainer.CreateFile(fileName);
        using var writer = new BinaryWriter(stream);

        // Revision number for this save format. (byte)
        writer.Write(RandomizerStats.SAVE_FORMAT_REVISION);

        // Whether this is a multi-world seed or solo generated one. (bool)
        writer.Write(Game.RandomizerStats.Multiworld);

        // AP slot #, used for validation on connection. (int32, char[])
        writer.Write(manager.Slot);

        // AP seed name (or seed for solo), used for validation on connection.
        writer.Write(manager.SeedName); // (int32, char[])

        // AP Generator version (int32, char[])
        writer.Write(manager.GeneratorVersion);

        // Game version (int32, char[])
        writer.Write(LevelEV.RLRX_VERSION);

        // Server connection info (for remembering last connection)  (int32, char[])
        writer.Write(manager.Address);
        writer.Write(manager.SlotName);
        writer.Write(manager.Password);

        // Chests checked and total (int32)
        writer.Write(Game.RandomizerStats.BrownChestsChecked);
        writer.Write(Game.RandomizerStats.BrownChestsTotal);
        writer.Write(Game.RandomizerStats.SilverChestsChecked);
        writer.Write(Game.RandomizerStats.SilverChestsTotal);
        writer.Write(Game.RandomizerStats.GoldChestsChecked);
        writer.Write(Game.RandomizerStats.GoldChestsTotal);
        writer.Write(Game.RandomizerStats.FairyChestsChecked);
        writer.Write(Game.RandomizerStats.FairyChestsTotal);

        // Diary Totals (byte) -- checked is tracked in PlayerData
        writer.Write(Game.RandomizerStats.DiaryEntryTotal);

        if (saveBackup && stream is FileStream fileStream)
        {
            fileStream.Flush(true);
        }
    }

    private void SavePlayerData(bool saveBackup)
    {
        var fileName = FileNamePlayer;
        if (saveBackup)
        {
            fileName = fileName.Insert(0, "AutoSave_");
        }

        fileName = fileName.Insert(0, "Profile" + Game.GameConfig.ProfileSlot + "/");

        // If you have an old version save file, update the name to match the new format before saving.
        if (Game.PlayerStats.RevisionNumber <= 0)
        {
            var playerName = Game.PlayerStats.PlayerName;
            var romanNumeral = "";
            Game.ConvertPlayerNameFormat(ref playerName, ref romanNumeral);
            Game.PlayerStats.PlayerName = playerName;
            Game.PlayerStats.RomanNumeral = romanNumeral;
        }

        using (var stream = _storageContainer.CreateFile(fileName))
            //using (Stream stream = m_storageContainer.OpenFile(fileName, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
        {
            using (var writer = new BinaryWriter(stream))
            {
                // Saving player gold. (int32)
                writer.Write(Game.PlayerStats.Gold);

                // Saving player health. (int32)
                Game.PlayerStats.CurrentHealth = Game.ScreenManager.Player.CurrentHealth;
                writer.Write(Game.PlayerStats.CurrentHealth);
                // Saving player mana. (int32)
                Game.PlayerStats.CurrentMana = (int)Game.ScreenManager.Player.CurrentMana;
                writer.Write(Game.PlayerStats.CurrentMana);

                // Saving player Age. (byte)
                writer.Write(Game.PlayerStats.Age);
                // Saving player's Child Age (byte)
                writer.Write(Game.PlayerStats.ChildAge);
                // Saving player spell (byte)
                writer.Write(Game.PlayerStats.Spell);
                // Saving player class (byte)
                writer.Write(Game.PlayerStats.Class);
                // Saving player special item (byte)
                writer.Write(Game.PlayerStats.SpecialItem);
                // Saving traits. Saved as bytes, but should be loaded into a Vector2.
                writer.Write(Game.PlayerStats.Traits.Trait1);
                writer.Write(Game.PlayerStats.Traits.Trait2);
                // Saving player name (string)
                writer.Write(Game.PlayerStats.PlayerName);

                // Saving player parts. All bytes.
                writer.Write(Game.PlayerStats.HeadPiece);
                writer.Write(Game.PlayerStats.ShoulderPiece);
                writer.Write(Game.PlayerStats.ChestPiece);

                // Saving diary entry (byte)
                writer.Write(Game.PlayerStats.DiaryEntry);

                // Saving bonus stats. All int32s.
                writer.Write(Game.PlayerStats.BonusHealth);
                writer.Write(Game.PlayerStats.BonusStrength);
                writer.Write(Game.PlayerStats.BonusMana);
                writer.Write(Game.PlayerStats.BonusDefense);
                writer.Write(Game.PlayerStats.BonusWeight);
                writer.Write(Game.PlayerStats.BonusMagic);

                // Saving lich health and mana. Only needed for lich.
                writer.Write(Game.PlayerStats.LichHealth); // int32
                writer.Write(Game.PlayerStats.LichMana); // int32
                writer.Write(Game.PlayerStats.LichHealthMod); // Single

                // Saving boss progress. All bools.
                writer.Write(Game.PlayerStats.NewBossBeaten);
                writer.Write(Game.PlayerStats.EyeballBossBeaten);
                writer.Write(Game.PlayerStats.FairyBossBeaten);
                writer.Write(Game.PlayerStats.FireballBossBeaten);
                writer.Write(Game.PlayerStats.BlobBossBeaten);
                writer.Write(Game.PlayerStats.LastbossBeaten);

                // Saving the number of times the castle was beaten (for new game plus) and number of enemies beaten (for rank). (int32)
                writer.Write(Game.PlayerStats.TimesCastleBeaten);
                writer.Write(Game.PlayerStats.NumEnemiesBeaten);

                // Saving misc. flags.
                // Saving castle lock state. Bool.
                writer.Write(Game.PlayerStats.TutorialComplete);
                writer.Write(Game.PlayerStats.CharacterFound);
                writer.Write(Game.PlayerStats.LoadStartingRoom);

                writer.Write(Game.PlayerStats.LockCastle);
                writer.Write(Game.PlayerStats.SpokeToBlacksmith);
                writer.Write(Game.PlayerStats.SpokeToEnchantress);
                writer.Write(Game.PlayerStats.SpokeToArchitect);
                writer.Write(Game.PlayerStats.SpokeToTollCollector);
                writer.Write(Game.PlayerStats.IsDead);
                writer.Write(Game.PlayerStats.FinalDoorOpened);
                writer.Write(Game.PlayerStats.RerolledChildren);
                writer.Write(Game.PlayerStats.IsFemale);
                writer.Write(Game.PlayerStats.TimesDead); // int32;
                writer.Write(Game.PlayerStats.HasArchitectFee);
                writer.Write(Game.PlayerStats.ReadLastDiary);
                writer.Write(Game.PlayerStats.SpokenToLastBoss);
                writer.Write(Game.PlayerStats.HardcoreMode);

                Game.PlayerStats.TotalHoursPlayed += Game.HoursPlayedSinceLastSave;
                Game.HoursPlayedSinceLastSave = 0;
                writer.Write(Game.PlayerStats.TotalHoursPlayed);

                // Saving the wizard spell list.
                writer.Write(Game.PlayerStats.WizardSpellList[0]);
                writer.Write(Game.PlayerStats.WizardSpellList[1]);
                writer.Write(Game.PlayerStats.WizardSpellList[2]);

                if (LevelEV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("\nSaving Player Stats");
                    Console.WriteLine("Gold: " + Game.PlayerStats.Gold);
                    Console.WriteLine("Current Health: " + Game.PlayerStats.CurrentHealth);
                    Console.WriteLine("Current Mana: " + Game.PlayerStats.CurrentMana);
                    Console.WriteLine("Age: " + Game.PlayerStats.Age);
                    Console.WriteLine("Child Age: " + Game.PlayerStats.ChildAge);
                    Console.WriteLine("Spell: " + Game.PlayerStats.Spell);
                    Console.WriteLine("Class: " + Game.PlayerStats.Class);
                    Console.WriteLine("Special Item: " + Game.PlayerStats.SpecialItem);
                    Console.WriteLine("Traits: " + Game.PlayerStats.Traits.Trait1 + ", " +
                                      Game.PlayerStats.Traits.Trait2);
                    Console.WriteLine("Name: " + Game.PlayerStats.PlayerName);
                    Console.WriteLine("---------------");
                    Console.WriteLine("Head Piece: " + Game.PlayerStats.HeadPiece);
                    Console.WriteLine("Shoulder Piece: " + Game.PlayerStats.ShoulderPiece);
                    Console.WriteLine("Chest Piece: " + Game.PlayerStats.ChestPiece);
                    Console.WriteLine("---------------");
                    Console.WriteLine("Diary Entry: " + Game.PlayerStats.DiaryEntry);
                    Console.WriteLine("---------------");
                    Console.WriteLine("Bonus Health: " + Game.PlayerStats.BonusHealth);
                    Console.WriteLine("Bonus Strength: " + Game.PlayerStats.BonusStrength);
                    Console.WriteLine("Bonus Mana: " + Game.PlayerStats.BonusMana);
                    Console.WriteLine("Bonus Armor: " + Game.PlayerStats.BonusDefense);
                    Console.WriteLine("Bonus Weight: " + Game.PlayerStats.BonusWeight);
                    Console.WriteLine("Bonus Magic: " + Game.PlayerStats.BonusMagic);
                    Console.WriteLine("---------------");
                    Console.WriteLine("Lich Health: " + Game.PlayerStats.LichHealth);
                    Console.WriteLine("Lich Mana: " + Game.PlayerStats.LichMana);
                    Console.WriteLine("Lich Health Mod: " + Game.PlayerStats.LichHealthMod);
                    Console.WriteLine("---------------");
                    Console.WriteLine("New Boss Beaten: " + Game.PlayerStats.NewBossBeaten);
                    Console.WriteLine("Eyeball Boss Beaten: " + Game.PlayerStats.EyeballBossBeaten);
                    Console.WriteLine("Fairy Boss Beaten: " + Game.PlayerStats.FairyBossBeaten);
                    Console.WriteLine("Fireball Boss Beaten: " + Game.PlayerStats.FireballBossBeaten);
                    Console.WriteLine("Blob Boss Beaten: " + Game.PlayerStats.BlobBossBeaten);
                    Console.WriteLine("Last Boss Beaten: " + Game.PlayerStats.LastbossBeaten);
                    Console.WriteLine("---------------");
                    Console.WriteLine("Times Castle Beaten: " + Game.PlayerStats.TimesCastleBeaten);
                    Console.WriteLine("Number of Enemies Beaten: " + Game.PlayerStats.NumEnemiesBeaten);
                    Console.WriteLine("---------------");
                    Console.WriteLine("Tutorial Complete: " + Game.PlayerStats.TutorialComplete);
                    Console.WriteLine("Character Found: " + Game.PlayerStats.CharacterFound);
                    Console.WriteLine("Load Starting Room: " + Game.PlayerStats.LoadStartingRoom);
                    Console.WriteLine("---------------");
                    Console.WriteLine("Spoke to Blacksmith: " + Game.PlayerStats.SpokeToBlacksmith);
                    Console.WriteLine("Spoke to Enchantress: " + Game.PlayerStats.SpokeToEnchantress);
                    Console.WriteLine("Spoke to Architect: " + Game.PlayerStats.SpokeToArchitect);
                    Console.WriteLine("Spoke to Toll Collector: " + Game.PlayerStats.SpokeToTollCollector);
                    Console.WriteLine("Player Is Dead: " + Game.PlayerStats.IsDead);
                    Console.WriteLine("Final Door Opened: " + Game.PlayerStats.FinalDoorOpened);
                    Console.WriteLine("Rerolled Children: " + Game.PlayerStats.RerolledChildren);
                    Console.WriteLine("Is Female: " + Game.PlayerStats.IsFemale);
                    Console.WriteLine("Times Dead: " + Game.PlayerStats.TimesDead);
                    Console.WriteLine("Has Architect Fee: " + Game.PlayerStats.HasArchitectFee);
                    Console.WriteLine("Player read last diary: " + Game.PlayerStats.ReadLastDiary);
                    Console.WriteLine("Player has spoken to last boss: " + Game.PlayerStats.SpokenToLastBoss);
                    Console.WriteLine("Is Hardcore mode: " + Game.PlayerStats.HardcoreMode);
                    Console.WriteLine("Total Hours Played " + Game.PlayerStats.TotalHoursPlayed);
                    Console.WriteLine("Wizard Spell 1: " + Game.PlayerStats.WizardSpellList[0]);
                    Console.WriteLine("Wizard Spell 2: " + Game.PlayerStats.WizardSpellList[1]);
                    Console.WriteLine("Wizard Spell 3: " + Game.PlayerStats.WizardSpellList[2]);
                }

                Console.WriteLine("///// ENEMY LIST DATA - BEGIN SAVING /////");
                // Saving the currently created branch.
                List<Vector4> enemyList = Game.PlayerStats.EnemiesKilledList;
                foreach (var enemy in enemyList)
                {
                    writer.Write((byte)enemy.X);
                    writer.Write((byte)enemy.Y);
                    writer.Write((byte)enemy.Z);
                    writer.Write((byte)enemy.W);
                }

                if (LevelEV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("Saving Enemy List Data");
                    var counter = 0;
                    foreach (var enemy in enemyList)
                    {
                        Console.WriteLine("Enemy Type: " + counter + ", Difficulty: Basic, Killed: " + enemy.X);
                        Console.WriteLine("Enemy Type: " + counter + ", Difficulty: Advanced, Killed: " + enemy.Y);
                        Console.WriteLine("Enemy Type: " + counter + ", Difficulty: Expert, Killed: " + enemy.Z);
                        Console.WriteLine("Enemy Type: " + counter + ", Difficulty: Miniboss, Killed: " + enemy.W);
                        counter++;
                    }
                }

                var numKilledEnemiesInRun = Game.PlayerStats.EnemiesKilledInRun.Count;
                List<Vector2> enemiesKilledInRun = Game.PlayerStats.EnemiesKilledInRun;
                writer.Write(numKilledEnemiesInRun);

                foreach (var enemyData in enemiesKilledInRun)
                {
                    writer.Write((int)enemyData.X); // Saving enemy's room location.
                    writer.Write((int)enemyData.Y); // Saving enemy's index in that specific room.
                }

                if (LevelEV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("Saving num enemies killed");
                    Console.WriteLine("Number of enemies killed in run: " + numKilledEnemiesInRun);
                    foreach (var enemy in enemiesKilledInRun)
                    {
                        Console.WriteLine("Enemy Room Index: " + enemy.X);
                        Console.WriteLine("Enemy Index in EnemyList: " + enemy.Y);
                    }
                }

                Console.WriteLine("///// ENEMY LIST DATA - SAVE COMPLETE /////");

                Console.WriteLine("///// DLC DATA - BEGIN SAVING /////");
                // Saving Challenge Room Data. All bools.
                writer.Write(Game.PlayerStats.ChallengeEyeballUnlocked);
                writer.Write(Game.PlayerStats.ChallengeSkullUnlocked);
                writer.Write(Game.PlayerStats.ChallengeFireballUnlocked);
                writer.Write(Game.PlayerStats.ChallengeBlobUnlocked);
                writer.Write(Game.PlayerStats.ChallengeLastBossUnlocked);

                writer.Write(Game.PlayerStats.ChallengeEyeballBeaten);
                writer.Write(Game.PlayerStats.ChallengeSkullBeaten);
                writer.Write(Game.PlayerStats.ChallengeFireballBeaten);
                writer.Write(Game.PlayerStats.ChallengeBlobBeaten);
                writer.Write(Game.PlayerStats.ChallengeLastBossBeaten);

                writer.Write(Game.PlayerStats.ChallengeEyeballTimesUpgraded);
                writer.Write(Game.PlayerStats.ChallengeSkullTimesUpgraded);
                writer.Write(Game.PlayerStats.ChallengeFireballTimesUpgraded);
                writer.Write(Game.PlayerStats.ChallengeBlobTimesUpgraded);
                writer.Write(Game.PlayerStats.ChallengeLastBossTimesUpgraded);

                writer.Write(Game.PlayerStats.RomanNumeral);
                writer.Write(Game.PlayerStats.HasProsopagnosia);
                writer.Write(LevelEV.SAVE_FILE_REVISION_NUMBER);
                writer.Write(Game.PlayerStats.ArchitectUsed);

                if (LevelEV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("Eyeball Challenge Unlocked: " + Game.PlayerStats.ChallengeEyeballUnlocked);
                    Console.WriteLine("Skull Challenge Unlocked: " + Game.PlayerStats.ChallengeSkullUnlocked);
                    Console.WriteLine("Fireball Challenge Unlocked: " + Game.PlayerStats.ChallengeFireballUnlocked);
                    Console.WriteLine("Blob Challenge Unlocked: " + Game.PlayerStats.ChallengeBlobUnlocked);
                    Console.WriteLine("Last Boss Challenge Unlocked: " + Game.PlayerStats.ChallengeLastBossUnlocked);

                    Console.WriteLine("Eyeball Challenge Beaten: " + Game.PlayerStats.ChallengeEyeballBeaten);
                    Console.WriteLine("Skull Challenge Beaten: " + Game.PlayerStats.ChallengeSkullBeaten);
                    Console.WriteLine("Fireball Challenge Beaten: " + Game.PlayerStats.ChallengeFireballBeaten);
                    Console.WriteLine("Blob Challenge Beaten: " + Game.PlayerStats.ChallengeBlobBeaten);
                    Console.WriteLine("Last Boss Challenge Beaten: " + Game.PlayerStats.ChallengeLastBossBeaten);

                    Console.WriteLine("Eyeball Challenge Times Upgraded: " +
                                      Game.PlayerStats.ChallengeEyeballTimesUpgraded);
                    Console.WriteLine("Skull Challenge Times Upgraded: " +
                                      Game.PlayerStats.ChallengeSkullTimesUpgraded);
                    Console.WriteLine("Fireball Challenge Times Upgraded: " +
                                      Game.PlayerStats.ChallengeFireballTimesUpgraded);
                    Console.WriteLine("Blob Challenge Times Upgraded: " + Game.PlayerStats.ChallengeBlobTimesUpgraded);
                    Console.WriteLine("Last Boss Challenge Times Upgraded: " +
                                      Game.PlayerStats.ChallengeLastBossTimesUpgraded);

                    Console.WriteLine("Player Name Number: " + Game.PlayerStats.RomanNumeral);
                    Console.WriteLine("Player HasProsopagnosia: " + Game.PlayerStats.HasProsopagnosia);
                    Console.WriteLine("Save File Revision Number: " + LevelEV.SAVE_FILE_REVISION_NUMBER);
                    Console.WriteLine("Architect used: " + Game.PlayerStats.ArchitectUsed);
                }

                Console.WriteLine("///// DLC DATA - SAVE COMPLETE /////");

                if (saveBackup)
                {
                    var fileStream = stream as FileStream;
                    if (fileStream != null)
                    {
                        fileStream.Flush(true);
                    }
                }

                writer.Close();
            }

            stream.Close();
        }
    }

    private void SaveUpgradeData(bool saveBackup)
    {
        var fileName = FileNameUpgrades;
        if (saveBackup)
        {
            fileName = fileName.Insert(0, "AutoSave_");
        }

        fileName = fileName.Insert(0, "Profile" + Game.GameConfig.ProfileSlot + "/");

        using (var stream = _storageContainer.CreateFile(fileName))
        {
            using (var writer = new BinaryWriter(stream))
            {
                if (LevelEV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("\nSaving Equipment States");
                }

                // Saving the base equipment states.
                List<byte[]> blueprintArray = Game.PlayerStats.GetBlueprintArray;
                if (LevelEV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("Standard Blueprints");
                }

                foreach (var categoryType in blueprintArray)
                {
                    foreach (var equipmentState in categoryType)
                    {
                        writer.Write(equipmentState);
                        if (LevelEV.ShowSaveLoadDebugText)
                        {
                            Console.Write(" " + equipmentState);
                        }
                    }

                    if (LevelEV.ShowSaveLoadDebugText)
                    {
                        Console.Write("\n");
                    }
                }

                // Saving the ability equipment states.
                List<byte[]> abilityBPArray = Game.PlayerStats.GetRuneArray;
                if (LevelEV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("\nRune Blueprints");
                }

                foreach (var categoryType in abilityBPArray)
                {
                    foreach (var equipmentState in categoryType)
                    {
                        writer.Write(equipmentState);
                        if (LevelEV.ShowSaveLoadDebugText)
                        {
                            Console.Write(" " + equipmentState);
                        }
                    }

                    if (LevelEV.ShowSaveLoadDebugText)
                    {
                        Console.Write("\n");
                    }
                }

                // Saving equipped items
                var equippedArray = Game.PlayerStats.GetEquippedArray;
                if (LevelEV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("\nEquipped Standard Item");
                }

                foreach (var equipmentState in equippedArray)
                {
                    writer.Write(equipmentState);
                    if (LevelEV.ShowSaveLoadDebugText)
                    {
                        Console.Write(" " + equipmentState);
                    }
                }

                // Saving equipped abilities
                var equippedAbilityArray = Game.PlayerStats.GetEquippedRuneArray;
                if (LevelEV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("\nEquipped Abilities");
                }

                foreach (var equipmentState in equippedAbilityArray)
                {
                    writer.Write(equipmentState);
                    if (LevelEV.ShowSaveLoadDebugText)
                    {
                        Console.Write(" " + equipmentState);
                    }
                }

                // Saving skills data.
                SkillObj[] skillArray = SkillSystem.GetSkillArray();
                if (LevelEV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("\nskills");
                }

                foreach (var skill in skillArray)
                {
                    writer.Write(skill.CurrentLevel);
                    if (LevelEV.ShowSaveLoadDebugText)
                    {
                        Console.Write(" " + skill.CurrentLevel);
                    }
                }

                if (saveBackup)
                {
                    var fileStream = stream as FileStream;
                    if (fileStream != null)
                    {
                        fileStream.Flush(true);
                    }
                }

                writer.Close();
            }

            stream.Close();
        }
    }

    private void SaveMap(bool saveBackup)
    {
        var fileName = FileNameMap;
        if (saveBackup)
        {
            fileName = fileName.Insert(0, "AutoSave_");
        }

        fileName = fileName.Insert(0, "Profile" + Game.GameConfig.ProfileSlot + "/");

        using (var stream = _storageContainer.CreateFile(fileName))
        {
            using (var writer = new BinaryWriter(stream))
            {
                if (LevelEV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("\nSaving Map");
                }

                var newlineCounter = 0;

                var levelToSave = Game.ScreenManager.GetLevelScreen();
                if (levelToSave != null)
                {
                    //Console.WriteLine("START");
                    //foreach (RoomObj room in levelToSave.RoomList)
                    //{
                    //    if (room.Name != "Boss" && room.Name != "Compass" && room.Name != "Bonus")
                    //    {
                    //        int count = 0;
                    //        foreach (GameObj obj in room.GameObjList)
                    //        {
                    //            if (obj is ChestObj)
                    //                count++;
                    //        }
                    //        Console.WriteLine(count);
                    //    }
                    //}
                    //Console.WriteLine("END");

                    //Console.WriteLine("Enemy start");
                    //foreach (RoomObj room in levelToSave.RoomList)
                    //{
                    //    int count = 0;
                    //    foreach (EnemyObj enemy in room.EnemyList)
                    //    {
                    //        if (enemy.IsProcedural == true)
                    //            count++;
                    //    }
                    //    Console.WriteLine(count);
                    //}
                    //Console.WriteLine("Enemy End");

                    // Store the number of rooms in the level first.

                    writer.Write(levelToSave.RoomList.Count -
                                 12); // Subtracting the 5 boss rooms + the tutorial room + compass room + 5 challenge rooms.

                    if (LevelEV.ShowSaveLoadDebugText)
                    {
                        Console.WriteLine("Map size: " +
                                          (levelToSave.RoomList.Count -
                                           12)); // Subtracting the 5 boss rooms + the tutorial room + the compass room + 5 challenge rooms.
                    }

                    var enemyTypeList = new List<byte>();
                    var enemyDifficultyList = new List<byte>();

                    // Storing the actual map.
                    foreach (var room in levelToSave.RoomList)
                    {
                        if (room.Name != "Boss" && room.Name != "Tutorial" && room.Name != "Ending" &&
                            room.Name != "Compass" &&
                            room.Name !=
                            "ChallengeBoss") // && room.Name != "Bonus") // Do not store boss rooms because they will be added when loaded.
                        {
                            // Saving the room's pool index.
                            writer.Write(room.PoolIndex);
                            // Saving the level type of the room. (byte)
                            writer.Write((byte)room.LevelType);
                            // Saving the level's position. (int)
                            writer.Write((int)room.X);
                            writer.Write((int)room.Y);

                            // Saving the room's colour.
                            writer.Write(room.TextureColor.R);
                            writer.Write(room.TextureColor.G);
                            writer.Write(room.TextureColor.B);

                            if (LevelEV.ShowSaveLoadDebugText)
                            {
                                Console.Write("I:" + room.PoolIndex + " T:" + (int)room.LevelType + ", ");
                            }

                            newlineCounter++;
                            if (newlineCounter > 5)
                            {
                                newlineCounter = 0;
                                if (LevelEV.ShowSaveLoadDebugText)
                                {
                                    Console.Write("\n");
                                }
                            }

                            foreach (var enemy in room.EnemyList)
                            {
                                if (enemy.IsProcedural)
                                {
                                    enemyTypeList.Add(enemy.Type);
                                    enemyDifficultyList.Add((byte)enemy.Difficulty);
                                }
                            }
                        }
                    }

                    // Saving the enemies in the level
                    var numEnemies = enemyTypeList.Count;
                    writer.Write(numEnemies); // Saving the number of enemies in the game. (int32)
                    foreach (var enemyType in enemyTypeList)
                    {
                        writer.Write(enemyType); //(byte)
                    }

                    // Saving the difficulty of each enemy.
                    foreach (var enemyDifficulty in enemyDifficultyList)
                    {
                        writer.Write(enemyDifficulty);
                    }
                }
                else
                {
                    Console.WriteLine(
                        "WARNING: Attempting to save LEVEL screen but it was null. Make sure it exists in the screen manager before saving it.");
                }

                if (saveBackup)
                {
                    var fileStream = stream as FileStream;
                    if (fileStream != null)
                    {
                        fileStream.Flush(true);
                    }
                }

                writer.Close();
            }

            stream.Close();
        }
    }

    private void SaveMapData(bool saveBackup)
    {
        var fileName = FileNameMapData;
        if (saveBackup)
        {
            fileName = fileName.Insert(0, "AutoSave_");
        }

        fileName = fileName.Insert(0, "Profile" + Game.GameConfig.ProfileSlot + "/");

        using (var stream = _storageContainer.CreateFile(fileName))
        {
            using (var writer = new BinaryWriter(stream))
            {
                var levelToSave = Game.ScreenManager.GetLevelScreen();
                if (levelToSave != null)
                {
                    List<RoomObj> levelMapList = levelToSave.MapRoomsAdded;

                    var roomVisited = new List<bool>();
                    var bonusRoomCompleted = new List<bool>();
                    var bonusRoomData = new List<int>();
                    var chestOpened = new List<bool>();
                    var chestTypes = new List<byte>();
                    var fairyChestFailed = new List<bool>();
                    var enemyDead = new List<bool>();
                    var breakablesOpened = new List<bool>();

                    foreach (var room in levelToSave.RoomList)
                    {
                        if (levelMapList.Contains(room))
                        {
                            roomVisited.Add(true);
                        }
                        else
                        {
                            roomVisited.Add(false);
                        }

                        var bonusRoom = room as BonusRoomObj;
                        if (bonusRoom != null)
                        {
                            if (bonusRoom.RoomCompleted)
                            {
                                bonusRoomCompleted.Add(true);
                            }
                            else
                            {
                                bonusRoomCompleted.Add(false);
                            }

                            bonusRoomData.Add(bonusRoom.ID);
                        }

                        if (room.Name != "Boss" && room.Name != "ChallengeBoss") // && room.Name != "Bonus")
                        {
                            foreach (var enemy in room.EnemyList)
                            {
                                if (enemy.IsKilled)
                                {
                                    enemyDead.Add(true);
                                }
                                else
                                {
                                    enemyDead.Add(false);
                                }
                            }
                        }

                        if (room.Name != "Bonus" && room.Name != "Boss" && room.Name != "Compass" &&
                            room.Name !=
                            "ChallengeBoss") // Don't save bonus room or boss room chests, or bonus room breakables (it's ok if they reset).
                        {
                            foreach (var obj in room.GameObjList)
                            {
                                // Saving breakables state.
                                var breakable = obj as BreakableObj;
                                if (breakable != null)
                                {
                                    if (breakable.Broken)
                                    {
                                        breakablesOpened.Add(true);
                                    }
                                    else
                                    {
                                        breakablesOpened.Add(false);
                                    }
                                }

                                // Saving chest states.
                                var chest = obj as ChestObj;
                                if (chest != null)
                                {
                                    chestTypes.Add(chest.ChestType);

                                    if (chest.IsOpen)
                                    {
                                        chestOpened.Add(true);
                                    }
                                    else
                                    {
                                        chestOpened.Add(false);
                                    }

                                    var fairyChest = chest as FairyChestObj;
                                    if (fairyChest != null)
                                    {
                                        if (fairyChest.State == ChestState.Failed)
                                        {
                                            fairyChestFailed.Add(true);
                                        }
                                        else
                                        {
                                            fairyChestFailed.Add(false);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Saving rooms visited state.
                    writer.Write(roomVisited.Count); // int32
                    foreach (var state in roomVisited)
                    {
                        writer.Write(state);
                    }

                    // Saving the state of the bonus rooms.
                    writer.Write(bonusRoomCompleted.Count); // int32
                    foreach (var state in bonusRoomCompleted)
                    {
                        writer.Write(state);
                    }

                    // Saving the data of the bonus rooms.
                    // No need to save number of bonus rooms because that's saved above.
                    foreach (var data in bonusRoomData)
                    {
                        writer.Write(data);
                    }

                    // Saving the type of chests.
                    writer.Write(chestTypes.Count); // int32
                    foreach (var type in chestTypes)
                    {
                        writer.Write(type);
                    }

                    // Saving the state of chests.
                    writer.Write(chestOpened.Count); // int32
                    foreach (var state in chestOpened)
                    {
                        writer.Write(state);
                    }

                    // Saving the state of fairy chests.
                    writer.Write(fairyChestFailed.Count); // int32
                    foreach (var state in fairyChestFailed)
                    {
                        writer.Write(state);
                    }

                    // Saving the state of enemies
                    writer.Write(enemyDead.Count); // int32
                    foreach (var state in enemyDead)
                    {
                        writer.Write(state);
                    }

                    // Saving breakable object states
                    writer.Write(breakablesOpened.Count); // int32
                    foreach (var state in breakablesOpened)
                    {
                        writer.Write(state);
                    }
                }
                else
                {
                    Console.WriteLine(
                        "WARNING: Attempting to save level screen MAP data but level was null. Make sure it exists in the screen manager before saving it.");
                }

                if (saveBackup)
                {
                    var fileStream = stream as FileStream;
                    if (fileStream != null)
                    {
                        fileStream.Flush(true);
                    }
                }

                writer.Close();
            }

            stream.Close();
        }
    }

    private void SaveLineageData(bool saveBackup)
    {
        var fileName = FileNameLineage;
        if (saveBackup)
        {
            fileName = fileName.Insert(0, "AutoSave_");
        }

        fileName = fileName.Insert(0, "Profile" + Game.GameConfig.ProfileSlot + "/");

        using (var stream = _storageContainer.CreateFile(fileName))
        {
            using (var writer = new BinaryWriter(stream))
            {
                Console.WriteLine("///// PLAYER LINEAGE DATA - BEGIN SAVING /////");
                // Saving the currently created branch.
                List<PlayerLineageData> currentBranches = Game.PlayerStats.CurrentBranches;
                var numChildren = 0;

                if (currentBranches != null)
                {
                    numChildren = currentBranches.Count;
                    writer.Write(numChildren);

                    for (var i = 0; i < numChildren; i++)
                    {
                        // If you have an old version save file, update the name to match the new format before saving.
                        if (Game.PlayerStats.RevisionNumber <= 0)
                        {
                            var playerName = currentBranches[i].Name;
                            var romanNumeral = "";
                            Game.ConvertPlayerNameFormat(ref playerName, ref romanNumeral);
                            var data = currentBranches[i];
                            data.Name = playerName;
                            data.RomanNumeral = romanNumeral;
                            currentBranches[i] = data;
                        }

                        writer.Write(currentBranches[i].Name); // string
                        writer.Write(currentBranches[i].Spell); // byte
                        writer.Write(currentBranches[i].Class); // byte
                        writer.Write(currentBranches[i].HeadPiece); // byte
                        writer.Write(currentBranches[i].ChestPiece); // byte
                        writer.Write(currentBranches[i].ShoulderPiece); // byte

                        writer.Write(currentBranches[i].Age); // byte
                        writer.Write(currentBranches[i].ChildAge); // byte

                        writer.Write(currentBranches[i].Traits.Trait1); // byte
                        writer.Write(currentBranches[i].Traits.Trait2); // byte
                        writer.Write(currentBranches[i].IsFemale); // bool
                        writer.Write(currentBranches[i].RomanNumeral); // string
                    }
                }
                else
                {
                    writer.Write(numChildren); // Writing zero children.
                }

                if (LevelEV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("Saving Current Branch Lineage Data");
                    for (var i = 0; i < numChildren; i++)
                    {
                        Console.WriteLine("Player Name: " + currentBranches[i].Name);
                        Console.WriteLine("Spell: " + currentBranches[i].Name);
                        Console.WriteLine("Class: " + currentBranches[i].Name);
                        Console.WriteLine("Head Piece: " + currentBranches[i].HeadPiece);
                        Console.WriteLine("Chest Piece: " + currentBranches[i].ChestPiece);
                        Console.WriteLine("Shoulder Piece: " + currentBranches[i].ShoulderPiece);
                        Console.WriteLine("Player Age: " + currentBranches[i].Age);
                        Console.WriteLine("Player Child Age: " + currentBranches[i].ChildAge);
                        Console.WriteLine("Traits: " + currentBranches[i].Traits.Trait1 + ", " +
                                          currentBranches[i].Traits.Trait2);
                        Console.WriteLine("Is Female: " + currentBranches[i].IsFemale);
                        Console.WriteLine("Roman Numeral: " + currentBranches[i].RomanNumeral);
                    }
                }

                ////////////////////////////////////////

                // Saving family tree info
                List<FamilyTreeNode> familyTreeArray = Game.PlayerStats.FamilyTreeArray;
                var numBranches = 0;

                if (familyTreeArray != null)
                {
                    numBranches = familyTreeArray.Count;
                    writer.Write(numBranches);

                    for (var i = 0; i < numBranches; i++)
                    {
                        // If you have an old version save file, update the name to match the new format before saving.
                        if (Game.PlayerStats.RevisionNumber <= 0)
                        {
                            var playerName = familyTreeArray[i].Name;
                            var romanNumeral = "";
                            Game.ConvertPlayerNameFormat(ref playerName, ref romanNumeral);
                            var data = familyTreeArray[i];
                            data.Name = playerName;
                            data.RomanNumeral = romanNumeral;
                            familyTreeArray[i] = data;
                        }

                        writer.Write(familyTreeArray[i].Name); // string
                        writer.Write(familyTreeArray[i].Age); // byte
                        writer.Write(familyTreeArray[i].Class); // byte
                        writer.Write(familyTreeArray[i].HeadPiece); // byte
                        writer.Write(familyTreeArray[i].ChestPiece); // byte
                        writer.Write(familyTreeArray[i].ShoulderPiece); // byte
                        writer.Write(familyTreeArray[i].NumEnemiesBeaten); // int
                        writer.Write(familyTreeArray[i].BeatenABoss); // bool
                        writer.Write(familyTreeArray[i].Traits.Trait1); // byte
                        writer.Write(familyTreeArray[i].Traits.Trait2); // byte
                        writer.Write(familyTreeArray[i].IsFemale); // bool
                        writer.Write(familyTreeArray[i].RomanNumeral); // string
                    }
                }
                else
                {
                    writer.Write(numBranches); // Tree has no nodes.
                }

                if (LevelEV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("Saving Family Tree Data");
                    Console.WriteLine("Number of Branches: " + numBranches);
                    for (var i = 0; i < numBranches; i++)
                    {
                        Console.WriteLine("/// Saving branch");
                        Console.WriteLine("Name: " + familyTreeArray[i].Name);
                        Console.WriteLine("Age: " + familyTreeArray[i].Age);
                        Console.WriteLine("Class: " + familyTreeArray[i].Class);
                        Console.WriteLine("Head Piece: " + familyTreeArray[i].HeadPiece);
                        Console.WriteLine("Chest Piece: " + familyTreeArray[i].ChestPiece);
                        Console.WriteLine("Shoulder Piece: " + familyTreeArray[i].ShoulderPiece);
                        Console.WriteLine("Number of Enemies Beaten: " + familyTreeArray[i].NumEnemiesBeaten);
                        Console.WriteLine("Beaten a Boss: " + familyTreeArray[i].BeatenABoss);
                        Console.WriteLine("Traits: " + familyTreeArray[i].Traits.Trait1 + ", " +
                                          familyTreeArray[i].Traits.Trait2);
                        Console.WriteLine("Is Female: " + familyTreeArray[i].IsFemale);
                        Console.WriteLine("Roman Numeral: " + familyTreeArray[i].RomanNumeral);
                    }
                }

                Console.WriteLine("///// PLAYER LINEAGE DATA - SAVE COMPLETE /////");
                if (saveBackup)
                {
                    var fileStream = stream as FileStream;
                    if (fileStream != null)
                    {
                        fileStream.Flush(true);
                    }
                }

                writer.Close();
            }

            stream.Close();
        }
    }

    private void LoadData(SaveType loadType, ProceduralLevelScreen level)
    {
        if (FileExists(loadType))
        {
            switch (loadType)
            {
                case SaveType.PlayerData:
                    //if (Game.ScreenManager.Player != null)
                    LoadPlayerData();
                    break;
                case SaveType.UpgradeData:
                    //if (Game.ScreenManager.Player != null)
                    LoadUpgradeData();
                    break;
                case SaveType.Map:
                    Console.WriteLine("Cannot load Map directly from LoadData. Call LoadMap() instead.");
                    //if (Game.PlayerStats.IsDead == false)
                    //{
                    //    ProceduralLevelScreen createdLevel = LoadMap();
                    //    createdLevel.LoadGameData = true;
                    //}
                    //else
                    //    Game.ScreenManager.DisplayScreen(ScreenType.Lineage, true, null);
                    break;
                case SaveType.MapData:
                    if (level != null)
                    {
                        LoadMapData(level);
                    }
                    else
                    {
                        Console.WriteLine("Could not load Map data. Level was null.");
                    }

                    break;
                case SaveType.Lineage:
                    LoadLineageData();
                    break;
                case SaveType.Archipelago:
                    LoadArchipelagoData();
                    break;
            }

            Console.WriteLine("\nData of type " + loadType + " Loaded.");
        }
        else
        {
            Console.WriteLine("Could not load data of type " + loadType + " because data did not exist.");
        }
    }

    private void LoadArchipelagoData()
    {
        using var stream = _storageContainer.OpenFile(
            $"Profile${Game.GameConfig.ProfileSlot}/{FileNameArchipelago}",
            FileMode.Open, FileAccess.Read, FileShare.Read);
        using var reader = new BinaryReader(stream);

        if (reader.ReadByte() < RandomizerStats.SAVE_FORMAT_REVISION)
        {
            throw new NotImplementedException("Loading previous save formats is not currently supported.");
        }

        // Metadata
        Game.RandomizerStats.Multiworld = reader.ReadBoolean();
        reader.ReadInt32();  // slot
        reader.ReadString(); // seed
        reader.ReadString(); // gen version
        reader.ReadString(); // game version
        reader.ReadString(); // address
        reader.ReadString(); // slot name
        reader.ReadString(); // password

        // Chests
        Game.RandomizerStats.BrownChestsChecked = reader.ReadInt32();
        Game.RandomizerStats.BrownChestsTotal = reader.ReadInt32();
        Game.RandomizerStats.SilverChestsChecked = reader.ReadInt32();
        Game.RandomizerStats.SilverChestsTotal = reader.ReadInt32();
        Game.RandomizerStats.GoldChestsChecked = reader.ReadInt32();
        Game.RandomizerStats.GoldChestsTotal = reader.ReadInt32();
        Game.RandomizerStats.FairyChestsChecked = reader.ReadInt32();
        Game.RandomizerStats.FairyChestsTotal = reader.ReadInt32();
        Game.RandomizerStats.DiaryEntryTotal = reader.ReadByte();
    }

    private void LoadPlayerData()
    {
        using (var stream = _storageContainer.OpenFile(
                   "Profile" + Game.GameConfig.ProfileSlot + "/" + FileNamePlayer, FileMode.Open, FileAccess.Read,
                   FileShare.Read))
        {
            using (var reader = new BinaryReader(stream))
            {
                //Game.PlayerStats.PlayerPosition = new Vector2(reader.ReadInt32(), reader.ReadInt32());
                Game.PlayerStats.Gold = reader.ReadInt32();
                Game.PlayerStats.CurrentHealth = reader.ReadInt32();
                Game.PlayerStats.CurrentMana = reader.ReadInt32();
                Game.PlayerStats.Age = reader.ReadByte();
                Game.PlayerStats.ChildAge = reader.ReadByte();
                Game.PlayerStats.Spell = reader.ReadByte();
                Game.PlayerStats.Class = reader.ReadByte();
                Game.PlayerStats.SpecialItem = reader.ReadByte();
                Game.PlayerStats.Traits = (reader.ReadByte(), reader.ReadByte());
                Game.PlayerStats.PlayerName = reader.ReadString();

                // Reading player parts
                Game.PlayerStats.HeadPiece = reader.ReadByte();
                Game.PlayerStats.ShoulderPiece = reader.ReadByte();
                Game.PlayerStats.ChestPiece = reader.ReadByte();

                // Necessary to kick in the try catch block in CDGSplashScreen since reader.ReadByte() returns 0 instead of crashing when reading incorrect data.
                //if (Game.PlayerStats.HeadPiece == 0 || Game.PlayerStats.ShoulderPiece == 0 || Game.PlayerStats.ChestPiece == 0)
                if (Game.PlayerStats.HeadPiece == 0 && Game.PlayerStats.ShoulderPiece == 0 &&
                    Game.PlayerStats.ChestPiece == 0)
                {
                    throw new Exception("Corrupted Save File: All equipment pieces are 0.");
                }

                // Reading Diary entry position
                Game.PlayerStats.DiaryEntry = reader.ReadByte();

                // Reading bonus stats.
                Game.PlayerStats.BonusHealth = reader.ReadInt32();
                Game.PlayerStats.BonusStrength = reader.ReadInt32();
                Game.PlayerStats.BonusMana = reader.ReadInt32();
                Game.PlayerStats.BonusDefense = reader.ReadInt32();
                Game.PlayerStats.BonusWeight = reader.ReadInt32();
                Game.PlayerStats.BonusMagic = reader.ReadInt32();

                // Reading lich stats.
                Game.PlayerStats.LichHealth = reader.ReadInt32();
                Game.PlayerStats.LichMana = reader.ReadInt32();
                Game.PlayerStats.LichHealthMod = reader.ReadSingle();

                // Reading boss progress states
                Game.PlayerStats.NewBossBeaten = reader.ReadBoolean();
                Game.PlayerStats.EyeballBossBeaten = reader.ReadBoolean();
                Game.PlayerStats.FairyBossBeaten = reader.ReadBoolean();
                Game.PlayerStats.FireballBossBeaten = reader.ReadBoolean();
                Game.PlayerStats.BlobBossBeaten = reader.ReadBoolean();
                Game.PlayerStats.LastbossBeaten = reader.ReadBoolean();

                // Reading new game plus progress
                Game.PlayerStats.TimesCastleBeaten = reader.ReadInt32();
                Game.PlayerStats.NumEnemiesBeaten = reader.ReadInt32();

                // Loading misc flags
                Game.PlayerStats.TutorialComplete = reader.ReadBoolean();
                Game.PlayerStats.CharacterFound = reader.ReadBoolean();
                Game.PlayerStats.LoadStartingRoom = reader.ReadBoolean();

                Game.PlayerStats.LockCastle = reader.ReadBoolean();
                Game.PlayerStats.SpokeToBlacksmith = reader.ReadBoolean();
                Game.PlayerStats.SpokeToEnchantress = reader.ReadBoolean();
                Game.PlayerStats.SpokeToArchitect = reader.ReadBoolean();
                Game.PlayerStats.SpokeToTollCollector = reader.ReadBoolean();
                Game.PlayerStats.IsDead = reader.ReadBoolean();
                Game.PlayerStats.FinalDoorOpened = reader.ReadBoolean();
                Game.PlayerStats.RerolledChildren = reader.ReadBoolean();
                Game.PlayerStats.IsFemale = reader.ReadBoolean();
                Game.PlayerStats.TimesDead = reader.ReadInt32();
                Game.PlayerStats.HasArchitectFee = reader.ReadBoolean();
                Game.PlayerStats.ReadLastDiary = reader.ReadBoolean();
                Game.PlayerStats.SpokenToLastBoss = reader.ReadBoolean();
                Game.PlayerStats.HardcoreMode = reader.ReadBoolean();
                Game.PlayerStats.TotalHoursPlayed = reader.ReadSingle();

                // Loading wizard spells
                var wizardSpell1 = reader.ReadByte();
                var wizardSpell2 = reader.ReadByte();
                var wizardSpell3 = reader.ReadByte();

                Game.PlayerStats.WizardSpellList = [wizardSpell1, wizardSpell2, wizardSpell3];

                if (LevelEV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("\nLoading Player Stats");
                    Console.WriteLine("Gold: " + Game.PlayerStats.Gold);
                    Console.WriteLine("Current Health: " + Game.PlayerStats.CurrentHealth);
                    Console.WriteLine("Current Mana: " + Game.PlayerStats.CurrentMana);
                    Console.WriteLine("Age: " + Game.PlayerStats.Age);
                    Console.WriteLine("Child Age: " + Game.PlayerStats.ChildAge);
                    Console.WriteLine("Spell: " + Game.PlayerStats.Spell);
                    Console.WriteLine("Class: " + Game.PlayerStats.Class);
                    Console.WriteLine("Special Item: " + Game.PlayerStats.SpecialItem);
                    Console.WriteLine("Traits: " + Game.PlayerStats.Traits.Trait1 + ", " +
                                      Game.PlayerStats.Traits.Trait2);
                    Console.WriteLine("Name: " + Game.PlayerStats.PlayerName);
                    Console.WriteLine("---------------");
                    Console.WriteLine("Head Piece: " + Game.PlayerStats.HeadPiece);
                    Console.WriteLine("Shoulder Piece: " + Game.PlayerStats.ShoulderPiece);
                    Console.WriteLine("Chest Piece: " + Game.PlayerStats.ChestPiece);
                    Console.WriteLine("---------------");
                    Console.WriteLine("Diary Entry: " + Game.PlayerStats.DiaryEntry);
                    Console.WriteLine("---------------");
                    Console.WriteLine("Bonus Health: " + Game.PlayerStats.BonusHealth);
                    Console.WriteLine("Bonus Strength: " + Game.PlayerStats.BonusStrength);
                    Console.WriteLine("Bonus Mana: " + Game.PlayerStats.BonusMana);
                    Console.WriteLine("Bonus Armor: " + Game.PlayerStats.BonusDefense);
                    Console.WriteLine("Bonus Weight: " + Game.PlayerStats.BonusWeight);
                    Console.WriteLine("Bonus Magic: " + Game.PlayerStats.BonusMagic);
                    Console.WriteLine("---------------");
                    Console.WriteLine("Lich Health: " + Game.PlayerStats.LichHealth);
                    Console.WriteLine("Lich Mana: " + Game.PlayerStats.LichMana);
                    Console.WriteLine("Lich Health Mod: " + Game.PlayerStats.LichHealthMod);
                    Console.WriteLine("---------------");
                    Console.WriteLine("New Boss Beaten: " + Game.PlayerStats.NewBossBeaten);
                    Console.WriteLine("Eyeball Boss Beaten: " + Game.PlayerStats.EyeballBossBeaten);
                    Console.WriteLine("Fairy Boss Beaten: " + Game.PlayerStats.FairyBossBeaten);
                    Console.WriteLine("Fireball Boss Beaten: " + Game.PlayerStats.FireballBossBeaten);
                    Console.WriteLine("Blob Boss Beaten: " + Game.PlayerStats.BlobBossBeaten);
                    Console.WriteLine("Last Boss Beaten: " + Game.PlayerStats.LastbossBeaten);
                    Console.WriteLine("---------------");
                    Console.WriteLine("Times Castle Beaten: " + Game.PlayerStats.TimesCastleBeaten);
                    Console.WriteLine("Number of Enemies Beaten: " + Game.PlayerStats.NumEnemiesBeaten);
                    Console.WriteLine("---------------");
                    Console.WriteLine("Tutorial Complete: " + Game.PlayerStats.TutorialComplete);
                    Console.WriteLine("Character Found: " + Game.PlayerStats.CharacterFound);
                    Console.WriteLine("Load Starting Room: " + Game.PlayerStats.LoadStartingRoom);
                    Console.WriteLine("---------------");
                    Console.WriteLine("Castle Locked: " + Game.PlayerStats.LockCastle);
                    Console.WriteLine("Spoke to Blacksmith: " + Game.PlayerStats.SpokeToBlacksmith);
                    Console.WriteLine("Spoke to Enchantress: " + Game.PlayerStats.SpokeToEnchantress);
                    Console.WriteLine("Spoke to Architect: " + Game.PlayerStats.SpokeToArchitect);
                    Console.WriteLine("Spoke to Toll Collector: " + Game.PlayerStats.SpokeToTollCollector);
                    Console.WriteLine("Player Is Dead: " + Game.PlayerStats.IsDead);
                    Console.WriteLine("Final Door Opened: " + Game.PlayerStats.FinalDoorOpened);
                    Console.WriteLine("Rerolled Children: " + Game.PlayerStats.RerolledChildren);
                    Console.WriteLine("Is Female: " + Game.PlayerStats.IsFemale);
                    Console.WriteLine("Times Dead: " + Game.PlayerStats.TimesDead);
                    Console.WriteLine("Has Architect Fee: " + Game.PlayerStats.HasArchitectFee);
                    Console.WriteLine("Player read last diary: " + Game.PlayerStats.ReadLastDiary);
                    Console.WriteLine("Player has spoken to last boss: " + Game.PlayerStats.SpokenToLastBoss);
                    Console.WriteLine("Is Hardcore mode: " + Game.PlayerStats.HardcoreMode);
                    Console.WriteLine("Total Hours Played " + Game.PlayerStats.TotalHoursPlayed);
                    Console.WriteLine("Wizard Spell 1: " + Game.PlayerStats.WizardSpellList[0]);
                    Console.WriteLine("Wizard Spell 2: " + Game.PlayerStats.WizardSpellList[1]);
                    Console.WriteLine("Wizard Spell 3: " + Game.PlayerStats.WizardSpellList[2]);
                }

                Console.WriteLine("///// ENEMY LIST DATA - BEGIN LOADING /////");

                for (var i = 0; i < EnemyType.TOTAL; i++)
                {
                    var enemyStats = new Vector4(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(),
                        reader.ReadByte());
                    Game.PlayerStats.EnemiesKilledList[i] = enemyStats;
                }

                if (LevelEV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("Loading Enemy List Data");
                    var counter = 0;
                    foreach (var enemy in Game.PlayerStats.EnemiesKilledList)
                    {
                        Console.WriteLine("Enemy Type: " + counter + ", Difficulty: Basic, Killed: " + enemy.X);
                        Console.WriteLine("Enemy Type: " + counter + ", Difficulty: Advanced, Killed: " + enemy.Y);
                        Console.WriteLine("Enemy Type: " + counter + ", Difficulty: Expert, Killed: " + enemy.Z);
                        Console.WriteLine("Enemy Type: " + counter + ", Difficulty: Miniboss, Killed: " + enemy.W);

                        counter++;
                    }
                }

                var numKilledEnemiesInRun = reader.ReadInt32();

                for (var i = 0; i < numKilledEnemiesInRun; i++)
                {
                    var enemyKilled = new Vector2(reader.ReadInt32(), reader.ReadInt32());
                    Game.PlayerStats.EnemiesKilledInRun.Add(enemyKilled);
                }

                if (LevelEV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("Loading num enemies killed");
                    Console.WriteLine("Number of enemies killed in run: " + numKilledEnemiesInRun);
                    foreach (var enemy in Game.PlayerStats.EnemiesKilledInRun)
                    {
                        Console.WriteLine("Enemy Room Index: " + enemy.X);
                        Console.WriteLine("Enemy Index in EnemyList: " + enemy.Y);
                    }
                }

                Console.WriteLine("///// ENEMY LIST DATA - LOAD COMPLETE /////");


                if (reader.PeekChar() != -1) // If a character is found, then DLC is in the file.
                {
                    Console.WriteLine("///// DLC DATA FOUND - BEGIN LOADING /////");
                    Game.PlayerStats.ChallengeEyeballUnlocked = reader.ReadBoolean();
                    Game.PlayerStats.ChallengeSkullUnlocked = reader.ReadBoolean();
                    Game.PlayerStats.ChallengeFireballUnlocked = reader.ReadBoolean();
                    Game.PlayerStats.ChallengeBlobUnlocked = reader.ReadBoolean();
                    Game.PlayerStats.ChallengeLastBossUnlocked = reader.ReadBoolean();

                    Game.PlayerStats.ChallengeEyeballBeaten = reader.ReadBoolean();
                    Game.PlayerStats.ChallengeSkullBeaten = reader.ReadBoolean();
                    Game.PlayerStats.ChallengeFireballBeaten = reader.ReadBoolean();
                    Game.PlayerStats.ChallengeBlobBeaten = reader.ReadBoolean();
                    Game.PlayerStats.ChallengeLastBossBeaten = reader.ReadBoolean();

                    if (LevelEV.ShowSaveLoadDebugText)
                    {
                        Console.WriteLine("Eyeball Challenge Unlocked: " + Game.PlayerStats.ChallengeEyeballUnlocked);
                        Console.WriteLine("Skull Challenge Unlocked: " + Game.PlayerStats.ChallengeSkullUnlocked);
                        Console.WriteLine("Fireball Challenge Unlocked: " + Game.PlayerStats.ChallengeFireballUnlocked);
                        Console.WriteLine("Blob Challenge Unlocked: " + Game.PlayerStats.ChallengeBlobUnlocked);
                        Console.WriteLine("Last Boss Challenge Unlocked: " +
                                          Game.PlayerStats.ChallengeLastBossUnlocked);

                        Console.WriteLine("Eyeball Challenge Beaten: " + Game.PlayerStats.ChallengeEyeballBeaten);
                        Console.WriteLine("Skull Challenge Beaten: " + Game.PlayerStats.ChallengeSkullBeaten);
                        Console.WriteLine("Fireball Challenge Beaten: " + Game.PlayerStats.ChallengeFireballBeaten);
                        Console.WriteLine("Blob Challenge Beaten: " + Game.PlayerStats.ChallengeBlobBeaten);
                        Console.WriteLine("Last Boss Challenge Beaten: " + Game.PlayerStats.ChallengeLastBossBeaten);
                    }

                    if (reader.PeekChar() != -1) // Even more DLC added
                    {
                        Game.PlayerStats.ChallengeEyeballTimesUpgraded = reader.ReadSByte();
                        Game.PlayerStats.ChallengeSkullTimesUpgraded = reader.ReadSByte();
                        Game.PlayerStats.ChallengeFireballTimesUpgraded = reader.ReadSByte();
                        Game.PlayerStats.ChallengeBlobTimesUpgraded = reader.ReadSByte();
                        Game.PlayerStats.ChallengeLastBossTimesUpgraded = reader.ReadSByte();
                        Game.PlayerStats.RomanNumeral = reader.ReadString();
                        Game.PlayerStats.HasProsopagnosia = reader.ReadBoolean();
                        Game.PlayerStats.RevisionNumber = reader.ReadInt32();

                        if (LevelEV.ShowSaveLoadDebugText)
                        {
                            Console.WriteLine("Eyeball Challenge Times Upgraded: " +
                                              Game.PlayerStats.ChallengeEyeballTimesUpgraded);
                            Console.WriteLine("Skull Challenge Times Upgraded: " +
                                              Game.PlayerStats.ChallengeSkullTimesUpgraded);
                            Console.WriteLine("Fireball Challenge Times Upgraded: " +
                                              Game.PlayerStats.ChallengeFireballTimesUpgraded);
                            Console.WriteLine("Blob Challenge Times Upgraded: " +
                                              Game.PlayerStats.ChallengeBlobTimesUpgraded);
                            Console.WriteLine("Last Boss Challenge Times Upgraded: " +
                                              Game.PlayerStats.ChallengeLastBossTimesUpgraded);
                            Console.WriteLine("Player Name Number: " + Game.PlayerStats.RomanNumeral);
                            Console.WriteLine("Player has Prosopagnosia: " + Game.PlayerStats.HasProsopagnosia);
                        }

                        if (reader.PeekChar() != -1) // Even more...
                        {
                            Game.PlayerStats.ArchitectUsed = reader.ReadBoolean();

                            if (LevelEV.ShowSaveLoadDebugText)
                            {
                                Console.WriteLine("Architect used: " + Game.PlayerStats.ArchitectUsed);
                            }
                        }
                    }

                    Console.WriteLine("///// DLC DATA - LOADING COMPLETE /////");
                }
                else
                {
                    Console.WriteLine("///// NO DLC DATA FOUND - SKIPPED LOADING /////");
                }

                reader.Close();
            }

            stream.Close();
        }
    }

    private void LoadUpgradeData()
    {
        using (var stream = _storageContainer.OpenFile(
                   "Profile" + Game.GameConfig.ProfileSlot + "/" + FileNameUpgrades, FileMode.Open, FileAccess.Read,
                   FileShare.Read))
        {
            using (var reader = new BinaryReader(stream))
            {
                if (LevelEV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("\nLoading Equipment States");
                    Console.WriteLine("\nLoading Standard Blueprints");
                }

                List<byte[]> blueprintArray = Game.PlayerStats.GetBlueprintArray;
                for (var i = 0; i < EquipmentCategoryType.TOTAL; i++)
                {
                    for (var k = 0; k < EquipmentBaseType.TOTAL; k++)
                    {
                        blueprintArray[i][k] = reader.ReadByte();
                        if (LevelEV.ShowSaveLoadDebugText)
                        {
                            Console.Write(" " + blueprintArray[i][k]);
                        }
                    }

                    if (LevelEV.ShowSaveLoadDebugText)
                    {
                        Console.Write("\n");
                    }
                }

                if (LevelEV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("\nLoading Ability Blueprints");
                }

                List<byte[]> abilityBPArray = Game.PlayerStats.GetRuneArray;
                for (var i = 0; i < EquipmentCategoryType.TOTAL; i++)
                {
                    for (var k = 0; k < EquipmentAbilityType.TOTAL; k++)
                    {
                        abilityBPArray[i][k] = reader.ReadByte();
                        if (LevelEV.ShowSaveLoadDebugText)
                        {
                            Console.Write(" " + abilityBPArray[i][k]);
                        }
                    }

                    if (LevelEV.ShowSaveLoadDebugText)
                    {
                        Console.Write("\n");
                    }
                }

                if (LevelEV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("\nLoading Equipped Standard Items");
                }

                var equippedArray = Game.PlayerStats.GetEquippedArray;
                for (var i = 0; i < EquipmentCategoryType.TOTAL; i++)
                {
                    equippedArray[i] = reader.ReadSByte();
                    if (LevelEV.ShowSaveLoadDebugText)
                    {
                        Console.Write(" " + equippedArray[i]);
                    }
                }

                if (LevelEV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("\nLoading Equipped Abilities");
                }

                var equippedAbilityArray = Game.PlayerStats.GetEquippedRuneArray;
                for (var i = 0; i < EquipmentCategoryType.TOTAL; i++)
                {
                    equippedAbilityArray[i] = reader.ReadSByte();
                    if (LevelEV.ShowSaveLoadDebugText)
                    {
                        Console.Write(" " + equippedAbilityArray[i]);
                    }
                }

                SkillObj[] traitArray = SkillSystem.GetSkillArray();
                if (LevelEV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("\nLoading Traits");
                }

                SkillSystem.ResetAllTraits(); // Reset all traits first.
                Game.PlayerStats.CurrentLevel = 0; // Reset player level.
                for (var i = 0; i < (int)SkillType.Divider - 2; i++) //The starting 2 traits are null and filler.
                {
                    var traitLevel = reader.ReadInt32();
                    for (var k = 0; k < traitLevel; k++)
                    {
                        SkillSystem.LevelUpTrait(traitArray[i], false);
                    }

                    if (LevelEV.ShowSaveLoadDebugText)
                    {
                        Console.Write(" " + traitArray[i].CurrentLevel);
                    }
                }

                reader.Close();

                Game.ScreenManager.Player.UpdateEquipmentColours();
            }

            stream.Close();

            // These checks probably aren't necessary anymore.
            // Your game was corrupted if you have double jump equipped but no enchantress unlocked.
            //if (Game.PlayerStats.GetNumberOfEquippedRunes(EquipmentAbilityType.DoubleJump) > 0 && SkillSystem.GetSkill(SkillType.Enchanter).CurrentLevel < 1 && LevelEV.CREATE_RETAIL_VERSION == true)
            //    throw new Exception("Corrupted Save file");

            // Another check for corruption on the skill tree
            //bool possibleCorruption = false;
            //List<FamilyTreeNode> familyTree = Game.PlayerStats.FamilyTreeArray;
            //foreach (FamilyTreeNode node in familyTree)
            //{
            //    if (node.Class > ClassType.Assassin)
            //    {
            //        possibleCorruption = true;
            //        break;
            //    }
            //}

            //// If you ever find a class that is normally locked in the family tree, and the smithy is not unlocked, then the file is corrupt.
            //if (possibleCorruption == true && SkillSystem.GetSkill(SkillType.Smithy).CurrentLevel < 1 && LevelEV.CREATE_RETAIL_VERSION == true)
            //    throw new Exception("Corrupted Save file");
        }
    }

    public ProceduralLevelScreen LoadMap()
    {
        GetStorageContainer();
        ProceduralLevelScreen loadedLevel = null;
        using (var stream = _storageContainer.OpenFile("Profile" + Game.GameConfig.ProfileSlot + "/" + FileNameMap,
                   FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            using (var reader = new BinaryReader(stream))
            {
                var roomSize = reader.ReadInt32();
                var roomList = new Vector4[roomSize];
                var roomColorList = new Vector3[roomSize];

                for (var i = 0; i < roomSize; i++)
                {
                    roomList[i].W = reader.ReadInt32(); // Reading the pool index.
                    roomList[i].X = reader.ReadByte(); // Reading the level type.
                    roomList[i].Y = reader.ReadInt32(); // Reading the level's X pos.
                    roomList[i].Z = reader.ReadInt32(); // Reading the level's Y pos.

                    roomColorList[i].X = reader.ReadByte(); // Reading RGB
                    roomColorList[i].Y = reader.ReadByte();
                    roomColorList[i].Z = reader.ReadByte();
                }

                loadedLevel = LevelBuilder2.CreateLevel(roomList, roomColorList);

                var numEnemies = reader.ReadInt32(); // Reading the number of enemies in the game.
                var enemyList = new List<byte>();
                for (var i = 0; i < numEnemies; i++)
                {
                    enemyList.Add(reader.ReadByte()); // Reading the enemy type and storing it into the array.
                }

                var enemyDifficultyList = new List<byte>();
                for (var i = 0; i < numEnemies; i++) // Reading the enemy difficulty and storing it in an array.
                {
                    enemyDifficultyList.Add(reader.ReadByte());
                }

                LevelBuilder2.OverrideProceduralEnemies(loadedLevel, enemyList.ToArray(),
                    enemyDifficultyList.ToArray());

                reader.Close();
            }

            stream.Close();
        }

        _storageContainer.Dispose();

        return loadedLevel;
    }

    private void LoadMapData(ProceduralLevelScreen createdLevel)
    {
        //createdLevel.InitializeChests(true); // Can't remember why this was put here.  It was disabled because it was shifting chests twice, screwing up their positions.

        using (var stream = _storageContainer.OpenFile(
                   "Profile" + Game.GameConfig.ProfileSlot + "/" + FileNameMapData, FileMode.Open, FileAccess.Read,
                   FileShare.Read))
        {
            using (var reader = new BinaryReader(stream))
            {
                // Loading room visited state.
                var numRooms = reader.ReadInt32();
                var roomsVisited = new List<bool>();
                for (var i = 0; i < numRooms; i++)
                {
                    roomsVisited.Add(reader.ReadBoolean());
                }

                // Loading bonus room states.
                var numBonusRooms = reader.ReadInt32();
                var bonusRoomStates = new List<bool>();
                for (var i = 0; i < numBonusRooms; i++)
                {
                    bonusRoomStates.Add(reader.ReadBoolean());
                }

                // Loading bonus room data.
                var bonusRoomData = new List<int>();
                for (var i = 0; i < numBonusRooms; i++)
                {
                    bonusRoomData.Add(reader.ReadInt32());
                }

                // Loading chest types
                var numChests = reader.ReadInt32();
                var chestTypes = new List<byte>();
                for (var i = 0; i < numChests; i++)
                {
                    chestTypes.Add(reader.ReadByte());
                }

                // Loading chest states
                numChests = reader.ReadInt32();
                var chestStates = new List<bool>();
                for (var i = 0; i < numChests; i++)
                {
                    chestStates.Add(reader.ReadBoolean());
                }

                // Loading fairy chest states
                numChests = reader.ReadInt32();
                var fairyChestStates = new List<bool>();
                for (var i = 0; i < numChests; i++)
                {
                    fairyChestStates.Add(reader.ReadBoolean());
                }

                // Loading enemy states
                var numEnemies = reader.ReadInt32();
                var enemyStates = new List<bool>();
                for (var i = 0; i < numEnemies; i++)
                {
                    enemyStates.Add(reader.ReadBoolean());
                }

                // Loading breakable object states
                var numBreakables = reader.ReadInt32();
                var breakableStates = new List<bool>();
                for (var i = 0; i < numBreakables; i++)
                {
                    breakableStates.Add(reader.ReadBoolean());
                }

                //int roomCounter = 0;
                var bonusRoomCounter = 0;
                var chestTypeCounter = 0;
                var chestCounter = 0;
                var fairyChestCounter = 0;
                var enemyCounter = 0;
                var breakablesCounter = 0;

                //foreach (RoomObj room in createdLevel.RoomList)
                //{
                //    if (room.Name != "Boss")
                //    {
                //        int counter = 0;
                //        foreach (GameObj obj in room.GameObjList)
                //        {
                //            if (obj is ChestObj)
                //                counter++;
                //        }
                //        Console.WriteLine(counter);
                //    }
                //}

                foreach (var room in createdLevel.RoomList)
                {
                    //DO NOT set the state of rooms visited yet. This must be done AFTER chest states, otherwise the map won't update properly.

                    // Setting the state of bonus rooms.
                    if (numBonusRooms > 0)
                    {
                        var bonusRoom = room as BonusRoomObj;
                        if (bonusRoom != null)
                        {
                            var bonusRoomState = bonusRoomStates[bonusRoomCounter];
                            var roomData = bonusRoomData[bonusRoomCounter];

                            bonusRoomCounter++;

                            if (bonusRoomState)
                            {
                                bonusRoom.RoomCompleted = true;
                            }

                            bonusRoom.ID = roomData;
                        }
                    }

                    // Setting the state of enemies.
                    // Only bring enemies back to life if you are locking the castle (i.e. not reloading a file but starting a new lineage).
                    if (numEnemies > 0)
                    {
                        if (Game.PlayerStats.LockCastle == false)
                        {
                            if (room.Name != "Boss" && room.Name != "ChallengeBoss") // && room.Name != "Bonus")
                            {
                                foreach (var enemy in room.EnemyList)
                                {
                                    var enemyState = enemyStates[enemyCounter];
                                    enemyCounter++;

                                    if (enemyState)
                                    {
                                        enemy.KillSilently();
                                    }
                                }
                            }
                        }
                    }

                    // Setting the states of chests.
                    if (room.Name != "Bonus" && room.Name != "Boss" && room.Name != "Compass" &&
                        room.Name != "ChallengeBoss") // Don't save bonus room chests or breakables.
                    {
                        foreach (var obj in room.GameObjList)
                        {
                            // Only save breakable states if the castle is not locked.
                            if (Game.PlayerStats.LockCastle == false)
                            {
                                if (numBreakables > 0)
                                {
                                    var breakable = obj as BreakableObj;
                                    if (breakable != null)
                                    {
                                        var breakableState = breakableStates[breakablesCounter];
                                        breakablesCounter++;
                                        if (breakableState)
                                        {
                                            breakable.ForceBreak();
                                        }
                                    }
                                }
                            }

                            //if (numChests > 0)
                            //{
                            var chest = obj as ChestObj;
                            if (chest != null)
                            {
                                chest.IsProcedural = false;
                                var chestType = chestTypes[chestTypeCounter];
                                chestTypeCounter++;
                                chest.ChestType = chestType;

                                var chestState = chestStates[chestCounter];
                                chestCounter++;

                                if (chestState)
                                {
                                    chest.ForceOpen();
                                }

                                // Only reset fairy chests if you are locking the castle (i.e. not reloading a file but starting a new lineage).
                                if (Game.PlayerStats.LockCastle == false)
                                {
                                    var fairyChest = chest as FairyChestObj;
                                    if (fairyChest != null)
                                    {
                                        var fairyChestState = fairyChestStates[fairyChestCounter];
                                        fairyChestCounter++;

                                        if (fairyChestState)
                                        {
                                            fairyChest.SetChestFailed(true);
                                        }
                                    }
                                }
                            }
                            //}
                        }
                    }
                }

                if (numRooms > 0)
                {
                    var roomsVisitedList = new List<RoomObj>();
                    // Setting rooms visited states after the chests have been set.
                    //foreach (RoomObj room in createdLevel.RoomList)
                    //{
                    //    bool roomState = roomsVisited[roomCounter];
                    //    roomCounter++;

                    //    if (roomState == true)
                    //        roomsVisitedList.Add(room);
                    //}

                    var roomsVisitedCount = roomsVisited.Count;
                    for (var i = 0; i < roomsVisitedCount; i++)
                    {
                        if (roomsVisited[i])
                        {
                            roomsVisitedList.Add(createdLevel.RoomList[i]);
                        }
                    }

                    createdLevel.MapRoomsUnveiled = roomsVisitedList;
                }

                reader.Close();
            }

            stream.Close();
        }
    }

    private void LoadLineageData()
    {
        using var stream = _storageContainer.OpenFile($"Profile{Game.GameConfig.ProfileSlot}/{FileNameLineage}", FileMode.Open, FileAccess.Read, FileShare.Read);
        using (var reader = new BinaryReader(stream))
        {
            Console.WriteLine(@"///// PLAYER LINEAGE DATA - BEGIN LOADING /////");

            // Loading the currently created branch.
            var loadedBranches = new List<PlayerLineageData>();
            var numChildren = reader.ReadInt32();

            for (var i = 0; i < numChildren; i++)
            {
                var data = new PlayerLineageData();

                data.Name = reader.ReadString();
                data.Spell = reader.ReadByte();
                data.Class = reader.ReadByte();
                data.HeadPiece = reader.ReadByte();
                data.ChestPiece = reader.ReadByte();
                data.ShoulderPiece = reader.ReadByte();
                data.Age = reader.ReadByte();
                data.ChildAge = reader.ReadByte();
                data.Traits = (reader.ReadByte(), reader.ReadByte());
                data.IsFemale = reader.ReadBoolean();

                if (Game.PlayerStats.RevisionNumber > 0)
                {
                    data.RomanNumeral = reader.ReadString();
                }

                loadedBranches.Add(data);
            }

            if (loadedBranches.Count > 0)
            {
                // Loading the CurrentBranches into Game.PlayerStats.
                Game.PlayerStats.CurrentBranches = loadedBranches;

                if (LevelEV.ShowSaveLoadDebugText)
                {
                    Console.WriteLine("Loading Current Branch Lineage Data");
                    List<PlayerLineageData> currentBranches = Game.PlayerStats.CurrentBranches;
                    for (var i = 0; i < numChildren; i++)
                    {
                        Console.WriteLine("Player Name: " + currentBranches[i].Name);
                        Console.WriteLine("Spell: " + currentBranches[i].Name);
                        Console.WriteLine("Class: " + currentBranches[i].Name);
                        Console.WriteLine("Head Piece: " + currentBranches[i].HeadPiece);
                        Console.WriteLine("Chest Piece: " + currentBranches[i].ChestPiece);
                        Console.WriteLine("Shoulder Piece: " + currentBranches[i].ShoulderPiece);
                        Console.WriteLine("Player Age: " + currentBranches[i].Age);
                        Console.WriteLine("Player Child Age: " + currentBranches[i].ChildAge);
                        Console.WriteLine("Traits: " + currentBranches[i].Traits.Trait1 + ", " +
                                          currentBranches[i].Traits.Trait2);
                        Console.WriteLine("Is Female: " + currentBranches[i].IsFemale);
                        if (Game.PlayerStats.RevisionNumber > 0)
                        {
                            Console.WriteLine("Roman Number:" + currentBranches[i].RomanNumeral);
                        }
                    }
                }
            }

            ////////////////////////////////////////

            // Loading family tree info

            var familyTree = new List<FamilyTreeNode>();
            var numBranches = reader.ReadInt32();

            for (var i = 0; i < numBranches; i++)
            {
                var data = new FamilyTreeNode();
                data.Name = reader.ReadString();
                data.Age = reader.ReadByte();
                data.Class = reader.ReadByte();
                data.HeadPiece = reader.ReadByte();
                data.ChestPiece = reader.ReadByte();
                data.ShoulderPiece = reader.ReadByte();
                data.NumEnemiesBeaten = reader.ReadInt32();
                data.BeatenABoss = reader.ReadBoolean();
                data.Traits.Trait1 = reader.ReadByte();
                data.Traits.Trait2 = reader.ReadByte();
                data.IsFemale = reader.ReadBoolean();
                if (Game.PlayerStats.RevisionNumber > 0)
                {
                    data.RomanNumeral = reader.ReadString();
                }

                familyTree.Add(data);
            }

            if (familyTree.Count > 0)
            {
                // Loading the created Family Tree list into Game.PlayerStats.
                Game.PlayerStats.FamilyTreeArray = familyTree;

                if (LevelEV.ShowSaveLoadDebugText)
                {
                    List<FamilyTreeNode> familyTreeArray = Game.PlayerStats.FamilyTreeArray;
                    Console.WriteLine("Loading Family Tree Data");
                    Console.WriteLine("Number of Branches: " + numBranches);
                    for (var i = 0; i < numBranches; i++)
                    {
                        Console.WriteLine("/// Saving branch");
                        Console.WriteLine("Name: " + familyTreeArray[i].Name);
                        Console.WriteLine("Age: " + familyTreeArray[i].Age);
                        Console.WriteLine("Class: " + familyTreeArray[i].Class);
                        Console.WriteLine("Head Piece: " + familyTreeArray[i].HeadPiece);
                        Console.WriteLine("Chest Piece: " + familyTreeArray[i].ChestPiece);
                        Console.WriteLine("Shoulder Piece: " + familyTreeArray[i].ShoulderPiece);
                        Console.WriteLine("Number of Enemies Beaten: " + familyTreeArray[i].NumEnemiesBeaten);
                        Console.WriteLine("Beaten a Boss: " + familyTreeArray[i].BeatenABoss);
                        Console.WriteLine("Traits: " + familyTreeArray[i].Traits.Trait1 + ", " +
                                          familyTreeArray[i].Traits.Trait2);
                        Console.WriteLine("Is Female: " + familyTreeArray[i].IsFemale);
                        if (Game.PlayerStats.RevisionNumber > 0)
                        {
                            Console.WriteLine("Roman Numeral: " + familyTreeArray[i].RomanNumeral);
                        }
                    }
                }
            }

            ///////////////////////////////////////////
            Console.WriteLine(@"///// PLAYER LINEAGE DATA - LOAD COMPLETE /////");

            reader.Close();
        }

        stream.Close();
    }

    public ProfileSaveHeader GetSaveHeader(byte profile)
    {
        var header = new ProfileSaveHeader();

        GetStorageContainer();

        // Archipelago
        if (_storageContainer.FileExists($"Profile{profile}/{FileNameArchipelago}"))
        {
            using var stream = _storageContainer.OpenFile($"Profile{profile}/{FileNameArchipelago}", FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new BinaryReader(stream);

            reader.ReadByte(); // revision #
            header.MultiWorld = reader.ReadBoolean();
            header.Slot = reader.ReadInt32();
            header.SeedName = reader.ReadString();
            header.GeneratorVersion = reader.ReadString();
            header.GameVersion = reader.ReadString();
            header.Address = reader.ReadString();
            header.SlotName = reader.ReadString();
            header.Password = reader.ReadString();

            header.LocationCounts[1] = (reader.ReadInt32(), reader.ReadInt32());
            header.LocationCounts[2] = (reader.ReadInt32(), reader.ReadInt32());
            header.LocationCounts[3] = (reader.ReadInt32(), reader.ReadInt32());
            header.LocationCounts[4] = (reader.ReadInt32(), reader.ReadInt32());
            header.LocationCounts[0] = (0, reader.ReadByte());
        }

        // Player
        if (_storageContainer.FileExists($"Profile{profile}/{FileNamePlayer}"))
        {
            using var stream = _storageContainer.OpenFile($"Profile{profile}/{FileNamePlayer}", FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new BinaryReader(stream);

            header.Gold = reader.ReadInt32(); // Gold
            reader.ReadInt32(); // Health
            reader.ReadInt32(); // Mana
            reader.ReadByte(); // Age
            reader.ReadByte(); // Child Age
            reader.ReadByte(); // Spell
            header.PlayerClass = reader.ReadByte();
            reader.ReadByte(); // Special Item
            reader.ReadByte(); // TraitX
            reader.ReadByte(); // TraitY
            header.PlayerName = reader.ReadString();

            reader.ReadByte(); // Head Piece
            reader.ReadByte(); // Shoulder Piece
            reader.ReadByte(); // Chest Piece
            header.LocationCounts[0].Checked = reader.ReadByte(); // Diary Entry
            reader.ReadInt32(); // Bonus Health
            reader.ReadInt32(); // Bonus Strength
            reader.ReadInt32(); // Bonus Mana
            reader.ReadInt32(); // Bonus Defense
            reader.ReadInt32(); // Bonus Weight
            reader.ReadInt32(); // Bonus Magic

            // Reading lich stats.
            reader.ReadInt32();
            reader.ReadInt32();
            reader.ReadSingle();

            // Reading boss progress states
            reader.ReadBoolean();
            reader.ReadBoolean();
            reader.ReadBoolean();
            reader.ReadBoolean();
            reader.ReadBoolean();
            reader.ReadBoolean();

            // Reading new game plus progress
            reader.ReadInt32();
            reader.ReadInt32();

            // Loading misc flags
            reader.ReadBoolean();
            reader.ReadBoolean();
            reader.ReadBoolean();

            reader.ReadBoolean();
            reader.ReadBoolean();
            reader.ReadBoolean();
            reader.ReadBoolean();
            reader.ReadBoolean();
            header.IsDead = reader.ReadBoolean();
            reader.ReadBoolean();
            reader.ReadBoolean();
            header.IsFemale = reader.ReadBoolean();
        }

        // Equipment & Skills
        if (_storageContainer.FileExists($"Profile{profile}/{FileNameUpgrades}"))
        {
            using var stream = _storageContainer.OpenFile($"Profile{profile}/{FileNameUpgrades}", FileMode.Open, FileAccess.Read, FileShare.Read);
            using var reader = new BinaryReader(stream);

            for (var category = 0; category < EquipmentCategoryType.TOTAL; category++)
            {
                header.Blueprints[category] = new byte[EquipmentBaseType.TOTAL];
                for (var @base = 0; @base < EquipmentBaseType.TOTAL; @base++)
                {
                    header.Blueprints[category][@base] = reader.ReadByte();
                }
            }

            for (var category = 0; category < EquipmentCategoryType.TOTAL; category++)
            {
                header.Runes[category] = new byte[EquipmentAbilityType.TOTAL];
                for (var @base = 0; @base < EquipmentAbilityType.TOTAL; @base++)
                {
                    header.Runes[category][@base] = reader.ReadByte();
                }
            }

            for (var category = 0; category < EquipmentCategoryType.TOTAL; category++)
            {
                reader.ReadSByte();
            }

            for (var category = 0; category < EquipmentCategoryType.TOTAL; category++)
            {
                reader.ReadSByte();
            }

            var levelCounter = 0;
            for (var skill = 0; skill < (int)SkillType.Divider - 2; skill++) //The starting 2 traits are null and filler.
            {
                var traitLevel = reader.ReadInt32();
                levelCounter += traitLevel;

                // Haggling
                if (skill == (int)SkillType.PricesDown)
                {
                    header.CharonFee -= 10 * traitLevel;
                }
            }

            header.Level = levelCounter;
        }

        _storageContainer.Dispose();
        _storageContainer = null;

        return header;
    }

    public bool FileExists(SaveType saveType, byte profile = 0)
    {
        if (profile == 0)
        {
            profile = Game.GameConfig.ProfileSlot;
        }

        GetStorageContainer();

        var fileExists = saveType switch
        {
            SaveType.PlayerData  => _storageContainer.FileExists($"Profile{profile}/{FileNamePlayer}"),
            SaveType.UpgradeData => _storageContainer.FileExists($"Profile{profile}/{FileNameUpgrades}"),
            SaveType.Map         => _storageContainer.FileExists($"Profile{profile}/{FileNameMap}"),
            SaveType.MapData     => _storageContainer.FileExists($"Profile{profile}/{FileNameMapData}"),
            SaveType.Lineage     => _storageContainer.FileExists($"Profile{profile}/{FileNameLineage}"),
            SaveType.Archipelago => _storageContainer.FileExists($"Profile{profile}/{FileNameArchipelago}"),
            _                    => false,
        };

        if (_storageContainer is { IsDisposed: false })
        {
            return fileExists;
        }

        _storageContainer.Dispose();
        _storageContainer = null;

        return fileExists;
    }

    private void GetStorageContainer()
    {
        if (_storageContainer is { IsDisposed: false })
        {
            return;
        }

        var asyncResult = StorageDevice.BeginShowSelector(null, null);
        asyncResult.AsyncWaitHandle.WaitOne();
        var storageDevice = StorageDevice.EndShowSelector(asyncResult);
        asyncResult.AsyncWaitHandle.Close();
        asyncResult = storageDevice.BeginOpenContainer(StorageContainerName, null, null);
        asyncResult.AsyncWaitHandle.WaitOne();
        _storageContainer = storageDevice.EndOpenContainer(asyncResult);
        asyncResult.AsyncWaitHandle.Close();
    }

    // This code was added to create profile directories in case the computer doesn't have them.
    // Older versions of this game do not use directories.
    private void PerformDirectoryCheck()
    {
        GetStorageContainer();

        // Creating the directories.
        if (_storageContainer.DirectoryExists("Profile1") == false)
        {
            _storageContainer.CreateDirectory("Profile1");

            // Copying all files from the base directory into Profile1.
            CopyFile(_storageContainer, FileNamePlayer, "Profile1");
            CopyFile(_storageContainer, "AutoSave_" + FileNamePlayer, "Profile1");

            CopyFile(_storageContainer, FileNameUpgrades, "Profile1");
            CopyFile(_storageContainer, "AutoSave_" + FileNameUpgrades, "Profile1");

            CopyFile(_storageContainer, FileNameMap, "Profile1");
            CopyFile(_storageContainer, "AutoSave_" + FileNameMap, "Profile1");

            CopyFile(_storageContainer, FileNameMapData, "Profile1");
            CopyFile(_storageContainer, "AutoSave_" + FileNameMapData, "Profile1");

            CopyFile(_storageContainer, FileNameLineage, "Profile1");
            CopyFile(_storageContainer, "AutoSave_" + FileNameLineage, "Profile1");

            CopyFile(_storageContainer, FileNameArchipelago, "Profile1");
            CopyFile(_storageContainer, "AutoSave_" + FileNameArchipelago, "Profile1");
        }

        if (_storageContainer.DirectoryExists("Profile2") == false)
        {
            _storageContainer.CreateDirectory("Profile2");
        }

        if (_storageContainer.DirectoryExists("Profile3") == false)
        {
            _storageContainer.CreateDirectory("Profile3");
        }

        if (_storageContainer.DirectoryExists("Profile4") == false)
        {
            _storageContainer.CreateDirectory("Profile4");
        }

        if (_storageContainer.DirectoryExists("Profile5") == false)
        {
            _storageContainer.CreateDirectory("Profile5");
        }

        _storageContainer.Dispose();
        _storageContainer = null;
    }

    private static void CopyFile(StorageContainer storageContainer, string fileName, string profileName)
    {
        if (!storageContainer.FileExists(fileName))
        {
            return;
        }

        var fileToCopy = storageContainer.OpenFile(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        var copiedFile = storageContainer.CreateFile(profileName + "/" + fileName);
        fileToCopy.CopyTo(copiedFile);
        fileToCopy.Close();
        copiedFile.Close();
    }
}

public enum SaveType
{
    None,
    PlayerData,
    UpgradeData,
    Map,
    MapData,
    Lineage,
    Archipelago,
}

public record ProfileSaveHeader
{
    public string PlayerName { get; set; } // `null` means failed to load or no save file
    public byte PlayerClass { get; set; }
    public bool IsFemale { get; set; }
    public int Level { get; set; }
    public bool IsDead { get; set; }

    public bool MultiWorld { get; set; } = true;
    public string Address { get; set; } = "";
    public string SlotName { get; set; } = "";
    public string Password { get; set; } = "";

    public int Slot { get; set; }
    public string SeedName { get; set; } = "???";
    public string GeneratorVersion { get; set; } = "???";
    public string GameVersion { get; set; } = "???";
    public int Gold { get; set; }
    public (int Checked, int Total)[] LocationCounts { get; set; } = [(0, 0), (0, 0), (0, 0), (0, 0), (0, 0)];
    public byte[][] Blueprints { get; set; } = new byte[EquipmentCategoryType.TOTAL][];
    public byte[][] Runes { get; set; } = new byte[EquipmentCategoryType.TOTAL][];
    public int ArchitectFee { get; set; } = 40;
    public int CharonFee { get; set; } = 100;
}
