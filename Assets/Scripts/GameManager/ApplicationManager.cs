using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ApplicationManager : SingletonMonobehaviour<ApplicationManager>
{
    public Action<ApplicationState> OnApplicationStateChanged;
    public ApplicationState applicationState = ApplicationState.Start;
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        ChangeApplicationState(ApplicationState.Start);
    }

    public void ChangeApplicationState(ApplicationState newState)
    {
        applicationState = newState;
        switch (applicationState)
        {
            case ApplicationState.Start:
                SceneManager.LoadScene("RecordStoreInit", LoadSceneMode.Additive);
                break;
            case ApplicationState.CreateMapMode:
                SceneManager.LoadScene("InstructionScene", LoadSceneMode.Additive);
                SceneManager.LoadScene("CreateMapNavigationScene", LoadSceneMode.Additive);
                break;
            case ApplicationState.LoadingMapMode:
                SceneManager.LoadScene("ListMapScene", LoadSceneMode.Additive);
                SceneManager.LoadScene("LoadMapNavigation", LoadSceneMode.Additive);
                break;
            case ApplicationState.Anchor:
                break;
            case ApplicationState.CloudAnchorInCreateMap:
                break;
            case ApplicationState.WallManager:
                break;
            case ApplicationState.ObjectParent:
                break;
            case ApplicationState.ObjectManager:
                break;
            case ApplicationState.TestMap:
                break;
            case ApplicationState.ListMap:
                break;
            case ApplicationState.Instruction:
                break;
            case ApplicationState.View:
                break;
        }
        OnApplicationStateChanged?.Invoke(newState);
    }


}
