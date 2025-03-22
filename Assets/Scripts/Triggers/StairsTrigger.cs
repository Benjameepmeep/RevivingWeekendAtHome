using UnityEngine;

// Make sure this script runs after FloorManager.cs in Project Settings, in Script Execution Order.
    public class StairsTrigger : MonoBehaviour
    {
       
        [SerializeField] private bool goingUpwards;
        [SerializeField] private StairsController stairsController;


        void Start()
        {
            if (stairsController == null)
            {
                stairsController = FindFirstObjectByType<StairsController>();
            }
        }

        public void EnterTopFloor()
        {
            stairsController.EnterTopFloor();
        }

        public void EnterBottomFloor()
        {
            stairsController.EnterBottomFloor();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (goingUpwards)
            {
                EnterTopFloor();
            }
            else
            {
                EnterBottomFloor();
            }
            
            FloorManager.Instance.dataTransfer.SwitchFloors();
        }
    }

