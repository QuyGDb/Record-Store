using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TransformObjectsManager : MonoBehaviour
{
    [Header("Input Action")]
    public InputActionReference pinchGapDeltaRef;
    [Header("Scale Settings")]
    public float scaleSensitivity = 0.01f;
    public Vector3 minScale = Vector3.one * 0.1f;
    public Vector3 maxScale = Vector3.one * 10f;
    private InputAction pinchAction;
    private GameObject gameObjectSelected;

    private void Awake()
    {
        StaticEventHandler.OnXRGrabInteractableSelected += OnXRGrabInteractableSelected;
        pinchAction = pinchGapDeltaRef.action;
        pinchAction.Enable();
        pinchAction.performed += OnPinchPerformed;
    }
    private void Start()
    {
        GameResources.Instance.transformObjectsManager = this;
    }
    private void OnXRGrabInteractableSelected(GameObject obj)
    {
        XRGrabInteractable interactable = obj.GetComponent<XRGrabInteractable>();
        if (GameManager.Instance.applicationState == ApplicationState.ObjectManager)
        {
            if (obj.GetComponent<WallManager>() != null) return;
        }
        if (interactable != null && interactable.trackScale)
        {
            gameObjectSelected = obj;
        }
    }

    private void OnDestroy()
    {
        StaticEventHandler.OnXRGrabInteractableSelected -= OnXRGrabInteractableSelected;
        pinchAction.performed -= OnPinchPerformed;
        pinchAction.Disable();
    }
    private void OnPinchPerformed(InputAction.CallbackContext context)
    {

        float pinchDelta = context.ReadValue<float>();
        float scaleFactor = 1 + pinchDelta * scaleSensitivity;
        Vector3 newScale = gameObjectSelected.transform.localScale * scaleFactor;
        newScale = Vector3.Max(minScale, Vector3.Min(maxScale, newScale));
        gameObjectSelected.transform.localScale = newScale;
    }


    public void SaveTransformSelectInstrument()
    {
        InstrumentShowcase instrumentShowcase = gameObjectSelected.GetComponent<InstrumentShowcase>();
        if (instrumentShowcase != null)
        {
            ES3.Save(instrumentShowcase.instrumentShowcaseSO.instrumentName, instrumentShowcase.transform);
        }
    }
}