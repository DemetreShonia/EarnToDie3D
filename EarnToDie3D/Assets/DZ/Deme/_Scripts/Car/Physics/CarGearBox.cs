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

        public float CurrentGearRatio { get; private set; }
        
        public void Initialize(CarWheel[] wheels, float gearRatio)
        {
            _wheels = wheels;
            _isInitialized = true;
            CurrentGearRatio = gearRatio / 10; // division is necessary to get fraction
            //print(CurrentGearRatio);
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
        public void ChangeWheelSmokeVariables(float speed)
        {
            foreach (var wheel in _wheels)
            {
                wheel.UpdateSmokeVariables(speed);
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
