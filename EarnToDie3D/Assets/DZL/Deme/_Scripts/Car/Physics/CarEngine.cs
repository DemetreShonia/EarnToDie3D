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
        Rigidbody _rb;
        [SerializeField] float _breakTorque = 1000f;
        [SerializeField] float _turboTorque = 100f;
        public float CurrentSpeed => _rb.velocity.magnitude * 3.6f;

        float _currentTorque;
        bool _isInitialized = false;

        CarGearBox _gearBox;
        CarInput _carInput;

        Fuel _petrol, _turbo;

        bool _isCarTurboEnabled;
        public void Initialize(CarGearBox gearBox, CarInput carInput, float maxFuel, float maxTurbo)
        {
            _gearBox = gearBox;
            _carInput = carInput;

            _turbo = new Fuel(maxTurbo);
            _petrol = new Fuel(maxFuel);

            _rb = GetComponent<Rigidbody>();
            _isInitialized = true;
        }
        public void Move()
        {
            if (!_isInitialized) return;

            if(_petrol.IsTankEmpty)
            {
                Debug.Log("Fuel Is Out");
                _gearBox.TryBrake(_breakTorque);
                return;
            }
            //float gear = _gearBox.CurrentGear;

            _currentTorque = _rpmTorqueCurve.Evaluate(Mathf.Abs(_carInput.MoveInput)) * _maxTorque; // * GearNum

            float torqueSigned = Mathf.Sign(_carInput.MoveInput) * _currentTorque;
            if (_isCarTurboEnabled)
            {
                torqueSigned += _turboTorque;
                _turbo.ConsumeFuel(CurrentSpeed * Time.deltaTime);
            }
            else
                _petrol.ConsumeFuel(CurrentSpeed * Time.deltaTime);

            _gearBox.TryBrake(_carInput.IsBrakePressed ? _breakTorque : 0);

            _gearBox.TryApplyMotorTorque(torqueSigned);
        }

        public void SetIsCarTurboEnabled(bool enabled) => _isCarTurboEnabled = _turbo.IsTankEmpty ? false : enabled;
    }
}
