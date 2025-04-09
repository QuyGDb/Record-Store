using Google.XR.ARCoreExtensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ObjectSpawnerAtAnchors : MonoBehaviour
{
    private List<GameObject> createdObjectsList = new List<GameObject>();
    private WallManager wallManager;
    private Transform wallTransform;
    private List<GameObject> objectsPrefabList = new List<GameObject>();

    private void Awake()
    {
        StaticEventHandler.OnInstantiateAtAnchor += OnInstantiateAtAnchor;
        GameResources.Instance.objectSpawnerAtAnchors = this;
    }
    private void OnDestroy()
    {
        StaticEventHandler.OnInstantiateAtAnchor -= OnInstantiateAtAnchor;
    }

    private void OnInstantiateAtAnchor(ARCloudAnchor aRAnchor, AnchorType type)
    {
        objectsPrefabList.Clear();
#if PLATFORM_ANDROID && !UNITY_EDITOR
        switch (type)
        {
            case AnchorType.IntrumentShowCaseVN:
                foreach (var instrumentShowcaseSO in GameResources.Instance.instrumentShowCaseVN.instrumentShowcaseList)
                {
                    objectsPrefabList.Add(instrumentShowcaseSO.instrumentPrefab);
                }
                InitializeWall(aRAnchor);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
#endif

#if UNITY_EDITOR
        List<InstrumentShowcaseSO> instrumentShowcaseSOs = GameResources.Instance.instrumentShowCaseVN.instrumentShowcaseList;
        foreach (var instrumentShowcaseSO in instrumentShowcaseSOs)
        {
            objectsPrefabList.Add(instrumentShowcaseSO.instrumentPrefab);
        }
        InitializeWall(aRAnchor);

#endif
    }

    private void InitializeWall(ARCloudAnchor cloudAnchor)
    {
        GameObject wall = Instantiate(GameResources.Instance.wallSO_ShowcaseVN.wallPrefab, transform);
#if PLATFORM_ANDROID && !UNITY_EDITOR
        wall.transform.SetParent(cloudAnchor.transform);
#endif
        wall.transform.localPosition = new Vector3(0, 0, 0);
        wall.transform.localRotation = Quaternion.Euler(0, 0, 0);
        wallManager = wall.GetComponent<WallManager>();
        wallManager.wallSO = GameResources.Instance.wallSO_ShowcaseVN;
        GameResources.Instance.wallManager = wallManager;
        wallTransform = wall.transform;
    }
    public void InitializeObjectsOnWall()
    {
        foreach (var item in objectsPrefabList)
        {
            GameObject obj = Instantiate(item, wallTransform);
            createdObjectsList.Add(obj);
        }
        XRGrabInteractable xRGrabInteractable = wallManager.GetComponent<XRGrabInteractable>();
        xRGrabInteractable.enabled = false;
        StaticEventHandler.InvokeXRGrabInteractableSelected(null);
    }
}
