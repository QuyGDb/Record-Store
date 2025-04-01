using Google.XR.ARCoreExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

public class CloudAnchorDispatcher : MonoBehaviour
{
    [SerializeField] private LayerMask cloudAnchorLayer; // Layer của cloud anchor
    private ARCloudAnchor currentARCloudAnchor;
    private InputSystem_Actions inputActions;
    [SerializeField] private TextMeshProUGUI TextMeshProUGUI;
    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        inputActions.Enable();

        inputActions.Touch.TouchPress.performed += ctx => OnTouchPerformed(ctx);
        inputActions.Mouse.MouseClick.performed += ctx => OnMousePerformed(ctx);
    }

    void OnDisable()
    {
        inputActions.Touch.TouchPress.performed -= ctx => OnTouchPerformed(ctx);
        inputActions.Mouse.MouseClick.performed -= ctx => OnMousePerformed(ctx);
        inputActions.Disable();
    }

    private void OnTouchPerformed(InputAction.CallbackContext context)
    {
        TextMeshProUGUI.text = "Touch performed";
        Debug.Log("Touch performed");
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            CheckForCloudAnchor(touchPosition);
        }
    }

    private void OnMousePerformed(InputAction.CallbackContext context)
    {
        TextMeshProUGUI.text = "Mouse performed";
        Debug.Log("Mouse performed");
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            CheckForCloudAnchor(mousePosition);
        }
    }

    void CheckForCloudAnchor(Vector2 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;
        TextMeshProUGUI.text = "Checking for cloud anchor";
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, cloudAnchorLayer))
        {
            if (hit.collider != null)
            {
                TextMeshProUGUI.text = "Cloud Anchor Hit";
                if (currentARCloudAnchor == hit.collider.gameObject.GetComponent<ARAnchor>())
                {
                    currentARCloudAnchor.GetComponent<SpriteRenderer>().color = Color.white;
                }
                currentARCloudAnchor = hit.collider.gameObject.GetComponent<ARCloudAnchor>();
                currentARCloudAnchor.GetComponent<SpriteRenderer>().color = Color.red;
                StaticEventHandler.InvokeCloudAnchorSelected(currentARCloudAnchor);
                TextMeshProUGUI.text = "Cloud Anchor Selected";
            }
        }
    }
}