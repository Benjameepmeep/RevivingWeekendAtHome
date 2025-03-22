using Cinemachine;
using UnityEngine;

    public class WindowsInteraction : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera mainCamera;
        private GameObject _lookAtPoint;
        [SerializeField] private GameObject player;
        [SerializeField] private bool insideTrigger;
        [SerializeField] private bool isLookingOutside;

        private void Start()
        {
            _lookAtPoint = transform.GetChild(0).gameObject;
        }

        private void Update()
        {
            if (player == null){
                player = GameObject.FindWithTag("Player");
            }
            if (insideTrigger && UserInput.Interact)
            {
                if (isLookingOutside)
                {
                    ReturnCamera();
                }
                else
                {
                    LookOutside();
                }
            }
        }

        private void LookOutside()
        {
            if (!mainCamera){
            CheckForCamera();
            }
            mainCamera.Follow = _lookAtPoint.transform;
            isLookingOutside = true;
        }

        private void ReturnCamera()
        {
            if (!mainCamera){
                CheckForCamera();
            }
            mainCamera.Follow = player.transform; 
            isLookingOutside = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            insideTrigger = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            insideTrigger = false;
            ReturnCamera();
        }

        private void CheckForCamera()
        {
            mainCamera = FindFirstObjectByType<CinemachineVirtualCamera>();
        }
    }

