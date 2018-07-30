using System;
using CCore.Assets;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CCore.Scenes
{
    public static class SceneController
    {
        private static Action sceneLoadedCallback;

        /// <summary>
        /// Open a scene found at given path. Set scene active is disabled by default.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="openSceneMode"></param>
        /// <param name="setSceneActive"></param>
        /// <returns>Scene</returns>
        public static Scene OpenSceneInEditor(
            string path,
            OpenSceneMode openSceneMode = OpenSceneMode.Single,
            bool setSceneActive = false
        )
        {
            Scene scene = EditorSceneManager.OpenScene(path, openSceneMode);

            if (setSceneActive)
            {
                EditorSceneManager.SetActiveScene(scene);
            }

            return scene;
        }

        /// <summary>
        /// Close scene based on name. Remove scene is true by default.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="removeScene"></param>
        /// <returns>Scene</returns>
        public static bool CloseSceneInEditor(string name, bool removeScene = true)
        {
            Scene scene = EditorSceneManager.GetSceneByName(name);

            return EditorSceneManager.CloseScene(scene, removeScene);
        }

        public static void OpenGameScenesInEditor()
        {
            SceneConfig sceneConfig = AssetHelper.LoadAsset<SceneConfig>("SceneConfig");

            if (sceneConfig == null)
            {
                UnityEngine.Debug.LogError("Trying to open Game Scenes in Editor, but no SceneConfig was created!");

                return;
            }

            if (sceneConfig.GameScenes.Length == 0)
            {
                UnityEngine.Debug.LogWarning("Trying to open Game Scenes in Editor, but no scenes were assigned in the SceneConfig!");
            }

            for (int i = 0; i < sceneConfig.GameScenes.Length; i++)
            {
                OpenSceneMode openSceneMode = OpenSceneMode.Single;

                bool makeSceneActive = i == 0;

                if (i > 0)
                {
                    openSceneMode = OpenSceneMode.Additive;
                }

                string assetPath = AssetHelper.GetAssetPath(sceneConfig.GameScenes[i]);

                OpenSceneInEditor(assetPath, openSceneMode, makeSceneActive);
            }
        }

        /// <summary>
        /// Save all scenes and close them
        /// </summary>
        public static void CloseAllScenesInEditor()
        {
            SaveAllOpenScenesInEditor();
            
            for (int i = 0; i < EditorSceneManager.sceneCount; i++)
            {
                Scene scene = EditorSceneManager.GetSceneAt(i);

                EditorSceneManager.CloseScene(scene, true);
            }
        }

        /// <summary>
        /// Save all scenes and close them
        /// </summary>
        public static void CloseAllScenesInEditor(params string[] filter)
        {
            SaveAllOpenScenesInEditor();
            
            for (int i = 0; i < EditorSceneManager.sceneCount; i++)
            {
                Scene scene = EditorSceneManager.GetSceneAt(i);

                bool isSceneFiltered = false;

                for (int k = 0; k < filter.Length; k++)
                {
                    if (scene.name == filter[k])
                    {
                        isSceneFiltered = true;

                        break;
                    }
                }

                if (isSceneFiltered)
                {
                    continue;
                }

                EditorSceneManager.CloseScene(scene, true);
            }
        }

        /// <summary>
        /// Save all scenes. This is also done when closing all scenes.
        /// </summary>
        public static void SaveAllOpenScenesInEditor()
        {
            bool success = EditorSceneManager.SaveOpenScenes();

            if (!success)
            {
                EditorUtility.DisplayDialog(
                    "ERROR SAVING SCENES",
                    "There was a mishap while trying to save all open scenes in the editor :(",
                    "OK");
            }
        }

        /// <summary>
        /// Close all open scenes and load scene with given name
        /// </summary>
        /// <param name="sceneName"></param>
        public static void LoadScene(string sceneName, UnityAction<Scene, LoadSceneMode> callback = null)
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

            if (callback != null)
            {
                SceneManager.sceneLoaded += callback;
            }
        }

        /// <summary>
        /// Load scene with given name additively
        /// </summary>
        /// <param name="sceneName"></param>
        public static void LoadSceneAdditive(string sceneName, Action callback = null)
        {
            // First check if the scene was already loaded, handy when working in editor mode
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == sceneName)
                {
                    if (callback != null)
                    {
                        callback();
                    }

                    return;
                }
            }

            sceneLoadedCallback = callback;

            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        public static void UnLoadScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            
            if (sceneLoadedCallback != null)
            {
                sceneLoadedCallback();

                sceneLoadedCallback = null;
            }
        }
    }
}