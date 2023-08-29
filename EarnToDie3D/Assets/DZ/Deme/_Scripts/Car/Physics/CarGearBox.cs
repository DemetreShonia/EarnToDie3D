using UnityEngine;

namespace DumbRide
{
    public enum Axle
    {
        None,
        Front,
        Rear
    }
    public class CarGearBox : MonoBehaviour
    {
        // TODO: Automatic Transmission
        public enum DriveType
        {
            FrontWheelDrive,
            RearWheelDrive,
            AllWheelDrive
        }

        [SerializeField] DriveType _driveType = DriveType.AllWheelDrive;
        [SerializeField] float[] _gears;
        CarWheel[] _wheels;
        bool _isInitialized = false;

        int _currentGearID = 0;

        public float CurrentGear => _currentGearID >= 0 && _currentGearID < _gears.Length ? _gears[_currentGearID] : 0f;
        public void NextGear()
        {
            _currentGearID++;
            _currentGearID %= _gears.Length;
        }
        public void PrevGear()
        {
            _currentGearID--;
            _currentGearID = _currentGearID < 0 ? _gears.Length - 1 : _currentGearID;
        }
        public void Initialize(CarWheel[] wheels)
        {
            _wheels = wheels;
            _isInitialized = true;
        }
        public void TryBrake(float curTorque)
        {
            if (!_isInitialized) return;

            int wheelCount = _wheels.Length;
            float currentTorque = curTorque / wheelCount; // distribute torque to wheels

            foreach (var wheel in _wheels)
            {
                wheel.ApplyBrakeTorque(currentTorque);
            }
        }
        public void TryApplyMotorTorque(float torque)
        {
            if (!_isInitialized) return;

            float maxTorq = torque;
            int wheelCount = _wheels.Length;

            // distribute torque to wheels, case 4 or case 2
            float currentTorque = _driveType == DriveType.AllWheelDrive ? maxTorq / wheelCount : maxTorq * 0.5f;
            foreach (var wheel in _wheels)
            {
                switch (_driveType)
                {
                    case DriveType.AllWheelDrive:
                        wheel.ApplyMotorTorque(currentTorque);
                        break;
                    case DriveType.FrontWheelDrive:
                        if (wheel.WheelAxle == Axle.Front)
                            wheel.ApplyMotorTorque(currentTorque);
                        break;
                    case DriveType.RearWheelDrive:
                        if (wheel.WheelAxle == Axle.Rear)
                            wheel.ApplyMotorTorque(currentTorque);
                        break;
                }
            }
        }
    }
}
