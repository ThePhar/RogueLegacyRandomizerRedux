namespace RogueCastle.GameStructs;

public struct FamilyTreeNode
{
    public string Name;
    public byte Age;
    public byte ChildAge;
    public byte Class;
    public byte HeadPiece;
    public byte ChestPiece;
    public byte ShoulderPiece;
    public int NumEnemiesBeaten;
    public bool BeatenABoss;
    public bool IsFemale;
    public Traits Traits;
    private string _romanNumeral;

    public string RomanNumeral
    {
        get => _romanNumeral ??= "";
        set => _romanNumeral = value;
    }
}
