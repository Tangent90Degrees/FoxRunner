using UnityEngine;

/// <summary>
/// The base class of all singleton mano-behaviour classes.
/// </summary>
/// <typeparam name="T">The type of singleton instance.</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    /// <summary>
    /// The unique instance of this singleton class.
    /// </summary>
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this as T;
        }
    }

    protected virtual void OnDestroy()
    {
        Instance = null;
    }
}
