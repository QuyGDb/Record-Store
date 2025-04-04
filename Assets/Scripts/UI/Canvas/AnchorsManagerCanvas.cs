using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using static UnityEngine.Rendering.DebugUI;

public class AnchorsManagerCanvas : MonoBehaviour
{
    private TMP_Dropdown anchorActionDropdown;
    private AnchorTypeDropdown anchorTypeDropdown;
    private AnchorsManager anchorsManager;
    [SerializeField] private ARPlaneManager arPlaneManager;
    [HideInInspector] public TMP_InputField inputField;
    [SerializeField] private UnityEngine.UI.Button saveButtons;
    [SerializeField] private TextMeshProUGUI warningText;
    bool isPlanesActive = false;
    private void Awake()
    {
        anchorActionDropdown = GetComponentInChildren<TMP_Dropdown>();
        anchorTypeDropdown = GetComponentInChildren<AnchorTypeDropdown>();
        inputField = GetComponentInChildren<TMP_InputField>();
        anchorActionDropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        StaticEventHandler.OnAnchorsManager += OnAnchorsManager;
        StaticEventHandler.OnMainDropdownChanged += OnMainDropdownChanged;
        saveButtons.onClick.AddListener(IsVaildCloudAnchor);

    }
    private void OnDestroy()
    {
        anchorActionDropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
        saveButtons.onClick.RemoveAllListeners();
        StaticEventHandler.OnAnchorsManager -= OnAnchorsManager;
        StaticEventHandler.OnMainDropdownChanged -= OnMainDropdownChanged;
        saveButtons.onClick.RemoveListener(IsVaildCloudAnchor);
    }

    private void OnMainDropdownChanged(int index)
    {
        if (index != 0)
        {
            anchorActionDropdown.value = 0;
        }
    }

    void IsVaildCloudAnchor()
    {

        if (string.IsNullOrWhiteSpace(inputField.text) && anchorTypeDropdown.dropdown.value == 0)
        {
            warningText.text = "Vui lòng nhập dữ liệu!";
        }
        else
        {
            StaticEventHandler.InvokeSendInfo(inputField.text, anchorTypeDropdown.anchorType);
        }
    }
    private void OnAnchorsManager(AnchorsManager anchorsManager)
    {
        this.anchorsManager = anchorsManager;
        anchorsManager.anchorAction = (AnchorAction)anchorActionDropdown.value;
    }

    private void OnDropdownValueChanged(int value)
    {
        anchorsManager.anchorAction = (AnchorAction)value;

        if (value == 3)
        {
            anchorsManager.DeleteAnchor();
        }
        if (value == 4)
        {
            foreach (var plane in arPlaneManager.trackables)
            {
                plane.gameObject.SetActive(isPlanesActive);
            }
            isPlanesActive = !isPlanesActive;
        }
    }

}
