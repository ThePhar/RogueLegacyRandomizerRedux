using System;
using DS2DEngine;

namespace RogueCastle.Randomizer;

public class RandomizerStats : IDisposableObj
{
    public const byte SAVE_FORMAT_REVISION = 0;

    // Meta Data
    public bool Multiworld { get; set; } = true;
    public string SavedAddress { get; set; } = "";
    public string SavedSlotName { get; set; } = "";
    public string SavedPassword { get; set; } = "";

    // Generation
    public int BrownChestsChecked { get; set; }
    public int BrownChestsTotal { get; set; }
    public int SilverChestsChecked { get; set; }
    public int SilverChestsTotal { get; set; }
    public int GoldChestsChecked { get; set; }
    public int GoldChestsTotal { get; set; }
    public int FairyChestsChecked { get; set; }
    public int FairyChestsTotal { get; set; }
    public byte DiaryEntryTotal { get; set; } // Diary Entry Checked is tracked in PlayerStats

    public bool IsDisposed { get; set; }

    public void Dispose()
    {
        if (IsDisposed)
        {
            return;
        }
    }
}
