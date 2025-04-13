using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveObjectsTransformManager : MonoBehaviour
{
    private Button[] buttons;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        GameResources.Instance.objectSceneText = textMeshProUGUI;
    }

    private void Start()
    {
        buttons[0].onClick.AddListener((GameResources.Instance.transformObjectsManager.SaveSelectedItemTransformWithIdentifier));
        buttons[1].onClick.AddListener(() => GameManager.Instance.ChangeApplicationState(ApplicationState.LoadMapMode));
    }
    private void OnDestroy()
    {
        buttons[0].onClick.RemoveListener((GameResources.Instance.transformObjectsManager.SaveSelectedItemTransformWithIdentifier));
        buttons[1].onClick.RemoveListener(() => GameManager.Instance.ChangeApplicationState(ApplicationState.LoadMapMode));
    }

    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(textMeshProUGUI), textMeshProUGUI);
    }
#endif
    #endregion
}






