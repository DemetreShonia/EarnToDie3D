using UnityEngine;
using System;

namespace DumbRide
{
    [RequireComponent(typeof(Rigidbody), typeof(CarGearBox), typeof(CarInput))]
    public class CarController : MonoBehaviour
    {
        #region Fields
        
        
        [Header("Car Properties")]
        [SerializeField] Transform _centerOfMass;
        [SerializeField] CarWheel[] _wheels;

        [Header("Acceleration Properties")]
        [SerializeField] float _maxAcceleration = 30.0f;
        [SerializeField] float _brakeAcceleration = 50.0f;

        [Header("Steering Properties")]
        [SerializeField] float _wheelBase = 4f;
        [SerializeField] float _rearTrack = 2.5f;
        [SerializeField] float _turnRadius = 5.0f;
        
        Rigidbody _carRb;
        CarGearBox _gearBox;
        CarInput _carInput;
        #endregion

        void Start()
        {
            _carRb = GetComponent<Rigidbody>();
            _gearBox = GetComponent<CarGearBox>();
            _carInput = GetComponent<CarInput>();

            _carRb.centerOfMass = _centerOfMass.localPosition;
            _gearBox.Initialize(_wheels);
        }

        void Update()
        {
            _carInput.UpdateInputs();
            AnimateWheels();
        }
        void FixedUpdate()
        {
            SteerAckerman();
            UpdateTorques();
        }
        
        void SteerAckerman()
        {
            float ackermanLeft = 0f;
            float ackermanRight = 0f;
            float steerInput = _carInput.SteerInput;

            if (steerInput > 0f)
            {
                ackermanLeft = Mathf.Rad2Deg * Mathf.Atan(_wheelBase / (_turnRadius + (_rearTrack / 2))) * steerInput;
                ackermanRight = Mathf.Rad2Deg * Mathf.Atan(_wheelBase / (_turnRadius - (_rearTrack / 2))) * steerInput;
            }
            else if (steerInput < 0f)
            {
                ackermanLeft = Mathf.Rad2Deg * Mathf.Atan(_wheelBase / (_turnRadius - (_rearTrack / 2))) * steerInput;
                ackermanRight = Mathf.Rad2Deg * Mathf.Atan(_wheelBase / (_turnRadius + (_rearTrack / 2))) * steerInput;
            }

            foreach (var wheel in _wheels)
            {
                switch (wheel.CarWheelType)
                {
                    case CarWheel.WheelType.FrontLeft:
                        wheel.ApplySteerAngle(ackermanLeft);
                        break;
                    case CarWheel.WheelType.FrontRight:
                        wheel.ApplySteerAngle(ackermanRight);
                        break;
                }
            }
        }

        void UpdateTorques()
        {
            _gearBox.TryApplyMotorTorque(_maxAcceleration, _carInput.MoveInput);
            _gearBox.TryBrake(_carInput.IsBrakePressed ? _brakeAcceleration : 0);
        }
        void AnimateWheels()
        {
            foreach (var wheel in _wheels)
                wheel.AnimateWheel();
        }
    }

}