using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBounceOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Amplitude of the bounce (e.g., 0.1 equals 10% scale increase)
    public float bounceAmplitude = 0.1f;
    // Speed (frequency) of the bounce
    public float bounceSpeed = 3f;

    public bool MainUIBtn = false;


    private Vector3 originalScale;
    private Coroutine bounceCoroutine;

    private UserInput userInput;

    

    void Update()
    {
        if (UserInput.AnyKeyOrStart)
        {
            HandleAnyButtonPress();
        }
    }

    private void HandleAnyButtonPress()
    {
        // If MainUIBtn is enabled and no UI element is currently selected, select this GameObject.
        if (MainUIBtn && EventSystem.current != null && EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Start a single bounce
        if (bounceCoroutine != null)
        {
            StopCoroutine(bounceCoroutine);
        }
        bounceCoroutine = StartCoroutine(BounceOnce());
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // If the pointer leaves, stop any running bounce and reset scale.
        if (bounceCoroutine != null)
        {
            StopCoroutine(bounceCoroutine);
            bounceCoroutine = null;
        }
        transform.localScale = originalScale;

        if (EventSystem.current != null)
        {
        EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private IEnumerator BounceOnce()
    {
        float elapsed = 0f;
        // One bounce: one full sine wave cycle from 0 to π.
        float duration = Mathf.PI / bounceSpeed;

        while (elapsed < duration)
        {
            // Calculate a damping factor that linearly interpolates from 1 to 0.
            float damping = Mathf.Lerp(1f, 0f, elapsed / duration);
            float scaleFactor = 1 + bounceAmplitude * damping * Mathf.Sin(bounceSpeed * elapsed);
            transform.localScale = originalScale * scaleFactor;
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
        bounceCoroutine = null;
    }
}