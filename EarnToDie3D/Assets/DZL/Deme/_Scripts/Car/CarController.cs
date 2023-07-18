using UnityEngine;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DumbRide
{

    public class CarController : MonoBehaviour
    {
        [SerializeField] GearBox _gearBox; 
        [Header("Car Properties")]
        [SerializeField]
        [Tooltip("Percent of velocity")]
        [Range(0.1f, 0.5f)] float _downPressurePercent = 0.2f;
        [SerializeField] Vector3 _centerOfMass;
        [SerializeField] List<Wheel> _wheels;


        [Header("Acceleration Properties")]
        [SerializeField] float _maxAcceleration = 30.0f;
        [SerializeField] float _brakeAcceleration = 50.0f;

        [Header("Steering Properties")]
        [SerializeField] float _turnSensitivity = 1.0f;
        [SerializeField] float _steerLerpSpeed = 200.0f;
        [SerializeField] float _maxSteerAngle = 30.0f;

        const string VERTICAL_AXIS = "Vertical";
        const string HORIZONTAL_AXIS = "Horizontal";

        float _moveInput;
        float _steerInput;
        float _downPressure; // not to turn upside down car when reaching high velocities
        Rigidbody _carRb;


        void Start()
        {
            _carRb = GetComponent<Rigidbody>();
            _carRb.centerOfMass = _centerOfMass;
        }

        void Update()
        {
            CheckInputs();
            AnimateWheels();
        }
        void FixedUpdate()
        {
            SteerAckerman();
            TryBrake();
            _gearBox.ApplyMotorTorque(_maxAcceleration, _moveInput);
        }
        void CheckInputs()
        {
            _moveInput = Input.GetAxis(VERTICAL_AXIS);
            _steerInput = Input.GetAxis(HORIZONTAL_AXIS);
        }


        // steer properties
        [SerializeField] float _wheelBase = 4f;
        [SerializeField] float _rearTrack = 2.5f;
        [SerializeField] float _turnRadius = 5.0f;
        void SteerAckerman()
        {
            float ackermanLeft = 0f;
            float ackermanRight = 0f;

            if (_steerInput > 0f)
            {
                ackermanLeft = Mathf.Rad2Deg * Mathf.Atan(_wheelBase / (_turnRadius + (_rearTrack / 2))) * _steerInput;
                ackermanRight = Mathf.Rad2Deg * Mathf.Atan(_wheelBase / (_turnRadius - (_rearTrack / 2))) * _steerInput;
            }
            else if (_steerInput < 0f)
            {
                ackermanLeft = Mathf.Rad2Deg * Mathf.Atan(_wheelBase / (_turnRadius - (_rearTrack / 2))) * _steerInput;
                ackermanRight = Mathf.Rad2Deg * Mathf.Atan(_wheelBase / (_turnRadius + (_rearTrack / 2))) * _steerInput;
            }

            foreach (var wheel in _wheels)
            {
                switch (wheel.CarWheelType)
                {
                    case Wheel.WheelType.FrontLeft:
                        wheel.ApplySteerAngle(ackermanLeft);
                        break;
                    case Wheel.WheelType.FrontRight:
                        wheel.ApplySteerAngle(ackermanRight);
                        break;
                }
            }
        }

        void TryBrake()
        {
            foreach (var wheel in _wheels)
                wheel.ApplyBrakeTorque((Input.GetKey(KeyCode.Space) || _moveInput == 0f) ? _brakeAcceleration : 0);
        }

        void AnimateWheels()
        {
            foreach (var wheel in _wheels)
            {
                wheel.AnimateWheel();
            }
        }
    }

}