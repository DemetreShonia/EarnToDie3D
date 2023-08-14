using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public class ZombieController : MonoBehaviour
    {
        [SerializeField] Transform _ragdollParent;
        [SerializeField] ZombieRagdollPart[] _ragdollParts;
        [SerializeField] float _standUpTimeAfterKick = 5f;

        Animator _animator;
        WaitForSeconds _waitForSeconds;

        bool _isHitByCar = false; // is on ground / ragdoll position
        void Start()
        {
            _animator = GetComponent<Animator>();
            _waitForSeconds = new WaitForSeconds(_standUpTimeAfterKick); 
            foreach (var part in _ragdollParts)
            {
                part.DisablePart();
            }
        }
        public void CarHitsZombie()
        {
            if (_isHitByCar) return;

            IEnumerator Routine ()
            {
                _isHitByCar = true;

                _animator.enabled = false;
                foreach (var part in _ragdollParts)
                {
                    part.EnablePart();
                }

                _ragdollParent.SetParent(null);

                yield return _waitForSeconds;

                _animator.enabled = true; // this can be interpolated

                foreach (var part in _ragdollParts)
                {
                    part.DisablePart();
                }

                _isHitByCar = false;
                transform.position = _ragdollParent.position;
                _ragdollParent.SetParent(transform);

                bool isFacingUp = _animator.GetBoneTransform(HumanBodyBones.Hips).forward.y > 0f;
                print($"{isFacingUp} is facing up");
                _animator.SetTrigger(isFacingUp ? AnimationStrings.STAND_UP_BACK : AnimationStrings.STAND_UP_FRONT);
            }

            StartCoroutine(Routine());
        }
    }
}
