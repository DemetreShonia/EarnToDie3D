using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    [RequireComponent(typeof(BoxCollider))]
    public class CarHitter : MonoBehaviour
    {
        [SerializeField] float _damageMultiplier = 1f;
        [SerializeField] float _hitForceMultiplier = 1f;

        float _additiveHitForce;

        bool _isInitialized = false;
        Rigidbody _rb;
        public void Initialize(Rigidbody rb, float hitForce)
        {
            _rb = rb;
            _isInitialized = true;
            _additiveHitForce = hitForce;
        }
        public void SetDamageMultiplier(float multiplier)
        {
            _damageMultiplier = multiplier;
        }
        public void Hit(Transform hitTransform)
        {
            if (!_isInitialized) return;

            // hitting is filtered via layer, only zombies will be hit btw
            if (hitTransform.TryGetComponent(out IBodyPart bodyPartHit))
            {
                var speed = _rb.velocity.magnitude * 3.6f; // this is formula to calculate speed of moving object
                bodyPartHit.ApplyHit(transform.forward, _hitForceMultiplier * speed + _additiveHitForce, (int)(_damageMultiplier * speed));
            }
        }
    }
}
