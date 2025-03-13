using UnityEngine;


public class CatFoodFull : MonoBehaviour
{
    public static bool CatBowlFull;
    private SpriteRenderer _sprite;

    private void Start()
    {
        _sprite = gameObject.GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (CatBowlFull)
        {
            _sprite.enabled = true;
        }
        else
        {
            _sprite.enabled = false;
        }
    }
}

