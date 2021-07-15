﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using Verse;

namespace ProfitableWeapons
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        private static readonly Type patchType = typeof(HarmonyPatches);

        static HarmonyPatches()
        {
#if DEBUG
                Harmony.DEBUG = true;
#endif

            // Do automatic patches
            ProfitableWeapons.harmonyInstance.PatchAll();

            // Manual patches
            var tryCastShotPostfix = new HarmonyMethod(patchType, nameof(Postfix_TryCastShot));
            ProfitableWeapons.harmonyInstance.Patch(AccessTools.Method(typeof(Verb_LaunchProjectile), "TryCastShot"),
                postfix: tryCastShotPostfix);
            ProfitableWeapons.harmonyInstance.Patch(AccessTools.Method(typeof(Verb_MeleeAttack), "TryCastShot"),
                postfix: tryCastShotPostfix);

            // Try and patch Combat Extended
            if (ModCompatibilityCheck.CombatExtended)
            {
                // Melee verb
                var meleeVerbCE = GenTypes.GetTypeInAnyAssembly("CombatExtended.Verb_MeleeAttackCE");
                if (meleeVerbCE != null)
                {
                    ProfitableWeapons.harmonyInstance.Patch(AccessTools.Method(meleeVerbCE, "TryCastShot"),
                        postfix: tryCastShotPostfix);
                }
                else
                {
                    Log.Error("Profitable Weapons - Couldn't find CombatExtended.Verb_MeleeAttackCE type to patch");
                }

                // Ranged verb
                var launchProjectileVerbCE = GenTypes.GetTypeInAnyAssembly("CombatExtended.Verb_LaunchProjectileCE");
                if (launchProjectileVerbCE != null)
                {
                    ProfitableWeapons.harmonyInstance.Patch(AccessTools.Method(launchProjectileVerbCE, "TryCastShot"),
                        postfix: tryCastShotPostfix);
                }
                else
                {
                    Log.Error(
                        "Profitable Weapons - Couldn't find CombatExtended.Verb_LaunchProjectileCE type to patch");
                }
            }

            // Try and patch Mending
            if (ModCompatibilityCheck.Mending)
            {
                // Mending JobDriver
                var mendingJobDriver = GenTypes.GetTypeInAnyAssembly("MendAndRecycle.JobDriver_Mend");
                if (mendingJobDriver != null)
                {
                    ProfitableWeapons.harmonyInstance.Patch(
                        mendingJobDriver.GetNestedTypes(BindingFlags.NonPublic | BindingFlags.Instance).First()
                            .GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                            .MaxBy(mi => mi.GetMethodBody()?.GetILAsByteArray().Length ?? -1),
                        transpiler: new HarmonyMethod(patchType,
                            nameof(Transpile_MendAndReycle_JobDriver_Mend_MendToil_TickAction)));
                }
                else
                {
                    Log.Error("Profitable Weapons - Couldn't find MendAndRecycle.JobDriver_Mend type to patch");
                }
            }

            // Try and patch Nano Repair Tech
            if (!ModCompatibilityCheck.NanoRepairTech)
            {
                return;
            }

            // Nano repairing
            var nanoRepair = GenTypes.GetTypeInAnyAssembly("Ogre.NanoRepairTech.NanoRepair");
            if (nanoRepair != null)
            {
                ProfitableWeapons.harmonyInstance.Patch(AccessTools.Method(nanoRepair, "ProcessTick"),
                    transpiler: new HarmonyMethod(patchType, nameof(Transpile_NanoRepairTech_NanoRepair_ProcessTick)));
            }
            else
            {
                Log.Error("Profitable Weapons - Couldn't find Ogre.NanoRepairTech.NanoRepair type to patch");
            }
        }

        public static void Postfix_TryCastShot(Verb __instance, bool __result)
        {
            if (__result && __instance.EquipmentSource?.TryGetComp<CompLootedWeapon>() is { } lootedComp)
            {
                lootedComp.ModifyAttackCounter(__instance);
            }
        }

        #region Patches on other mods

        public static void RemoveUsedWeaponFlag(bool settingAllowed, Thing thing) // Helper method for transpiler
        {
            // If settings allow for Mending to remove looted weapon flags and the thing in question has CompLootedWeapon, remove the looted flag
            if (settingAllowed && thing.TryGetComp<CompLootedWeapon>() is { } lootedComp)
            {
                lootedComp.RemoveLootedWeaponFlag();
            }
        }

        // MendAndRecycle
        public static IEnumerable<CodeInstruction> Transpile_MendAndReycle_JobDriver_Mend_MendToil_TickAction(
            IEnumerable<CodeInstruction> instructions)
        {
#if DEBUG
                Log.Message("Transpiler start: Transpile_MendAndReycle.JobDriver_Mend.MendToil.TickAction (1 match)");
#endif

            var instructionList = instructions.ToList();

            var removeDeadmanSettingFieldInfo =
                AccessTools.Field(GenTypes.GetTypeInAnyAssembly("MendAndRecycle.Settings"), "removesDeadman");

            foreach (var instruction in instructionList)
            {
                // If instruction checks for 'remove deadman' setting, add call to our helper method before it
                if (instruction.opcode == OpCodes.Ldsfld && instruction.OperandIs(removeDeadmanSettingFieldInfo))
                {
#if DEBUG
                        Log.Message("Transpile_MendAndReycle.JobDriver_Mend.MendToil.TickAction match 1 of 1");
#endif

                    yield return new CodeInstruction(OpCodes.Ldsfld,
                        AccessTools.Field(typeof(ProfitableWeaponsSettings),
                            "mendingRemoveLootedFlag")); // ProfitableWeaponsSettings.mendingRemoveLootedFlag
                    yield return new CodeInstruction(OpCodes.Ldloc_0); // thing
                    yield return new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(patchType,
                            nameof(RemoveUsedWeaponFlag
                            ))); // RemoveUsedWeaponFlag(ProfitableWeaponsSettings.mendingRemoveLootedFlag, thing)
                }

                yield return instruction;
            }
        }

        public static void RemoveUsedWeaponFlagNano(bool settingAllowed, Thing thing)
        {
            if (thing.HitPoints >= thing.MaxHitPoints)
            {
                RemoveUsedWeaponFlag(settingAllowed, thing);
            }
        }

        // Nano Repair Tech
        public static IEnumerable<CodeInstruction> Transpile_NanoRepairTech_NanoRepair_ProcessTick(
            IEnumerable<CodeInstruction> instructions)
        {
#if DEBUG
                Log.Message("Transpiler start: Transpile_NanoRepairTech.NanoRepair.ProcessTick (1 match)");
#endif

            var instructionList = instructions.ToList();
            var done = false;

            var unused = AccessTools.Property(typeof(Thing), nameof(Thing.HitPoints)).GetGetMethod();

            for (var i = 0; i < instructionList.Count; i++)
            {
                var instruction = instructionList[i];

                // Add extra instructions after the 3rd nop when nano repairing weapons
                if (!done && i >= 7 &&
                    instructionList[i - 4].opcode == OpCodes.Stloc_3 &&
                    instructionList[i - 5].opcode == OpCodes.Sub &&
                    instructionList[i - 6].opcode == OpCodes.Ldc_I4_1 &&
                    instructionList[i - 7].opcode == OpCodes.Ldloc_3)
                {
#if DEBUG
                        Log.Message("Transpile_NanoRepairTech.NanoRepair.ProcessTick match 1 of 1");
#endif

                    yield return new CodeInstruction(OpCodes.Ldsfld,
                        AccessTools.Field(typeof(ProfitableWeaponsSettings),
                            "nanoRepairRemoveLootedFlag")); // ProfitableWeaponsSettings.nanoRepairRemoveLootedFlag
                    yield return new CodeInstruction(OpCodes.Ldloc_S, 17); // thing
                    yield return new CodeInstruction(OpCodes.Call,
                        AccessTools.Method(patchType,
                            nameof(RemoveUsedWeaponFlagNano
                            ))); // RemoveUsedWeaponFlagNano(ProfitableWeaponsSettings.nanoRepairRemoveLootedFlag, thing)
                    done = true;
                }

                yield return instruction;
            }
        }

        #endregion
    }
}