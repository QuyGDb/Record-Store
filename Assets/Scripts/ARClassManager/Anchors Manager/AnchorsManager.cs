using Google.XR.ARCoreExtensions;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AnchorsManager : MonoBehaviour
{
    public LayerMask anchorLayer;
    private ARAnchorManager arAnchorsManager;
    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hitResults = new List<ARRaycastHit>();
    [ShowInInspector]
    public Dictionary<string, ARAnchor> trackedAnchors = new Dictionary<string, ARAnchor>();
    public AnchorAction anchorAction;
    private ARAnchor previousSelectAnchor;
    [HideInInspector] public ARAnchor currentSelectAnchor;
    private ARCameraCapture arCameraCapture;
    [SerializeField] private TextMeshProUGUI quatityText;
    public byte[] imageByte;
    private InputSystem_Actions inputActions;
    private void Awake()
    {
        arAnchorsManager = GetComponent<ARAnchorManager>();
        arRaycastManager = GetComponent<ARRaycastManager>();
        arCameraCapture = GetComponentInChildren<ARCameraCapture>();
        inputActions = new InputSystem_Actions();
    }
    private void Start()
    {
        StaticEventHandler.InvokeAnchorsManager(this);
    }
    void OnEnable()
    {
        inputActions.Enable();

        inputActions.Touch.TouchPress.performed += ctx => OnTouchPerformed(ctx);
        inputActions.Mouse.MouseClick.performed += ctx => OnMousePerformed(ctx);
        arAnchorsManager.trackablesChanged.AddListener(OnAnchorChanged);
    }

    private void OnMousePerformed(InputAction.CallbackContext ctx)
    {
        Vector2 inputPosition = Vector2.zero;
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            inputPosition = Mouse.current.position.ReadValue();
        }
        HandleAnchorAction(inputPosition);
    }

    private void OnTouchPerformed(InputAction.CallbackContext ctx)
    {

        Vector2 inputPosition = Vector2.zero;
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            inputPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        }

        HandleAnchorAction(inputPosition);
    }

    void OnDisable()
    {
        inputActions.Touch.TouchPress.performed -= ctx => OnTouchPerformed(ctx);
        inputActions.Mouse.MouseClick.performed -= ctx => OnMousePerformed(ctx);
        arAnchorsManager.trackablesChanged.RemoveListener(OnAnchorChanged);
        inputActions.Disable();
    }



    private void OnAnchorChanged(ARTrackablesChangedEventArgs<ARAnchor> eventArgs)
    {
        foreach (var newAnchor in eventArgs.added)
        {
            trackedAnchors[newAnchor.trackableId.ToString()] = newAnchor;
        }
        foreach (var updatedAnchor in eventArgs.updated)
        {
            if (trackedAnchors.ContainsKey(updatedAnchor.trackableId.ToString()))
            {
                trackedAnchors[updatedAnchor.trackableId.ToString()] = updatedAnchor;
            }
        }
        foreach (var removedAnchor in eventArgs.removed)
        {
            trackedAnchors.Remove(removedAnchor.Key.ToString());
        }
    }

    private async void PlaceAnchor(Vector2 position)
    {
        if (arRaycastManager.Raycast(position, hitResults, TrackableType.AllTypes))
        {
            Pose hitPose = hitResults[0].pose;
            hitPose.rotation = Quaternion.Euler(0, 0, 0);
            Result<ARAnchor> result = await arAnchorsManager.TryAddAnchorAsync(hitPose);
            ARAnchor anchor = result.value;
#if UNITY_EDITOR

            await CaptureScreenshot(anchor);
#endif
#if PLATFORM_ANDROID && !UNITY_EDITOR
            imageByte = await arCameraCapture.Capture();

#endif
        }

    }
    public ARAnchor SelectAnchor()
    {
        Vector2 inputPosition = Vector2.zero;
        bool isPressed = false;

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            inputPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            isPressed = true;
        }
#if UNITY_EDITOR
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            inputPosition = Mouse.current.position.ReadValue();
            isPressed = true;
            Debug.Log(inputPosition);
        }
#endif
        if (isPressed)
        {

            Ray ray = Camera.main.ScreenPointToRay(inputPosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, anchorLayer))
            {
                if (hit.collider != null)
                {
                    ARAnchor anchor = hit.transform.GetComponent<ARAnchor>();
                    if (anchor != null && anchor != currentSelectAnchor)
                    {
                        previousSelectAnchor = currentSelectAnchor;
                        if (previousSelectAnchor != null)
                            previousSelectAnchor.GetComponent<SpriteRenderer>().color = Color.white;

                        currentSelectAnchor = anchor;
                        anchor.GetComponent<SpriteRenderer>().color = Color.green;
                        return anchor;
                    }
                    else if (anchor != null && anchor == currentSelectAnchor)
                    {
                        // Nếu nhấn lại cùng một Anchor, bỏ chọn
                        currentSelectAnchor.GetComponent<SpriteRenderer>().color = Color.white;
                        currentSelectAnchor = null;
                        return null;
                    }
                }


            }
        }
        return null;
    }
    public void DeleteAnchor()
    {
        arAnchorsManager.TryRemoveAnchor(currentSelectAnchor);
    }
    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;


        if (anchorAction == AnchorAction.Delete)
        {
            if (currentSelectAnchor != null)
                DeleteAnchor();
        }
    }

    void HandleAnchorAction(Vector2 touchPostion)
    {
        if (anchorAction == AnchorAction.Create)
        {
            PlaceAnchor(touchPostion);
        }

        if (anchorAction == AnchorAction.Select)
        {
            SelectAnchor();
        }
        if (anchorAction == AnchorAction.None)
        {
            return;
        }
    }

    async Awaitable<Texture2D> CaptureScreenshot(ARAnchor anchor)
    {
        await Awaitable.EndOfFrameAsync();
        byte[] textureData;

        int width = Screen.width;
        int height = Screen.height;

        Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture.Apply();
        textureData = texture.EncodeToPNG();
        return texture;

    }


    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(quatityText), quatityText);
    }
#endif
    #endregion
}