using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public class CarInput : MonoBehaviour
    {
        [SerializeField] KeyCode _brakeKey = KeyCode.Space;

        float _moveInput;
        float _steerInput;
        public float MoveInput => _moveInput;
        public float SteerInput => _steerInput;
        public bool IsBrakePressed => (Input.GetKey(_brakeKey) || _moveInput == 0f);

        public void UpdateInputs()
        {
            _moveInput = Input.GetAxis(InputStrings.VERTICAL);
            _steerInput = Input.GetAxis(InputStrings.HORIZONTAL);
        }
    }
}
