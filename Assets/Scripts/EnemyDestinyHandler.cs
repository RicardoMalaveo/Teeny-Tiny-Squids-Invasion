using UnityEngine;

public class EnemyDestinyHandler : MonoBehaviour
{
    private EnemyInfo info;
    private bool isDead = false;

    private void Awake()
    {
        info = GetComponent<EnemyInfo>();
    }

    public void ApplyDamage(float amount)
    {
        if (isDead) return;

        info.health -= amount;

        if (info.health <= 0)
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
            Debug.Log("Enemy die before reaching the castle");
        }
        else
        {
            Debug.Log("Enemy reached the castle");
        }

        Destroy(gameObject);
    }
}