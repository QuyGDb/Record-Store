using System;
using UnityEngine;
using UnityEngine.UI;

public class AnchorsManagerCanvas : MonoBehaviour
{
    private Dropdown dropdown;
    private AnchorsManager anchorsManager;
    private Button[] buttons;
    private void Awake()
    {
        dropdown = GetComponent<Dropdown>();
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
    }

    private void OnDropdownValueChanged(int arg0)
    {
        anchorsManager.anchorAction = (AnchorAction)arg0;
    }
}
