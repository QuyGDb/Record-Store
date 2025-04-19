using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonobehaviour<GameManager>
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
                SceneManager.LoadScene("LoadMapScene", LoadSceneMode.Additive);
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
