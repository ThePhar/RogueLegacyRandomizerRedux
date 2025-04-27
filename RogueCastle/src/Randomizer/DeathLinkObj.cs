using DS2DEngine;

namespace RogueCastle.Randomizer;

public class DeathLinkObj : GameObj
{
    public readonly string Cause;
    
    public DeathLinkObj(string player, string cause = "")
    {
        Name = player;
        Cause = cause;
    }

    protected override GameObj CreateCloneInstance()
    {
        return new DeathLinkObj(Name, Cause);
    }
}