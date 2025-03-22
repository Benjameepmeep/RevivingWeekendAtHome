using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class AudioController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider sfxVolumeSlider, musicVolumeSlider, dialogueVolumeSlider;

    void Update()
    {
        
        sfxVolumeSlider.value = audioMixer.GetFloat("SFXVolume", out float sfxValue) ? sfxValue : 0;
        musicVolumeSlider.value = audioMixer.GetFloat("MusicVolume", out float musicValue) ? musicValue : 0;
        dialogueVolumeSlider.value = audioMixer.GetFloat("DialogueVolume", out float dialogueValue) ? dialogueValue : 0;
    }

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

