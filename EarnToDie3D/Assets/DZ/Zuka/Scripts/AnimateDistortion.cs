using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DumbRide
{   
    [RequireComponent(typeof(MeshRenderer))]
    public class AnimateDistortion : MonoBehaviour
    {
        [SerializeField] private float animationTime;
        [SerializeField] private AnimationCurve animationCurve;
        [SerializeField] private string animationVariable = "_State";
        private Material distortionMaterial;
        
        void Start()
        {
            distortionMaterial = GetComponent<MeshRenderer>().material;
        }

        public void Animate()
        {
            StartCoroutine(AnimationProcess());
        }

        IEnumerator AnimationProcess()
        {
            float start = Time.time;
            float val = 0;
            while (true)
            {
                val = (Time.time - start) / animationTime;
                if (val > 1)
                {
                    val = 1;
                }

                distortionMaterial.SetFloat(animationVariable, animationCurve.Evaluate(val));
                if (val == 1)
                {
                    break;
                }
                yield return null;
            }

        }
    }
}
