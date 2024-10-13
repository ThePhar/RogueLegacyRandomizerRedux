using System;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Packets;
using Newtonsoft.Json;

namespace RogueCastle.Randomizer;

public class ArchipelagoManager
{
    private static readonly Version SupportedVersion = new(0, 5, 0);

    private ArchipelagoSession _session;
    private DeathLinkService   _deathLinkService;
    private DateTime           _lastDeath;
    
    public SlotData SlotData { get; private set; }

    public LoginFailure TryConnect()
    {
        _lastDeath = DateTime.MinValue;
        _session = ArchipelagoSessionFactory.CreateSession("wss://archipelago.gg:38373");

        _session.Socket.ErrorReceived += OnError;
        _session.Socket.PacketReceived += OnPacketReceived;

        var result = _session.TryConnectAndLogin(
            game: "Rogue Legacy", 
            name: "Phar", 
            itemsHandlingFlags: ItemsHandlingFlags.AllItems,
            version: SupportedVersion
        );

        if (!result.Successful)
        {
            return result as LoginFailure;
        }

        _deathLinkService = _session.CreateDeathLinkService();

        SlotData = _session.DataStorage.GetSlotData<SlotData>();
        
        return null;
    }

    private static void OnError(Exception exception, string message)
    {
        Console.WriteLine(@$"[Error]: ${message}");
        Console.WriteLine(exception);
    }

    private static void OnPacketReceived(ArchipelagoPacketBase packet)
    {
        Console.WriteLine(@$"[Packet]: Received a {packet.PacketType} packet.");
    }
}
