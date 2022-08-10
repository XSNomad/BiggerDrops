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
    [HarmonyPatch(typeof(SkirmishMechBayPanel), "SelectLance")]
    public static class SkirmishMechBayPanel_SelectLance
    {
    public static void Prefix(SkirmishMechBayPanel __instance, LanceDef lance)
        {
            try
            {
                int maxUnits = DropManager.DefaultMechSlots + DropManager.MaxAdditionalMechSlots;
                if (lance != null)
                {
                    maxUnits = lance.LanceUnits.Length;
                }
                if (__instance.loadoutSlots.Length >= maxUnits) { return; }
                if (__instance.loadoutSlots.Length < 2) { maxUnits = __instance.loadoutSlots.Length; return; };
                float ydelta = __instance.loadoutSlots[1].GetComponent<RectTransform>().localPosition.y - __instance.loadoutSlots[0].GetComponent<RectTransform>().localPosition.y;
                int addUnits = maxUnits - __instance.loadoutSlots.Length;
                GameObject srcLayout = __instance.loadoutSlots[__instance.loadoutSlots.Length - 1].gameObject;
                List<LanceLoadoutSlot> slots = new();
                slots.AddRange(__instance.loadoutSlots);
                for (int t = 0; t < addUnits; ++t)
                {
                    GameObject nLayout = UnityEngine.Object.Instantiate(srcLayout, srcLayout.transform.parent);
                    RectTransform rt = nLayout.GetComponent<RectTransform>();
                    Vector3 pos = rt.localPosition;
                    pos.y = srcLayout.GetComponent<RectTransform>().localPosition.y + (t + 1) * ydelta;
                    rt.localPosition = pos;
                    slots.Add(nLayout.GetComponent<LanceLoadoutSlot>());
                }
                __instance.loadoutSlots = slots.ToArray();
            }
            catch//(Exception e)
            {

            }
        }
    }
}