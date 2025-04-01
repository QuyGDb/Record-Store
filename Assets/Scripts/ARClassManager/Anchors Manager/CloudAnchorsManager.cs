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
    [ShowInInspector]
    public Dictionary<string, AnchorDetails> cloudAnchorDetails = new Dictionary<string, AnchorDetails>();
    [ShowInInspector]
    public Dictionary<string, ARCloudAnchor> cloudAnchors = new Dictionary<string, ARCloudAnchor>();
    [SerializeField] private TextMeshProUGUI notify;
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
#if PLATFORM_ANDROID && !UNITY_EDITOR
        HostCloudAnchorPromise hostCloudAnchorPromise = arAnchorsManager.HostCloudAnchorAsync(aRAnchor, 300);
        while (hostCloudAnchorPromise.State == PromiseState.Pending)
        {
            notify.text = $"🔄 HOST + {Time.frameCount}";
            yield return null;
        }

        if (hostCloudAnchorPromise.Result.CloudAnchorState == CloudAnchorState.Success)
        {
            AnchorDetails anchorDetails = InitializeAnchorDetails(aRAnchor, hostCloudAnchorPromise);
            cloudAnchorDetails.Add(hostCloudAnchorPromise.Result.CloudAnchorId, anchorDetails);
            notify.text = $"{hostCloudAnchorPromise.Result.CloudAnchorId}";

        }
        else
            notify.text = $"Error {hostCloudAnchorPromise.Result.CloudAnchorState}";
#endif

#if UNITY_EDITOR
        AnchorDetails anchorDetailsEditor = new AnchorDetails();
        anchorDetailsEditor.anchorImage = aRAnchor.GetComponentInChildren<Image>().sprite.texture.EncodeToPNG();
        anchorDetailsEditor.anchorName = nameCurrentAnchor;
        anchorDetailsEditor.anchorDescription = descriptionCurrentAnchor;
        anchorDetailsEditor.cloudAnchorId = Random.Range(0, 10).ToString();
        yield return new WaitForSeconds(1);
        cloudAnchorDetails.Add(anchorDetailsEditor.cloudAnchorId, anchorDetailsEditor);
#endif
        SaveCloudAnchorDetails();
    }
    AnchorDetails InitializeAnchorDetails(ARAnchor aRAnchor, HostCloudAnchorPromise hostCloudAnchorPromise)
    {
        AnchorDetails anchorDetails = new AnchorDetails();
        anchorDetails.anchorImage = anchorsManager.imageByte;
        notify.text = $"📍 Đã tạo Cloud Anchor: {hostCloudAnchorPromise}";
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
            yield return null;
        }
        if (resolveCloudAnchorPromise.Result.CloudAnchorState == CloudAnchorState.Success)
        {
            ARCloudAnchor aRCloudAnchor = resolveCloudAnchorPromise.Result.Anchor;

            cloudAnchorsSelectedList.Remove(cloudAnchorId);
            ARAnchor aRAnchor = aRCloudAnchor.GetComponent<ARAnchor>();
            aRAnchor.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            notify.text = $"❌ Không thể tải Cloud Anchor {cloudAnchorId}. Trạng thái: {resolveCloudAnchorPromise.Result.CloudAnchorState}";
        }
    }

    void SaveCloudAnchorDetails()
    {
        ES3.Save("cloudAnchorDetails", cloudAnchorDetails);

        foreach (var cloudAnchor in cloudAnchorDetails)
        {
            Debug.Log(cloudAnchor.Value.anchorName);
        }
        StaticEventHandler.InvokeCloudAnchorDetailsChanged(cloudAnchorDetails);
    }
    void LoadCloudAnchorDetails()
    {
        cloudAnchorDetails = ES3.Load("cloudAnchorDetails", cloudAnchorDetails);
        StaticEventHandler.InvokeCloudAnchorDetailsChanged(cloudAnchorDetails);
    }


}

