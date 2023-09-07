using UnityEngine;
using MoreMountains.Feedbacks;
namespace DumbRide
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] float _speed;
        [SerializeField] LayerMask _canHit;
        [SerializeField] float _raySphereRadius = .3f;
        [SerializeField] MMFeedbacks _hitFeedback;
        Vector3 _lastPosition;
        bool _targetWasHit = false;

        Vector3 _dir;
        Transform _transform;

        bool _isInitialized = false;

        int _bulletDamage;

        public void Initialize(int damage)
        {
            _isInitialized = true;
            _transform = transform;
            _dir = _transform.forward;
            _bulletDamage = damage;
        }
        
        void Update()
        {
            if (_isInitialized && !_targetWasHit)
            {
                _lastPosition = _transform.position;
                _transform.position += _dir * _speed * Time.deltaTime;
            }
        }

        void LateUpdate()
        {
            if (!_isInitialized || _targetWasHit) return;

            RaycastHit hit;
            if(Physics.SphereCast(_lastPosition, _raySphereRadius, transform.forward, out hit, (transform.position - _lastPosition).magnitude, _canHit))
            {
                transform.position = hit.point;
                _targetWasHit = true;
                _hitFeedback?.PlayFeedbacks();

                if (hit.collider.TryGetComponent(out IBodyPart bodyPart))
                {
                    bodyPart.ApplyHit(-hit.normal, _bulletDamage * 10, _bulletDamage); // add 10x force and damage
                }
            }
        }
    }
}
