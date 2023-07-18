using UnityEngine;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DumbRide
{
    public enum Axle
    {
        Front,
        Rear
    }
    public enum DriveType
    {
        FrontWheelDrive,
        RearWheelDrive,
        AllWheelDrive
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axle axel;
    }

    public class CarController : MonoBehaviour
    {
        [Header("Car Properties")]
        [SerializeField] DriveType _driveType = DriveType.RearWheelDrive;
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
            ApplyForces();
            SteerFrontWheels();
            TryBrake();
        }
        void CheckInputs()
        {
            _moveInput = Input.GetAxis(VERTICAL_AXIS);
            _steerInput = Input.GetAxis(HORIZONTAL_AXIS);
        }


        // MOVE THIS TO GearBox Script
        void ApplyForces()
        {
            // make down pressure 20% of the velocity
            //_downPressure = Mathf.Lerp(_downPressure, _carRb.velocity.magnitude * _downPressurePercent, Time.fixedDeltaTime);
            //_carRb.AddForce(-transform.up * _downPressure, ForceMode.Force);
            float _currentTorque = _maxAcceleration * _moveInput;

            void ApplyBothAxles()
            {
                _currentTorque /= 4; // distribute to 4 wheels

                foreach (var wheel in _wheels)
                {
                    wheel.wheelCollider.motorTorque = _currentTorque;
                }
            }

            void ApplySingleAxle(Axle axel)
            {
                _currentTorque /= 2; // distribute to 2 wheels

                foreach (var wheel in _wheels)
                {
                    if(wheel.axel == axel)
                        wheel.wheelCollider.motorTorque = _currentTorque;
                }
            }
            switch (_driveType)
            {
                case DriveType.AllWheelDrive:
                    ApplyBothAxles();
                    break;
                case DriveType.FrontWheelDrive:
                    ApplySingleAxle(Axle.Front);
                    break;
                case DriveType.RearWheelDrive:
                    ApplySingleAxle(Axle.Rear);
                    break;
            }
        }
        // steer properties
        [SerializeField] float _wheelBase = 4f;
        [SerializeField] float _rearTrack = 2.5f;
        [SerializeField] float _turnRadius = 5.0f;
        void SteerFrontWheels()
        {
            // ackerman steer formula
            // Wheelbase = distance between front and rear axles
            // Reartrack = distance between rear wheels
            // TurnRadius = radius of the turn
            //var steerAngle = Mathf.Rad2Deg * Mathf.Atan(_wheelBase / (_turnRadius + (_rearTrack / 2))) * _steerInput;
            
            float ackermanLeft = 0f;
            float ackermanRight = 0f;

            if(_steerInput > 0f)
            {
                ackermanLeft = Mathf.Rad2Deg * Mathf.Atan(_wheelBase / (_turnRadius + (_rearTrack / 2))) * _steerInput;
                ackermanRight = Mathf.Rad2Deg * Mathf.Atan(_wheelBase / (_turnRadius - (_rearTrack / 2))) * _steerInput;
            }
            else if (_steerInput < 0f)
            {
                ackermanLeft = Mathf.Rad2Deg * Mathf.Atan(_wheelBase / (_turnRadius - (_rearTrack / 2))) * _steerInput;
                ackermanRight = Mathf.Rad2Deg * Mathf.Atan(_wheelBase / (_turnRadius + (_rearTrack / 2))) * _steerInput;
            }

            // refactor this!
            int id = 0;
            foreach (var wheel in _wheels)
            {
                if(id == 2)
                    break;
                if (wheel.axel == Axle.Front)
                {
                    if (id == 0)
                        wheel.wheelCollider.steerAngle = ackermanLeft;
                    else if (id == 1)
                        wheel.wheelCollider.steerAngle = ackermanRight;
                    id++;
                }
            }
        }

        void TryBrake()
        {
            foreach (var wheel in _wheels)
                wheel.wheelCollider.brakeTorque = (Input.GetKey(KeyCode.Space) || _moveInput == 0f) ? _brakeAcceleration : 0;
        }

        void AnimateWheels()
        {
            foreach (var wheel in _wheels)
            {
                Quaternion rot;
                Vector3 pos;
                wheel.wheelCollider.GetWorldPose(out pos, out rot);
                wheel.wheelModel.transform.position = pos;
                wheel.wheelModel.transform.rotation = rot;
            }
        }
    }

}