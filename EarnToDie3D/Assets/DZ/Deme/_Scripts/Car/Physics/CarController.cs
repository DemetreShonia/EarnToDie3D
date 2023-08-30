using UnityEngine;
using System;
using System.Drawing;

namespace DumbRide
{
    [RequireComponent(typeof(Rigidbody), typeof(CarGearBox), typeof(CarInput))]
    public class CarController : MonoBehaviour
    {
        #region Fields

        [Header("Car Properties")]
        [SerializeField] GarageDataSO _carData;
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

        void Start()
        {
            SetInitialReferences();
            LoadInGameCarDataFromGarage();
            _gearBox.Initialize(_wheels);

            var maxFuel = _carData.GetLevelData(PartEnum.Engine).GetMaxStat(); // fuel id

            _carEngine.Initialize(_gearBox, _carInput, new Fuel(_dataFromGarage.fuelLiter, maxFuel), _dataFromGarage.engineTorque);
            InitializeDecorators();
            InitializeWheels();
        }

        void InitializeWheels()
        {
            foreach(var wheel in _wheels)
                wheel.InitializeWheel(_dataFromGarage.wheelMass);
        }

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
        void SetInitialReferences()
        {
            _carRb = GetComponent<Rigidbody>();
            _gearBox = GetComponent<CarGearBox>();
            _carInput = GetComponent<CarInput>();
            _carEngine = GetComponent<CarEngine>();
            _carRb.centerOfMass = _centerOfMass.localPosition;
        }
        void InitializeDecorators()
        {
            if (_dataFromGarage.decoratorDatas != null)
            {
                // if we have not purchased anything, what should we enable? Nothing!
                for (int i = 0; i < _decorators.Length; i++)
                {
                    var dec = _decorators[i];
                    var decData = _dataFromGarage.GetDecorator(i);

                    dec.Initialize(decData);
                    if (dec.Type == DecoratorType.Turbo)
                    {
                        var maxTurbo = _carData.GetLevelData(PartEnum.Turbo).GetMaxStat(); // turbo id

                        (dec as DecoratorTurbo).SetCarEngine(_carEngine);
                        _carEngine.SetTurbo(new Fuel(decData.quantity, maxTurbo), decData.power);
                    }
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

        void OnCollisionEnter(Collision collision)
        {
            ContactPoint firstContact = collision.GetContact(0);
            if (firstContact.thisCollider.gameObject.TryGetComponent(out CarHitter carHitter))
                carHitter.Hit(firstContact.otherCollider.transform);
        }
        void OnTriggerEnter(Collider other)
        {
            var go = other.gameObject;
            if (go.CompareTag(TagStrings.ZOMBIE_PART))
            {
                if (go.transform.root.TryGetComponent(out ZombieController zombie))
                    zombie.EnableRagdoll();
            }
        }
    }

}