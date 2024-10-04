using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public interface ITarget
    {
        void TakeDamage(int amount);   
        void Die();
    }
}
