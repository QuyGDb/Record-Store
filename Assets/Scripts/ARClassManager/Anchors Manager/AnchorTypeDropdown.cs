using System.Linq;
using TMPro;
using UnityEngine;

public class AnchorTypeDropdown : MonoBehaviour
{
    [HideInInspector] public TMP_Dropdown dropdown;
    public AnchorType anchorType;
    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
    }
    void Start()
    {
        // Gán sự kiện cho dropdown
        dropdown.onValueChanged.AddListener(HandleDropdownChange);

        PopulateDropdown();
    }

    void PopulateDropdown()
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(System.Enum.GetNames(typeof(AnchorType)).ToList());
    }

    void HandleDropdownChange(int index)
    {
        AnchorType selectedOption = (AnchorType)index;

        switch (selectedOption)
        {
            case AnchorType.None:
                anchorType = AnchorType.None;
                break;
            case AnchorType.IntrumentShowCaseVN:
                anchorType = AnchorType.IntrumentShowCaseVN;
                break;
            case AnchorType.InstrumentShowCaseOversea:
                anchorType = AnchorType.InstrumentShowCaseOversea;
                break;
            case AnchorType.Poster:
                anchorType = AnchorType.Poster;
                break;
            default:
                break;
        }
    }
}
