using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class AnchorsManagerCanvas : MonoBehaviour
{
    [SerializeField] private Button saveButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private TextMeshProUGUI anchorCanvasText;
    public InputActionReference touchPressAction;
    bool isHasAnchor = false;
    private void Awake()
    {
        touchPressAction.action.Enable();
        touchPressAction.action.started += OnTouchPress;
        GameResources.Instance.anchorSceneText = anchorCanvasText;
    }

    private void Start()
    {
        saveButton.onClick.AddListener(IsValidCloudAnchor);
        deleteButton.onClick.AddListener(() =>
        {
            var temp = GameResources.Instance.anchorsManager.DeleteAnchor();
            isHasAnchor = !temp;
        });

    }
    private void OnDestroy()
    {
        touchPressAction.action.started -= OnTouchPress;
        saveButton.onClick.RemoveListener(IsValidCloudAnchor);
        deleteButton.onClick.RemoveAllListeners();
    }

    private async void OnTouchPress(InputAction.CallbackContext context)
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        Vector2 touchPosition = context.ReadValue<Vector2>();
        if (!isHasAnchor && ApplicationManager.Instance.currentAppState == ApplicationState.Anchor)
        {
            isHasAnchor = await GameResources.Instance.anchorsManager.PlaceAnchor(touchPosition);
        }
    }

    void IsValidCloudAnchor()
    {
        if (!isHasAnchor) return;
        StaticEventHandler.InvokeHostCurrentSelectAnchor();
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
