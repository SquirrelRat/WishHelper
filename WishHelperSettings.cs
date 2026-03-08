using ExileCore.Shared.Attributes;
using ExileCore.Shared.Interfaces;
using ExileCore.Shared.Nodes;
using SharpDX;
using System.Collections.Generic;

namespace WishHelper;

public class WishHelperSettings : ISettings
{
    public ToggleNode Enable { get; set; } = new ToggleNode(true);
    
    public ToggleNode DrawBox { get; set; } = new ToggleNode(true);
    public ToggleNode DrawFrame { get; set; } = new ToggleNode(true);
    public ToggleNode ShowTierLabel { get; set; } = new ToggleNode(true);
    public RangeNode<int> FrameThickness { get; set; } = new RangeNode<int>(3, 1, 10);
    
    public ColorNode STierColor { get; set; } = new ColorNode(Color.Purple);
    public ColorNode ATierColor { get; set; } = new ColorNode(Color.Green);
    public ColorNode BTierColor { get; set; } = new ColorNode(Color.Yellow);
    public ColorNode CTierColor { get; set; } = new ColorNode(Color.Orange);
    public ColorNode DTierColor { get; set; } = new ColorNode(Color.Red);
    
    [IgnoreMenu] public ListNode FishesTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode TrovesTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode StrangeHorizonsTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode ReflectionTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode ProvidenceTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode JewelsTier { get; set; } = new ListNode();
    
    [IgnoreMenu] public ListNode WealthTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode ForeknowledgeTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode ScarabsTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode GoldTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode EminenceTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode FortuneTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode SkitteringTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode AuguryTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode DistantHorizonsTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode TitansTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode ProsperityTier { get; set; } = new ListNode();
    
    [IgnoreMenu] public ListNode KnowledgeTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode GlitteringTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode GlyphsTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode HorizonsTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode TreasuresTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode HordesTier { get; set; } = new ListNode();
    
    [IgnoreMenu] public ListNode AncientProtectionTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode AncientArmamentsTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode AncientCuriosTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode BindingTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode RegencyTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode ConnectionsTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode MosaicsTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode SwiftnessTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode HelmsTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode MittsTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode ProtectionTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode BladesTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode MissilesTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode BastionsTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode TrinketsTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode CraftsmanshipTier { get; set; } = new ListNode();
    
    [IgnoreMenu] public ListNode PowerTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode SoulsTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode FlamesTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode FightingChanceTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode RebirthTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode FoesTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode PursuitTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode TerrorTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode BetrayalTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode PhantomsTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode MeddlingTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode RiskTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode UncertaintyTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode HindranceTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode AvariceTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode ElementsTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode WispsTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode OasesTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode RustTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode GodhoodTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode MomentumTier { get; set; } = new ListNode();
    [IgnoreMenu] public ListNode CroaksTier { get; set; } = new ListNode();

    public WishHelperSettings()
    {
        var tierOptions = new List<string> { "S", "A", "B", "C", "D", "F" };
        
        FishesTier.Values = tierOptions; FishesTier.Value = "S";
        TrovesTier.Values = tierOptions; TrovesTier.Value = "S";
        StrangeHorizonsTier.Values = tierOptions; StrangeHorizonsTier.Value = "S";
        ReflectionTier.Values = tierOptions; ReflectionTier.Value = "S";
        ProvidenceTier.Values = tierOptions; ProvidenceTier.Value = "S";
        JewelsTier.Values = tierOptions; JewelsTier.Value = "S";
        
        WealthTier.Values = tierOptions; WealthTier.Value = "A";
        ForeknowledgeTier.Values = tierOptions; ForeknowledgeTier.Value = "A";
        ScarabsTier.Values = tierOptions; ScarabsTier.Value = "A";
        GoldTier.Values = tierOptions; GoldTier.Value = "A";
        EminenceTier.Values = tierOptions; EminenceTier.Value = "A";
        FortuneTier.Values = tierOptions; FortuneTier.Value = "A";
        SkitteringTier.Values = tierOptions; SkitteringTier.Value = "A";
        AuguryTier.Values = tierOptions; AuguryTier.Value = "A";
        DistantHorizonsTier.Values = tierOptions; DistantHorizonsTier.Value = "A";
        TitansTier.Values = tierOptions; TitansTier.Value = "A";
        ProsperityTier.Values = tierOptions; ProsperityTier.Value = "A";
        
        KnowledgeTier.Values = tierOptions; KnowledgeTier.Value = "B";
        GlitteringTier.Values = tierOptions; GlitteringTier.Value = "B";
        GlyphsTier.Values = tierOptions; GlyphsTier.Value = "B";
        HorizonsTier.Values = tierOptions; HorizonsTier.Value = "B";
        TreasuresTier.Values = tierOptions; TreasuresTier.Value = "B";
        HordesTier.Values = tierOptions; HordesTier.Value = "B";
        
        AncientProtectionTier.Values = tierOptions; AncientProtectionTier.Value = "C";
        AncientArmamentsTier.Values = tierOptions; AncientArmamentsTier.Value = "C";
        AncientCuriosTier.Values = tierOptions; AncientCuriosTier.Value = "C";
        BindingTier.Values = tierOptions; BindingTier.Value = "C";
        RegencyTier.Values = tierOptions; RegencyTier.Value = "C";
        ConnectionsTier.Values = tierOptions; ConnectionsTier.Value = "C";
        MosaicsTier.Values = tierOptions; MosaicsTier.Value = "C";
        SwiftnessTier.Values = tierOptions; SwiftnessTier.Value = "C";
        HelmsTier.Values = tierOptions; HelmsTier.Value = "C";
        MittsTier.Values = tierOptions; MittsTier.Value = "C";
        ProtectionTier.Values = tierOptions; ProtectionTier.Value = "C";
        BladesTier.Values = tierOptions; BladesTier.Value = "C";
        MissilesTier.Values = tierOptions; MissilesTier.Value = "C";
        BastionsTier.Values = tierOptions; BastionsTier.Value = "C";
        TrinketsTier.Values = tierOptions; TrinketsTier.Value = "C";
        CraftsmanshipTier.Values = tierOptions; CraftsmanshipTier.Value = "C";
        
        PowerTier.Values = tierOptions; PowerTier.Value = "F";
        SoulsTier.Values = tierOptions; SoulsTier.Value = "F";
        FlamesTier.Values = tierOptions; FlamesTier.Value = "F";
        FightingChanceTier.Values = tierOptions; FightingChanceTier.Value = "F";
        RebirthTier.Values = tierOptions; RebirthTier.Value = "D";
        FoesTier.Values = tierOptions; FoesTier.Value = "D";
        PursuitTier.Values = tierOptions; PursuitTier.Value = "D";
        TerrorTier.Values = tierOptions; TerrorTier.Value = "D";
        BetrayalTier.Values = tierOptions; BetrayalTier.Value = "D";
        PhantomsTier.Values = tierOptions; PhantomsTier.Value = "D";
        MeddlingTier.Values = tierOptions; MeddlingTier.Value = "D";
        RiskTier.Values = tierOptions; RiskTier.Value = "D";
        UncertaintyTier.Values = tierOptions; UncertaintyTier.Value = "D";
        HindranceTier.Values = tierOptions; HindranceTier.Value = "D";
        AvariceTier.Values = tierOptions; AvariceTier.Value = "D";
        ElementsTier.Values = tierOptions; ElementsTier.Value = "D";
        WispsTier.Values = tierOptions; WispsTier.Value = "D";
        OasesTier.Values = tierOptions; OasesTier.Value = "D";
        RustTier.Values = tierOptions; RustTier.Value = "D";
        GodhoodTier.Values = tierOptions; GodhoodTier.Value = "D";
        MomentumTier.Values = tierOptions; MomentumTier.Value = "D";
        CroaksTier.Values = tierOptions; CroaksTier.Value = "F";
    }
}
