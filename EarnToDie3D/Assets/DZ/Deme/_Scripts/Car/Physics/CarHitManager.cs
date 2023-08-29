using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public class CarHitManager : MonoBehaviour
    {
        [SerializeField] float _hitForce;
        CarHitter[] _hitters;
        Rigidbody _rb;

        void Start()
        {
            _rb = GetComponentInParent<Rigidbody>();
            _hitters = GetComponentsInChildren<CarHitter>();

            foreach (var hitter in _hitters)
            {
                hitter.Initialize(_rb, _hitForce);
            }
        }

    }
}
