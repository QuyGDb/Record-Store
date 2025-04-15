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
        loadedTransform = ES3.Load(gameObject.name, transform);
        if (loadedTransform != null)
        {
            transform.localPosition = loadedTransform.localPosition;
            transform.localRotation = loadedTransform.localRotation;
            transform.localScale = loadedTransform.localScale;
        }
        GameManager.Instance.OnApplicationStateChanged += OnApplicationStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnApplicationStateChanged -= OnApplicationStateChanged;
    }

    private void OnApplicationStateChanged(ApplicationState state)
    {
        if (state == ApplicationState.LoadMapMode)
        {
            grabInteractable.enabled = false;
        }
    }
}
