using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PictureFrameManager : MonoBehaviour, IObjectDisplayer
{
    private List<GameObject> popList = new List<GameObject>();
    private List<GameObject> rapList = new List<GameObject>();
    private List<GameObject> rockList = new List<GameObject>();

    public Transform popGenreOnWall;
    public Transform rapGenreOnWall;
    public Transform rockGenreOnWall;
    private List<GameObject> popPrefabList = new List<GameObject>();
    private List<GameObject> rapPrefabList = new List<GameObject>();
    private List<GameObject> rockPrefabList = new List<GameObject>();
    bool isCreated = false;
    private void Start()
    {
        LoadPrefabListFromRessource();
        LoadObjectFromPrefab();
        GameResources.Instance.pictureFrameManager = this;
    }

    void ToggleInteractableItem(GameObject item, bool isEnabled)
    {
        item.GetComponent<XRGrabInteractable>().enabled = isEnabled;
        item.GetComponentInChildren<Collider>().enabled = isEnabled;
    }
    private void LoadPrefabListFromRessource()
    {
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
    }
    private void LoadObjectFromPrefab()
    {
        foreach (var item in popPrefabList)
        {
            GameObject obj = Instantiate(item, popGenreOnWall);
            popList.Add(obj);
            obj.gameObject.SetActive(false);
        }
        foreach (var item in rapPrefabList)
        {
            GameObject obj = Instantiate(item, rapGenreOnWall);
            rapList.Add(obj);
            obj.gameObject.SetActive(false);
        }
        foreach (var item in rockPrefabList)
        {
            GameObject obj = Instantiate(item, rockGenreOnWall);
            rockList.Add(obj);
            obj.gameObject.SetActive(false);
        }
    }
    public async void ShowObjects()
    {
        if (GameResources.Instance.currentwallManager == null) return;
        ApplicationManager.Instance.ChangeApplicationState(ApplicationState.ObjectParent);
        if (isCreated) return;
        foreach (var item in popList)
        {
            item.gameObject.SetActive(true);
            item.GetComponent<XRGrabInteractable>().enabled = false;
            item.GetComponentInChildren<Collider>().enabled = false;
            await Awaitable.WaitForSecondsAsync(0.2f);
        }
        foreach (var item in rapList)
        {
            item.gameObject.SetActive(true);
            item.GetComponent<XRGrabInteractable>().enabled = false;
            item.GetComponentInChildren<Collider>().enabled = false;
            await Awaitable.WaitForSecondsAsync(0.4f);
        }
        foreach (var item in rockList)
        {
            item.gameObject.SetActive(true);
            item.GetComponent<XRGrabInteractable>().enabled = false;
            item.GetComponentInChildren<Collider>().enabled = false;
            await Awaitable.WaitForSecondsAsync(0.3f);
        }
        isCreated = true;
    }
}
