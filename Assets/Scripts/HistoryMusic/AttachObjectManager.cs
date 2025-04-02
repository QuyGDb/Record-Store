using Google.XR.ARCoreExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class AttachObjectManager : MonoBehaviour
{
    public List<InstrumentDetails> selectedInstrumentList = new List<InstrumentDetails>();
    private ARAnchor currentCloudAnchor;
    public float maxDistance = 100f;
    public LayerMask hitLayers;
    private GameObject selectedGameObject;
    public float scaleSpeed = 0.1f;
    private InputSystem_Actions inputActions;
    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        StaticEventHandler.OnAnchorSelected += StaticEventHandler_OnCloudAnchorSelected;
        StaticEventHandler.OnInstrumentSelected += StaticEventHandler_OnInstrumentSelected;
        StaticEventHandler.OnCurrentAnchorChanged += StaticEventHandler_OnCurrentCloudAnchorChanged;
        inputActions.Enable();
        inputActions.Mouse.MouseClick.performed += ctx => SelectObject(ctx);
        inputActions.Touch.TouchPress.performed += ctx => SelectObject(ctx);
    }

    private void OnDisable()
    {
        StaticEventHandler.OnAnchorSelected -= StaticEventHandler_OnCloudAnchorSelected;
        StaticEventHandler.OnInstrumentSelected -= StaticEventHandler_OnInstrumentSelected;
        StaticEventHandler.OnCurrentAnchorChanged -= StaticEventHandler_OnCurrentCloudAnchorChanged;
        inputActions.Mouse.MouseClick.performed -= ctx => SelectObject(ctx);
        inputActions.Touch.TouchPress.performed -= ctx => SelectObject(ctx);
        inputActions.Disable();
    }

    private void StaticEventHandler_OnCloudAnchorSelected(ARAnchor anchor)
    {
        currentCloudAnchor = anchor;
    }

    private void Start()
    {
        StaticEventHandler.InvokeAttachObjectManagerChanged(this);
    }
    private void StaticEventHandler_OnCurrentCloudAnchorChanged(ARAnchor anchor)
    {
        currentCloudAnchor = anchor;
    }



    private void StaticEventHandler_OnInstrumentSelected(InstrumentDetails instrument, bool isAdd)
    {
        if (isAdd)
        {
            selectedInstrumentList.Add(instrument);
        }
        else
        {
            selectedInstrumentList.Remove(instrument);
        }
    }
    private void Update()
    {

        MoveObject(selectedGameObject);
        ScaleObject();
    }
    public void PlaceObject()
    {
        foreach (InstrumentDetails instrument in selectedInstrumentList)
        {
            GameObject instrumentObject = Instantiate(instrument.instrumentPrefab, currentCloudAnchor.transform);
            instrumentObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            instrumentObject.transform.position = currentCloudAnchor.transform.position;
        }

    }

    public void SelectObject(InputAction.CallbackContext context)
    {
        Vector2 screenPosition = Vector2.zero;
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            screenPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            screenPosition = Mouse.current.position.ReadValue();
        }
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, hitLayers))
        {
            selectedGameObject = hit.collider.gameObject;
        }
    }

    private void MoveObject(GameObject gameObject)
    {
        if (gameObject != null)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            gameObject.transform.position = HelperUtilities.GetWorldPosition(mousePosition);
        }
    }

    public void ScaleObject()
    {
        if (selectedGameObject == null) return; // Không có object được chọn thì không làm gì

        if (Mouse.current != null && Mouse.current.scroll.ReadValue().y != 0)
        {
            float scrollDelta = Mouse.current.scroll.ReadValue().y * scaleSpeed;
            selectedGameObject.transform.localScale += Vector3.one * scrollDelta;
        }
#if PLATFORM_ANDROID && !UNITY_EDITOR

        if (Touchscreen.current != null && Touchscreen.current.touches.Count >= 2)
        {
            TouchControl touch1 = Touchscreen.current.touches[0];
            TouchControl touch2 = Touchscreen.current.touches[1];

            Vector2 prevPos1 = touch1.startPosition.ReadValue();
            Vector2 prevPos2 = touch2.startPosition.ReadValue();
            Vector2 currPos1 = touch1.position.ReadValue();
            Vector2 currPos2 = touch2.position.ReadValue();

            float prevDistance = Vector2.Distance(prevPos1, prevPos2);
            float currDistance = Vector2.Distance(currPos1, currPos2);
            float scaleFactor = (currDistance - prevDistance) * scaleSpeed;

            selectedGameObject.transform.localScale += Vector3.one * scaleFactor;
        }
#endif
    }
    public void SaveObjectAtReleasePosition()
    {
        Transform transform = selectedGameObject.transform;
        ES3.Save(selectedGameObject.name, transform);
    }

    public void DeteleCurrentSelectedObject()
    {
        if (selectedGameObject != null)
        {
            Destroy(selectedGameObject);
        }
    }
}


