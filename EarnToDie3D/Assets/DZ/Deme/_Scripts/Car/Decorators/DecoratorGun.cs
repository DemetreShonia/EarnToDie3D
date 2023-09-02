using UnityEngine;
using MoreMountains.Feedbacks;
namespace DumbRide
{
    public class DecoratorGun : Decorator
    {
        [SerializeField] GameObject _bloodParticle;
        [SerializeField] Transform _gunHead;
        [SerializeField] Transform _shootPosition;
        [SerializeField] float _rotationSpeed = 7f;
        [SerializeField] Animator _animator;
        [SerializeField] float _shootCooldown = 0.5f;
        [SerializeField] MMFeedbacks _shootStartFeedback;
        [SerializeField] GameObject _bullet;
        [SerializeField] float _sphereCastRadius = 1;
        [SerializeField] LayerMask _enemyLayer;
        [SerializeField] float _aimGroundYOffset = 1;
        [SerializeField] LayerMask _cameraRayCanHit;
        Camera _mainCamera;

        float _nextShoot;
        public override void Initialize(DecoratorData data)
        {
            base.Initialize(data);
            _mainCamera = Camera.main;
        }

        public override void Animate()
        {
            _shootStartFeedback?.PlayFeedbacks();
            _animator.SetTrigger(AnimationStrings.FIRE);
        }

        public override void PlaySound()
        {

        }

        void Shoot()
        {
            Animate();

            GameObject bullet = Instantiate(_bullet);
            bullet.transform.position = _shootPosition.position;
            bullet.transform.LookAt(_shootPosition.position + _shootPosition.forward);

            //if (Physics.Raycast(_shootPosition.position, _shootPosition.forward, out RaycastHit hit))
            //{
            //    if (hit.collider.TryGetComponent(out IBodyPart bodyPart))
            //    {
            //        //target.TakeDamage((int)_data.power);
            //        bodyPart.ApplyHit(-hit.normal, 300, 500);

            //        var blood = Instantiate(_bloodParticle, hit.point, Quaternion.LookRotation(hit.normal));
            //        Destroy(blood, 1f);
            //    }
            //}
            // playsound, animate on keyframe anim event
        }
        
        public override void CheckForInputs()
        {
            Quaternion targetRotation = _gunHead.rotation;
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            


            if (Physics.Raycast(ray, out hit, Mathf.Infinity,_cameraRayCanHit))
            {
                Vector3 direction = hit.point + Vector3.up * _aimGroundYOffset - _gunHead.position;
              
                //direction.y = 0f;
                if (Physics.SphereCast(_gunHead.position, _sphereCastRadius, direction.normalized, out hit, Mathf.Infinity, _enemyLayer))
                {
                    targetRotation = Quaternion.LookRotation((hit.point - _gunHead.position).normalized);
                    _gunHead.rotation = targetRotation;
                }
                else
                {
                    targetRotation = Quaternion.LookRotation(direction.normalized);
                }
            }
            float blend = 1 - Mathf.Pow(0.5f, Time.deltaTime * _rotationSpeed);
            _gunHead.rotation = Quaternion.Lerp(_gunHead.rotation, targetRotation, blend);

            if (_nextShoot < Time.timeSinceLevelLoad + _shootCooldown)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Shoot();
                    _nextShoot = Time.timeSinceLevelLoad + _shootCooldown;
                }
            }

        }
    }
}
