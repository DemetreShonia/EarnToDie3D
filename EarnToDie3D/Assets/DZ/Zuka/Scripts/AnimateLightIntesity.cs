using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{
    [RequireComponent(typeof(Light))]
    public class AnimateLightIntesity : MonoBehaviour
    {
        [SerializeField] private float animationTime;
        [SerializeField] private AnimationCurve animationCurve;
        [SerializeField] private Vector2 remapCurve = new Vector2(0, 200);
        private Light lightObj;

        void Start()
        {
            lightObj = GetComponent<Light>();
        }

        public void Animate()
        {
            StartCoroutine(AnimationProcess());
        }

        IEnumerator AnimationProcess()
        {
            float start = Time.time;
            float val;
            float evaluated;
            
            while (true)
            {
                val = (Time.time - start) / animationTime;
                if (val > 1)
                {
                    val = 1;
                }

                evaluated = MoreMountains.Tools.MMMaths.Remap(animationCurve.Evaluate(val), 0, 1, remapCurve.x, remapCurve.y);

                lightObj.intensity = evaluated;

                if (val == 1)
                {
                    break;
                }
                yield return null;
            }

        }
    }
}
