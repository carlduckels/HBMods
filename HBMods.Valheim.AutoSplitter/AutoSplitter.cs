using System;
using System.Collections.Generic;
using HBMods.Valheim.AutoSplitter.Helpers;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using static ZoneSystem;

namespace HBMods.Valheim.AutoSplitter
{
    [BepInPlugin("badger.ValheimModAutosplitter", "BadgerMod - Autosplitter", "1.0.0")]
    [BepInProcess("valheim.exe")]
    public class AutoSplitter : BaseUnityPlugin
    {
        private readonly Harmony _harmony = new Harmony("badger.ValheimModAutosplitter");

        static List<string> _localGlobalKeys = new List<string>();

        private static int _deathCounter = 0;
        private static bool _firstMove = false;
        private static bool _isPaused = false;
        private static int _logoutCounter = 0;


        private static List<string> _deadBossList = new List<string>();


        //==========================================================================================
        // Called at Start - Setup the TCP Connection to LiveSplit
        //==========================================================================================
        void Awake()
        {
            _harmony.PatchAll();

            TcpConnection.Connect("127.0.0.1", 1966);
        }


        [HarmonyPatch(typeof(Player), nameof(Player.SetIntro))]
        class WorldGenerator_Initialize_Patch
        {
            static void Postfix(bool ___m_intro)
            {
                Debug.Log($"SET INTRO =      {___m_intro}      <==================================================");
                Handle_Reset();
            }
        }



        //==========================================================================================
        // Called when the Player Presses a Key
        //==========================================================================================
        [HarmonyPatch(typeof(Player), nameof(Player.SetControls))]
        class Player_SetControls_Patch
        {
            static void Prefix(Vector3 ___m_moveDir)
            {
                if (_firstMove)
                    return;

                if ((___m_moveDir.x != 0f) || (___m_moveDir.y != 0f) || (___m_moveDir.z != 0f))
                    Handle_FirstMove();
            }
        }


        //==========================================================================================
        // Called when the Player Kills a Boss (or Troll or Surtling)
        //==========================================================================================
        [HarmonyPatch(typeof(ZoneSystem), nameof(ZoneSystem.SetGlobalKey))]
        class OfferingBowl_Interact_Patch
        {
            static void Postfix()
            {
                Handle_SetGlobalKey();
            }
        }


        //==========================================================================================
        // Called when the Player Dies
        //==========================================================================================
        [HarmonyPatch(typeof(Player), nameof(Player.CreateTombStone))]
        class Player_OnDeath_Patch
        {
            static void Prefix()
            {
                Handle_Death();
            }
        }


        //==========================================================================================
        // Called when the Player Logs Out
        //==========================================================================================
        [HarmonyPatch(typeof(Game), nameof(Game.Logout))]
        class Player_Logout_Patch
        {
            static void Prefix(bool ___m_shuttingDown)
            {
                if (!___m_shuttingDown)
                {
                    Handle_Pause();
                }
            }
        }


        private static void Handle_Reset()
        {
            TcpConnection.Send("reset");
        }


        private static void Handle_FirstMove()
        {
            _firstMove = true;

            if (_isPaused)
            {
                Handle_Resume();
            }
            else
            {
                TcpConnection.Send("start");
            }
        }


        private static void Handle_SetGlobalKey()
        {
            try
            {
                List<string> a = ZoneSystem.instance.GetGlobalKeys();

                foreach (string key in a)
                {
                    if (!_localGlobalKeys.Contains(key))
                    {
                        _localGlobalKeys.Add(key);

                        Handle_NewGlobalKey(key);
                    }
                }
            }
            catch (Exception)
            {
                // Ignore
            }
        }


        private static void Handle_NewGlobalKey(string key)
        {
            string bossName = "";

            Debug.Log($"New Key [{_localGlobalKeys.Count}]: {key}");

            switch (key)
            {
                case "defeated_eikthyr":
                    bossName = "Eikthyr";
                    break;
                case "defeated_gdking":
                    bossName = "TheElder";
                    break;
                case "defeated_bonemass":
                    bossName = "Bonemass";
                    break;
                case "defeated_dragon":
                    bossName = "Moder";
                    break;
                case "defeated_goblinking":
                    bossName = "Yagluth";
                    break;
                case "KilledTroll":
                case "killed_surtling":
                    // Ignored
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrEmpty(bossName))
            {
                if (!_deadBossList.Contains(bossName))
                {
                    _deadBossList.Add(bossName);
                    TcpConnection.Send($"split {bossName}");
                }
            }
        }


        private static void Handle_Death()
        {
            _deathCounter++;

            TcpConnection.Send($"death {_deathCounter}");

            Handle_Pause();
        }


        private static void Handle_Pause()
        {
            _isPaused = true;

            _firstMove = false;

            TcpConnection.Send($"pause");

            _logoutCounter++;

            TcpConnection.Send($"logout {_logoutCounter}");
        }


        private static void Handle_Resume()
        {
            _isPaused = false;

            TcpConnection.Send($"resume");
        }
    }
}
