using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ResolutionChanger : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(OnResolutionChanged);
    }

    private void OnResolutionChanged(int index)
    {
        if (SettingsController.Instance != null)
        {
            SettingsController.Instance.ApplyResolution(index);
        }
        else
        {
            Debug.LogWarning("No SettingsController in scene");
        }
    }
}
