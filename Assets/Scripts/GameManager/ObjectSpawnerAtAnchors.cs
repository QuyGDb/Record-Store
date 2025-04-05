using Google.XR.ARCoreExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObjectSpawnerAtAnchors : MonoBehaviour
{
    private List<GameObject> instrumentShowcaseList = new List<GameObject>();

    private TransformWallManager transformWallManager;
    private void Awake()
    {
        StaticEventHandler.OnInstantiateAtAnchor += OnInstantiateAtAnchor;

    }
    private void OnDestroy()
    {
        StaticEventHandler.OnInstantiateAtAnchor -= OnInstantiateAtAnchor;
    }

    private void OnInstantiateAtAnchor(ARCloudAnchor aRAnchor, AnchorType type)
    {
        switch (type)
        {
            case AnchorType.IntrumentShowCaseVN:
                List<InstrumentShowcaseSO> instrumentShowcaseSOs = GameResources.Instance.instrumentShowCaseVN.instrumentShowcaseList;
                List<GameObject> instrumentShowcasePrefabList = new List<GameObject>();
                foreach (var instrumentShowcaseSO in instrumentShowcaseSOs)
                {
                    instrumentShowcasePrefabList.Add(instrumentShowcaseSO.instrumentPrefab);
                }
                InitializeWall(aRAnchor, instrumentShowcasePrefabList);

                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    private void InitializeWall(ARCloudAnchor cloudAnchor, List<GameObject> instrumentShowcasePrefabList)
    {
        GameObject wall = Instantiate(GameResources.Instance.wallSO_ShowcaseVN.wallPrefab, transform);
        wall.transform.SetParent(cloudAnchor.transform);
        wall.transform.localPosition = new Vector3(0, 0, 0);
        wall.transform.localRotation = Quaternion.Euler(0, 0, 0);
        transformWallManager = wall.GetComponent<TransformWallManager>();
        transformWallManager.wallSO = GameResources.Instance.wallSO_ShowcaseVN;
        InitializeObjectsAtAnchors(wall.transform, instrumentShowcasePrefabList);

        StaticEventHandler.InvokeTransformWallManager(transformWallManager);
    }

    void InitializeObjectsAtAnchors(Transform transform, List<GameObject> gameObject)
    {
        foreach (var item in gameObject)
        {
            GameObject obj = Instantiate(item, transform);
            instrumentShowcaseList.Add(obj);
        }

    }
}
