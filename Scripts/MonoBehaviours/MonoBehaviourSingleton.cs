namespace CCore
{
    // TODO: This is just a temp workaround, find a nice way to do DI
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T cachedInstance;

        public static T Instance
        {
            get
            {
                if (cachedInstance == null)
                {
                    T[] instances = FindObjectsOfType<T>();

                    // No instance found
                    if (instances == null || instances.Length == 0)
                    {
                        return null;
                    }

                    cachedInstance = instances[0];
                }

                return cachedInstance;
            }
        }
    }
}