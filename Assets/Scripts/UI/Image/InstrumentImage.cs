using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InstrumentImage : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public Image instrumentImage;
    private Color defaultColor;
    private Color selectedColor;
    bool isSelected = false;
    [HideInInspector] public InstrumentDetails instrmentDetails;
    public void Awake()
    {
        instrumentImage = GetComponent<Image>();
        defaultColor = instrumentImage.color;
        selectedColor = new Color(0.5f, 0.5f, 0.5f, 1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isSelected = !isSelected;
        if (isSelected)
        {
            StaticEventHandler.InvokeInstrumentSelected(instrmentDetails, isSelected);
            instrumentImage.color = selectedColor;
        }
        else
        {
            StaticEventHandler.InvokeInstrumentSelected(instrmentDetails, isSelected);
            instrumentImage.color = defaultColor;
        }
    }


}
