using UnityEngine;
using UnityEngine.UI;

public class VolumeMusic : MonoBehaviour
{
    private Slider Musicslider;

    void Awake()
    {
        Musicslider = GetComponent<Slider>();
        Musicslider.onValueChanged.AddListener(OnChanged);
    }

    void Start()
    {
        if (AudioController.Instance != null)
            Musicslider.value = AudioController.Instance.musicSource.volume;
    }

    private void OnChanged(float value)
    {
        if (AudioController.Instance != null)
            AudioController.Instance.SetMusicVolume(value);
    }
}
