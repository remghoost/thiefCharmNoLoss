using GlobalSettings;
using HarmonyLib;
using System.Reflection;
using BepInEx;

namespace thiefCharmNoLoss;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class thiefCharmNoLoss : BaseUnityPlugin
{
    void Awake()
    {
        var harmony = new Harmony("com.remghoost.thiefcharmnoloss");
        harmony.PatchAll();
    }
}

[HarmonyPatch(typeof(Gameplay))]
[HarmonyPatch("Get")]
class Gameplay_Get_Patch
{
    static void Postfix(Gameplay __result)
    {
        if (__result == null) return;

        // Zero out floats
        typeof(Gameplay).GetField("thiefCharmGeoLossLooseChance", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.SetValue(__result, 0f);

        // Zero out MinMaxInt
        var minMaxZero = new TeamCherry.SharedUtils.MinMaxInt(0, 0);

        typeof(Gameplay).GetField("thiefCharmGeoLossCap", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.SetValue(__result, minMaxZero);

        typeof(Gameplay).GetField("thiefCharmGeoLossLooseAmount", BindingFlags.NonPublic | BindingFlags.Instance)
            ?.SetValue(__result, minMaxZero);
    }
}
