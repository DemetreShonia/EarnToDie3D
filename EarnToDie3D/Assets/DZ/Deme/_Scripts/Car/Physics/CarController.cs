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
        Decorator[] _decorators; // Blade,Gun, Turbo order is necessary


        [Header("Steering Properties")]
        [SerializeField] float _wheelBase = 4f;
        [SerializeField] float _rearTrack = 2.5f;
        [SerializeField] float _turnRadius = 5.0f;
        
        Rigidbody _carRb;
        CarGearBox _gearBox;
        CarInput _carInput;
        CarEngine _carEngine;

        InGameCarData _dataFromGarage;
        #endregion


        void LoadInGameCarDataFromGarage()
        {
            // Love this function <3
            if(SceneSwitchManager.Instance == null)
            {
                _dataFromGarage = DefaultData.MyIngameCarData;
                Debug.LogWarning("Using Default Data, Garage Data Passer was not found!");
            }
            else
            {
                _dataFromGarage = SceneSwitchManager.Instance.InGameCarData;
            }
        }
        void Start()
        {
            _carRb = GetComponent<Rigidbody>();
            _gearBox = GetComponent<CarGearBox>();
            _carInput = GetComponent<CarInput>();
            _carEngine = GetComponent<CarEngine>();

            _carRb.centerOfMass = _centerOfMass.localPosition;
            _gearBox.Initialize(_wheels);

            LoadInGameCarDataFromGarage();
            float fuelLiter = _dataFromGarage.fuelLiter;
            float turboLiter = 1000; // _dataFromGarage.GetDecorator(DecoratorType.Turbo).quantity; // by default 0...


            _carEngine.Initialize(_gearBox, _carInput, fuelLiter, turboLiter);

            if(_dataFromGarage.decoratorDatas != null)
            {
                // if we have not purchased anything, what should we enable? Nothing!
                for (int i = 0; i < _decorators.Length; i++)
                {
                    var dec = _decorators[i];
                    var decData = _dataFromGarage.GetDecorator(i);

                    dec.Initialize(decData);
                    if (dec.Type == DecoratorType.Turbo)
                        (dec as DecoratorTurbo).SetCarEngine(_carEngine);
                }
            }
            else
            {
                // disable all decorators on car
                for (int i = 0; i < _decorators.Length; i++)
                    _decorators[i].SetActive(false);
            }
        }

        void Update()
        {
            _carInput.UpdateInputs();

            foreach (var decorator in _decorators)
                if(decorator.IsUnlocked)
                    decorator.CheckForInputs();

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