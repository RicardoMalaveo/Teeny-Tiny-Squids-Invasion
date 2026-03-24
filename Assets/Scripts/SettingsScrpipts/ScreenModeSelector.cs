using UnityEngine;
using TMPro;

public class WindowModeSelector : MonoBehaviour
{
    private TMP_Dropdown _dropdown;

    void Awake()
    {
        _dropdown = GetComponent<TMP_Dropdown>();
        _dropdown.onValueChanged.AddListener(OnModeChanged);
    }

    private void OnModeChanged(int index)
    {
        if (SettingsController.Instance != null)
        {
            SettingsController.Instance.ChangeWindowMode(index);
        }
    }
}