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
        [SerializeField] Bullet _bullet;
        [SerializeField] float _sphereCastRadius = 1;
        [SerializeField] LayerMask _enemyLayer;
        [SerializeField] float _aimGroundYOffset = 1;
        [SerializeField] LayerMask _cameraRayCanHit;
        [SerializeField] float _minGunAngleWhenNearShooting = 20;
        [SerializeField] float _crossHairSphereRadius = 2f;


        Camera _mainCamera;

        float _nextShoot;

        int _currentAmmoCount;
        public override void Initialize(DecoratorData data)
        {
            base.Initialize(data);
            _mainCamera = Camera.main;
            _currentAmmoCount = _data.quantity;
            UpdateUI();
        }

        public override void Animate()
        {
            _shootStartFeedback?.PlayFeedbacks();
            _animator.SetTrigger(AnimationStrings.FIRE);
        }

        public override void PlaySound()
        {

        }
        void UpdateUI()
        {
            if(CarUIManager.Instance != null)
                CarUIManager.Instance.UpdateAmmoCount(_currentAmmoCount);
        }
        void Shoot()
        {
            Animate();

            var bullet = Instantiate(_bullet);
            bullet.transform.position = _shootPosition.position;
            bullet.transform.LookAt(_shootPosition.position + _shootPosition.forward);
            bullet.Initialize(_data.power);
            _currentAmmoCount--;
            UpdateUI();
        }


        public override void CheckForInputs()
        {
            Quaternion targetRotation = _gunHead.rotation;
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            var rotSpeed = _rotationSpeed;

            if (Physics.SphereCast(ray, _crossHairSphereRadius, out hit, Mathf.Infinity, _cameraRayCanHit))
            {
                bool cursorIsNearZombie = hit.transform.CompareTag(TagStrings.ZOMBIE_PART);

                Vector3 direction = hit.point + Vector3.up * _aimGroundYOffset - _gunHead.position;

                if (cursorIsNearZombie)
                {
                    rotSpeed *= 3f; // rotate 3x faster when cursor is near zombie
                    Debug.DrawLine(_gunHead.position, hit.transform.position);
                    direction = (hit.transform.position - _gunHead.position).normalized;
                }
                targetRotation = Quaternion.LookRotation(direction.normalized);
            }

            //var eu = targetRotation.eulerAngles;
            //if (eu.x > _minGunAngleWhenNearShooting)
            //    eu.x = _minGunAngleWhenNearShooting;

            //targetRotation = Quaternion.Euler(eu);
            //_gunHead.rotation = targetRotation;

            float blend = 1 - Mathf.Pow(0.5f, Time.deltaTime * rotSpeed);
            _gunHead.rotation = Quaternion.Lerp(_gunHead.rotation, targetRotation, blend);


            if (_nextShoot < Time.timeSinceLevelLoad + _shootCooldown)
            {
                if (Input.GetMouseButtonDown(0) && _currentAmmoCount > 0)
                {
                    Shoot();
                    _nextShoot = Time.timeSinceLevelLoad + _shootCooldown;
                }
            }

        }
    }
}
