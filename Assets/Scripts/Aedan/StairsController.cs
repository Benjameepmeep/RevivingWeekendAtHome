using UnityEngine;

public class StairsController : MonoBehaviour
{
    [SerializeField] private GameObject bottomFloor;
    [SerializeField] private GameObject stairsLeadingUp;

    [SerializeField] private GameObject topFloor;
    [SerializeField] private GameObject stairsLeadingDown;

    

    private void Start()
    {
        if (stairsLeadingUp == null)
        {
            stairsLeadingUp = GameObject.Find("StairsGoingUpwards");
        }
        if (stairsLeadingDown == null)
        {
            stairsLeadingDown = GameObject.Find("StairsGoingDownwards");
        }
        
        Invoke("SetFloorManagerFloors", 0.1f);

        
    }


    private void SetFloorManagerFloors()
    {
        if (FloorManager.Instance == null)
        {
            Invoke("SetFloorManagerFloors", 0.1f);
            return;
        }
        FloorManager.Instance.bottomFloor = bottomFloor;
        FloorManager.Instance.stairsLeadingUp = stairsLeadingUp;
        FloorManager.Instance.topFloor = topFloor;
        FloorManager.Instance.stairsLeadingDown = stairsLeadingDown;
    }

    public void EnterBottomFloor()
    {
        if (!FloorManager.Instance.dataTransfer.onTopFloor) return;
        Debug.LogWarning("Entering bottom floor");
        bottomFloor.SetActive(true);

        topFloor.SetActive(false);

        FloorManager.Instance.dataTransfer.onTopFloor = false;
        FloorManager.Instance.EnableCatVisuals();
    }
    public void EnterTopFloor()
    {
        if (FloorManager.Instance.dataTransfer.onTopFloor) return;
        Debug.LogWarning("Entering top floor");
        topFloor.SetActive(true);
 
        bottomFloor.SetActive(false);

        FloorManager.Instance.dataTransfer.onTopFloor = true;
        FloorManager.Instance.DisableCatVisuals();

    }


     // gameObject.layer uses only integers, but we can turn a layer name into a layer integer using LayerMask.NameToLayer()
    // The code below assigns the gameObject "cat" the layer with the name "Cat".
}
