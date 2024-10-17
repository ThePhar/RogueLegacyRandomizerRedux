using Microsoft.Xna.Framework;

namespace RogueCastle.GameStructs;

public static class PlayerPart
{
    public const int NONE        =-1;
    public const int WINGS       = 0;
    public const int CAPE        = 1;
    public const int LEGS        = 2;
    public const int SHOULDER_B  = 3;
    public const int CHEST       = 4;
    public const int BOOBS       = 5;
    public const int ARMS        = 6;
    public const int HAIR        = 7;
    public const int NECK        = 8;
    public const int SHOULDER_A  = 9;
    public const int SWORD1      = 10;
    public const int SWORD2      = 11;
    public const int HEAD        = 12;
    public const int BOWTIE      = 13;
    public const int GLASSES     = 14;
    public const int EXTRA       = 15;
    public const int LIGHT       = 16;

    public const int NUM_HEAD_PIECES = 5;
    public const int NUM_CHEST_PIECES = 5;
    public const int NUM_SHOULDER_PIECES = 5;

    public const int DRAGON_HELM = 6;
    public const int INTRO_HELM = 7;

    public static Vector3 GetPartIndices(int category)
    {
        return category switch
        {
            EquipmentCategoryType.CAPE  => new Vector3(CAPE, NECK, NONE),
            EquipmentCategoryType.CHEST => new Vector3(CHEST, SHOULDER_B, SHOULDER_A),
            EquipmentCategoryType.HELM  => new Vector3(HEAD, HAIR, NONE),
            EquipmentCategoryType.LIMBS => new Vector3(ARMS, LEGS, NONE),
            EquipmentCategoryType.SWORD => new Vector3(SWORD1, SWORD2, NONE),
            _                           => new Vector3(-1, -1, -1),
        };
    }
}
