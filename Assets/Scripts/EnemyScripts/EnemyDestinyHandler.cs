using UnityEngine;

public class EnemyDestinyHandler : MonoBehaviour
{
    public EnemyInfo data;
    private bool isDead = false;
    private float currentHealth;
    private Transform castleTarget;

    public void Setup(EnemyInfo info, Transform target)
    {
        data = info;
        currentHealth = data.maxHealth;
        castleTarget = target;
    }

    public void ApplyDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            HandleDeath(true);
        }
    }

    public void ReachCastle()
    {
        if (isDead) return;

        int damage = Mathf.RoundToInt(data.dangerLevel);
        //GameManager.Instance.TakeCastleDamage(damage);

        HandleDeath(false);
    }

    private void HandleDeath(bool killedByPlayer)
    {
        if (isDead) return;
        isDead = true;

        if (killedByPlayer)
        {
            GameManager.Instance.AddSand(data.dangerLevel);
        }

        WaveManager.Instance.DecrementEnemyCount();
        Destroy(gameObject);
    }
}