using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIBounceOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Amplitude of the bounce (e.g., 0.1 equals 10% scale increase)
    public float bounceAmplitude = 0.1f;
    // Speed (frequency) of the bounce
    public float bounceSpeed = 3f;

    private Vector3 originalScale;
    private Coroutine bounceCoroutine;

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