using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public class NeedleMeter : MonoBehaviour
    {
        [SerializeField] AnimationCurve _needleMeterCurve;
        [SerializeField] RectTransform _needleTransform;
        [SerializeField] float _maxAngle = 0f; // note that it is reversed here and max angle is < min angle, this is max speed angle
        float _minAngle = 0f;
        float _currentAngle = 0f;

        void Start()
        {
            _minAngle = _needleTransform.localEulerAngles.z;
            _currentAngle = _minAngle;
        }
        public void UpdateCurrentAngle(float percent)
        {
            _currentAngle = Mathf.Lerp(_minAngle, _maxAngle, percent); // reversed
            _needleTransform.eulerAngles = new Vector3(0, 0, _currentAngle);
        }
    }
}
