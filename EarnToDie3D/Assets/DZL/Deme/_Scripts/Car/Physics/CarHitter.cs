using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    [RequireComponent(typeof(BoxCollider))]
    public class CarHitter : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            print(gameObject.name + " hit " + other.gameObject.name);
        }
    }
}
