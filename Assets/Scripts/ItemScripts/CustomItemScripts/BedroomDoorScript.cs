using UnityEngine;

    public class BedroomDoorScript : MonoBehaviour
    {
        [SerializeField] private float slideTimer;
    
        private bool _triggerActive;
        private Animator _animator;
    
        private AudioSource _audioSource;
        [SerializeField] private AudioClip bedroomDoorClosing, bedroomDoorOpening;
    
        void Start()
        {
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();

            if (DataTransfer.bedroomDoorOpen)
            {
                Debug.Log("BedroomDoorIsOpen");
                _animator.Play("BedroomDoorOpen");
            }
            else if (!DataTransfer.bedroomDoorOpen)
            {
                Debug.Log("BedroomDoorIsClosed");
                _animator.Play("BedroomDoorClosed");
            }
        }

        private void Update()
        {
            slideTimer += Time.deltaTime;
        
            if (!_triggerActive)
                return;
        
            // if animation is done, and player is within triggerBox and presses Interact, continue downwards.
            if (!(slideTimer > 1f) || !UserInput.Interact) 
                return;
            
            // If bedroomDoor is open, close it. If bedroomDoor is closed, open it.
            switch (DataTransfer.bedroomDoorOpen)
            {
                case false:
                    Debug.Log("BedroomDoorIsOpening");
                    _animator.Play("BedroomDoorOpening");
                    _audioSource.PlayOneShot(bedroomDoorOpening);
                    break;
                case true:
                    Debug.Log("BedroomDoorIsClosing");
                    _animator.Play("BedroomDoorClosing");
                    _audioSource.PlayOneShot(bedroomDoorClosing);
                    break;
            }
            DataTransfer.OpenOrCloseBedroomDoor();
        
            // Reset the timer to be 0.
            slideTimer = -Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D other) { _triggerActive = true; }
        private void OnTriggerExit2D(Collider2D other) { _triggerActive = false; }
    }
