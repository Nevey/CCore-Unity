using System;
using UnityEditor;
using UnityEngine;

namespace CCore.Assets
{
    public static class AssetHelper
    {
        /// <summary>
        /// Creates an asset at given path.
        /// </summary>
        /// <param name="assetPath"></param>
        public static void CreateAsset<T>(UnityEngine.Object asset, string assetPath)
            where T : UnityEngine.Object
        {
            asset = Convert.ChangeType(asset, typeof(T)) as T;

            AssetDatabase.CreateAsset(asset, assetPath);

            EditorUtility.SetDirty(asset);

            AssetDatabase.SaveAssets();

            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Loads an asset. If it finds multiple assets with given name, returns the first one.
        /// </summary>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public static T LoadAsset<T>(string assetName)
        {
            Type type = typeof(T);

            string filter = String.Format("{0} t:{1}", assetName, type.Name);
            
            var guids = AssetDatabase.FindAssets(filter, null);

            if (guids.Length > 1)
            {
                Debug.LogWarningFormat("Found more than one <b>{0}</b> with the name <b>{1}</b>. "
                    + "Try searching for this asset with a specific path...", type.Name, assetName);
            }

            if (guids.Length == 0)
            {
                return default(T);
            }

            string assetPath = AssetDatabase.GUIDToAssetPath(guids[0]);

            return LoadAssetAtPath<T>(assetPath);
        }

        /// <summary>
        /// Loads an asset from a given path.
        /// Example of asset path: "Assets/Game/Images/image.png"
        /// </summary>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public static T LoadAssetAtPath<T>(string assetPath)
        {
            UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityEngine.Object));

            return (T)Convert.ChangeType(asset, typeof(T));
        }

        public static string GetAssetPath(UnityEngine.Object asset)
        {
            return AssetDatabase.GetAssetPath(asset);
        }
    }
}
