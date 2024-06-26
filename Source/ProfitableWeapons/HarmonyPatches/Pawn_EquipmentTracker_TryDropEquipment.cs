﻿using HarmonyLib;
using Verse;

namespace ProfitableWeapons;

[HarmonyPatch(typeof(Pawn_EquipmentTracker), nameof(Pawn_EquipmentTracker.TryDropEquipment))]
public static class Pawn_EquipmentTracker_TryDropEquipment
{
    public static void Postfix(Pawn ___pawn, ref ThingWithComps eq)
    {
        // Try to flag equipped weapon as looted
        if (eq.TryGetComp<CompLootedWeapon>() is { } lootedComp)
        {
            lootedComp.CheckLootedWeapon(___pawn);
        }
    }
}