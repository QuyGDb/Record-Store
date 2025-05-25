using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ObjectParent : MonoBehaviour, INameable
{
    [SerializeField] private string objParentName;
    private ObjectSaver objectSaver;
    public Transform parentTransform;
    private bool isLoadedTransform = false;
    private void Awake()
    {
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
        if (state == ApplicationState.WallManager)
        {
            if (GameResources.Instance.currentwallManager == null) return;
            transform.SetParent(GameResources.Instance.currentwallManager.transform);
            if (!isLoadedTransform)
            {
                objectSaver.LoadTransform();
                isLoadedTransform = true;
            }
        }
        if (state == ApplicationState.ObjectManager)
        {
            objectSaver.SaveTransform();
        }
        if (state == ApplicationState.ObjectParent)
        {
            ToggleInteractableObjectParent(true);
            if (parentTransform == null)
            {
                parentTransform = transform.parent.parent;
            }
            transform.SetParent(parentTransform);
            transform.localScale = Vector3.one;
        }
        else
        {
            ToggleInteractableObjectParent(false);
        }
    }


    void ToggleInteractableObjectParent(bool isEnable)
    {
        GetComponent<XRGrabInteractable>().enabled = isEnable;
        GetComponent<Collider>().enabled = isEnable;
    }

    public string GetKey()
    {
        return objParentName;
    }
}
