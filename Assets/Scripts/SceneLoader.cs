using UnityEngine;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneNameToLoad;

    private Button myButton;

    private void Awake()
    {
        myButton = GetComponent<Button>();

        myButton.onClick.AddListener(() => {
            if (GameSceneManager.Instance != null)
            {
                GameSceneManager.Instance.LoadScene(sceneNameToLoad);
            }
        });
    }
}
