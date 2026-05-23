using UnityEngine;
using UnityEngine.UI;

public class PlayButtonSound : MonoBehaviour
{
    [SerializeField] private string soundName;
    [SerializeField] bool isUISound = true;

    void Start()
    {
        Button btn = GetComponent<Button>();

        // Al pulsar el botón, ejecutamos la reproducción
        btn.onClick.AddListener(() => {
            if (AudioController.Instance != null)
            {
                if (isUISound)
                    AudioController.Instance.PlayUI(soundName);
                else
                    AudioController.Instance.PlaySFX(soundName);
            }
        });
    }
}
