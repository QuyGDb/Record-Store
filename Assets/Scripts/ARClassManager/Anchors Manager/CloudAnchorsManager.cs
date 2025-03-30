using Google.XR.ARCoreExtensions;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using static TMPro.TMP_Compatibility;
using static UnityEngine.Rendering.DebugUI;

public class CloudAnchorsManager : MonoBehaviour
{
    AnchorsManager anchorsManager;
    private ARAnchorManager arAnchorsManager;
    [SerializeField] private GameObject cloundAnchorPrefab;
    public Dictionary<string, AnchorDetails> cloudAnchorDetails = new Dictionary<string, AnchorDetails>();
    [ShowInInspector]
    public Dictionary<string, ARCloudAnchor> cloudAnchors = new Dictionary<string, ARCloudAnchor>();
    [SerializeField] private TextMeshProUGUI saveStateText;

    List<string> cloudAnchorsSelectedList = new List<string>();
    private string nameCurrentAnchor;
    private string descriptionCurrentAnchor;
    private void Awake()
    {
        anchorsManager = GetComponent<AnchorsManager>();
        arAnchorsManager = GetComponent<ARAnchorManager>();
        LoadCloudAnchorDetails();
        StaticEventHandler.OnSendAnchorInfo += OnSendIAnchornfo;
        StaticEventHandler.OnSelectCloudAnchor += OnSelectCloudAnchor;

    }
    public void RemoveCloudAnchorInAnchorDetails()
    {
        if (cloudAnchorsSelectedList == null || cloudAnchorsSelectedList.Count == 0) return;
        foreach (var cloudAnchorsSelected in cloudAnchorsSelectedList)
        {
            if (cloudAnchorDetails.ContainsKey(cloudAnchorsSelected))
            {
                cloudAnchorDetails.Remove(cloudAnchorsSelected);
            }
        }
        SaveCloudAnchorDetails();
    }
    private void Start()
    {
        StaticEventHandler.InvokeCloudAnchorsManager(this);
    }
    private void OnDestroy()
    {
        StaticEventHandler.OnSendAnchorInfo -= OnSendIAnchornfo;
        StaticEventHandler.OnSelectCloudAnchor -= OnSelectCloudAnchor;
    }

    private void OnSelectCloudAnchor(bool isOn, string cloudAnchorId)
    {
        if (isOn)
        {
            cloudAnchorsSelectedList.Add(cloudAnchorId);
        }
        else
        {
            cloudAnchorsSelectedList.Remove(cloudAnchorId);
        }
    }

    public void HostCurrentSelectAnchor()
    {
        ARAnchor anchor = anchorsManager.currentSelectAnchor;
        if (anchor != null)
        {
            StartCoroutine(HostCloudAnchorRoutine(anchor));
        }
    }


    private void OnSendIAnchornfo(string arg1, string arg2)
    {
        nameCurrentAnchor = arg1;
        descriptionCurrentAnchor = arg2;
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
            AnchorDetails anchorDetails = InitializeAnchorDetails(aRAnchor, hostCloudAnchorPromise);
            cloudAnchorDetails.Add(hostCloudAnchorPromise.Result.CloudAnchorId, anchorDetails);
            saveStateText.text = $"{hostCloudAnchorPromise.Result.CloudAnchorId}";
            SaveCloudAnchorDetails();
        }
        else
        {
            saveStateText.text = $"Error {hostCloudAnchorPromise.Result.CloudAnchorState}";
        }
    }


    public void ResolveSelectedCloudAnchor()
    {
        foreach (var cloudAnchorId in cloudAnchorsSelectedList)
        {
            StartCoroutine(ResolveCloudAnchorRoutine(cloudAnchorId));
        }
    }

    private IEnumerator ResolveCloudAnchorRoutine(string cloudAnchorId)
    {
        ResolveCloudAnchorPromise resolveCloudAnchorPromise = arAnchorsManager.ResolveCloudAnchorAsync(cloudAnchorId);

        while (resolveCloudAnchorPromise.State == PromiseState.Pending)
        {
            saveStateText.text = $"🔄 FIND + {Time.frameCount}";
            yield return null;
        }


        if (resolveCloudAnchorPromise.Result.CloudAnchorState == CloudAnchorState.Success)
        {
            ARCloudAnchor aRCloudAnchor = resolveCloudAnchorPromise.Result.Anchor;

            cloudAnchors.Add(cloudAnchorId, aRCloudAnchor);

            GameObject gameObject = Instantiate(cloundAnchorPrefab, aRCloudAnchor.transform);
            gameObject.transform.localPosition = Vector3.zero;
            cloudAnchorsSelectedList.Remove(cloudAnchorId);
            saveStateText.text = $"📍 Đã tải Cloud Anchor: {cloudAnchorId}";
            //Vector3 anchorPosition = aRCloudAnchor.transform.position;
            //Quaternion anchorRotation = aRCloudAnchor.transform.rotation;
            //gameObject.GetComponentInChildren<TextMeshProUGUI>().text =
            //    $"📍 Pos: {anchorPosition.x:F2}, {anchorPosition.y:F2}, {anchorPosition.z:F2}\n" +
            //    $"🔄 Rot: {anchorRotation.eulerAngles.x:F2}, {anchorRotation.eulerAngles.y:F2}, {anchorRotation.eulerAngles.z:F2}";
        }
        else
        {
            Debug.LogError($"❌ Không thể tải Cloud Anchor {cloudAnchorId}. Trạng thái: {resolveCloudAnchorPromise.Result.CloudAnchorState}");
            saveStateText.text = $"❌ Không thể tải Cloud Anchor {cloudAnchorId}. Trạng thái: {resolveCloudAnchorPromise.Result.CloudAnchorState}";
        }
    }

    void SaveCloudAnchorDetails()
    {
        ES3.Save("cloudAnchorDetails", cloudAnchorDetails);
        StaticEventHandler.InvokeCloudAnchorDetailsChanged(cloudAnchorDetails);
    }
    void LoadCloudAnchorDetails()
    {
        cloudAnchorDetails = ES3.Load("cloudAnchorDetails", cloudAnchorDetails);
        StaticEventHandler.InvokeCloudAnchorDetailsChanged(cloudAnchorDetails);
    }

    AnchorDetails InitializeAnchorDetails(ARAnchor aRAnchor, HostCloudAnchorPromise hostCloudAnchorPromise)
    {
        AnchorDetails anchorDetails = new AnchorDetails();
        anchorDetails.anchorrSprite = aRAnchor.GetComponentInChildren<Image>().sprite;
        anchorDetails.anchorName = nameCurrentAnchor;
        anchorDetails.anchorDescription = descriptionCurrentAnchor;
        anchorDetails.cloudAnchorId = hostCloudAnchorPromise.Result.CloudAnchorId;
        return anchorDetails;
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