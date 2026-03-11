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
    private readonly List<(Element Element, Color Color, string Tier, int Weight, string ShortDescription)> _highlightedElements = new();
    private List<(WishData Wish, RangeNode<int> Weight)> _wishList = new();
    private Element _recommendedElement;
    private Element _confirmButton;
    private ProfileManager _profileManager = null!;
    private string _newProfileName = "";
    private int _selectedProfileIndex = 0;

    private record WishData(string Name, string Description, string ShortDescription);

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
        new("Wish for Jewels", "An additional Jewel Cache will appear in the Mirage Area.", "Jewel Cache"),
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
        new("Wish for Godhood", "Players in Mirage Area have Echoing Shrine and Divine Shrine.", "Echo/Divine Shrines"),
        new("Wish for Knowledge", "Players in Mirage Area gain 50% increased Experience.", "50% XP"),
        new("Wish for Power", "Enemies slain by Players in Mirage Area explode on death.", "Explode"),
        new("Wish for Momentum", "Players in Mirage Area have Onslaught and Adrenaline.", "Onslaught/Adrenaline"),
        new("Wish for Souls", "Players in Mirage Area have Soul Eater.", "Soul Eater"),
        new("Wish for Elements", "Players in Mirage Area have the Blessing of the Storm.", "Storm Blessing"),
        new("Wish for Wisps", "Enemies in Mirage Area have a chance to be empowered by Wildwood Wisps.", "Wildwood Wisps"),
        new("Wish for Croaks", "Mirage Area contains additional frogs.", "More Frogs"),
        new("Wish for Glyphs", "Portal Scrolls and Wisdom Scrolls found in Mirage Area will instead drop as other Currencies.", "Scrolls > Currency"),
        new("Wish for Trinkets", "Jewellery found in Mirage Area will instead drop as Jewels.", "Jewellery > Jewels"),
        new("Wish for Terror", "Map Boss of the Mirage Area will be accompanied by a Pinnacle Atlas Boss from The Feared.", "Boss + Pinnacle"),
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
        new("Wish for Bastions", "Breaking the Astral Chain in the Mirage Area will reward Rare Shields.", "Rare Shields")
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
        13 => Settings.JewelsWeight,
        14 => Settings.MeddlingWeight,
        15 => Settings.RiskWeight,
        16 => Settings.ProsperityWeight,
        17 => Settings.UncertaintyWeight,
        18 => Settings.HindranceWeight,
        19 => Settings.PursuitWeight,
        20 => Settings.OasesWeight,
        21 => Settings.RustWeight,
        22 => Settings.ProvidenceWeight,
        23 => Settings.ReflectionWeight,
        24 => Settings.GodhoodWeight,
        25 => Settings.KnowledgeWeight,
        26 => Settings.PowerWeight,
        27 => Settings.MomentumWeight,
        28 => Settings.SoulsWeight,
        29 => Settings.ElementsWeight,
        30 => Settings.WispsWeight,
        31 => Settings.CroaksWeight,
        32 => Settings.GlyphsWeight,
        33 => Settings.TrinketsWeight,
        34 => Settings.TerrorWeight,
        35 => Settings.EminenceWeight,
        36 => Settings.AvariceWeight,
        37 => Settings.BetrayalWeight,
        38 => Settings.PhantomsWeight,
        39 => Settings.FortuneWeight,
        40 => Settings.SkitteringWeight,
        41 => Settings.AuguryWeight,
        42 => Settings.DistantHorizonsWeight,
        43 => Settings.StrangeHorizonsWeight,
        44 => Settings.BindingWeight,
        45 => Settings.RegencyWeight,
        46 => Settings.ConnectionsWeight,
        47 => Settings.AncientProtectionWeight,
        48 => Settings.AncientArmamentsWeight,
        49 => Settings.AncientCuriosWeight,
        50 => Settings.CraftsmanshipWeight,
        51 => Settings.MosaicsWeight,
        52 => Settings.SwiftnessWeight,
        53 => Settings.HelmsWeight,
        54 => Settings.MittsWeight,
        55 => Settings.ProtectionWeight,
        56 => Settings.BladesWeight,
        57 => Settings.MissilesWeight,
        58 => Settings.BastionsWeight,
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
                _highlightedElements.Add((wishInfo.Value.Element, wishInfo.Value.Color, wishInfo.Value.Tier, wishInfo.Value.Weight, wishInfo.Value.ShortDescription));
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
                    Graphics.DrawBox(rect, new Color(color.R, color.G, color.B, (byte)60));

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
            }
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
