using UnityEngine;

public abstract class Singletone<T> : MonoBehaviour where T : Singletone<T>
{
    public static T instance;

    public static T Instance { get { return instance; } }

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = (T)this;
        DontDestroyOnLoad(instance);
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}
