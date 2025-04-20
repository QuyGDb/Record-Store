using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CloudAnchorLoadMapButton : MonoBehaviour
{
    private Button button;
    private void Awake()
    {
        button = GetComponentInChildren<Button>();
    }
    private void Start()
    {
        button.onClick.AddListener(GameResources.Instance.cloudAnchorsManager.ResolveSelectedCloudAnchor);
    }
    private void OnDestroy()
    {
        button.onClick.RemoveListener(GameResources.Instance.cloudAnchorsManager.ResolveSelectedCloudAnchor);
    }
}
