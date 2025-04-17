using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class TransformObjectsManager : MonoBehaviour
{
    [Header("Input Action")]
    public InputActionReference pinchGapDeltaRef;
    [Header("Scale Settings")]
    public float scaleSensitivity = 0.01f;
    [Header("Scale Limits")]
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
        if (obj == null)
        {
            gameObjectSelected = null;
            return;
        }
        XRGrabInteractable interactable = obj.GetComponent<XRGrabInteractable>();
        gameObjectSelected = obj;
    }

    private void OnDestroy()
    {
        StaticEventHandler.OnXRGrabInteractableSelected -= OnXRGrabInteractableSelected;
        pinchAction.performed -= OnPinchPerformed;
        pinchAction.Disable();
    }
    private void OnPinchPerformed(InputAction.CallbackContext context)
    {
        if (gameObjectSelected == null) return;
        float pinchDelta = context.ReadValue<float>();
        float scaleFactor = 1 + pinchDelta * scaleSensitivity;
        Vector3 newScale = gameObjectSelected.transform.localScale * scaleFactor;
        newScale = Vector3.Max(minScale, Vector3.Min(maxScale, newScale));
        gameObjectSelected.transform.localScale = newScale;
    }
    public void SaveSelectedItemTransformWithIdentifier()
    {
        if (gameObjectSelected == null) return;
        InstrumentShowcase instrumentShowcase = gameObjectSelected.GetComponent<InstrumentShowcase>();
        if (instrumentShowcase != null)
        {
            SaveTransformSelectObject(instrumentShowcase.transform, instrumentShowcase.instrumentShowcaseSO.instrumentName);
            return;
        }
        PictureFrame pictureFrame = gameObjectSelected.GetComponent<PictureFrame>();
        if (pictureFrame != null)
        {
            SaveTransformSelectObject(pictureFrame.transform, pictureFrame.pictureName);
            return;
        }

    }

    private void SaveTransformSelectObject(Transform objectTransform, string name)
    {
        ObjectSaver objectSaver = objectTransform.GetComponent<ObjectSaver>();
        if (objectSaver != null)
        {
            objectSaver.SaveTransform(name);
        }
        GameResources.Instance.objectSceneText.text = objectTransform.name;

    }
}
