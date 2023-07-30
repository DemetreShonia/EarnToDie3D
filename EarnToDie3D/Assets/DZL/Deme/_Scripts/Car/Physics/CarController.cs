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

        [Header("Decorators")]
        [SerializeField]
        Decorator[] _decorators;

        [Header("Steering Properties")]
        [SerializeField] float _wheelBase = 4f;
        [SerializeField] float _rearTrack = 2.5f;
        [SerializeField] float _turnRadius = 5.0f;
        
        Rigidbody _carRb;
        CarGearBox _gearBox;
        CarInput _carInput;
        CarEngine _carEngine;
        #endregion

        void Start()
        {
            _carRb = GetComponent<Rigidbody>();
            _gearBox = GetComponent<CarGearBox>();
            _carInput = GetComponent<CarInput>();
            _carEngine = GetComponent<CarEngine>();

            _carRb.centerOfMass = _centerOfMass.localPosition;
            _gearBox.Initialize(_wheels);
            _carEngine.Initialize(_gearBox, _carInput);

            CurrentCarData _selectedCarData = DefaultData.MyCurrentCarData;
            _selectedCarData.turboData.isUnlocked = true;
            foreach (var decorator in _decorators)
            {
                decorator.Initialize(_selectedCarData);
            }
        }

        void Update()
        {
            _carInput.UpdateInputs();
            AnimateWheels();
        }
        void FixedUpdate()
        {
            _carEngine.Move();
            SteerAckerman();
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

        void AnimateWheels()
        {
            foreach (var wheel in _wheels)
                wheel.AnimateWheel();

        }
    }

}