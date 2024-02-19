using UnityEngine;

namespace Core
{
    public class Singleton : MonoBehaviour
    {
        private static Singleton _instance = null;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(this);
        }
    }
}