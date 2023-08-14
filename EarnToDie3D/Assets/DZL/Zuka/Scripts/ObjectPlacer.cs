using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ObjectPlacer : MonoBehaviour
{
    public float RayLength = 200;
    public Transform PropsParent;
    public LayerMask PropsMask;
    public Transform GrassParent;
    public LayerMask GrassMask;

    [Range(0f, 1f)]
    public float grassAngleAlign = 0.5f;

    public LayerMask GroundMask;
    public bool placeProps;
    public bool placeGrass;

    void Update()
    {
        if (placeProps)
        {
            placeProps = false;
            if (PropsParent != null)
            {
                Transform[] props = PropsParent.GetComponentsInChildren<Transform>();
                for (int i = 1; i < props.Length; i++)
                {
                    RaycastHit hit;
                    Ray ray = new Ray(props[i].position + Vector3.up * 10, Vector3.down);
                    if (Physics.Raycast(ray, out hit, RayLength, GroundMask))
                    {
                        Collider col = props[i].transform.GetComponent<Collider>();
                        if (col == null)
                        {
                            props[i].position = hit.point;
                        }
                        else
                        {
                            props[i].position += hit.point - col.bounds.center;
                        }
                        
                    }
                }
            }

            PropsParent = null;
        }

        if (placeGrass)
        {
            placeGrass = false;
            if (GrassParent != null)
            {
                Transform[] grass = GrassParent.GetComponentsInChildren<Transform>();
                for (int i = 1; i < grass.Length; i++)
                {
                    if ((GrassMask.value & (1 << grass[i].gameObject.layer)) > 0)
                    {
                        // good
                    }
                    else
                    {
                        continue;
                    }

                    RaycastHit hit;
                    Ray ray = new Ray(grass[i].position + Vector3.up * 10, Vector3.down);
                    if (Physics.Raycast(ray, out hit, RayLength, GroundMask))
                    {
                        grass[i].position = hit.point;


                        

                        grass[i].rotation = Quaternion.Lerp(Quaternion.Euler(-90, 0, 0), Quaternion.LookRotation(hit.normal), grassAngleAlign);
                        grass[i].rotation *= Quaternion.Euler(90, 0, 0);
                    }
                }
            }
            //GrassParent = null;
        }
    }

}
