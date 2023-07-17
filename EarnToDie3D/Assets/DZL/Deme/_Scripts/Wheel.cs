using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public class Wheel : MonoBehaviour
    {
        [Header("Steering Properties")]
        [SerializeField] bool _isFrontWheel = false;
        [SerializeField] float _tireGripFactor = 1;
        [SerializeField] float _tireMass = 10;

        [Header("Raycast Properties")]
        [SerializeField] float _rayLength = 1f;
        [SerializeField] LayerMask _groundLayer = default;

        [Header("Spring Properties")]
        [SerializeField] float _springStrength = 100f;
        [SerializeField] float _springDamper = 15f;
        [SerializeField] float _suspensionRestDist = 1f;

        Transform _tireTransform;
        Rigidbody _carRigidbody;
        RaycastHit _tireRay = default;
        bool _rayDidHit = true;


        void Start()
        {
            _tireTransform = transform;
            _carRigidbody = _tireTransform.root.GetComponent<Rigidbody>();
        }
        void FixedUpdate()
        {
            CastRay();
            if (_rayDidHit)
            {
                HandleSpring();
            }
        }
        void HandleSpring()
        {
            Vector3 springDir = _tireTransform.up;
            Debug.DrawRay(_tireTransform.position, springDir * 100, Color.yellow);
            Vector3 tireWordlVel = _carRigidbody.GetPointVelocity(_tireTransform.position);
            print(tireWordlVel + " TireWorldVelocity");

            float offset = _suspensionRestDist - _tireRay.distance;
            if(Mathf.Approximately(offset, 0))
            {
                return;
            }
            print(offset + " Offset");

            float vel = Vector3.Dot(springDir, tireWordlVel);

            float force = (offset * _springStrength) - (vel * _springDamper);
            print(force + " Force");
            _carRigidbody.AddForceAtPosition(springDir * force, _tireTransform.position);
        }
        void HandleSteering()
        {
            // steering
            

            if (_rayDidHit)
            {
                Vector3 steeringDir = _tireTransform.right;

                Vector3 tireWorldVel = _carRigidbody.GetPointVelocity(_tireTransform.position);


                float steeringVel = Vector3.Dot(steeringDir, tireWorldVel);

                float desiredVelChange = -steeringVel * _tireGripFactor;

                float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

                _carRigidbody.AddForceAtPosition(steeringDir * _tireMass * desiredAccel, _tireTransform.position);
            }
        }
         
        void CastRay()
        {
            if(_rayDidHit = Physics.Raycast(_tireTransform.position, -_tireTransform.up, out RaycastHit tireRay, _rayLength, _groundLayer))
            {
                _tireRay = tireRay;
            }
            Debug.DrawRay(_tireTransform.position, -_tireTransform.up * 100, Color.red);
        }
    }

}