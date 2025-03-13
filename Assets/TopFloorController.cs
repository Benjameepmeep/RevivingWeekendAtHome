using UnityEngine;

public class TopFloorController : MonoBehaviour
{
    [SerializeField] private GameObject topFloor;
    [SerializeField] private GameObject stairsTopFloor;
    
    private void Start()
    {
        FloorManager.Instance.topFloor = topFloor;
        FloorManager.Instance.stairsTopFloor = stairsTopFloor;
    }

}
