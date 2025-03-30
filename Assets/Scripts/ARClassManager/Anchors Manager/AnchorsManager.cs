using Google.XR.ARCoreExtensions;
using Sirenix.OdinInspector;
using System;
using System.Collections;
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
    private ARAnchorManager arAnchorsManager;
    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hitResults = new List<ARRaycastHit>();
    [ShowInInspector]
    public Dictionary<string, ARAnchor> trackedAnchors = new Dictionary<string, ARAnchor>();
    FeatureMapQuality quality;
    public AnchorAction anchorAction;
    private ARAnchor previousSelectAnchor;
    [HideInInspector] public ARAnchor currentSelectAnchor;
    [SerializeField] private TextMeshProUGUI quatityText;
    private XROrigin xrOrigin;
    private Image image;
    private void Awake()
    {
        arAnchorsManager = GetComponent<ARAnchorManager>();
        arRaycastManager = GetComponent<ARRaycastManager>();
        xrOrigin = GetComponent<XROrigin>();
    }
    private void Start()
    {
        StaticEventHandler.InvokeAnchorsManager(this);
    }
    void OnEnable() => arAnchorsManager.trackablesChanged.AddListener(OnAnchorChanged);

    void OnDisable() => arAnchorsManager.trackablesChanged.RemoveListener(OnAnchorChanged);


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

            //Pose cameraPose = new Pose(xrOrigin.Camera.transform.localPosition,
            //                           xrOrigin.Camera.transform.localRotation);
            //quality = arAnchorsManager.EstimateFeatureMapQualityForHosting(cameraPose);
            //quatityText.text = quality.ToString();
            //if (quality == FeatureMapQuality.Insufficient)
            //    return;
            //if (quality == FeatureMapQuality.Sufficient)
            //    return;
            //if (quality == FeatureMapQuality.Good)
            //{
            //    quatityText.text = quality.ToString();
            Result<ARAnchor> result = await arAnchorsManager.TryAddAnchorAsync(hitPose);
            ARAnchor anchor = result.value;
            CaptureScreenshot(anchor);
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
        }
#endif
        if (isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(inputPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                ARAnchor anchor = hit.transform.GetComponent<ARAnchor>();
                if (anchor != null && anchor != currentSelectAnchor)
                {
                    previousSelectAnchor = currentSelectAnchor;
                    if (previousSelectAnchor != null)
                        previousSelectAnchor.GetComponent<MeshRenderer>().material = GameResources.Instance.defaultMaterial;
                    currentSelectAnchor = anchor;

                    anchor.GetComponent<MeshRenderer>().material = GameResources.Instance.selectAnchorMAT;
                    return anchor;
                }
                if (anchor != null && anchor == currentSelectAnchor)
                {
                    currentSelectAnchor.GetComponent<MeshRenderer>().material = GameResources.Instance.defaultMaterial;
                    currentSelectAnchor = null;
                    return null;
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
        //Pose cameraPose = new Pose(xrOrigin.Camera.transform.localPosition,
        //                              xrOrigin.Camera.transform.localRotation);
        //quality = arAnchorsManager.EstimateFeatureMapQualityForHosting(cameraPose);
        //quatityText.text = quality.ToString();
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            HandleAnchorAction(Touchscreen.current.primaryTouch.position.ReadValue());
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleAnchorAction(Mouse.current.position.ReadValue());
        }
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

    async void CaptureScreenshot(ARAnchor anchor)
    {
        await Awaitable.EndOfFrameAsync();

        int width = Screen.width;
        int height = Screen.height;

        // Tạo Texture2D
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        texture.Apply();

        image = anchor.GetComponentInChildren<Image>();
        image.sprite = Sprite.Create(texture, new Rect(0, 0, width, height), Vector2.zero);
    }
}