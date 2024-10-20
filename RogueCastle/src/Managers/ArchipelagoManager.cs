using System;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Exceptions;
using RogueCastle.GameStructs;

namespace RogueCastle.Managers;

public class ArchipelagoManager(Game game)
{
    private static readonly Version SupportedVersion = new(0, 5, 1);
    private readonly Game _game = game;
    private DeathLinkService _deathLinkService;
    private ArchipelagoSession _session;

    public ConnectionStatus Status { get; private set; }

    public string SeedName => _session.RoomState.Seed;

    public int Slot => _session.Players.ActivePlayer.Slot;

    public string GeneratorVersion => _session.RoomState.GeneratorVersion.ToString();

    public async Task<bool> ConnectAsync(string address, string slotname, string password)
    {
        // Start fresh.
        Disconnect();

        _session = ArchipelagoSessionFactory.CreateSession(address);

        _session.Socket.ErrorReceived += OnError;
        _session.Socket.PacketReceived += OnPacketReceived;

        try
        {
            Status = ConnectionStatus.Processing;

            await _session.ConnectAsync();
            var result = await _session.LoginAsync(
                "Rogue Legacy",
                slotname,
                ItemsHandlingFlags.AllItems,
                SupportedVersion,
                password: password);

            if (!result.Successful)
            {
                var failure = result as LoginFailure;

                // Display issue
                DialogueManager.SetLanguage(DialogueManager.GetCurrentLanguage());
                DialogueManager.AddText("MultiworldConnectFailure", ["Failed to Connect", "Failed to Connect"], ["Unable to connect to Archipelago Server.", failure!.Errors[0]]);

                Game.ScreenManager.DialogueScreen.SetDialogue("MultiworldConnectFailure");
                Game.ScreenManager.DisplayScreen(ScreenType.DIALOGUE, false);
                return false;
            }

            Status = ConnectionStatus.Ready;
            return true;
        }
        catch (ArchipelagoSocketClosedException exception)
        {
            throw;
        }
        catch (ArchipelagoServerRejectedPacketException exception)
        {
            throw;
        }
    }

    public void Disconnect()
    {
        _session?.Socket.DisconnectAsync();
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

public enum ConnectionStatus
{
    Disconnected,
    Processing,
    Ready,
}
