using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManagerCanvas : MonoBehaviour
{
    private Button[] buttons;
    private TMP_Dropdown musicObjectsDropdown;
    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        musicObjectsDropdown = GetComponentInChildren<TMP_Dropdown>();
        StaticEventHandler.OnInstrumentsManagerChanged += OnInstrumentsManagerChanged;
    }
    private void OnDestroy()
    {
        StaticEventHandler.OnInstrumentsManagerChanged -= OnInstrumentsManagerChanged;
    }
    private void Start()
    {
        musicObjectsDropdown.gameObject.SetActive(false);
    }
    private void OnInstrumentsManagerChanged(InstrumentsManager manager)
    {

        buttons[2].onClick.AddListener((manager.SaveObjectAtReleasePosition));
        buttons[3].onClick.AddListener((manager.DeteleCurrentSelectedObject));

    }




}
