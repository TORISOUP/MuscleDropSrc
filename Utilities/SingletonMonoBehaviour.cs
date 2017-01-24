using UnityEngine;

namespace Assets.MuscleDrop.Scripts.Utilities
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        protected static T instance;

        public static T Instance
        {
            get
            {
                if (instance != null) return instance;
                instance = (T)FindObjectOfType(typeof(T));

                return instance;
            }
        }

        protected void Awake()
        {
            CheckInstance();
        }

        protected bool CheckInstance()
        {
            if (instance == null)
            {
                instance = (T)this;
                return true;
            }
            if (Instance == this)
            {
                return true;
            }

            Destroy(this);
            return false;
        }
    }
}
