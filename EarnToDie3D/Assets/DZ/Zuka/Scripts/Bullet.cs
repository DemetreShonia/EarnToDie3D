using UnityEngine;
using MoreMountains.Feedbacks;
namespace DumbRide
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] float _speed;
        [SerializeField] LayerMask _canHit;
        [SerializeField] float _raySphereRadius = .3f;
        
        Vector3 _lastPosition;
        bool _targetWasHit = false;

        Vector3 _dir;
        Transform _transform;

        bool _isInitialized = false;

        int _bulletDamage;

        ZombieHitParticlesController _zombieHitParticlesController;
        GroundHitParticlesController _hitParticlesController;
        public void Initialize(int damage, GroundHitParticlesController hitControllerScript, ZombieHitParticlesController zombieHitParticleControllerScript)
        {
            _isInitialized = true;
            _transform = transform;
            _dir = _transform.forward;
            _bulletDamage = damage;
            _hitParticlesController = hitControllerScript;
            _zombieHitParticlesController = zombieHitParticleControllerScript;
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


                Vector3 towards = transform.position - _lastPosition;
                towards.y = 0;
                towards.Normalize();
                if (hit.collider.TryGetComponent(out IBodyPart bodyPart))
                {
                    bodyPart.ApplyHit(-hit.normal, _bulletDamage * 10, _bulletDamage); // add 10x force and damage
                    _zombieHitParticlesController.PlayEffect(hit.point, hit.normal, towards);
                }
                else
                {
                    _hitParticlesController.PlayEffect(hit.point, hit.normal, towards);
                }
            }
        }
    }
}
