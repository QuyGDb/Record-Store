using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InstrumentImage : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public Image image;
    private Color defaultColor;
    private Color selectedColor;
    bool isSelected = false;
    public InstrumentShowcaseSO instrumentShowcase;
    public void Awake()
    {
        image = GetComponent<Image>();
        defaultColor = image.color;
        selectedColor = new Color(0.5f, 0.5f, 0.5f, 1);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        isSelected = !isSelected;
        if (isSelected)
        {
            StaticEventHandler.InvokeInstrumentShowcaseSOSelected(instrumentShowcase, isSelected);
            Debug.Log(instrumentShowcase);
            image.color = selectedColor;
        }
        else
        {
            StaticEventHandler.InvokeInstrumentShowcaseSOSelected(instrumentShowcase, isSelected);
            image.color = defaultColor;
        }
    }


}
