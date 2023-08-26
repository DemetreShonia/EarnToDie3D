using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DumbRide
{
    [RequireComponent(typeof(WheelCollider))]
    public class CarWheel : MonoBehaviour
    {
        #region Properties
        public enum WheelType
        {
            None,
            FrontLeft,
            FrontRight,
            RearLeft,
            RearRight
        }

        [SerializeField] WheelType _wheelType;
        public WheelType CarWheelType => _wheelType;

        public Axle WheelAxle { get; private set; } = Axle.None;
        
        public float RPM => _wheelCollider.rpm;
        #endregion

        [SerializeField] GameObject _wheelMesh; // visual
        WheelCollider _wheelCollider;

        void Start()
        {
            if(_wheelType == WheelType.None)
                Debug.LogError("Wheel Type not set!");
            else
                WheelAxle = _wheelType == WheelType.FrontLeft || _wheelType == WheelType.FrontRight ? Axle.Front : Axle.Rear;

            _wheelCollider = GetComponent<WheelCollider>();
        }
        public void ApplyMotorTorque(float torque) => _wheelCollider.motorTorque = torque;
        public void ApplyBrakeTorque(float brakeTorque) => _wheelCollider.brakeTorque = brakeTorque;
        public void ApplySteerAngle(float angle) => _wheelCollider.steerAngle = angle;
        public void AnimateWheel()
        {
            _wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
            _wheelMesh.transform.position = pos;
            _wheelMesh.transform.rotation = rot;
        }
    }
}