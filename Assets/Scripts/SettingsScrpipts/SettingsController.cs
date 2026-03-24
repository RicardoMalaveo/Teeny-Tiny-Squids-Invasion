using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public static SettingsController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ChangeWindowMode(int index)
    {
        switch (index)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                Debug.Log("Full Screen Mode");
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                Debug.Log("Borderless Window Mode");
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                Debug.Log("Windowed Mode");
                break;
        }
    }
    public void ApplyResolution(int index)
    {
        FullScreenMode currentMode = Screen.fullScreenMode;

        switch (index)
        {
            case 0:
                Screen.SetResolution(1366, 768, currentMode);
                Debug.Log("Resolution set to: 1366 X 768");
                break;
            case 1:
                Screen.SetResolution(1920, 1080, currentMode);
                Debug.Log("Resolution set to: 1920 X 1080");
                break;
            case 2:
                Screen.SetResolution(2560, 1440, currentMode);
                Debug.Log("Resolution set to: 2560 X 1440");
                break;
            case 3:
                Screen.SetResolution(3840, 2160, currentMode);
                Debug.Log("Resolution set to: 3840 X 2160");
                break;
        }
    }
}

