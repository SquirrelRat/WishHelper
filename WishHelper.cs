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
    private readonly List<(Element Element, Color Color, string Tier, string ShortDescription)> _highlightedElements = new();
    private List<(WishData Wish, ListNode Tier)> _wishList = new();

    private record WishData(string Name, string Description, string ShortDescription);

    private static readonly List<WishData> Wishes = new()
    {
        new("Wish for Fishes", "Claim a Fishing Rod.", "Fishing Rod"),
        new("Wish for Troves", "An additional Unique Strongbox will appear in the Mirage Area.", "+1 Unique Strongbox"),
        new("Wish for Strange Horizons", "Breaking the Astral Chain in the Mirage Area will reward a Unique Map.", "Unique Map"),
        new("Wish for Reflection", "A Reflecting Mist will appear on breaking the Astral Chain in the Mirage Area.", "Reflecting Mist"),
        new("Wish for Providence", "The Nameless Seer will appear on breaking the Astral Chain in the Mirage Area.", "Nameless Seer"),
        new("Wish for Jewels", "An additional Jewel Cache will appear in the Mirage Area.", "+1 Jewel Cache"),
        new("Wish for Wealth", "100% more Currency found in the Mirage Area.", "100% Currency"),
        new("Wish for Foreknowledge", "100% more Divination Cards found in the Mirage Area.", "100% Div Cards"),
        new("Wish for Scarabs", "80% more Scarabs found in the Mirage Area.", "80% Scarabs"),
        new("Wish for Gold", "Players in Mirage Area find 80% more Gold from slain Enemies.", "80% Gold"),
        new("Wish for Eminence", "Breaking the Astral Chain in the Mirage Area will reward an additional Unique Jewel.", "Unique Jewel"),
        new("Wish for Fortune", "Breaking the Astral Chain in the Mirage Area will reward a cache of Currency.", "Currency Cache"),
        new("Wish for Skittering", "Breaking the Astral Chain in the Mirage Area will reward a cache of Scarabs.", "Scarab Cache"),
        new("Wish for Augury", "Breaking the Astral Chain in the Mirage Area will reward a cache of Stacked Decks.", "Stacked Decks"),
        new("Wish for Distant Horizons", "Breaking the Astral Chain in the Mirage Area will reward a cache of Maps.", "Map Cache"),
        new("Wish for Titans", "Mirage Area will contain additional packs of Atlas Bosses.", "Atlas Boss Packs"),
        new("Wish for Prosperity", "Mirage Area will contain an additional fountain of wealth.", "Fountain of Wealth"),
        new("Wish for Knowledge", "Players in Mirage Area gain 50% increased Experience.", "50% XP"),
        new("Wish for Glittering", "Skill and Support Gems found in Mirage Area will have a random amount of Quality.", "Quality Gems"),
        new("Wish for Glyphs", "Portal Scrolls and Wisdom Scrolls found in Mirage Area will instead drop as other Currencies.", "Scrolls → Currency"),
        new("Wish for Horizons", "100% more Maps found in the Mirage Area.", "100% Maps"),
        new("Wish for Treasures", "80% increased Rarity of items found in the Mirage Area.", "80% Rarity"),
        new("Wish for Hordes", "20% increased Pack Size in the Mirage Area.", "20% Pack Size"),
        new("Wish for Ancient Protection", "Breaking the Astral Chain in the Mirage Area will reward a Unique Armour.", "Unique Armour"),
        new("Wish for Ancient Armaments", "Breaking the Astral Chain in the Mirage Area will reward a Unique Weapon.", "Unique Weapon"),
        new("Wish for Ancient Curios", "Breaking the Astral Chain in the Mirage Area will reward a Unique Jewellery item.", "Unique Jewellery"),
        new("Wish for Binding", "Breaking the Astral Chain in the Mirage Area will reward Orbs of Binding.", "Orbs of Binding"),
        new("Wish for Regency", "Breaking the Astral Chain in the Mirage Area will reward Regal Orbs.", "Regal Orbs"),
        new("Wish for Connections", "Breaking the Astral Chain in the Mirage Area will reward Fusing or Jewellers Orbs.", "Fusing/Jewellers"),
        new("Wish for Mosaics", "Breaking the Astral Chain in the Mirage Area will reward Chromatic Orbs.", "Chromatic Orbs"),
        new("Wish for Swiftness", "Breaking the Astral Chain in the Mirage Area will reward Rare Boots.", "Rare Boots"),
        new("Wish for Helms", "Breaking the Astral Chain in the Mirage Area will reward Rare Helmets.", "Rare Helmets"),
        new("Wish for Mitts", "Breaking the Astral Chain in the Mirage Area will reward Rare Gloves.", "Rare Gloves"),
        new("Wish for Protection", "Breaking the Astral Chain in the Mirage Area will reward Rare Body Armours.", "Rare Body Armour"),
        new("Wish for Blades", "Breaking the Astral Chain in the Mirage Area will reward Rare Melee Weapons.", "Rare Melee"),
        new("Wish for Missiles", "Breaking the Astral Chain in the Mirage Area will reward Rare Ranged Weapons.", "Rare Ranged"),
        new("Wish for Bastions", "Breaking the Astral Chain in the Mirage Area will reward Rare Shields.", "Rare Shields"),
        new("Wish for Trinkets", "Breaking the Astral Chain in the Mirage Area will reward Rare Jewellery.", "Rare Jewellery"),
        new("Wish for Craftsmanship", "Breaking the Astral Chain in the Mirage Area will reward a Five-Linked Body Armour.", "5-Linked Armour"),
        new("Wish for Power", "Enemies slain by Players in Mirage Area explode on death. (No loot reward)", "Explode (No Loot)"),
        new("Wish for Souls", "Players in Mirage Area have Soul Eater. (No loot reward)", "Soul Eater (No Loot)"),
        new("Wish for Flames", "Players in Mirage Area have the Blessing of the Storm. (No loot reward)", "Storm Bless (No Loot)"),
        new("Wish for a Fighting Chance", "Map Boss of the Mirage Area will be accompanied by a Pinnacle Atlas Boss from The Feared.", "Boss + Pinnacle"),
        new("Wish for Rebirth", "Monsters in the Mirage Area will have a chance to revive when slain.", "Monsters Revive"),
        new("Wish for Foes", "Rare Monsters in the Mirage Area will have two additional modifiers.", "Rare +2 Mods"),
        new("Wish for Pursuit", "Enemies in the Mirage Area have a 4% chance to release a Golden Volatile on death.", "4% Golden Volatile"),
        new("Wish for Terror", "Map Boss of the Mirage Area will be accompanied by a Pinnacle Atlas Boss from The Feared.", "Boss + Pinnacle"),
        new("Wish for Betrayal", "Some packs in the Mirage Area will be replaced with Syndicate members.", "Packs → Syndicate"),
        new("Wish for Phantoms", "Some packs in the Mirage Area will be replaced with Monsters that cannot drop Equipment.", "Packs → No Equip"),
        new("Wish for Meddling", "Mirage Area will contain 12 additional packs of Astral monsters.", "+12 Astral Packs"),
        new("Wish for Risk", "Mirage Area will contain 12 additional packs of difficult and rewarding monsters.", "+12 Hard Packs"),
        new("Wish for Uncertainty", "Mirage Area will be affected by 10 random Scarab modifiers.", "10 Scarab Mods"),
        new("Wish for Hindrance", "Enemies in the Mirage Area will be Chilled and Hindered.", "Chilled/Hindered"),
        new("Wish for Avarice", "Some packs in the Mirage Area will be replaced with Monsters that convert dropped Equipment to Gold.", "Equip → Gold"),
        new("Wish for Elements", "Players in Mirage Area have the Blessing of the Storm.", "Storm Blessing"),
        new("Wish for Wisps", "Enemies in Mirage Area have a chance to be empowered by Wildwood Wisps.", "Wildwood Wisps"),
        new("Wish for Oases", "Mirage Area has patches of Oasis Ground.", "Oasis Ground"),
        new("Wish for Rust", "Map Boss of the Mirage Area will be accompanied by Ridan, of the Afarud.", "Boss + Ridan"),
        new("Wish for Godhood", "Players in Mirage Area have Echoing Shrine and Divine Shrine.", "Echo/Divine Shrine"),
        new("Wish for Momentum", "Players in Mirage Area have Onslaught and Adrenaline.", "Onslaught/Adrenaline"),
        new("Wish for Croaks", "Mirage Area contains additional frogs.", "More Frogs")
    };

    public override bool Initialise()
    {
        _wishList = Wishes.Select((wish, index) => (wish, GetTierSetting(index))).ToList();
        return true;
    }

    private ListNode GetTierSetting(int index) => index switch
    {
        0 => Settings.FishesTier,
        1 => Settings.TrovesTier,
        2 => Settings.StrangeHorizonsTier,
        3 => Settings.ReflectionTier,
        4 => Settings.ProvidenceTier,
        5 => Settings.JewelsTier,
        6 => Settings.WealthTier,
        7 => Settings.ForeknowledgeTier,
        8 => Settings.ScarabsTier,
        9 => Settings.GoldTier,
        10 => Settings.EminenceTier,
        11 => Settings.FortuneTier,
        12 => Settings.SkitteringTier,
        13 => Settings.AuguryTier,
        14 => Settings.DistantHorizonsTier,
        15 => Settings.TitansTier,
        16 => Settings.ProsperityTier,
        17 => Settings.KnowledgeTier,
        18 => Settings.GlitteringTier,
        19 => Settings.GlyphsTier,
        20 => Settings.HorizonsTier,
        21 => Settings.TreasuresTier,
        22 => Settings.HordesTier,
        23 => Settings.AncientProtectionTier,
        24 => Settings.AncientArmamentsTier,
        25 => Settings.AncientCuriosTier,
        26 => Settings.BindingTier,
        27 => Settings.RegencyTier,
        28 => Settings.ConnectionsTier,
        29 => Settings.MosaicsTier,
        30 => Settings.SwiftnessTier,
        31 => Settings.HelmsTier,
        32 => Settings.MittsTier,
        33 => Settings.ProtectionTier,
        34 => Settings.BladesTier,
        35 => Settings.MissilesTier,
        36 => Settings.BastionsTier,
        37 => Settings.TrinketsTier,
        38 => Settings.CraftsmanshipTier,
        39 => Settings.PowerTier,
        40 => Settings.SoulsTier,
        41 => Settings.FlamesTier,
        42 => Settings.FightingChanceTier,
        43 => Settings.RebirthTier,
        44 => Settings.FoesTier,
        45 => Settings.PursuitTier,
        46 => Settings.TerrorTier,
        47 => Settings.BetrayalTier,
        48 => Settings.PhantomsTier,
        49 => Settings.MeddlingTier,
        50 => Settings.RiskTier,
        51 => Settings.UncertaintyTier,
        52 => Settings.HindranceTier,
        53 => Settings.AvariceTier,
        54 => Settings.ElementsTier,
        55 => Settings.WispsTier,
        56 => Settings.OasesTier,
        57 => Settings.RustTier,
        58 => Settings.GodhoodTier,
        59 => Settings.MomentumTier,
        60 => Settings.CroaksTier,
        _ => Settings.FishesTier
    };

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
                _highlightedElements.Add((wishInfo.Value.Element, wishInfo.Value.Color, wishInfo.Value.Tier, wishInfo.Value.ShortDescription));
        }

        return null;
    }

    private (string WishName, Element Element, Color Color, string Tier, string ShortDescription)? FindWishName(Element element)
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
                    return (text, element, GetColorForTier(tier), tier, wish.ShortDescription);
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
            var expandedRect = new RectangleF(rect.X - 1, rect.Y - 1, rect.Width + 2, rect.Height + 2);
            
            if (Settings.DrawBox)
                Graphics.DrawBox(rect, new Color(color.R, color.G, color.B, (byte)60));
            
            if (Settings.DrawFrame)
                Graphics.DrawFrame(expandedRect, color, Settings.FrameThickness);
            
            if (Settings.ShowCustomText)
                DrawCustomTextOverlay(rect, item.Tier, item.ShortDescription, color);
            
            if (Settings.ShowTierLabel)
            {
                var labelPos = new System.Numerics.Vector2(rect.X + 5, rect.Y + 5);
                Graphics.DrawTextWithBackground(item.Tier, labelPos, Color.White, Color.Black);
            }
        }
    }

    private void DrawCustomTextOverlay(RectangleF rect, string tier, string shortDescription, Color tierColor)
    {
        const int padding = 5;
        
        var tierText = $"Tier: {tier}";
        var tierSize = Graphics.MeasureText(tierText);
        var tierX = rect.X + (rect.Width - tierSize.X) / 2;
        var tierPos = new System.Numerics.Vector2(tierX, rect.Y + padding);
        Graphics.DrawTextWithBackground(tierText, tierPos, tierColor, Color.Black);
        
        var descSize = Graphics.MeasureText(shortDescription);
        var descX = rect.X + (rect.Width - descSize.X) / 2;
        var descPos = new System.Numerics.Vector2(descX, rect.Bottom - descSize.Y - padding);
        Graphics.DrawTextWithBackground(shortDescription, descPos, tierColor, Color.Black);
    }

    public override void DrawSettings()
    {
        base.DrawSettings();

        var tierOptions = new List<string> { "S", "A", "B", "C", "D", "F" };
        var sortedWishes = _wishList.OrderBy(w => w.Wish.Name.Replace("Wish for ", "").Trim()).ToList();
        
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
