using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum GameState { Prep, Wave, GameOver }
    public GameState currentState;

    [SerializeField] private int initialPlayerSand;
    public float refundPercentage;
    public int CurrentSand { get; private set; }


    [SerializeField] private int totalPlayerHP;
    private int currentPlayerHP;
    [SerializeField] private int CurrentWave;
    public bool isGameOver = false;

    [SerializeField] private float initialPrepTime;
    [SerializeField] private float timeBetweenWaves;
    private float currentCountdown;

    //references to UI info
    [SerializeField] private TextMeshProUGUI sandText;
    [SerializeField] private TextMeshProUGUI timerText;

    private void Awake()
    {

        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        CurrentSand = initialPlayerSand;
        currentPlayerHP = totalPlayerHP;
        UpdateSandUI();
    }

    private void Start()
    {
        StartCountdown(initialPrepTime);
    }
    private void Update()
    {
        if (isGameOver) return;

        if (currentState == GameState.Prep)
        {
            HandleCountdown();
        }
    }

    private void HandleCountdown()
    {
        currentCountdown -= Time.deltaTime;
        UpdateTimerUI();

        if (currentCountdown <= 0)
        {
            StartWave();
        }
    }
    public void StartCountdown(float seconds)
    {
        currentState = GameState.Prep;
        currentCountdown = seconds;
    }
    public void SkipCountdown()
    {
        if (currentState == GameState.Prep) StartWave();
    }
    public void StartWave()
    {
        if (currentState == GameState.Wave) return;

        currentState = GameState.Wave;
        currentCountdown = 0;
        UpdateTimerUI();

        WaveManager.Instance.StartNextWave();
    }

    public void OnWaveExtinction()
    {
        if (isGameOver) return;
        StartCountdown(timeBetweenWaves);
    }
    private void UpdateTimerUI()
    {
        float t = Mathf.Max(0, currentCountdown);
        int minutes = Mathf.FloorToInt(t / 60);
        int seconds = Mathf.FloorToInt(t % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void DamagePlayer(int amount)
    {
        if (isGameOver)
        {
            return;
        }

        currentPlayerHP -= amount;
        Debug.Log("Castle HP: " + currentPlayerHP);

        if (currentPlayerHP <= 0)
        {
            LoseGame();
        }
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

    private void LoseGame()
    {
        isGameOver = true;
        Debug.Log("game over");
    }
}
