using UnityEngine;
using UnityEngine.UI;

public class ShowObjectsButton : MonoBehaviour
{
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    private void Start()
    {
        button.onClick.AddListener(GameResources.Instance.instrumentShowcaseManager.ShowObjects);
        button.onClick.AddListener(GameResources.Instance.pictureFrameManager.ShowObjects);
    }
    private void OnDestroy()
    {
        button.onClick.RemoveListener(GameResources.Instance.instrumentShowcaseManager.ShowObjects);
        button.onClick.RemoveListener(GameResources.Instance.pictureFrameManager.ShowObjects);
    }
}
