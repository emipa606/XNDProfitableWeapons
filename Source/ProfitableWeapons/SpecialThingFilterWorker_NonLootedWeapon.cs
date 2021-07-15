using Verse;

namespace ProfitableWeapons
{
    public class SpecialThingFilterWorker_NonLootedWeapon : SpecialThingFilterWorker
    {
        public override bool Matches(Thing t)
        {
            return t.TryGetComp<CompLootedWeapon>() is {IsUsedWeapon: false};
        }
    }
}