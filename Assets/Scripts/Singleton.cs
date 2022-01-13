using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));
                if (_instance == null)
                {
                    GameObject singletonObject = new GameObject($"{typeof(T)} (Singleton)");
                    _instance = singletonObject.AddComponent<T>();
                    singletonObject.transform.parent = null;
                    DontDestroyOnLoad(singletonObject);
                }
            }

            return _instance;
        }
    }
}