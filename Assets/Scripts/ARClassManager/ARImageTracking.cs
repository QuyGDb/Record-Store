using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARImageTracking : MonoBehaviour
{
    ARTrackedImageManager trackedImageManager;
    private Dictionary<string, GameObject> spawnerAlbums = new Dictionary<string, GameObject>();
    void OnEnable() => trackedImageManager.trackablesChanged.AddListener(OnChanged);

    void OnDisable() => trackedImageManager.trackablesChanged.RemoveListener(OnChanged);
    private void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }
    void OnChanged(ARTrackablesChangedEventArgs<ARTrackedImage> eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            AddPrefabForImage(newImage);
        }



        foreach (var removed in eventArgs.removed)
        {
            RemoveARAlbum(removed.Value);
        }

    }
    void AddPrefabForImage(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;
        GameObject prefabToSpawn = null;

        switch (imageName)
        {
            case "Gieo":
                prefabToSpawn = GameManager.Instance.albumSOs[0].albumPrefab;
                break;
            case "SDDBP":
                prefabToSpawn = GameManager.Instance.albumSOs[1].albumPrefab;
                break;

        }
        if (prefabToSpawn != null)
        {
            GameObject spawned = Instantiate(prefabToSpawn, trackedImage.transform.position, Quaternion.identity);
            spawnerAlbums.Add(trackedImage.referenceImage.name, spawned);
            spawned.transform.SetParent(trackedImage.transform);
        }
    }


    void RemoveARAlbum(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        if (spawnerAlbums.ContainsKey(imageName))
        {
            GameObject spawnedObject = spawnerAlbums[imageName];
            spawnedObject.gameObject.SetActive(false);
        }
    }
}

