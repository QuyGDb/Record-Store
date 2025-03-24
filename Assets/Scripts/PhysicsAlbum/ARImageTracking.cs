using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

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

        //foreach (var updatedImage in eventArgs.updated)
        //{
        //    UpdatePrefabPosition(updatedImage);
        //}

        foreach (var removed in eventArgs.removed)
        {
            RemovePrefab(removed.Value);
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
            GameObject spawned = Instantiate(prefabToSpawn, trackedImage.transform.position, trackedImage.transform.rotation);
            //Debug.Log("Spawned prefab for image: " + imageName);
            //Debug.Log("Spawned prefab for image: " + spawned.name);
            //spawnerAlbums.Add(trackedImage.referenceImage.name, spawned);
            //spawned.transform.SetParent(trackedImage.transform);
        }
        Debug.Log("Added prefab for image: " + imageName);
    }

    //void UpdatePrefabPosition(ARTrackedImage trackedImage)
    //{
    //    string imageName = trackedImage.referenceImage.name;

    //    if (spawnerAlbums.ContainsKey(imageName))
    //    {
    //        GameObject spawnedObject = spawnerAlbums[imageName];
    //        spawnedObject.transform.SetParent(trackedImage.transform);
    //        spawnedObject.transform.localPosition = Vector3.zero;
    //        spawnedObject.transform.localRotation = Quaternion.identity;
    //    }
    //}


    void RemovePrefab(ARTrackedImage trackedImage)
    {
        string imageName = trackedImage.referenceImage.name;

        if (spawnerAlbums.ContainsKey(imageName))
        {
            GameObject spawnedObject = spawnerAlbums[imageName];
            // spawnedObject.gameObject.SetActive(false);
        }
    }
}

