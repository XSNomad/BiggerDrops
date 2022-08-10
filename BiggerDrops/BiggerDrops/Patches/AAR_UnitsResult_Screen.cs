using BattleTech;
using BattleTech.UI;
using Harmony;
using System;
using System.Collections.Generic;
using UnityEngine;
using BiggerDrops.Data;
using BiggerDrops.Features;

namespace BiggerDrops.Patches
{
    [HarmonyPatch(typeof(AAR_UnitsResult_Screen), "InitializeData")]
    public static class AAR_UnitsResult_Screen_InitializeData
    {
        public static bool Prefix(AAR_UnitsResult_Screen __instance, MissionResults mission, SimGameState sim, Contract contract)
        {
            try
            {
                //List<AAR_UnitStatusWidget> UnitWidgets = (List<AAR_UnitStatusWidget>)AccessTools.Field(typeof(AAR_UnitsResult_Screen), "UnitWidgets").GetValue(__instance);
                var UnitWidgets = __instance.UnitWidgets;
                GameObject nextButton = __instance.transform.FindRecursive("buttonPanel").gameObject;
                nextButton.transform.localPosition = new Vector3(150, 400, 0);

                Transform parent = UnitWidgets[0].transform.parent;
                parent.localPosition = new Vector3(0, 115, 0);
                foreach (AAR_UnitStatusWidget oldWidget in UnitWidgets)
                {
                    oldWidget.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
                }
                GameObject newparent = UnityEngine.Object.Instantiate(parent.gameObject);
                newparent.transform.parent = parent.parent;
                newparent.name = "newparent";
                newparent.transform.localPosition = new Vector3(0, -325, 0);
                foreach (Transform t in newparent.transform)
                {
                    UnitWidgets.Add(t.gameObject.GetComponent<AAR_UnitStatusWidget>());
                }
                //AccessTools.Field(typeof(AAR_UnitsResult_Screen), "UnitWidgets").SetValue(__instance, __instance.UnitWidgets);

                List<UnitResult> UnitResults = new();
                for (int i = 0; i < 8; i++)
                {
                    if (i < contract.PlayerUnitResults.Count)
                    {
                        UnitResults.Add(contract.PlayerUnitResults[i]);
                    }
                    else
                    {
                        UnitResults.Add(null);
                    }
                }
                //AccessTools.Field(typeof(AAR_UnitsResult_Screen), "simState").SetValue(__instance, sim);
                __instance.simState = sim;
                //AccessTools.Field(typeof(AAR_UnitsResult_Screen), "missionResultParent").SetValue(__instance, mission);
                __instance.missionResultParent = mission;
                //AccessTools.Field(typeof(AAR_UnitsResult_Screen), "theContract").SetValue(__instance, contract);
                __instance.theContract = contract;
                //AccessTools.Field(typeof(AAR_UnitsResult_Screen), "numUnits").SetValue(__instance, contract.PlayerUnitResults.Count);
                __instance.numUnits = contract.PlayerUnitResults.Count;
                //AccessTools.Field(typeof(AAR_UnitsResult_Screen), "UnitResults").SetValue(__instance, UnitResults);
                __instance.UnitResults = UnitResults;
                __instance.Visible = false;
                __instance.InitializeWidgets();
                return false;
            }
            catch //(Exception e)
            {
                return true;
            }
        }
    }

    [HarmonyPatch(typeof(AAR_UnitsResult_Screen), "FillInData")]
    public static class AAR_UnitsResult_Screen_FillInData
    {
        public static bool Prefix(AAR_UnitsResult_Screen __instance)
        {
            try
            {
                //Contract theContract = (Contract)AccessTools.Field(typeof(AAR_UnitsResult_Screen), "theContract").GetValue(__instance);
                //var theContract = __instance.theContract;
                //List<AAR_UnitStatusWidget> UnitWidgets = __instance.UnitWidgets;
                    //(List<AAR_UnitStatusWidget>)AccessTools.Field(typeof(AAR_UnitsResult_Screen), "UnitWidgets").GetValue(__instance);
                //List<UnitResult> UnitResults = __instance.UnitResults;
                    //(List<UnitResult>)AccessTools.Field(typeof(AAR_UnitsResult_Screen), "UnitResults").GetValue(__instance);
                //int experienceEarned = __instance.theContract.ExperienceEarned;
                for (int i = 0; i < 8; i++)
                {
                    __instance.UnitWidgets[i].SetMechIconValueTextActive(false);
                    if (__instance.UnitResults[i] != null)
                    {
                        __instance.UnitWidgets[i].SetNoUnitDeployedOverlayActive(false);
                        __instance.UnitWidgets[i].FillInData(__instance.theContract.ExperienceEarned);
                    }
                    else
                    {
                        __instance.UnitWidgets[i].SetNoUnitDeployedOverlayActive(true);
                    }
                }
                //AccessTools.Field(typeof(AAR_UnitsResult_Screen), "UnitWidgets").SetValue(__instance, UnitWidgets);
                return false;
            }
            catch //(Exception e)
            {
                return true;
            }
        }
    }
}