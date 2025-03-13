using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemMain : MonoBehaviour
{
    private static EventSystemMain _instance;

    public static EventSystemMain Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void DisableEventSystem()
    {
        GetComponent<EventSystem>().enabled = false;
    }

    public void EnableEventSystem()
    {
        GetComponent<EventSystem>().enabled = true;
    }
}
