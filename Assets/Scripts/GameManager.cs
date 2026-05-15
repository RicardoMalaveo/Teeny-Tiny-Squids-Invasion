using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum GameState { Prep, Wave, Paused, GameOver }
    public GameState currentState;
    private GameState previousState;

    [SerializeField] private float totalPlayerHP;
    [SerializeField] private float currentPlayerHP;
    [SerializeField] private float initialPlayerSand;
    public float refundPercentage;
    public float CurrentSand { get; private set; }
    public bool isGameOver = false;

    private float savedTimeScale = 1f;
    private int currentWaveNumber = 1;
    [SerializeField] private float initialPrepTime;
    [SerializeField] private float timeBetweenWaves;
    private float currentCountdown;

    //references to UI info
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private TextMeshProUGUI sandText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI currentWave;
    [SerializeField] private TextMeshProUGUI gameState;


    [SerializeField] private GameObject timeScalex2; 
    [SerializeField] private GameObject timeScalex4; 
    [SerializeField] private GameObject timeScalex1; 

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
        UpdateSpeedButtons(1f);
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
        currentWaveNumber++;
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

    public void ToggleMenu()
    {
        if (currentState != GameState.Paused)
        {
            previousState = currentState;
            currentState = GameState.Paused;
            Time.timeScale = 0f;
            optionsPanel.SetActive(true);
        }
        else
        {
            currentState = previousState;
            Time.timeScale = savedTimeScale;
            optionsPanel.SetActive(false);
        }
    }

    public void ChangeGameSpeed(float newSpeed)
    {
        savedTimeScale = newSpeed;
        Time.timeScale = newSpeed;
        UpdateSpeedButtons(newSpeed);
    }

    private void UpdateSpeedButtons(float speed)
    {
        timeScalex1.SetActive(speed == 4f); 
        timeScalex2.SetActive(speed == 1f);
        timeScalex4.SetActive(speed == 2f);
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