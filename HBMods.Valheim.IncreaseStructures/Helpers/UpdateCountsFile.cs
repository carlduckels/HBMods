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

            data.Add("DebugPrintOptions = 0");
            data.Add("");
            data.Add(";Swamp Structures");
            data.Add("Bonemass = 5");
            data.Add("SunkenCrypt4 = 400");
            data.Add("Grave1 = 200");
            data.Add("SwampRuin1 = 50");
            data.Add("SwampRuin2 = 50");
            data.Add("FireHole = 200");
            data.Add("Runestone_Draugr = 50");
            data.Add("InfestedTree01 = 700");
            data.Add("SwampHut1 = 50");
            data.Add("SwampHut2 = 50");
            data.Add("SwampHut3 = 50");
            data.Add("SwampHut4 = 50");
            data.Add("SwampHut5 = 25");
            data.Add("SwampWell1 = 25");
            data.Add("Runestone_Swamps = 100");

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
