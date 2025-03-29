using Google.XR.ARCoreExtensions;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
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

    public AnchorAction anchorAction;
    public ARAnchor currentSelectAnchor;
    private void Awake()
    {
        arAnchorsManager = GetComponent<ARAnchorManager>();
        arRaycastManager = GetComponent<ARRaycastManager>();
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
            Result<ARAnchor> result = await arAnchorsManager.TryAddAnchorAsync(hitPose);
            ARAnchor anchor = result.value;
        }
    }
    public ARAnchor SelectAnchor()
    {
        Vector2 inputPosition = Vector2.zero;
        bool isPressed = false;

        // Kiểm tra nếu đang dùng màn hình cảm ứng
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
        Debug.Log(IsPointerOverUI());
        if (IsPointerOverUI())
            return;
#if UNITY_ANDROID || UNITY_IOS
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            HandleAnchorAction(Touchscreen.current.primaryTouch.position.ReadValue());
#endif
        }


#if UNITY_EDITOR
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleAnchorAction(Mouse.current.position.ReadValue());
#endif
        }

        void HandleAnchorAction(Vector2 touchPostion)
        {
            if (anchorAction == AnchorAction.Create)
            {
                PlaceAnchor(touchPostion);
            }
            if (anchorAction == AnchorAction.Create)
            {
                DeleteAnchor();
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
    }

    public bool IsPointerOverUI()
    {
        Vector2 pointerPosition;

#if UNITY_ANDROID || UNITY_IOS
        if (Touchscreen.current == null || Touchscreen.current.primaryTouch.press.isPressed == false)
            return false;
        pointerPosition = Touchscreen.current.primaryTouch.position.ReadValue();
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
        pointerPosition = Mouse.current.position.ReadValue();
#else
        return false; // Mặc định không hỗ trợ nền tảng khác
#endif

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = pointerPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}