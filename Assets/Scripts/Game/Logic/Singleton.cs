using UnityEngine;

namespace Game.Logic
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance = null;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else if (Instance == this)
            {
                Destroy(gameObject);
            }

            // DontDestroyOnLoad(gameObject);

            InitializeManager();
        }

        protected virtual void InitializeManager()
        {
        }
    }
}