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
        ApplicationManager.Instance.OnApplicationStateChanged += OnApplicationStateChanged;
        objectSaver.LoadTransform(instrumentShowcaseSO.instrumentName);
        Debug.Log("InstrumentShowcaseSO name: " + instrumentShowcaseSO.instrumentName + Settings.es3Name);
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
}
