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

            InGameCarData _selectedCarData = DefaultData.MyCurrentCarData;
            float fuelLiter = _selectedCarData.fuelLiter;
            float turboLiter = _selectedCarData.turboData.power; // by default 0...

            fuelLiter = 10000;
            turboLiter = 100f;

            _carEngine.Initialize(_gearBox, _carInput, fuelLiter, turboLiter);

            _selectedCarData.turboData.isUnlocked = true;
            _selectedCarData.gunData.isUnlocked = true;
            foreach (var decorator in _decorators)
            {
                switch (decorator.Type)
                {
                    case DecoratorType.Blade:
                        decorator.Initialize(_selectedCarData.bladeData);
                        break;
                    case DecoratorType.Gun:
                        decorator.Initialize(_selectedCarData.gunData);
                        break;
                    case DecoratorType.Turbo:
                        decorator.Initialize(_selectedCarData.turboData);
                        (decorator as DecoratorTurbo).SetCarEngine(_carEngine);
                        break;
                }
            }
        }

        void Update()
        {
            _carInput.UpdateInputs();

            foreach (var decorator in _decorators)
            {
                decorator.CheckForInputs();
            }

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