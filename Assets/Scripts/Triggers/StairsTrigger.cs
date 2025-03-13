using UnityEngine;

// Make sure this script runs after FloorManager.cs in Project Settings, in Script Execution Order.
    public class StairsTrigger : MonoBehaviour
    {
        // Declare variables
        private GameObject _cat;
        private SpriteRenderer _catSprite;
        public GameObject[] bottomFloorArray;
        public GameObject[] topFloorArray;

        [Header("Turn this on if this is the trigger on the bottom floor.")]
        [SerializeField] private bool bottomFloorTrigger;
        
        private void Awake()
        {
            // Apply tags to all activated gameObjects from FloorManager.
            
            bottomFloorArray = GameObject.FindGameObjectsWithTag("BottomFloor");
            topFloorArray = GameObject.FindGameObjectsWithTag("TopFloor");
            _cat = GameObject.FindWithTag("Cat");
            _catSprite = _cat.GetComponentInChildren<SpriteRenderer>();
        }

        public void PlayerOnTopFloor()
        {
            Debug.Log("Player on Top Floor");
            foreach(GameObject bottomFloorGameObject in bottomFloorArray)
                if (!bottomFloorGameObject.Equals(gameObject))
                    bottomFloorGameObject.SetActive(false);
            
            foreach(GameObject topFloorGameObject in topFloorArray)
                topFloorGameObject.SetActive(true);
                
            // gameObject.layer uses only integers, but we can turn a layer name into a layer integer using LayerMask.NameToLayer()
            // The code below assigns the gameObject "cat" the layer with the name "Cat".
                
            int layerCat = LayerMask.NameToLayer("Cat");
            _cat.layer = layerCat;
            _catSprite.enabled = false;
            DataTransfer.SwitchFloors();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (bottomFloorTrigger)
            {
                PlayerOnTopFloor();
            }
            else if (!bottomFloorTrigger)
            {
                Debug.Log("Going from Top Floor to Bottom Floor");
                foreach(GameObject topFloorGameObject in topFloorArray)
                    if (!topFloorGameObject.Equals(gameObject))
                        topFloorGameObject.SetActive(false);
            
                foreach(GameObject bottomFloorGameObject in bottomFloorArray)
                    bottomFloorGameObject.SetActive(true);
                
                int layerDefault = LayerMask.NameToLayer("Default");
                _cat.layer = layerDefault;
                _catSprite.enabled = true;
                // DataTransfer.onTopFloor = false;
            }
            DataTransfer.SwitchFloors();
        }
    }

