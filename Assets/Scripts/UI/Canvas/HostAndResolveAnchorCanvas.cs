using System;
using UnityEngine;
using UnityEngine.UI;

public class HostAndResolveAnchorCanvas : MonoBehaviour
{
    private Button[] buttons;
    private ScrollRect scrollRect;
    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        scrollRect = GetComponentInChildren<ScrollRect>();
        buttons[0].onClick.AddListener(ToggleScrollRect);
        StaticEventHandler.OnCloudAnchorsManager += OnCloudAnchorsManager;
    }

    private void OnDestroy()
    {
        StaticEventHandler.OnCloudAnchorsManager -= OnCloudAnchorsManager;
        buttons[0].onClick.RemoveAllListeners();
        buttons[1].onClick.RemoveAllListeners();
        buttons[2].onClick.RemoveAllListeners();
    }

    private void OnCloudAnchorsManager(CloudAnchorsManager manager)
    {
        buttons[1].onClick.AddListener(manager.ResolveSelectedCloudAnchor);
        buttons[2].onClick.AddListener(manager.RemoveCloudAnchorInAnchorDetails);
    }

    void ToggleScrollRect()
    {
        scrollRect.gameObject.SetActive(!scrollRect.gameObject.activeSelf);
    }
}
