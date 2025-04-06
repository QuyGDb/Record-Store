using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class AnchorsManagerCanvas : MonoBehaviour
{
    private AnchorTypeDropdown anchorTypeDropdown;
    private AnchorsManager anchorsManager;
    [HideInInspector] public TMP_InputField inputField;
    [SerializeField] private Button saveButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private TextMeshProUGUI warningText;
    public InputActionReference touchPressAction;
    bool isHasAnchor = false;
    private void Awake()
    {
        touchPressAction.action.Enable();
        touchPressAction.action.started += OnTouchPress;
        anchorTypeDropdown = GetComponentInChildren<AnchorTypeDropdown>();
        inputField = GetComponentInChildren<TMP_InputField>();
        StaticEventHandler.OnAnchorsManager += OnAnchorsManager;

    }

    private void Start()
    {
        saveButton.onClick.AddListener(IsValidCloudAnchor);
        deleteButton.onClick.AddListener(() =>
        {
            var temp = anchorsManager.DeleteAnchor();
            isHasAnchor = !temp;
        });

    }
    private void OnDestroy()
    {
        touchPressAction.action.started -= OnTouchPress;
        StaticEventHandler.OnAnchorsManager -= OnAnchorsManager;
        saveButton.onClick.RemoveListener(IsValidCloudAnchor);
        deleteButton.onClick.RemoveAllListeners();
    }

    private async void OnTouchPress(InputAction.CallbackContext context)
    {
        Vector2 touchPosition = context.ReadValue<Vector2>();
        if (!isHasAnchor)
        {
            isHasAnchor = await anchorsManager.PlaceAnchor(touchPosition);
        }
    }


    private void OnAnchorsManager(AnchorsManager manager)
    {
        anchorsManager = manager;
    }
    void IsValidCloudAnchor()
    {

        if (string.IsNullOrWhiteSpace(inputField.text) && anchorTypeDropdown.dropdown.value == 0)
        {
            warningText.text = "Please enter name";
        }
        else
        {
            StaticEventHandler.InvokeSendInfo(inputField.text, anchorTypeDropdown.anchorType);

            isHasAnchor = false;
        }
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(saveButton), saveButton);
        HelperUtilities.ValidateCheckNullValue(this, nameof(deleteButton), deleteButton);
    }
#endif
    #endregion
}
