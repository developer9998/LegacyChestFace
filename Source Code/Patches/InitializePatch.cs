using HarmonyLib;
using System.Collections;

namespace LegacyChestFace.Patches
{
    [HarmonyPatch(typeof(GorillaLocomotion.Player))]
    [HarmonyPatch("Awake", MethodType.Normal)]
    internal class InitializePatch
    {
        private static void Postfix(GorillaLocomotion.Player __instance) => __instance.StartCoroutine(Delay());

        //https://github.com/legoandmars/Utilla/blob/master/Utilla/HarmonyPatches/Patches/PostInitializedPatch.cs
        static IEnumerator Delay()
        {
            yield return 0;
            Plugin.Instance.OnInitialize();
        }
    }
}
