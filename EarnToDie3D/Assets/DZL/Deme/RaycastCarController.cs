using UnityEngine;

[System.Serializable]
public class WheelInfo
{
    public Transform wheelTransform;
    public float suspensionDistance = 0.3f;
    public float suspensionForce = 10000f;
    public float forwardFriction = 1f;
    public float sidewaysFriction = 1f;
    public LayerMask groundLayer;
    public bool isSteeringWheel;
    public bool isPoweredWheel;
}

public class RaycastCarController : MonoBehaviour
{
    [Header("Control Parameters")]
    public float maxSteeringAngle = 30f;
    public float motorForce = 1000f;
    public float brakeForce = 2000f;
    public float gravity = 9.81f;

    [Header("Wheel Settings")]
    public WheelInfo[] wheelInfos;

    private float horizontalInput;
    private float verticalInput;
    private bool isBraking;

    private void Update()
    {
        // Get user input for steering, acceleration, and braking
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isBraking = Input.GetKey(KeyCode.Space);
    }

    private void FixedUpdate()
    {
        ApplySteering();
        ApplyDrivingForce();
        ApplyBrakingForce();
        UpdateWheelPositions();
    }

    private void ApplySteering()
    {
        float steeringAngle = maxSteeringAngle * horizontalInput;

        foreach (WheelInfo wheelInfo in wheelInfos)
        {
            if (wheelInfo.isSteeringWheel)
            {
                wheelInfo.wheelTransform.localRotation = Quaternion.Euler(0f, steeringAngle, 0f);
            }
        }
    }

    private void ApplyDrivingForce()
    {
        foreach (WheelInfo wheelInfo in wheelInfos)
        {
            if (wheelInfo.isPoweredWheel)
            {
                float force = verticalInput * motorForce;
                RaycastHit hit;
                bool isGrounded = Physics.Raycast(wheelInfo.wheelTransform.position, -transform.up, out hit, wheelInfo.suspensionDistance + 0.1f, wheelInfo.groundLayer);
                if (isGrounded)
                {
                    Vector3 groundNormal = hit.normal;
                    Vector3 forwardForce = transform.forward * force;
                    Vector3 suspensionForce = -groundNormal * wheelInfo.suspensionForce;

                    Vector3 totalForce = forwardForce + suspensionForce;
                    wheelInfo.wheelTransform.root.GetComponent<Rigidbody>().AddForceAtPosition(totalForce, wheelInfo.wheelTransform.position);
                }
            }
        }
    }

    private void ApplyBrakingForce()
    {
        foreach (WheelInfo wheelInfo in wheelInfos)
        {
            if (isBraking)
            {
                float brakeTorque = brakeForce;
                wheelInfo.wheelTransform.root.GetComponent<Rigidbody>().AddForceAtPosition(-wheelInfo.wheelTransform.forward * brakeTorque, wheelInfo.wheelTransform.position);
            }
        }
    }

    private void UpdateWheelPositions()
    {
        foreach (WheelInfo wheelInfo in wheelInfos)
        {
            RaycastHit hit;
            bool isGrounded = Physics.Raycast(wheelInfo.wheelTransform.position, -transform.up, out hit, wheelInfo.suspensionDistance + 0.1f, wheelInfo.groundLayer);
            if (isGrounded)
            {
                wheelInfo.wheelTransform.position = hit.point + hit.normal * wheelInfo.suspensionDistance;
                wheelInfo.wheelTransform.rotation = Quaternion.FromToRotation(wheelInfo.wheelTransform.up, hit.normal) * wheelInfo.wheelTransform.rotation;
            }
            else
            {
                wheelInfo.wheelTransform.position -= wheelInfo.wheelTransform.up * gravity * Time.fixedDeltaTime;
            }
        }
    }
}
