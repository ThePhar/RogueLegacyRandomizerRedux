namespace RogueCastle.GameStructs;

public struct PlayerLineageData
{
    public string Name;
    public SpellType Spell;
    public byte Class;
    public byte Age;
    public byte ChildAge;
    public bool IsFemale;
    public byte HeadPiece;
    public byte ChestPiece;
    public byte ShoulderPiece;
    public Traits Traits;

    private string _romanNumeral;

    public string RomanNumeral
    {
        get => _romanNumeral ??= "";
        set => _romanNumeral = value;
    }
}
