using System.Collections;
using Pathfinding;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;


    public class CatScript : MonoBehaviour
    {
        public AIPath aiPath;
        public Animator anim;
        private GameObject cat;

        public GameObject goal;
        public AudioSource meowAudioSource;
        public AudioClip[] meows;
        public Vector2[] goalSpots;

        public bool[] staysForRandom;
        public float[] howLongToStay;

        private string _direction;
        private Vector2 _directionValue;
        private Vector2 _lastPosition;

        private SortingGroup catSortingGroup;

        public bool onTheMove = true;

        private int _randomGoal;
        private float _timeBetweenMeows;
        public bool destinationReached;
        private bool _goingTowardsCatFood;

        private void Start()
        {
            cat = transform.parent.gameObject;

            catSortingGroup = cat.GetComponent<SortingGroup>();


            Invoke("Check", 0.3f);
            
        }


        private void Check(){
            
            if (FloorManager.Instance == null)
            {
                Invoke("Check", 0.15f);
                return;
            }

            
            FloorManager.Instance.cat = cat;
            FloorManager.Instance.catTriggerBox = GetComponent<BoxCollider2D>();
            FloorManager.Instance.catSprite = cat.GetComponentInChildren<SpriteRenderer>();

            if (FloorManager.Instance.dataTransfer.catOutside)
            {
                catSortingGroup.sortingOrder = FloorManager.Instance.dataTransfer.CatSortingOrderOutside;
                // Debug.Log("Cat Is Outside");
            }
            else
            {
                catSortingGroup.sortingOrder = FloorManager.Instance.dataTransfer.catSortingOrderInside;
                // Debug.Log("Cat Is Inside");
            }

            if (FloorManager.Instance.dataTransfer.onTopFloor){
                FloorManager.Instance.DisableCatVisuals();
            }
        }

        private void Update()
        {
            if (FloorManager.Instance == null) return;
            // TODO: Fix bug that happens when player is outside and walks inside, causing the cat to appear above the inside walls if outside as well.


            if (FloorManager.Instance.dataTransfer.catOutside)
            {
                catSortingGroup.sortingOrder = FloorManager.Instance.dataTransfer.CatSortingOrderOutside;
            }
            else
            {
                catSortingGroup.sortingOrder = FloorManager.Instance.dataTransfer.catSortingOrderInside;
            }

            // Consider using Math.Sign to return a value of either -1, 0 or 1, and use that for animations.
            // Calculates direction from last-pos to current-pos
            var position = transform.position;
            var directionValue = ((Vector2)position - _lastPosition).normalized;

            // Updates last-pos for use in next Update() cycle
            _lastPosition = position;

            if (directionValue.y > Mathf.Sqrt(0.5f))
            {
                _direction = "_Up";
            }
            else if (directionValue.y < -Mathf.Sqrt(0.5f))
            {
                _direction = "_Down";
            }
            else if (directionValue.x > Mathf.Sqrt(0.5f))
            {
                _direction = "_Right";
            }
            else if (directionValue.x < -Mathf.Sqrt(0.5f))
            {
                _direction = "_Left";
            }

            Animate(directionValue == Vector2.zero ? "Toes_Idle" : "Toes_Walk");
        }

        private void FixedUpdate()
        {
            if (FloorManager.Instance == null) return;
            //Counter For Random Occasional Meows
            if (_timeBetweenMeows > 0)
            {
                _timeBetweenMeows -= Time.deltaTime;
            }
            else
            {
                meowAudioSource.clip = meows[Random.Range(0, meows.Length)];
                meowAudioSource.PlayOneShot(meowAudioSource.clip);
                _timeBetweenMeows = Random.Range(10, 30);
            }
            
            if (!onTheMove) return;

            //If At Destination. Play Goal Function
            if (aiPath.reachedDestination && !destinationReached)
            {
                destinationReached = true;
                onTheMove = false;
                OnGoalEnter();
            }

            if (!FloorManager.Instance.dataTransfer.CatBowlFull) return;

            _goingTowardsCatFood = true;
            goal.transform.position = new Vector3(1, 0, 0);
        }
        
        private void Animate(string unitAnimation)
        {
            anim.Play(unitAnimation + _direction);
        }

        //Plays When At Goal, Sets A New Goal, Then Starts Coroutine
        internal void OnGoalEnter()
        {
            StartCoroutine(WaitForNewGoal());
        }

        // Waits random amount of seconds, then starts new Goal
        private IEnumerator WaitForNewGoal()
        {
            yield return new WaitForSeconds(howLongToStay[_randomGoal]);
            if (staysForRandom[_randomGoal])
            {
                yield return new WaitForSeconds(Random.Range(0, 10));
            }
    
            if (_goingTowardsCatFood)
            {
                yield return new WaitForSeconds(13);
                FloorManager.Instance.dataTransfer.CatBowlFull = false;
                _goingTowardsCatFood = false;
            }

            _randomGoal = Random.Range(0, goalSpots.Length);
            goal.transform.position = goalSpots[_randomGoal];
            onTheMove = true;
            destinationReached = false;
        }
    }
