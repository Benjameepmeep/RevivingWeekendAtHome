using System.Collections;
using UnityEngine;

    public class GlassDoorManager : MonoBehaviour
    {
        private Animator _animator;
        private BoxCollider2D _collider2D;
        private AudioSource _audioSource;
        [SerializeField] private AudioClip doorClosing, doorOpening;
        
        private bool _triggerActive;

        private bool _doorActionCooldown = false;
        
        void Start()
        {
            _collider2D = GetComponent<BoxCollider2D>();
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();

            Check();

            
        }

        private void Check(){

            if (FloorManager.Instance == null){
                Invoke(nameof(Check), 0.2f);

                return;
            }

            if (FloorManager.Instance.dataTransfer.glassDoorOpen)
            {
                _animator.Play("GlassDoorOpening");
            }
            else
            {
                _animator.Play("GlassDoorClosing");
            }

        }

        private IEnumerator PlayerIsNearGlassDoor()
        {
            yield return new WaitUntil(() => _triggerActive && UserInput.Interact && !_doorActionCooldown);
            // Start cooldown
            StartCoroutine(DoorActionCooldown());

            if (!_triggerActive) yield break;
            
           
            
            // If glassDoor is Open, close it. If glassDoor is Closed, open it.
            if (FloorManager.Instance.dataTransfer.glassDoorOpen)
            {
                _animator.Play("GlassDoorClosing");
                _audioSource.PlayOneShot(doorClosing);
                StartCoroutine(UpdateCatPath(false));
            }
            else
            {
                _animator.Play("GlassDoorOpening");
                _audioSource.PlayOneShot(doorOpening);
                StartCoroutine(UpdateCatPath(true));
                FloorManager.Instance.dataTransfer.numberOfTimesOpenedDoorOrCatFlap++;
            }
            FloorManager.Instance.dataTransfer.OpenOrCloseGlassDoor();
            yield return PlayerIsNearGlassDoor();
        }
        
        private IEnumerator DoorActionCooldown()
        {
            _doorActionCooldown = true;
            yield return new WaitForSecondsRealtime(2f);
            _doorActionCooldown = false;
        }

        private IEnumerator UpdateCatPath(bool doorIsOpening)
        {
            if (doorIsOpening)
            {
                yield return new WaitForSeconds(2.25f);
            }
            else
            {
                yield return new WaitForSeconds(1.5f);
            }
            AstarPath.active.UpdateGraphs(_collider2D.bounds); 
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other == null || !other.CompareTag("Player")) return;
            _triggerActive = true;
            StartCoroutine(PlayerIsNearGlassDoor());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other == null || !other.CompareTag("Player")) { return; }
            _triggerActive = false; 
        }
    }

