using System;
using TMPro;
using UnityEngine;

public class InstrumentDropdown : MonoBehaviour
{
    private TMP_Dropdown dropdown;

    private void Awake()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        dropdown.onValueChanged.AddListener(OnDropdownValueChanged);
    }
    private void OnDestroy()
    {
        dropdown.onValueChanged.RemoveListener(OnDropdownValueChanged);
    }
    private void Start()
    {
        StaticEventHandler.InvokeInstrumentShowCaseListVNChanged(GameResources.Instance.instrumentShowCaseVN);
    }
    private void OnDropdownValueChanged(int objects)
    {
        switch (dropdown.value)
        {
            case 0:
                StaticEventHandler.InvokeInstrumentShowCaseListVNChanged(GameResources.Instance.instrumentShowCaseVN);
                break;
            case 1:
                StaticEventHandler.InvokeInstrumentShowCaseListVNChanged(GameResources.Instance.instrumentShowCaseOversea);
                break;

        }
    }
}
