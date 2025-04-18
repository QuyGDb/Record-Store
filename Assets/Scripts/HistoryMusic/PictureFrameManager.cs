using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PictureFrameManager : MonoBehaviour, IObjectDisplayer
{
    private List<GameObject> popList = new List<GameObject>();
    private List<GameObject> rapList = new List<GameObject>();
    private List<GameObject> rockList = new List<GameObject>();

    [SerializeField] private Transform popGenreOnWall;
    [SerializeField] private Transform rapGenreOnWall;
    [SerializeField] private Transform rockGenreOnWall;
    private List<GameObject> popPrefabList = new List<GameObject>();
    private List<GameObject> rapPrefabList = new List<GameObject>();
    private List<GameObject> rockPrefabList = new List<GameObject>();
    bool isCreated = false;

    private void Start()
    {
        LoadPrefabListFromRessource();
        LoadObjectFromPrefab();
        GameManager.Instance.OnApplicationStateChanged += OnApplicationStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnApplicationStateChanged -= OnApplicationStateChanged;
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
            foreach (var item in popList)
            {
                ToggleInteractableItem(item, true);
            }
            foreach (var item in rapList)
            {
                ToggleInteractableItem(item, true);
            }
            foreach (var item in rockList)
            {
                ToggleInteractableItem(item, true);
            }
        }
        else
        {
            foreach (var item in popList)
            {
                ToggleInteractableItem(item, false);
            }
            foreach (var item in rapList)
            {
                ToggleInteractableItem(item, false);
            }
            foreach (var item in rockList)
            {
                ToggleInteractableItem(item, false);
            }
        }
    }
    void ToggleInteractableObjectParent(bool isEnable)
    {
        popGenreOnWall.GetComponent<XRGrabInteractable>().enabled = isEnable;
        popGenreOnWall.GetComponent<Collider>().enabled = isEnable;
        rapGenreOnWall.GetComponent<XRGrabInteractable>().enabled = isEnable;
        rapGenreOnWall.GetComponent<Collider>().enabled = isEnable;
        rockGenreOnWall.GetComponent<XRGrabInteractable>().enabled = isEnable;
        rockGenreOnWall.GetComponent<Collider>().enabled = isEnable;
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
    public void ShowObjects()
    {
        GameManager.Instance.ChangeApplicationState(ApplicationState.ObjectParent);
        if (isCreated) return;
        foreach (var item in popList)
        {
            item.gameObject.SetActive(true);
            item.GetComponent<XRGrabInteractable>().enabled = false;
            item.GetComponentInChildren<Collider>().enabled = false;
        }
        foreach (var item in rapList)
        {
            item.gameObject.SetActive(true);
            item.GetComponent<XRGrabInteractable>().enabled = false;
            item.GetComponentInChildren<Collider>().enabled = false;
        }
        foreach (var item in rockList)
        {
            item.gameObject.SetActive(true);
            item.GetComponent<XRGrabInteractable>().enabled = false;
            item.GetComponentInChildren<Collider>().enabled = false;
        }
        isCreated = true;
    }
}
