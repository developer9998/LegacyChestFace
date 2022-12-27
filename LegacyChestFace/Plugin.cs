using BepInEx;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using UnityEngine;

namespace LegacyChestFace
{
    [Description("HauntedModMenu")]
    [BepInIncompatibility("org.legoandmars.gorillatag.modmenupatch")] // I don't want cheaters using my mods
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }

        // Textures
        public Texture2D originalFace;
        public Texture2D legacyFace;
        public Texture2D originalFur;
        public Texture2D legacyFur;
        public Texture2D originalAltFur;
        public Texture2D legacyAltFur;

        internal void Awake() => Instance = this;

        internal void OnEnable() => HarmonyPatches.ApplyHarmonyPatches();

        internal void OnDisable() => HarmonyPatches.RemoveHarmonyPatches();

        internal void OnInitialize()
        {
            originalFace = (Texture2D)GorillaTagger.Instance.offlineVRRig.headMesh.transform.Find("gorillaface").GetComponent<Renderer>().material.mainTexture;
            originalFur = (Texture2D)GorillaTagger.Instance.offlineVRRig.materialsToChangeTo[0].mainTexture;
            originalAltFur = (Texture2D)GorillaTagger.Instance.offlineVRRig.materialsToChangeTo[4].mainTexture;
            legacyFace = GetImage("LegacyChestFace.Resources.gorillachestface.png", 128, 128);
            legacyFur = GetImage("LegacyChestFace.Resources.lightfur.png", 64, 64);
            legacyAltFur = GetImage("LegacyChestFace.Resources.lightfurPaintBall.png", 64, 64);
        }

        /// <summary>
        /// Custom image loading via. the stream
        /// </summary>
        /// <param name="path">The path of the file including the format</param>
        /// <param name="w">The width of the image</param>
        /// <param name="h">The height of the image</param>
        /// <returns>The generated image with the path</returns>
        Texture2D GetImage(string path, int w, int h)
        {
            // https://github.com/fchb1239/UnityImageDownloader
            // https://stackoverflow.com/questions/1080442/

            Texture2D tex = new Texture2D(w, h, TextureFormat.RGBA32, false){filterMode = FilterMode.Point};

            Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            byte[] bytes = new byte[manifestResourceStream.Length];
            manifestResourceStream.Read(bytes, 0, bytes.Length);

            tex.LoadImage(bytes);
            tex.Apply();

            return tex;
        }
    }
}
