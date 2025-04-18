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
    }
    private void OnApplicationStateChanged(ApplicationState state)
    {
        if (state == ApplicationState.ObjectParent)
        {
            ToggleInteractableObjectParent(true);
        }
        else
        {
            ToggleInteractableObjectParent(false);
        }
        if (state == ApplicationState.ObjectManager)
        {
            foreach (var item in instrumentList)
            {
                ToggleInteractableItem(item, true);
            }
        }
        else
        {
            foreach (var item in instrumentList)
            {
                ToggleInteractableItem(item, false);
            }
        }
    }
    void ToggleInteractableObjectParent(bool isEnable)
    {
        instrumentOnWall.GetComponent<XRGrabInteractable>().enabled = isEnable;
        instrumentOnWall.GetComponent<Collider>().enabled = isEnable;
    }
    void ToggleInteractableItem(GameObject item, bool isEnabled)
    {
        item.GetComponent<XRGrabInteractable>().enabled = isEnabled;
        item.GetComponentInChildren<Collider>().enabled = isEnabled;
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
            obj.gameObject.SetActive(false);
        }

    }
    public void ShowObjects()
    {
        GameManager.Instance.ChangeApplicationState(ApplicationState.ObjectParent);
        if (isCreated) return;
        foreach (var item in instrumentList)
        {
            item.gameObject.SetActive(true);
            item.GetComponent<XRGrabInteractable>().enabled = false;
            item.GetComponentInChildren<Collider>().enabled = false;
        }
        isCreated = true;
    }


}
