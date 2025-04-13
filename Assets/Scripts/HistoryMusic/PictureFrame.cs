using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PictureFrame : MonoBehaviour
{
    public string pictureName;
    private Transform loadedTransform;
    private XRGrabInteractable grabInteractable;
    private void Awake()
    {
        loadedTransform = ES3.Load(pictureName, transform);
        if (loadedTransform != null)
        {
            transform.localPosition = loadedTransform.localPosition;
            transform.localRotation = loadedTransform.localRotation;
            transform.localScale = loadedTransform.localScale;
        }
        grabInteractable = GetComponent<XRGrabInteractable>();
    }
    private void Start()
    {
        grabInteractable.selectEntered.AddListener((temp) =>
        {
            StaticEventHandler.InvokeXRGrabInteractableSelected(gameObject);
        });
    }
    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener((temp) =>
        {
            StaticEventHandler.InvokeXRGrabInteractableSelected(gameObject);
        });
    }
}
