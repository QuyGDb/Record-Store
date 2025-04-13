using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ObjectParent : MonoBehaviour
{
    [SerializeField] private string objParentName;
    private Transform saveTransform;

    private void Awake()
    {
        saveTransform = ES3.Load(objParentName, transform);
        LoadTransform();
    }
    private void LoadTransform()
    {
        if (saveTransform != null)
        {
            transform.position = saveTransform.localPosition;
            transform.rotation = saveTransform.localRotation;
            transform.localScale = saveTransform.localScale;
        }
    }
    private void Start()
    {
        GameManager.Instance.OnApplicationStateChanged += OnApplicationStateChanged;
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
            Debug.Log($"Saved {objParentName} transform");
        }
    }
}
