using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ObjectParent : MonoBehaviour
{
    [SerializeField] private string objParentName;
    private ObjectSaver objectSaver;

    private void Awake()
    {
        objectSaver = GetComponent<ObjectSaver>();

    }
    private void Start()
    {
        GameManager.Instance.OnApplicationStateChanged += OnApplicationStateChanged;
        objectSaver.LoadTransform(objParentName);
    }
    private void OnDestroy()
    {
        GameManager.Instance.OnApplicationStateChanged -= OnApplicationStateChanged;
    }

    private void OnApplicationStateChanged(ApplicationState state)
    {
        if (state == ApplicationState.ObjectManager)
        {
            objectSaver.SaveTransform(objParentName);
        }
    }
}
