using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float velocity = 10.0f;
    public Transform cameraTransform;

    [Header("Objetos Guia (Indicadores)")]
    public GameObject indicadorMovimiento;
    public GameObject indicadorRotacion;
    public GameObject indicadorInclinacion;

    void Start()
    {
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
        if (indicadorMovimiento != null) indicadorMovimiento.SetActive(false);
        if (indicadorRotacion != null) indicadorRotacion.SetActive(false);
        if (indicadorInclinacion != null) indicadorInclinacion.SetActive(false);
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
        bool seEstaMoviendo = Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f;

        if (indicadorMovimiento != null)
        {
            indicadorMovimiento.SetActive(seEstaMoviendo);
        }
        bool estaRotando = Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E);
        if (indicadorRotacion != null)
        {
            indicadorRotacion.SetActive(estaRotando);
        }
        bool estaInclinando = Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F);
        if (indicadorInclinacion != null)
        {
            indicadorInclinacion.SetActive(estaInclinando);
        }
    }
}