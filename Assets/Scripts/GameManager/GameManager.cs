using System;
using UnityEngine;
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
        ChangeApplicationState(ApplicationState.CreateMapMode);
    }
    public void ChangeApplicationState(ApplicationState newState)
    {
        applicationState = newState;
        switch (applicationState)
        {
            case ApplicationState.Start:
                break;
            case ApplicationState.CreateMapMode:
                SceneManager.LoadScene("NavigationScene", LoadSceneMode.Additive);
                SceneManager.LoadScene("InstructionScene", LoadSceneMode.Additive);
                break;
            case ApplicationState.LoadMapMode:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        OnApplicationStateChanged?.Invoke(newState);
    }
}
