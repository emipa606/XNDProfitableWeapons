using System.Linq;
using RimWorld;
using Verse;

namespace ProfitableWeapons;

[StaticConstructorOnStartup]
internal static class StaticConstructorClass
{
    static StaticConstructorClass()
    {
        // Dynamically patch all ThingDefs that are weapons but not stuff like woodlog and bottles
        foreach (var weaponDef in DefDatabase<ThingDef>.AllDefs.Where(d =>
                     d.IsWeapon && !d.IsDrug && !d.IsStuff && !d.HasComp(typeof(CompLootedWeapon))))
        {
            // Set sell price multiplier based on settings
            weaponDef.SetStatBaseValue(StatDefOf.SellPriceFactor, ProfitableWeaponsSettings.nonLootedSellPriceMult);

            // CompLootedWeapon
            weaponDef.comps ??= [];
            weaponDef.comps.Add(new CompProperties
            {
                compClass = typeof(CompLootedWeapon)
            });
        }
    }
}