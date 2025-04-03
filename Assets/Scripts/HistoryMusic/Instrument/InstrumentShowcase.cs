using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class InstrumentShowcase : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    public InstrumentShowcaseSO instrumentShowcaseSO;
    private LocalAxis localAxis;
    private Transform saveTransform;
    private void Awake()
    {
        localAxis = GetComponent<LocalAxis>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(Select);
        saveTransform = ES3.Load(instrumentShowcaseSO.instrumentName, transform);

    }

    private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(Select);
        }
    }
    private void Start()
    {
        if (saveTransform != null)
        {
            LoadTransform();
        }

    }

    private void LoadTransform()
    {
        gameObject.transform.position = saveTransform.position;
        gameObject.transform.rotation = saveTransform.rotation;
        gameObject.transform.localScale = saveTransform.localScale;
    }

    private void Select(SelectEnterEventArgs selectEnterEventArgs)
    {
        localAxis.enabled = true;
        StaticEventHandler.InvokeInstrumentShowcaseChanged(this);
        StaticEventHandler.InvokeRotateObjectChanged(this.transform);
    }
    public void Deselect()
    {
        localAxis.enabled = false;
    }
}
