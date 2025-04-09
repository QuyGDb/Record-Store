using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveObjectsButton : MonoBehaviour
{
    private Button button;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    private void Awake()
    {
        button = GetComponent<Button>();
        GameResources.Instance.objectSceneText = textMeshProUGUI;
    }
    private void OnDestroy()
    {
        button.onClick.RemoveListener((GameResources.Instance.transformObjectsManager.SaveTransformSelectInstrument));
    }
    private void Start()
    {
        button.onClick.AddListener((GameResources.Instance.transformObjectsManager.SaveTransformSelectInstrument));
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






