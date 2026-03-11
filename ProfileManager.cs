using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace WishHelper;

public record ProfileData(string Name, string Description, Dictionary<string, int> Weights);

public class ProfileManager
{
    private readonly string _configPath;
    private readonly string _profilesFilePath;
    private List<ProfileData> _profiles = new();
    private readonly WishHelperSettings _settings;

    public ProfileManager(WishHelperSettings settings, string configDirectory)
    {
        _settings = settings;
        _configPath = configDirectory;
        _profilesFilePath = Path.Combine(_configPath, "profiles.json");
    }

    public IReadOnlyList<ProfileData> Profiles => _profiles;

    public void LoadProfiles()
    {
        if (!File.Exists(_profilesFilePath))
        {
            CreateDefaultProfiles();
            return;
        }

        try
        {
            var json = File.ReadAllText(_profilesFilePath);
            _profiles = System.Text.Json.JsonSerializer.Deserialize<List<ProfileData>>(json) ?? new List<ProfileData>();
        }
        catch
        {
            _profiles = new List<ProfileData>();
            CreateDefaultProfiles();
        }
    }

    public void SaveProfiles()
    {
        try
        {
            Directory.CreateDirectory(_configPath);
            var json = System.Text.Json.JsonSerializer.Serialize(_profiles, new System.Text.Json.JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(_profilesFilePath, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[WishHelper] Failed to save profiles: {ex.Message}");
        }
    }

    public void SaveCurrentProfile(string name, string description = "")
    {
        var weights = GetCurrentWeights();
        var existingProfile = _profiles.FindIndex(p => p.Name == name);
        
        var profile = new ProfileData(name, description, weights);
        
        if (existingProfile >= 0)
        {
            _profiles[existingProfile] = profile;
        }
        else
        {
            _profiles.Add(profile);
        }
        
        SaveProfiles();
    }

    public void LoadProfile(string name)
    {
        var profile = _profiles.FirstOrDefault(p => p.Name == name);
        if (profile == null || name == "Default")
        {
            _settings.ResetWeightsToDefaults();
            return;
        }

        ApplyWeights(profile.Weights);
    }

    public void DeleteProfile(string name)
    {
        if (name == "Default") return;
        
        _profiles.RemoveAll(p => p.Name == name);
        SaveProfiles();
    }

    private Dictionary<string, int> GetCurrentWeights()
    {
        return new Dictionary<string, int>
        {
            ["FishesWeight"] = _settings.FishesWeight.Value,
            ["TrovesWeight"] = _settings.TrovesWeight.Value,
            ["StrangeHorizonsWeight"] = _settings.StrangeHorizonsWeight.Value,
            ["ReflectionWeight"] = _settings.ReflectionWeight.Value,
            ["ProvidenceWeight"] = _settings.ProvidenceWeight.Value,
            ["JewelsWeight"] = _settings.JewelsWeight.Value,
            ["WealthWeight"] = _settings.WealthWeight.Value,
            ["ForeknowledgeWeight"] = _settings.ForeknowledgeWeight.Value,
            ["ScarabsWeight"] = _settings.ScarabsWeight.Value,
            ["GoldWeight"] = _settings.GoldWeight.Value,
            ["EminenceWeight"] = _settings.EminenceWeight.Value,
            ["FortuneWeight"] = _settings.FortuneWeight.Value,
            ["SkitteringWeight"] = _settings.SkitteringWeight.Value,
            ["AuguryWeight"] = _settings.AuguryWeight.Value,
            ["DistantHorizonsWeight"] = _settings.DistantHorizonsWeight.Value,
            ["TitansWeight"] = _settings.TitansWeight.Value,
            ["ProsperityWeight"] = _settings.ProsperityWeight.Value,
            ["KnowledgeWeight"] = _settings.KnowledgeWeight.Value,
            ["GlitteringWeight"] = _settings.GlitteringWeight.Value,
            ["GlyphsWeight"] = _settings.GlyphsWeight.Value,
            ["HorizonsWeight"] = _settings.HorizonsWeight.Value,
            ["TreasuresWeight"] = _settings.TreasuresWeight.Value,
            ["HordesWeight"] = _settings.HordesWeight.Value,
            ["AncientProtectionWeight"] = _settings.AncientProtectionWeight.Value,
            ["AncientArmamentsWeight"] = _settings.AncientArmamentsWeight.Value,
            ["AncientCuriosWeight"] = _settings.AncientCuriosWeight.Value,
            ["BindingWeight"] = _settings.BindingWeight.Value,
            ["RegencyWeight"] = _settings.RegencyWeight.Value,
            ["ConnectionsWeight"] = _settings.ConnectionsWeight.Value,
            ["MosaicsWeight"] = _settings.MosaicsWeight.Value,
            ["SwiftnessWeight"] = _settings.SwiftnessWeight.Value,
            ["HelmsWeight"] = _settings.HelmsWeight.Value,
            ["MittsWeight"] = _settings.MittsWeight.Value,
            ["ProtectionWeight"] = _settings.ProtectionWeight.Value,
            ["BladesWeight"] = _settings.BladesWeight.Value,
            ["MissilesWeight"] = _settings.MissilesWeight.Value,
            ["BastionsWeight"] = _settings.BastionsWeight.Value,
            ["TrinketsWeight"] = _settings.TrinketsWeight.Value,
            ["CraftsmanshipWeight"] = _settings.CraftsmanshipWeight.Value,
            ["PowerWeight"] = _settings.PowerWeight.Value,
            ["SoulsWeight"] = _settings.SoulsWeight.Value,
            ["FlamesWeight"] = _settings.FlamesWeight.Value,
            ["FightingChanceWeight"] = _settings.FightingChanceWeight.Value,
            ["RebirthWeight"] = _settings.RebirthWeight.Value,
            ["FoesWeight"] = _settings.FoesWeight.Value,
            ["PursuitWeight"] = _settings.PursuitWeight.Value,
            ["TerrorWeight"] = _settings.TerrorWeight.Value,
            ["BetrayalWeight"] = _settings.BetrayalWeight.Value,
            ["PhantomsWeight"] = _settings.PhantomsWeight.Value,
            ["MeddlingWeight"] = _settings.MeddlingWeight.Value,
            ["RiskWeight"] = _settings.RiskWeight.Value,
            ["UncertaintyWeight"] = _settings.UncertaintyWeight.Value,
            ["HindranceWeight"] = _settings.HindranceWeight.Value,
            ["AvariceWeight"] = _settings.AvariceWeight.Value,
            ["ElementsWeight"] = _settings.ElementsWeight.Value,
            ["WispsWeight"] = _settings.WispsWeight.Value,
            ["OasesWeight"] = _settings.OasesWeight.Value,
            ["RustWeight"] = _settings.RustWeight.Value,
            ["GodhoodWeight"] = _settings.GodhoodWeight.Value,
            ["MomentumWeight"] = _settings.MomentumWeight.Value,
            ["CroaksWeight"] = _settings.CroaksWeight.Value
        };
    }

    private void ApplyWeights(Dictionary<string, int> weights)
    {
        if (weights.TryGetValue("FishesWeight", out var val)) _settings.FishesWeight.Value = val;
        if (weights.TryGetValue("TrovesWeight", out val)) _settings.TrovesWeight.Value = val;
        if (weights.TryGetValue("StrangeHorizonsWeight", out val)) _settings.StrangeHorizonsWeight.Value = val;
        if (weights.TryGetValue("ReflectionWeight", out val)) _settings.ReflectionWeight.Value = val;
        if (weights.TryGetValue("ProvidenceWeight", out val)) _settings.ProvidenceWeight.Value = val;
        if (weights.TryGetValue("JewelsWeight", out val)) _settings.JewelsWeight.Value = val;
        if (weights.TryGetValue("WealthWeight", out val)) _settings.WealthWeight.Value = val;
        if (weights.TryGetValue("ForeknowledgeWeight", out val)) _settings.ForeknowledgeWeight.Value = val;
        if (weights.TryGetValue("ScarabsWeight", out val)) _settings.ScarabsWeight.Value = val;
        if (weights.TryGetValue("GoldWeight", out val)) _settings.GoldWeight.Value = val;
        if (weights.TryGetValue("EminenceWeight", out val)) _settings.EminenceWeight.Value = val;
        if (weights.TryGetValue("FortuneWeight", out val)) _settings.FortuneWeight.Value = val;
        if (weights.TryGetValue("SkitteringWeight", out val)) _settings.SkitteringWeight.Value = val;
        if (weights.TryGetValue("AuguryWeight", out val)) _settings.AuguryWeight.Value = val;
        if (weights.TryGetValue("DistantHorizonsWeight", out val)) _settings.DistantHorizonsWeight.Value = val;
        if (weights.TryGetValue("TitansWeight", out val)) _settings.TitansWeight.Value = val;
        if (weights.TryGetValue("ProsperityWeight", out val)) _settings.ProsperityWeight.Value = val;
        if (weights.TryGetValue("KnowledgeWeight", out val)) _settings.KnowledgeWeight.Value = val;
        if (weights.TryGetValue("GlitteringWeight", out val)) _settings.GlitteringWeight.Value = val;
        if (weights.TryGetValue("GlyphsWeight", out val)) _settings.GlyphsWeight.Value = val;
        if (weights.TryGetValue("HorizonsWeight", out val)) _settings.HorizonsWeight.Value = val;
        if (weights.TryGetValue("TreasuresWeight", out val)) _settings.TreasuresWeight.Value = val;
        if (weights.TryGetValue("HordesWeight", out val)) _settings.HordesWeight.Value = val;
        if (weights.TryGetValue("AncientProtectionWeight", out val)) _settings.AncientProtectionWeight.Value = val;
        if (weights.TryGetValue("AncientArmamentsWeight", out val)) _settings.AncientArmamentsWeight.Value = val;
        if (weights.TryGetValue("AncientCuriosWeight", out val)) _settings.AncientCuriosWeight.Value = val;
        if (weights.TryGetValue("BindingWeight", out val)) _settings.BindingWeight.Value = val;
        if (weights.TryGetValue("RegencyWeight", out val)) _settings.RegencyWeight.Value = val;
        if (weights.TryGetValue("ConnectionsWeight", out val)) _settings.ConnectionsWeight.Value = val;
        if (weights.TryGetValue("MosaicsWeight", out val)) _settings.MosaicsWeight.Value = val;
        if (weights.TryGetValue("SwiftnessWeight", out val)) _settings.SwiftnessWeight.Value = val;
        if (weights.TryGetValue("HelmsWeight", out val)) _settings.HelmsWeight.Value = val;
        if (weights.TryGetValue("MittsWeight", out val)) _settings.MittsWeight.Value = val;
        if (weights.TryGetValue("ProtectionWeight", out val)) _settings.ProtectionWeight.Value = val;
        if (weights.TryGetValue("BladesWeight", out val)) _settings.BladesWeight.Value = val;
        if (weights.TryGetValue("MissilesWeight", out val)) _settings.MissilesWeight.Value = val;
        if (weights.TryGetValue("BastionsWeight", out val)) _settings.BastionsWeight.Value = val;
        if (weights.TryGetValue("TrinketsWeight", out val)) _settings.TrinketsWeight.Value = val;
        if (weights.TryGetValue("CraftsmanshipWeight", out val)) _settings.CraftsmanshipWeight.Value = val;
        if (weights.TryGetValue("PowerWeight", out val)) _settings.PowerWeight.Value = val;
        if (weights.TryGetValue("SoulsWeight", out val)) _settings.SoulsWeight.Value = val;
        if (weights.TryGetValue("FlamesWeight", out val)) _settings.FlamesWeight.Value = val;
        if (weights.TryGetValue("FightingChanceWeight", out val)) _settings.FightingChanceWeight.Value = val;
        if (weights.TryGetValue("RebirthWeight", out val)) _settings.RebirthWeight.Value = val;
        if (weights.TryGetValue("FoesWeight", out val)) _settings.FoesWeight.Value = val;
        if (weights.TryGetValue("PursuitWeight", out val)) _settings.PursuitWeight.Value = val;
        if (weights.TryGetValue("TerrorWeight", out val)) _settings.TerrorWeight.Value = val;
        if (weights.TryGetValue("BetrayalWeight", out val)) _settings.BetrayalWeight.Value = val;
        if (weights.TryGetValue("PhantomsWeight", out val)) _settings.PhantomsWeight.Value = val;
        if (weights.TryGetValue("MeddlingWeight", out val)) _settings.MeddlingWeight.Value = val;
        if (weights.TryGetValue("RiskWeight", out val)) _settings.RiskWeight.Value = val;
        if (weights.TryGetValue("UncertaintyWeight", out val)) _settings.UncertaintyWeight.Value = val;
        if (weights.TryGetValue("HindranceWeight", out val)) _settings.HindranceWeight.Value = val;
        if (weights.TryGetValue("AvariceWeight", out val)) _settings.AvariceWeight.Value = val;
        if (weights.TryGetValue("ElementsWeight", out val)) _settings.ElementsWeight.Value = val;
        if (weights.TryGetValue("WispsWeight", out val)) _settings.WispsWeight.Value = val;
        if (weights.TryGetValue("OasesWeight", out val)) _settings.OasesWeight.Value = val;
        if (weights.TryGetValue("RustWeight", out val)) _settings.RustWeight.Value = val;
        if (weights.TryGetValue("GodhoodWeight", out val)) _settings.GodhoodWeight.Value = val;
        if (weights.TryGetValue("MomentumWeight", out val)) _settings.MomentumWeight.Value = val;
        if (weights.TryGetValue("CroaksWeight", out val)) _settings.CroaksWeight.Value = val;
    }

    private void CreateDefaultProfiles()
    {

        if (_profiles.Count == 0)
        {
            SaveProfiles();
        }
    }
}
