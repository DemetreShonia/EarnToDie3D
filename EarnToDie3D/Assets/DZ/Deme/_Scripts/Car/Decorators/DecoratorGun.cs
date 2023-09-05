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
        [SerializeField] float _minGunAngleWhenNearShooting = 20;

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

            GameObject bullet = Instantiate(_bullet);
            bullet.transform.position = _shootPosition.position;
            bullet.transform.LookAt(_shootPosition.position + _shootPosition.forward);

            _currentAmmoCount--;
            UpdateUI();
        }

        public override void CheckForInputs()
        {
            Quaternion targetRotation = _gunHead.rotation;
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit, Mathf.Infinity,_cameraRayCanHit))
            {
                Vector3 direction = hit.point + Vector3.up * _aimGroundYOffset - _gunHead.position;
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

            var eu = _gunHead.localEulerAngles;
            if (eu.x > _minGunAngleWhenNearShooting)
                eu.x = _minGunAngleWhenNearShooting;

            _gunHead.localEulerAngles = eu;

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
