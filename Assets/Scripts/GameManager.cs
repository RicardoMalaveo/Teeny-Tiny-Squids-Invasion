using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum GameState { Prep, Wave, GameOver }
    public GameState currentState;
    private int currentWaveNumber = 1;

    [SerializeField] private float totalPlayerHP;
    [SerializeField] private float currentPlayerHP;
    [SerializeField] private float initialPlayerSand;
    public float refundPercentage;
    public float CurrentSand { get; private set; }
    public bool isGameOver = false;

    [SerializeField] private float initialPrepTime;
    [SerializeField] private float timeBetweenWaves;
    private float currentCountdown;

    //references to UI info
    [SerializeField] private TextMeshProUGUI sandText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI currentWave;
    [SerializeField] private TextMeshProUGUI gameState;

    private void Awake()
    {
        if (Instance == null) Instance = this; else Destroy(gameObject);

        CurrentSand = initialPlayerSand;
        currentPlayerHP = totalPlayerHP;
    }

    private void Start()
    {
        UpdateUI();
        StartCountdown(initialPrepTime);
    }

    public void SkipCountdown()
    {
        if (currentState == GameState.Prep)
        {
            StartWave();
        }
    }

    private void Update()
    {
        if (isGameOver) return;

        currentCountdown -= Time.deltaTime;
        UpdateTimerUI();

        if (currentState == GameState.Prep && currentCountdown <= 0)
        {
            StartWave();
        }
    }

    public void StartCountdown(float seconds)
    {
        currentState = GameState.Prep;
        currentCountdown = seconds;
        UpdateUI();
    }

    public void StartWave()
    {
        if (currentState == GameState.Wave) return;

        currentState = GameState.Wave;
        currentCountdown = WaveManager.Instance.GetCurrentWaveDuration();
        UpdateUI();
        WaveManager.Instance.StartNextWave();
    }

    public void EnemyWaveOver()
    {
        if (isGameOver) return;
        currentWaveNumber++; // Move to next wave
        StartCountdown(timeBetweenWaves);
    }
    private void UpdateTimerUI()
    {
        float t = Mathf.Max(0, currentCountdown);
        int minutes = Mathf.FloorToInt(t / 60);
        int seconds = Mathf.FloorToInt(t % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void DamagePlayer(float amount)
    {
        if (isGameOver) return;

        currentPlayerHP = Mathf.Max(0, currentPlayerHP - amount);
        Debug.Log("Castle HP: " + currentPlayerHP);
        UpdateUI();
        if (currentPlayerHP <= 0) LoseGame();
    }

    public bool TryPurchase(int cost)
    {
        if (CurrentSand >= cost)
        {
            CurrentSand -= cost;
            UpdateUI();
            return true;
        }
        Debug.Log("Not enough sand");
        return false;
    }

    public void AddSand(float amount)
    {
        Debug.Log("Adding sand: " + amount);
        CurrentSand += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        sandText.text = "Sand: " + CurrentSand;
        hpText.text = "Castle HP: "+ currentPlayerHP;
        currentWave.text = "Enemy Wave: " + currentWaveNumber;

        gameState.text = (currentState == GameState.Prep) ? "PREPARING" : "ENEMY WAVE!";
        gameState.color = (currentState == GameState.Prep) ? Color.blue : Color.red;
    }

    private void LoseGame()
    {
        isGameOver = true;
        Debug.Log("game over");
    }
}