using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnchorsManagerCanvas : MonoBehaviour
{
    private TMP_Dropdown dropdown;
    private AnchorsManager anchorsManager;
    private Button[] buttons;
    private void Awake()
    {
        dropdown = GetComponentInChildren<TMP_Dropdown>();
        buttons = GetComponentsInChildren<Button>();
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        StaticEventHandler.OnAnchorsManager += OnAnchorsManager;
    }
    private void OnDestroy()
    {
        dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
        StaticEventHandler.OnAnchorsManager -= OnAnchorsManager;
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
