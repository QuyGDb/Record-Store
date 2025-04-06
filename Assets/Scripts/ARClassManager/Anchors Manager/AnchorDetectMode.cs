using UnityEngine;
using UnityEngine.UI;

public class AnchorDetectMode : MonoBehaviour
{
    private AnchorsManager anchorManager;

    private Button[] buttons;
    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        StaticEventHandler.OnAnchorsManager += OnAnchorsManager;
    }
    private void Start()
    {
        buttons[0].onClick.AddListener(ToggleARPlane);
        buttons[1].onClick.AddListener(ToggleARPointCloud);
    }
    private void OnDestroy()
    {
        StaticEventHandler.OnAnchorsManager -= OnAnchorsManager;
        buttons[0].onClick.RemoveListener(ToggleARPlane);
        buttons[1].onClick.RemoveListener(ToggleARPointCloud);
    }


    private void OnAnchorsManager(AnchorsManager anchorManager)
    {
        if (anchorManager != null)
        {
            this.anchorManager = anchorManager;
        }
    }
    private void ToggleARPlane()
    {
        if (anchorManager != null)
        {
            anchorManager.arPlaneManager.enabled = !anchorManager.arPlaneManager.enabled;
        }

        foreach (var item in anchorManager.arPlaneManager.trackables)
        {
            item.gameObject.SetActive(!item.gameObject.activeSelf);
        }

    }

    private void ToggleARPointCloud()
    {
        if (anchorManager != null)
        {
            anchorManager.arPointCloudManager.enabled = !anchorManager.arPointCloudManager.enabled;
        }
        foreach (var item in anchorManager.arPointCloudManager.trackables)
        {
            item.gameObject.SetActive(!item.gameObject.activeSelf);
        }
    }
}
