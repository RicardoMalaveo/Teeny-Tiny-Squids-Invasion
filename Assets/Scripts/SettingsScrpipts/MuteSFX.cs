using UnityEngine;
using UnityEngine.UI;

public class MuteSFX : MonoBehaviour
{
    private Toggle _toggle;

    void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnChanged);
    }

    void Start()
    {
        if (AudioController.Instance != null)
            _toggle.isOn = !AudioController.Instance.SFXSource.mute;
    }

    private void OnChanged(bool isOn)
    {
        if (AudioController.Instance != null)
            AudioController.Instance.ToggleSFX(isOn);
    }
}
