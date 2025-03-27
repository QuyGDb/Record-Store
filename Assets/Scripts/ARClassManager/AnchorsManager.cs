using EasyTextEffects.Editor.MyBoxCopy.Extensions;
using Google.XR.ARCoreExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AnchorsManager : MonoBehaviour
{
    private ARAnchorManager arAnchorsManager;

    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hitResults = new List<ARRaycastHit>();
    private Dictionary<string, ARAnchor> trackedAnchors = new Dictionary<string, ARAnchor>();
    private Dictionary<string, ARCloudAnchor> cloudAnchors = new Dictionary<string, ARCloudAnchor>();
    private void Awake()
    {
        arAnchorsManager = GetComponent<ARAnchorManager>();
        arRaycastManager = GetComponent<ARRaycastManager>();
    }
    void OnEnable() => arAnchorsManager.trackablesChanged.AddListener(OnAnchorChanged);

    void OnDisable() => arAnchorsManager.trackablesChanged.RemoveListener(OnAnchorChanged);

    private void OnAnchorChanged(ARTrackablesChangedEventArgs<ARAnchor> eventArgs)
    {
        foreach (var newAnchor in eventArgs.added)
        {
            trackedAnchors[newAnchor.trackableId.ToString()] = newAnchor;
        }
        foreach (var updatedAnchor in eventArgs.updated)
        {
            if (trackedAnchors.ContainsKey(updatedAnchor.trackableId.ToString()))
            {
                trackedAnchors[updatedAnchor.trackableId.ToString()] = updatedAnchor;
            }
        }
        foreach (var removedAnchor in eventArgs.removed)
        {
            trackedAnchors.Remove(removedAnchor.Key.ToString());
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
    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            PlaceAnchor(Mouse.current.position.ReadValue());
            Debug.Log(trackedAnchors.Count);
        }
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            StartCoroutine(HostCloudAnchor());
        }
    }
    private IEnumerator HostCloudAnchor()
    {
        foreach (var anchor in trackedAnchors)
        {
            ARAnchor arAnchor = anchor.Value;

            HostCloudAnchorPromise hostCloudAnchorPromise = arAnchorsManager.HostCloudAnchorAsync(arAnchor, 300);
            while (hostCloudAnchorPromise.State == PromiseState.Pending)
            {
                yield return null;
            }

            if (hostCloudAnchorPromise.Result.CloudAnchorState == CloudAnchorState.Success)
            {
                Debug.Log($"✅ Cloud Anchor Hosted! ID: {hostCloudAnchorPromise.Result.CloudAnchorId}");
            }
            else
            {
                Debug.LogError($"❌ Lưu Cloud Anchor thất bại! Trạng thái: {hostCloudAnchorPromise.Result.CloudAnchorState}");
            }
        }
    }
}
//    private IEnumerator ResolveCloudAnchor(string cloudAnchorId)
//    {
//        ResolveCloudAnchorPromise resolveCloudAnchorPromise = arAnchorsManager.ResolveCloudAnchorAsync(cloudAnchorId);

//        // Đợi cho đến khi Promise hoàn thành
//        while (resolveCloudAnchorPromise.State == PromiseState.Pending)
//        {
//            yield return null; // Chờ frame tiếp theo
//        }

//        // Kiểm tra kết quả
//        if (resolveCloudAnchorPromise.Result.CloudAnchorState == CloudAnchorState.Success)
//        {
//            Debug.Log("✅ Cloud Anchor tải thành công!");
//            InstantiateAnchor(resolveCloudAnchorPromise.Result);
//        }
//        else
//        {
//            Debug.LogError($"❌ Không thể tải Cloud Anchor. Trạng thái: {resolveCloudAnchorPromise.Result.CloudAnchorState}");
//        }
//    }
//    // Hàm tạo Anchor từ dữ liệu tải về
//    private void InstantiateAnchor(ARCloudAnchor cloudAnchor)
//    {
//        if (cloudAnchor != null)
//        {
//            Debug.Log("📍 Tạo lại Anchor trên AR Scene.");
//        }

//    }
//}