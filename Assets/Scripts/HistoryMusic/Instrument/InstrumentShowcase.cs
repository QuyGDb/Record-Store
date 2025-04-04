using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class InstrumentShowcase : MonoBehaviour
{
    private XRGrabInteractable grabInteractable;
    public InstrumentShowcaseSO instrumentShowcaseSO;
    private LocalAxis localAxis;
    private LocalTransfrom localTransfrom;
    private void Awake()
    {
        localAxis = GetComponent<LocalAxis>();
        grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(Select);
        localTransfrom = ES3.Load(instrumentShowcaseSO.instrumentName, localTransfrom);

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
        LoadTransform();
    }

    public async void LoadTransform()
    {
        gameObject.SetActive(false);
        await Awaitable.WaitForSecondsAsync(3f);
        gameObject.SetActive(true);
        gameObject.transform.localPosition = localTransfrom.localPosition;
        gameObject.transform.localRotation = localTransfrom.localRotation;
        gameObject.transform.localScale = localTransfrom.localScale;
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
