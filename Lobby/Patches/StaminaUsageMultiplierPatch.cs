﻿using HarmonyLib;
using InventorySystem;

namespace Lobby.Patches
{
    [HarmonyPatch(typeof(Inventory), nameof(Inventory.StaminaUsageMultiplier), MethodType.Getter)]
    public class StaminaUsageMultiplierPatch
    {
        private static void Postfix(Inventory __instance, ref float __result)
        {
            if (Lobby.Config.InfinityStamina && EventHandlers.IsLobby)
                __result = 0;
        }
    }
}