using UnityEngine;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CatFoodScript : MonoBehaviour
{
    public ItemType[] itemScrub;
    public TMP_Text itemName;
    public TMP_Text itemText;
    public Image itemImage;
    public AudioSource audioPlayer;
    public PlayableDirector timeline;
    public static bool catFoodFull;
    //Should Get From Another Script

    private void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        //aitemName.text = ItemObjectScript.currentObjectInt.ToString();
        audioPlayer.PlayOneShot(itemScrub[ItemObjectScript.currentObjectInt].itemAudio);

        if (catFoodFull)
        {
            itemText.text = itemScrub[1].itemText;
            itemImage.sprite = itemScrub[1].itemImage;
        }
        else
        {
            itemImage.sprite = itemScrub[0].itemImage;
            itemName.text = itemScrub[0].itemName;
        }
    }

    public void OnClickYes()
    {
        catFoodFull = true;
        timeline.Play();
        audioPlayer.PlayOneShot(itemScrub[ItemObjectScript.currentObjectInt].cutSceneAudio);
    }

    public void OnClickNo()
    {
        Debug.Log("DeleteScene");
        SceneManager.UnloadSceneAsync("InteractableItem");
    }
}