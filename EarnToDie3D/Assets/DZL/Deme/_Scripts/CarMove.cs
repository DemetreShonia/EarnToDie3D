using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public class CarMove : MonoBehaviour
    {
        
        void HandleTire()
        {
            bool rayDidHit = true;
            Transform tireTransform = null;
            Rigidbody carRigidbody = null;
            RaycastHit tireRay = default;
            float springStrength = 10f;
            float springDamper = 10f;
            float suspensionRestDist = 1f;

            if(rayDidHit)
            {
                Vector3 springDir = tireTransform.up;

                Vector3 tireWordlVel = carRigidbody.GetPointVelocity(tireTransform.position);

                float offset = suspensionRestDist - tireRay.distance;

                float vel = Vector3.Dot(springDir, tireWordlVel);

                float force = (offset * springStrength) - (vel * springDamper);
                carRigidbody.AddForceAtPosition(springDir * force, tireTransform.position);
            }

            // steering
            float tireGripFactor = 1;
            float tireMass = 10;

            if (rayDidHit)
            {
                Vector3 steeringDir = tireTransform.right;

                Vector3 tireWorldVel = carRigidbody.GetPointVelocity(tireTransform.position);


                float steeringVel = Vector3.Dot(steeringDir, tireWorldVel);

                float desiredVelChange = -steeringVel * tireGripFactor;

                float desiredAccel = desiredVelChange / Time.fixedDeltaTime;

                carRigidbody.AddForceAtPosition(steeringDir * tireMass * desiredAccel, tireTransform.position);
            }


            // acceleration / braking
            Transform carTransform = null;
            float accelInput = 0f;
            float carTopSpeed = 1f;

            AnimationCurve powerCurve = default;

            if (rayDidHit)
            {
                Vector3 accelDir = tireTransform.forward;
                if(accelInput > 0.0f)
                {
                    float carSpeed = Vector3.Dot(carTransform.forward, carRigidbody.velocity);

                    float normalizedSpeed = Mathf.Clamp01(Mathf.Abs(carSpeed) / carTopSpeed);

                    float availableTorque = powerCurve.Evaluate(normalizedSpeed) * accelInput;

                    carRigidbody.AddForceAtPosition(accelDir * availableTorque, tireTransform.position);
                }
            }


        }
    }

}