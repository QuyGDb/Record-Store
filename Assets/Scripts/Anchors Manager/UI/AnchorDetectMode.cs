using UnityEngine;
using UnityEngine.UI;

public class AnchorDetectMode : MonoBehaviour
{
    private Button[] buttons;
    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
    }
    private void Start()
    {
        buttons[0].onClick.AddListener(ToggleARPlane);
        buttons[1].onClick.AddListener(ToggleARPointCloud);
        buttons[2].onClick.AddListener(ToggleAROcclusion);
    }
    private void OnDestroy()
    {
        buttons[0].onClick.RemoveListener(ToggleARPlane);
        buttons[1].onClick.RemoveListener(ToggleARPointCloud);
        buttons[2].onClick.RemoveListener(ToggleAROcclusion);
    }


    private void ToggleARPlane()
    {
        if (GameResources.Instance.anchorsManager != null)
        {
            GameResources.Instance.anchorsManager.arPlaneManager.enabled = !GameResources.Instance.anchorsManager.arPlaneManager.enabled;
        }

        foreach (var item in GameResources.Instance.anchorsManager.arPlaneManager.trackables)
        {
            item.gameObject.SetActive(!item.gameObject.activeSelf);
        }

    }

    private void ToggleARPointCloud()
    {
        if (GameResources.Instance.anchorsManager != null)
        {
            GameResources.Instance.anchorsManager.arPointCloudManager.enabled = !GameResources.Instance.anchorsManager.arPointCloudManager.enabled;
        }
        foreach (var item in GameResources.Instance.anchorsManager.arPointCloudManager.trackables)
        {
            item.gameObject.SetActive(!item.gameObject.activeSelf);
        }
    }
    private void ToggleAROcclusion()
    {
        if (GameResources.Instance.anchorsManager != null)
        {
            GameResources.Instance.occlusionManager.enabled = !GameResources.Instance.occlusionManager.enabled;
        }
    }
}
