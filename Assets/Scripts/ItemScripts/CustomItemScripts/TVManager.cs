using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

    public class TVManager : MonoBehaviour
    {
        private Light2D _tvLight;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip tvTurningOn, tvTurningOff;

        [Header("Tv Light Settings:")] 
        [SerializeField] [Range(0, 1)] private float lightIntensity = 0.25f;
        [SerializeField] [Range(0, 1)] private float colourTransparency = 0.02f;
        
        [Header("How often (in seconds) does the TV switch colours?")]
        [SerializeField] private float colourSwitchTimer = 2f;
        private bool _triggerActive;
        
        private void Start()
        {
            _tvLight = GetComponent<Light2D>();
            StartCoroutine(NewScene());
        }

        private IEnumerator NewScene()
        {
            yield return null;

            if (DataTransfer.tvOn)
            {
                StartCoroutine(ChangeLightContinuously());
            }
            else
            {
                _tvLight.enabled = false;
            }
        }

        private IEnumerator PlayerNearTV()
        {
            yield return new WaitUntil(() => !ItemObjectScript.inItemScene && UserInput.Interact || !_triggerActive);
            if (!_triggerActive) yield break;
            DataTransfer.TurnTVOnOrOff();
            yield return null;
            if (DataTransfer.tvOn)
            {
                audioSource.PlayOneShot(tvTurningOn);
                _tvLight.enabled = true; 
                StartCoroutine(ChangeLightContinuously());
            }
            else if (!DataTransfer.tvOn)
            {
                audioSource.PlayOneShot(tvTurningOff);
                _tvLight.enabled = false;
                StopCoroutine(ChangeLightContinuously());
            }
            yield return PlayerNearTV();
        }

        #region --- Triggers ---

        private void OnTriggerEnter2D(Collider2D other)
        { 
            if (!other.CompareTag("Player")) return; 
            _triggerActive = true;
            StartCoroutine(PlayerNearTV());
        }
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _triggerActive = false;
        }

        #endregion
        
        private IEnumerator ChangeLightContinuously()
        {
            if (!DataTransfer.tvOn)
            { 
                _tvLight.intensity = 0;
                yield break;
            }
            _tvLight.intensity = lightIntensity;
            _tvLight.color = new Color(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255), colourTransparency);
            yield return new WaitForSeconds(colourSwitchTimer);
            yield return ChangeLightContinuously();
        }
    }

