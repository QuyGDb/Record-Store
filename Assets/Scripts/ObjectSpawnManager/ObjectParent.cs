using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ObjectParent : MonoBehaviour
{
    [SerializeField] private string objParentName;
    private ObjectSaver objectSaver;
    private LocalAxis localAxis;
    Transform parentTransform;
    private void Awake()
    {
        objectSaver = GetComponent<ObjectSaver>();
        localAxis = GetComponent<LocalAxis>();

    }
    private void Start()
    {
        ApplicationManager.Instance.OnApplicationStateChanged += OnApplicationStateChanged;
        StaticEventHandler.OnNameMapText += OnNameMapText;
    }
    private void OnDestroy()
    {
        ApplicationManager.Instance.OnApplicationStateChanged -= OnApplicationStateChanged;
        StaticEventHandler.OnNameMapText -= OnNameMapText;
    }

    private void OnNameMapText(string es3Name)
    {
        objectSaver.LoadTransform(objParentName);
    }

    private void OnApplicationStateChanged(ApplicationState state)
    {
        if (state == ApplicationState.WallManager)
        {
            parentTransform = transform.parent;
        }
        if (state == ApplicationState.ObjectManager)
        {
            objectSaver.SaveTransform(objParentName);
        }
        if (state == ApplicationState.ObjectParent)
        {
            ToggleInteractableObjectParent(true);
            localAxis.enabled = true;
        }
        else
        {
            ToggleInteractableObjectParent(false);
            localAxis.enabled = false;
        }
        KeepObjectParentOnWall(state);

    }

    void KeepObjectParentOnWall(ApplicationState applicationState)
    {
        if (applicationState == ApplicationState.WallManager)
        {
            if (GameResources.Instance.currentwallManager == null) return;
            transform.SetParent(GameResources.Instance.currentwallManager.transform);
        }
        else
        {
            if (parentTransform == null) return;
            transform.SetParent(parentTransform);
            transform.localScale = Vector3.one;
        }
    }
    void ToggleInteractableObjectParent(bool isEnable)
    {
        GetComponent<XRGrabInteractable>().enabled = isEnable;
        GetComponent<Collider>().enabled = isEnable;
    }
}
