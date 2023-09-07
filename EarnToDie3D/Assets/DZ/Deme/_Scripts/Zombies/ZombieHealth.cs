using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public class ZombieHealth : HealthBase
    {
        public override void Die()
        {
            base.Die();
            GetComponent<ZombieController>().EnableRagdoll();
            print("DEAD");
            Destroy(gameObject, 3f);
        }
    }
}
