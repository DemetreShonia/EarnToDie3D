using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPosition : MonoBehaviour
{
    [SerializeField]
    private bool x;

    [SerializeField]
    private bool y;

    [SerializeField]
    private bool z;

    public Transform target;

    private Vector3 pos;
    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        pos = transform.position;

        if (x)
        {
            pos.x = target.position.x;
        }

        if (y)
        {
            pos.y = target.position.y;
        }

        if (z)
        {
            pos.z = target.position.z;
        }

        transform.position = pos;

    }

}
