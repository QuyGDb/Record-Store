using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VinylShowcase : MonoBehaviour
{
    XRGrabInteractable grabInteractable;
    ObjectSaver objectSaver;
    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        objectSaver = GetComponent<ObjectSaver>();

    }
    private void Start()
    {
        objectSaver.LoadTransform(gameObject.name);
        GameManager.Instance.OnApplicationStateChanged += OnApplicationStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnApplicationStateChanged -= OnApplicationStateChanged;
    }


    private void OnApplicationStateChanged(ApplicationState state)
    {
        if (state == ApplicationState.ObjectManager)
        {
            objectSaver.SaveTransform(gameObject.name);
        }

        if (state == ApplicationState.LoadMapMode)
        {
            grabInteractable.enabled = false;
        }
    }
}
