using UnityEditor;

namespace CCore.Scenes
{
    public class ScenesMenu
    {
        [MenuItem("CCore/Scenes/Open Game Scenes")]
        private static void OpenGameScenesInEditor()
        {
            SceneController.OpenGameScenesInEditor();
        }
    }
}