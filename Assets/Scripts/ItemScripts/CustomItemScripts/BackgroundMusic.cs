using System.Collections;
using UnityEngine;


    public class BackgroundMusic : MonoBehaviour
    {
        [SerializeField] private AudioSource turnOnSfxSource;
        [SerializeField] private AudioSource turnOffSfxSource;
        public AudioSource musicSource;
        [SerializeField] private AudioClip radioMusic;
        
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

            if (FloorManager.Instance.dataTransfer.radioOn)
            {
                musicSource.mute = false;
            }
            else
            {
                musicSource.mute = true;
            }
        }

        private void Update()
        {
            if (FloorManager.Instance == null) return;
            if (FloorManager.Instance.dataTransfer.onTopFloor)
            {
                //Debug.Log("On top floor, music reduced to 0.4f.");
                musicSource.volume = 0.1f;
            }
            else if (!FloorManager.Instance.dataTransfer.onTopFloor)
            {
                if (FloorManager.Instance.dataTransfer.playerInside)
                {
                    //Debug.Log("Inside bottom floor, music at 1.0f.");
                    musicSource.volume = 0.2f;
                }
                else if (!FloorManager.Instance.dataTransfer.playerInside)
                {
                    //Debug.Log("Outside, music at 0.6f.");
                    musicSource.volume = 0.1f;
                }
            }
        }

        public void RadioIsBeingInteractedWith()
        {
            if (FloorManager.Instance.dataTransfer.radioOn)
            {
                // Debug.Log("Radio on and being interacted with");
                turnOffSfxSource.Play(0);
                StartCoroutine(MuteRadio(0.30f));
            }
            else
            {
                // Debug.Log("Radio off and being interacted with");
                turnOnSfxSource.Play(0);
                StartCoroutine(UnmuteRadio(1.8f));
            }
            FloorManager.Instance.dataTransfer.ToggleRadio();
        }
    
        private IEnumerator UnmuteRadio(float delay)
        {
            yield return new WaitForSeconds(delay);
            // Debug.Log("Radio music is playing");
            // TODO: Instead of volume going straight to max, make it so the volume gradually is increased.
            musicSource.mute = false;
        } 
    
        private IEnumerator MuteRadio(float delay)
        {
            yield return new WaitForSeconds(delay);
            // Debug.Log("Radio music is muted");
            musicSource.mute = true;
        }
    }

