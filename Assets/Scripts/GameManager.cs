using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private int initialPlayerSand;
    public float refundPercentage;
    public int CurrentSand { get; private set; }
    [SerializeField] private int totalPlayerHP;
    [SerializeField] private int CurrentWave;


    //references to UI info
    [SerializeField] private TextMeshProUGUI sandText;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        CurrentSand = initialPlayerSand;
        UpdateSandUI();
    }

    public bool TryPurchase(int cost)
    {
        if (CurrentSand >= cost)
        {
            CurrentSand -= cost;
            UpdateSandUI();
            return true;
        }
        Debug.Log("Not enough sand");
        return false;
    }

    public void AddSand(int amount)
    {
        Debug.Log("refunding: " + amount);
        CurrentSand += amount;
        UpdateSandUI();
    }

    private void UpdateSandUI()
    {
        sandText.text = "Sand: " + CurrentSand.ToString();
    }
}
