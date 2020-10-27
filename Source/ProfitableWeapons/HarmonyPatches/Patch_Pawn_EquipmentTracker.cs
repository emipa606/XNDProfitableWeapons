using Verse;
using HarmonyLib;

namespace ProfitableWeapons
{

    [StaticConstructorOnStartup]
    public static class Patch_Pawn_EquipmentTracker
    {

        [HarmonyPatch(typeof(Pawn_EquipmentTracker), nameof(Pawn_EquipmentTracker.TryDropEquipment))]
        public static class TryDropEquipment
        {

            public static void Postfix(Pawn ___pawn, ref ThingWithComps eq)
            {
                // Try to flag equipped weapon as looted
                if (eq.TryGetComp<CompLootedWeapon>() is CompLootedWeapon lootedComp)
                    lootedComp.CheckLootedWeapon(___pawn);
            }

        }

    }
}
