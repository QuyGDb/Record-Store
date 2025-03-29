using Google.XR.ARCoreExtensions;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CloudAnchorsManager : MonoBehaviour
{
    AnchorsManager anchorsManager;
    private ARAnchorManager arAnchorsManager;
    [SerializeField] private GameObject cloundAnchorPrefab;
    public Dictionary<string, AnchorType> cloudAnchorIdToAnchorType = new Dictionary<string, AnchorType>();
    [ShowInInspector]
    public Dictionary<string, ARCloudAnchor> cloudAnchors = new Dictionary<string, ARCloudAnchor>();
    private void Awake()
    {
        anchorsManager = GetComponent<AnchorsManager>();
        arAnchorsManager = GetComponent<ARAnchorManager>();
    }

    public void HostSelectAnchor()
    {
        ARAnchor anchor = anchorsManager.SelectAnchor();
        AnchorType anchorType = anchor.GetComponent<AnchorDetailsHandler>().anchorType;
        if (anchor != null)
        {
            StartCoroutine(HostCloudAnchorRoutine(anchor, anchorType));
        }
    }


    private IEnumerator HostCloudAnchorRoutine(ARAnchor aRAnchor, AnchorType anChorType)
    {

        HostCloudAnchorPromise hostCloudAnchorPromise = arAnchorsManager.HostCloudAnchorAsync(aRAnchor, 300);
        while (hostCloudAnchorPromise.State == PromiseState.Pending)
        {
            yield return null;
        }

        if (hostCloudAnchorPromise.Result.CloudAnchorState == CloudAnchorState.Success)
        {
            cloudAnchorIdToAnchorType.Add(hostCloudAnchorPromise.Result.CloudAnchorId, anChorType);
            SaveCloudAnchorIdsToAnchorType();
        }
        else
        {
            Debug.LogError($"Error {hostCloudAnchorPromise.Result.CloudAnchorState}");
        }
    }

    public void ResolveAllCloudAnchors()
    {
        StartCoroutine(ResolveAllCloudAnchorsRoutine());
    }

    private IEnumerator ResolveAllCloudAnchorsRoutine()
    {
        foreach (var cloudAnchorId in cloudAnchorIdToAnchorType)
        {
            yield return StartCoroutine(ResolveCloudAnchorRoutine(cloudAnchorId.Key, cloudAnchorId.Value));
        }
    }


    private IEnumerator ResolveCloudAnchorRoutine(string cloudAnchorId, AnchorType anChorType)
    {
        ResolveCloudAnchorPromise resolveCloudAnchorPromise = arAnchorsManager.ResolveCloudAnchorAsync(cloudAnchorId);

        while (resolveCloudAnchorPromise.State == PromiseState.Pending)
        {
            yield return null;
        }

        if (resolveCloudAnchorPromise.Result.CloudAnchorState == CloudAnchorState.Success)
        {
            ARCloudAnchor aRCloudAnchor = resolveCloudAnchorPromise.Result.Anchor;
            aRCloudAnchor.gameObject.layer = LayerMask.NameToLayer(anChorType.ToString());
            cloudAnchors.Add(cloudAnchorId, resolveCloudAnchorPromise.Result.Anchor);
            GameObject gameObject = Instantiate(cloundAnchorPrefab, aRCloudAnchor.transform);
            gameObject.transform.localPosition = Vector3.zero;

        }
        else
        {
            Debug.LogError($"❌ Không thể tải Cloud Anchor. Trạng thái: {resolveCloudAnchorPromise.Result.CloudAnchorState}");
        }
    }

    void SaveCloudAnchorIdsToAnchorType()
    {
        ES3.Save("cloudAnchorIdToAnchorType", cloudAnchorIdToAnchorType);
    }
}

//private IEnumerator HostCloudAllAnchorsRoutine()
//{
//    foreach (var anchor in anchorsManager.trackedAnchors)
//    {
//        yield return StartCoroutine(HostCloudAnchorRoutine(anchor.Value));
//    }
//}
//public void HostCloudAllAnchors()
//{
//    StartCoroutine(HostCloudAllAnchorsRoutine());
//}
//public void ResolveSelectCloudAnchor()
//{

//    StartCoroutine(ResolveCloudAnchorRoutine(anchor.cloudAnchorId));
//}