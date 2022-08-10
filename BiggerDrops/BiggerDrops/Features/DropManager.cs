using Newtonsoft.Json;
using System.Collections.Generic;
using BattleTech;
using System;
using System.Linq;
using BiggerDrops.Data;

namespace BiggerDrops.Features
{
    using static BiggerDrops;
    public static class DropManager
    {
        public static readonly int DefaultMechSlots = 4;
        public static readonly string EmployerLanceGuid = "ecc8d4f2-74b4-465d-adf6-84445e5dfc230";
        public static readonly string AdditionalMechSlotsStat = "BiggerDrops_AdditionalMechSlots";
        public static readonly string AdditionPlayerMechsStat = "BiggerDrops_AdditionalPlayerMechSlots";
        public static readonly string MaxTonnageStat = "BiggerDrops_MaxTonnage";
        public static readonly string CuVehicleStat = "BiggerDrops_CuVehicleCount";
        public static readonly string legacyUpgradeDone = "BiggerDrops_LegacyUpgrade";
        public static readonly int MinCuBays = 3;
        public static readonly int MaxAdditionalMechSlots = 4;

        private static StatCollection companyStats;
        private static List<string> SlotOrder = new();

        public static void FindSlotOrder(List<DropSlotDef> defs)
        {
            Dictionary<int, List<string>> Buckets = new();
            foreach (DropSlotDef def in defs)
            {
                if (Buckets.ContainsKey(def.Order))
                {
                    Buckets[def.Order].Add(def.Description.Id);
                }
                else
                {
                    List<string> temp = new() { def.Description.Id };
                    Buckets.Add(def.Order, temp);
                }
            }

            SlotOrder = new List<string>();
            foreach (KeyValuePair<int, List<string>> pair in Buckets.OrderBy(i => i.Key))
            {
                foreach (string id in pair.Value)
                {
                    SlotOrder.Add(id);
                }
            }
        }

        public static int AdditionalMechSlots()
        {
            if (settings.allowUpgrades && companyStats != null)
            {
                int maxSize = MaxAdditionalMechSlots;
                int val = companyStats.GetValue<int>(AdditionalMechSlotsStat);
                return val > maxSize ? maxSize : val;
            }

            return Math.Max(Math.Min(MaxAdditionalMechSlots, settings.additinalMechSlots), 0);
        }

        public static int AdditionalPlayerMechs()
        {
            if (settings.allowUpgrades && companyStats != null)
            {
                int maxSize = MaxAdditionalMechSlots;
                int val = companyStats.GetValue<int>(AdditionPlayerMechsStat);
                return val > maxSize ? maxSize : val;
            }

            return Math.Max(Math.Min(MaxAdditionalMechSlots, settings.additinalPlayerMechSlots), 0);
        }

        public static int MaxTonnage()
        {
            if (settings.allowUpgrades && companyStats != null)
            {
                return companyStats.GetValue<int>(MaxTonnageStat);
            }
            return Math.Max(settings.defaultMaxTonnage, 0);
        }

        public static int VehicleCount()
        {
            if (settings.allowUpgrades && companyStats != null)
            {
                int val = companyStats.GetValue<int>(CuVehicleStat);
                return val > settings.MAX_VEHICLE_SLOTS ? settings.MAX_VEHICLE_SLOTS : val;
            }
            return settings.CuInitialVehicles;
        }

        public static void setCompanyStats(StatCollection stats)
        {
            companyStats = stats;
            if (settings.allowUpgrades)
            {

                if (!companyStats.ContainsStatistic(AdditionalMechSlotsStat))
                {
                    companyStats.AddStatistic(AdditionalMechSlotsStat,
                        Math.Max(Math.Min(MaxAdditionalMechSlots, settings.additinalMechSlots), 0));
                }

                if (!companyStats.ContainsStatistic(AdditionPlayerMechsStat))
                {
                    companyStats.AddStatistic(AdditionPlayerMechsStat,
                        Math.Max(Math.Min(MaxAdditionalMechSlots, settings.additinalPlayerMechSlots),
                            0));
                }

                if (!companyStats.ContainsStatistic(CuVehicleStat))
                {
                    companyStats.AddStatistic(CuVehicleStat, settings.CuInitialVehicles);
                };
            }

            if (!companyStats.ContainsStatistic(MaxTonnageStat))
            {
                companyStats.AddStatistic(MaxTonnageStat,
                    Math.Max(settings.defaultMaxTonnage, 0));
            }
        }
    }
}