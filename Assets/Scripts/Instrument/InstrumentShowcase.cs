using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class InstrumentShowcase : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    public InstrumentShowcaseSO instrumentShowcaseSO;
    private LocalAxis localAxis;
    private Transform localTransfrom;
    private void Awake()
    {
        localAxis = GetComponent<LocalAxis>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(Select);
        grabInteractable.selectExited.AddListener(Deselect);
        localTransfrom = ES3.Load(instrumentShowcaseSO.instrumentName, localTransfrom);

    }
    private void Start()
    {
        LoadTransform();
    }
    private void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(Select);
        }
    }
    public async void LoadTransform()
    {
        gameObject.SetActive(false);
        await Awaitable.NextFrameAsync();
        gameObject.SetActive(true);
        if (localTransfrom == null) return;
        gameObject.transform.localPosition = localTransfrom.localPosition;
        gameObject.transform.localRotation = localTransfrom.localRotation;
        gameObject.transform.localScale = localTransfrom.localScale;
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
