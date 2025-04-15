using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
public class WallManagerCanvas : MonoBehaviour
{
    private TMP_Dropdown dropdown;
    private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI placeHolder;
    [SerializeField] private PlaneEdge selectedEdge;
    private Button[] buttons;

    private void Awake()
    {
        dropdown = GetComponentInChildren<TMP_Dropdown>();
        inputField = GetComponentInChildren<TMP_InputField>();
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
        inputField.onEndEdit.AddListener(OnInputFieldEndEdit);
        buttons = GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(AddZpositon);
        buttons[1].onClick.AddListener(SubtractZpositon);
        buttons[2].onClick.AddListener(AddYRotate);
        buttons[3].onClick.AddListener(SubtractYRotate);
        buttons[4].onClick.AddListener(GameResources.Instance.objectSpawnerAtAnchors.InitializeObjects);
    }

    private void OnEnable()
    {
        if (inputField != null)
        {
            placeHolder.text = "Enter scale value";
        }
    }
    private void Start()
    {
        selectedEdge = PlaneEdge.Left;
    }

    private void OnDestroy()
    {
        dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
        inputField.onEndEdit.RemoveListener(OnInputFieldEndEdit);
        buttons[0].onClick.RemoveListener(AddZpositon);
        buttons[1].onClick.RemoveListener(SubtractZpositon);
        buttons[2].onClick.RemoveListener(AddYRotate);
        buttons[3].onClick.RemoveListener(SubtractYRotate);
        buttons[4].onClick.RemoveListener(GameResources.Instance.objectSpawnerAtAnchors.InitializeObjects);
    }
    private void OnInputFieldEndEdit(string value)
    {
        if (float.TryParse(value, out float result))
        {
            GameResources.Instance.wallManager.StretchPlaneFromEdge(selectedEdge, result);
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
                selectedEdge = PlaneEdge.Left;
                break;
            case 1:
                selectedEdge = PlaneEdge.Right;
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
        if (GameResources.Instance.wallManager == null) return;
        GameResources.Instance.wallManager.transform.position += new Vector3(0, 0, 0.1f);
    }
    private void SubtractZpositon()
    {
        if (GameResources.Instance.wallManager == null) return;
        GameResources.Instance.wallManager.transform.position -= new Vector3(0, 0, 0.1f);
    }

    private void AddYRotate()
    {
        if (GameResources.Instance.wallManager == null) return;
        GameResources.Instance.wallManager.transform.Rotate(new Vector3(0, 1, 0), 1);
    }
    private void SubtractYRotate()
    {
        if (GameResources.Instance.wallManager == null) return;
        GameResources.Instance.wallManager.transform.Rotate(new Vector3(0, -1, 0), 1);
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
