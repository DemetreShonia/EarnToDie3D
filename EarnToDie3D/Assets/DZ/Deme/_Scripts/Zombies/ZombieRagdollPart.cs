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

        ZombieController _zombieController;

        void OnEnable()
        {
            _rb = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>(); // can be null
            _zombieHealth = transform.root.GetComponent<ZombieHealth>();
            _zombieController = transform.root.GetComponent<ZombieController>();
        }
        public void ApplyHit(Vector3 forceDir, float force, int damage)
        {
            _rb.AddForce(forceDir * force, ForceMode.Impulse);
            _zombieHealth.TakeDamage(damage);
            _zombieController.CarHitsZombie();
        }
        public void DisablePart()
        {
            _rb.isKinematic = true;
        }
        public void EnablePart()
        {
            _rb.isKinematic = false;
        }
        public void AdjustRBMass(float factor)
        {
            _rb.mass *= factor;
        }
    }
}
