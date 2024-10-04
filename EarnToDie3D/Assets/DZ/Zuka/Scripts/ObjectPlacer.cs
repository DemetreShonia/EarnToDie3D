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
    public Transform AllParent;
    [Range(0f, 1f)]
    public float grassAngleAlign = 0.5f;

    public string roadTag;
    public LayerMask GroundMask;
    public bool placeProps;
    public bool placeGrass;
    public bool placeAll;

    void Update()
    {
        if (placeProps)
        {
            placeProps = false;
            PlaceProps();

        }

        if (placeGrass)
        {
            placeGrass = false;
            PlaceGlass();
            
        }

        if (placeAll)
        {
            placeAll = false;
            PlaceAll();
        }
    }

    private void PlaceGlass()
    {
        if (GrassParent != null)
        {
            Transform[] grass = GrassParent.GetComponentsInChildren<Transform>();
            for (int i = 1; i < grass.Length; i++)
            {
                if (grass[i] == null || !((GrassMask.value & (1 << grass[i].gameObject.layer)) > 0))
                {
                    continue;
                }
                PlaceOneGrass(grass[i]);
            }
        }
    }

    private void PlaceOneGrass(Transform grss)
    {
        RaycastHit hit;
        Ray ray = new Ray(grss.position + Vector3.up * 10, Vector3.down);
        if (Physics.Raycast(ray, out hit, RayLength, GroundMask))
        {

            if (hit.transform.CompareTag(roadTag))
            {
                DestroyImmediate(grss.gameObject);
                return;
            }

            grss.position = hit.point;
            grss.rotation = Quaternion.Lerp(Quaternion.Euler(-90, 0, 0), Quaternion.LookRotation(hit.normal), grassAngleAlign);
            grss.rotation *= Quaternion.Euler(90, 0, 0);
        }
        else
        {
            
            DestroyImmediate(grss.gameObject);
        }
    }

    private void PlaceProps()
    {
        if (PropsParent != null)
        {
            Transform[] props = PropsParent.GetComponentsInChildren<Transform>();
            for (int i = 1; i < props.Length; i++)
            {
                if (props[i] == null || !((PropsMask.value & (1 << props[i].gameObject.layer)) > 0))
                {
                    continue;
                }
                PlaceOneProp(props[i]);
            }
        }
    }

    private void PlaceOneProp(Transform prop)
    {
        RaycastHit hit;
        Ray ray = new Ray(prop.position + Vector3.up * 10, Vector3.down);
        if (Physics.Raycast(ray, out hit, RayLength, GroundMask))
        {
            if (hit.transform.CompareTag(roadTag))
            {
                DestroyImmediate(prop.gameObject);
                return;
            }
            prop.position = hit.point;
        }
    }

    private void PlaceAll()
    {
        if (AllParent != null)
        {
            Transform[] props = AllParent.GetComponentsInChildren<Transform>();
            for (int i = 1; i < props.Length; i++)
            {
                if (props[i] == null)
                {
                    continue;
                }
                if ((PropsMask.value & (1 << props[i].gameObject.layer)) > 0)
                {
                    PlaceOneProp(props[i]);
                    continue;
                }
                if ((GrassMask.value & (1 << props[i].gameObject.layer)) > 0)
                {
                    PlaceOneGrass(props[i]);
                    continue;
                }
            }
        }
    }
}
