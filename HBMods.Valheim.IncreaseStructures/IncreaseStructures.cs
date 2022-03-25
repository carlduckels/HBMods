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

                    Debug.Log($"Debug Zone Location: PrefabName, Qty, Enable, IsAddedToMap, Biome(s)");

                    foreach (ZoneLocation item in ___m_locations)
                    {
                        DebugZoneLocation(item);
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


        private static void DebugZoneLocation(ZoneLocation item)
        {
            Debug.Log($"Debug Zone Location: {item.m_prefabName}, {item.m_quantity}, {item.m_enable}, {IsAddedToMap(item)}, {BiomeToList(item.m_biome)}");
        }


        private static bool IsAddedToMap(ZoneLocation item) => (item.m_quantity > 0) && (item.m_enable);


        private static string BiomeToList(Heightmap.Biome biome)
        {
            List<string> biomes = new List<string>();

            if (HasBiome(biome, Heightmap.Biome.Meadows)) biomes.Add(Heightmap.Biome.Meadows.ToString());
            if (HasBiome(biome, Heightmap.Biome.Swamp)) biomes.Add(Heightmap.Biome.Swamp.ToString());
            if (HasBiome(biome, Heightmap.Biome.Mountain)) biomes.Add(Heightmap.Biome.Mountain.ToString());
            if (HasBiome(biome, Heightmap.Biome.BlackForest)) biomes.Add(Heightmap.Biome.BlackForest.ToString());
            if (HasBiome(biome, Heightmap.Biome.Plains)) biomes.Add(Heightmap.Biome.Plains.ToString());
            if (HasBiome(biome, Heightmap.Biome.AshLands)) biomes.Add(Heightmap.Biome.AshLands.ToString());
            if (HasBiome(biome, Heightmap.Biome.DeepNorth)) biomes.Add(Heightmap.Biome.DeepNorth.ToString());
            if (HasBiome(biome, Heightmap.Biome.Ocean)) biomes.Add(Heightmap.Biome.Ocean.ToString());
            if (HasBiome(biome, Heightmap.Biome.Mistlands)) biomes.Add(Heightmap.Biome.Mistlands.ToString());

            return string.Join(" | ", biomes);
        }

        private static bool HasBiome(Heightmap.Biome biomeBits, Heightmap.Biome biome) => ((biomeBits & biome) == biome);
    }
}
