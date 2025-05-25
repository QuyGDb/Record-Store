using Google.XR.ARCoreExtensions;
using System;
using UnityEngine;

public class ObjectSpawnerAtAnchor : MonoBehaviour
{
    [SerializeField] Transform[] objectTranformList;
    private GameObject wall;
    GameObject vinlyShowse;
    GameObject portal;
    private void Start()
    {
        StaticEventHandler.OnInstantiateAtAnchor += OnInstantiateAtAnchor;
        wall = Instantiate(GameResources.Instance.wallPrefab, transform);
        wall.transform.localPosition = Settings.hidenPosition;
        vinlyShowse = Instantiate(GameResources.Instance.VinylShowCasePrefab, transform);
        vinlyShowse.transform.localPosition = Settings.hidenPosition;
        portal = Instantiate(GameResources.Instance.PortalPrefab, transform);
        portal.transform.localPosition = Settings.hidenPosition;
    }

    private void OnDestroy()
    {
        StaticEventHandler.OnInstantiateAtAnchor -= OnInstantiateAtAnchor;
    }

    private void OnInstantiateAtAnchor(ARCloudAnchor aRAnchor)
    {
        ActiveWall(aRAnchor);
        ActiveVinylShowCase(aRAnchor);
        ActivePortal(aRAnchor);
    }

    void ActiveWall(ARCloudAnchor arAnchor)
    {
        GameResources.Instance.currentwallManager = wall.GetComponent<WallManager>();
        SetTransformWall(wall, arAnchor);
        SetTransformWallAndObjectParent(arAnchor);
    }

    private async void SetTransformWallAndObjectParent(ARCloudAnchor arAnchor)
    {
        await Awaitable.NextFrameAsync();
        foreach (var item in objectTranformList)
        {
#if PLATFORM_ANDROID && !UNITY_EDITOR
            item.SetParent(arAnchor.transform);
            item.GetComponent<ObjectParent>().parentTransform = arAnchor.transform;
#endif
            item.GetComponent<ObjectSaver>().LoadTransform();

        }
    }

    private async void SetTransformWall(GameObject wall, ARCloudAnchor arAnchor)
    {
        await Awaitable.NextFrameAsync();
#if PLATFORM_ANDROID && !UNITY_EDITOR
        wall.transform.SetParent(arAnchor.transform);
#endif
        wall.GetComponent<ObjectSaver>().LoadTransform();
    }

    private void ActiveVinylShowCase(ARCloudAnchor aRCloudAnchor)
    {
#if PLATFORM_ANDROID && !UNITY_EDITOR
        vinlyShowse.transform.SetParent(aRCloudAnchor.transform);
#endif
        vinlyShowse.GetComponent<ObjectSaver>().LoadTransform();
    }

    private void ActivePortal(ARCloudAnchor aRCloudAnchor)
    {

#if PLATFORM_ANDROID && !UNITY_EDITOR
        portal.transform.SetParent(aRCloudAnchor.transform);
#endif
        portal.GetComponent<ObjectSaver>().LoadTransform();
    }
}
