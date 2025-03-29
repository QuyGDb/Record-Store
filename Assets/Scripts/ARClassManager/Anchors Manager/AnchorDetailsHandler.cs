using UnityEngine;
using UnityEngine.UI;

public class AnchorDetailsHandler : MonoBehaviour
{
    public AnchorType anchorType;
    private Dropdown anchorTypeDropdown;

    private void Awake()
    {
        anchorTypeDropdown = GetComponentInChildren<Dropdown>();
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
