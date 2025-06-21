using Google.XR.ARCoreExtensions;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class CloudAnchorsManager : MonoBehaviour
{
    AnchorsManager anchorsManager;
    [ShowInInspector]
    public AnchorDetails currentAnchorDetails;
    [ShowInInspector]
    public ARCloudAnchor currentCloudAnchor;
    string key = "cloudAnchorDetails";

    private void Awake()
    {
        anchorsManager = GetComponent<AnchorsManager>();
        StaticEventHandler.OnHostCurrentSelectAnchor += HostCurrentSelectAnchor;

    }

    private void Start()
    {
        GameResources.Instance.cloudAnchorsManager = this;
    }

    private void OnDestroy()
    {
        StaticEventHandler.OnHostCurrentSelectAnchor -= HostCurrentSelectAnchor;
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
        float timeout = 100f;
        float timer = 0f;
        while (hostCloudAnchorPromise.State == PromiseState.Pending && timer < timeout)
        {

            GameResources.Instance.anchorSceneText.text = $"🔄 HOST + {Time.frameCount}";
            timer += Time.deltaTime;
            yield return null;
        }

        if (hostCloudAnchorPromise.Result.CloudAnchorState == CloudAnchorState.Success)
        {
            currentAnchorDetails = InitializeAnchorDetails(aRAnchor, hostCloudAnchorPromise);
            currentCloudAnchor = hostCloudAnchorPromise.Result;
            GameResources.Instance.anchorSceneText.text = $"{hostCloudAnchorPromise.Result.CloudAnchorId}";
        }
        else
            GameResources.Instance.anchorSceneText.text = $"Error {hostCloudAnchorPromise.Result.CloudAnchorState}";
#endif

#if UNITY_EDITOR
        currentAnchorDetails = new AnchorDetails();
        currentAnchorDetails.anchorImage = anchorsManager.imageByte;
        currentAnchorDetails.cloudAnchorId = Random.Range(0, 100).ToString();
        yield return new WaitForSeconds(1);
#endif
        SaveCloudAnchorDetails();
    }
    AnchorDetails InitializeAnchorDetails(ARAnchor aRAnchor, HostCloudAnchorPromise hostCloudAnchorPromise)
    {
        AnchorDetails anchorDetails = new AnchorDetails();
        anchorDetails.anchorImage = anchorsManager.imageByte;
        anchorDetails.cloudAnchorId = hostCloudAnchorPromise.Result.CloudAnchorId;
        return anchorDetails;
    }

    public void ResolveSelectedCloudAnchor()
    {
        if (currentAnchorDetails != null)
        {
            StartCoroutine(ResolveCloudAnchorRoutine(currentAnchorDetails.cloudAnchorId));
        }
    }

    private IEnumerator ResolveCloudAnchorRoutine(string cloudAnchorId)
    {
#if PLATFORM_ANDROID && !UNITY_EDITOR
        var resolveCloudAnchorPromise = arAnchorsManager.ResolveCloudAnchorAsync(cloudAnchorId);

        if (!GameResources.Instance.contentCloudAnchor.activeSelf)
            GameResources.Instance.contentCloudAnchor.SetActive(true);

        float timeout = 100f;
        float timer = 0f;

        while (resolveCloudAnchorPromise.State == PromiseState.Pending && timer < timeout)
        {
            if (GameResources.Instance.notifyResolveText != null)
                GameResources.Instance.notifyResolveText.text = $"RESOLVE + {Time.frameCount}";

            timer += Time.deltaTime;
            yield return null;
        }

        if (resolveCloudAnchorPromise.State == PromiseState.Pending)
        {
            yield break;
        }

        if (resolveCloudAnchorPromise.Result != null && resolveCloudAnchorPromise.Result.CloudAnchorState == CloudAnchorState.Success)
        {
            if (GameResources.Instance.notifyResolveText != null)
            {
                  StaticEventHandler.InvokeInstantiateAtAnchor(aRAnchor);
                GameResources.Instance.notifyResolveText.text = $"Cloud Anchor resolved: {cloudAnchorId}";
            }
            StaticEventHandler.InvokeCloudAnchorResolved(true, $"Cloud Anchor resolved: {cloudAnchorId}");
            currentCloudAnchor = resolveCloudAnchorPromise.Result;
        }
        else
        {
            var state = resolveCloudAnchorPromise.Result?.CloudAnchorState.ToString() ?? "Unknown";
            if (GameResources.Instance.notifyResolveText != null)
                GameResources.Instance.notifyResolveText.text = $"Failed to resolve: {state}";
            StaticEventHandler.InvokeCloudAnchorResolved(false, $"Failed to resolve: {state}");
        }
#endif

#if UNITY_EDITOR
        StaticEventHandler.InvokeInstantiateAtAnchor(null);
        StaticEventHandler.InvokeCloudAnchorResolved(true, $"Failed");
        yield return null;
#endif
    }


    public void SaveCloudAnchorDetails()
    {
        ES3.Save(key, currentAnchorDetails, Settings.es3Name);
    }

    private void OnEnable()
    {
        StaticEventHandler.OnNameMapText += LoadCloudAnchorDetails;
    }
    private void OnDisable()
    {
        StaticEventHandler.OnNameMapText -= LoadCloudAnchorDetails;
    }

    public void LoadCloudAnchorDetails(string fileName)
    {
        if (ES3.FileExists(fileName))
        {
            currentAnchorDetails = ES3.Load<AnchorDetails>(key, fileName);
        }
        else
        {
            currentAnchorDetails = null;
        }
    }



}

