﻿using HarmonyLib;
using Verse;

namespace ProfitableWeapons;

[HarmonyPatch(typeof(Pawn_InventoryTracker), nameof(Pawn_InventoryTracker.DropAllNearPawn))]
public static class Pawn_InventoryTracker_DropAllNearPawn
{
    public static void Prefix(Pawn ___pawn, ref ThingOwner ___innerContainer)
    {
        // If set to flag inventory weapons as looted, go through each item that was in inventory and attempt to flag as looted
        if (!ProfitableWeaponsSettings.flagInventoryWeapons)
        {
            return;
        }

        foreach (var thing in ___innerContainer)
        {
            if (thing.TryGetComp<CompLootedWeapon>() is { } lootedComp)
            {
                lootedComp.CheckLootedWeapon(___pawn);
            }
        }
    }
}