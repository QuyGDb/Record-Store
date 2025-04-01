using Google.XR.ARCoreExtensions;
using UnityEngine;
using UnityEngine.InputSystem;

public class CloudAnchorDispatcher : MonoBehaviour
{
    [SerializeField] private LayerMask cloudAnchorLayer; // Layer của cloud anchor
    private ARCloudAnchor previousARCloudARAnchor;
    private ARCloudAnchor currentARCloudAnchor;
    private InputSystem_Actions inputActions;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
        cloudAnchorLayer = LayerMask.GetMask("CloudAnchor");
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
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(screenPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, cloudAnchorLayer);

        if (hit.collider != null)
        {
            if (previousARCloudARAnchor == hit.collider.gameObject.GetComponent<ARCloudAnchor>())
            {
                previousARCloudARAnchor.GetComponent<SpriteRenderer>().color = Color.white;
                return;
            }
            // for current anchor
            currentARCloudAnchor = hit.collider.gameObject.GetComponent<ARCloudAnchor>();
            currentARCloudAnchor.GetComponent<SpriteRenderer>().color = Color.red;
            StaticEventHandler.InvokeCloudAnchorSelected(currentARCloudAnchor);
            previousARCloudARAnchor = currentARCloudAnchor;
        }
    }
}