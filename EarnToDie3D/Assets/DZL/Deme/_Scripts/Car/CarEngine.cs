using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public class CarEngine : MonoBehaviour
    {
        [SerializeField] float _maxHorsePower = 500f;
        [SerializeField] AnimationCurve _powerCurve;
        // for speedometer
        public float CurrentSpeed => _rb.velocity.magnitude * 3.6f;
        Rigidbody _rb;
        [SerializeField] float _breakTorque = 1000f;

        float _totalPower;
        bool _isInitialized = false;
        CarWheel[] _wheels;
        CarGearBox _gearBox;
        CarInput _carInput;
        public void Initialize(CarWheel[] wheels, CarGearBox gearBox, CarInput carInput)
        {
            _wheels = wheels;
            _gearBox = gearBox;
            _carInput = carInput;

            _rb = GetComponent<Rigidbody>();
            _isInitialized = true;
        }
        public void Move()
        {
            if (!_isInitialized) return;
            float gear = _gearBox.CurrentGear;

            _totalPower = _powerCurve.Evaluate(Mathf.Abs(_carInput.MoveInput)) * _maxHorsePower * gear; // * GearNum

            float power = Mathf.Sign(_carInput.MoveInput) * _totalPower;

            _gearBox.TryApplyMotorTorque(power);

            _gearBox.TryBrake(_carInput.IsBrakePressed ? _breakTorque : 0);
        }

        // for UI
        //void UpdateNeedle()
        //{
        //    if (!_isInitialized) return;
        //    float _desiredPosition = startPos - endPos;
        //    float temp = _engineRPM / 10000;
        //    _neetle.transform.eulerAngles = new Vector3(0,0, startPos - temp * desiredPos)
        //}
    }
}
