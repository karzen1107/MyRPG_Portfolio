using UnityEngine;

public abstract class Singletone_Destroy<T> : MonoBehaviour where T : Singletone_Destroy<T>
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
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }
}
