using UnityEngine;

namespace CCore.Scenes
{
    [CreateAssetMenu(fileName = "SceneConfig", menuName = "CCore/SceneConfig")]
    public class SceneConfig : ScriptableObject
    {
        // TODO: Create a scene reference
        [SerializeField] private Object[] gameScenes;

        public Object[] GameScenes { get { return gameScenes; } }

        // TODO: Add other scene lists here
    }
}