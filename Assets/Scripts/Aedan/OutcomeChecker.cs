using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutcomeChecker : MonoBehaviour
{
    
    [SerializeField] private AudioSource audioSource;

    void Start()
    {
        Checck();
        audioSource = GetComponent<AudioSource>();
    }

    private void Checck(){

        if (FloorManager.Instance == null)
        {
            Invoke(nameof(Checck), 0.2f);
            return;
        }
        
        if (FloorManager.Instance.dataTransfer.outcome == DataTransfer.Outcome.stayed_in_bed_all_day)
        {
            audioSource.Play();
        }
        }
        
           
    }
    
    



