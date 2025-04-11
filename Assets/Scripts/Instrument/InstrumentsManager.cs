using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InstrumentsManager : MonoBehaviour
{
    [SerializeField] private InputActionReference touchActionRef;
    private InputAction touchAction;
    private void Awake()
    {
        touchAction = touchActionRef.action;
        touchAction.Enable();
        touchAction.performed += OnTouchPerformed;
    }
    private void OnDestroy()
    {
        touchAction.performed -= OnTouchPerformed;
        touchAction.Disable();
    }

    private void OnTouchPerformed(InputAction.CallbackContext context)
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit))
        {
            if (hit.collider != null)
            {
                Instrument instrument = hit.collider.GetComponent<Instrument>();
                if (instrument != null)
                {
                }
            }
        }
    }
}
