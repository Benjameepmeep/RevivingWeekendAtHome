using System.Collections;
using UnityEngine;

    public class CatFlapScript : MonoBehaviour
    {
        [SerializeField] private GameObject catFlapDoorObject;
        private BoxCollider2D _catFlapDoorTrigger;
        private Vector3 _originalDoorPosition;

        [SerializeField] private AudioSource catFlapAudioSource;
        [SerializeField] private AudioClip catFlapUnlock, catFlapLock;
    
        private bool _playerInTriggerArea;
        
        // Start is called before the first frame update
        void Awake()
        {
            catFlapDoorObject = GameObject.FindWithTag("CatFlapCollision");
            _catFlapDoorTrigger = GetComponent<BoxCollider2D>();
            if (catFlapDoorObject != null)
            {
                _originalDoorPosition = transform.position;
            }
        }

        private void Start()
        {
            Check();
        }

        private void Check()
        {
            if (FloorManager.Instance == null)
            {
                Invoke(nameof(Check), 0.2f);
                return;
            }

            if (FloorManager.Instance.dataTransfer.catFlapClosed)
            {
                catFlapDoorObject.SetActive(true);
                transform.position = _originalDoorPosition;
            }
            else
            {
                UnlockCatFlap();
            }
        }

        private void Update()
        {
            if (_playerInTriggerArea && UserInput.Interact)
            {
                ToggleCatFlap();
            }
        }

        private void ToggleCatFlap()
        {
            FloorManager.Instance.dataTransfer.ToggleCatFlap();
            
            if (FloorManager.Instance.dataTransfer.catFlapClosed)
            {
                catFlapAudioSource.PlayOneShot(catFlapLock);
                catFlapDoorObject.SetActive(true);
                // Ensure door is in original position when closed
                transform.position = _originalDoorPosition;
                GetComponent<BoxCollider2D>().offset = new Vector2(GetComponent<BoxCollider2D>().offset.x + 0.4f, GetComponent<BoxCollider2D>().offset.y);

                //Debug.Log("Catflap is now locked");
                            
                StartCoroutine(UpdateCatPath());

            }
            else
            {
                UnlockCatFlap();
                
                FloorManager.Instance.dataTransfer.numberOfTimesOpenedDoorOrCatFlap++;
            }
        }

        private void UnlockCatFlap()
        {

            catFlapAudioSource.PlayOneShot(catFlapUnlock);
            catFlapDoorObject.SetActive(false);
            // Shift door to the right by 0.4 units when open
            // Even though the door is technically inactive, we set the position so when it becomes active again, it's in the right place
            transform.position = _originalDoorPosition + new Vector3(0.4f, 0, 0);
            GetComponent<BoxCollider2D>().offset = new Vector2(GetComponent<BoxCollider2D>().offset.x - 0.4f, GetComponent<BoxCollider2D>().offset.y);
            //Debug.Log("Catflap is now unlocked");
            StartCoroutine(UpdateCatPath());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _playerInTriggerArea = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _playerInTriggerArea = false;
            }
        }

        private IEnumerator UpdateCatPath()
        {
            yield return new WaitForSeconds(0.3f);
            AstarPath.active.UpdateGraphs(_catFlapDoorTrigger.bounds);
        }
    }

