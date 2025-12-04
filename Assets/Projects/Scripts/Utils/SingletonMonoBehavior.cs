using UnityEngine;

namespace Projects.Scripts.Utils
{
    public abstract class SingletonMonoBehavior<T> : MonoBehaviour where T : SingletonMonoBehavior<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance) return _instance;
                _instance = FindAnyObjectByType<T>();
                Debug.Assert(_instance, $"{typeof(T).Name}のインスタンスがシーン上に存在しません。");
                return _instance;
            }
        }
    }
}