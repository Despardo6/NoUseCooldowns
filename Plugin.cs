using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;

namespace NoUseCooldowns.NoCooldowns
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private const string PLUGIN_GUID = "Despardo6.NoUseCooldowns";
        private const string PLUGIN_NAME = "No use Cooldowns";
        private const string PLUGIN_VERSION = "1.0.1";

        internal static ManualLogSource Log;

        private void Awake()
        {
            Log = base.Logger;

            Harmony.CreateAndPatchAll(typeof(CooldownPatches));

            Log.LogInfo($"{PLUGIN_NAME} v{PLUGIN_VERSION} loaded!");
        }
    }

    internal static class CooldownPatches
    {
        // List of NoisemakerProp item names to remove cooldowns from.  Easily add/remove items here.
        private static readonly List<string> NoisemakerItems = new List<string>()
        {
            "Hairdryer",
            "Clown horn",
            "Airhorn",
            "Cash register",
            "Remote",
            "Pro-flashlight",
            "Flashlight",
            "Laser pointer",
            "Boombox",
            "Weed killer",
            "Unnamed",
            "Gnarpy",
            "Arson Plush",
            "Arson Plush (Dirty)",
            "Cookie Fumo",
            "Maxwell",
            "Cat",
        };


        // Patch for Noisemaker Props
        [HarmonyPatch(typeof(NoisemakerProp))]
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void NoisemakerPropStartPatch(NoisemakerProp __instance)
        {
            // Check if the item's name is in our list.  .Contains() is efficient for List lookups.
            if (NoisemakerItems.Contains(__instance.itemProperties.itemName))
            {
                __instance.useCooldown = 0f;
                Plugin.Log.LogDebug($"Removed cooldown from: {__instance.itemProperties.itemName}");
            }
        }

        // Patch for InteractTrigger
        [HarmonyPatch(typeof(InteractTrigger))]
        [HarmonyPatch("Start")]
        [HarmonyPostfix]
        private static void InteractTriggerStartPatch(InteractTrigger __instance)
        {
            __instance.cooldownTime = 0f;
            Plugin.Log.LogDebug($"Removed cooldown from: {__instance.name}");
        }
    }
}