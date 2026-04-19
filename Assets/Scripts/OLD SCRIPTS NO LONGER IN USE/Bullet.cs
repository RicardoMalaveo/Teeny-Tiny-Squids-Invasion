using UnityEngine;

public class Bullet : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        DestroyOnImpact();
    }

    private void DestroyOnImpact()
    {
        Destroy(gameObject);
    }
}
