using System;
using UnityEngine;

public class ObjectSaver : MonoBehaviour
{
    private ARTransform arTransform = new ARTransform();
    ES3Settings settings;
    public void SaveTransform(string key)
    {
        if (gameObject == null) return;
        settings = new ES3Settings(Settings.es3Name);
        Debug.Log("Saving transform: " + Settings.es3Name);
        arTransform.position = transform.localPosition;
        arTransform.rotation = transform.localRotation;
        arTransform.scale = transform.localScale;
        ES3.Save(key, arTransform, settings);

    }
    public async void LoadTransform(string key)
    {
        await Awaitable.NextFrameAsync();
        settings = new ES3Settings(Settings.es3Name);
        if (ES3.KeyExists(key, settings))
        {
            Debug.Log("Loading transform: " + Settings.es3Name);
            ES3.LoadInto(key, arTransform, settings);
            transform.localPosition = arTransform.position;
            transform.localRotation = arTransform.rotation;
            transform.localScale = arTransform.scale;
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
    }
}
