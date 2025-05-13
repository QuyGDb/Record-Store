using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class VinylShowcase : MonoBehaviour, INameable
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
        ApplicationManager.Instance.OnApplicationStateChanged += OnApplicationStateChanged;
    }
    private void OnDestroy()
    {
        ApplicationManager.Instance.OnApplicationStateChanged -= OnApplicationStateChanged;
    }


    private void OnApplicationStateChanged(ApplicationState state)
    {

        if (state == ApplicationState.TestMap)
        {
            objectSaver.SaveTransform();
            grabInteractable.enabled = false;
        }
    }

    public string GetKey()
    {
        return gameObject.name;
    }
}
