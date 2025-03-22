using UnityEngine;

public class DeadToesReaction : MonoBehaviour
{
    [SerializeField] private AudioClip newBackgroundMusic;
    private bool realised;

    private float minDistanceToCat = 4.5f;

    private void Update()
    {
        if (FloorManager.Instance.player != null)
        {
            float distanceToCat = Vector3.Distance(FloorManager.Instance.player.transform.position, transform.position);

            if (distanceToCat < minDistanceToCat)
            {
                if (!FloorManager.Instance.dataTransfer.onTopFloor)
                {
               
                
                    Realisation();
                

                }

            }
        }   
    
    } 


    private void Realisation()
    {
        if (realised) return;
        realised = true;
        GetComponent<AudioSource>().Play();

        // Get the background music GameObject
        BackgroundMusic backgroundMusicObj = GameObject.FindGameObjectWithTag("BackgroundMusic").GetComponent<BackgroundMusic>();
        
        if (backgroundMusicObj != null)
        {
        backgroundMusicObj.musicSource.clip = newBackgroundMusic;
        backgroundMusicObj.musicSource.Play();
            
        }

    }
        
}
    

