using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public void LoadMainMenu()
    {
        // Ahora no hay conflicto: SceneController (tú) vs SceneManager (Unity)
        SceneManager.LoadScene(1);
    }
    public void LoadInitialScene()
    {
        // Ahora no hay conflicto: SceneController (tú) vs SceneManager (Unity)
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Exiting game...");
        Application.Quit();
    }
}