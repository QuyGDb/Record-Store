using Google.XR.ARCoreExtensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ObjectSpawnerAtAnchors : MonoBehaviour
{
    private List<GameObject> instrumentList = new List<GameObject>();
    private List<GameObject> popList = new List<GameObject>();
    private List<GameObject> rapList = new List<GameObject>();
    private List<GameObject> rockList = new List<GameObject>();
    private WallManager wallManager;
    private Transform wallTransform;
    [SerializeField] private Transform instrumentOnWall;
    [SerializeField] private Transform popGenreOnWall;
    [SerializeField] private Transform rapGenreOnWall;
    [SerializeField] private Transform rockGenreOnWall;
    private List<GameObject> instrumentPrefabList = new List<GameObject>();
    private List<GameObject> popPrefabList = new List<GameObject>();
    private List<GameObject> rapPrefabList = new List<GameObject>();
    private List<GameObject> rockPrefabList = new List<GameObject>();
    AnchorType anchorType;

    private void Awake()
    {
        StaticEventHandler.OnInstantiateAtAnchor += OnInstantiateAtAnchor;
        GameResources.Instance.objectSpawnerAtAnchors = this;
        GameManager.Instance.OnApplicationStateChanged += OnApplicationStateChanged;
    }
    private void OnDestroy()
    {
        StaticEventHandler.OnInstantiateAtAnchor -= OnInstantiateAtAnchor;
        GameManager.Instance.OnApplicationStateChanged -= OnApplicationStateChanged;
    }

    private void OnApplicationStateChanged(ApplicationState state)
    {
        if (state == ApplicationState.WallManager)
        {
            foreach (var item in instrumentList)
            {
                item.GetComponent<XRGrabInteractable>().enabled = false;
            }
            foreach (var item in popList)
            {
                item.GetComponent<XRGrabInteractable>().enabled = false;
            }
            foreach (var item in rapList)
            {
                item.GetComponent<XRGrabInteractable>().enabled = false;
            }
            foreach (var item in rockList)
            {
                item.GetComponent<XRGrabInteractable>().enabled = false;
            }
        }

        if (state == ApplicationState.ObjectManager)
        {
            foreach (var item in instrumentList)
            {
                item.GetComponent<XRGrabInteractable>().enabled = true;
            }
            foreach (var item in popList)
            {
                item.GetComponent<XRGrabInteractable>().enabled = true;
            }
            foreach (var item in rapList)
            {
                item.GetComponent<XRGrabInteractable>().enabled = true;
            }
            foreach (var item in rockList)
            {
                item.GetComponent<XRGrabInteractable>().enabled = true;
            }
        }
    }

    private void OnInstantiateAtAnchor(ARCloudAnchor aRAnchor, AnchorType type)
    {
        instrumentPrefabList.Clear();
#if PLATFORM_ANDROID 
        switch (type)
        {
            case AnchorType.IntrumentShowCase:
                foreach (var instrumentShowcaseSO in GameResources.Instance.instrumentShowCaseVN.instrumentShowcaseList)
                {
                    instrumentPrefabList.Add(instrumentShowcaseSO.instrumentPrefab);
                }
                SetupInstrumentShowcaseWall(aRAnchor);
                anchorType = AnchorType.IntrumentShowCase;
                break;
            case AnchorType.MusicHistory:
                foreach (var popPrefab in GameResources.Instance.pop.prefabs)
                {
                    popPrefabList.Add(popPrefab);
                }
                foreach (var rapPrefab in GameResources.Instance.rap.prefabs)
                {
                    rapPrefabList.Add(rapPrefab);
                }
                foreach (var rockPrefab in GameResources.Instance.rock.prefabs)
                {
                    rockPrefabList.Add(rockPrefab);
                }
                SetupHistoryMusicWall(aRAnchor);
                anchorType = AnchorType.MusicHistory;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
#endif

#if UNITY_EDITOR
        List<InstrumentShowcaseSO> instrumentShowcaseSOs = GameResources.Instance.instrumentShowCaseVN.instrumentShowcaseList;
        foreach (var instrumentShowcaseSO in instrumentShowcaseSOs)
        {
            instrumentPrefabList.Add(instrumentShowcaseSO.instrumentPrefab);
        }
        SetupInstrumentShowcaseWall(aRAnchor);
#endif
    }

    private void SetupHistoryMusicWall(ARCloudAnchor aRAnchor)
    {
        GameObject wall = Instantiate(GameResources.Instance.wall_HistoryMusic.wallPrefab, transform);
#if PLATFORM_ANDROID && !UNITY_EDITOR
        wall.transform.SetParent(aRAnchor.transform);
        instrumentOnWall.SetParent(aRAnchor.transform);
#endif
        wall.transform.localPosition = new Vector3(0, 0, 0);
        wall.transform.localRotation = Quaternion.Euler(0, 0, 0);
        wallManager = wall.GetComponent<WallManager>();
        wallManager.wallSO = GameResources.Instance.wall_HistoryMusic;
        GameResources.Instance.wallManager = wallManager;
        wallTransform = wall.transform;
    }

    private void SetupInstrumentShowcaseWall(ARCloudAnchor cloudAnchor)
    {
        GameObject wall = Instantiate(GameResources.Instance.wallSO_Showcase.wallPrefab, transform);
#if PLATFORM_ANDROID && !UNITY_EDITOR
        wall.transform.SetParent(cloudAnchor.transform);
        instrumentOnWall.SetParent(cloudAnchor.transform);
#endif
        wall.transform.localPosition = new Vector3(0, 0, 0);
        wall.transform.localRotation = Quaternion.Euler(0, 0, 0);
        wallManager = wall.GetComponent<WallManager>();
        wallManager.wallSO = GameResources.Instance.wallSO_Showcase;
        GameResources.Instance.wallManager = wallManager;
        wallTransform = wall.transform;
    }

    public void InitializeObjects()
    {
        if (anchorType == AnchorType.IntrumentShowCase)
        {
            InitializeInstrumentOnWall();
        }
        else if (anchorType == AnchorType.MusicHistory)
        {
            InitializeMusicHistoryOnWall();
        }
    }

    private void InitializeInstrumentOnWall()
    {
        instrumentOnWall.localPosition = wallTransform.localPosition;
        instrumentOnWall.localRotation = wallTransform.localRotation;
        foreach (var item in instrumentPrefabList)
        {
            GameObject obj = Instantiate(item, instrumentOnWall);
            instrumentList.Add(obj);
            GetComponent<XRGrabInteractable>().enabled = false;
        }
        XRGrabInteractable xRGrabInteractable = wallManager.GetComponent<XRGrabInteractable>();
        xRGrabInteractable.enabled = false;
        StaticEventHandler.InvokeXRGrabInteractableSelected(null);
    }

    private void InitializeMusicHistoryOnWall()
    {
        popGenreOnWall.localPosition = wallTransform.localPosition;
        popGenreOnWall.localRotation = wallTransform.localRotation;
        foreach (var item in popPrefabList)
        {
            GameObject obj = Instantiate(item, popGenreOnWall);
            popList.Add(obj);
            GetComponent<XRGrabInteractable>().enabled = false;
        }
        rapGenreOnWall.localPosition = wallTransform.localPosition;
        rapGenreOnWall.localRotation = wallTransform.localRotation;
        foreach (var item in rapPrefabList)
        {
            GameObject obj = Instantiate(item, rapGenreOnWall);
            rapList.Add(obj);
            GetComponent<XRGrabInteractable>().enabled = false;
        }
        rockGenreOnWall.localPosition = wallTransform.localPosition;
        rockGenreOnWall.localRotation = wallTransform.localRotation;
        foreach (var item in rockPrefabList)
        {
            GameObject obj = Instantiate(item, rockGenreOnWall);
            rockList.Add(obj);
            GetComponent<XRGrabInteractable>().enabled = false;
        }
        XRGrabInteractable xRGrabInteractable = wallManager.GetComponent<XRGrabInteractable>();
        xRGrabInteractable.enabled = false;
        StaticEventHandler.InvokeXRGrabInteractableSelected(null);
    }

}
