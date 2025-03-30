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
    }

    private void OnEnable()
    {
        StaticEventHandler.OnCloudAnchorsManager += OnCloudAnchorsManager;

    }
    private void OnDisable()
    {
        StaticEventHandler.OnCloudAnchorsManager -= OnCloudAnchorsManager;
        buttons[0].onClick.RemoveAllListeners();
        buttons[1].onClick.RemoveAllListeners();
    }

    private void OnCloudAnchorsManager(CloudAnchorsManager manager)
    {
        buttons[0].onClick.AddListener(ToggleScrollRec);
        buttons[1].onClick.AddListener(manager.ResolveSelectedCloudAnchor);
        buttons[2].onClick.AddListener(manager.RemoveCloudAnchorInAnchorDetails);
    }

    void ToggleScrollRec()
    {
        scrollRect.gameObject.SetActive(!scrollRect.gameObject.activeSelf);
    }
}
