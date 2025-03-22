using UnityEngine;

    public class PlayerMovement : MonoBehaviour
    {
        private UserInput _userInput;
        public Animator anim;

        public bool permaLockMovement;
        [SerializeField] private Rigidbody2D rb; 
               
        [Header("Configurable Parameters")]
        [SerializeField] private float moveSpeed = 2.25f;
        
        private Vector2 _directionV2;
        
        // We're giving the string any initial value ("_Down"), to avoid yellow errors with Animator Index-1; which is the Animator not finding a string to play;
        // This makes sure the animator always can fall back to play "_Down", at any point!
        private string _direction = "_Down";

        private void Start()
        {
            _userInput = GetComponent<UserInput>();
        }
        

        private void Update()
        {
            if (permaLockMovement){
                _directionV2 = Vector2.zero; // Immediately stop movement
                Animate("Player_Idle");
                return;
            }
                
            if (!_userInput.gameScreenControlsActive) return;
            
            // Get raw input values
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            
            _directionV2.x = horizontalInput;
            _directionV2.y = verticalInput;
            
            
            // Animation direction
            if (_directionV2.y > Mathf.Sqrt(0.5f))
            {
                _direction = "_Up";
            }
            else if (_directionV2.y < -Mathf.Sqrt(0.5f))
            {
                _direction = "_Down";
            }
            else if (_directionV2.x > Mathf.Sqrt(0.5f))
            {
                _direction = "_Right";
            }
            else if (_directionV2.x < -Mathf.Sqrt(0.5f))
            {
                _direction = "_Left";
            }

            Animate(_directionV2 == Vector2.zero ? "Player_Idle" : "Player_Walk");
        }
        
            
        private void Animate(string unitAnimation)
        {
            anim.Play(unitAnimation + _direction);
        }

        private void FixedUpdate()
        {
            if (permaLockMovement) return;
            if (FloorManager.Instance == null) return;
            if (!FloorManager.Instance.dataTransfer.playerCanMove) return;

#if UNITY_EDITOR


            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                moveSpeed = 5.5f;
            }
            else
            {
                moveSpeed = 2.25f;
            }
#endif
            rb.MovePosition(rb.position + _directionV2.normalized * (moveSpeed * Time.fixedDeltaTime));
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.CompareTag("BottomFloor") && !other.CompareTag("CatPNG") &&
                !other.CompareTag("TopFloor")) return;
            //Debug.Log("Currently inside Trigger of: " + other.gameObject.name);
        }
    }
