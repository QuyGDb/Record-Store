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
    }
    private void Start()
    {
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
            instrumentOnWall.GetComponent<XRGrabInteractable>().enabled = false;
            popGenreOnWall.GetComponent<XRGrabInteractable>().enabled = false;
            rapGenreOnWall.GetComponent<XRGrabInteractable>().enabled = false;
            rockGenreOnWall.GetComponent<XRGrabInteractable>().enabled = false;
            instrumentOnWall.GetComponent<Collider>().enabled = false;
            popGenreOnWall.GetComponent<Collider>().enabled = false;
            rapGenreOnWall.GetComponent<Collider>().enabled = false;
            rockGenreOnWall.GetComponent<Collider>().enabled = false;

        }
    }

    private void OnInstantiateAtAnchor(ARCloudAnchor aRAnchor, AnchorType type)
    {
        instrumentPrefabList.Clear();
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
            case AnchorType.VinylShowCase:
                CreateVinylShowCase(aRAnchor);
                break;
            case AnchorType.Portal:
                CreatePortal(aRAnchor);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }

    }



    private void SetupHistoryMusicWall(ARCloudAnchor aRAnchor)
    {
        popGenreOnWall.gameObject.SetActive(true);
        rapGenreOnWall.gameObject.SetActive(true);
        rockGenreOnWall.gameObject.SetActive(true);
        GameObject wall = Instantiate(GameResources.Instance.wall_HistoryMusic.wallPrefab, transform);
        wallManager = wall.GetComponent<WallManager>();
#if PLATFORM_ANDROID && !UNITY_EDITOR
        wall.transform.SetParent(aRAnchor.transform);
        // tam thoi gan vao anchor, sau khi chinh xong wall, se gan vao world
        popGenreOnWall.SetParent(aRAnchor.transform);
        rapGenreOnWall.SetParent(aRAnchor.transform);
        rockGenreOnWall.SetParent(aRAnchor.transform);

        wall.transform.localPosition = new Vector3(0, 0, 0);
      
#endif
        popGenreOnWall.gameObject.SetActive(false);
        rapGenreOnWall.gameObject.SetActive(false);
        rockGenreOnWall.gameObject.SetActive(false);
        wallManager.wallSO = GameResources.Instance.wall_HistoryMusic;
        GameResources.Instance.wallManager = wallManager;
    }

    private void SetupInstrumentShowcaseWall(ARCloudAnchor cloudAnchor)
    {
        instrumentOnWall.gameObject.SetActive(true);
        GameObject wall = Instantiate(GameResources.Instance.wallSO_Showcase.wallPrefab, transform);
        wallManager = wall.GetComponent<WallManager>();

#if PLATFORM_ANDROID && !UNITY_EDITOR
        wall.transform.SetParent(cloudAnchor.transform);
        instrumentOnWall.SetParent(cloudAnchor.transform);
        wall.transform.localPosition = new Vector3(0, 0, 0);
#endif
        instrumentOnWall.gameObject.SetActive(false);
        wallManager.wallSO = GameResources.Instance.wallSO_Showcase;
        GameResources.Instance.wallManager = wallManager;
    }

    public void InitializeObjects()
    {
        if (anchorType == AnchorType.IntrumentShowCase)
        {
            InitializeInstrumentOnWall();
        }
        if (anchorType == AnchorType.MusicHistory)
        {
            InitializeMusicHistoryOnWall();
        }
    }

    private void InitializeInstrumentOnWall()
    {
        instrumentOnWall.transform.SetParent(wallManager.transform);
        instrumentOnWall.gameObject.SetActive(true);
        instrumentOnWall.transform.localPosition = new Vector3(0, 0, 0);
        foreach (var item in instrumentPrefabList)
        {
            GameObject obj = Instantiate(item, instrumentOnWall);
            instrumentList.Add(obj);
            obj.GetComponent<XRGrabInteractable>().enabled = false;
        }
        XRGrabInteractable xRGrabInteractable = wallManager.GetComponent<XRGrabInteractable>();
        xRGrabInteractable.enabled = false;
        StaticEventHandler.InvokeXRGrabInteractableSelected(null);
    }

    private void InitializeMusicHistoryOnWall()
    {
        popGenreOnWall.transform.SetParent(wallManager.transform);
        rapGenreOnWall.transform.SetParent(wallManager.transform);
        rockGenreOnWall.transform.SetParent(wallManager.transform);
        popGenreOnWall.gameObject.SetActive(true);
        rapGenreOnWall.gameObject.SetActive(true);
        rockGenreOnWall.gameObject.SetActive(true);
        popGenreOnWall.transform.localPosition = new Vector3(0, 0, 0);
        rapGenreOnWall.transform.localPosition = new Vector3(0, 0, 0);
        rockGenreOnWall.transform.localPosition = new Vector3(0, 0, 0);
        Instantiate(GameResources.Instance.pop.logo, popGenreOnWall);
        foreach (var item in popPrefabList)
        {
            GameObject obj = Instantiate(item, popGenreOnWall);
            popList.Add(obj);
            obj.GetComponent<XRGrabInteractable>().enabled = false;
        }
        //rap
        Instantiate(GameResources.Instance.rap.logo, rapGenreOnWall);
        foreach (var item in rapPrefabList)
        {
            GameObject obj = Instantiate(item, rapGenreOnWall);
            rapList.Add(obj);
            obj.GetComponent<XRGrabInteractable>().enabled = false;
        }
        //rock
        Instantiate(GameResources.Instance.rock.logo, rockGenreOnWall);
        foreach (var item in rockPrefabList)
        {
            GameObject obj = Instantiate(item, rockGenreOnWall);
            rockList.Add(obj);
            obj.GetComponent<XRGrabInteractable>().enabled = false;
        }
        XRGrabInteractable xRGrabInteractable = wallManager.GetComponent<XRGrabInteractable>();
        xRGrabInteractable.enabled = false;
        StaticEventHandler.InvokeXRGrabInteractableSelected(null);
    }
    private void CreateVinylShowCase(ARCloudAnchor aRCloudAnchor)
    {
        GameObject vinlyShowse = Instantiate(GameResources.Instance.VinylShowCasePrefab, transform);

#if PLATFORM_ANDROID && !UNITY_EDITOR
        vinlyShowse.transform.SetParent(aRCloudAnchor.transform);
        vinlyShowse.transform.localPosition = new Vector3(0, 0, 0);
#endif
    }
    private void CreatePortal(ARCloudAnchor aRCloudAnchor)
    {
        GameObject portal = Instantiate(GameResources.Instance.PortalPrefab, transform);
#if PLATFORM_ANDROID && !UNITY_EDITOR
        portal.transform.SetParent(aRCloudAnchor.transform);
        portal.transform.localPosition = new Vector3(0, 0, 0);
#endif
    }
}
