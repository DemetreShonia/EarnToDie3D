using UnityEngine;

namespace DumbRide
{
    public class DecoratorGun : Decorator
    {
        [SerializeField] GameObject _bloodParticle;
        [SerializeField] Transform _gunHead;
        [SerializeField] Transform _shootPosition;
        [SerializeField] float _rotationSpeed = 7f;
        [SerializeField] Animator _animator;
        Camera _mainCamera;

        [SerializeField] float _shootCooldown = 0.5f;
        float _nextShoot;
        public override void Initialize(DecoratorData data)
        {
            base.Initialize(data);
            _mainCamera = Camera.main;
        }

        public override void Animate()
        {
            _animator.SetTrigger(AnimationStrings.FIRE);
        }

        public override void PlaySound()
        {

        }

        void Shoot()
        {
            Animate();

            if (Physics.Raycast(_shootPosition.position, _shootPosition.forward, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out IBodyPart bodyPart))
                {
                    //target.TakeDamage((int)_data.power);
                    bodyPart.ApplyHit(-hit.normal, 300, 500);

                    var blood = Instantiate(_bloodParticle, hit.point, Quaternion.LookRotation(hit.normal));
                    Destroy(blood, 1f);
                }
            }
            // playsound, animate on keyframe anim event
        }
        
        public override void CheckForInputs()
        {
            Quaternion targetRotation = _gunHead.rotation;
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 direction = hit.point - _gunHead.position;
                direction.y = 0f;

                targetRotation = Quaternion.LookRotation(direction.normalized);
            }

            _gunHead.rotation = Quaternion.Lerp(_gunHead.rotation, targetRotation, _rotationSpeed * Time.deltaTime);


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
