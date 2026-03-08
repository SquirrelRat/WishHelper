using ExileCore;
using ExileCore.PoEMemory;
using ExileCore.PoEMemory.Elements;
using ExileCore.Shared.Nodes;
using ImGuiNET;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using Color = SharpDX.Color;

namespace WishHelper;

public class WishHelper : BaseSettingsPlugin<WishHelperSettings>
{
    private readonly List<(Element Element, Color Color, string Tier)> _highlightedElements = new();
    private List<(WishData, ListNode)> _wishList = new();

    private record WishData(string Name, string Description);

    private static readonly List<WishData> Wishes = new()
    {
        new("Wish for Fishes", "Claim a Fishing Rod."),
        new("Wish for Troves", "An additional Unique Strongbox will appear in the Mirage Area."),
        new("Wish for Strange Horizons", "Breaking the Astral Chain in the Mirage Area will reward a Unique Map."),
        new("Wish for Reflection", "A Reflecting Mist will appear on breaking the Astral Chain in the Mirage Area."),
        new("Wish for Providence", "The Nameless Seer will appear on breaking the Astral Chain in the Mirage Area."),
        new("Wish for Jewels", "An additional Jewel Cache will appear in the Mirage Area."),
        new("Wish for Wealth", "100% more Currency found in the Mirage Area."),
        new("Wish for Foreknowledge", "100% more Divination Cards found in the Mirage Area."),
        new("Wish for Scarabs", "80% more Scarabs found in the Mirage Area."),
        new("Wish for Gold", "Players in Mirage Area find 80% more Gold from slain Enemies."),
        new("Wish for Eminence", "Breaking the Astral Chain in the Mirage Area will reward an additional Unique Jewel."),
        new("Wish for Fortune", "Breaking the Astral Chain in the Mirage Area will reward a cache of Currency."),
        new("Wish for Skittering", "Breaking the Astral Chain in the Mirage Area will reward a cache of Scarabs."),
        new("Wish for Augury", "Breaking the Astral Chain in the Mirage Area will reward a cache of Stacked Decks."),
        new("Wish for Distant Horizons", "Breaking the Astral Chain in the Mirage Area will reward a cache of Maps."),
        new("Wish for Titans", "Mirage Area will contain additional packs of Atlas Bosses."),
        new("Wish for Prosperity", "Mirage Area will contain an additional fountain of wealth."),
        new("Wish for Knowledge", "Players in Mirage Area gain 50% increased Experience."),
        new("Wish for Glittering", "Skill and Support Gems found in Mirage Area will have a random amount of Quality."),
        new("Wish for Glyphs", "Portal Scrolls and Wisdom Scrolls found in Mirage Area will instead drop as other Currencies."),
        new("Wish for Horizons", "100% more Maps found in the Mirage Area."),
        new("Wish for Treasures", "80% increased Rarity of items found in the Mirage Area."),
        new("Wish for Hordes", "20% increased Pack Size in the Mirage Area."),
        new("Wish for Ancient Protection", "Breaking the Astral Chain in the Mirage Area will reward a Unique Armour."),
        new("Wish for Ancient Armaments", "Breaking the Astral Chain in the Mirage Area will reward a Unique Weapon."),
        new("Wish for Ancient Curios", "Breaking the Astral Chain in the Mirage Area will reward a Unique Jewellery item."),
        new("Wish for Binding", "Breaking the Astral Chain in the Mirage Area will reward Orbs of Binding."),
        new("Wish for Regency", "Breaking the Astral Chain in the Mirage Area will reward Regal Orbs."),
        new("Wish for Connections", "Breaking the Astral Chain in the Mirage Area will reward Fusing or Jewellers Orbs."),
        new("Wish for Mosaics", "Breaking the Astral Chain in the Mirage Area will reward Chromatic Orbs."),
        new("Wish for Swiftness", "Breaking the Astral Chain in the Mirage Area will reward Rare Boots."),
        new("Wish for Helms", "Breaking the Astral Chain in the Mirage Area will reward Rare Helmets."),
        new("Wish for Mitts", "Breaking the Astral Chain in the Mirage Area will reward Rare Gloves."),
        new("Wish for Protection", "Breaking the Astral Chain in the Mirage Area will reward Rare Body Armours."),
        new("Wish for Blades", "Breaking the Astral Chain in the Mirage Area will reward Rare Melee Weapons."),
        new("Wish for Missiles", "Breaking the Astral Chain in the Mirage Area will reward Rare Ranged Weapons."),
        new("Wish for Bastions", "Breaking the Astral Chain in the Mirage Area will reward Rare Shields."),
        new("Wish for Trinkets", "Breaking the Astral Chain in the Mirage Area will reward Rare Jewellery."),
        new("Wish for Craftsmanship", "Breaking the Astral Chain in the Mirage Area will reward a Five-Linked Body Armour."),
        new("Wish for Power", "Enemies slain by Players in Mirage Area explode on death. (No loot reward)"),
        new("Wish for Souls", "Players in Mirage Area have Soul Eater. (No loot reward)"),
        new("Wish for Flames", "Players in Mirage Area have the Blessing of the Storm. (No loot reward)"),
        new("Wish for a Fighting Chance", "Map Boss of the Mirage Area will be accompanied by a Pinnacle Atlas Boss from The Feared."),
        new("Wish for Rebirth", "Monsters in the Mirage Area will have a chance to revive when slain."),
        new("Wish for Foes", "Rare Monsters in the Mirage Area will have two additional modifiers."),
        new("Wish for Pursuit", "Enemies in the Mirage Area have a 4% chance to release a Golden Volatile on death."),
        new("Wish for Terror", "Map Boss of the Mirage Area will be accompanied by a Pinnacle Atlas Boss from The Feared."),
        new("Wish for Betrayal", "Some packs in the Mirage Area will be replaced with Syndicate members."),
        new("Wish for Phantoms", "Some packs in the Mirage Area will be replaced with Monsters that cannot drop Equipment."),
        new("Wish for Meddling", "Mirage Area will contain 12 additional packs of Astral monsters."),
        new("Wish for Risk", "Mirage Area will contain 12 additional packs of difficult and rewarding monsters."),
        new("Wish for Uncertainty", "Mirage Area will be affected by 10 random Scarab modifiers."),
        new("Wish for Hindrance", "Enemies in the Mirage Area will be Chilled and Hindered."),
        new("Wish for Avarice", "Some packs in the Mirage Area will be replaced with Monsters that convert dropped Equipment to Gold."),
        new("Wish for Elements", "Players in Mirage Area have the Blessing of the Storm."),
        new("Wish for Wisps", "Enemies in Mirage Area have a chance to be empowered by Wildwood Wisps."),
        new("Wish for Oases", "Mirage Area has patches of Oasis Ground."),
        new("Wish for Rust", "Map Boss of the Mirage Area will be accompanied by Ridan, of the Afarud."),
        new("Wish for Godhood", "Players in Mirage Area have Echoing Shrine and Divine Shrine."),
        new("Wish for Momentum", "Players in Mirage Area have Onslaught and Adrenaline."),
        new("Wish for Croaks", "Mirage Area contains additional frogs.")
    };

    public override bool Initialise()
    {
        _wishList = new()
        {
            (Wishes[0], Settings.FishesTier),
            (Wishes[1], Settings.TrovesTier),
            (Wishes[2], Settings.StrangeHorizonsTier),
            (Wishes[3], Settings.ReflectionTier),
            (Wishes[4], Settings.ProvidenceTier),
            (Wishes[5], Settings.JewelsTier),
            (Wishes[6], Settings.WealthTier),
            (Wishes[7], Settings.ForeknowledgeTier),
            (Wishes[8], Settings.ScarabsTier),
            (Wishes[9], Settings.GoldTier),
            (Wishes[10], Settings.EminenceTier),
            (Wishes[11], Settings.FortuneTier),
            (Wishes[12], Settings.SkitteringTier),
            (Wishes[13], Settings.AuguryTier),
            (Wishes[14], Settings.DistantHorizonsTier),
            (Wishes[15], Settings.TitansTier),
            (Wishes[16], Settings.ProsperityTier),
            (Wishes[17], Settings.KnowledgeTier),
            (Wishes[18], Settings.GlitteringTier),
            (Wishes[19], Settings.GlyphsTier),
            (Wishes[20], Settings.HorizonsTier),
            (Wishes[21], Settings.TreasuresTier),
            (Wishes[22], Settings.HordesTier),
            (Wishes[23], Settings.AncientProtectionTier),
            (Wishes[24], Settings.AncientArmamentsTier),
            (Wishes[25], Settings.AncientCuriosTier),
            (Wishes[26], Settings.BindingTier),
            (Wishes[27], Settings.RegencyTier),
            (Wishes[28], Settings.ConnectionsTier),
            (Wishes[29], Settings.MosaicsTier),
            (Wishes[30], Settings.SwiftnessTier),
            (Wishes[31], Settings.HelmsTier),
            (Wishes[32], Settings.MittsTier),
            (Wishes[33], Settings.ProtectionTier),
            (Wishes[34], Settings.BladesTier),
            (Wishes[35], Settings.MissilesTier),
            (Wishes[36], Settings.BastionsTier),
            (Wishes[37], Settings.TrinketsTier),
            (Wishes[38], Settings.CraftsmanshipTier),
            (Wishes[39], Settings.PowerTier),
            (Wishes[40], Settings.SoulsTier),
            (Wishes[41], Settings.FlamesTier),
            (Wishes[42], Settings.FightingChanceTier),
            (Wishes[43], Settings.RebirthTier),
            (Wishes[44], Settings.FoesTier),
            (Wishes[45], Settings.PursuitTier),
            (Wishes[46], Settings.TerrorTier),
            (Wishes[47], Settings.BetrayalTier),
            (Wishes[48], Settings.PhantomsTier),
            (Wishes[49], Settings.MeddlingTier),
            (Wishes[50], Settings.RiskTier),
            (Wishes[51], Settings.UncertaintyTier),
            (Wishes[52], Settings.HindranceTier),
            (Wishes[53], Settings.AvariceTier),
            (Wishes[54], Settings.ElementsTier),
            (Wishes[55], Settings.WispsTier),
            (Wishes[56], Settings.OasesTier),
            (Wishes[57], Settings.RustTier),
            (Wishes[58], Settings.GodhoodTier),
            (Wishes[59], Settings.MomentumTier),
            (Wishes[60], Settings.CroaksTier)
        };
        return true;
    }

    private Color GetColorForTier(string tier) => tier switch
    {
        "S" => Settings.STierColor.Value,
        "A" => Settings.ATierColor.Value,
        "B" => Settings.BTierColor.Value,
        "C" => Settings.CTierColor.Value,
        _ => Settings.DTierColor.Value
    };

    public override void AreaChange(AreaInstance area) => _highlightedElements.Clear();

    public override Job Tick()
    {
        _highlightedElements.Clear();

        if (!Settings.Enable.Value) return null;

        var miragePanel = GameController.Game.IngameState.IngameUi.MirageWishesPanel;
        if (miragePanel?.IsVisible != true || miragePanel.Children.Count <= 4) return null;

        var child4 = miragePanel.Children[4];
        if (child4 == null) return null;

        foreach (var index in new[] { 3, 4, 5 })
        {
            if (child4.Children.Count <= index) continue;
            var targetChild = child4.Children[index];
            if (targetChild == null) continue;

            var wishInfo = FindWishName(targetChild);
            if (wishInfo.HasValue)
                _highlightedElements.Add((wishInfo.Value.Element, wishInfo.Value.Color, wishInfo.Value.Tier));
        }

        return null;
    }

    private (string WishName, Element Element, Color Color, string Tier)? FindWishName(Element element)
    {
        if (element == null) return null;

        if (!string.IsNullOrEmpty(element.Text))
        {
            var text = element.Text;
            foreach (var (wish, tierNode) in _wishList)
            {
                if (text.Contains(wish.Name, StringComparison.OrdinalIgnoreCase))
                {
                    var tier = tierNode.Value;
                    return (text, element, GetColorForTier(tier), tier);
                }
            }
        }

        foreach (var child in element.Children)
        {
            var result = FindWishName(child);
            if (result.HasValue) return result;
        }

        return null;
    }

    public override void Render()
    {
        if (!Settings.Enable.Value) return;

        foreach (var item in _highlightedElements)
        {
            if (item.Element?.Address == 0) continue;

            var rect = item.Element.GetClientRect();
            var color = item.Color;
            
            if (Settings.DrawBox)
                Graphics.DrawBox(rect, new Color((byte)color.R, (byte)color.G, (byte)color.B, (byte)80));
            
            if (Settings.DrawFrame)
                Graphics.DrawFrame(rect, color, Settings.FrameThickness);
            
            if (Settings.ShowTierLabel)
            {
                var labelPos = new SharpDX.Vector2(rect.X + 5, rect.Y + 5);
                var labelSize = Graphics.MeasureText(item.Tier);
                var labelRect = new RectangleF(labelPos.X, labelPos.Y, labelSize.X + 10, labelSize.Y + 4);
                Graphics.DrawBox(labelRect, new Color((byte)0, (byte)0, (byte)0, (byte)180));
                Graphics.DrawText(item.Tier, labelPos + new SharpDX.Vector2(5, 2), Color.White);
            }
        }
    }

    public override void DrawSettings()
    {
        base.DrawSettings();

        var tierOptions = new List<string> { "S", "A", "B", "C", "D", "F" };
        var sortedWishes = _wishList.OrderBy(w => w.Item1.Name.Replace("Wish for ", "").Trim()).ToList();
        
        if (ImGui.TreeNode("Wish Tier Customization"))
        {
            foreach (var (wish, tierNode) in sortedWishes)
            {
                ImGui.Text(wish.Name);
                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip(wish.Description);
                
                ImGui.SameLine(250);
                var currentTier = tierNode.Value;
                if (ImGui.BeginCombo($"##{wish.Name}", currentTier, ImGuiComboFlags.WidthFitPreview))
                {
                    foreach (var tier in tierOptions)
                    {
                        if (ImGui.Selectable(tier, currentTier == tier))
                            tierNode.Value = tier;
                    }
                    ImGui.EndCombo();
                }
            }
            ImGui.TreePop();
        }
    }
}
