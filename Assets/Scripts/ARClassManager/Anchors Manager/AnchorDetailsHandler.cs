using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnchorDetailsHandler : MonoBehaviour
{
    public AnchorType anchorType;
    private TMP_Dropdown anchorTypeDropdown;

    private void Awake()
    {
        anchorTypeDropdown = GetComponentInChildren<TMP_Dropdown>();
        anchorTypeDropdown.onValueChanged.AddListener(OnAnchorTypeChanged);
    }
    private void OnDestroy()
    {
        anchorTypeDropdown.onValueChanged.RemoveListener(OnAnchorTypeChanged);
    }
    private void OnAnchorTypeChanged(int value)
    {
        anchorType = (AnchorType)value;
        gameObject.layer = LayerMask.NameToLayer(anchorType.ToString());
    }
}
