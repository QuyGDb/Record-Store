using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MusicHistoryManager : MonoBehaviour
{
    [SerializeField] private GameObject musicHistoryPrefab;
    [SerializeField] private InputActionReference touchActionRef;
    private InputAction touchAction;

    private void Awake()
    {
        touchAction = touchActionRef.action;
        touchAction.Enable();
        touchAction.started += OnTouchStarted;
    }
    private void OnDestroy()
    {
        touchAction.started -= OnTouchStarted;
        touchAction.Disable();
    }
    private void OnTouchStarted(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.applicationState != ApplicationState.LoadMapMode) return;
        Vector2 touchPosition = context.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        int musicHistoryLayerMask = 1 << LayerMask.NameToLayer("MusicHistory");
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, musicHistoryLayerMask))
        {
            var musicHistory = hit.collider.gameObject.GetComponent<MusicHistoryUI>();
            if (musicHistory != null)
            {
                GameObject obj = Instantiate(musicHistoryPrefab, transform);
                obj.SetActive(true);
                MusicHistoryUI musicHistoryUI = obj.GetComponent<MusicHistoryUI>();
                //  musicHistoryUI.PlayVideo(musicHistory);
            }
        }
    }
}
