using System;
using UnityEngine;
using UnityEngine.UI;

public class HostAndReSolveAnchorCanvas : MonoBehaviour
{
    private Button[] buttons;
    private CloudAnchorsManager cloudAnchorsManager;
    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
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
        cloudAnchorsManager = manager;
        buttons[0].onClick.AddListener(() => cloudAnchorsManager.HostCurrentSelectAnchor());
        buttons[1].onClick.AddListener(() => cloudAnchorsManager.ResolveAllCloudAnchors());

    }
}
