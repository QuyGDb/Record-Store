using Google.XR.ARCoreExtensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ObjectSpawnerAtAnchors : MonoBehaviour
{
    private List<GameObject> instrumentShowcaseList = new List<GameObject>();
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
                InitializeObjectsAtAnchors(aRAnchor, instrumentShowcasePrefabList);
                break;
            case AnchorType.InstrumentShowCaseOversea:
                InstrumentShowcaseListSO instrumentShowcaseListSOOversea = GameResources.Instance.instrumentShowCaseOversea;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }

    void InitializeObjectsAtAnchors(ARCloudAnchor aRAnchor, List<GameObject> gameObject)
    {
        foreach (var item in gameObject)
        {
            GameObject obj = Instantiate(item, aRAnchor.transform);
            instrumentShowcaseList.Add(obj);
        }

    }
}
