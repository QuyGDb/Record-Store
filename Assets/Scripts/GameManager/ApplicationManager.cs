using System;
using Google.XR.ARCoreExtensions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ApplicationManager : SingletonMonobehaviour<ApplicationManager>
{
    public Action<ApplicationState> OnApplicationStateChanged;
    public ApplicationState currentAppState = ApplicationState.None;
    public ApplicationState previousApplicationState;
    public CloudAnchorsManager cloudAnchorsManager;

    protected override void Awake()
    {
        base.Awake();
    }


    public void ChangeApplicationState(ApplicationState newState)
    {
        previousApplicationState = currentAppState;
        currentAppState = newState;

        switch (currentAppState)
        {
            case ApplicationState.Admin:
                SceneManager.LoadScene("InstructionScene", LoadSceneMode.Additive);
                SceneManager.LoadScene("NavigationScene", LoadSceneMode.Additive);
                break;
            case ApplicationState.Anchor:
                break;
            case ApplicationState.CloudAnchor:
                break;
            case ApplicationState.WallManager:
                break;
            case ApplicationState.ObjectParent:
                break;
            case ApplicationState.ObjectManager:
                break;
            case ApplicationState.TestMap:
                break;
        }
        OnApplicationStateChanged?.Invoke(newState);
    }


}
