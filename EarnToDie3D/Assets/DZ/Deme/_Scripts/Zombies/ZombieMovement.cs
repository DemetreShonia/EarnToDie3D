using Cinemachine.Utility;
using UnityEngine;

namespace DumbRide
{
    public class ZombieMovement : MonoBehaviour
    {
        [SerializeField] Transform _feetPosition;
        [SerializeField] Transform _target;
        [SerializeField] float _movementSpeed = 10f;
        [SerializeField] float _rotationSpeed = 10f;
        Rigidbody _rb;
        Vector3 _moveDir;
        Vector3 MoveDirection => _moveDir.normalized;
        Animator _animator;

        void Start()
        {
            _rb = GetComponent<Rigidbody>();

        }
        void OnEnable()
        {
            _animator = GetComponent<Animator>();
            _animator.SetBool("IsMoving", true);
        }
        void OnDisable()
        {
            _animator.SetBool("IsMoving", false);
        }

        void FixedUpdate()
        {
            UpdateMoveDirection();
            MoveTowardsTarget();
        }
        void Update()
        {
            RotateTowardsTarget();
        }
        void UpdateMoveDirection()
        {
            _moveDir = _target.position - transform.position;
            _moveDir.Normalize();
        }
        void MoveTowardsTarget()
        {
            //var dir = IsOnSlope ? SlopeDirection : MoveDirection;
            Debug.DrawRay(transform.position, _moveDir * 10, Color.red, 0.1f);
            var vel = (MoveDirection * _movementSpeed);
            _rb.velocity = new Vector3(vel.x, _rb.velocity.y, vel.z);
        }
        void RotateTowardsTarget()
        {
            var dir = MoveDirection;
            dir.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
        }
        RaycastHit _slopeHit;
        bool IsOnSlope
        {
            get
            {
                if (Physics.Raycast(_feetPosition.position, Vector3.down, out _slopeHit, 1.5f))
                {
                    float angle = Vector3.Angle(_slopeHit.normal, Vector3.up);
                    return angle < 45f && angle > 0;
                }
                return false;
            }
        }
        Vector3 SlopeDirection => Vector3.ProjectOnPlane(_moveDir, _slopeHit.normal).normalized;

    }
}
