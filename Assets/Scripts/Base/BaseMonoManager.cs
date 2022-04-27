using UnityEngine;

public class BaseMonoManager<T> : MonoBehaviour where T:MonoBehaviour
{
    private static T _instance;

    private void Awake()
    {
        _instance = this as T;
    }
    public static T GetInstance()
    {
        if (_instance == null)
        {
            Debug.LogError("manager为空 ");
        }
        return _instance;
    }
}
