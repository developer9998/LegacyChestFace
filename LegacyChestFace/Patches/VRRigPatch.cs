using HarmonyLib;
using UnityEngine;

namespace LegacyChestFace.Patches
{
    [HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("Awake", MethodType.Normal)]
    internal class VRRigPatch
    {
        internal static void Postfix(VRRig __instance)
        {
            if (__instance.GetComponent<VRRigFace>() == null) __instance.gameObject.AddComponent<VRRigFace>();
        }
    }

    internal class VRRigFace : MonoBehaviour
    {
        internal Renderer faceRenderer;
        internal Renderer chestRenderer;
        internal VRRig thisRig;

        // Textures
        public Texture2D originalFace;
        public Texture2D legacyFace;
        public Texture2D originalFur;
        public Texture2D legacyFur;

        internal void LateUpdate()
        {
            if (originalFace == null)
            {
                TryGetComponent(out thisRig);

                originalFace = Plugin.Instance.originalFace;
                legacyFace = Plugin.Instance.legacyFace;
                originalFur = Plugin.Instance.originalFur;
                legacyFur = Plugin.Instance.legacyFur;

                thisRig.headMesh.transform.Find("gorillaface").TryGetComponent(out faceRenderer);
                thisRig.headMesh.transform.parent.Find("gorillachest").TryGetComponent(out chestRenderer);
            }

            if (originalFace == null) Destroy(this);

            if (Plugin.Instance.enabled)
            {
                thisRig.materialsToChangeTo[0].mainTexture = legacyFur;
                faceRenderer.material.mainTexture = legacyFace;
                chestRenderer.material.mainTexture = legacyFace;

            }
            else
            {
                thisRig.materialsToChangeTo[0].mainTexture = originalFur;
                faceRenderer.material.mainTexture = originalFace;
                chestRenderer.material.mainTexture = originalFace;
            }
        }
    }
}
