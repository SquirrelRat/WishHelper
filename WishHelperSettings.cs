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
    public ToggleNode ShowWeightOnCards { get; set; } = new ToggleNode(true);
    public RangeNode<int> FrameThickness { get; set; } = new RangeNode<int>(3, 1, 10);

    [Menu("Select Recommended Hotkey")]
    public HotkeyNode SelectRecommendedHotkey { get; set; } = new HotkeyNode(Keys.None);

    public ColorNode STierColor { get; set; } = new ColorNode(Color.Purple);
    public ColorNode ATierColor { get; set; } = new ColorNode(Color.Green);
    public ColorNode BTierColor { get; set; } = new ColorNode(Color.Yellow);
    public ColorNode CTierColor { get; set; } = new ColorNode(Color.Orange);
    public ColorNode DTierColor { get; set; } = new ColorNode(Color.Red);

    [IgnoreMenu] public RangeNode<int> FishesWeight { get; set; } = new RangeNode<int>(980, 0, 1000);
    [IgnoreMenu] public RangeNode<int> TrovesWeight { get; set; } = new RangeNode<int>(770, 0, 1000);
    [IgnoreMenu] public RangeNode<int> StrangeHorizonsWeight { get; set; } = new RangeNode<int>(840, 0, 1000);
    [IgnoreMenu] public RangeNode<int> ReflectionWeight { get; set; } = new RangeNode<int>(660, 0, 1000);
    [IgnoreMenu] public RangeNode<int> ProvidenceWeight { get; set; } = new RangeNode<int>(960, 0, 1000);
    [IgnoreMenu] public RangeNode<int> JewelsWeight { get; set; } = new RangeNode<int>(600, 0, 1000);

    [IgnoreMenu] public RangeNode<int> WealthWeight { get; set; } = new RangeNode<int>(890, 0, 1000);
    [IgnoreMenu] public RangeNode<int> ForeknowledgeWeight { get; set; } = new RangeNode<int>(880, 0, 1000);
    [IgnoreMenu] public RangeNode<int> ScarabsWeight { get; set; } = new RangeNode<int>(870, 0, 1000);
    [IgnoreMenu] public RangeNode<int> GoldWeight { get; set; } = new RangeNode<int>(720, 0, 1000);
    [IgnoreMenu] public RangeNode<int> EminenceWeight { get; set; } = new RangeNode<int>(910, 0, 1000);
    [IgnoreMenu] public RangeNode<int> FortuneWeight { get; set; } = new RangeNode<int>(700, 0, 1000);
    [IgnoreMenu] public RangeNode<int> SkitteringWeight { get; set; } = new RangeNode<int>(920, 0, 1000);
    [IgnoreMenu] public RangeNode<int> AuguryWeight { get; set; } = new RangeNode<int>(760, 0, 1000);
    [IgnoreMenu] public RangeNode<int> DistantHorizonsWeight { get; set; } = new RangeNode<int>(830, 0, 1000);
    [IgnoreMenu] public RangeNode<int> TitansWeight { get; set; } = new RangeNode<int>(620, 0, 1000);
    [IgnoreMenu] public RangeNode<int> ProsperityWeight { get; set; } = new RangeNode<int>(1000, 0, 1000);

    [IgnoreMenu] public RangeNode<int> KnowledgeWeight { get; set; } = new RangeNode<int>(580, 0, 1000);
    [IgnoreMenu] public RangeNode<int> GlitteringWeight { get; set; } = new RangeNode<int>(610, 0, 1000);
    [IgnoreMenu] public RangeNode<int> GlyphsWeight { get; set; } = new RangeNode<int>(940, 0, 1000);
    [IgnoreMenu] public RangeNode<int> HorizonsWeight { get; set; } = new RangeNode<int>(860, 0, 1000);
    [IgnoreMenu] public RangeNode<int> TreasuresWeight { get; set; } = new RangeNode<int>(690, 0, 1000);
    [IgnoreMenu] public RangeNode<int> HordesWeight { get; set; } = new RangeNode<int>(680, 0, 1000);

    [IgnoreMenu] public RangeNode<int> AncientProtectionWeight { get; set; } = new RangeNode<int>(730, 0, 1000);
    [IgnoreMenu] public RangeNode<int> AncientArmamentsWeight { get; set; } = new RangeNode<int>(750, 0, 1000);
    [IgnoreMenu] public RangeNode<int> AncientCuriosWeight { get; set; } = new RangeNode<int>(740, 0, 1000);
    [IgnoreMenu] public RangeNode<int> BindingWeight { get; set; } = new RangeNode<int>(480, 0, 1000);
    [IgnoreMenu] public RangeNode<int> RegencyWeight { get; set; } = new RangeNode<int>(470, 0, 1000);
    [IgnoreMenu] public RangeNode<int> ConnectionsWeight { get; set; } = new RangeNode<int>(370, 0, 1000);
    [IgnoreMenu] public RangeNode<int> MosaicsWeight { get; set; } = new RangeNode<int>(460, 0, 1000);
    [IgnoreMenu] public RangeNode<int> SwiftnessWeight { get; set; } = new RangeNode<int>(440, 0, 1000);
    [IgnoreMenu] public RangeNode<int> HelmsWeight { get; set; } = new RangeNode<int>(430, 0, 1000);
    [IgnoreMenu] public RangeNode<int> MittsWeight { get; set; } = new RangeNode<int>(420, 0, 1000);
    [IgnoreMenu] public RangeNode<int> ProtectionWeight { get; set; } = new RangeNode<int>(410, 0, 1000);
    [IgnoreMenu] public RangeNode<int> BladesWeight { get; set; } = new RangeNode<int>(400, 0, 1000);
    [IgnoreMenu] public RangeNode<int> MissilesWeight { get; set; } = new RangeNode<int>(390, 0, 1000);
    [IgnoreMenu] public RangeNode<int> BastionsWeight { get; set; } = new RangeNode<int>(380, 0, 1000);
    [IgnoreMenu] public RangeNode<int> TrinketsWeight { get; set; } = new RangeNode<int>(570, 0, 1000);
    [IgnoreMenu] public RangeNode<int> CraftsmanshipWeight { get; set; } = new RangeNode<int>(450, 0, 1000);

    [IgnoreMenu] public RangeNode<int> PowerWeight { get; set; } = new RangeNode<int>(330, 0, 1000);
    [IgnoreMenu] public RangeNode<int> SoulsWeight { get; set; } = new RangeNode<int>(560, 0, 1000);
    [IgnoreMenu] public RangeNode<int> FlamesWeight { get; set; } = new RangeNode<int>(530, 0, 1000);
    [IgnoreMenu] public RangeNode<int> FightingChanceWeight { get; set; } = new RangeNode<int>(520, 0, 1000);
    [IgnoreMenu] public RangeNode<int> RebirthWeight { get; set; } = new RangeNode<int>(670, 0, 1000);
    [IgnoreMenu] public RangeNode<int> FoesWeight { get; set; } = new RangeNode<int>(640, 0, 1000);
    [IgnoreMenu] public RangeNode<int> PursuitWeight { get; set; } = new RangeNode<int>(900, 0, 1000);
    [IgnoreMenu] public RangeNode<int> TerrorWeight { get; set; } = new RangeNode<int>(780, 0, 1000);
    [IgnoreMenu] public RangeNode<int> BetrayalWeight { get; set; } = new RangeNode<int>(320, 0, 1000);
    [IgnoreMenu] public RangeNode<int> PhantomsWeight { get; set; } = new RangeNode<int>(500, 0, 1000);
    [IgnoreMenu] public RangeNode<int> MeddlingWeight { get; set; } = new RangeNode<int>(590, 0, 1000);
    [IgnoreMenu] public RangeNode<int> RiskWeight { get; set; } = new RangeNode<int>(630, 0, 1000);
    [IgnoreMenu] public RangeNode<int> UncertaintyWeight { get; set; } = new RangeNode<int>(930, 0, 1000);
    [IgnoreMenu] public RangeNode<int> HindranceWeight { get; set; } = new RangeNode<int>(490, 0, 1000);
    [IgnoreMenu] public RangeNode<int> AvariceWeight { get; set; } = new RangeNode<int>(650, 0, 1000);
    [IgnoreMenu] public RangeNode<int> ElementsWeight { get; set; } = new RangeNode<int>(550, 0, 1000);
    [IgnoreMenu] public RangeNode<int> WispsWeight { get; set; } = new RangeNode<int>(340, 0, 1000);
    [IgnoreMenu] public RangeNode<int> OasesWeight { get; set; } = new RangeNode<int>(360, 0, 1000);
    [IgnoreMenu] public RangeNode<int> RustWeight { get; set; } = new RangeNode<int>(710, 0, 1000);
    [IgnoreMenu] public RangeNode<int> GodhoodWeight { get; set; } = new RangeNode<int>(510, 0, 1000);
    [IgnoreMenu] public RangeNode<int> MomentumWeight { get; set; } = new RangeNode<int>(540, 0, 1000);
    [IgnoreMenu] public RangeNode<int> CroaksWeight { get; set; } = new RangeNode<int>(350, 0, 1000);

    public static string GetTierFromWeight(int weight)
    {
        if (weight >= 900) return "S";
        if (weight >= 800) return "A";
        if (weight >= 700) return "B";
        if (weight >= 600) return "C";
        return "D";
    }

    public void ResetWeightsToDefaults()
    {
        FishesWeight.Value = 980;
        TrovesWeight.Value = 770;
        StrangeHorizonsWeight.Value = 840;
        ReflectionWeight.Value = 660;
        ProvidenceWeight.Value = 960;
        JewelsWeight.Value = 600;
        WealthWeight.Value = 890;
        ForeknowledgeWeight.Value = 880;
        ScarabsWeight.Value = 870;
        GoldWeight.Value = 720;
        EminenceWeight.Value = 910;
        FortuneWeight.Value = 700;
        SkitteringWeight.Value = 920;
        AuguryWeight.Value = 760;
        DistantHorizonsWeight.Value = 830;
        TitansWeight.Value = 620;
        ProsperityWeight.Value = 1000;
        KnowledgeWeight.Value = 580;
        GlitteringWeight.Value = 610;
        GlyphsWeight.Value = 940;
        HorizonsWeight.Value = 860;
        TreasuresWeight.Value = 690;
        HordesWeight.Value = 680;
        AncientProtectionWeight.Value = 730;
        AncientArmamentsWeight.Value = 750;
        AncientCuriosWeight.Value = 740;
        BindingWeight.Value = 480;
        RegencyWeight.Value = 470;
        ConnectionsWeight.Value = 370;
        MosaicsWeight.Value = 460;
        SwiftnessWeight.Value = 440;
        HelmsWeight.Value = 430;
        MittsWeight.Value = 420;
        ProtectionWeight.Value = 410;
        BladesWeight.Value = 400;
        MissilesWeight.Value = 390;
        BastionsWeight.Value = 380;
        TrinketsWeight.Value = 570;
        CraftsmanshipWeight.Value = 450;
        PowerWeight.Value = 330;
        SoulsWeight.Value = 560;
        FlamesWeight.Value = 530;
        FightingChanceWeight.Value = 520;
        RebirthWeight.Value = 670;
        FoesWeight.Value = 640;
        PursuitWeight.Value = 900;
        TerrorWeight.Value = 780;
        BetrayalWeight.Value = 320;
        PhantomsWeight.Value = 500;
        MeddlingWeight.Value = 590;
        RiskWeight.Value = 630;
        UncertaintyWeight.Value = 930;
        HindranceWeight.Value = 490;
        AvariceWeight.Value = 650;
        ElementsWeight.Value = 550;
        WispsWeight.Value = 340;
        OasesWeight.Value = 360;
        RustWeight.Value = 710;
        GodhoodWeight.Value = 510;
        MomentumWeight.Value = 540;
        CroaksWeight.Value = 350;
    }
}
