﻿using Verse;

namespace ProfitableWeapons
{
    public class CompLootedWeapon : ThingComp
    {
        private const int BaseAttacksUntilWellUsedThreshold = 20;
        private int attackCounter;

        private bool isLootedWeaponInt;

        private bool WellUsedWeapon => attackCounter >= BaseAttacksUntilWellUsedThreshold *
                                       (parent.def.IsRangedWeapon ? parent.def.Verbs[0].burstShotCount : 1)
                                       && ProfitableWeaponsSettings.flagFromWellUsed;

        public bool IsUsedWeapon => isLootedWeaponInt || WellUsedWeapon;

        public void ModifyAttackCounter(Verb verb)
        {
            if (ProfitableWeaponsSettings.flagFromWellUsed && !WellUsedWeapon)
                // Prevent odd situations where burst ranged weapons can melee lots without getting flagged
            {
                attackCounter += verb.EquipmentSource.def.IsRangedWeapon && verb.IsMeleeAttack
                    ? verb.EquipmentSource.def.Verbs[0].burstShotCount
                    : 1;
            }
        }

        public override string TransformLabel(string label)
        {
            if (IsUsedWeapon)
            {
                label += " (" + "LootedWeaponChar".Translate() + ")";
            }

            return label;
        }

        public void CheckLootedWeapon(Pawn pawn)
        {
            if (pawn.Faction == null || pawn.Faction.IsPlayer)
            {
                return;
            }

            if (!pawn.IsPrisonerOfColony)
            {
                isLootedWeaponInt = true;
            }
            else if (pawn.guest == null)
            {
                isLootedWeaponInt = true;
            }
        }

        public void RemoveLootedWeaponFlag()
        {
            isLootedWeaponInt = false;
            attackCounter = 0;
        }

        public override void PostExposeData()
        {
            Scribe_Values.Look(ref isLootedWeaponInt, "looted");
            Scribe_Values.Look(ref attackCounter, "attacks");
        }
    }
}