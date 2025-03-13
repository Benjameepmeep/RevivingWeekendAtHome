using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


    // TODO: Make sure the VFX Weather shows better onto the snow
    public class OutdoorTrigger : MonoBehaviour
    {
        // Declare Variables
        [SerializeField] private GameObject[] bottomFloor;
        [SerializeField] private GameObject[] kitchenWithDoorAndLamp;
        
        [SerializeField] private GameObject player;
        [SerializeField] private SortingGroup playerSortingGroup;
        [SerializeField] private SortingGroup vfxSortingGroup;
        
        [SerializeField] private float transparencyValue, moveAmount, playerOutdoorLightValue;
        private float _playerLightIntensity;
        
        void Start()
        {
            // Fetch all of the objects with the Outdoor and OutdoorToDespawn tags and store them in a list
            // and hide all of the outdoor objects.
            
            bottomFloor = GameObject.FindGameObjectsWithTag("BottomFloor");
            
            player = GameObject.FindWithTag("Player");

            _playerLightIntensity = player.GetComponentInChildren<Light2D>().intensity;
            
            playerSortingGroup = player.GetComponent<SortingGroup>();
            playerSortingGroup.sortingOrder = DataTransfer.playerSortingOrder;
            
            vfxSortingGroup = GameObject.FindWithTag("VFX").GetComponent<SortingGroup>();
            vfxSortingGroup.sortingOrder = DataTransfer.vfxSortingOrder;
        }
        
        public void PlayerStartsOutside(bool startsOutside)
        {
            if (startsOutside)
            {
                DataTransfer.playerInside = true;
                PlayerOutdoors();
            }
            else
            {
                DataTransfer.playerInside = false;
                PlayerIndoors();
            }
        }

        private void PlayerIndoors()
        {
            foreach (GameObject bottomFloorGameObject in bottomFloor)
            {
                bottomFloorGameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }

            foreach (GameObject kitchenAndMoreGameObject in kitchenWithDoorAndLamp)
            {
                kitchenAndMoreGameObject.GetComponent<SpriteRenderer>().sortingOrder = 45;
            }
            var transform1 = transform;
            var position = transform1.position;
            position = new Vector3(position.x, position.y + moveAmount);
            transform1.position = position;

            player.GetComponentInChildren<Light2D>().intensity = _playerLightIntensity;
            
            DataTransfer.PlayerInsideOrOutside();
            playerSortingGroup.sortingOrder = DataTransfer.playerSortingOrder;
        }

        private void PlayerOutdoors()
        {
            foreach (GameObject bottomFloorGameObject in bottomFloor)
            {
                bottomFloorGameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, transparencyValue);
            }
                
            foreach (GameObject kitchenAndMoreGameObject in kitchenWithDoorAndLamp)
            {
                kitchenAndMoreGameObject.GetComponent<SpriteRenderer>().sortingOrder = 55;
            }

            var transform1 = transform;
            var position = transform1.position;
            position = new Vector3(position.x, position.y - moveAmount);
            transform1.position = position;

            player.GetComponentInChildren<Light2D>().intensity = playerOutdoorLightValue;
            
            DataTransfer.PlayerInsideOrOutside();
            playerSortingGroup.sortingOrder = DataTransfer.playerSortingOrder;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other == null) return;
            if (!other.CompareTag("Player")) return;
            
            // Ran when the player enters the outdoor trigger
            // Checks if the player is currently inside or outside
            // Then it activates all of the objects corresponding to where the player is + hiding objects that need to be hidden
            // Sets the light2d child of player to an intensity of 0 and then moves down
            // Does opposite when player reenter.
            
            if (DataTransfer.playerInside)
            {
                Debug.Log("Player Goes Outdoors");

                PlayerOutdoors();
            }
            else if (!DataTransfer.playerInside)
            {
                Debug.Log("Player Goes Inside");
            
                PlayerIndoors();
            }
        }
    }
