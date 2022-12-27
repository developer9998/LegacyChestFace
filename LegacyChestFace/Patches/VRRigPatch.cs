using System;
using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace LegacyChestFace.Patches
{
    [HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("Awake", MethodType.Normal)]
    internal class VRRigPatch
    {
        public static void Postfix(VRRig __instance)
        {
            if (__instance.GetComponent<VRRigFace>() == null) __instance.gameObject.AddComponent<VRRigFace>();
        }
    }

    internal class VRRigFace : MonoBehaviour
    {
        internal Renderer faceRenderer;
        internal Renderer chestRenderer;
        internal VRRig thisRig;
        internal bool lastEnable;

        // Textures
        public Texture2D originalFace;
        public Texture2D legacyFace;
        public Texture2D originalFur;
        public Texture2D legacyFur;
        public Texture2D originalAltFur;
        public Texture2D legacyAltFur;

        internal bool NullCheck()
        {
            return originalFace is null || legacyFace is null || originalFur is null || legacyFur is null || faceRenderer is null || chestRenderer is null || thisRig is null;
        }

        internal void GetNullResources()
        {
            try
            {
                // Components
                TryGetComponent(out thisRig);
                thisRig.headMesh.transform.Find("gorillaface").TryGetComponent(out faceRenderer);
                thisRig.headMesh.transform.parent.Find("gorillachest").TryGetComponent(out chestRenderer);

                // Textures
                originalFace = Plugin.Instance.originalFace;
                legacyFace = Plugin.Instance.legacyFace;
                originalFur = Plugin.Instance.originalFur;
                legacyFur = Plugin.Instance.legacyFur;
                originalAltFur = Plugin.Instance.originalAltFur;
                legacyAltFur = Plugin.Instance.legacyAltFur;
                Debug.Log("Successfully loaded resources");
            }
            catch(Exception e)
            {
                Debug.LogError("Failed to load resources: " + e);
            }
        }

        internal void LateUpdate()
        {
            if (NullCheck()) GetNullResources();
            else
            {
                if (lastEnable != Plugin.Instance.enabled)
                {
                    lastEnable = Plugin.Instance.enabled;
                    if (lastEnable)
                    {
                        faceRenderer.material.mainTexture = legacyFace;
                        chestRenderer.material.mainTexture = legacyFace;

                        int[] paintIndex = new int[6] { 4, 5, 6, 8, 9, 10 };
                        for(int i = 0; i < paintIndex.Length; i++)
                        {
                            if (i == 0) thisRig.materialsToChangeTo[i].mainTexture = legacyFur;
                            else if (paintIndex.Contains(i)) thisRig.materialsToChangeTo[i].mainTexture = legacyAltFur;
                        }
                    }
                    else
                    {
                        faceRenderer.material.mainTexture = originalFace;
                        chestRenderer.material.mainTexture = originalFace;

                        int[] paintIndex = new int[6] { 4, 5, 6, 8, 9, 10 };
                        for (int i = 0; i < paintIndex.Length; i++)
                        {
                            if (i == 0) thisRig.materialsToChangeTo[i].mainTexture = originalFur;
                            else if (paintIndex.Contains(i)) thisRig.materialsToChangeTo[i].mainTexture = originalAltFur;
                        }
                    }
                }
            }
        }
    }
}
