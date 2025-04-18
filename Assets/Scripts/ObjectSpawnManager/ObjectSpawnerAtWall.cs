using Google.XR.ARCoreExtensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ObjectSpawnerAtWall : MonoBehaviour
{

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
        XRGrabInteractable xRGrabInteractable = GameResources.Instance.currentwallManager.GetComponent<XRGrabInteractable>();

        if (state == ApplicationState.WallManager)
        {
            xRGrabInteractable.enabled = true;
            GetComponentInChildren<Collider>().enabled = true;
        }
        else
        {
            xRGrabInteractable.enabled = false;
            GetComponentInChildren<Collider>().enabled = false;
        }


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
    void CreateWall(ARCloudAnchor arRchor)
    {
        GameObject wall = Instantiate(GameResources.Instance.wallPrefab, transform);
        GameResources.Instance.currentwallManager = wall.GetComponent<WallManager>();
#if PLATFORM_ANDROID && !UNITY_EDITOR
        wall.transform.SetParent(arRchor.transform);
        wall.transform.localPosition = new Vector3(0, 0, 0);
#endif
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
