using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BathroomDoor : MonoBehaviour
{
    private bool _playerIsInTrigger;
    private GameObject _player;

    [SerializeField] private Sprite[] doorSprites;
    private SpriteRenderer doorSpriteRenderer;
    [SerializeField] private BoxCollider2D doorCollider;
    [SerializeField] private GameObject DoorSpriteToToggle;


    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
        doorSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_playerIsInTrigger && UserInput.Interact)
        {
            doorSpriteRenderer.sprite = doorSpriteRenderer.sprite == doorSprites[0] ? doorSprites[1] : doorSprites[0];
            doorCollider.enabled = !doorCollider.enabled;
            DoorSpriteToToggle.SetActive(!DoorSpriteToToggle.activeSelf);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerIsInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _playerIsInTrigger = false;
        }
    }
}
