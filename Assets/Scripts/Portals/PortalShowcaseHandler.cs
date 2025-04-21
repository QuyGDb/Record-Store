using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PortalShowcaseHandler : MonoBehaviour
{
    private XRGrabInteractable grabTransformer;
    private ObjectSaver objectSaver;
    private InputAction touchAction;
    public string portalname = "PortalShowcase";
    private void Awake()
    {
        touchAction = GameResources.Instance.touchRef.action;
        touchAction.Enable();
        touchAction.started += OnTouchStarted;
        grabTransformer = GetComponent<XRGrabInteractable>();
        objectSaver = GetComponent<ObjectSaver>();

    }

    private void OnTouchStarted(InputAction.CallbackContext context)
    {
        if (ApplicationManager.Instance.applicationState == ApplicationState.TestMap ||
           ApplicationManager.Instance.applicationState == ApplicationState.View)
        {
            Vector2 touchPosition = context.ReadValue<Vector2>();
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject.CompareTag("Portal"))
                {
                    var portal = hit.collider.GetComponent<Portal>();
                    if (portal != null)
                    {
                        portal.SetDoorState();
                    }
                }
            }
        }
    }

    void Start()
    {
        objectSaver.LoadTransform(portalname);
        grabTransformer.selectEntered.AddListener(OnSelectEntered);
        StaticEventHandler.OnMovePortal += MovePortal;
        ApplicationManager.Instance.OnApplicationStateChanged += OnApplicationStateChanged;

    }

    private void OnDestroy()
    {
        grabTransformer.selectEntered.RemoveListener(OnSelectEntered);
        touchAction.started -= OnTouchStarted;
        touchAction.Disable();
        StaticEventHandler.OnMovePortal -= MovePortal;
        ApplicationManager.Instance.OnApplicationStateChanged -= OnApplicationStateChanged;
    }

    private void OnApplicationStateChanged(ApplicationState state)
    {

        if (state == ApplicationState.TestMap)
        {
            objectSaver.SaveTransform(portalname);
            grabTransformer.enabled = false;
        }

    }

    private void MovePortal()
    {
        transform.DOLocalMoveZ(1f, 2.5f);
    }
    private void OnSelectEntered(SelectEnterEventArgs arg0)
    {
        StaticEventHandler.InvokeXRGrabInteractableSelected(this.gameObject);
    }


}
