using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


    public class AudioController : MonoBehaviour
    {
        public AudioMixer audioMixer;

        public void SetSfxVolume(Slider volume)
        {
            audioMixer.SetFloat("SFXVolume", volume.value);
        }
        public void SetMusicVolume(Slider volume)
        {
            audioMixer.SetFloat("MusicVolume", volume.value);
        }
        public void SetDialogueVolume(Slider volume)
        {
            audioMixer.SetFloat("DialogueVolume", volume.value);
        }
    }

