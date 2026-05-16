using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBar : MonoBehaviour
{
    [SerializeField] private Image enemyHPBar;
    private Transform mainCameraTransform;
    [SerializeField] private  Color colorGreen;
    [SerializeField] private  Color colorYellow;
    [SerializeField] private  Color colorOrange;
    [SerializeField] private  Color colorRed;

    private void Start()
    {
        mainCameraTransform = Camera.main.transform;
    }

    public void UpdateHPBar(float currentHealth, float maxHealth)
    {
        enemyHPBar.fillAmount = currentHealth / maxHealth;

        if (enemyHPBar.fillAmount >= 0.70f)
        {
            enemyHPBar.color = colorGreen;
        }
        else if (enemyHPBar.fillAmount >= 0.50f)
        {
            enemyHPBar.color = colorYellow;
        }
        else if (enemyHPBar.fillAmount >= 0.30f)
        {
            enemyHPBar.color = colorOrange;
        }
        else
        {
            enemyHPBar.color = colorRed;
        }
    }

    private void LateUpdate()
    {
       transform.LookAt(transform.position + mainCameraTransform.forward);
    }
}
