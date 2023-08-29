using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public class CarEngine : MonoBehaviour
    {
        [SerializeField] AnimationCurve _rpmTorqueCurve;
        // for speedometer
        Rigidbody _rb;
        
        float _breakTorque;
        float _maxTorque;
        float _turboTorque;

        public float CurrentSpeed => _rb.velocity.magnitude * 3.6f;

        float _currentTorque;
        bool _isInitialized = false;

        CarGearBox _gearBox;
        CarInput _carInput;

        Fuel _petrol, _turbo;

        bool _isCarTurboEnabled;
        public void Initialize(CarGearBox gearBox, CarInput carInput, float maxFuel, float maxTorque)
        {
            _maxTorque = maxTorque;
            _gearBox = gearBox;
            _carInput = carInput;
            _breakTorque = 2 * _maxTorque;

            _petrol = new Fuel(maxFuel);

            _rb = GetComponent<Rigidbody>();
            _isInitialized = true;
        }
        public void SetTurbo(float turboFuel, float turboTorque)
        {
            _turbo = new Fuel(turboFuel);
            _turboTorque = turboTorque;
        }

        public void Move()
        {
            if (!_isInitialized) return;

            if (_petrol.IsTankEmpty)
            {
                Debug.Log("Fuel Is Out");
                _gearBox.TryBrake(_breakTorque);
                return;
            }
            //float gear = _gearBox.CurrentGear;

            _currentTorque = _rpmTorqueCurve.Evaluate(Mathf.Abs(_carInput.MoveInput)) * _maxTorque; // * GearNum

            float torqueSigned = Mathf.Sign(_carInput.MoveInput) * _currentTorque;
            print(_isCarTurboEnabled);
            if (_isCarTurboEnabled)
            {
                if(_turbo.IsTankEmpty)
                {
                    Debug.Log("Turbo Is Out");
                }
                Debug.Log("CONSUMING TURBO");
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
