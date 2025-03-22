using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemMain : MonoBehaviour
{
    public void DisableEventSystem()
    {
        GetComponent<EventSystem>().enabled = false;
    }

    public void EnableEventSystem()
    {
        GetComponent<EventSystem>().enabled = true;
    }
}
