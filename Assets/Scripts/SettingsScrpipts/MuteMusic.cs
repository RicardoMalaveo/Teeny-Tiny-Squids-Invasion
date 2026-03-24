using UnityEngine;
using UnityEngine.UI;

public class MuteMusic : MonoBehaviour
{
    private Toggle _toggle;

    void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnChanged);
    }

    void Start()
    {
        // Sincroniza el estado visual al cargar la escena
        if (AudioController.Instance != null)
            _toggle.isOn = !AudioController.Instance.musicSource.mute;
    }

    private void OnChanged(bool isOn)
    {
        if (AudioController.Instance != null)
            AudioController.Instance.ToggleMusic(isOn);
    }
}
