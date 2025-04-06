using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManagerCanvas : MonoBehaviour
{
    private Button[] buttons;
    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        StaticEventHandler.OnInstrumentsManagerChanged += OnInstrumentsManagerChanged;
    }
    private void OnDestroy()
    {
        StaticEventHandler.OnInstrumentsManagerChanged -= OnInstrumentsManagerChanged;
    }
    private void Start()
    {
    }
    private void OnInstrumentsManagerChanged(InstrumentsManager manager)
    {

        buttons[2].onClick.AddListener((manager.SaveObjectAtReleasePosition));
        buttons[3].onClick.AddListener((manager.DeteleCurrentSelectedObject));

    }




}
