using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PortalShowcaseHandler : MonoBehaviour
{
    private XRGrabInteractable grabTransformer;
    private Transform portalTranform;
    private InputAction touchAction;
    public string portalname = "PortalShowcase";
    private void Awake()
    {
        touchAction = GameResources.Instance.touchRef.action;
        touchAction.Enable();
        touchAction.started += OnTouchStarted;
        grabTransformer = GetComponent<XRGrabInteractable>();

    }

    private void OnTouchStarted(InputAction.CallbackContext context)
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

    void Start()
    {
        portalTranform = ES3.Load(portalname, transform);
        LoadTransform();
        grabTransformer.selectEntered.AddListener(OnSelectEntered);
        StaticEventHandler.OnMovePortal += MovePortal;
    }



    private void OnDestroy()
    {
        grabTransformer.selectEntered.RemoveListener(OnSelectEntered);
        touchAction.started -= OnTouchStarted;
        touchAction.Disable();
        StaticEventHandler.OnMovePortal -= MovePortal;
    }
    private void MovePortal()
    {
        transform.DOLocalMoveZ(-1f, 2.5f);
    }
    private void OnSelectEntered(SelectEnterEventArgs arg0)
    {
        StaticEventHandler.InvokeXRGrabInteractableSelected(this.gameObject);
    }

    async void LoadTransform()
    {
        if (portalTranform != null)
        {
            await Awaitable.NextFrameAsync();
            grabTransformer.transform.localPosition = portalTranform.localPosition;
            grabTransformer.transform.localRotation = portalTranform.localRotation;
            grabTransformer.transform.localScale = portalTranform.localScale;
        }
    }
}
