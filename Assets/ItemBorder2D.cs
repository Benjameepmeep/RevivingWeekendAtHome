using System.Collections;
using UnityEngine;

public class ItemBorder2D : MonoBehaviour
{
    public float maxOpacity = 1f;
    public float timeToOccilate = 1f;
    public float borderPxSize = 2f;
    public Color borderColor = Color.white;
    
    [Tooltip("Material with outline shader. Create this asset in your project.")]
    public Material outlineMaterial;
    
    private GameObject borderObject;
    private Coroutine borderCoroutine;
    private SpriteRenderer spriteRenderer;
    private Material instanceMaterial;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float borderActivationDistance = 2f;
    private bool borderActive = false;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (playerTransform == null) playerTransform = GameObject.FindWithTag("Player").transform;
        
        StopDisplayingBorder();
    }

    void Update()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            
            // If player is within range and border is not active, show border
            if (distanceToPlayer <= borderActivationDistance && !borderActive)
            {
                StartDisplayingOccilatingBorder();
                borderActive = true;
            }
            // If player is out of range and border is active, hide border
            else if (distanceToPlayer > borderActivationDistance && borderActive)
            {
                StopDisplayingBorder();
                borderActive = false;
            }
        }
    }


    public void StartDisplayingOccilatingBorder()
    {
        if (borderObject == null)
        {
            CreateBorder();
        }
        if (borderCoroutine != null)
        {
            StopCoroutine(borderCoroutine);
        }
        borderCoroutine = StartCoroutine(BorderOscillationCoroutine());
    }

    public void StopDisplayingBorder()
    {
        if (borderCoroutine != null)
        {
            StopCoroutine(borderCoroutine);
            borderCoroutine = null;
        }
        
        if (borderObject != null && instanceMaterial != null)
        {
            // Get the current opacity of the border
            Color currentColor = instanceMaterial.GetColor("_OutlineColor");
            float currentOpacity = currentColor.a;
            
            // Start fade out coroutine instead of immediately destroying
            borderCoroutine = StartCoroutine(FadeOutBorder(currentOpacity));
        }
        else
        {
            // If there's no border object or material, just clean up
            CleanupBorder();
        }
    }

    private void CreateBorder()
    {
        // Create a material instance
        if (outlineMaterial != null)
        {
            instanceMaterial = new Material(outlineMaterial);
            instanceMaterial.SetFloat("_OutlineWidth", borderPxSize);
            instanceMaterial.SetColor("_OutlineColor", new Color(borderColor.r, borderColor.g, borderColor.b, 0f));
        }
        else
        {
            Debug.LogError("Outline material not assigned to ItemBorder2D component!");
            return;
        }

        borderObject = new GameObject("SpriteBorder");
        borderObject.transform.parent = transform;
        borderObject.transform.localPosition = Vector3.zero;
        borderObject.transform.localRotation = Quaternion.identity;
        borderObject.transform.localScale = Vector3.one;

        SpriteRenderer borderSR = borderObject.AddComponent<SpriteRenderer>();
        borderSR.sprite = spriteRenderer.sprite;
        borderSR.sortingLayerID = spriteRenderer.sortingLayerID;
        borderSR.sortingOrder = spriteRenderer.sortingOrder - 1; // Draw behind original sprite
        borderSR.material = instanceMaterial;
    }

    private IEnumerator BorderOscillationCoroutine()
    {
        float time = 0f;
        while (true)
        {
            time += Time.deltaTime;
            // Calculate alpha in a ping-pong manner between 0 and maxOpacity
            float alpha = Mathf.PingPong(time, timeToOccilate) / timeToOccilate * maxOpacity;
            
            if (instanceMaterial != null)
            {
                // Update the outline color's alpha in the material
                Color outlineColor = borderColor;
                outlineColor.a = alpha;
                instanceMaterial.SetColor("_OutlineColor", outlineColor);
            }
            
            yield return null;
        }
    }

    private IEnumerator FadeOutBorder(float startingOpacity)
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < timeToOccilate && borderObject != null && instanceMaterial != null)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / timeToOccilate;
            float currentAlpha = Mathf.Lerp(startingOpacity, 0f, normalizedTime);
            
            // Update the outline color's alpha in the material
            Color outlineColor = borderColor;
            outlineColor.a = currentAlpha;
            instanceMaterial.SetColor("_OutlineColor", outlineColor);
            
            yield return null;
        }
        
        // Clean up after fade out is complete
        CleanupBorder();
    }

    private void CleanupBorder()
    {
        if (borderObject != null)
        {
            Destroy(borderObject);
            borderObject = null;
        }
        if (instanceMaterial != null)
        {
            Destroy(instanceMaterial);
            instanceMaterial = null;
        }
    }
}