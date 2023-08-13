using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public class ZombieRagdollPart : MonoBehaviour, IBodyPart
    {
        Rigidbody _rb;
        Collider _collider;
        ZombieHealth _zombieHealth;

        void OnEnable()
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>(); // can be null
            _zombieHealth = transform.root.GetComponent<ZombieHealth>();        
        }
        public void ApplyHit(Vector3 forceDir, float force, int damage)
        {
            _rb.AddForce(forceDir * force, ForceMode.Impulse);
            _zombieHealth.TakeDamage(damage);
        }
    }
}
