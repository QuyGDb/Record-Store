using Google.XR.ARCoreExtensions;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

public class CloudAnchorDispatcher : MonoBehaviour
{
    [SerializeField] private LayerMask cloudAnchorLayer; // Layer của cloud anchor
    private ARAnchor currentARCloudAnchor;
    //  private InputSystem_Actions inputActions;
    int mainDropDownValue;
    void Awake()
    {
        // inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        //inputActions.Enable();
        //inputActions.Touch.TouchPress.performed += ctx => OnTouchPerformed(ctx);
        //inputActions.Mouse.MouseClick.performed += ctx => OnMousePerformed(ctx);
        StaticEventHandler.OnMainDropdownChanged += OnMainDropdownChanged;
    }

    void OnDisable()
    {
        //inputActions.Touch.TouchPress.performed -= ctx => OnTouchPerformed(ctx);
        //inputActions.Mouse.MouseClick.performed -= ctx => OnMousePerformed(ctx);
        //StaticEventHandler.OnMainDropdownChanged -= OnMainDropdownChanged;
        //inputActions.Disable();
    }

    private void OnMainDropdownChanged(int value)
    {
        mainDropDownValue = value;
    }

    private void OnTouchPerformed(InputAction.CallbackContext context)
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            CheckForCloudAnchor(touchPosition);
        }
    }

    private void OnMousePerformed(InputAction.CallbackContext context)
    {
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            CheckForCloudAnchor(mousePosition);
        }
    }

    void CheckForCloudAnchor(Vector2 screenPosition)
    {
        if (mainDropDownValue != 2) return;
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, cloudAnchorLayer))
        {
            if (hit.collider != null)
            {
                ARAnchor hitAnchor = hit.collider.gameObject.GetComponent<ARAnchor>();

                if (hitAnchor != null)
                {
                    if (currentARCloudAnchor == hitAnchor)
                    {
                        currentARCloudAnchor.GetComponent<SpriteRenderer>().color = Color.white;
                        currentARCloudAnchor = null;
                        StaticEventHandler.InvokeAnchorSelected(null);
                    }
                    else
                    {
                        if (currentARCloudAnchor != null)
                        {
                            currentARCloudAnchor.GetComponent<SpriteRenderer>().color = Color.white;
                        }

                        currentARCloudAnchor = hitAnchor;
                        currentARCloudAnchor.GetComponent<SpriteRenderer>().color = Color.red;
                        StaticEventHandler.InvokeAnchorSelected(currentARCloudAnchor);
                    }
                }
            }
        }
    }
}