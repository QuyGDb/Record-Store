using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Anchor : MonoBehaviour
{
    XRGrabInteractable grabInteractable;
    private SpriteRenderer spriteRenderer;
    bool isSelected = false;
    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        spriteRenderer = GetComponent<SpriteRenderer>();


    }
    private void Start()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.AddListener(OnSelect);
        }
    }

    private void OnSelect(SelectEnterEventArgs arg0)
    {
        isSelected = !isSelected;
        StaticEventHandler.InvokeAnchorChanged(this, isSelected);
        if (isSelected)
        {
            spriteRenderer.color = Color.green;
            StaticEventHandler.InvokeXRGrabInteractableSelected(gameObject);
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }
}
