using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace DumbRide
{
    
    public class ZombieHitParticlesController : MonoBehaviour
    {
        [SerializeField] VisualEffect _particlesVFX;
      
        public void PlayEffect(Vector3 pos, Vector3 normal, Vector3 towards)
        {
            _particlesVFX.SetVector3("direction", (towards + normal).normalized);
            _particlesVFX.transform.position = pos;
            _particlesVFX.Play();
        }
    }
}
