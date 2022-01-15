using UnityEngine;


namespace MjKkaya.ShortestPath.Utilities
{
    public abstract class Singleton<T> : MonoBehaviour
    {
        public static T mInstance;
        public static T Instance
        {
            get
            {
                return mInstance;
            }
        }


        protected void Initialize(T instanceType)
        {
            if (mInstance != null)
            {
                Destroy(gameObject);
                return;
            }

            mInstance = instanceType;
            DontDestroyOnLoad(gameObject);
        }
    }
}