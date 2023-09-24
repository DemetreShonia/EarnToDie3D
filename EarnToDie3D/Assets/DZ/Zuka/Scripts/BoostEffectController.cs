using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using MoreMountains.Feedbacks;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace DumbRide
{
    public class BoostEffectController : MonoBehaviour
    {
        [SerializeField] MMFeedbacks _boostEffectStart;
        [SerializeField] MMFeedbacks _boostEffectEnd;
        [SerializeField] CinemachineVirtualCamera _cam;
        [SerializeField] float _boostFov = 60;
        [SerializeField] float _lerpSpeed = 1;
        [SerializeField] float _boostChromaticAberration = 1;
        [SerializeField] float _chromaticLerpSpeed = 1;
        [SerializeField] Volume _postPVolume;

        ChromaticAberration _chromaticAber;

        float _defaultFov = 60;
        float _targetFov = 60;

        float _defaultChromatic = 0;
        float _targetChromatic = 0;

        private void Start()
        {
            _defaultFov = _cam.m_Lens.FieldOfView;
            if (_postPVolume.profile.TryGet<ChromaticAberration>(out var c))
            {
                _chromaticAber = c;
            }
            _defaultChromatic = c.intensity.value;

            _targetFov = _defaultFov;
            _targetChromatic = _defaultChromatic;
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                StartEffect();
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                EndEffect();
            }
            UpdateFov();
            UpdateChromatic();
        }
        private void UpdateFov()
        {
            if (_cam.m_Lens.FieldOfView == _targetFov)
            {
                return;
            }
            if (Mathf.Abs(_cam.m_Lens.FieldOfView - _targetFov) < 0.1f)
            {
                _cam.m_Lens.FieldOfView = _targetFov;
                return;
            }
            float blend = 1 - Mathf.Pow(0.5f, _lerpSpeed * Time.deltaTime);
            _cam.m_Lens.FieldOfView = Mathf.Lerp(_cam.m_Lens.FieldOfView, _targetFov, blend);
        }

        private void UpdateChromatic()
        {
            if (_chromaticAber.intensity.value == _targetChromatic)
            {
                return;
            }
            if (Mathf.Abs(_chromaticAber.intensity.value - _targetChromatic) < 0.1f)
            {
                _chromaticAber.intensity.value = _targetChromatic;
                return;
            }
            float blend = 1 - Mathf.Pow(0.5f, _chromaticLerpSpeed * Time.deltaTime);
            _chromaticAber.intensity.value = Mathf.Lerp(_chromaticAber.intensity.value, _targetChromatic, blend);
        }

        private void StartEffect()
        {
            _targetFov = _boostFov;
            _targetChromatic = _boostChromaticAberration;
            _boostEffectStart?.PlayFeedbacks();
        }

        private void EndEffect()
        {
            _targetFov = _defaultFov;
            _targetChromatic = _defaultChromatic;
            _boostEffectEnd?.PlayFeedbacks();
        }
    }
}
