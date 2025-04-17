using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PictureFrame : MonoBehaviour
{
    public string pictureName;
    private XRGrabInteractable grabInteractable;
    private ObjectSaver objectSaver;
    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        objectSaver = GetComponent<ObjectSaver>();
    }



    private void Start()
    {
        grabInteractable.selectEntered.AddListener((temp) =>
        {
            StaticEventHandler.InvokeXRGrabInteractableSelected(gameObject);
        });
        GameManager.Instance.OnApplicationStateChanged += OnApplicationStateChanged;
        objectSaver.LoadTransform(pictureName);
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
