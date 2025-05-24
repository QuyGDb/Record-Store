using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

[RequireComponent(typeof(XRGrabInteractable))]
[RequireComponent(typeof(XRGeneralGrabTransformer))]
[RequireComponent(typeof(LocalAxis))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ObjectSaver))]
public class InstrumentShowcase : MonoBehaviour, INameable
{
    private XRGrabInteractable grabInteractable;
    public InstrumentShowcaseSO instrumentShowcaseSO;
    private LocalAxis localAxis;
    private void Awake()
    {
        localAxis = GetComponent<LocalAxis>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(Select);
        grabInteractable.selectExited.AddListener(Deselect);
    }

    private void Start()
    {
        ApplicationManager.Instance.OnApplicationStateChanged += OnApplicationStateChanged;

    }
    private void OnDestroy()
    {
        ApplicationManager.Instance.OnApplicationStateChanged -= OnApplicationStateChanged;
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(Select);
        }
    }

    private void OnApplicationStateChanged(ApplicationState state)
    {

        if (state == ApplicationState.ObjectManager)
        {
            ToggleInteractableItem(gameObject, true);
        }
        else
        {
            ToggleInteractableItem(gameObject, false);
            StaticEventHandler.InvokeXRGrabInteractableSelected(null);
        }
    }
    void ToggleInteractableItem(GameObject item, bool isEnabled)
    {
        item.GetComponent<XRGrabInteractable>().enabled = isEnabled;
        item.GetComponentInChildren<Collider>().enabled = isEnabled;
    }
    private void Select(SelectEnterEventArgs selectEnterEventArgs)
    {
        localAxis.enabled = true;
        StaticEventHandler.InvokeXRGrabInteractableSelected(gameObject);
    }
    private void Deselect(SelectExitEventArgs arg0)
    {
        localAxis.enabled = false;
    }

    public string GetKey()
    {
        return instrumentShowcaseSO.instrumentName;
    }
}
