using System.Collections;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;


    public class CatInteractionScript : MonoBehaviour
    {
        private GameObject _catPNG;
        private CatScript _catScript;
        
        [Header("If a random number between 0-100 is below the threshold, choose new goal:")] 
        [Range(0, 100)]
        [SerializeField] private int threshold = 33;

        private void Start()
        {
            _catPNG = GameObject.FindGameObjectWithTag("CatPNG");
            _catScript = GetComponent<CatScript>();
            _catScript.aiPath = GetComponentInParent<AIPath>();
        }
        public IEnumerator CheckIfCatShouldStop()
        {
            yield return new WaitUntil(() => !ItemController.playerIsInsideItemTrigger || ItemObjectScript.currentlyOpeningItem);
            
            if (ItemObjectScript.currentlyOpeningItem)
            {
                StartCoroutine(InteractingWithCat());
            }
            else if (!ItemController.playerIsInsideItemTrigger) 
                yield break;
        }

        private IEnumerator InteractingWithCat()
        {
            Debug.Log("Cat is stopping, cuz player is interacting with it");
            _catScript.aiPath.isStopped = true;
            yield return new WaitUntil(() => ItemObjectScript.inItemScene);
            yield return null;
            yield return new WaitUntil(() => !ItemObjectScript.inItemScene);
            _catScript.aiPath.isStopped = false;
            if (CheckIfCatChangesRoute())
            {
                _catScript.OnGoalEnter();
            }
        }

        private bool CheckIfCatChangesRoute()
        {
            var number = Random.Range(1, 100);
            return number <= threshold;
        }
    }

