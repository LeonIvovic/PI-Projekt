using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T GetInstance()
    {
        T exists = FindObjectOfType<T>();
        if (instance == null)
        {
            instance = exists;
            DontDestroyOnLoad(instance);
        }
        else if (instance != exists)
        {
            Debug.LogWarning("Attempting to add a second instance of " + instance + " Removing...");
            Destroy(exists);
        }

        return instance;
    }
}