using UnityEngine;
using System;
using System.Collections.Generic;

namespace DumbRide
{
    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;
    }

    public class CarController : MonoBehaviour
    {
        [Header("Acceleration Properties")]
        [SerializeField] float _maxAcceleration = 30.0f;
        [SerializeField] float _brakeAcceleration = 50.0f;

        [Header("Steering Properties")]
        [SerializeField] float _turnSensitivity = 1.0f;
        [SerializeField] float _steerLerpSpeed = 200.0f;
        [SerializeField] float _maxSteerAngle = 30.0f;

        [Header("Car Properties")]
        [SerializeField]
        [Tooltip("Percent of velocity")]
        [Range(0.1f, 0.5f)] float _downPressurePercent = 0.2f; 
        [SerializeField] Vector3 _centerOfMass;
        [SerializeField] List<Wheel> _wheels;


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

        void ApplyForces()
        {
            // make down pressure 20% of the velocity
            _downPressure = Mathf.Lerp(_downPressure, _carRb.velocity.magnitude * _downPressurePercent, Time.fixedDeltaTime);
            _carRb.AddForce(-transform.up * _downPressure, ForceMode.Force);
            foreach (var wheel in _wheels)
            {
                wheel.wheelCollider.motorTorque = _moveInput * _maxAcceleration;
            }
        }

        void SteerFrontWheels()
        {
            foreach (var wheel in _wheels)
            {
                if (wheel.axel == Axel.Front)
                {
                    var _steerAngle = _steerInput * _turnSensitivity * _maxSteerAngle;
                    wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, Time.deltaTime * _steerLerpSpeed);
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