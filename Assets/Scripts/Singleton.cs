using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    private void Awake()
    {
        GetInstance();
    }

    public static T GetInstance()
    {
        T[] exists = FindObjectsOfType<T>();


        foreach (var obj in exists)
        {
            if (instance == null)
            {
                instance = obj;
                DontDestroyOnLoad(obj);
            }
            else if (instance != obj)
            {
                Debug.LogWarning("Attempting to add a second instance of " + obj + " Removing...");
                Destroy(obj.gameObject);
            }
        }

        return instance;
    }
}