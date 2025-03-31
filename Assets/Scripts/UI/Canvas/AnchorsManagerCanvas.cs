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
        saveButtons.onClick.AddListener(IsVaildCloudAnchor);
    }
    private void OnDestroy()
    {
        dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
        saveButtons.onClick.RemoveAllListeners();
        StaticEventHandler.OnAnchorsManager -= OnAnchorsManager;
        saveButtons.onClick.RemoveListener(IsVaildCloudAnchor);
    }


    void IsVaildCloudAnchor()
    {

        if (string.IsNullOrWhiteSpace(inputFields[0].text) && string.IsNullOrWhiteSpace(inputFields[1].text))
        {

            warningText.text = "Vui lòng nhập dữ liệu!";
        }
        else
        {
            StaticEventHandler.InvokeSendInfo(inputFields[0].text, inputFields[1].text);
            inputFields[0].text = "";
            inputFields[1].text = "";
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
