using System;
using System.Xml;
using UnityEngine;

public class ObjectSaver : MonoBehaviour
{
    private ARTransform arTransform = new ARTransform();
    ES3Settings settings;
    INameable nameable;
    string key;
    bool isHasArTransform = false;
    public void SaveTransform(string key)
    {
        if (gameObject == null) return;
        arTransform.position = transform.localPosition;
        arTransform.rotation = transform.localRotation;
        arTransform.scale = transform.localScale;
        ES3.Save(key, arTransform, settings);
    }
    private void Awake()
    {
        settings = new ES3Settings(Settings.es3Name);
        nameable = GetComponent<INameable>();
        StaticEventHandler.OnNameMapText += StaticEventHandler_OnNameMapText;
    }

    private void OnDestroy()
    {
        StaticEventHandler.OnNameMapText -= StaticEventHandler_OnNameMapText;
    }

    private void StaticEventHandler_OnNameMapText(string obj)
    {
        key = nameable.GetKey();
        GetTransform();
    }

    public void LoadTransform()
    {
        if (isHasArTransform)
        {
            transform.localPosition = arTransform.position;
            transform.localRotation = arTransform.rotation;
            transform.localScale = arTransform.scale;
        }
        else
        {
            transform.position = Vector3.zero;
        }
    }

    public void GetTransform()
    {

        if (ES3.KeyExists(key, settings))
        {
            ES3.LoadInto(key, arTransform, settings);
            isHasArTransform = true;
            Debug.Log(arTransform.position + " " + arTransform.rotation + " " + arTransform.scale);
        }
    }
}
