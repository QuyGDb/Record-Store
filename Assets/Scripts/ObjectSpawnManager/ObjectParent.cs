using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ObjectParent : MonoBehaviour
{
    [SerializeField] private string objParentName;
    private Transform saveTransform;


    private async void LoadTransform()
    {
        await Awaitable.NextFrameAsync();
        if (ES3.KeyExists(objParentName) == false)
            return;
        saveTransform = ES3.Load(objParentName, transform);
        if (saveTransform != null)
        {
            transform.localPosition = saveTransform.localPosition;
            transform.localRotation = saveTransform.localRotation;
            transform.localScale = saveTransform.localScale;
        }
    }
    private void Start()
    {
        GameManager.Instance.OnApplicationStateChanged += OnApplicationStateChanged;
        LoadTransform();
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnApplicationStateChanged -= OnApplicationStateChanged;
    }

    private void OnApplicationStateChanged(ApplicationState state)
    {
        if (state == ApplicationState.ObjectManager)
        {
            ES3.Save(objParentName, transform);
        }
    }
}
