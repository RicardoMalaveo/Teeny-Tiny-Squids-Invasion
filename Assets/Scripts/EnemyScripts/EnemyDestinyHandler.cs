using UnityEngine;

public class EnemyDestinyHandler : MonoBehaviour
{
    public EnemyInfo data;
    private bool isDead = false;
    private float currentHealth;

    private void Awake()
    {
        data = GetComponent<EnemyInfo>();
        currentHealth = data.maxHealth;
    }

    public void ApplyDamage(float amount)
    {
        if (isDead) return;

        data.maxHealth -= amount;

        if (data.maxHealth <= 0)
        {
            HandleDeath(true);
        }
    }

    public void ReachCastle()
    {
        if (isDead) return;

        ////castle damage
        //if (GameManager.Instance != null)
        //{
        //    GameManager.Instance.TakeCastleDamage(1);
        //}

        HandleDeath(false);
    }

    private void HandleDeath(bool killedByPlayer)
    {
        if (isDead) return;
        isDead = true;

        if (killedByPlayer)
        {
            //if (GameManager.Instance != null)
            //{
            //    GameManager.Instance.AddResources(info.sandReward);
            //}
        }
        else
        {
        }

        Destroy(gameObject);
    }
}