using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class AnchorsManagerCanvas : MonoBehaviour
{
    private TMP_Dropdown dropdown;
    private AnchorsManager anchorsManager;
    private CloudAnchorsManager cloudAnchorsManager;
    public InputField[] inputFields;
    private UnityEngine.UI.Button buttons;
    private TextMeshProUGUI warningText;
    private void Awake()
    {
        dropdown = GetComponentInChildren<TMP_Dropdown>();
        buttons = GetComponentInChildren<UnityEngine.UI.Button>();
        inputFields = GetComponentsInChildren<InputField>();
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        StaticEventHandler.OnAnchorsManager += OnAnchorsManager;
        StaticEventHandler.OnCloudAnchorsManager += OnCloudAnchorsManager;
        StaticEventHandler.OnSendAnchorInfo += OnSendIAnchornfo;
    }
    private void OnDestroy()
    {
        dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
        buttons.onClick.RemoveAllListeners();
        StaticEventHandler.OnAnchorsManager -= OnAnchorsManager;
        StaticEventHandler.OnCloudAnchorsManager -= OnCloudAnchorsManager;
        StaticEventHandler.OnSendAnchorInfo -= OnSendIAnchornfo;

    }

    private void OnSendIAnchornfo(string arg1, string arg2)
    {

    }

    private void OnCloudAnchorsManager(CloudAnchorsManager manager)
    {
        cloudAnchorsManager = manager;
        buttons.onClick.AddListener(() =>
        {
            if (IsVaildCloudAnchor())
                cloudAnchorsManager.HostCurrentSelectAnchor();
        });

    }
    bool IsVaildCloudAnchor()
    {

        if (string.IsNullOrWhiteSpace(inputFields[0].text) && string.IsNullOrWhiteSpace(inputFields[1].text))
        {
            warningText.text = "Vui lòng nhập dữ liệu!";
            warningText.gameObject.SetActive(true);
            return false;
        }
        else
        {
            warningText.gameObject.SetActive(false);
            return true;

        }
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
