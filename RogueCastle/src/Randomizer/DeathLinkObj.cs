using DS2DEngine;

namespace RogueCastle.Randomizer;

public class DeathLinkObj : GameObj
{
    public DeathLinkObj(string source)
    {
        Name = source;
    }

    protected override GameObj CreateCloneInstance()
    {
        return new DeathLinkObj(Name);
    }
}
