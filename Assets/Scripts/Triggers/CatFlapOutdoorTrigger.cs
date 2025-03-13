using UnityEngine;
using UnityEngine.Rendering;


    public class CatFlapOutdoorTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject cat;
        [SerializeField] private SortingGroup catSortingGroup;
    
        private void Awake()
        {
            // Fetch all of the objects with the Outdoor and OutdoorToDespawn tags and store them in a list
            // and hide all of the outdoor objects
            cat = GameObject.FindWithTag("Cat");
            if (cat != null)
            {
                catSortingGroup = cat.GetComponent<SortingGroup>();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision == null) return;
            if (!collision.CompareTag("Cat")) return;
            
            DataTransfer.catOutside = true;
            
            Debug.Log("Cat Is Outside");
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision == null) return;
            if (!collision.CompareTag("Cat")) return;
            
            DataTransfer.catOutside = false;
            Debug.Log("Cat Is Inside");
            // The line is to prevent errors during Unity Load/Unload in Editor with missing SortingGroup.
            if (catSortingGroup == null)
                return;
        }
    }

