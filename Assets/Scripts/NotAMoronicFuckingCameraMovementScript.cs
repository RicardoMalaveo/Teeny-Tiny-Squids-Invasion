using UnityEngine;

public class NotAMoronicFuckingCameraMovementScript : MonoBehaviour
{
    [SerializeField] private Transform pitchPivot;

    [SerializeField] private float cameraMovementSpeed;
    [SerializeField] private Vector2 mapSizeLimit;

    [SerializeField] private float rotationSpeed;
    [SerializeField] private float horizontalRotationLimit;
    [SerializeField] private float minPitch;
    [SerializeField] private float maxPitch;
    [SerializeField] private float resetSpeed;

    private float currentYaw = 0f;
    private float currentPitch;

    private Quaternion rootHomeRotation;
    private Quaternion pitchHomeRotation;

    void Start()
    {
        rootHomeRotation = transform.localRotation;
        pitchHomeRotation = pitchPivot.localRotation;

        currentPitch = pitchPivot.localEulerAngles.x;
        if (currentPitch > 180) currentPitch -= 360;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
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

    void HandleRotation()
    {
        if (Input.GetMouseButton(2) || Input.GetMouseButton(1))
        {
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            float mouseY = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            currentYaw = Mathf.Clamp(currentYaw + mouseX, -horizontalRotationLimit, horizontalRotationLimit);
            currentPitch = Mathf.Clamp(currentPitch + mouseY, minPitch, maxPitch);
        }
        else
        {
            currentYaw = Mathf.Lerp(currentYaw, 0, Time.deltaTime * resetSpeed);
            currentPitch = Mathf.Lerp(currentPitch, pitchHomeRotation.eulerAngles.x, Time.deltaTime * resetSpeed);
        }

        transform.localRotation = rootHomeRotation * Quaternion.Euler(0, currentYaw, 0);

        pitchPivot.localRotation = Quaternion.Euler(currentPitch, 0, 0);
    }
}