using UnityEngine;


    public class BathroomTrigger : MonoBehaviour
    {
        // Declare Variables
        [SerializeField] private GameObject insideBathroomToDespawn;
    
        void Start()
        {
            if (insideBathroomToDespawn == null) insideBathroomToDespawn = GameObject.FindWithTag("Inside Bathroom to Despawn");
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.LogWarning("Player entered the bathroom trigger");
                insideBathroomToDespawn.SetActive(false);
            }
            
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.LogWarning("Player exited the bathroom trigger");
                insideBathroomToDespawn.SetActive(true);
            }
        }
    }

