using UnityEngine;


public class CatFoodFull : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    void Update()
    {

        if (FloorManager.Instance == null)
        {
            return;
        }
        if (FloorManager.Instance.dataTransfer.CatBowlFull)
        {
            spriteRenderer.sprite = sprites[1];
        }
        else
        {
            spriteRenderer.sprite = sprites[0];
        }
    }
}

