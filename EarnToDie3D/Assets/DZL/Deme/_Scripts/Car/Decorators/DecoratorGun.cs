using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    public class DecoratorGun : Decorator
    {
        [SerializeField] Transform _gunHead;
        Camera _mainCamera;
        public override void Initialize(DecoratorData data)
        {
            base.Initialize(data);
            _mainCamera = Camera.main;
        }

        public override void Animate()
        {
        }

        public override void PlaySound()
        {
        }
        public override void CheckForInputs()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 direction = hit.point - _gunHead.position;

                direction.y = 0f;

                _gunHead.forward = direction.normalized;
            }
        }
    }
}
