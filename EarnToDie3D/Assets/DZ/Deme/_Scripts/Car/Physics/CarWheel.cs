using UnityEngine;
using UnityEngine.VFX;
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
        [SerializeField] float _wheelVisualOffset;

        [SerializeField] VisualEffect _wheelSmoke;
        [SerializeField] AnimationCurve _rateAnimationCurve;
        [SerializeField] float _rateMultiplayer = 10;
        [SerializeField] string _sandTag;
        WheelCollider _wheelCollider;
        bool _wheelTouchesSand;
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
        
        public void InitializeWheel(int mass) => _wheelCollider.mass = mass; // this can be  wheel info in future, used for level system
        public void AnimateWheel()
        {
            _wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
            _wheelMesh.transform.position = pos + _wheelMesh.transform.right * _wheelVisualOffset;
            _wheelMesh.transform.rotation = rot;
        }
        
        public bool IsGrounded => _wheelCollider.isGrounded;
        public bool IsLeft => _wheelType == WheelType.FrontLeft || _wheelType == WheelType.RearLeft;

        void Update()
        {
            WheelHit hit;

            if (_wheelCollider.GetGroundHit(out hit))
            {
                if (hit.collider.CompareTag(_sandTag))
                {
                    if (!_wheelTouchesSand)
                    {
                        _wheelSmoke.Play();
                    }
                    _wheelTouchesSand = true;
                }
                else
                {
                    if (_wheelTouchesSand)
                    {
                        _wheelSmoke.Stop();
                    }
                    _wheelTouchesSand = false;
                }
            }
            else
            {
                if (_wheelTouchesSand)
                {
                    _wheelSmoke.Stop();
                }
                _wheelTouchesSand = false;
            }
        }

        public void UpdateSmokeVariables(float speed)
        {
            _wheelSmoke.SetFloat("rate", _rateAnimationCurve.Evaluate(speed) * _rateMultiplayer);
        }
    }
}
