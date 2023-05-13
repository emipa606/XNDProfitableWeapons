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
            switch (curMod.Name)
            {
                case "Combat Extended":
                    CombatExtended = true;
                    break;
                case "MendAndRecycle":
                    Mending = true;
                    break;
                case "Nano Repair Tech":
                    NanoRepairTech = true;
                    break;
            }
        }
    }
}