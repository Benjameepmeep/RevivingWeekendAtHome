using UnityEngine;

public class ChangeBools : MonoBehaviour
{
    private OutdoorTrigger _outdoorTrigger;
    public bool playerOutside; // Change this in Editor if player starts scene outside.
    
    // Update is called once per frame
    void Start()
    {
        _outdoorTrigger = GameObject.Find("OutdoorTrigger").GetComponent<OutdoorTrigger>();
        
        if (playerOutside)
        {
            _outdoorTrigger.PlayerStartsOutside(true);
        }
    }
}
