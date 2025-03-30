using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class AnchorsManagerCanvas : MonoBehaviour
{
    private TMP_Dropdown dropdown;
    private AnchorsManager anchorsManager;
    [SerializeField] public TMP_InputField[] inputFields;
    [SerializeField] private UnityEngine.UI.Button saveButtons;
    [SerializeField] private TextMeshProUGUI warningText;
    private void Awake()
    {
        dropdown = GetComponentInChildren<TMP_Dropdown>();
        inputFields = GetComponentsInChildren<TMP_InputField>();
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        StaticEventHandler.OnAnchorsManager += OnAnchorsManager;
        StaticEventHandler.OnCloudAnchorsManager += OnCloudAnchorsManager;
    }
    private void OnDestroy()
    {
        dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
        saveButtons.onClick.RemoveAllListeners();
        StaticEventHandler.OnAnchorsManager -= OnAnchorsManager;
        StaticEventHandler.OnCloudAnchorsManager -= OnCloudAnchorsManager;
        saveButtons.onClick.RemoveListener(IsVaildCloudAnchor);
    }


    private void OnCloudAnchorsManager(CloudAnchorsManager manager)
    {
        saveButtons.onClick.AddListener(IsVaildCloudAnchor);
    }

    void IsVaildCloudAnchor()
    {
        if (string.IsNullOrWhiteSpace(inputFields[0].text) && string.IsNullOrWhiteSpace(inputFields[1].text))
        {
            warningText.text = "Vui lòng nhập dữ liệu!";
        }
        else
        {
            inputFields[0].text = "";
            inputFields[1].text = "";
            StaticEventHandler.InvokeSendInfo(inputFields[0].text, inputFields[1].text);

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
