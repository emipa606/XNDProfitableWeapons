using System.Linq;
using Verse;

namespace ProfitableWeapons;

[StaticConstructorOnStartup]
public class ModCompatibilityCheck
{
    public static readonly bool CombatExtended;

    public static readonly bool Mending;

    public static readonly bool NanoRepairTech;

    static ModCompatibilityCheck()
    {
        var activeMods = ModsConfig.ActiveModsInLoadOrder.ToList();
        foreach (var curMod in activeMods)
        {
            if (curMod.Name == "Combat Extended")
            {
                CombatExtended = true;
            }
            else if (curMod.Name == "MendAndRecycle")
            {
                Mending = true;
            }
            else if (curMod.Name == "Nano Repair Tech")
            {
                NanoRepairTech = true;
            }
        }
    }
}