using BepInEx;
using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Networking;

namespace LegacyChestFace
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    [Description("HauntedModMenu")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }

        // Images
        public Texture2D originalImage;
        public Texture2D legacyImage;

        private void Start() => Instance = this;

        private void OnEnable() => HarmonyPatches.ApplyHarmonyPatches();

        private void OnDisable() => HarmonyPatches.RemoveHarmonyPatches();

        public void OnInitialize()
        {
            if (originalImage == null) originalImage = Resources.Load<Texture2D>("objects/treeroom/materials/gorillachestface");
            if (legacyImage == null) StartCoroutine(GetLegacyImage());
        }

        public Renderer GetFaceImage(VRRig rig, bool face)
        {
            Renderer gorillaface = rig.headMesh.transform.Find("gorillaface").GetComponent<Renderer>();
            Renderer gorillachest = rig.headMesh.transform.parent.Find("gorillachest").GetComponent<Renderer>();

            if (face)
                return gorillaface;
            return gorillachest;
        }

        //https://github.com/fchb1239/UnityImageDownloader
        private IEnumerator GetLegacyImage()
        {
            var imageGet = GetImageRequest();
            yield return imageGet.SendWebRequest();

            Texture2D tex = new Texture2D(128, 128, TextureFormat.RGB24, false);
            tex.filterMode = FilterMode.Point;
            tex.LoadImage(imageGet.downloadHandler.data);

            legacyImage = tex;
        }

        private UnityWebRequest GetImageRequest()
        {
            var request = new UnityWebRequest($"https://raw.githubusercontent.com/developer9998/LegacyChestFace/main/gorillachestface.png", "GET"){downloadHandler = new DownloadHandlerBuffer()};

            return request;
        }
    }
}
