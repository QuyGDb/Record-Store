using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class InstrumentShowcase : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    public InstrumentShowcaseSO instrumentShowcaseSO;
    private LocalAxis localAxis;
    ObjectSaver objectSaver;
    private void Awake()
    {
        localAxis = GetComponent<LocalAxis>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        objectSaver = GetComponent<ObjectSaver>();
        grabInteractable.selectEntered.AddListener(Select);
        grabInteractable.selectExited.AddListener(Deselect);

    }

    private void Start()
    {
        GameManager.Instance.OnApplicationStateChanged += OnApplicationStateChanged;
        objectSaver.LoadTransform(instrumentShowcaseSO.instrumentName);
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnApplicationStateChanged -= OnApplicationStateChanged;
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(Select);
        }
    }

    private void OnApplicationStateChanged(ApplicationState state)
    {
        if (state == ApplicationState.LoadMapMode)
        {
            grabInteractable.enabled = false;
            GetComponentInChildren<Collider>().enabled = false;
        }
    }
    private void Select(SelectEnterEventArgs selectEnterEventArgs)
    {
        localAxis.enabled = true;
        StaticEventHandler.InvokeInstrumentShowcaseChanged(this);
        StaticEventHandler.InvokeXRGrabInteractableSelected(gameObject);
    }
    private void Deselect(SelectExitEventArgs arg0)
    {
        localAxis.enabled = false;
    }
}
