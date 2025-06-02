using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PictureFrame : MonoBehaviour, INameable
{
    public string pictureName;
    private XRGrabInteractable grabInteractable;
    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void Start()
    {
        grabInteractable.selectEntered.AddListener((temp) =>
        {
            StaticEventHandler.InvokeXRGrabInteractableSelected(gameObject);
        });
        if(ApplicationManager.Instance == null)
            return;
        ApplicationManager.Instance.OnApplicationStateChanged += OnApplicationStateChanged;
    }

    private void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener((temp) =>
        {
            StaticEventHandler.InvokeXRGrabInteractableSelected(gameObject);
        });
        if (ApplicationManager.Instance == null)
            return;
        ApplicationManager.Instance.OnApplicationStateChanged -= OnApplicationStateChanged;
    }


    private void OnApplicationStateChanged(ApplicationState state)
    {

        if (state == ApplicationState.ObjectManager)
        {
            ToggleInteractableItem(gameObject, true);
        }
        else
        {
            ToggleInteractableItem(gameObject, false);
        }
    }
    void ToggleInteractableItem(GameObject item, bool isEnabled)
    {
        item.GetComponent<XRGrabInteractable>().enabled = isEnabled;
        item.GetComponentInChildren<Collider>().enabled = isEnabled;
    }

    public string GetKey()
    {
        return pictureName;
    }
}
