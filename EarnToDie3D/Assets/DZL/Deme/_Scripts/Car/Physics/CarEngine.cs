using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public class CarEngine : MonoBehaviour
    {
        [SerializeField] float _maxTorque = 500f;
        [SerializeField] AnimationCurve _rpmTorqueCurve;
        // for speedometer
        public float CurrentSpeed => _rb.velocity.magnitude * 3.6f;
        Rigidbody _rb;
        [SerializeField] float _breakTorque = 1000f;

        float _currentTorque;
        bool _isInitialized = false;

        CarGearBox _gearBox;
        CarInput _carInput;
        public void Initialize(CarGearBox gearBox, CarInput carInput)
        {
            _gearBox = gearBox;
            _carInput = carInput;

            _rb = GetComponent<Rigidbody>();
            _isInitialized = true;
        }
        public void Move()
        {
            if (!_isInitialized) return;
            //float gear = _gearBox.CurrentGear;

            _currentTorque = _rpmTorqueCurve.Evaluate(Mathf.Abs(_carInput.MoveInput)) * _maxTorque; // * GearNum

            float torqueSigned = Mathf.Sign(_carInput.MoveInput) * _currentTorque;

            _gearBox.TryApplyMotorTorque(torqueSigned);

            _gearBox.TryBrake(_carInput.IsBrakePressed ? _breakTorque : 0);
        }
    }
}
