using System.Collections;
using UnityEngine;

    public class GlassDoorManager : MonoBehaviour
    {
        private Animator _animator;
        private BoxCollider2D _collider2D;
        private AudioSource _audioSource;
        [SerializeField] private AudioClip doorClosing, doorOpening;
        
        private bool _triggerActive;
        private float _slideTimer;
        void Start()
        {
            _collider2D = GetComponent<BoxCollider2D>();
            _animator = GetComponent<Animator>();
            _audioSource = GetComponent<AudioSource>();

            if (DataTransfer.glassDoorOpen)
            {
                _animator.Play("GlassDoorOpen");
            }
            else
            {
                _animator.Play("GlassDoorClosed");
            }
        }

        private IEnumerator PlayerIsNearGlassDoor()
        {
            yield return new WaitUntil(() => !_triggerActive || UserInput.Interact);

            if (!_triggerActive) yield break;
            
            // If glassDoor is Open, close it. If glassDoor is Closed, open it.
            if (DataTransfer.glassDoorOpen)
            {
                _animator.Play("GlassDoorOpening");
                _audioSource.PlayOneShot(doorOpening);
                StartCoroutine(UpdateCatPath(true));
            }
            else
            {
                _animator.Play("GlassDoorClosing");
                _audioSource.PlayOneShot(doorClosing);
                StartCoroutine(UpdateCatPath(false));
            }
            DataTransfer.OpenOrCloseGlassDoor();
            yield return PlayerIsNearGlassDoor();
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

