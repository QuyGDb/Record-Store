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
        buttons[0].onClick.AddListener(MoveWallForward);
        buttons[1].onClick.AddListener(MoveWallBehind);
        buttons[2].onClick.AddListener(AddYRotate);
        buttons[3].onClick.AddListener(SubtractYRotate);
        buttons[4].onClick.AddListener(ShowObjects);
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
        buttons[0].onClick.RemoveListener(MoveWallForward);
        buttons[1].onClick.RemoveListener(MoveWallBehind);
        buttons[2].onClick.RemoveListener(AddYRotate);
        buttons[3].onClick.RemoveListener(SubtractYRotate);
        buttons[4].onClick.RemoveListener(ShowObjects);
    }
    void ShowObjects()
    {
        GameResources.Instance.instrumentShowcaseManager.ShowObjects();
        GameResources.Instance.pictureFrameManager.ShowObjects();
        ApplicationManager.Instance.ChangeApplicationState(ApplicationState.ObjectParent);
    }
    private void OnInputFieldEndEdit(string value)
    {
        if (float.TryParse(value, out float result))
        {
            GameResources.Instance.currentwallManager.StretchPlaneFromEdge(selectedEdge, result);
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

    private void MoveWallForward()
    {
        if (ApplicationManager.Instance.applicationState != ApplicationState.WallManager) return;
        if (GameResources.Instance.currentwallManager == null) return;
        GameResources.Instance.currentwallManager.transform.position += new Vector3(0, 0, 0.1f);


    }
    private void MoveWallBehind()
    {
        if (ApplicationManager.Instance.applicationState != ApplicationState.WallManager) return;
        if (GameResources.Instance.currentwallManager == null) return;
        GameResources.Instance.currentwallManager.transform.position -= new Vector3(0, 0, 0.1f);
    }

    private void AddYRotate()
    {
        if (GameResources.Instance.currentwallManager == null) return;
        if (ApplicationManager.Instance.applicationState != ApplicationState.WallManager) return;
        GameResources.Instance.currentwallManager.transform.Rotate(new Vector3(0, 1, 0), 5);
    }
    private void SubtractYRotate()
    {
        if (GameResources.Instance.currentwallManager == null) return;
        if (ApplicationManager.Instance.applicationState != ApplicationState.WallManager) return;
        GameResources.Instance.currentwallManager.transform.Rotate(new Vector3(0, -1, 0), 5);
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
