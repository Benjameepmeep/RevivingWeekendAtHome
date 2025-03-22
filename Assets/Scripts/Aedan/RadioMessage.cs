using System.Collections;
using UnityEngine;

public class RadioMessage : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private float delay = 30f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            StartCoroutine(PlayAudioAfterDelay(delay));
        }
        else
        {
            Debug.LogError("RadioMessage: No AudioSource component found.");
        }
    }

    private IEnumerator PlayAudioAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.Play();
    }
}