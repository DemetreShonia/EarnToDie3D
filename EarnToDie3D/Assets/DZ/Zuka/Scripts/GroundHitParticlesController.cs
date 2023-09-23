using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace DumbRide
{
    
    public class GroundHitParticlesController : MonoBehaviour
    {
        [SerializeField] VisualEffect _particlesVFX;
        [SerializeField] VisualEffect _dustVFX;

        public void PlayEffect(Vector3 pos, Vector3 normal, Vector3 towards)
        {
            _particlesVFX.SetVector3("direction", normal);
            _particlesVFX.transform.position = pos;
            _particlesVFX.Play();

            _dustVFX.transform.position = pos;
            _dustVFX.SetVector3("direction", (towards + normal).normalized);
            _dustVFX.Play();
        }
    }
}
