using BattleTech;
using BattleTech.Save;
using Harmony;
using BiggerDrops.Data;
using BiggerDrops.Features;

namespace BiggerDrops.Patches
{
    [HarmonyPatch(typeof(SimGameState), "Rehydrate", typeof(GameInstanceSave))]
    class SimGameState_RehydratePatch
    {
        public static void Postfix(SimGameState __instance, GameInstanceSave gameInstanceSave)
        {
            if (BiggerDrops.settings.allowUpgrades)
            {
               DropManager.setCompanyStats(__instance.CompanyStats);
            }
        }
    }

    [HarmonyPatch(typeof(SimGameState), "InitCompanyStats")]
    class SimGameState_InitCompanyStatsPatch
    {
        public static void Postfix(SimGameState __instance)
        {
            if (BiggerDrops.settings.allowUpgrades)
            {
                DropManager.setCompanyStats(__instance.CompanyStats);
            }
        }
    }
}