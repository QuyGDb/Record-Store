using Google.XR.ARCoreExtensions;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using static TMPro.TMP_Compatibility;

public class CloudAnchorsManager : MonoBehaviour
{
    AnchorsManager anchorsManager;
    private ARAnchorManager arAnchorsManager;
    [SerializeField] private GameObject cloundAnchorPrefab;
    public Dictionary<string, AnchorType> cloudAnchorIdToAnchorType = new Dictionary<string, AnchorType>();
    [ShowInInspector]
    public Dictionary<string, ARCloudAnchor> cloudAnchors = new Dictionary<string, ARCloudAnchor>();
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    private void Awake()
    {
        anchorsManager = GetComponent<AnchorsManager>();
        arAnchorsManager = GetComponent<ARAnchorManager>();
        cloudAnchorIdToAnchorType = ES3.Load("cloudAnchorIdToAnchorType", cloudAnchorIdToAnchorType);
    }
    private void Start()
    {
        StaticEventHandler.InvokeCloudAnchorsManager(this);
    }
    public void HostCurrentSelectAnchor()
    {
        ARAnchor anchor = anchorsManager.currentSelectAnchor;
        AnchorType anchorType = anchor.GetComponent<AnchorDetailsHandler>().anchorType;
        if (anchor != null)
        {
            textMeshProUGUI.text = $"Đang tải current Cloud Anchor";
            StartCoroutine(HostCloudAnchorRoutine(anchor, anchorType));
        }
    }


    private IEnumerator HostCloudAnchorRoutine(ARAnchor aRAnchor, AnchorType anChorType)
    {
        textMeshProUGUI.text = $"Đang tải Cloud Anchor";
        HostCloudAnchorPromise hostCloudAnchorPromise = arAnchorsManager.HostCloudAnchorAsync(aRAnchor, 300);
        textMeshProUGUI.text = $"Đang tải Cloud Anchor1";
        while (hostCloudAnchorPromise.State == PromiseState.Pending)
        {
            yield return null;
        }

        if (hostCloudAnchorPromise.Result.CloudAnchorState == CloudAnchorState.Success)
        {
            cloudAnchorIdToAnchorType.Add(hostCloudAnchorPromise.Result.CloudAnchorId, anChorType);

            SaveCloudAnchorIdsToAnchorType();

            textMeshProUGUI.text = $"Cloud Anchor Id: {hostCloudAnchorPromise.Result.CloudAnchorId}";
        }
        else
        {
            Debug.LogError($"Error {hostCloudAnchorPromise.Result.CloudAnchorState}");
        }
    }

    public void ResolveAllCloudAnchors()
    {

        if (cloudAnchorIdToAnchorType == null || cloudAnchorIdToAnchorType.Count == 0)
        {
            return;
        }

        StartCoroutine(ResolveAllCloudAnchorsRoutine());
    }

    private IEnumerator ResolveAllCloudAnchorsRoutine()
    {
        if (cloudAnchorIdToAnchorType == null || cloudAnchorIdToAnchorType.Count == 0)
        {
            yield break;
        }
        foreach (var cloudAnchorId in cloudAnchorIdToAnchorType)
        {
            yield return StartCoroutine(ResolveCloudAnchorRoutine(cloudAnchorId.Key, cloudAnchorId.Value));
        }
    }


    private IEnumerator ResolveCloudAnchorRoutine(string cloudAnchorId, AnchorType anChorType)
    {
        textMeshProUGUI.text = $"🔄 Đang tải Cloud Anchor: {cloudAnchorId}";
        ResolveCloudAnchorPromise resolveCloudAnchorPromise = arAnchorsManager.ResolveCloudAnchorAsync(cloudAnchorId);

        while (resolveCloudAnchorPromise.State == PromiseState.Pending)
        {
            yield return null;
        }


        if (resolveCloudAnchorPromise.Result.CloudAnchorState == CloudAnchorState.Success)
        {
            ARCloudAnchor aRCloudAnchor = resolveCloudAnchorPromise.Result.Anchor;


            // Gán AnchorType
            // AnchorDetailsHandler detailsHandler = aRCloudAnchor.gameObject.AddComponent<AnchorDetailsHandler>();
            //  detailsHandler.anchorType = anChorType;

            cloudAnchors.Add(cloudAnchorId, aRCloudAnchor);

            GameObject gameObject = Instantiate(cloundAnchorPrefab, aRCloudAnchor.transform);
            gameObject.transform.localPosition = Vector3.zero;
            textMeshProUGUI.text = $"📍 Đã tải Cloud Anchor: {cloudAnchorId}";
            //Vector3 anchorPosition = aRCloudAnchor.transform.position;
            //Quaternion anchorRotation = aRCloudAnchor.transform.rotation;
            //gameObject.GetComponentInChildren<TextMeshProUGUI>().text =
            //    $"📍 Pos: {anchorPosition.x:F2}, {anchorPosition.y:F2}, {anchorPosition.z:F2}\n" +
            //    $"🔄 Rot: {anchorRotation.eulerAngles.x:F2}, {anchorRotation.eulerAngles.y:F2}, {anchorRotation.eulerAngles.z:F2}";
        }
        else
        {
            Debug.LogError($"❌ Không thể tải Cloud Anchor {cloudAnchorId}. Trạng thái: {resolveCloudAnchorPromise.Result.CloudAnchorState}");
            textMeshProUGUI.text = $"❌ Không thể tải Cloud Anchor {cloudAnchorId}. Trạng thái: {resolveCloudAnchorPromise.Result.CloudAnchorState}";
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