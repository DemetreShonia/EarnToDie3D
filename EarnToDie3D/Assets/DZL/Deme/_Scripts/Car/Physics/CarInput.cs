using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public class CarInput : MonoBehaviour
    {
        float _moveInput;
        float _steerInput;
        public float MoveInput => _moveInput;
        public float SteerInput => _steerInput;
        public bool IsBrakePressed => (Input.GetKey(GlobalInputs.BRAKE_KEY) || _moveInput == 0f);

        public void UpdateInputs()
        {
            _moveInput = Input.GetAxis(InputStrings.VERTICAL);
            _steerInput = Input.GetAxis(InputStrings.HORIZONTAL);
        }
    }
}
