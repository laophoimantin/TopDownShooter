using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static object _lock = new object();
    private static bool _isQuitting = false;
    [SerializeField] public bool _dontDestroyOnLoad;

    public static T Instance
    {
        get
        {
            if (_isQuitting)
            {
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    var instanceList = FindObjectsOfType<T>();

                    if (instanceList.Length > 1)
                    {
                        Debug.Log("There is more than one singleton object of type " + typeof(T).Name);
                        for (int i = 1; i < instanceList.Length; i++)
                        {
                            Destroy(instanceList[i]);
                        }
                    }

                    if (instanceList.Length > 0)
                    {
                        _instance = instanceList[0];
                    }
                    else
                    {
                        Debug.Log($"No instances found: {typeof(T)} instance");
                        //_instance = new GameObject($"{typeof(T).Name} (singleton").AddComponent<T>();
                        return null;
                    }
                }
            }

            return _instance;
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    protected virtual void OnApplicationQuit()
    {
        _isQuitting = true;
    }

    protected virtual void Awake()
    {
        _isQuitting = false;
        if (_dontDestroyOnLoad)
        {
            DontDestroyOnLoad(Instance);
        }

        if (_instance == null)
        {
            _instance = this as T;
        }
        else if (_instance != this)
        {
            Debug.LogWarning($"Duplicate singleton instance of {typeof(T).Name} detected, destroying duplicate: {gameObject.name}");
            Destroy(gameObject);
        }
    }
}