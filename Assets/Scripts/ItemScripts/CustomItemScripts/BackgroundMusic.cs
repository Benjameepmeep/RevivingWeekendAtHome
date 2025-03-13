using System;
using System.Collections;
using UnityEngine;


    public class BackgroundMusic : MonoBehaviour
    {
        [SerializeField] private AudioSource turnOnSfxSource;
        [SerializeField] private AudioSource turnOffSfxSource;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioClip radioMusic;
        
        private void Start()
        {
            if (!DataTransfer.radioOn)
            {
                StartCoroutine(MuteRadio(0f));
            }
            musicSource.clip = radioMusic;
            musicSource.Play(0);
            if (!musicSource.loop)
            {
                musicSource.loop = true;
            }
        }

        private void Update()
        {
            if (DataTransfer.onTopFloor)
            {
                //Debug.Log("On top floor, music reduced to 0.4f.");
                musicSource.volume = 0.4f;
            }
            else if (!DataTransfer.onTopFloor)
            {
                if (DataTransfer.playerInside)
                {
                    //Debug.Log("Inside bottom floor, music at 1.0f.");
                    musicSource.volume = 1f;
                }
                else if (!DataTransfer.playerInside)
                {
                    //Debug.Log("Outside, music at 0.6f.");
                    musicSource.volume = 0.6f;
                }
            }
        }

        public void RadioIsBeingInteractedWith()
        {
            if (DataTransfer.radioOn)
            {
                // Debug.Log("Radio on and being interacted with");
                turnOffSfxSource.Play(0);
                StartCoroutine(MuteRadio(0.30f));
            }
            else if (!DataTransfer.radioOn)
            {
                // Debug.Log("Radio off and being interacted with");
                turnOnSfxSource.Play(0);
                StartCoroutine(UnmuteRadio(1.8f));
            }
            DataTransfer.TurnRadioOnOrOff();
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

