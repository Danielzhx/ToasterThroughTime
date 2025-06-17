using UnityEngine;

namespace TTT.Utillities 
{
    public class SingletonMonobehaviour<T> : MonoBehaviour where T : SingletonMonobehaviour<T>
    { 
        static T _instance;

        public static T Instance => _instance ? _instance : _instance = FindAnyObjectByType<T>();
        public void OnDestroy() => _instance = null;
    }
}
