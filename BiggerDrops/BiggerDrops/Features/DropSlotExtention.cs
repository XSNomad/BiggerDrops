using System;
using System.Collections.Generic;
using System.Reflection;
using BiggerDrops.Data;

namespace BiggerDrops.Features
{
    public static class DropSlotExtention
    {
        private static Dictionary<string, DropSlotDef> dropSlotTypes = new();
        public static readonly string FALLBACK_DROP_SLOT_TYPE_NAME = "fallback_slot";
        public static readonly string BIGGER_DROPS_LAYOUT_ID = "bigger_drops_layout_id";
        public static void Register(this DropSlotDef def)
        {
            if (dropSlotTypes.ContainsKey(def.Description.Id))
            {
                dropSlotTypes[def.Description.Id] = def;
            }
            else
            {
                dropSlotTypes.Add(def.Description.Id, def);
            }
        }
    }
}