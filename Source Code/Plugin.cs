using BepInEx;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using System;

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
            if (originalImage == null) originalImage = Resources.Load<Texture2D>(string.Format("objects/treeroom/materials/{0}", PluginInfo.imageName));
            if (legacyImage == null) legacyImage = GetLegacyImageFunction();
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
        //https://stackoverflow.com/questions/1080442/
        Texture2D GetLegacyImageFunction()
        {
            Texture2D tex = new Texture2D(128, 128, (TextureFormat)3, false){filterMode = FilterMode.Point};

            Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format("{0}.Resources.{1}.png", PluginInfo.Name, PluginInfo.imageName));
            byte[] bytes = new byte[manifestResourceStream.Length];
            manifestResourceStream.Read(bytes, 0, bytes.Length);

            tex.LoadImage(bytes);
            tex.Apply();

            return tex;
        }
    }
}
