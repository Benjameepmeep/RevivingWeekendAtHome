using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BackToMainMenu : MonoBehaviour
{
    private string sceneName;

    private float elapsedFadeTime;
        private float startExposure;
        private float endExposure;
        private float fadeDuration;

        private ColorAdjustments colorAdjustments;
        private Volume globalVolume;
        [SerializeField] private bool isCredits;

    void Start()
    {
        if (isCredits){
            Invoke(nameof(AfterWaiting), 36f);
        }
        
    }

    private void AfterWaiting(){
        BackToMainMenuScene("AScene 0 - Title Screen");
    }



    public void BackToMainMenuScene(string nameOfScene)
    {        
        sceneName = nameOfScene;
        Invoke("LoadScene", 2f);

        globalVolume = FindFirstObjectByType<Volume>().GetComponent<Volume>();
        globalVolume.profile.TryGet(out colorAdjustments);


        StartCoroutine(FadeToBlack());
    }


    private void LoadScene()
    {
        // Load the specified scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    private IEnumerator FadeToBlack()
    {
        fadeDuration = 1.8f; 
        elapsedFadeTime = 0f;
        
        startExposure = 0f;
        endExposure = -15f;

       
       while (elapsedFadeTime < fadeDuration)
        {
            float newExposure = Mathf.Lerp(startExposure, endExposure, elapsedFadeTime / fadeDuration);
            colorAdjustments.postExposure.Override(newExposure);
    
            elapsedFadeTime += Time.unscaledDeltaTime;
            yield return null;
        }

        colorAdjustments.postExposure.Override(endExposure);

        
        elapsedFadeTime = 0f;
        if (FloorManager.Instance) Destroy(FloorManager.Instance.transform.parent.gameObject);
        
        
        
    }
    
}
