using Google.XR.ARCoreExtensions;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class AnchorsManager : MonoBehaviour
{
    [HideInInspector] public ARAnchorManager arAnchorsManager;
    [HideInInspector] public ARPlaneManager arPlaneManager;
    [HideInInspector] public ARPointCloudManager arPointCloudManager;
    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hitResults = new List<ARRaycastHit>();
    [ShowInInspector] public Dictionary<string, ARAnchor> trackedAnchors = new Dictionary<string, ARAnchor>();
    [HideInInspector] public ARAnchor currentSelectAnchor;
    public byte[] imageByte;
    private void Awake()
    {
        arAnchorsManager = GetComponent<ARAnchorManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();
        arPointCloudManager = GetComponent<ARPointCloudManager>();
        arRaycastManager = GetComponent<ARRaycastManager>();
        arAnchorsManager.trackablesChanged.AddListener(OnAnchorChanged);
        StaticEventHandler.OnAnchorCreated += OnAnchorChanged;
        ApplicationManager.Instance.OnApplicationStateChanged += OnApplicationState;
        GameResources.Instance.anchorsManager = this;
    }

    private void OnDestroy()
    {
        arAnchorsManager.trackablesChanged.RemoveListener(OnAnchorChanged);
        StaticEventHandler.OnAnchorCreated -= OnAnchorChanged;
        ApplicationManager.Instance.OnApplicationStateChanged += OnApplicationState;
    }

    private void OnApplicationState(ApplicationState state)
    {
        if (state == ApplicationState.Anchor)
        {
            InvokeRepeating(nameof(CheckEstimateFeatureMapQualityForHosting), 0f, 1f);
        }
        else
        {
            if (ApplicationManager.Instance.previousApplicationState == ApplicationState.Anchor)
            {
                foreach (var item in trackedAnchors)
                {
                    Destroy(item.Value.gameObject);
                }
                CancelInvoke(nameof(CheckEstimateFeatureMapQualityForHosting));
            }
        }
        if (state == ApplicationState.Client)
        {
            foreach (var item in trackedAnchors)
            {
                item.Value.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        if (state == ApplicationState.Client)
        {
            arPointCloudManager.enabled = false;
            arPlaneManager.enabled = false;
        }
    }

    private void OnAnchorChanged(Anchor anchor, bool isSelect)
    {
        if (isSelect)
        {
            currentSelectAnchor = anchor.GetComponent<ARAnchor>();
        }
        else
        {
            currentSelectAnchor = null;
        }
    }

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

    public async Awaitable<bool> PlaceAnchor(Vector2 position)
    {
        if (arRaycastManager.Raycast(position, hitResults, TrackableType.AllTypes))
        {
            Pose hitPose = hitResults[0].pose;
            hitPose.rotation = Quaternion.Euler(0, 0, 0);
            Result<ARAnchor> result = await arAnchorsManager.TryAddAnchorAsync(hitPose);
            ARAnchor anchor = result.value;

#if UNITY_EDITOR
            imageByte = await HelperUtilities.CaptureScreenshot(anchor);
#endif
#if PLATFORM_ANDROID && !UNITY_EDITOR
            imageByte = await HelperUtilities.CaptureWithARObjects();

#endif
            return anchor != null;
        }
        return false;
    }

    public bool DeleteAnchor()
    {
        return arAnchorsManager.TryRemoveAnchor(currentSelectAnchor);
    }
    private Pose GetCameraPose()
    {
        Pose cameraPose = new Pose();
        cameraPose.position = Camera.main.transform.position;
        cameraPose.rotation = Camera.main.transform.rotation;
        return cameraPose;
    }

    void CheckEstimateFeatureMapQualityForHosting()
    {
#if PLATFORM_ANDROID && !UNITY_EDITOR
        if (GameResources.Instance.anchorSceneText == null || arAnchorsManager == null)
            return;
        GameResources.Instance.anchorSceneText.text = arAnchorsManager.EstimateFeatureMapQualityForHosting(GetCameraPose()).ToString();
#endif
    }
}
