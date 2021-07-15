using Verse;

namespace ProfitableWeapons
{
    public class SpecialThingFilterWorker_LootedWeapon : SpecialThingFilterWorker
    {
        public override bool Matches(Thing t)
        {
            return t.TryGetComp<CompLootedWeapon>() is {IsUsedWeapon: true};
        }
    }
}