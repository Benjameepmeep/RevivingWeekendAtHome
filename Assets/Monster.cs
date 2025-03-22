using UnityEngine;
using UnityEngine.SceneManagement;

public class Monster : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    // Start is called before the first frame update
    void Start()
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


        if (FloorManager.Instance.dataTransfer.glassDoorOpen)
        {
            SpawnMonster();
        }
        else {
            
            if (FloorManager.Instance.dataTransfer.catIsDead)
            {
                FloorManager.Instance.dataTransfer.outcome = DataTransfer.Outcome.Player_alive_cat_dead;
                LoadSurvivalCredits();
            }
            else
            {
                FloorManager.Instance.dataTransfer.outcome = DataTransfer.Outcome.Player_alive_cat_alive;
                LoadSurvivalCredits();
            }

        }
    }

    private void SpawnMonster()
    {
        FloorManager.Instance.dataTransfer.outcome = DataTransfer.Outcome.Player_dead_cat_dead;
        GetComponent<AudioSource>().Play();

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        Invoke(nameof(LoadDeathCredits), 5f);
    }


    private void LoadDeathCredits()
    {
        SceneManager.LoadScene("DeathCredits");
    }

    private void LoadSurvivalCredits()
    {
        SceneManager.LoadScene("PlayerSurviveCredits");
    }
    
}
