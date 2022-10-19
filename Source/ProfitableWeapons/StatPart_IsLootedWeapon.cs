using RimWorld;
using Verse;

namespace ProfitableWeapons;

public class StatPart_IsLootedWeapon : StatPart
{
    public override void TransformValue(StatRequest req, ref float val)
    {
        if (req.HasThing && req.Thing.TryGetComp<CompLootedWeapon>() is { IsUsedWeapon: true })
        {
            val *= ProfitableWeaponsSettings.lootedSellPriceMult;
        }
    }

    public override string ExplanationPart(StatRequest req)
    {
        if (req.HasThing && req.Thing.TryGetComp<CompLootedWeapon>() is { IsUsedWeapon: true })
        {
            return "StatsReport_IsLootedWeapon".Translate() + ": x" +
                   ProfitableWeaponsSettings.lootedSellPriceMult.ToStringPercent();
        }

        return null;
    }
}