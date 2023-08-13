using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public interface IBodyPart 
    {
        void ApplyHit(Vector3 forceDir, float force, int damage);
    }
}
