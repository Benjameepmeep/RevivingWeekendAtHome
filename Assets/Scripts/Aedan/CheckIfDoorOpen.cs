using UnityEngine;

public class CheckIfDoorOpen : MonoBehaviour
{
    [SerializeField] private GameObject deadCat;

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


        if (FloorManager.Instance.dataTransfer.glassDoorOpen || !FloorManager.Instance.dataTransfer.catFlapClosed)
        {
            KillCat();
        }
    }


    private void KillCat()
    {
        Debug.LogWarning("Cat is dead");
        deadCat.SetActive(true);
        GetComponent<AudioSource>().Play();

        FloorManager.Instance.dataTransfer.catIsDead = true;
        GameObject.FindGameObjectWithTag("CatStuff").gameObject.SetActive(false);



    }
}
