using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MusicHistoryManager : MonoBehaviour
{
    [SerializeField] private GameObject musicHistoryPrefab;
    private MusicHistoryUI musicHistoryUI;
    private InputAction touchAction;
    private Vector3 offset;
    private void Awake()
    {
        touchAction = GameResources.Instance.touchRef.action;
        touchAction.Enable();
        touchAction.started += OnTouchStarted;
    }
    private void OnDestroy()
    {
        touchAction.started -= OnTouchStarted;
        touchAction.Disable();
    }
    private void Start()
    {
        GameObject gameObject = Instantiate(musicHistoryPrefab, transform);
        musicHistoryUI = gameObject.GetComponent<MusicHistoryUI>();
        musicHistoryUI.gameObject.SetActive(false);
    }
    private void OnTouchStarted(InputAction.CallbackContext context)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (GameManager.Instance.applicationState != ApplicationState.LoadMapMode) return;
        Vector2 touchPosition = context.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        int musicHistoryLayerMask = 1 << LayerMask.NameToLayer("MusicHistory");
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, musicHistoryLayerMask))
        {
            offset = (hit.point - Camera.main.transform.position).normalized;
            if (hit.transform.CompareTag("Pop"))
            {
                musicHistoryUI.gameObject.SetActive(true);
                musicHistoryUI.gameObject.transform.position = hit.transform.position - offset;
                musicHistoryUI.PlayVideo(GameResources.Instance.pop.videoClip);
            }
            else if (hit.transform.CompareTag("Rap"))
            {
                musicHistoryUI.gameObject.SetActive(true);
                musicHistoryUI.gameObject.transform.position = hit.transform.position - offset;
                musicHistoryUI.PlayVideo(GameResources.Instance.rap.videoClip);
            }

            else if (hit.transform.CompareTag("Rock"))
            {
                musicHistoryUI.gameObject.SetActive(true);
                musicHistoryUI.gameObject.transform.position = hit.transform.position - offset;
                musicHistoryUI.PlayVideo(GameResources.Instance.rock.videoClip);
            }

        }
    }
}