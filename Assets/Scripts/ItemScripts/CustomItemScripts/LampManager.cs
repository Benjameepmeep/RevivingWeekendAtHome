using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

    public class LampManager : MonoBehaviour
    {
        [SerializeField] private Light2D lampLight;
    
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip lampOnSfx, lampOffSfx;
    
        private bool _playerInTriggerArea;
        private bool _interactCooldown;

        private void Start()
        {
            lampLight = GetComponent<Light2D>();

            
            Check();
        }

        private void Check(){

            if (FloorManager.Instance == null){
                Invoke(nameof(Check), 0.2f);

                return;
            }

            if (FloorManager.Instance.dataTransfer.lampOn)
            {
                lampLight.enabled = true;
            }
            else
            {
                lampLight.enabled = false;
            }
        }
        
        private IEnumerator NewScene()
        {
            yield return null;
            lampLight.enabled = FloorManager.Instance.dataTransfer.lampOn;
        }

        private void Update()
        {
            if (_playerInTriggerArea && UserInput.Interact && !ItemObjectScript.inItemScene && !_interactCooldown)
            {
                StartCoroutine(ToggleLamp());
            }
        }

        private IEnumerator ToggleLamp()
        {
            _interactCooldown = true;
            
            FloorManager.Instance.dataTransfer.ToggleLamp();
            
            if (FloorManager.Instance.dataTransfer.lampOn)
            {
                audioSource.PlayOneShot(lampOnSfx);
                lampLight.enabled = true; 
            }
            else
            {
                audioSource.PlayOneShot(lampOffSfx);
                lampLight.enabled = false;
            }
            
            yield return new WaitForSeconds(0.3f); // Small cooldown to prevent multiple toggles
            _interactCooldown = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return; 
            _playerInTriggerArea = true;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return; 
            _playerInTriggerArea = false;
        }
    }

