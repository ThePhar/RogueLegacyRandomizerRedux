using System;
using System.Collections.Generic;
using Archipelago.MultiClient.Net.Models;
using DS2DEngine;
using Microsoft.Xna.Framework;
using Tweener;
using Color = Microsoft.Xna.Framework.Color;

namespace RogueCastle.GameObjects;

public class ItemsReceivedHUD : ObjContainer {
    private const int MAXIMUM_ELEMENTS = 5;
    private const int ITEM_TIMEOUT_SECONDS = 8;

    public List<ItemsReceivedElement> Elements { get; private set; } = [];
    private List<ItemsReceivedElement> ViewableElements => Elements.GetRange(0, Math.Min(Elements.Count, MAXIMUM_ELEMENTS));

    public void Update() {
        var elementY = Y;
        foreach (var element in ViewableElements) {
            element.FirstDrawnTime ??= DateTime.Now;

            if (element.FirstDrawnTime?.AddSeconds(ITEM_TIMEOUT_SECONDS) < DateTime.Now && !element.FadingOut) {
                element.FadeOut();
            }

            element.UpdateY(elementY);
            elementY += 48f;
        }
    }

    public override void Draw(Camera2D camera) {
        foreach (var element in ViewableElements) {
            element.Draw(camera);
        }

        base.Draw(camera);
    }

    public override void Dispose() {
        if (IsDisposed) {
            return;
        }

        Elements.Clear();
        Elements = null;
        base.Dispose();
    }
}

public class ItemsReceivedElement : ObjContainer {
    private readonly ItemsReceivedHUD _hud;
    private SpriteObj _icon;
    private TextObj _item;
    private TextObj _sender;

    public ItemsReceivedElement(ItemsReceivedHUD hud, int type, ItemInfo item, int sender, params object[] args) {
        var manager = Program.Game.ArchipelagoManager;

        _hud = hud;
        _item = new TextObj(Game.BitFont) {
            Text = item.ItemDisplayName,
            TextureColor = manager.GetItemColor(item),
            OutlineWidth = 1,
            OutlineColour = Color.Black,
            FontSize = 10f,
            Align = Types.TextAlign.Right,
            Y = _hud.Y,
            ForceDraw = true,
        };
        _sender = new TextObj(Game.JunicodeFont) {
            Text = sender == manager.Slot
                ? "LOC_ID_RANDOMIZER_RECEIVED_SELF".GetResourceString()
                : string.Format("LOC_ID_RANDOMIZER_RECEIVED_OTHER".GetResourceString(), manager.GetPlayerName(sender)),
            OutlineWidth = 1,
            OutlineColour = Color.Black,
            FontSize = 6f,
            Align = Types.TextAlign.Right,
            Y = _hud.Y + 8,
            ForceDraw = true,
        };

        // TODO: Implement all icon types.
        _icon = new SpriteObj("BlueprintIcon_Sprite") {
            Y = _hud.Y,
            AnchorY = 0,
            ForceDraw = true,
        };

        // Adjust X.
        _icon.X = _hud.X + 180 + ((60f - _icon.Width) / _icon.Width);
        _icon.AnchorX = _icon.Width;
        _icon.Scale = new Vector2(32f / _icon.Width, 32f / _icon.Height);
        _item.X = _hud.X + 180 - _icon.Width - 8;
        _item.AnchorX = _item.Width;
        _sender.X = _hud.X + 180 - _icon.Width - 8;
        _sender.AnchorX = _sender.Width;
    }

    public DateTime? FirstDrawnTime { get; set; }
    public bool FadingOut { get; private set; }

    public void FadeOut() {
        Tween.To(_icon, 3f, Tween.EaseNone, "Opacity", "0");
        Tween.To(_item, 3f, Tween.EaseNone, "Opacity", "0");
        Tween.To(_sender, 3f, Tween.EaseNone, "Opacity", "0");
        Tween.RunFunction(3f, this, "Dispose");
        FadingOut = true;
    }

    public void UpdateY(float y) {
        _icon.Y = _hud.Y + y;
        _item.Y = _hud.Y + y;
        _sender.Y = _hud.Y + y + 12f;
    }

    public override void Draw(Camera2D camera) {
        FirstDrawnTime ??= DateTime.Now;

        _icon.Draw(camera);
        _item.Draw(camera);
        _sender.Draw(camera);

        base.Draw(camera);
    }

    public override void Dispose() {
        if (IsDisposed) {
            return;
        }

        _hud.Elements.Remove(this);
        _icon.Dispose();
        _icon = null;
        _item.Dispose();
        _item = null;
        _sender.Dispose();
        _sender = null;
        base.Dispose();
    }
}
