using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine;

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

    private List<(List<GameObject> prefabList, List<GameObject> instanceList, Transform parent)> genreData = new List<(List<GameObject>, List<GameObject>, Transform)>();

    private bool isCreated = false;

    private void Start()
    {
        LoadPrefabListFromResources();
        LoadObjectFromPrefabs();
        GameResources.Instance.pictureFrameManager = this;
    }

    void ToggleInteractableItem(GameObject item, bool isEnabled)
    {
        item.GetComponent<XRGrabInteractable>().enabled = isEnabled;
        item.GetComponentInChildren<Collider>().enabled = isEnabled;
    }

    private void LoadPrefabListFromResources()
    {
        popPrefabList.AddRange(GameResources.Instance.pop.prefabs);
        rapPrefabList.AddRange(GameResources.Instance.rap.prefabs);
        rockPrefabList.AddRange(GameResources.Instance.rock.prefabs);

        genreData.Add((popPrefabList, popList, popGenreOnWall));
        genreData.Add((rapPrefabList, rapList, rapGenreOnWall));
        genreData.Add((rockPrefabList, rockList, rockGenreOnWall));
    }

    private void LoadObjectFromPrefabs()
    {
        foreach (var (prefabList, instanceList, parent) in genreData)
        {
            foreach (var prefab in prefabList)
            {
                GameObject obj = Instantiate(prefab, parent);
                instanceList.Add(obj);
                obj.transform.localPosition = Settings.hidenPosition;
            }
        }
    }

    public async void ShowObjects()
    {
        if (GameResources.Instance.currentwallManager == null) return;
        ApplicationManager.Instance.ChangeApplicationState(ApplicationState.ObjectParent);
        if (isCreated) return;

        foreach (var (_, instanceList, _) in genreData)
        {
            foreach (var item in instanceList)
            {
                item.GetComponent<PictureFrame>().LoadTransformPictureFrame();
                ToggleInteractableItem(item, false);
                await Awaitable.NextFrameAsync();
            }
        }
        isCreated = true;
    }
}
