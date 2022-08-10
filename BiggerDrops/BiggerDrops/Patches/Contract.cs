using BattleTech;
using Harmony;
using System;
using System.Collections.Generic;
using BiggerDrops.Data;
using BiggerDrops.Features;

namespace BiggerDrops.Patches
{
    [HarmonyPatch(typeof(Contract), "CompleteContract")]
    public static class Contract_CompleteContract
    {
        public static void Prefix(Contract __instance)
        {
            try
            {
                CombatGameState combat = __instance.BattleTechGame.Combat;
                List<Mech> allMechs = combat.AllMechs;
                foreach (Mech mech in allMechs)
                {
                    if (Fields.callsigns.Contains(mech.pilot.Callsign))
                    {
                        mech._teamId = combat.LocalPlayerTeam.GUID;
                        //AccessTools.Field(typeof(Mech), "_teamId").SetValue(mech, combat.LocalPlayerTeam.GUID);
                    }
                }

            }
            catch //(Exception e)
            {
            }
        }
    }
}