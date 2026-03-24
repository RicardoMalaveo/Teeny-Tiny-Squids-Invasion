using UnityEngine;
using UnityEngine.UI;

public class VolumeUI : MonoBehaviour
{
    private Slider UIslider;

    void Awake()
    {
        UIslider = GetComponent<Slider>();
        UIslider.onValueChanged.AddListener(OnChanged);
    }

    void Start()
    {
        if (AudioController.Instance != null)
            UIslider.value = AudioController.Instance.UISource.volume;
    }

    private void OnChanged(float value)
    {
        if (AudioController.Instance != null)
            AudioController.Instance.SetUIVolume(value);
    }
}
