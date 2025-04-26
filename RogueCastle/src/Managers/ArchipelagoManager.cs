using System;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using RogueCastle.GameStructs;
using RogueCastle.Randomizer;

namespace RogueCastle.Managers;

public class ArchipelagoManager(Game game)
{
    private static readonly Version SupportedVersion = new(0, 6, 2);
    private readonly Game _game = game;
    private DeathLinkService _deathLinkService;
    private ArchipelagoSession _session;

    public ConnectionStatus Status { get; private set; }

    public SlotDataV1 SlotData { get; private set; }
    
    public string Password { get; private set; }

    public string SeedName => _session.RoomState.Seed;
    public int Slot => _session.Players.ActivePlayer.Slot;
    public string GeneratorVersion => _session.RoomState.GeneratorVersion.ToString();
    public string Address => _session.Socket.Uri.ToString();
    public string SlotName => _session.Players.ActivePlayer.Name;


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
                DialogueManager.AddText("MultiworldConnectFailure",
                    ["LOC_ID_RANDOMIZER_DIALOGUE_TITLE_1", "LOC_ID_RANDOMIZER_DIALOGUE_TITLE_1"],
                    ["LOC_ID_RANDOMIZER_DIALOGUE_TEXT_1", "LOC_ID_RANDOMIZER_DIALOGUE_TEXT_PURE"]);

                Game.ScreenManager.DialogueScreen.SetDialogue("MultiworldConnectFailure", failure!.Errors[0]);
                Game.ScreenManager.DisplayScreen(ScreenType.DIALOGUE, false);
                return false;
            }


            Password = password;
            Status = ConnectionStatus.Ready;
            var success = result as LoginSuccessful;
            
            // todo test for v2
            SlotData = new SlotDataV1(success!.SlotData);
            Console.WriteLine(SlotData);

            return true;
        }
        catch (Exception exception)
        {
            // Display issue
            DialogueManager.AddText("MultiworldConnectFailure",
                ["LOC_ID_RANDOMIZER_DIALOGUE_TITLE_1", "LOC_ID_RANDOMIZER_DIALOGUE_TITLE_1"],
                ["LOC_ID_RANDOMIZER_DIALOGUE_TEXT_1", "LOC_ID_RANDOMIZER_DIALOGUE_TEXT_PURE"]);

            Game.ScreenManager.DialogueScreen.SetDialogue("MultiworldConnectFailure",
                $"{exception.Message}");
            Game.ScreenManager.DisplayScreen(ScreenType.DIALOGUE, false);
            return false;
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
