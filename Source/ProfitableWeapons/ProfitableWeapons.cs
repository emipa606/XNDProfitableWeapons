using HarmonyLib;
using Mlie;
using UnityEngine;
using Verse;

namespace ProfitableWeapons;

public class ProfitableWeapons : Mod
{
    private static ProfitableWeaponsSettings settings;
    public static Harmony HarmonyInstance;
    public static string CurrentVersion;

    public ProfitableWeapons(ModContentPack content) : base(content)
    {
#if DEBUG
                Log.Error("XeoNovaDan left debugging enabled in Profitable Weapons - please let him know!");
#endif

        settings = GetSettings<ProfitableWeaponsSettings>();
        HarmonyInstance = new Harmony("XeoNovaDan.ProfitableWeapons");
        CurrentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    public override string SettingsCategory()
    {
        return "ProfitableWeaponsSettingsCategory".Translate();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        settings.DoWindowContents(inRect);
    }
}