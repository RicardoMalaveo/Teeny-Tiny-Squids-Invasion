using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public enum GameState { Prep, Wave, Paused, Victory, Defeat }
    public GameState currentState;
    private GameState previousState;

    [SerializeField] private float totalPlayerHP;
    [SerializeField] private float currentPlayerHP;
    [SerializeField] private float initialPlayerSand;
    public float refundPercentage;
    public float CurrentSand { get; private set; }

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
    [SerializeField] private TextMeshProUGUI tideStateText;

    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;

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
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
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
        if (currentState == GameState.Defeat || currentState == GameState.Victory || currentState == GameState.Paused) return;

        if (currentState == GameState.Prep)
        {
            currentCountdown -= Time.deltaTime;
            UpdateTimerUI();

            if (currentCountdown <= 0)
            {
                StartWave();
            }
        }
        else if (currentState == GameState.Wave)
        {
            timerText.text = "combate";
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
        if (currentState == GameState.Defeat || currentState == GameState.Victory) return;
        ChangeGameSpeed(1f);
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
        if (currentState == GameState.Defeat || currentState == GameState.Victory) return;

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
        if (currentState == GameState.Defeat || currentState == GameState.Victory) return;

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
    public void UpdateTideStatusUI(WaveData upcomingWave)
    {

        if (upcomingWave.executeHighTide)
        {
            tideStateText.text = "CAMPO DE BATALLA INUNDADO";
            tideStateText.color = new Color(0.5f, 0f, 0.5f);
        }
        else if (upcomingWave.startTideWarning)
        {
            tideStateText.text = "MAREA CRECIENTE";
            tideStateText.color = new Color(1f, 0.5f, 0f);
        }
        else
        {
            tideStateText.text = "MAREA BAJA";
            tideStateText.color = Color.cyan;
        }
    }
    private void UpdateUI()
    {
        sandText.text = "Arena: " + CurrentSand;
        hpText.text = "Vida del castillo: "+ currentPlayerHP;
        currentWave.text = "Oleada Enemiga: " + currentWaveNumber;

        switch (currentState)
        {
            case GameState.Prep:
                gameState.text = "Preparando Oleada";
                gameState.color = Color.blue;
                break;
            case GameState.Wave:
                gameState.text = "¡Oleada Enemiga!";
                gameState.color = Color.red;
                break;
            case GameState.Victory:
                gameState.text = "¡Victoria!";
                gameState.color = Color.green;
                break;
            case GameState.Defeat:
                gameState.text = "¡Derrota!";
                gameState.color = Color.black;
                break;
        }
    }

    private void LoseGame()
    {
        currentState = GameState.Defeat;
        Time.timeScale = 0f; 

        defeatPanel.SetActive(true);
        UpdateUI();
        Debug.Log("Game Over");
    }

    public void WinGame()
    {
        currentState = GameState.Victory;
        Time.timeScale = 0f;

        victoryPanel.SetActive(true);
        UpdateUI();
        Debug.Log("Victoria");
    }
}