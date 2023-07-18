using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DumbRide
{
    public enum Axle
    {
        None,
        Front,
        Rear
    }
    public class GearBox : MonoBehaviour
    {
        public enum DriveType
        {
            FrontWheelDrive,
            RearWheelDrive,
            AllWheelDrive
        }

        [SerializeField] DriveType _driveType = DriveType.AllWheelDrive;
        [SerializeField] Wheel[] _wheels;

        public void ApplyMotorTorque(float _maxAcceleration, float _moveInput)
        {
            float maxTorq = _maxAcceleration * _moveInput;
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
