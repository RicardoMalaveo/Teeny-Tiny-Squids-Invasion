using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float velocity = 10.0f;
    public Transform cameraTransform;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();
        Vector3 moveDirection = (camForward * vertical) + (camRight * horizontal);
        transform.Translate(moveDirection * velocity * Time.deltaTime, Space.World);
    }
}