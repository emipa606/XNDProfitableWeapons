using UnityEngine;
using Verse;

namespace ProfitableWeapons;

public class ProfitableWeaponsSettings : ModSettings
{
    private const int SliderRectHeight = 18;

    public static bool FlagInventoryWeapons = true;

    public static bool FlagFromWellUsed = true;

    public static float NonLootedSellPriceMultiplier = 1;

    public static float LootedSellPriceMultiplier = 0.2f;

    private static bool mendingRemoveLootedFlag = true;

    private static bool nanoRepairRemoveLootedFlag = true;

    public void DoWindowContents(Rect rect)
    {
        var options = new Listing_Standard();
        var defaultColor = GUI.color;
        options.Begin(rect);

        GUI.color = defaultColor;
        Text.Font = GameFont.Small;
        Text.Anchor = TextAnchor.UpperLeft;
        options.Gap();

        // Flag inventory weapons
        options.CheckboxLabeled("SettingFlagInventoryWeapons".Translate(), ref FlagInventoryWeapons,
            "SettingFlagInventoryWeaponsToolTip".Translate());
        options.Gap();

        // Flag well-used weapons
        options.CheckboxLabeled("SettingFlagWellUsedWeapons".Translate(), ref FlagFromWellUsed,
            "SettingFlagWellUsedWeaponsToolTip".Translate());
        options.Gap(12 + SliderRectHeight);

        // Non-looted sell price multiplier
        NonLootedSellPriceMultiplier = Widgets.HorizontalSlider(options.GetRect(SliderRectHeight),
            NonLootedSellPriceMultiplier,
            0, 1,
            leftAlignedLabel: "SettingNonLootedSellMult".Translate(),
            rightAlignedLabel: NonLootedSellPriceMultiplier.ToStringPercent(), roundTo: 0.01f);
        Text.Font = GameFont.Tiny;
        options.Label("SettingNonLootedSellMultNote".Translate());
        Text.Font = GameFont.Small;
        options.Gap(12 + SliderRectHeight);

        // Looted sell price multiplier
        LootedSellPriceMultiplier = Widgets.HorizontalSlider(options.GetRect(SliderRectHeight),
            LootedSellPriceMultiplier,
            0, 1,
            leftAlignedLabel: "SettingLootedSellMult".Translate(),
            rightAlignedLabel: LootedSellPriceMultiplier.ToStringPercent(), roundTo: 0.01f);
        Text.Font = GameFont.Tiny;
        options.Label(
            "SettingLootedSellMultNote".Translate((NonLootedSellPriceMultiplier * LootedSellPriceMultiplier)
                .ToStringPercent()));
        Text.Font = GameFont.Small;
        options.Gap();

        // MendAndRecycle integration
        Text.Font = GameFont.Medium;
        options.Label("MendAndRecycle");
        Text.Font = GameFont.Small;
        options.Gap(6);
        if (ModCompatibilityCheck.Mending)
        {
            options.CheckboxLabeled("MendingRemoveLootedFlag".Translate(), ref mendingRemoveLootedFlag,
                "MendingRemoveLootedFlagToolTip".Translate());
        }
        else
        {
            GUI.color = Color.grey;
            options.Label("MendingIsNotActive".Translate());
            GUI.color = defaultColor;
        }

        options.Gap();

        // Nano Repair Tech integration
        Text.Font = GameFont.Medium;
        options.Label("Nano Repair Tech");
        Text.Font = GameFont.Small;
        options.Gap(6);
        if (ModCompatibilityCheck.Mending)
        {
            options.CheckboxLabeled("MendingRemoveLootedFlag".Translate(), ref nanoRepairRemoveLootedFlag,
                "NanoRepairTechRemoveLootedFlagToolTip".Translate());
        }
        else
        {
            GUI.color = Color.grey;
            options.Label("NanoRepairTechIsNotActive".Translate());
            GUI.color = defaultColor;
        }

        if (ProfitableWeapons.CurrentVersion != null)
        {
            options.Gap();
            GUI.contentColor = Color.gray;
            options.Label("xnd.CurrentModVersion".Translate(ProfitableWeapons.CurrentVersion));
            GUI.contentColor = Color.white;
        }

        options.End();

        Mod.GetSettings<ProfitableWeaponsSettings>().Write();
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref FlagInventoryWeapons, "flagInventoryWeapons", true);
        Scribe_Values.Look(ref NonLootedSellPriceMultiplier, "nonLootedSellPriceMult", 1);
        Scribe_Values.Look(ref LootedSellPriceMultiplier, "lootedSellPriceMult", 0.2f);
        Scribe_Values.Look(ref FlagFromWellUsed, "flagFromWellUsed", true);
        Scribe_Values.Look(ref mendingRemoveLootedFlag, "mendingRemoveLootedFlag", true);
        Scribe_Values.Look(ref nanoRepairRemoveLootedFlag, "nanoRepairRemoveLootedFlag", true);
    }
}