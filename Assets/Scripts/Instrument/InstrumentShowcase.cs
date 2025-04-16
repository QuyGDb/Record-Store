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

    }
    private void OnEnable()
    {
        LoadTransform();
    }
    private void Start()
    {
        GameManager.Instance.OnApplicationStateChanged += OnApplicationStateChanged;
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

    public async void LoadTransform()
    {
        localTransfrom = ES3.Load<Transform>(instrumentShowcaseSO.instrumentName, transform);
        await Awaitable.NextFrameAsync();
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
