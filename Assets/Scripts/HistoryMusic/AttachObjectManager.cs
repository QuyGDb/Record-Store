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
    private int mainDropDownValue;
    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }
    private void OnEnable()
    {
        StaticEventHandler.OnAnchorSelected += StaticEventHandler_OnCloudAnchorSelected;
        StaticEventHandler.OnInstrumentSelected += StaticEventHandler_OnInstrumentSelected;
        StaticEventHandler.OnCurrentAnchorChanged += StaticEventHandler_OnCurrentCloudAnchorChanged;
        StaticEventHandler.OnMainDropdownChanged += OnMainDropdownChanged;
        inputActions.Enable();
        inputActions.Mouse.MouseClick.performed += ctx => SelectObject(ctx);
        inputActions.Touch.TouchPress.performed += ctx => SelectObject(ctx);
    }

    private void OnDisable()
    {
        StaticEventHandler.OnAnchorSelected -= StaticEventHandler_OnCloudAnchorSelected;
        StaticEventHandler.OnInstrumentSelected -= StaticEventHandler_OnInstrumentSelected;
        StaticEventHandler.OnCurrentAnchorChanged -= StaticEventHandler_OnCurrentCloudAnchorChanged;
        StaticEventHandler.OnMainDropdownChanged -= OnMainDropdownChanged;
        inputActions.Mouse.MouseClick.performed -= ctx => SelectObject(ctx);
        inputActions.Touch.TouchPress.performed -= ctx => SelectObject(ctx);
        inputActions.Disable();
    }

    private void OnMainDropdownChanged(int value)
    {
        mainDropDownValue = value;
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
        // select list of instrument to add to the select anchor
        if (isAdd)
        {
            selectedInstrumentList.Add(instrument);
        }
        else
        {
            selectedInstrumentList.Remove(instrument);
        }
    }

    public void PlaceObject()
    {
        if (currentCloudAnchor == null) return;
        foreach (InstrumentDetails instrument in selectedInstrumentList)
        {
            GameObject instrumentObject = Instantiate(instrument.instrumentPrefab, currentCloudAnchor.transform);
            instrumentObject.transform.rotation = Quaternion.Euler(-90, 0, 0);
            instrumentObject.transform.position = currentCloudAnchor.transform.position;
        }
    }

    public void SelectObject(InputAction.CallbackContext context)
    {
        if (mainDropDownValue != 3) return;
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
            Debug.Log(selectedGameObject.name);
        }
    }

    public void SaveObjectAtReleasePosition()
    {
        if (selectedGameObject == null) return;
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


