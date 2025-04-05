using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
public class ScaleWallCanvas : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI placeHolder;
    private TransformWallManager transformWallManager;
    private PlaneEdge selectedEdge;
    private Button[] buttons;
    public int OnTransformWallManager { get; private set; }

    private void Awake()
    {
        dropdown = GetComponentInChildren<TMP_Dropdown>();
        inputField = GetComponentInChildren<TMP_InputField>();
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        inputField.onEndEdit.AddListener(OnInputFieldEndEdit);
        StaticEventHandler.OnTransformWallManagerChanged += GetTransformWallManager;
        buttons = GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(AddZpositon);
        buttons[1].onClick.AddListener(SubtractZpositon);
    }

    private void GetTransformWallManager(TransformWallManager manager)
    {
        transformWallManager = manager;
        if (transformWallManager != null)
        {
            placeHolder.text = "Enter scale value";
        }

    }

    private void OnEnable()
    {

        if (inputField != null)
        {
            placeHolder.text = "Enter scale value";
        }
    }


    private void OnDestroy()
    {
        dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
        inputField.onEndEdit.RemoveListener(OnInputFieldEndEdit);
        StaticEventHandler.OnTransformWallManagerChanged -= GetTransformWallManager;
        buttons[0].onClick.RemoveListener(AddZpositon);
        buttons[1].onClick.RemoveListener(SubtractZpositon);
    }
    private void OnInputFieldEndEdit(string value)
    {
        if (float.TryParse(value, out float result))
        {
            transformWallManager.StretchPlaneFromEdge(selectedEdge, result);
        }
        else
        {
            placeHolder.text = "Invalid input";
        }
    }

    private void OnDropdownValueChanged(int value)
    {
        switch (value)
        {
            case 0:
                selectedEdge = PlaneEdge.Right;
                break;
            case 1:
                selectedEdge = PlaneEdge.Left;
                break;
            case 2:
                selectedEdge = PlaneEdge.Top;
                break;
            case 3:
                selectedEdge = PlaneEdge.Bottom;
                break;
        }
    }

    private void AddZpositon()
    {
        transform.position += new Vector3(0, 0, 0.01f);
    }
    private void SubtractZpositon()
    {
        transform.position -= new Vector3(0, 0, 0.01f);
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(placeHolder), placeHolder);
    }
#endif
    #endregion

}
