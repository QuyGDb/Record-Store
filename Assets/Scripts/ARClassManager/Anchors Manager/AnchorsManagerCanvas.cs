using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnchorsManagerCanvas : MonoBehaviour
{
    private TMP_Dropdown dropdown;
    private AnchorsManager anchorsManager;
    private CloudAnchorsManager cloudAnchorsManager;
    private Button buttons;
    private void Awake()
    {
        dropdown = GetComponentInChildren<TMP_Dropdown>();
        buttons = GetComponentInChildren<Button>();
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        StaticEventHandler.OnAnchorsManager += OnAnchorsManager;
        StaticEventHandler.OnCloudAnchorsManager += OnCloudAnchorsManager;
    }
    private void OnDestroy()
    {
        dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
        buttons.onClick.RemoveAllListeners();
        StaticEventHandler.OnAnchorsManager -= OnAnchorsManager;
        StaticEventHandler.OnCloudAnchorsManager -= OnCloudAnchorsManager;

    }

    private void OnCloudAnchorsManager(CloudAnchorsManager manager)
    {
        cloudAnchorsManager = manager;
        buttons.onClick.AddListener(() => cloudAnchorsManager.HostCurrentSelectAnchor());
    }

    private void OnAnchorsManager(AnchorsManager anchorsManager)
    {
        this.anchorsManager = anchorsManager;
        anchorsManager.anchorAction = (AnchorAction)dropdown.value;
    }

    private void OnDropdownValueChanged(int arg0)
    {
        anchorsManager.anchorAction = (AnchorAction)arg0;
    }
}
