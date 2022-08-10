using BattleTech;
using BattleTech.UI;
using Harmony;
using System;
using System.Reflection;
using UnityEngine;
using BiggerDrops.Data;

namespace BiggerDrops.Features
{
    /*[HarmonyPatch(typeof(PathNodeGrid))]
    [HarmonyPatch("ResetPathGrid")]
    [HarmonyPatch(new Type[] { typeof(Vector3), typeof(float), typeof(PathingCapabilitiesDef), typeof(float), typeof(MoveType) })]
    [HarmonyPatch(MethodType.Normal)]
    public static class CombatHUDMechwarriorTray_ResetPathGrid {
      public static bool Prefix(PathNodeGrid __instance,ref CombatGameState ___combat) {
        Logger.M.TWL(0, "PathNodeGrid.ResetPathGrid");
        if (___combat == null) {
          Logger.M.WL(1, "combat is null ... skipping");
          return false;
        } else {
          Logger.M.WL(1, "combat is not null");
        }
        return true;
      }
    }*/
    [HarmonyPatch(typeof(CombatHUDMechwarriorTray))]
    [HarmonyPatch("SetTrayState")]
    [HarmonyPatch(MethodType.Normal)]
    public static class Init
    {
        public static bool Prefix(CombatHUDMechwarriorTray __instance)
        {
            try
            {
                /*for (int index = 0; index < __instance.PortraitHolders.Length; ++index) {
                  Vector3[] corners = new Vector3[4];
                  RectTransform prectt = __instance.PortraitHolders[index].GetComponent<RectTransform>();
                  prectt.GetLocalCorners(corners);
                  Logger.M.WL(1, "portrait "+ prectt.name+ ":" + __instance.PortraitHolders[index].GetInstanceID() + ". index:" + index + " pos:" +prectt.localPosition+" corners 0:" + corners[0] + " 1:" + corners[1] + " 2:" + corners[2] + " 3:" + corners[3]);
                }*/
                if (__instance.PortraitHolders.Length <= 4) { return true; }
                CombatHUDMoraleBar combatHUDMoraleBar = (CombatHUDMoraleBar)typeof(CombatHUDMechwarriorTray).GetProperty("moraleDisplay", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance, new object[0] { });
                RectTransform rtr = combatHUDMoraleBar.gameObject.GetComponent<RectTransform>();
                RectTransform prt = __instance.PortraitHolders[0].GetComponent<RectTransform>();
                Vector3[] pcorners = new Vector3[4];
                prt.GetLocalCorners(pcorners);
                Vector3 pos = rtr.localPosition;
                pos.x = pcorners[0].x + prt.localPosition.x - 10f;
                rtr.localPosition = pos;
                combatHUDMoraleBar.gameObject.SetActive(true);
            }
            catch //(Exception e)
            {
            }
            return true;
        }
    }
    [HarmonyPatch(typeof(CombatHUDMechwarriorTray))]
    [HarmonyPatch("RefreshTeam")]
    [HarmonyPatch(new Type[] { typeof(Team) })]
    [HarmonyPatch(MethodType.Normal)]
    public static class CombatHUDMechwarriorTray_RefreshTeam
    {
        public static bool Prefix(CombatHUDMechwarriorTray __instance, Team team, CombatGameState ___Combat)
        {
            try
            {
                typeof(CombatHUDMechwarriorTray).GetField("displayedTeam", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(__instance, team);
                CombatHUDPortrait[] Portraits = (CombatHUDPortrait[])typeof(CombatHUDMechwarriorTray).GetField("Portraits", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(__instance);
                for (int index = 0; index < Portraits.Length; ++index)
                {
                    if (index < team.unitCount)
                    {
                        Portraits[index].DisplayedActor = team.units[index];
                    }
                    else
                    {
                        Portraits[index].DisplayedActor = null;
                    }
                }
                return false;
            }
            catch //(Exception e)
            {
                return true;
            }
        }
    }
    [HarmonyPatch(typeof(CombatHUDMechwarriorTray))]
    [HarmonyPatch("Init")]
    [HarmonyPatch(new Type[] { typeof(CombatGameState), typeof(CombatHUD) })]
    [HarmonyPatch(MethodType.Normal)]
    public static class CombatHUDMechwarriorTray_Init
    {
        public static bool Prefix(CombatHUDMechwarriorTray __instance, CombatGameState Combat, CombatHUD HUD)
        {
            if (DropManager.AdditionalPlayerMechs() == 0)
            {
                return true;
            }
            try
            {
                //int portraitsCount = 8;
                int portraitsCount = Combat.LocalPlayerTeam.unitCount > (DropManager.DefaultMechSlots + DropManager.MaxAdditionalMechSlots) ?
                                                            DropManager.DefaultMechSlots + DropManager.MaxAdditionalMechSlots : Combat.LocalPlayerTeam.unitCount;
                if (__instance.PortraitHolders.Length >= portraitsCount)
                {
                    return true;
                }
                GameObject[] portraitHolders = new GameObject[portraitsCount];
                HBSDOTweenToggle[] portraitTweens = new HBSDOTweenToggle[portraitsCount];
                GameObject layout = __instance.PortraitHolders[0].transform.parent.gameObject;
                for (int index = 0; index < portraitHolders.Length; ++index)
                {
                    if (index < __instance.PortraitHolders.Length)
                    {
                        portraitHolders[index] = __instance.PortraitHolders[index];
                        continue;
                    }
                    GameObject srcPortraitHolder = portraitHolders[index % (__instance.PortraitHolders.Length)];
                    GameObject newPortraitHolder = UnityEngine.Object.Instantiate(srcPortraitHolder, srcPortraitHolder.transform.parent);
                    Vector3[] corners = new Vector3[4];
                    srcPortraitHolder.GetComponent<RectTransform>().GetWorldCorners(corners);
                    float height = corners[2].z - corners[0].z;
                    newPortraitHolder.SetActive(true);
                    portraitHolders[index] = newPortraitHolder;
                    newPortraitHolder.GetComponent<RectTransform>().GetWorldCorners(corners);
                }
                __instance.PortraitHolders = portraitHolders;
                float spacing = 117.2f;
                if (__instance.portraitTweens.Length > 1)
                {
                    Vector3[] corners0 = new Vector3[4];
                    __instance.portraitTweens[0].gameObject.GetComponent<RectTransform>().GetWorldCorners(corners0);
                    Vector3[] corners1 = new Vector3[4];
                    __instance.portraitTweens[1].gameObject.GetComponent<RectTransform>().GetWorldCorners(corners1);
                    spacing = corners1[0].x - corners0[0].x;
                }
                float diff = 0f;
                Vector3[] cornersl = new Vector3[4];
                __instance.portraitTweens[__instance.portraitTweens.Length - 1].gameObject.GetComponent<RectTransform>().GetWorldCorners(cornersl);
                Vector3[] cornersf = new Vector3[4];
                __instance.portraitTweens[0].gameObject.GetComponent<RectTransform>().GetWorldCorners(cornersf);
                diff = cornersl[0].x - cornersf[0].x + spacing;
                for (int index = 0; index < portraitTweens.Length; ++index)
                {
                    if (index < __instance.portraitTweens.Length)
                    {
                        portraitTweens[index] = __instance.portraitTweens[index];
                        continue;
                    }
                    HBSDOTweenToggle srcPortraitTween = portraitTweens[index % (__instance.portraitTweens.Length)];
                    GameObject newPortraitTweenGO = UnityEngine.Object.Instantiate(srcPortraitTween.gameObject, srcPortraitTween.gameObject.transform.parent);
                    HBSDOTweenToggle newPortraitTween = newPortraitTweenGO.GetComponent<HBSDOTweenToggle>();
                    newPortraitTweenGO.transform.localPosition += Vector3.right * diff;
                    newPortraitTween.TweenObjects[0] = portraitHolders[index];
                    portraitTweens[index] = newPortraitTween;
                }
                __instance.portraitTweens = portraitTweens;
                return true;
            }
            catch //(Exception e)
            {
                return true;
            }
        }
        public static void Postfix(CombatHUDMechwarriorTray __instance, CombatGameState Combat, CombatHUD HUD)
        {
            try
            {
                if (__instance.PortraitHolders.Length <= 4) { return; }
                for (int index = 0; index < __instance.PortraitHolders.Length; ++index)
                {
                    Vector3[] corners = new Vector3[4];
                    __instance.PortraitHolders[index].GetComponent<RectTransform>().GetWorldCorners(corners);
                }
                for (int index = 0; index < __instance.portraitTweens.Length; ++index)
                {
                    Vector3[] corners = new Vector3[4];
                    __instance.portraitTweens[index].gameObject.GetComponent<RectTransform>().GetWorldCorners(corners);
                }
                CombatHUDMoraleBar combatHUDMoraleBar = (CombatHUDMoraleBar)typeof(CombatHUDMechwarriorTray).GetProperty("moraleDisplay", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(__instance, new object[0] { });
                combatHUDMoraleBar.gameObject.SetActive(false);
            }
            catch //(Exception e)
            {
            }
        }
    }
}