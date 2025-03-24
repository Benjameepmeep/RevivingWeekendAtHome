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
        [SerializeField] private bool startOutdoors;

        private bool addedTheFurniture;
        
        void Start()
        {
            // Fetch all of the objects with the Outdoor and OutdoorToDespawn tags and store them in a list
            // and hide all of the outdoor objects.
            
            AddTheFurniture();

            
            player = GameObject.FindWithTag("Player");

            _playerLightIntensity = player.GetComponentInChildren<Light2D>().intensity;
            
            playerSortingGroup = player.GetComponent<SortingGroup>();
            
            Check();
        }


        void Update()
        {
            if (!addedTheFurniture)
            {
                if (FloorManager.Instance.dataTransfer.onTopFloor){
                    addedTheFurniture = true;
                    
                    Invoke(nameof(AddTheFurniture), 0.5f);
                }
            }
        }

        private void AddTheFurniture(){

            bottomFloor = GameObject.FindGameObjectsWithTag("BottomFloor");

                        kitchenWithDoorAndLamp = new GameObject[]
                        {
                            GameObject.Find("WallShelfKitchen"),
                            GameObject.Find("Oven"),
                            GameObject.Find("KitchenCounterTop"),
                            GameObject.Find("RightSideKitchenCounter"),
                            GameObject.Find("Fridge"),
                            GameObject.Find("CatFlap"),
                            GameObject.Find("Lamp"),
                            GameObject.Find("Glass Door + Trigger")
            };

            
            
        }

        private void Check(){

            if (FloorManager.Instance == null)
            {
                Invoke(nameof(Check), 0.2f);
                return;
            }
            playerSortingGroup.sortingOrder = FloorManager.Instance.dataTransfer.playerSortingOrder;
            
            vfxSortingGroup = GameObject.FindWithTag("VFX").GetComponent<SortingGroup>();
            vfxSortingGroup.sortingOrder = FloorManager.Instance.dataTransfer.vfxSortingOrder;

            if (startOutdoors)
            {
                PlayerStartsOutside(true);
            }
            else
            {
                PlayerStartsOutside(false);
            }

            if (FloorManager.Instance.dataTransfer.onTopFloor)
            {
                addedTheFurniture = false;
            }
            else
            {
                addedTheFurniture = true;
            }
            
        }
        
        public void PlayerStartsOutside(bool startsOutside)
        {
            if (startsOutside)
            {
                FloorManager.Instance.dataTransfer.playerInside = true;
                PlayerOutdoors();
            }
            else
            {
                FloorManager.Instance.dataTransfer.playerInside = false;
                PlayerIndoors();
            }
        }

        private void PlayerIndoors()
        {
            //if (FloorManager.Instance.dataTransfer.onTopFloor) return;

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
            
            FloorManager.Instance.dataTransfer.PlayerInsideOrOutside();
            playerSortingGroup.sortingOrder = FloorManager.Instance.dataTransfer.playerSortingOrder;
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
            
            FloorManager.Instance.dataTransfer.PlayerInsideOrOutside();
            playerSortingGroup.sortingOrder = FloorManager.Instance.dataTransfer.playerSortingOrder;
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
            
            if (FloorManager.Instance.dataTransfer.playerInside)
            {
                Debug.Log("Player Goes Outdoors");

                PlayerOutdoors();
            }
            else if (!FloorManager.Instance.dataTransfer.playerInside)
            {
                Debug.Log("Player Goes Inside");
            
                PlayerIndoors();
            }
        }
    }
