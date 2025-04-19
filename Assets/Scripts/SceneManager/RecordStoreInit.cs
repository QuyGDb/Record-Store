using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RecordStoreInit : MonoBehaviour
{
    private Button[] buttons;

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();

    }
    private void Start()
    {
        buttons[0].onClick.AddListener(NavigateCreateStoreScene);
        buttons[1].onClick.AddListener(NavigateLoadStoreScene);
    }
    private void OnDestroy()
    {
        buttons[0].onClick.RemoveListener(NavigateCreateStoreScene);
        buttons[1].onClick.RemoveListener(NavigateLoadStoreScene);
    }
    void NavigateCreateStoreScene()
    {
        UnloadInitScene();
        GameManager.Instance.ChangeApplicationState(ApplicationState.CreateMapMode);
    }
    void NavigateLoadStoreScene()
    {
        UnloadInitScene();
        GameManager.Instance.ChangeApplicationState(ApplicationState.LoadingMapMode);
    }
    void UnloadInitScene()
    {
        SceneManager.UnloadSceneAsync("RecordStoreInit");
    }
}
