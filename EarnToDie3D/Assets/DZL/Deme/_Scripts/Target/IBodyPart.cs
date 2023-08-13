using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public interface IBodyPart 
    {
        void AddForce(Vector3 forceDir, float force, int damage);
    }
}
