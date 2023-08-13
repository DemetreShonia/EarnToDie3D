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

        bool _isInitialized = false;
        Rigidbody _rb;
        public void Initialize(Rigidbody rb)
        {
            _rb = rb;
            _isInitialized = true;
        }
        void OnTriggerEnter(Collider other)
        {
            if (!_isInitialized) return;

            // hitting is filtered via layer, only zombies will be hit btw
            if (other.gameObject.TryGetComponent(out IBodyPart bodyPartHit))
            {
                var speed = _rb.velocity.magnitude * 3.6f; // this is formula to calculate speed of moving object
                bodyPartHit.ApplyHit(transform.forward, _hitForceMultiplier * speed, (int)(_damageMultiplier * speed));
            }
        }
    }
}
