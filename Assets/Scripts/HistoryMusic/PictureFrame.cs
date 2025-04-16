using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PictureFrame : MonoBehaviour
{
    public string pictureName;
    private Transform loadedTransform;
    private XRGrabInteractable grabInteractable;

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }
    private void OnEnable()
    {
        LoadTransform();
    }

    private async void LoadTransform()
    {
        loadedTransform = ES3.Load(pictureName, transform);
        if (loadedTransform != null)
        {
            await Awaitable.NextFrameAsync();
            transform.localPosition = loadedTransform.localPosition;
            transform.localRotation = loadedTransform.localRotation;
            transform.localScale = loadedTransform.localScale;
        }
    }
    private void Start()
    {
        grabInteractable.selectEntered.AddListener((temp) =>
        {
            StaticEventHandler.InvokeXRGrabInteractableSelected(gameObject);
        });
        GameManager.Instance.OnApplicationStateChanged += OnApplicationStateChanged;
    }
    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener((temp) =>
        {
            StaticEventHandler.InvokeXRGrabInteractableSelected(gameObject);
        });
        GameManager.Instance.OnApplicationStateChanged -= OnApplicationStateChanged;
    }

    private void OnApplicationStateChanged(ApplicationState state)
    {
        if (state == ApplicationState.LoadMapMode)
        {
            grabInteractable.enabled = false;
            GetComponentInChildren<Collider>().enabled = false;
        }
    }
}
