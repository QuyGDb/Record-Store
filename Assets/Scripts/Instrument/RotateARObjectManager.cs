using UnityEngine;
using UnityEngine.UI;

public class RotateARObjectManager : MonoBehaviour
{
    private Button[] buttons;
    Transform currentTransform;
    private void Awake()
    {
        StaticEventHandler.OnXRGrabInteractableSelected += HandleXRGrabInteractableSelected;
        buttons = GetComponentsInChildren<Button>();
    }

    private void Start()
    {
        buttons[0].onClick.AddListener(() => RotateXPositive(currentTransform));
        buttons[1].onClick.AddListener(() => RotateXNegative(currentTransform));
        buttons[2].onClick.AddListener(() => RotateYPositive(currentTransform));
        buttons[3].onClick.AddListener(() => RotateYNegative(currentTransform));
        buttons[4].onClick.AddListener(() => RotateZPositive(currentTransform));
        buttons[5].onClick.AddListener(() => RotateZNegative(currentTransform));
        buttons[6].onClick.AddListener(() => ResetRotate(currentTransform));
    }
    private void OnDestroy()
    {
        StaticEventHandler.OnXRGrabInteractableSelected -= HandleXRGrabInteractableSelected;

        buttons[0].onClick.RemoveListener(() => RotateXPositive(currentTransform));
        buttons[1].onClick.RemoveListener(() => RotateXNegative(currentTransform));
        buttons[2].onClick.RemoveListener(() => RotateYPositive(currentTransform));
        buttons[3].onClick.RemoveListener(() => RotateYNegative(currentTransform));
        buttons[4].onClick.RemoveListener(() => RotateZPositive(currentTransform));
        buttons[5].onClick.RemoveListener(() => RotateZNegative(currentTransform));
        buttons[6].onClick.RemoveListener(() => ResetRotate(currentTransform));

    }
    private void HandleXRGrabInteractableSelected(GameObject obj)
    {
        if (obj == null)
        {
            currentTransform = null;
            return;
        }

        if (obj.GetComponent<InstrumentShowcase>() != null)
        {
            currentTransform = obj.transform;
        }
    }

    private void RotateXPositive(Transform transform)
    {
        if (transform == null)
            return;
        transform.Rotate(Vector3.right, 90);
    }
    private void RotateXNegative(Transform transform)
    {
        if (transform == null)
            return;
        transform.Rotate(Vector3.right, -90);
    }
    private void RotateYPositive(Transform transform)
    {
        if (transform == null)
            return;
        transform.Rotate(Vector3.up, 90);
    }
    private void RotateYNegative(Transform transform)
    {
        if (transform == null)
            return;
        transform.Rotate(Vector3.up, -90);
    }
    private void RotateZPositive(Transform transform)
    {
        if (transform == null)
            return;
        transform.Rotate(Vector3.forward, 90);
    }
    private void RotateZNegative(Transform transform)
    {
        if (transform == null)
            return;
        transform.Rotate(Vector3.forward, -90);
    }
    private void ResetRotate(Transform transform)
    {
        if (transform == null)
            return;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
        transform.localPosition = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(1, 1, 1);
    }
    private void RotateObject(Transform transform)
    {
        if (transform == null)
            return;
        currentTransform = transform;
    }

}
