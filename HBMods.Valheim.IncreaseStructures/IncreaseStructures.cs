using System;
using System.Collections.Generic;
using BepInEx;
using HarmonyLib;
using HBMods.Valheim.IncreaseStructures.Helpers;
using UnityEngine;
using static ZoneSystem;


namespace HBMods.Valheim.IncreaseStructures
{
    [BepInPlugin("HoneyBadger.IncreaseStructures", "HoneyBadger - Increase Structure Spawns", "1.0.0")]
    [BepInProcess("valheim.exe")]
    public class IncreaseStructures : BaseUnityPlugin
    {
        private readonly Harmony _harmony = new Harmony("HoneyBadger.IncreaseStructureSpawns");

        private static bool _printDebug = false;
        private static UpdateCountsFile _countUpdates = new UpdateCountsFile();

        void Awake()
        {
            _harmony.PatchAll();

            ReadData();

            if (_countUpdates.NewCounts.ContainsKey("DebugPrintOptions"))
            {
                _printDebug = (_countUpdates.NewCounts["DebugPrintOptions"] != 0);
            }
        }

        private static void ReadData()
        {
            _countUpdates = new UpdateCountsFile();

            string fileName = "BepInEx\\config\\UpdateCounts.cfg";

            _countUpdates.ReadFile(fileName);
        }

        [HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.GenerateLocations), new Type[] { })]
        class ZoneSystem_GenerateLocations
        {
            public static bool Prefix(List<ZoneLocation> ___m_locations)
            {
                if (_printDebug)
                {
                    _printDebug = false;

                    foreach (ZoneLocation item in ___m_locations)
                    {
                        Debug.Log($"Item Debug: {item.m_prefabName,-24} = {item.m_quantity}");
                    }
                }

                foreach (ZoneLocation item in ___m_locations)
                {
                    if (_countUpdates.NewCounts.ContainsKey(item.m_prefabName))
                    {
                        int oldQty = item.m_quantity;
                        int targetQty = _countUpdates.NewCounts[item.m_prefabName];

                        if (item.m_quantity != targetQty)
                        {
                            item.m_quantity = targetQty;

                            Debug.Log($"Qty Changed:   Was = {oldQty,-5}  ==>   Now = {item.m_quantity,-5}    {item.m_prefabName}");
                        }
                    }
                }

                return true;
            }
        }
    }
}
