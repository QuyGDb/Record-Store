using Google.XR.ARCoreExtensions;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AnchorsManager : MonoBehaviour
{
    private ARAnchorManager arAnchorsManager;

    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hitResults = new List<ARRaycastHit>();
    [ShowInInspector]
    private Dictionary<string, ARAnchor> trackedAnchors = new Dictionary<string, ARAnchor>();
    [ShowInInspector]
    private Dictionary<string, ARCloudAnchor> cloudAnchors = new Dictionary<string, ARCloudAnchor>();
    [ShowInInspector]
    private List<string> cloudAnchorIds = new List<string>();
    private AnchorsManagerState anchorsManagerState = AnchorsManagerState.Creating;
    [SerializeField] private GameObject cloundAnchorPrefab;
    private void Awake()
    {
        arAnchorsManager = GetComponent<ARAnchorManager>();
        arRaycastManager = GetComponent<ARRaycastManager>();
    }
    private void Start()
    {
        StaticEventHandler.OnAnchorsManagerStateChanged += OnAnchorsManagerStateChanged;

    }
    void OnEnable() => arAnchorsManager.trackablesChanged.AddListener(OnAnchorChanged);

    void OnDisable() => arAnchorsManager.trackablesChanged.RemoveListener(OnAnchorChanged);
    private void OnDestroy()
    {
        StaticEventHandler.OnAnchorsManagerStateChanged -= OnAnchorsManagerStateChanged;
    }

    private void OnAnchorsManagerStateChanged(AnchorsManagerState state)
    {
        anchorsManagerState = state;
    }

    private void OnAnchorChanged(ARTrackablesChangedEventArgs<ARAnchor> eventArgs)
    {
        foreach (var newAnchor in eventArgs.added)
        {
            trackedAnchors[newAnchor.trackableId.ToString()] = newAnchor;
            Debug.Log($"🔵 Anchor mới: {newAnchor.trackableId}");
        }
        foreach (var updatedAnchor in eventArgs.updated)
        {
            if (trackedAnchors.ContainsKey(updatedAnchor.trackableId.ToString()))
            {
                trackedAnchors[updatedAnchor.trackableId.ToString()] = updatedAnchor;
                Debug.Log($"🟢 Anchor cập nhật: {updatedAnchor.trackableId}");
            }
        }
        foreach (var removedAnchor in eventArgs.removed)
        {
            trackedAnchors.Remove(removedAnchor.Key.ToString());
            Debug.Log($"🔴 Anchor bị xóa: {removedAnchor.Value.trackableId}");
        }
    }

    private async void PlaceAnchor(Vector2 position)
    {
        if (arRaycastManager.Raycast(position, hitResults, TrackableType.AllTypes))
        {
            Pose hitPose = hitResults[0].pose;
            Result<ARAnchor> result = await arAnchorsManager.TryAddAnchorAsync(hitPose);
            ARAnchor anchor = result.value;
        }
    }
    private ARAnchor SelectAnchor()
    {
        Vector2 inputPosition = Vector2.zero;
        bool isPressed = false;

        // Kiểm tra nếu đang dùng màn hình cảm ứng
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            inputPosition = Touchscreen.current.primaryTouch.position.ReadValue();
            isPressed = true;
        }
#if UNITY_EDITOR
        if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            inputPosition = Mouse.current.position.ReadValue();
            isPressed = true;
        }
#endif
        if (isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(inputPosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                ARAnchor anchor = hit.transform.GetComponent<ARAnchor>();
                if (anchor != null)
                {
                    return anchor;
                }
            }
        }
        return null;
    }

    public void DeleteAnchor()
    {
        ARAnchor anchor = SelectAnchor();
        if (anchor != null)
        {
            arAnchorsManager.TryRemoveAnchor(anchor);
        }
    }


    private void Update()
    {
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            if (anchorsManagerState == AnchorsManagerState.Creating)
            {
                PlaceAnchor(Touchscreen.current.primaryTouch.position.ReadValue());
            }
            if (anchorsManagerState == AnchorsManagerState.Deleting)
            {
                DeleteAnchor();
            }
        }
#if UNITY_EDITOR
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (anchorsManagerState == AnchorsManagerState.Creating)
            {
                PlaceAnchor(Mouse.current.position.ReadValue());
            }
            if (anchorsManagerState == AnchorsManagerState.Deleting)
            {
                DeleteAnchor();
            }
#endif
        }
    }
    public void HostCloudAllAnchors()
    {
        StartCoroutine(HostCloudAllAnchorsRoutine());
    }

    private IEnumerator HostCloudAllAnchorsRoutine()
    {
        foreach (var anchor in trackedAnchors)
        {
            yield return StartCoroutine(HostCloudAnchorRoutine(anchor.Value));
        }
    }

    public void HostSelectAnchor()
    {
        ARAnchor anchor = SelectAnchor();
        if (anchor != null)
        {
            StartCoroutine(HostCloudAnchorRoutine(anchor));
        }
    }


    private IEnumerator HostCloudAnchorRoutine(ARAnchor aRAnchor)
    {

        HostCloudAnchorPromise hostCloudAnchorPromise = arAnchorsManager.HostCloudAnchorAsync(aRAnchor, 300);
        while (hostCloudAnchorPromise.State == PromiseState.Pending)
        {
            yield return null;
        }

        if (hostCloudAnchorPromise.Result.CloudAnchorState == CloudAnchorState.Success)
        {
            cloudAnchorIds.Add(hostCloudAnchorPromise.Result.CloudAnchorId);
        }
        else
        {
            Debug.LogError($"❌ Lưu Cloud Anchor thất bại! Trạng thái: {hostCloudAnchorPromise.Result.CloudAnchorState}");
        }
    }

    public void ResolveAllCloudAnchors()
    {
        StartCoroutine(ResolveAllCloudAnchorsRoutine());
    }

    private IEnumerator ResolveAllCloudAnchorsRoutine()
    {
        foreach (var cloudAnchorId in cloudAnchorIds)
        {
            yield return StartCoroutine(ResolveCloudAnchorRoutine(cloudAnchorId));
        }
    }

    public void ResolveSelectCloudAnchor()
    {

        //StartCoroutine(ResolveCloudAnchorRoutine(anchor.cloudAnchorId));
    }
    private IEnumerator ResolveCloudAnchorRoutine(string cloudAnchorId)
    {
        ResolveCloudAnchorPromise resolveCloudAnchorPromise = arAnchorsManager.ResolveCloudAnchorAsync(cloudAnchorId);

        // Đợi cho đến khi Promise hoàn thành
        while (resolveCloudAnchorPromise.State == PromiseState.Pending)
        {
            yield return null; // Chờ frame tiếp theo
        }

        if (resolveCloudAnchorPromise.Result.CloudAnchorState == CloudAnchorState.Success)
        {
            ARCloudAnchor aRCloudAnchor = resolveCloudAnchorPromise.Result.Anchor;
            cloudAnchors.Add(cloudAnchorId, resolveCloudAnchorPromise.Result.Anchor);
            GameObject gameObject = Instantiate(cloundAnchorPrefab, aRCloudAnchor.transform);
            gameObject.transform.localPosition = Vector3.zero;

        }
        else
        {
            Debug.LogError($"❌ Không thể tải Cloud Anchor. Trạng thái: {resolveCloudAnchorPromise.Result.CloudAnchorState}");
        }
    }


}