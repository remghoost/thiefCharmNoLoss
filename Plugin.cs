using GlobalSettings;
using HarmonyLib;
using System.Reflection;
using BepInEx;
using TeamCherry.SharedUtils;
using UnityEngine;
using BepInEx.Logging;

namespace thiefCharmNoLoss;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class thiefCharmNoLoss : BaseUnityPlugin
{
    internal static ManualLogSource Log;

    void Awake()
    {
        Log = Logger;
        var harmony = new Harmony("com.remghoost.thiefcharmnoloss");
        harmony.PatchAll();
    }
}

[HarmonyPatch(typeof(Gameplay))]
[HarmonyPatch("Awake")]
class Gameplay_Awake_Patch
{
    static void Postfix(Gameplay __instance)
    {
        var type = typeof(Gameplay);

        void SetAndLog<T>(string fieldName, T newValue)
        {
            var field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field == null)
            {
                thiefCharmNoLoss.Log.LogWarning($"[Gameplay_Awake_Patch] Could not find field: {fieldName}");
                return;
            }

            var oldValue = field.GetValue(__instance);
            thiefCharmNoLoss.Log.LogInfo($"[Gameplay Awake] {fieldName} before: {oldValue}");

            field.SetValue(__instance, newValue);

            var updatedValue = field.GetValue(__instance);
            thiefCharmNoLoss.Log.LogInfo($"[Gameplay Awake] {fieldName} after: {updatedValue}");
        }

        // Zero out float + MinMaxInt
        SetAndLog("thiefCharmGeoLossLooseChance", 0f);
        var minMaxZero = new MinMaxInt(0, 0);
        SetAndLog("thiefCharmGeoLossCap", minMaxZero);
        SetAndLog("thiefCharmGeoLossLooseAmount", minMaxZero);

        // Nuke prefabs
        SetAndLog<GameObject>("thiefCharmHeroHitPrefab", null);
        SetAndLog<GameObject>("thiefSnatchEffectPrefab", null);
    }
}


