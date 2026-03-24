using UnityEngine;
using UnityEngine.UI;

public class VolumeSFX : MonoBehaviour
{
    private Slider SFXslider;

    void Awake()
    {
        SFXslider = GetComponent<Slider>();
        SFXslider.onValueChanged.AddListener(OnChanged);
    }

    void Start()
    {
        if (AudioController.Instance != null)
            SFXslider.value = AudioController.Instance.SFXSource.volume;
    }

    private void OnChanged(float value)
    {
        if (AudioController.Instance != null)
            AudioController.Instance.SetSFXVolume(value);
    }
}
