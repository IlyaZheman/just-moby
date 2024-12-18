using System.Threading.Tasks;
using UnityEngine;

namespace Game.Managers
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
        }

        private async void Start()
        {
            Initialize();
            await Task.Yield();
            StartManager();
        }

        protected virtual void Initialize()
        {
        }

        protected virtual void StartManager()
        {
        }
    }
}