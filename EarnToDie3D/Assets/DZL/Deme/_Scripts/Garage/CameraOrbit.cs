using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [SerializeField] float _acceleration = 10f;
    [SerializeField] float _deceleration = 10f;
    [SerializeField] float _maxRotationSpeed = 50f;
    [SerializeField] float _verticalClampAngle = 60f;
    [SerializeField] float _horizontalClampAngle = 80f;

    Transform _transform;
    bool _isRotating = false;
    Vector3 _lastMousePosition;
    float _rotationSpeedX = 0f;
    float _rotationSpeedY = 0f;

    void Start()
    {
        _transform = transform;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isRotating = true;
            _lastMousePosition = Input.mousePosition;
            _rotationSpeedX = 0f;
            _rotationSpeedY = 0f;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isRotating = false;
        }

        if (_isRotating)
        {
            Vector3 mouseDelta = Input.mousePosition - _lastMousePosition;
            _lastMousePosition = Input.mousePosition;

            _rotationSpeedX -= mouseDelta.y * _acceleration * Time.deltaTime;
            _rotationSpeedY += mouseDelta.x * _acceleration * Time.deltaTime;

            _rotationSpeedX = Mathf.Clamp(_rotationSpeedX, -_maxRotationSpeed, _maxRotationSpeed);
            _rotationSpeedY = Mathf.Clamp(_rotationSpeedY, -_maxRotationSpeed, _maxRotationSpeed);
            _rotationSpeedX = Mathf.Clamp(_rotationSpeedX, -_verticalClampAngle, _verticalClampAngle);

            _transform.Rotate(Vector3.up, _rotationSpeedY * Time.deltaTime, Space.World);
            _transform.Rotate(_transform.right, _rotationSpeedX * Time.deltaTime, Space.World);
        }
        else
        {
            _rotationSpeedX = Mathf.MoveTowards(_rotationSpeedX, 0f, _deceleration * Time.deltaTime);
            _rotationSpeedY = Mathf.MoveTowards(_rotationSpeedY, 0f, _deceleration * Time.deltaTime);

            _transform.Rotate(Vector3.up, _rotationSpeedY * Time.deltaTime, Space.World);
            _transform.Rotate(_transform.right, _rotationSpeedX * Time.deltaTime, Space.World);
        }
    }
}
