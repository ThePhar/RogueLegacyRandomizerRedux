using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.BounceFeatures.DeathLink;
using Archipelago.MultiClient.Net.Enums;
using Archipelago.MultiClient.Net.Helpers;
using Archipelago.MultiClient.Net.Models;
using DS2DEngine;
using RogueCastle.GameStructs;
using RogueCastle.Randomizer;
using Color = Microsoft.Xna.Framework.Color;

namespace RogueCastle.Managers;

public class ArchipelagoManager {
    private static readonly Version SupportedVersion = new(0, 6, 2);
    private DeathLinkService _deathLinkService;
    private ArchipelagoSession _session;

    public ConnectionStatus Status { get; private set; }
    public SlotDataV1 SlotData { get; private set; }
    public string Password { get; private set; }
    public DeathLink QueuedDeathLink { get; private set; }
    public ConcurrentQueue<ItemInfo> ItemQueue { get; private set; } = new();

    public string SeedName => _session.RoomState.Seed;
    public int Slot => _session.Players.ActivePlayer.Slot;
    public string GeneratorVersion => _session.RoomState.GeneratorVersion.ToString();
    public string Address => _session.Socket.Uri.ToString();
    public string SlotName => _session.Players.ActivePlayer.Name;

    public async Task<bool> ConnectAsync(string address, string slotname, string password) {
        // Start fresh.
        Disconnect();

        _session = ArchipelagoSessionFactory.CreateSession(address);
        _deathLinkService = _session.CreateDeathLinkService();

        _session.Socket.ErrorReceived += OnError;
        _session.Socket.PacketReceived += OnPacketReceived;
        _session.Items.ItemReceived += OnItemReceived;
        _deathLinkService.OnDeathLinkReceived += OnDeathLink;

        try {
            Status = ConnectionStatus.Processing;

            await _session.ConnectAsync();
            var result = await _session.LoginAsync(
                "Rogue Legacy",
                slotname,
                ItemsHandlingFlags.AllItems,
                SupportedVersion,
                password: password,
                tags: ["NoText"]);

            if (!result.Successful) {
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
            if (SlotData.DeathLink) {
                _deathLinkService.EnableDeathLink();
            }

            return true;
        } catch (Exception exception) {
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

    public void Disconnect() {
        _session?.Socket.DisconnectAsync();

        Status = ConnectionStatus.Disconnected;
        SlotData = null;
        Password = "";
        QueuedDeathLink = null;
        ItemQueue = new ConcurrentQueue<ItemInfo>();
    }

    public Color GetItemColor(ItemInfo item) {
        if (item.Flags.HasFlag(ItemFlags.Advancement | ItemFlags.NeverExclude)) {
            return Color.Yellow;
        }

        if (item.Flags.HasFlag(ItemFlags.Advancement)) {
            return Color.BlueViolet;
        }

        if (item.Flags.HasFlag(ItemFlags.NeverExclude)) {
            return Color.Blue;
        }

        if (item.Flags.HasFlag(ItemFlags.Trap)) {
            return Color.OrangeRed;
        }

        // Fallback for cheated items.
        if (item.Player.Slot == 0) {
            return Color.White;
        }

        return Color.Turquoise;
    }

    public string GetPlayerName(int slot) {
        return _session.Players.GetPlayerAlias(slot);
    }

    #region DeathLink

    public void SendDeath(GameObj deathCauseObj) {
        if (!_session.Socket.Connected || !_session.ConnectionInfo.Tags.Contains("DeathLink")) {
            return;
        }

        var player = _session.Players.GetPlayerName(_session.ConnectionInfo.Slot);
        var cause = $"{player} was slain by {deathCauseObj.Name}.";
        _deathLinkService.SendDeathLink(new DeathLink(player, cause));
    }

    public void ClearDeath() {
        QueuedDeathLink = null;
    }

    #endregion

    #region Event Handlers

    private static void OnError(Exception exception, string message) {
        Console.WriteLine(@$"[Error]: ${message}");
        Console.WriteLine(exception);
    }

    private static void OnPacketReceived(ArchipelagoPacketBase packet) {
        // TODO: This should probably be removed before a live-version goes out.
        Console.WriteLine(@$"[Packet]: Received a {packet.PacketType} packet.");
    }

    private void OnItemReceived(ReceivedItemsHelper helper) {
        while (helper.Any()) {
            ItemQueue.Enqueue(helper.DequeueItem());
        }
    }

    private void OnDeathLink(DeathLink deathLink) {
        QueuedDeathLink ??= deathLink;
    }

    #endregion
}

public enum ConnectionStatus {
    Disconnected,
    Processing,
    Ready,
}
