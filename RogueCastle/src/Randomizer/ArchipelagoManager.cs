using System;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;

namespace RogueCastle.Randomizer;

public class ArchipelagoManager(Game game)
{
    private static readonly Version          SupportedVersion = new(0, 5, 1);
    private readonly        Game             _game            = game;
    private                 DeathLinkService _deathLinkService;
    private                 DateTime         _lastDeath;

    private ArchipelagoSession _session;

    public SlotDataV3 SlotData { get; private set; }

    public LoginFailure TryConnect(string hostname, string username, string password)
    {
        _lastDeath = DateTime.MinValue;
        _session = ArchipelagoSessionFactory.CreateSession(hostname);

        _session.Socket.ErrorReceived += OnError;
        _session.Socket.PacketReceived += OnPacketReceived;

        var result = _session.TryConnectAndLogin(
            "Rogue Legacy",
            username,
            password: password,
            itemsHandlingFlags: ItemsHandlingFlags.AllItems,
            version: SupportedVersion
        );

        if (!result.Successful)
        {
            return result as LoginFailure;
        }

        _deathLinkService = _session.CreateDeathLinkService();

        BuildSlotData();
        return null;
    }

    public void Disconnect()
    {
        _session?.Socket.DisconnectAsync();
    }

    private void BuildSlotData()
    {
        SlotData = _session.DataStorage.GetSlotData<SlotDataV3>();

        _game.InitializeNameArray(true, SlotData.CharacterNamesLady);
        _game.InitializeNameArray(false, SlotData.CharacterNamesSir);

        Console.WriteLine(SlotData.ToString());
    }

    private static void OnError(Exception exception, string message)
    {
        Console.WriteLine(@$"[Error]: ${message}");
        Console.WriteLine(exception);
    }

    private static void OnPacketReceived(ArchipelagoPacketBase packet)
    {
        // TODO: This should probably be removed before a live-version goes out.
        Console.WriteLine(@$"[Packet]: Received a {packet.PacketType} packet.");
    }
}
