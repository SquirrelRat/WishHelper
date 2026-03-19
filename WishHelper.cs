using ExileCore;
using ExileCore.PoEMemory;
using ExileCore.PoEMemory.Elements;
using ExileCore.Shared.Nodes;
using ImGuiNET;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Color = SharpDX.Color;

namespace WishHelper;

public class WishHelper : BaseSettingsPlugin<WishHelperSettings>
{
    private readonly List<(Element Element, string WishName, Color Color, string Tier, int Weight, string ShortDescription)> _highlightedElements = new();
    private List<(WishData Wish, RangeNode<int> Weight)> _wishList = new();
    private Element _recommendedElement;
    private Element _confirmButton;
    private ProfileManager _profileManager = null!;
    private string _newProfileName = "";
    private int _selectedProfileIndex = 0;
    private (Element Element, string WishName, string Tier, int Weight, string ShortDescription, Color Color)? _hoveredElement;
    private bool _autoSelectTriggered;
    private DateTime _lastAutoSelectTime;

    private record WishData(string Name, string Description, string ShortDescription);

    private static readonly Dictionary<string, string> WishTooltips = new()
    {
        ["Wish for Fishes"] = "You get a fishing rod. Take it once for the meme, never again.",
        ["Wish for Wealth"] = "Double your currency drops. Best taken early league when every chaos counts.",
        ["Wish for Foreknowledge"] = "Double divination cards. The lottery ticket - you might get a Doctor worth 300 divines, or trash.",
        ["Wish for Scarabs"] = "80% more scarabs. Solid consistent value that's always useful for atlas sustain.",
        ["Wish for Gold"] = "80% more gold. Worth it for Faustus gambling or village mechanics.",
        ["Wish for Troves"] = "Extra unique strongbox. 99% trash, but that 1% could be league-start enabling.",
        ["Wish for Ancient Protection"] = "Guaranteed unique armour at chain break. Good if you desperately need a slot filled.",
        ["Wish for Ancient Armaments"] = "Guaranteed unique weapon at chain break. Weapons have more build-defining uniques.",
        ["Wish for Ancient Curios"] = "Guaranteed unique jewellery at chain break. Rings/amulets have many build-enabling options.",
        ["Wish for Craftsmanship"] = "Guaranteed 5-link body armour. Early league this saves dozens of fusings.",
        ["Wish for Knowledge"] = "50% more XP. Only take if leveling (sub-90) or pushing for 100.",
        ["Wish for Godhood"] = "You become unkillable with infinite mana/life. Hardcore safety net.",
        ["Wish for Souls"] = "Soul Eater buff - get faster as you kill. Fun for zoom builds.",
        ["Wish for Momentum"] = "Onslaught + Adrenaline. Movement speed and damage. Good for any build.",
        ["Wish for Horizons"] = "Double map drops. Take when struggling with atlas sustain.",
        ["Wish for Distant Horizons"] = "Guaranteed map cache. Reliable but boring atlas sustain.",
        ["Wish for Strange Horizons"] = "Guaranteed unique map. Take if collecting for completion.",
        ["Wish for Providence"] = "Nameless Seer gives mirrored items. High risk, high reward gamble.",
        ["Wish for Reflection"] = "Reflecting Mist appears. Mystery box mechanic for special items.",
        ["Wish for Uncertainty"] = "10 random scarab effects at once. Insane density but potentially lethal.",
        ["Wish for Titans"] = "3 extra Atlas boss packs. More loot, more danger.",
        ["Wish for Terror"] = "Boss spawns with Pinnacle boss from The Feared. Loot goblins take this.",
        ["Wish for Rust"] = "Boss spawns with Ridan. Extra boss = extra loot and danger.",
        ["Wish for Risk"] = "12 extra hard monster packs. More loot, noticeable difficulty spike.",
        ["Wish for Foes"] = "Rare monsters get +2 modifiers. Harder rares but better loot.",
        ["Wish for Rebirth"] = "Monsters can revive. Basically annoying. Last resort option.",
        ["Wish for Eminence"] = "Guaranteed unique jewel at chain break. Cluster jewels can be build-defining.",
        ["Wish for Glittering"] = "Skill gems drop with random quality. Good for 20% gems without GCPs.",
        ["Wish for Oases"] = "Oasis Ground patches. Regen life/mana while standing. Defensive option.",
        ["Wish for Hindrance"] = "Enemies chilled and hindered. Defensive layer for squishy builds.",
        ["Wish for Pursuit"] = "4% chance for Golden Volatiles. Loot explosions but dangerous.",
        ["Wish for Hordes"] = "20% pack size. More monsters = more loot. Simple and effective.",
        ["Wish for Treasures"] = "80% increased rarity. More rare items. Great early, meh late league.",
        ["Wish for Prosperity"] = "Extra gold fountain. See Wish for Gold.",
        ["Wish for Elements"] = "Storm Blessing buff. Good for lightning builds, mediocre otherwise.",
        ["Wish for Wisps"] = "Wildwood Wisps empower enemies. Dangerous but Wisp content is profitable.",
        ["Wish for Croaks"] = "More frogs. That's it. More frogs. Why wouldn't you take this?",
        ["Wish for Fortune"] = "Currency cache at chain break. Reliable currency income.",
        ["Wish for Skittering"] = "Scarab cache at chain break. Good for atlas sustain.",
        ["Wish for Augury"] = "Stacked Deck cache at chain break. Gamble for div cards.",
        ["Wish for Binding"] = "Orb of Binding rewards. Useful early for 4-links, meh later.",
        ["Wish for Regency"] = "Regal Orb rewards. Crafting currency, situational value.",
        ["Wish for Connections"] = "Fusing/Jeweller rewards. Linking currency always useful.",
        ["Wish for Glyphs"] = "Scrolls drop as other currency. Niche but can be decent early.",
        ["Wish for Trinkets"] = "Jewellery drops as Jewels. Good if you need jewels specifically.",
        ["Wish for Power"] = "Enemies explode on death. Clear speed boost for most builds.",
        ["Wish for Avarice"] = "Some packs convert equipment to gold. Niche gold farming.",
        ["Wish for Betrayal"] = "Some packs replaced with Syndicate. Good if you need Syndicate content.",
        ["Wish for Phantoms"] = "Some packs can't drop equipment. Generally avoid this one.",
        ["Wish for Meddling"] = "12 extra Astral monster packs. More monsters, more loot.",
        ["Wish for Flame"] = "Coin of Power reward. Pinnacle boss currency.",
        ["Wish for Tides"] = "Coin of Knowledge reward. Pinnacle boss currency.",
        ["Wish for Sands"] = "Coin of Skill reward. Pinnacle boss currency."
    };

    private static readonly List<WishData> Wishes = new()
    {
        new("Wish for Fishes", "Claim a Fishing Rod.", "Fishing Rod"),
        new("Wish for Foes", "Rare Monsters in the Mirage Area will have two additional modifiers.", "Rare +2 Mods"),
        new("Wish for Rebirth", "Monsters in the Mirage Area will have a chance to revive when slain.", "Monsters Revive"),
        new("Wish for Troves", "An additional Unique Strongbox will appear in the Mirage Area.", "Unique Strongbox"),
        new("Wish for Glittering", "Skill and Support Gems found in Mirage Area will have a random amount of Quality.", "Quality Gems"),
        new("Wish for Wealth", "100% more Currency found in the Mirage Area.", "100% Currency"),
        new("Wish for Foreknowledge", "100% more Divination Cards found in the Mirage Area.", "100% Div Cards"),
        new("Wish for Scarabs", "80% more Scarabs found in the Mirage Area.", "80% Scarabs"),
        new("Wish for Horizons", "100% more Maps found in the Mirage Area.", "100% Maps"),
        new("Wish for Treasures", "80% increased Rarity of items found in the Mirage Area.", "80% Rarity"),
        new("Wish for Hordes", "20% increased Pack Size in the Mirage Area.", "20% Pack Size"),
        new("Wish for Gold", "Players in Mirage Area find 80% more Gold from slain Enemies.", "80% Gold"),
        new("Wish for Titans", "Mirage Area will contain 3 additional packs of Atlas Bosses.", "3 Atlas Boss Packs"),
        new("Wish for Jewels", "An additional Bronze Jewel Cache will appear in the Mirage Area.", "Bronze Jewel Cache"),
        new("Wish for Jewels", "An additional Silver Jewel Cache will appear in the Mirage Area.", "Silver Jewel Cache"),
        new("Wish for Jewels", "An additional Golden Jewel Cache will appear in the Mirage Area.", "Golden Jewel Cache"),
        new("Wish for Meddling", "Mirage Area will contain 12 additional packs of Astral monsters.", "12 Astral Packs"),
        new("Wish for Risk", "Mirage Area will contain 12 additional packs of difficult and rewarding monsters.", "12 Hard Packs"),
        new("Wish for Prosperity", "Mirage Area will contain an additional fountain of wealth.", "Fountain of Wealth"),
        new("Wish for Uncertainty", "Mirage Area will be affected by 10 random Scarab modifiers.", "10 Scarab Mods"),
        new("Wish for Hindrance", "Enemies in the Mirage Area will be Chilled and Hindered.", "Chilled/Hindered"),
        new("Wish for Pursuit", "Enemies in the Mirage Area have a 4% chance to release a Golden Volatile on death.", "4% Golden Volatile"),
        new("Wish for Oases", "Mirage Area has patches of Oasis Ground.", "Oasis Ground"),
        new("Wish for Rust", "Map Boss of the Mirage Area will be accompanied by Ridan, of the Afarud.", "Boss + Ridan"),
        new("Wish for Providence", "The Nameless Seer will appear on breaking the Astral Chain in the Mirage Area.", "Nameless Seer"),
        new("Wish for Reflection", "A Reflecting Mist will appear on breaking the Astral Chain in the Mirage Area.", "Reflecting Mist"),
        new("Wish for Godhood", "Players in Mirage Area have Echoing Shrine, Divine Shrine and Cannot Die.", "Echo/Divine/Immortal"),
        new("Wish for Knowledge", "Players in Mirage Area gain 50% increased Experience.", "50% XP"),
        new("Wish for Power", "Enemies slain by Players in Mirage Area explode on death.", "Explode"),
        new("Wish for Momentum", "Players in Mirage Area have Onslaught and Adrenaline.", "Onslaught/Adrenaline"),
        new("Wish for Souls", "Players in Mirage Area have Soul Eater.", "Soul Eater"),
        new("Wish for Elements", "Players in Mirage Area have the Blessing of the Storm.", "Storm Blessing"),
        new("Wish for Wisps", "Enemies in Mirage Area have a chance to be empowered by Wildwood Wisps.", "Wildwood Wisps"),
        new("Wish for Croaks", "Mirage Area contains additional frogs.", "More Frogs"),
        new("Wish for Glyphs", "Portal Scrolls and Wisdom Scrolls found in Mirage Area will instead drop as other Currencies.", "Scrolls > Currency"),
        new("Wish for Trinkets", "Jewellery found in Mirage Area will instead drop as Jewels.", "Jewellery > Jewels"),
        new("Wish for Terror", "Map Bosses of the Mirage Area will be accompanied by a Pinnacle Atlas Boss from The Feared.", "Bosses + Pinnacle"),
        new("Wish for Eminence", "Breaking the Astral Chain in the Mirage Area will reward an additional Unique Jewel.", "Unique Jewel"),
        new("Wish for Avarice", "Some packs in the Mirage Area will be replaced with Monsters that convert dropped Equipment to Gold.", "Equip > Gold"),
        new("Wish for Betrayal", "Some packs in the Mirage Area will be replaced with Syndicate members.", "Syndicate"),
        new("Wish for Phantoms", "Some packs in the Mirage Area will be replaced with Monsters that cannot drop Equipment.", "No Equip Drops"),
        new("Wish for Fortune", "Breaking the Astral Chain in the Mirage Area will reward a cache of Currency.", "Currency Cache"),
        new("Wish for Skittering", "Breaking the Astral Chain in the Mirage Area will reward a cache of Scarabs.", "Scarab Cache"),
        new("Wish for Augury", "Breaking the Astral Chain in the Mirage Area will reward a cache of Stacked Decks.", "Stacked Decks"),
        new("Wish for Distant Horizons", "Breaking the Astral Chain in the Mirage Area will reward a cache of Maps.", "Map Cache"),
        new("Wish for Strange Horizons", "Breaking the Astral Chain in the Mirage Area will reward a Unique Map.", "Unique Map"),
        new("Wish for Binding", "Breaking the Astral Chain in the Mirage Area will reward Orbs of Binding.", "Binding Orbs"),
        new("Wish for Regency", "Breaking the Astral Chain in the Mirage Area will reward Regal Orbs.", "Regal Orbs"),
        new("Wish for Connections", "Breaking the Astral Chain in the Mirage Area will reward Fusing or Jewellers Orbs.", "Fusing/Jewellers"),
        new("Wish for Ancient Protection", "Breaking the Astral Chain in the Mirage Area will reward a Unique Armour.", "Unique Armour"),
        new("Wish for Ancient Armaments", "Breaking the Astral Chain in the Mirage Area will reward a Unique Weapon.", "Unique Weapon"),
        new("Wish for Ancient Curios", "Breaking the Astral Chain in the Mirage Area will reward a Unique Jewellery item.", "Unique Jewellery"),
        new("Wish for Craftsmanship", "Breaking the Astral Chain in the Mirage Area will reward a Five-Linked Body Armour.", "5L Armour"),
        new("Wish for Mosaics", "Breaking the Astral Chain in the Mirage Area will reward Chromatic Orbs.", "Chromatic Orbs"),
        new("Wish for Swiftness", "Breaking the Astral Chain in the Mirage Area will reward Rare Boots.", "Rare Boots"),
        new("Wish for Helms", "Breaking the Astral Chain in the Mirage Area will reward Rare Helmets.", "Rare Helmets"),
        new("Wish for Mitts", "Breaking the Astral Chain in the Mirage Area will reward Rare Gloves.", "Rare Gloves"),
        new("Wish for Protection", "Breaking the Astral Chain in the Mirage Area will reward Rare Body Armours.", "Rare Armour"),
        new("Wish for Blades", "Breaking the Astral Chain in the Mirage Area will reward Rare Melee Weapons.", "Rare Melee"),
        new("Wish for Missiles", "Breaking the Astral Chain in the Mirage Area will reward Rare Ranged Weapons.", "Rare Ranged"),
        new("Wish for Bastions", "Breaking the Astral Chain in the Mirage Area will reward Rare Shields.", "Rare Shields"),
        new("Wish for Flame", "Breaking the Astral Chain in the Mirage Area will reward a Coin of Power.", "Coin of Power"),
        new("Wish for Tides", "Breaking the Astral Chain in the Mirage Area will reward a Coin of Knowledge.", "Coin of Knowledge"),
        new("Wish for Sands", "Breaking the Astral Chain in the Mirage Area will reward a Coin of Skill.", "Coin of Skill")
    };

    public override bool Initialise()
    {
        _wishList = Wishes.Select((wish, index) => (wish, GetWeightSetting(index))).ToList();
        Input.RegisterKey(Settings.SelectRecommendedHotkey);
        Settings.SelectRecommendedHotkey.OnValueChanged += () => Input.RegisterKey(Settings.SelectRecommendedHotkey);
        
        _profileManager = new ProfileManager(Settings, ConfigDirectory);
        _profileManager.LoadProfiles();
        
        return true;
    }

    private RangeNode<int> GetWeightSetting(int index) => index switch
    {
        0 => Settings.FishesWeight,
        1 => Settings.FoesWeight,
        2 => Settings.RebirthWeight,
        3 => Settings.TrovesWeight,
        4 => Settings.GlitteringWeight,
        5 => Settings.WealthWeight,
        6 => Settings.ForeknowledgeWeight,
        7 => Settings.ScarabsWeight,
        8 => Settings.HorizonsWeight,
        9 => Settings.TreasuresWeight,
        10 => Settings.HordesWeight,
        11 => Settings.GoldWeight,
        12 => Settings.TitansWeight,
        13 => Settings.JewelsBronzeWeight,
        14 => Settings.JewelsSilverWeight,
        15 => Settings.JewelsGoldWeight,
        16 => Settings.MeddlingWeight,
        17 => Settings.RiskWeight,
        18 => Settings.ProsperityWeight,
        19 => Settings.UncertaintyWeight,
        20 => Settings.HindranceWeight,
        21 => Settings.PursuitWeight,
        22 => Settings.OasesWeight,
        23 => Settings.RustWeight,
        24 => Settings.ProvidenceWeight,
        25 => Settings.ReflectionWeight,
        26 => Settings.GodhoodWeight,
        27 => Settings.KnowledgeWeight,
        28 => Settings.PowerWeight,
        29 => Settings.MomentumWeight,
        30 => Settings.SoulsWeight,
        31 => Settings.ElementsWeight,
        32 => Settings.WispsWeight,
        33 => Settings.CroaksWeight,
        34 => Settings.GlyphsWeight,
        35 => Settings.TrinketsWeight,
        36 => Settings.TerrorWeight,
        37 => Settings.EminenceWeight,
        38 => Settings.AvariceWeight,
        39 => Settings.BetrayalWeight,
        40 => Settings.PhantomsWeight,
        41 => Settings.FortuneWeight,
        42 => Settings.SkitteringWeight,
        43 => Settings.AuguryWeight,
        44 => Settings.DistantHorizonsWeight,
        45 => Settings.StrangeHorizonsWeight,
        46 => Settings.BindingWeight,
        47 => Settings.RegencyWeight,
        48 => Settings.ConnectionsWeight,
        49 => Settings.AncientProtectionWeight,
        50 => Settings.AncientArmamentsWeight,
        51 => Settings.AncientCuriosWeight,
        52 => Settings.CraftsmanshipWeight,
        53 => Settings.MosaicsWeight,
        54 => Settings.SwiftnessWeight,
        55 => Settings.HelmsWeight,
        56 => Settings.MittsWeight,
        57 => Settings.ProtectionWeight,
        58 => Settings.BladesWeight,
        59 => Settings.MissilesWeight,
        60 => Settings.BastionsWeight,
        61 => Settings.FlameWeight,
        62 => Settings.TidesWeight,
        63 => Settings.SandsWeight,
        _ => Settings.FishesWeight
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
        _recommendedElement = null;
        _confirmButton = null;

        if (!Settings.Enable.Value) return null;

        var miragePanel = GameController.Game.IngameState.IngameUi.MirageWishesPanel;
        if (miragePanel?.IsVisible != true || miragePanel.Children.Count <= 4)
        {
            _autoSelectTriggered = false;
            return null;
        }

        var child4 = miragePanel.Children[4];
        if (child4 == null) return null;

        foreach (var index in new[] { 3, 4, 5 })
        {
            if (child4.Children.Count <= index) continue;
            var targetChild = child4.Children[index];
            if (targetChild == null) continue;

            var wishInfo = FindWishName(targetChild);
            if (wishInfo.HasValue)
                _highlightedElements.Add((wishInfo.Value.Element, wishInfo.Value.WishName, wishInfo.Value.Color, wishInfo.Value.Tier, wishInfo.Value.Weight, wishInfo.Value.ShortDescription));
        }

        if (child4.Children.Count > 6)
            _confirmButton = child4.Children[6];

        if (_highlightedElements.Count > 0)
        {
            var maxWeight = _highlightedElements.Max(item => item.Weight);
            var bestCount = _highlightedElements.Count(item => item.Weight == maxWeight);
            var hasUniqueWinner = bestCount == 1;

            if (hasUniqueWinner)
                _recommendedElement = _highlightedElements.First(item => item.Weight == maxWeight).Element;
        }

        if (Settings.AutoSelectRecommended.Value && _recommendedElement != null && !_autoSelectTriggered)
        {
            if ((DateTime.Now - _lastAutoSelectTime).TotalSeconds > 2)
            {
                _autoSelectTriggered = true;
                _lastAutoSelectTime = DateTime.Now;
                AutoSelectRecommendedWish();
            }
        }

        return null;
    }

    private (string WishName, Element Element, Color Color, string Tier, int Weight, string ShortDescription)? FindWishName(Element element)
    {
        if (element == null) return null;

        if (!string.IsNullOrEmpty(element.Text))
        {
            var text = element.Text;
            foreach (var (wish, weightNode) in _wishList)
            {
                if (text.Contains(wish.Name, StringComparison.OrdinalIgnoreCase))
                {
                    var weight = weightNode.Value;
                    var tier = WishHelperSettings.GetTierFromWeight(weight);
                    return (text, element, GetColorForTier(tier), tier, weight, wish.ShortDescription);
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

        var mousePos = new System.Numerics.Vector2(Input.MousePosition.X, Input.MousePosition.Y);
        _hoveredElement = null;

        if (_highlightedElements.Count > 0)
        {
            var maxWeight = _highlightedElements.Max(item => item.Weight);
            var bestCount = _highlightedElements.Count(item => item.Weight == maxWeight);
            var hasUniqueWinner = bestCount == 1;

            foreach (var item in _highlightedElements)
            {
                if (item.Element?.Address == 0) continue;

                var rect = item.Element.GetClientRect();
                var color = item.Color;
                var expandedRect = new RectangleF(rect.X - 1, rect.Y - 1, rect.Width + 2, rect.Height + 2);
                var isRecommended = hasUniqueWinner && item.Weight == maxWeight;

                if (Settings.DrawBox)
                    Graphics.DrawBox(rect, new Color((byte)color.R, (byte)color.G, (byte)color.B, (byte)60));

                if (Settings.DrawFrame)
                {
                    var thickness = isRecommended ? Settings.RecommendedFrameThickness.Value : Settings.NonRecommendedFrameThickness.Value;
                    Graphics.DrawFrame(expandedRect, color, thickness);
                }

                if (Settings.ShowCustomText)
                {
                    DrawCustomTextOverlay(rect, item.Tier, item.ShortDescription, color, isRecommended);
                }

                if (Settings.ShowTierLabel)
                {
                    var labelPos = new System.Numerics.Vector2(rect.X + 5, rect.Y + 5);
                    Graphics.DrawTextWithBackground(item.Tier, labelPos, Color.White, Color.Black);
                }

                if (Settings.ShowWeightOnCards)
                    DrawWeight(rect, item.Weight);

                if (isRecommended && Settings.RecommendationAnimation.Value != "None")
                {
                    ApplyRecommendationAnimation(rect, color);
                }

                var tooltipRect = new RectangleF(rect.X, rect.Y, rect.Width, rect.Height);
                if (mousePos.X >= tooltipRect.X && mousePos.X <= tooltipRect.Right &&
                    mousePos.Y >= tooltipRect.Y && mousePos.Y <= tooltipRect.Bottom)
                {
                    if (_hoveredElement == null)
                    {
                        _hoveredElement = (item.Element, item.WishName, item.Tier, item.Weight, item.ShortDescription, item.Color);
                    }
                }
            }
        }

        if (_hoveredElement.HasValue)
        {
            DrawTooltipImGui(_hoveredElement.Value, mousePos);
        }

        if (Settings.SelectRecommendedHotkey.PressedOnce())
        {
            SelectRecommended();
        }
    }

    private void DrawCustomTextOverlay(RectangleF rect, string tier, string shortDescription, Color tierColor, bool isRecommended)
    {
        const int padding = 5;

        string topText;
        Color topColor;

        if (isRecommended)
        {
            topText = "Recommended";
            topColor = Color.Green;
        }
        else
        {
            topText = "";
            topColor = tierColor;
        }

        if (!string.IsNullOrEmpty(topText))
        {
            var tierSize = Graphics.MeasureText(topText);
            var tierX = rect.X + (rect.Width - tierSize.X) / 2;
            var tierPos = new System.Numerics.Vector2(tierX, rect.Y + padding);
            Graphics.DrawTextWithBackground(topText, tierPos, topColor, Color.Black);
        }

        var descSize = Graphics.MeasureText(shortDescription);
        var descX = rect.X + (rect.Width - descSize.X) / 2;
        var descPos = new System.Numerics.Vector2(descX, rect.Bottom - descSize.Y - padding);
        Graphics.DrawTextWithBackground(shortDescription, descPos, tierColor, Color.Black);
    }

    private void DrawWeight(RectangleF rect, int weight)
    {
        var weightText = weight.ToString();
        var weightSize = Graphics.MeasureText(weightText);
        var weightPos = new System.Numerics.Vector2(rect.Right - weightSize.X - 5, rect.Y + 5);
        Graphics.DrawTextWithBackground(weightText, weightPos, Color.White, Color.Black);
    }

    private void DrawTooltipImGui((Element Element, string WishName, string Tier, int Weight, string ShortDescription, Color Color) hovered, System.Numerics.Vector2 mousePos)
    {
        ImGui.PushStyleColor(ImGuiCol.PopupBg, new System.Numerics.Vector4(0.08f, 0.08f, 0.12f, 1f));
        ImGui.PushStyleColor(ImGuiCol.Border, new System.Numerics.Vector4(hovered.Color.R / 255f, hovered.Color.G / 255f, hovered.Color.B / 255f, 1f));
        ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 2f);
        ImGui.PushStyleVar(ImGuiStyleVar.WindowPadding, new System.Numerics.Vector2(12, 12));
        ImGui.PushStyleVar(ImGuiStyleVar.PopupBorderSize, 2f);

        var wishName = hovered.WishName.Trim();
        var titleSize = Graphics.MeasureText(wishName);

        var maxWidth = titleSize.X;
        var descSize = Graphics.MeasureText(hovered.ShortDescription);
        if (descSize.X > maxWidth) maxWidth = descSize.X;

        const int padding = 12;
        var tooltipWidth = maxWidth + padding * 2;
        const float cursorHalfWidth = 12;
        var tooltipX = mousePos.X + cursorHalfWidth - tooltipWidth / 2;
        var tooltipY = mousePos.Y + 50;

        var windowSize = GameController.Window.GetWindowRectangle();
        if (tooltipX < 0) tooltipX = 0;
        if (tooltipX + tooltipWidth > windowSize.Width) tooltipX = windowSize.Width - tooltipWidth;

        ImGui.SetNextWindowPos(new System.Numerics.Vector2(tooltipX, tooltipY), ImGuiCond.Always);

        if (ImGui.Begin("##WishTooltip", ImGuiWindowFlags.Tooltip | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoInputs | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoSavedSettings))
        {
            var titleColor = new System.Numerics.Vector4(hovered.Color.R / 255f, hovered.Color.G / 255f, hovered.Color.B / 255f, 1f);

            ImGui.PushStyleColor(ImGuiCol.Text, titleColor);
            ImGui.Text(wishName);
            ImGui.PopStyleColor();

            ImGui.Separator();

            ImGui.TextColored(new System.Numerics.Vector4(0.86f, 0.86f, 0.86f, 1f), $"Tier: {hovered.Tier}");
            ImGui.TextColored(new System.Numerics.Vector4(0.86f, 0.86f, 0.86f, 1f), $"Weight: {hovered.Weight}");

            ImGui.Separator();

            var tooltipText = "No description available.";
            var wishKey = wishName.Split('\n')[0].Trim();
            if (WishTooltips.TryGetValue(wishKey, out var desc))
            {
                tooltipText = desc;
            }

            ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35);
            ImGui.TextColored(new System.Numerics.Vector4(0.9f, 0.9f, 0.7f, 1f), tooltipText);
            ImGui.PopTextWrapPos();

            ImGui.End();
        }

        ImGui.PopStyleVar(3);
        ImGui.PopStyleColor(2);
    }

    private void SelectRecommended()
    {
        if (_recommendedElement == null || _confirmButton == null)
            return;

        if (_recommendedElement.Address == 0 || _confirmButton.Address == 0)
            return;

        var cardRect = _recommendedElement.GetClientRect();
        var cardCenter = new System.Numerics.Vector2(
            cardRect.X + cardRect.Width / 2,
            cardRect.Y + cardRect.Height / 2
        );

        var confirmRect = _confirmButton.GetClientRect();
        var confirmCenter = new System.Numerics.Vector2(
            confirmRect.X + confirmRect.Width / 2,
            confirmRect.Y + confirmRect.Height / 2
        );

        ThreadPool.QueueUserWorkItem(_ =>
        {
            Input.SetCursorPos(cardCenter);
            Thread.Sleep(200);
            Input.Click(MouseButtons.Left);
            Thread.Sleep(250);
            Input.SetCursorPos(confirmCenter);
            Thread.Sleep(200);
            Input.Click(MouseButtons.Left);
        });
    }

    private void AutoSelectRecommendedWish()
    {
        if (_recommendedElement == null)
            return;

        if (_recommendedElement.Address == 0)
            return;

        var cardRect = _recommendedElement.GetClientRect();
        var cardCenter = new System.Numerics.Vector2(
            cardRect.X + cardRect.Width / 2,
            cardRect.Y + cardRect.Height / 2
        );

        var originalPos = new System.Numerics.Vector2(Input.MousePosition.X, Input.MousePosition.Y);

        ThreadPool.QueueUserWorkItem(_ =>
        {
            Input.SetCursorPos(cardCenter);
            Thread.Sleep(200);
            Input.Click(MouseButtons.Left);
            Thread.Sleep(100);
            Input.SetCursorPos(originalPos);
        });
    }

    private void DrawGlowRingEffect(RectangleF rect, Color color)
    {
        var glowRadius = 15 * Settings.AnimationIntensity.Value;
        var glowAlpha = (byte)(40);
        var glowColor = new Color((byte)color.R, (byte)color.G, (byte)color.B, (byte)glowAlpha);

        for (int i = 3; i >= 1; i--)
        {
            var glowRect = new RectangleF(
                rect.X - glowRadius * i,
                rect.Y - glowRadius * i,
                rect.Width + glowRadius * i * 2,
                rect.Height + glowRadius * i * 2
            );
            var layerAlpha = (byte)(glowAlpha * (4 - i) / 4);
            var layerColor = new Color((byte)color.R, (byte)color.G, (byte)color.B, (byte)layerAlpha);
            Graphics.DrawBox(glowRect, layerColor);
        }
    }

    private void DrawCornerBrackets(RectangleF rect, Color color)
    {
        var bracketSize = (int)(15 * Settings.AnimationIntensity.Value);
        var thickness = 2;
        var brightColor = new Color((byte)255, (byte)255, (byte)255, (byte)180);

        Graphics.DrawBox(new RectangleF(rect.X, rect.Y, bracketSize, thickness), brightColor);
        Graphics.DrawBox(new RectangleF(rect.X, rect.Y, thickness, bracketSize), brightColor);

        Graphics.DrawBox(new RectangleF(rect.Right - bracketSize, rect.Y, bracketSize, thickness), brightColor);
        Graphics.DrawBox(new RectangleF(rect.Right - thickness, rect.Y, thickness, bracketSize), brightColor);

        Graphics.DrawBox(new RectangleF(rect.X, rect.Bottom - thickness, bracketSize, thickness), brightColor);
        Graphics.DrawBox(new RectangleF(rect.X, rect.Bottom - bracketSize, thickness, bracketSize), brightColor);

        Graphics.DrawBox(new RectangleF(rect.Right - bracketSize, rect.Bottom - thickness, bracketSize, thickness), brightColor);
        Graphics.DrawBox(new RectangleF(rect.Right - thickness, rect.Bottom - bracketSize, thickness, bracketSize), brightColor);

        var currentTime = DateTime.UtcNow.TimeOfDay.TotalSeconds;
        var growPhase = (Math.Sin(currentTime * 1.5 * Settings.AnimationSpeed.Value) + 1) / 2;
        var growAmount = (float)(5 + growPhase * 8);

        var expandedRect = new RectangleF(rect.X - bracketSize, rect.Y - bracketSize, rect.Width + bracketSize * 2, rect.Height + bracketSize * 2);
        Graphics.DrawBox(new RectangleF(expandedRect.X, expandedRect.Y, bracketSize + growAmount, thickness), color);
        Graphics.DrawBox(new RectangleF(expandedRect.X, expandedRect.Y, thickness, bracketSize + growAmount), color);

        Graphics.DrawBox(new RectangleF(expandedRect.Right - bracketSize - growAmount, expandedRect.Y, bracketSize + growAmount, thickness), color);
        Graphics.DrawBox(new RectangleF(expandedRect.Right - thickness, expandedRect.Y, thickness, bracketSize + growAmount), color);

        Graphics.DrawBox(new RectangleF(expandedRect.X, expandedRect.Bottom - thickness, bracketSize + growAmount, thickness), color);
        Graphics.DrawBox(new RectangleF(expandedRect.X, expandedRect.Bottom - bracketSize - growAmount, thickness, bracketSize + growAmount), color);

        Graphics.DrawBox(new RectangleF(expandedRect.Right - bracketSize - growAmount, expandedRect.Bottom - thickness, bracketSize + growAmount, thickness), color);
        Graphics.DrawBox(new RectangleF(expandedRect.Right - thickness, expandedRect.Bottom - bracketSize - growAmount, thickness, bracketSize + growAmount), color);
    }

    private void DrawSnakeEffect(RectangleF rect, Color color)
    {
        var padding = 12 * Settings.AnimationIntensity.Value;
        var lineThickness = 3 * Settings.AnimationIntensity.Value;
        var snakeLength = 60;
        var currentTime = DateTime.UtcNow.TimeOfDay.TotalSeconds;
        var snakePosition = currentTime * 100 * Settings.AnimationSpeed.Value;

        var pathWidth = rect.Width + padding * 2;
        var pathHeight = rect.Height + padding * 2;
        var perimeter = (pathWidth + pathHeight) * 2;
        var startX = rect.X - padding;
        var startY = rect.Y - padding;

        for (int i = 0; i < snakeLength; i++)
        {
            var segmentOffset = (snakePosition - i) % perimeter;
            if (segmentOffset < 0) segmentOffset += perimeter;

            float sx, sy;
            var fade = 1f - (i / (float)snakeLength);
            var alpha = (byte)(Math.Max(0, fade * 255));
            var snakeColor = new Color((byte)255, (byte)255, (byte)255, alpha);

            if (segmentOffset < pathWidth)
            {
                sx = startX + (float)segmentOffset;
                sy = startY;
            }
            else if (segmentOffset < pathWidth + pathHeight)
            {
                sx = startX + pathWidth;
                sy = startY + (float)(segmentOffset - pathWidth);
            }
            else if (segmentOffset < pathWidth * 2 + pathHeight)
            {
                sx = startX + pathWidth - (float)(segmentOffset - (pathWidth + pathHeight));
                sy = startY + pathHeight;
            }
            else
            {
                sx = startX;
                sy = startY + pathHeight - (float)(segmentOffset - (pathWidth * 2 + pathHeight));
            }

            Graphics.DrawBox(new RectangleF(sx - lineThickness / 2, sy - lineThickness / 2, lineThickness, lineThickness), snakeColor);
        }
    }

    private void DrawLightningEffect(RectangleF rect, Color color)
    {
        var currentTime = DateTime.UtcNow.TimeOfDay.TotalSeconds;
        var flashCycle = (currentTime * Settings.AnimationSpeed.Value * 3) % (Math.PI * 2);
        var flashIntensity = Math.Pow(Math.Sin(flashCycle), 8);

        if (flashIntensity > 0.3)
        {
            var flashAlpha = (byte)(180 * flashIntensity);
            var flashColor = new Color((byte)255, (byte)255, (byte)200, flashAlpha);
            var flashRect = new RectangleF(rect.X - 10, rect.Y - 10, rect.Width + 20, rect.Height + 20);
            Graphics.DrawBox(flashRect, flashColor);
        }

        var strikePhase = (int)(currentTime * Settings.AnimationSpeed.Value * 2) % 4;
        var strikeColor = new Color((byte)255, (byte)255, (byte)150, (byte)255);
        var thickness = 2;

        switch (strikePhase)
        {
            case 0:
                Graphics.DrawBox(new RectangleF(rect.X - 15, rect.Y + rect.Height / 2 - 1, 12, thickness), strikeColor);
                break;
            case 1:
                Graphics.DrawBox(new RectangleF(rect.Right + 3, rect.Y + rect.Height / 2 - 1, 12, thickness), strikeColor);
                break;
            case 2:
                Graphics.DrawBox(new RectangleF(rect.X + rect.Width / 2 - 1, rect.Y - 15, thickness, 12), strikeColor);
                break;
            case 3:
                Graphics.DrawBox(new RectangleF(rect.X + rect.Width / 2 - 1, rect.Bottom + 3, thickness, 12), strikeColor);
                break;
        }

        var borderPulse = (byte)(100 + Math.Sin(currentTime * Settings.AnimationSpeed.Value * 4) * 80);
        var borderColor = new Color((byte)color.R, (byte)color.G, (byte)color.B, borderPulse);
        var borderRect = new RectangleF(rect.X - 3, rect.Y - 3, rect.Width + 6, rect.Height + 6);
        Graphics.DrawFrame(borderRect, borderColor, 3);
    }

    private void DrawBeaconEffect(RectangleF rect, Color color)
    {
        var currentTime = DateTime.UtcNow.TimeOfDay.TotalSeconds;
        var sweepTime = currentTime * Settings.AnimationSpeed.Value * 1.5;
        var sweepAngle = sweepTime % (Math.PI * 2);

        var beamLength = 50 * Settings.AnimationIntensity.Value;
        var beamX = rect.X + rect.Width / 2 + MathF.Cos((float)sweepAngle) * beamLength;
        var beamY = rect.Y + rect.Height / 2 + MathF.Sin((float)sweepAngle) * beamLength;

        var beamColor = new Color((byte)255, (byte)100, (byte)50, (byte)200);
        var centerX = rect.X + rect.Width / 2;
        var centerY = rect.Y + rect.Height / 2;

        for (int i = 0; i < 5; i++)
        {
            var t = i / 5f;
            var px = centerX + (beamX - centerX) * t;
            var py = centerY + (beamY - centerY) * t;
            var size = 4 - i * 0.5f;
            var alpha = (byte)(200 - i * 30);
            Graphics.DrawBox(new RectangleF(px - size / 2, py - size / 2, size, size), new Color((byte)255, (byte)150, (byte)50, alpha));
        }

        var radarAlpha = (byte)(80 + Math.Sin(currentTime * Settings.AnimationSpeed.Value * 3) * 40);
        var radarColor = new Color((byte)255, (byte)80, (byte)0, radarAlpha);

        for (int ring = 1; ring <= 3; ring++)
        {
            var ringSize = 15 * ring * Settings.AnimationIntensity.Value;
            var ringAlpha = (byte)(radarAlpha * (4 - ring) / 4);
            var ringColor = new Color((byte)255, (byte)100, (byte)50, ringAlpha);
            var ringRect = new RectangleF(
                centerX - ringSize,
                centerY - ringSize,
                ringSize * 2,
                ringSize * 2
            );
            Graphics.DrawFrame(ringRect, ringColor, 1);
        }

        var corePulse = (byte)(150 + Math.Sin(currentTime * Settings.AnimationSpeed.Value * 6) * 80);
        var coreColor = new Color((byte)255, (byte)200, (byte)100, corePulse);
        Graphics.DrawBox(new RectangleF(centerX - 4, centerY - 4, 8, 8), coreColor);

        var cornerColor = new Color((byte)255, (byte)150, (byte)50, (byte)255);
        var cornerSize = (int)(12 * Settings.AnimationIntensity.Value);
        var thickness = 3;

        Graphics.DrawBox(new RectangleF(rect.X, rect.Y, cornerSize, thickness), cornerColor);
        Graphics.DrawBox(new RectangleF(rect.X, rect.Y, thickness, cornerSize), cornerColor);
        Graphics.DrawBox(new RectangleF(rect.Right - cornerSize, rect.Y, cornerSize, thickness), cornerColor);
        Graphics.DrawBox(new RectangleF(rect.Right - thickness, rect.Y, thickness, cornerSize), cornerColor);
        Graphics.DrawBox(new RectangleF(rect.X, rect.Bottom - thickness, cornerSize, thickness), cornerColor);
        Graphics.DrawBox(new RectangleF(rect.X, rect.Bottom - cornerSize, thickness, cornerSize), cornerColor);
        Graphics.DrawBox(new RectangleF(rect.Right - cornerSize, rect.Bottom - thickness, cornerSize, thickness), cornerColor);
        Graphics.DrawBox(new RectangleF(rect.Right - thickness, rect.Bottom - cornerSize, thickness, cornerSize), cornerColor);
    }

    private void ApplyRecommendationAnimation(RectangleF rect, Color color)
    {
        var animation = Settings.RecommendationAnimation.Value;

        switch (animation)
        {
            case "Celestial":
                DrawGlowRingEffect(rect, color);
                DrawCornerBrackets(rect, color);
                DrawSnakeEffect(rect, color);
                break;
            case "Lightning":
                DrawLightningEffect(rect, color);
                break;
            case "Beacon":
                DrawBeaconEffect(rect, color);
                break;
        }
    }

    public override void DrawSettings()
    {
        base.DrawSettings();

        var sortedWishes = _wishList.OrderBy(w => w.Wish.Name.Replace("Wish for ", "").Trim()).ToList();

        if (ImGui.TreeNode("Wish Weight Customization"))
        {
            if (ImGui.Button("Reset All Weights to Defaults"))
            {
                Settings.ResetWeightsToDefaults();
            }

            ImGui.Separator();


            if (ImGui.TreeNode("Profile Management"))
            {
                ImGui.Text("Profiles:");
                ImGui.SameLine();
                
                var profiles = _profileManager.Profiles.Select(p => p.Name).ToList();
                profiles.Insert(0, "Default");
                
                if (_selectedProfileIndex >= profiles.Count)
                    _selectedProfileIndex = 0;
                
                ImGui.PushItemWidth(200);
                if (ImGui.Combo("##ProfileCombo", ref _selectedProfileIndex, profiles.ToArray(), profiles.Count))
                {
                    var selectedName = profiles[_selectedProfileIndex];
                    _profileManager.LoadProfile(selectedName);
                }
                ImGui.PopItemWidth();
                
                ImGui.SameLine();
                if (ImGui.Button("Delete") && _selectedProfileIndex > 0)
                {
                    var profileToDelete = profiles[_selectedProfileIndex];
                    _profileManager.DeleteProfile(profileToDelete);
                    _selectedProfileIndex = 0;
                }
                
                ImGui.Separator();
                
                ImGui.Text("New Profile:");
                ImGui.SameLine();
                ImGui.PushItemWidth(200);
                ImGui.InputText("##NewProfileName", ref _newProfileName, 50);
                ImGui.PopItemWidth();
                
                ImGui.SameLine();
                if (ImGui.Button("Save Current") && !string.IsNullOrWhiteSpace(_newProfileName))
                {
                    _profileManager.SaveCurrentProfile(_newProfileName.Trim());
                    _newProfileName = "";
                }
                
                ImGui.TreePop();
            }
            
            ImGui.Separator();

            foreach (var (wish, weightNode) in sortedWishes)
            {
                var weight = weightNode.Value;
                var tier = WishHelperSettings.GetTierFromWeight(weight);
                var tierColor = GetColorForTier(tier);
                var imguiColor = new System.Numerics.Vector4(tierColor.R / 255f, tierColor.G / 255f, tierColor.B / 255f, tierColor.A / 255f);

                ImGui.PushStyleColor(ImGuiCol.Text, imguiColor);
                ImGui.Text(wish.Name);
                ImGui.PopStyleColor();

                if (ImGui.IsItemHovered())
                    ImGui.SetTooltip(wish.Description);

                ImGui.SameLine(250);

                ImGui.PushItemWidth(200);
                if (ImGui.SliderInt($"##{wish.Name}_weight", ref weight, 0, 1000))
                {
                    weightNode.Value = weight;
                }
                ImGui.PopItemWidth();

                ImGui.SameLine();
                ImGui.Text("Tier: ");
                ImGui.SameLine();
                ImGui.PushStyleColor(ImGuiCol.Text, imguiColor);
                ImGui.Text(tier);
                ImGui.PopStyleColor();
            }
            ImGui.TreePop();
        }
    }
}
