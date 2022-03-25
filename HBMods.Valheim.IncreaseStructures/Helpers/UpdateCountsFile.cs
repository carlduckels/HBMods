using System.Collections.Generic;
using System.IO;


namespace HBMods.Valheim.IncreaseStructures.Helpers
{
    public class UpdateCountsFile
    {
        public Dictionary<string, int> NewCounts = new Dictionary<string, int>();

        public bool ReadFile(string path)
        {
            if (!File.Exists(path))
                CreateReadData(path);

            if (!File.Exists(path))
                return false;

            int counter = 0;

            // Read the file and display it line by line.  
            foreach (string line in System.IO.File.ReadLines(path))
            {
                ProcessLine(line);
                counter++;
            }

            return true;
        }
                
        private static void CreateReadData(string fileName)
        {
            List<string> data = new List<string>();

            data.Add("; AshLands");
            data.Add("Meteorite = 500");
            data.Add("");
            data.Add("");
            data.Add("; BlackForest");
            data.Add("Crypt2 = 200");
            data.Add("Crypt3 = 200");
            data.Add("Crypt4 = 200");
            data.Add("GDKing = 4");
            data.Add("Greydwarf_camp1 = 300");
            data.Add("Ruin1 = 200");
            data.Add("Ruin2 = 200");
            data.Add("Runestone_BlackForest = 50");
            data.Add("Runestone_Greydwarfs = 25");
            data.Add("StoneHouse3 = 200");
            data.Add("StoneHouse4 = 200");
            data.Add("StoneTowerRuins03 = 80");
            data.Add("StoneTowerRuins07 = 80");
            data.Add("StoneTowerRuins08 = 80");
            data.Add("StoneTowerRuins09 = 80");
            data.Add("StoneTowerRuins10 = 80");
            data.Add("TrollCave02 = 250");
            data.Add("Vendor_BlackForest = 10");
            data.Add("");
            data.Add("");
            data.Add("; Meadows");
            data.Add("Eikthyrnir = 3");
            data.Add("Runestone_Boars = 50");
            data.Add("Runestone_Meadows = 100");
            data.Add("ShipSetting01 = 100");
            data.Add("StartTemple = 1");
            data.Add("StoneCircle = 25");
            data.Add("WoodFarm1 = 10");
            data.Add("WoodHouse1 = 20");
            data.Add("WoodHouse10 = 20");
            data.Add("WoodHouse11 = 20");
            data.Add("WoodHouse12 = 20");
            data.Add("WoodHouse13 = 20");
            data.Add("WoodHouse2 = 20");
            data.Add("WoodHouse3 = 20");
            data.Add("WoodHouse4 = 20");
            data.Add("WoodHouse5 = 20");
            data.Add("WoodHouse6 = 20");
            data.Add("WoodHouse7 = 20");
            data.Add("WoodHouse8 = 20");
            data.Add("WoodHouse9 = 20");
            data.Add("WoodVillage1 = 15");
            data.Add("");
            data.Add("");
            data.Add("; Meadows | BlackForest");
            data.Add("Dolmen01 = 100");
            data.Add("Dolmen02 = 100");
            data.Add("Dolmen03 = 50");
            data.Add("");
            data.Add("");
            data.Add("; Mountain");
            data.Add("AbandonedLogCabin02 = 33");
            data.Add("AbandonedLogCabin03 = 33");
            data.Add("AbandonedLogCabin04 = 50");
            data.Add("Dragonqueen = 3");
            data.Add("DrakeLorestone = 50");
            data.Add("DrakeNest01 = 200");
            data.Add("MountainCave02 = 160");
            data.Add("MountainGrave01 = 100");
            data.Add("MountainWell1 = 25");
            data.Add("Runestone_Mountains = 100");
            data.Add("StoneTowerRuins04 = 50");
            data.Add("StoneTowerRuins05 = 50");
            data.Add("Waymarker01 = 50");
            data.Add("Waymarker02 = 50");
            data.Add("");
            data.Add("");
            data.Add("; Plains");
            data.Add("GoblinCamp2 = 200");
            data.Add("GoblinKing = 4");
            data.Add("Ruin3 = 50");
            data.Add("Runestone_Plains = 100");
            data.Add("StoneHenge1 = 5");
            data.Add("StoneHenge2 = 5");
            data.Add("StoneHenge3 = 5");
            data.Add("StoneHenge4 = 5");
            data.Add("StoneHenge5 = 20");
            data.Add("StoneHenge6 = 20");
            data.Add("StoneTower1 = 50");
            data.Add("StoneTower3 = 50");
            data.Add("TarPit1 = 100");
            data.Add("TarPit2 = 100");
            data.Add("TarPit3 = 100");
            data.Add("");
            data.Add("");
            data.Add("; Swamp");
            data.Add("Bonemass = 5");
            data.Add("FireHole = 200");
            data.Add("Grave1 = 200");
            data.Add("InfestedTree01 = 700");
            data.Add("Runestone_Draugr = 50");
            data.Add("Runestone_Swamps = 100");
            data.Add("SunkenCrypt4 = 400");
            data.Add("SwampHut1 = 50");
            data.Add("SwampHut2 = 50");
            data.Add("SwampHut3 = 50");
            data.Add("SwampHut4 = 50");
            data.Add("SwampHut5 = 25");
            data.Add("SwampRuin1 = 50");
            data.Add("SwampRuin2 = 50");
            data.Add("SwampWell1 = 25");
            data.Add("");
            data.Add("");
            data.Add("; Swamp | BlackForest | Plains | Ocean");
            data.Add("ShipWreck01 = 25");
            data.Add("ShipWreck02 = 25");
            data.Add("ShipWreck03 = 25");
            data.Add("ShipWreck04 = 25");
            data.Add("");
            data.Add("");
            data.Add("; Debug Options");
            data.Add("DebugPrintOptions = 0");

            File.WriteAllLines(fileName, data);
        }

        private bool ProcessLine(string line)
        {
            if (string.IsNullOrEmpty(line))
                return false;

            if (line.Trim().StartsWith(";"))
                return false;

            if (!line.Contains("="))
                return false;

            string[] strs = line.Split('=');

            if (strs.Length != 2)
                return false;

            var left = strs[0].Trim();
            var right = strs[1].Trim();

            if (string.IsNullOrEmpty(left) || string.IsNullOrEmpty(right))
                return false;

            if (int.TryParse(right, out int count))
            {
                NewCounts.Add(left, count);
                return true;
            }

            return false;
        }
    }
}
