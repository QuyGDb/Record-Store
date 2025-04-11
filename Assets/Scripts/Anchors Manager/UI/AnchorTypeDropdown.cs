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
            case AnchorType.IntrumentShowCase:
                anchorType = AnchorType.IntrumentShowCase;
                break;
            case AnchorType.Poster:
                anchorType = AnchorType.Poster;
                break;
            default:
                break;
        }
    }
}
