using ExileCore.Shared.Attributes;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using SharpDX;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WishHelper;

public class WishHelperSettings : ISettings
{
    public ToggleNode Enable { get; set; } = new ToggleNode(true);

    public ToggleNode DrawBox { get; set; } = new ToggleNode(true);
    public ToggleNode DrawFrame { get; set; } = new ToggleNode(true);
    public ToggleNode ShowTierLabel { get; set; } = new ToggleNode(true);
    public ToggleNode ShowCustomText { get; set; } = new ToggleNode(true);
    public ToggleNode ShowStarForSTier { get; set; } = new ToggleNode(true);
    public RangeNode<int> FrameThickness { get; set; } = new RangeNode<int>(3, 1, 10);

    [Menu("Select Recommended Hotkey")]
    public HotkeyNode SelectRecommendedHotkey { get; set; } = new HotkeyNode(Keys.None);

    public ColorNode STierColor { get; set; } = new ColorNode(Color.Purple);
    public ColorNode ATierColor { get; set; } = new ColorNode(Color.Green);
    public ColorNode BTierColor { get; set; } = new ColorNode(Color.Yellow);
    public ColorNode CTierColor { get; set; } = new ColorNode(Color.Orange);
    public ColorNode DTierColor { get; set; } = new ColorNode(Color.Red);

    [IgnoreMenu] public RangeNode<int> FishesWeight { get; set; } = new RangeNode<int>(98, 0, 100);
    [IgnoreMenu] public RangeNode<int> TrovesWeight { get; set; } = new RangeNode<int>(77, 0, 100);
    [IgnoreMenu] public RangeNode<int> StrangeHorizonsWeight { get; set; } = new RangeNode<int>(84, 0, 100);
    [IgnoreMenu] public RangeNode<int> ReflectionWeight { get; set; } = new RangeNode<int>(66, 0, 100);
    [IgnoreMenu] public RangeNode<int> ProvidenceWeight { get; set; } = new RangeNode<int>(96, 0, 100);
    [IgnoreMenu] public RangeNode<int> JewelsWeight { get; set; } = new RangeNode<int>(60, 0, 100);

    [IgnoreMenu] public RangeNode<int> WealthWeight { get; set; } = new RangeNode<int>(82, 0, 100);
    [IgnoreMenu] public RangeNode<int> ForeknowledgeWeight { get; set; } = new RangeNode<int>(80, 0, 100);
    [IgnoreMenu] public RangeNode<int> ScarabsWeight { get; set; } = new RangeNode<int>(81, 0, 100);
    [IgnoreMenu] public RangeNode<int> GoldWeight { get; set; } = new RangeNode<int>(100, 0, 100);
    [IgnoreMenu] public RangeNode<int> EminenceWeight { get; set; } = new RangeNode<int>(88, 0, 100);
    [IgnoreMenu] public RangeNode<int> FortuneWeight { get; set; } = new RangeNode<int>(70, 0, 100);
    [IgnoreMenu] public RangeNode<int> SkitteringWeight { get; set; } = new RangeNode<int>(92, 0, 100);
    [IgnoreMenu] public RangeNode<int> AuguryWeight { get; set; } = new RangeNode<int>(76, 0, 100);
    [IgnoreMenu] public RangeNode<int> DistantHorizonsWeight { get; set; } = new RangeNode<int>(83, 0, 100);
    [IgnoreMenu] public RangeNode<int> TitansWeight { get; set; } = new RangeNode<int>(62, 0, 100);
    [IgnoreMenu] public RangeNode<int> ProsperityWeight { get; set; } = new RangeNode<int>(100, 0, 100);

    [IgnoreMenu] public RangeNode<int> KnowledgeWeight { get; set; } = new RangeNode<int>(58, 0, 100);
    [IgnoreMenu] public RangeNode<int> GlitteringWeight { get; set; } = new RangeNode<int>(61, 0, 100);
    [IgnoreMenu] public RangeNode<int> GlyphsWeight { get; set; } = new RangeNode<int>(94, 0, 100);
    [IgnoreMenu] public RangeNode<int> HorizonsWeight { get; set; } = new RangeNode<int>(85, 0, 100);
    [IgnoreMenu] public RangeNode<int> TreasuresWeight { get; set; } = new RangeNode<int>(30, 0, 100);
    [IgnoreMenu] public RangeNode<int> HordesWeight { get; set; } = new RangeNode<int>(25, 0, 100);

    [IgnoreMenu] public RangeNode<int> AncientProtectionWeight { get; set; } = new RangeNode<int>(73, 0, 100);
    [IgnoreMenu] public RangeNode<int> AncientArmamentsWeight { get; set; } = new RangeNode<int>(75, 0, 100);
    [IgnoreMenu] public RangeNode<int> AncientCuriosWeight { get; set; } = new RangeNode<int>(74, 0, 100);
    [IgnoreMenu] public RangeNode<int> BindingWeight { get; set; } = new RangeNode<int>(48, 0, 100);
    [IgnoreMenu] public RangeNode<int> RegencyWeight { get; set; } = new RangeNode<int>(47, 0, 100);
    [IgnoreMenu] public RangeNode<int> ConnectionsWeight { get; set; } = new RangeNode<int>(37, 0, 100);
    [IgnoreMenu] public RangeNode<int> MosaicsWeight { get; set; } = new RangeNode<int>(46, 0, 100);
    [IgnoreMenu] public RangeNode<int> SwiftnessWeight { get; set; } = new RangeNode<int>(44, 0, 100);
    [IgnoreMenu] public RangeNode<int> HelmsWeight { get; set; } = new RangeNode<int>(43, 0, 100);
    [IgnoreMenu] public RangeNode<int> MittsWeight { get; set; } = new RangeNode<int>(42, 0, 100);
    [IgnoreMenu] public RangeNode<int> ProtectionWeight { get; set; } = new RangeNode<int>(41, 0, 100);
    [IgnoreMenu] public RangeNode<int> BladesWeight { get; set; } = new RangeNode<int>(40, 0, 100);
    [IgnoreMenu] public RangeNode<int> MissilesWeight { get; set; } = new RangeNode<int>(39, 0, 100);
    [IgnoreMenu] public RangeNode<int> BastionsWeight { get; set; } = new RangeNode<int>(38, 0, 100);
    [IgnoreMenu] public RangeNode<int> TrinketsWeight { get; set; } = new RangeNode<int>(60, 0, 100);
    [IgnoreMenu] public RangeNode<int> CraftsmanshipWeight { get; set; } = new RangeNode<int>(45, 0, 100);

    [IgnoreMenu] public RangeNode<int> PowerWeight { get; set; } = new RangeNode<int>(57, 0, 100);
    [IgnoreMenu] public RangeNode<int> SoulsWeight { get; set; } = new RangeNode<int>(56, 0, 100);
    [IgnoreMenu] public RangeNode<int> FlamesWeight { get; set; } = new RangeNode<int>(53, 0, 100);
    [IgnoreMenu] public RangeNode<int> FightingChanceWeight { get; set; } = new RangeNode<int>(52, 0, 100);
    [IgnoreMenu] public RangeNode<int> RebirthWeight { get; set; } = new RangeNode<int>(67, 0, 100);
    [IgnoreMenu] public RangeNode<int> FoesWeight { get; set; } = new RangeNode<int>(64, 0, 100);
    [IgnoreMenu] public RangeNode<int> PursuitWeight { get; set; } = new RangeNode<int>(89, 0, 100);
    [IgnoreMenu] public RangeNode<int> TerrorWeight { get; set; } = new RangeNode<int>(72, 0, 100);
    [IgnoreMenu] public RangeNode<int> BetrayalWeight { get; set; } = new RangeNode<int>(69, 0, 100);
    [IgnoreMenu] public RangeNode<int> PhantomsWeight { get; set; } = new RangeNode<int>(50, 0, 100);
    [IgnoreMenu] public RangeNode<int> MeddlingWeight { get; set; } = new RangeNode<int>(59, 0, 100);
    [IgnoreMenu] public RangeNode<int> RiskWeight { get; set; } = new RangeNode<int>(63, 0, 100);
    [IgnoreMenu] public RangeNode<int> UncertaintyWeight { get; set; } = new RangeNode<int>(79, 0, 100);
    [IgnoreMenu] public RangeNode<int> HindranceWeight { get; set; } = new RangeNode<int>(49, 0, 100);
    [IgnoreMenu] public RangeNode<int> AvariceWeight { get; set; } = new RangeNode<int>(65, 0, 100);
    [IgnoreMenu] public RangeNode<int> ElementsWeight { get; set; } = new RangeNode<int>(55, 0, 100);
    [IgnoreMenu] public RangeNode<int> WispsWeight { get; set; } = new RangeNode<int>(68, 0, 100);
    [IgnoreMenu] public RangeNode<int> OasesWeight { get; set; } = new RangeNode<int>(36, 0, 100);
    [IgnoreMenu] public RangeNode<int> RustWeight { get; set; } = new RangeNode<int>(71, 0, 100);
    [IgnoreMenu] public RangeNode<int> GodhoodWeight { get; set; } = new RangeNode<int>(51, 0, 100);
    [IgnoreMenu] public RangeNode<int> MomentumWeight { get; set; } = new RangeNode<int>(54, 0, 100);
    [IgnoreMenu] public RangeNode<int> CroaksWeight { get; set; } = new RangeNode<int>(35, 0, 100);

    public static string GetTierFromWeight(int weight)
    {
        if (weight >= 90) return "S";
        if (weight >= 80) return "A";
        if (weight >= 70) return "B";
        if (weight >= 60) return "C";
        return "D";
    }

    public void ResetWeightsToDefaults()
    {
        FishesWeight.Value = 98;
        TrovesWeight.Value = 77;
        StrangeHorizonsWeight.Value = 84;
        ReflectionWeight.Value = 66;
        ProvidenceWeight.Value = 96;
        JewelsWeight.Value = 60;
        WealthWeight.Value = 82;
        ForeknowledgeWeight.Value = 80;
        ScarabsWeight.Value = 81;
        GoldWeight.Value = 100;
        EminenceWeight.Value = 88;
        FortuneWeight.Value = 70;
        SkitteringWeight.Value = 92;
        AuguryWeight.Value = 76;
        DistantHorizonsWeight.Value = 83;
        TitansWeight.Value = 62;
        ProsperityWeight.Value = 100;
        KnowledgeWeight.Value = 58;
        GlitteringWeight.Value = 61;
        GlyphsWeight.Value = 94;
        HorizonsWeight.Value = 85;
        TreasuresWeight.Value = 30;
        HordesWeight.Value = 25;
        AncientProtectionWeight.Value = 73;
        AncientArmamentsWeight.Value = 75;
        AncientCuriosWeight.Value = 74;
        BindingWeight.Value = 48;
        RegencyWeight.Value = 47;
        ConnectionsWeight.Value = 37;
        MosaicsWeight.Value = 46;
        SwiftnessWeight.Value = 44;
        HelmsWeight.Value = 43;
        MittsWeight.Value = 42;
        ProtectionWeight.Value = 41;
        BladesWeight.Value = 40;
        MissilesWeight.Value = 39;
        BastionsWeight.Value = 38;
        TrinketsWeight.Value = 60;
        CraftsmanshipWeight.Value = 45;
        PowerWeight.Value = 57;
        SoulsWeight.Value = 56;
        FlamesWeight.Value = 53;
        FightingChanceWeight.Value = 52;
        RebirthWeight.Value = 67;
        FoesWeight.Value = 64;
        PursuitWeight.Value = 89;
        TerrorWeight.Value = 72;
        BetrayalWeight.Value = 69;
        PhantomsWeight.Value = 50;
        MeddlingWeight.Value = 59;
        RiskWeight.Value = 63;
        UncertaintyWeight.Value = 79;
        HindranceWeight.Value = 49;
        AvariceWeight.Value = 65;
        ElementsWeight.Value = 55;
        WispsWeight.Value = 68;
        OasesWeight.Value = 36;
        RustWeight.Value = 71;
        GodhoodWeight.Value = 51;
        MomentumWeight.Value = 54;
        CroaksWeight.Value = 35;
    }
}
