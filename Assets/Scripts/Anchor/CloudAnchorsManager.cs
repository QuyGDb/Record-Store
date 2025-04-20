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
    [SerializeField] List<string> cloudAnchorsSelectedList = new List<string>();
    private string nameCurrentAnchor;
    [SerializeField] private AnchorType currentAnchorType;
    string key = "cloudAnchorDetails";
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
        GameResources.Instance.cloudAnchorsManager = this;
        ApplicationManager.Instance.OnApplicationStateChanged += OnApplicationStateChanged;
        StaticEventHandler.OnNameMapText += LoadCloudAnchorDetails;


    }
    private void OnDestroy()
    {
        StaticEventHandler.OnSendAnchorInfo -= OnSendAnchorInfo;
        StaticEventHandler.OnSelectCloudAnchor -= OnSelectCloudAnchor;
        ApplicationManager.Instance.OnApplicationStateChanged -= OnApplicationStateChanged;
        StaticEventHandler.OnNameMapText -= LoadCloudAnchorDetails;
    }



    private void OnApplicationStateChanged(ApplicationState state)
    {
        if (state == ApplicationState.CloudAnchorInCreateMap || state == ApplicationState.CloudAnchorInLoadMap)
        {
            cloudAnchorsSelectedList.Clear();
        }
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

    private void OnSendAnchorInfo(string name, AnchorType anchorType)
    {
        nameCurrentAnchor = name;
        currentAnchorType = anchorType;
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
            GameResources.Instance.anchorSceneText.text = $"🔄 HOST + {Time.frameCount}";
            yield return null;
        }

        if (hostCloudAnchorPromise.Result.CloudAnchorState == CloudAnchorState.Success)
        {
            AnchorDetails anchorDetails = InitializeAnchorDetails(aRAnchor, hostCloudAnchorPromise);
            cloudAnchorDetails.Add(hostCloudAnchorPromise.Result.CloudAnchorId, anchorDetails);
            GameResources.Instance.anchorSceneText.text = $"{hostCloudAnchorPromise.Result.CloudAnchorId}";
        }
        else
            GameResources.Instance.anchorSceneText.text = $"Error {hostCloudAnchorPromise.Result.CloudAnchorState}";
#endif

#if UNITY_EDITOR
        AnchorDetails anchorDetailsEditor = new AnchorDetails();
        anchorDetailsEditor.anchorName = nameCurrentAnchor;
        anchorDetailsEditor.anchorType = currentAnchorType;
        anchorDetailsEditor.anchorImage = anchorsManager.imageByte;
        anchorDetailsEditor.cloudAnchorId = Random.Range(0, 100).ToString();
        yield return new WaitForSeconds(1);
        cloudAnchorDetails.Add(anchorDetailsEditor.cloudAnchorId, anchorDetailsEditor);
#endif
        SaveCloudAnchorDetails();
    }
    AnchorDetails InitializeAnchorDetails(ARAnchor aRAnchor, HostCloudAnchorPromise hostCloudAnchorPromise)
    {
        AnchorDetails anchorDetails = new AnchorDetails();
        anchorDetails.anchorImage = anchorsManager.imageByte;
        anchorDetails.anchorName = nameCurrentAnchor;
        anchorDetails.anchorType = currentAnchorType;
        GameResources.Instance.anchorSceneText.text = $"Cloud Anchor created: {hostCloudAnchorPromise}";
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
#if PLATFORM_ANDROID && !UNITY_EDITOR
        var resolveCloudAnchorPromise = arAnchorsManager.ResolveCloudAnchorAsync(cloudAnchorId);

        if (!GameResources.Instance.contentCloudAnchor.activeSelf)
            GameResources.Instance.contentCloudAnchor.SetActive(true);

        float timeout = 10f;
        float timer = 0f;

        while (resolveCloudAnchorPromise.State == PromiseState.Pending && timer < timeout)
        {
            if (GameResources.Instance.notifyResolveText != null)
                GameResources.Instance.notifyResolveText.text = $"🔄 RESOLVE + {Time.frameCount}";

            timer += Time.deltaTime;
            yield return null;
        }

        if (resolveCloudAnchorPromise.State == PromiseState.Pending)
        {
            Debug.LogWarning("Cloud anchor resolve timed out.");
            yield break;
        }

        if (resolveCloudAnchorPromise.Result != null && resolveCloudAnchorPromise.Result.CloudAnchorState == CloudAnchorState.Success)
        {
            var cloudAnchor = resolveCloudAnchorPromise.Result.Anchor;
            QueryARCloudAnchor(cloudAnchor, cloudAnchorId);

            cloudAnchorsSelectedList.Remove(cloudAnchorId);
            GameResources.Instance.resolveCloudAnchorIdList.Add(cloudAnchorId);
            if (GameResources.Instance.notifyResolveText != null)
                GameResources.Instance.notifyResolveText.text = $"Cloud Anchor resolved: {cloudAnchorId}";
            Debug.Log($"Successfully resolved cloud anchor: {cloudAnchorId}, Position: {cloudAnchor.pose.position}");
        }
        else
        {
            var state = resolveCloudAnchorPromise.Result?.CloudAnchorState.ToString() ?? "Unknown";
            if (GameResources.Instance.notifyResolveText != null)
                GameResources.Instance.notifyResolveText.text = $"Error {state}";
            Debug.LogWarning($"Unable to resolve cloud anchor: {cloudAnchorId}. State: {state}");
        }
#endif

#if UNITY_EDITOR
        yield return null;
        GameResources.Instance.contentCloudAnchor.SetActive(!GameResources.Instance.contentCloudAnchor.activeSelf);
        StaticEventHandler.InvokeInstantiateAtWall(null, currentAnchorType);
#endif
    }
    void QueryARCloudAnchor(ARCloudAnchor aRAnchor, string cloudAnchorId)
    {
        if (cloudAnchorDetails.ContainsKey(cloudAnchorId))
        {
            AnchorDetails anchorDetails = cloudAnchorDetails[cloudAnchorId];
            if (anchorDetails != null)
            {
                StaticEventHandler.InvokeInstantiateAtWall(aRAnchor, anchorDetails.anchorType);
            }
        }
    }
    void SaveCloudAnchorDetails()
    {
        ES3.Save(key, cloudAnchorDetails, Settings.es3Name);
        StaticEventHandler.InvokeAnchorDetailsChanged(cloudAnchorDetails);
    }
    void LoadCloudAnchorDetails(string es3Name)
    {
        var settings = new ES3Settings(es3Name);
        if (!ES3.KeyExists(key, settings))
        {
            cloudAnchorDetails = new Dictionary<string, AnchorDetails>();
            return;
        }
        cloudAnchorDetails = ES3.Load(key, cloudAnchorDetails, settings);
        StaticEventHandler.InvokeAnchorDetailsChanged(cloudAnchorDetails);
    }


}

