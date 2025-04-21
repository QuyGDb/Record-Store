using Google.XR.ARCoreExtensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ObjectSpawnerAtAnchor : MonoBehaviour
{
    [SerializeField] Transform[] objectTranformList;

    private void Start()
    {
        StaticEventHandler.OnInstantiateAtWall += OnInstantiateAtAnchor;
    }

    private void OnDestroy()
    {
        StaticEventHandler.OnInstantiateAtWall -= OnInstantiateAtAnchor;
    }

    private void OnInstantiateAtAnchor(ARCloudAnchor aRAnchor, AnchorType type)
    {
        switch (type)
        {
            case AnchorType.Wall:
                CreateWall(aRAnchor);
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
    void CreateWall(ARCloudAnchor arAnchor)
    {
        GameObject wall = Instantiate(GameResources.Instance.wallPrefab, transform);
        GameResources.Instance.currentwallManager = wall.GetComponent<WallManager>();
#if PLATFORM_ANDROID
        if (arAnchor == null) return;
        wall.transform.SetParent(arAnchor.transform);
        wall.transform.localPosition = new Vector3(0, 0, 0);
        wall.transform.localRotation = Quaternion.Euler(0, 0, 0);
        foreach (var item in objectTranformList)
        {
            item.SetParent(arAnchor.transform);
            item.localPosition = new Vector3(0, 0, 0);
            item.localRotation = Quaternion.Euler(0, 0, 0);
            item.GetComponent<ObjectParent>().LoadTransformObjectParent();
        }
#endif
    }

    private void CreateVinylShowCase(ARCloudAnchor aRCloudAnchor)
    {
        GameObject vinlyShowse = Instantiate(GameResources.Instance.VinylShowCasePrefab, transform);

#if PLATFORM_ANDROID && !UNITY_EDITOR
        vinlyShowse.transform.SetParent(aRCloudAnchor.transform);
        vinlyShowse.transform.localPosition = new Vector3(0, 0, 0);
        vinlyShowse.transform.localRotation = Quaternion.Euler(0, 0, 0);
#endif
    }
    private void CreatePortal(ARCloudAnchor aRCloudAnchor)
    {
        GameObject portal = Instantiate(GameResources.Instance.PortalPrefab, transform);
#if PLATFORM_ANDROID && !UNITY_EDITOR
        portal.transform.SetParent(aRCloudAnchor.transform);
        portal.transform.localPosition = new Vector3(0, 0, 0);
        portal.transform.localRotation = Quaternion.Euler(0, 0, 0);
#endif
    }

}
