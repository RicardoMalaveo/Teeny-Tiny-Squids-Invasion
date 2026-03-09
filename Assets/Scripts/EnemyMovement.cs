using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private aiMovementManager AIM;
    private Rigidbody rb;
    private float rotationInput;
    public float walkSpeed = 20F;
    private float rotationSpeed = 100F;
    public string enemyName = "Squid Juan";

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        getIM();
    }

    private void FixedUpdate()
    {
        Acceleration();
        RotateEnemy();
    }

    public void Acceleration()
    {

    }

    public void RotateEnemy()
    {
        //Enemy rotation
        rotationInput = AIM.horizontal;

        float rotation = rotationInput * rotationSpeed * Time.fixedDeltaTime;

        Quaternion turnRotation = Quaternion.Euler(0.0f, rotation, 0.0f);

        rb.MoveRotation(rb.rotation * turnRotation);
    }

    private void getIM()
    {
        AIM = GetComponent<aiMovementManager>();
    }
}
