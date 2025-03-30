using Google.XR.ARCoreExtensions;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;


public class CloudAnchorsManager : MonoBehaviour
{
    AnchorsManager anchorsManager;
    private ARAnchorManager arAnchorsManager;
    [SerializeField] private GameObject cloundAnchorPrefab;
    [ShowInInspector]
    public Dictionary<string, AnchorDetails> cloudAnchorDetails = new Dictionary<string, AnchorDetails>();
    [ShowInInspector]
    public Dictionary<string, ARCloudAnchor> cloudAnchors = new Dictionary<string, ARCloudAnchor>();
    [SerializeField] private TextMeshProUGUI hostNotifyText;
    [SerializeField] private TextMeshProUGUI resolveNotifyText;

    [SerializeField] List<string> cloudAnchorsSelectedList = new List<string>();
    private string nameCurrentAnchor;
    private string descriptionCurrentAnchor;
    private void Awake()
    {
        anchorsManager = GetComponent<AnchorsManager>();
        arAnchorsManager = GetComponent<ARAnchorManager>();
        StaticEventHandler.OnSendAnchorInfo += OnSendAnchorInfo;
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
                StaticEventHandler.InvokeCloudAnchorDetailsChanged(cloudAnchorDetails);
            }
        }
        SaveCloudAnchorDetails();
    }
    private void Start()
    {
        LoadCloudAnchorDetails();
        StaticEventHandler.InvokeCloudAnchorsManager(this);
    }
    private void OnDestroy()
    {
        StaticEventHandler.OnSendAnchorInfo -= OnSendAnchorInfo;
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




    private void OnSendAnchorInfo(string name, string description)
    {
        nameCurrentAnchor = name;
        descriptionCurrentAnchor = description;
        HostCurrentSelectAnchor();
    }
    public void HostCurrentSelectAnchor()
    {
        ARAnchor anchor = anchorsManager.currentSelectAnchor;
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
            AnchorDetails anchorDetails = InitializeAnchorDetails(aRAnchor, hostCloudAnchorPromise);
            cloudAnchorDetails.Add(hostCloudAnchorPromise.Result.CloudAnchorId, anchorDetails);
            hostNotifyText.text = $"{hostCloudAnchorPromise.Result.CloudAnchorId}";
            SaveCloudAnchorDetails();
        }
        else
        {
            hostNotifyText.text = $"Error {hostCloudAnchorPromise.Result.CloudAnchorState}";
        }
    }
    AnchorDetails InitializeAnchorDetails(ARAnchor aRAnchor, HostCloudAnchorPromise hostCloudAnchorPromise)
    {
        AnchorDetails anchorDetails = new AnchorDetails();
        anchorDetails.anchorSprite = aRAnchor.GetComponentInChildren<Image>().sprite;
        anchorDetails.anchorName = nameCurrentAnchor;
        anchorDetails.anchorDescription = descriptionCurrentAnchor;
        anchorDetails.cloudAnchorId = hostCloudAnchorPromise.Result.CloudAnchorId;
        return anchorDetails;
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
            // vi mat thoi gian pending nen anchorDetails du thoi gian de init
            resolveNotifyText.text = $"🔄 FIND + {Time.frameCount}";
            yield return null;
        }


        if (resolveCloudAnchorPromise.Result.CloudAnchorState == CloudAnchorState.Success)
        {
            ARCloudAnchor aRCloudAnchor = resolveCloudAnchorPromise.Result.Anchor;

            cloudAnchors.Add(cloudAnchorId, aRCloudAnchor);

            GameObject gameObject = Instantiate(cloundAnchorPrefab, aRCloudAnchor.transform);
            gameObject.transform.localPosition = Vector3.zero;
            cloudAnchorsSelectedList.Remove(cloudAnchorId);
            resolveNotifyText.text = $"📍 Đã tải Cloud Anchor: {cloudAnchorId}";
            //Vector3 anchorPosition = aRCloudAnchor.transform.position;
            //Quaternion anchorRotation = aRCloudAnchor.transform.rotation;
            //gameObject.GetComponentInChildren<TextMeshProUGUI>().text =
            //    $"📍 Pos: {anchorPosition.x:F2}, {anchorPosition.y:F2}, {anchorPosition.z:F2}\n" +
            //    $"🔄 Rot: {anchorRotation.eulerAngles.x:F2}, {anchorRotation.eulerAngles.y:F2}, {anchorRotation.eulerAngles.z:F2}";
        }
        else
        {
            resolveNotifyText.text = $"❌ Không thể tải Cloud Anchor {cloudAnchorId}. Trạng thái: {resolveCloudAnchorPromise.Result.CloudAnchorState}";
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


}

