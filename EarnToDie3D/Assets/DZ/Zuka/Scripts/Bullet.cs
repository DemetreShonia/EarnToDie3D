using System.Collections;
using System.Collections.Generic;
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
        bool didHit = false;
        void Update()
        {
            if (!didHit)
            {
                _lastPosition = transform.position;

                transform.position += transform.forward * _speed * Time.deltaTime;
            }
            else
            {

            }
        }

        private void LateUpdate()
        {
            if (didHit)
            {
                return;
            }
            RaycastHit hit;
            if(Physics.SphereCast(_lastPosition, _raySphereRadius, transform.forward, out hit, (transform.position - _lastPosition).magnitude, _canHit))
            {
                transform.position = hit.point;
                didHit = true;
                _hitFeedback?.PlayFeedbacks();
                DealDamage();
            }
        }

        private void DealDamage()
        {

        }
    }
}
