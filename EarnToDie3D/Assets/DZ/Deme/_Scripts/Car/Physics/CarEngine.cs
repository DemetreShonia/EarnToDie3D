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

        const float _maxCapturedSpeed = 80f; // TODO: Modify this
        public float CurrentSpeed => _rb.velocity.magnitude * 3.6f;

        float _currentTorque;
        bool _isInitialized = false;

        CarGearBox _gearBox;
        CarInput _carInput;

        Fuel _petrol, _turbo;

        bool _isCarTurboEnabled;
        CarUIManager _carUiManager;

        public void Initialize(CarGearBox gearBox, CarInput carInput, Fuel petrol, float maxTorque)
        {
            _maxTorque = maxTorque;
            _gearBox = gearBox;
            _carInput = carInput;
            _breakTorque = 2 * _maxTorque;

            _petrol = petrol;

            _rb = GetComponent<Rigidbody>();
            _isInitialized = true;

            _carUiManager = CarUIManager.Instance;
        }
        public void SetTurbo(Fuel turbo, float turboTorque)
        {
            _turbo = turbo;
            _turboTorque = turboTorque;
        }
        public void Move()
        {
            if (!_isInitialized) return;

            if(_carUiManager != null)
            {
                _carUiManager.UpdateFuelMeter(_petrol.FuelLeftPercent);
                _carUiManager.UpdateSpeedoMeter(CurrentSpeed / _maxCapturedSpeed);
                if(_turbo != null)
                    _carUiManager.UpdateTurboMeter(_turbo.FuelLeftPercent);
            }

            if (_petrol.IsTankEmpty)
            {
                Debug.Log("Fuel Is Out");
                _gearBox.TryBrake(_breakTorque);
                return;
            }
            //float gear = _gearBox.CurrentGear;

            // Instead of calculating RPM from wheels (Which is a bit strange but can be done easily),
            // I use Input To simulate RPM.
            // This way we are controlling wheels and not vice versa

            var _rpmPercent = Mathf.Abs(_carInput.MoveInput);
            _currentTorque = _rpmTorqueCurve.Evaluate(_rpmPercent) * _maxTorque * _gearBox.CurrentGearRatio; // * GearNum

            float torqueSigned = Mathf.Sign(_carInput.MoveInput) * _currentTorque;
            if (_isCarTurboEnabled)
            {
                if(_turbo.IsTankEmpty)
                {
                    Debug.Log("Turbo Is Out");
                }
                    
                torqueSigned += _turboTorque;
                _turbo.ConsumeFuel(CurrentSpeed * Time.deltaTime);
            }
            else
                _petrol.ConsumeFuel(CurrentSpeed * Time.deltaTime);

            _gearBox.TryBrake(_carInput.IsBrakePressed ? _breakTorque : 0);

            _gearBox.TryApplyMotorTorque(torqueSigned);

            _gearBox.ChangeWheelSmokeVariables(CurrentSpeed / _maxCapturedSpeed);
        }

        public void SetIsCarTurboEnabled(bool enabled) => _isCarTurboEnabled = _turbo.IsTankEmpty ? false : enabled;
    }
}
