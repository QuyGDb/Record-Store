using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VinylShowcase : MonoBehaviour
{
    XRGrabInteractable grabInteractable;
    Transform loadedTransform;
    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

    }
    private void Start()
    {
        LoadTransform();
        GameManager.Instance.OnApplicationStateChanged += OnApplicationStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnApplicationStateChanged -= OnApplicationStateChanged;
    }
    async void LoadTransform()
    {
        await Awaitable.NextFrameAsync();
        if (ES3.KeyExists(gameObject.name))
        {
            loadedTransform = ES3.Load(gameObject.name, transform);
        }
        if (loadedTransform != null)
        {
            transform.localPosition = loadedTransform.localPosition;
            transform.localRotation = loadedTransform.localRotation;
            transform.localScale = loadedTransform.localScale;
        }
    }

    private void OnApplicationStateChanged(ApplicationState state)
    {
        if (state == ApplicationState.ObjectManager)
        {
            ES3.Save(gameObject.name, transform);
        }

        if (state == ApplicationState.LoadMapMode)
        {
            grabInteractable.enabled = false;
        }
    }
}
