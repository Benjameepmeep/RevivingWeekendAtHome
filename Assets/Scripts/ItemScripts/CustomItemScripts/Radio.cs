using UnityEngine;


    public class Radio : MonoBehaviour
    {
        [SerializeField] private BackgroundMusic _backgroundMusic;

        void Start()
        {
            if (!_backgroundMusic) _backgroundMusic = GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<BackgroundMusic>();
        }
    
        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            if (UserInput.Interact)
            {
                _backgroundMusic.RadioIsBeingInteractedWith();
            }
        }
    }

