using UnityEngine;

public class CameraPivotMovement : MonoBehaviour
{
    [SerializeField] private Transform xAxisPivot;

    [SerializeField] private float cameraMovementSpeed;
    [SerializeField] private Vector2 mapSizeLimit;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private float horizontalRotationLimit;
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;
    [SerializeField] private float resetSpeed;

    private float currentYAxis = 0f;
    private float CurrentXAxis;
    private float OriginalXAxis;
    private float OriginalYAxis;

    void Start()
    {
        OriginalYAxis = transform.localEulerAngles.y;
        OriginalXAxis = xAxisPivot.localEulerAngles.x;

        if (OriginalXAxis > 180) OriginalXAxis -= 360;
        CurrentXAxis = OriginalXAxis;
    }

    void Update()
    {
        CameraPivotTransform();
        CameraPivotRotation();
    }

    void CameraPivotTransform()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        forward.y = 0;
        right.y = 0;

        Vector3 move = (forward.normalized * z + right.normalized * x) * cameraMovementSpeed * Time.deltaTime;
        transform.position += move;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -mapSizeLimit.x, mapSizeLimit.x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, -mapSizeLimit.y, mapSizeLimit.y)
        );
    }

    void CameraPivotRotation()
    {
        if (Input.GetMouseButton(2) || Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            currentYAxis = Mathf.Clamp(currentYAxis + mouseX, -horizontalRotationLimit, horizontalRotationLimit);
            CurrentXAxis = Mathf.Clamp(CurrentXAxis + mouseY, minPitch, maxPitch);
        }
        else
        {
            currentYAxis = 0;
            CurrentXAxis = OriginalXAxis;
        }

        transform.localRotation = Quaternion.Euler(0, OriginalYAxis + currentYAxis, 0);
        xAxisPivot.localRotation = Quaternion.Euler(CurrentXAxis, 0, 0);
    }
}