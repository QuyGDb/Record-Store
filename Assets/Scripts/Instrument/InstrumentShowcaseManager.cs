using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class InstrumentShowcaseManager : MonoBehaviour, IObjectDisplayer
{
    private List<GameObject> instrumentList = new List<GameObject>();
    private List<GameObject> instrumentPrefabList = new List<GameObject>();
    [SerializeField] private Transform instrumentOnWall;
    bool isCreated = false;

    private void Start()
    {
        LoadPrefabListFromResource();
        LoadObjectFromPrefab();
        GameResources.Instance.instrumentShowcaseManager = this;
    }

    private void LoadPrefabListFromResource()
    {
        foreach (var instrumentShowcaseSO in GameResources.Instance.instrumentShowCase.instrumentShowcaseList)
        {
            instrumentPrefabList.Add(instrumentShowcaseSO.instrumentPrefab);
        }
    }
    private void LoadObjectFromPrefab()
    {
        foreach (var item in instrumentPrefabList)
        {
            GameObject obj = Instantiate(item, instrumentOnWall);
            instrumentList.Add(obj);
            obj.transform.localPosition = Settings.hidenPosition;
        }
    }
    public void ShowObjects()
    {
        if (GameResources.Instance.currentwallManager == null) return;
        ApplicationManager.Instance.ChangeApplicationState(ApplicationState.ObjectParent);
        if (isCreated) return;
        foreach (var item in instrumentList)
        {
            item.GetComponent<ObjectSaver>().LoadTransform();
            item.GetComponent<XRGrabInteractable>().enabled = false;
            item.GetComponentInChildren<Collider>().enabled = false;
        }
        isCreated = true;
    }


}
