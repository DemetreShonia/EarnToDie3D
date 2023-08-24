using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    public Rigidbody rb;
    public float force = 5;
    public float rotSpeed = 1;
    public float maxSpeed = 10;
    public float hover = 5;
    public float rayDist = 3;
    public LayerMask groundLayer;
    private void FixedUpdate()
    {
        float vert = Input.GetAxis("Vertical");
        float hor = Input.GetAxis("Horizontal");

        Vector3 angles = transform.eulerAngles;
        angles.y += rotSpeed * hor;
        transform.eulerAngles = angles;

        rb.AddForce(force * transform.forward * vert);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        // hover
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out hit, rayDist, groundLayer))
        {
            float diff = transform.position.y - hit.point.y;
            rb.AddForce(Vector3.up * (1 - diff / rayDist) * hover);
        }
    }
}
