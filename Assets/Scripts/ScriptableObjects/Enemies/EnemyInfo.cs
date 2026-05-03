using UnityEngine;
[CreateAssetMenu(fileName = "Enemy Data", menuName = "Enemy Data")]
public class EnemyInfo : ScriptableObject
{
    public string enemyName;
    public GameObject enemyPrefab;

    public int maxHealth;
    public float moveSpeed;
    public float dangerLevel;


    public bool isAerial;
    public bool isArmored;
}
