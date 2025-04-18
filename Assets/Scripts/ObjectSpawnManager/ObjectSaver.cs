using UnityEngine;

public class ObjectSaver : MonoBehaviour
{
    private ARTransform arTransform = new ARTransform();
    public void SaveTransform(string key)
    {
        if (gameObject == null) return;
        arTransform.position = transform.localPosition;
        arTransform.rotation = transform.localRotation;
        arTransform.scale = transform.localScale;
        ES3.Save(key, arTransform);
    }
    public async void LoadTransform(string key)
    {
        await Awaitable.NextFrameAsync();
        if (ES3.KeyExists(key) == false) return;
        ES3.LoadInto(key, arTransform);
        transform.localPosition = arTransform.position;
        transform.localRotation = arTransform.rotation;
        transform.localScale = arTransform.scale;

    }
}
