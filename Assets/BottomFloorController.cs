using UnityEditor.EditorTools;
using UnityEngine;

public class BottomFloorController : MonoBehaviour
{
    [SerializeField] private GameObject bottomFloor;
    [Tooltip("It's okay for this to be not assigned")]
    [SerializeField] private GameObject stairsBottomFloor;

    private void Start()
    {
        FloorManager.Instance.bottomFloor = bottomFloor;
        FloorManager.Instance.stairsBottomFloor = stairsBottomFloor != null ? stairsBottomFloor : null; // sets it to null so that a previous scene's stairs doesnt affect it
    }

}
