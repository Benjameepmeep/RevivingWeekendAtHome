using System.Collections;
using UnityEngine;

public class ItemBorder2D : MonoBehaviour
{
    private float maxOpacity;
    private float timeToOccilate;
    private float borderPxSize;
    public Color borderColor = Color.white;
    
    [Tooltip("Material with outline shader. Create this asset in your project.")]
    public Material outlineMaterial;
    
    private GameObject borderObject;
    private Coroutine borderCoroutine;
    private SpriteRenderer spriteRenderer;
    private Material instanceMaterial;
    [SerializeField] private Transform playerTransform;
    private float borderActivationDistance = 2f;
    private bool borderActive = false;
    private Collider2D myCollider;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        myCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        if (playerTransform == null) playerTransform = GameObject.FindWithTag("Player").transform;
        
        StopDisplayingBorder();
        borderActivationDistance = 0.01f;
        borderPxSize = 3f;
        timeToOccilate = 1.5f;
        maxOpacity = 1f;
    }

    void Update()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer;
            
            if (myCollider != null)
            {
                // Get the player's collider
                Collider2D playerCollider = playerTransform.GetComponent<Collider2D>();
                
                if (playerCollider != null)
                {
                    // Use the distance between the colliders for more accurate proximity detection
                    distanceToPlayer = Physics2D.Distance(myCollider, playerCollider).distance;
                }
                else
                {
                    // Fallback to closest point on collider if player has no collider
                    Vector2 playerPos = playerTransform.position;
                    distanceToPlayer = Vector2.Distance(myCollider.ClosestPoint(playerPos), playerPos);
                }
            }
            else
            {
                // Fallback to simple transform distance if no collider is available
                distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            }
            
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
        
        while (elapsedTime < (timeToOccilate / 2) && borderObject != null && instanceMaterial != null)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / (timeToOccilate / 2);
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