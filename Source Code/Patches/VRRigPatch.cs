using HarmonyLib;
using UnityEngine;

namespace LegacyChestFace.Patches
{
    [HarmonyPatch(typeof(VRRig))]
    [HarmonyPatch("Awake", MethodType.Normal)]
    internal class VRRigPatch
    {
        private static void Postfix(VRRig __instance)
        {
            if (__instance.GetComponent<VRRigFace>() == null)
                __instance.gameObject.AddComponent<VRRigFace>();
        }
    }

    internal class VRRigFace : MonoBehaviour
    {
        Renderer faceRenderer = null;
        Renderer chestRenderer = null;

        // Images
        public Texture2D originalImage;
        public Texture2D legacyImage;

        public void Start()
        {
            originalImage = Plugin.Instance.originalImage;
            legacyImage = Plugin.Instance.legacyImage;
            faceRenderer = Plugin.Instance.GetFaceImage(gameObject.GetComponent<VRRig>(), true);
            chestRenderer = Plugin.Instance.GetFaceImage(gameObject.GetComponent<VRRig>(), false);
        }

        public void LateUpdate()
        {
            if (Plugin.Instance.legacyImage != null && legacyImage == null)
                legacyImage = Plugin.Instance.legacyImage;
            if (Plugin.Instance.originalImage != null && originalImage == null)
                originalImage = Plugin.Instance.originalImage;

            if (faceRenderer != null && chestRenderer != null && originalImage != null && legacyImage != null)
            {
                if (Plugin.Instance.enabled)
                {
                    faceRenderer.material.mainTexture = legacyImage;
                    chestRenderer.material.mainTexture = legacyImage;
                }
                else
                {
                    faceRenderer.material.mainTexture = originalImage;
                    chestRenderer.material.mainTexture = originalImage;
                }
            }
        }
    }
}
