using UnityEngine;
using UnityEngine.UI;

public class RotateARObjectManager : MonoBehaviour
{
    private Button[] buttons;
    Transform currentTransform;
    private void Awake()
    {
        StaticEventHandler.OnRotateObjectChanged += RotateObject;
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
        StaticEventHandler.OnRotateObjectChanged -= RotateObject;
        buttons[0].onClick.RemoveListener(() => RotateXPositive(currentTransform));
        buttons[1].onClick.RemoveListener(() => RotateXNegative(currentTransform));
        buttons[2].onClick.RemoveListener(() => RotateYPositive(currentTransform));
        buttons[3].onClick.RemoveListener(() => RotateYNegative(currentTransform));
        buttons[4].onClick.RemoveListener(() => RotateZPositive(currentTransform));
        buttons[5].onClick.RemoveListener(() => RotateZNegative(currentTransform));
        buttons[6].onClick.RemoveListener(() => ResetRotate(currentTransform));
    }
    private void RotateXPositive(Transform transform)
    {
        transform.Rotate(Vector3.right, 90);
    }
    private void RotateXNegative(Transform transform)
    {
        transform.Rotate(Vector3.right, -90);
    }
    private void RotateYPositive(Transform transform)
    {
        transform.Rotate(Vector3.up, 90);
    }
    private void RotateYNegative(Transform transform)
    {
        transform.Rotate(Vector3.up, -90);
    }
    private void RotateZPositive(Transform transform)
    {
        transform.Rotate(Vector3.forward, 90);
    }
    private void RotateZNegative(Transform transform)
    {
        transform.Rotate(Vector3.forward, -90);
    }
    private void ResetRotate(Transform transform)
    {
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
    private void RotateObject(Transform transform)
    {
        currentTransform = transform;
    }

}
