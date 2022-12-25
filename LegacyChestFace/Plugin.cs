using BepInEx;
using System.IO;
using System.Reflection;
using System.ComponentModel;
using UnityEngine;

namespace LegacyChestFace
{
    [Description("HauntedModMenu")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance { get; private set; }

        // Textures
        public Texture2D originalFace;
        public Texture2D legacyFace;
        public Texture2D originalFur;
        public Texture2D legacyFur;

        internal void Awake() => Instance = this;

        internal void OnEnable() => HarmonyPatches.ApplyHarmonyPatches();

        internal void OnDisable() => HarmonyPatches.RemoveHarmonyPatches();

        internal void OnInitialize()
        {
            originalFace = GorillaTagger.Instance.offlineVRRig.headMesh.transform.Find("gorillaface").GetComponent<Renderer>().material.mainTexture as Texture2D;
            originalFur = GorillaTagger.Instance.offlineVRRig.materialsToChangeTo[0].mainTexture as Texture2D;
            legacyFace = GetImage("LegacyChestFace.Resources.gorillachestface.png", 128, 128);
            legacyFur = GetImage("LegacyChestFace.Resources.lightfur.png", 64, 64);
        }

        // Custom image loading via. the stream
        // https://github.com/fchb1239/UnityImageDownloader
        // https://stackoverflow.com/questions/1080442/

        Texture2D GetImage(string path, int w, int h)
        {
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
