using UnityEngine;

public class ChangeBools : MonoBehaviour
{
    private StairsTrigger _stairsTrigger;
    private OutdoorTrigger _outdoorTrigger;
    public bool onTopFloor; // Change this in Editor if you're changing starting floors.
    public bool playerOutside; // Change this in Editor if player starts scene outside.
    
    // Update is called once per frame
    void Start()
    {
        _stairsTrigger = GetComponentInParent<StairsTrigger>();
        _outdoorTrigger = GameObject.Find("OutdoorTrigger").GetComponent<OutdoorTrigger>();
        if (onTopFloor)
        {
            _stairsTrigger.PlayerOnTopFloor();
        }
        if (playerOutside)
        {
            _outdoorTrigger.PlayerStartsOutside(true);
        }
    }
}
